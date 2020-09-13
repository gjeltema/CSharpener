// --------------------------------------------------------------------
// CSharpSyntaxNodeDataComparers.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic.Sorting
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public abstract class SyntaxNodeSorter : IComparer<CSharpSyntaxNodeData>
    {
        protected SyntaxNodeSorter()
        {
        }

        public abstract int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData);

        /// <summary>
        /// If left is true and right is false, returns -1. If left is false and right is true, returns 1.  Else returns 0.
        /// </summary>
        protected int CompareBool(bool left, bool right)
        {
            if (left && !right)
                return -1;
            if (!left && right)
                return 1;
            return 0;
        }

        /// <summary>
        /// If left is true and right is false, returns 1. If left is false and right is true, returns -1.  Else returns 0.
        /// </summary>
        protected int CompareReverseBool(bool left, bool right)
        {
            if (left && !right)
                return 1;
            if (!left && right)
                return -1;
            return 0;
        }
    }

    public abstract class SyntaxNodeSorterWithSortConfig : SyntaxNodeSorter
    {
        protected SyntaxNodeSorterWithSortConfig(SortingConfiguration sortConfig)
        {
            SortConfig = sortConfig;
        }

        protected SortingConfiguration SortConfig { get; }
    }

    public sealed class KindSorter : SyntaxNodeSorterWithSortConfig
    {
        public KindSorter(SortingConfiguration sortConfig) : base(sortConfig)
        { }

        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData)
        {
            if (leftData.Kind == rightData.Kind)
                return 0;

            return SortConfig.KindSortOrder[leftData.Kind] < SortConfig.KindSortOrder[rightData.Kind] ? -1 : 1;
        }
    }

    public sealed class ExternSorter : SyntaxNodeSorter
    {
        public ExternSorter()
        { }

        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData)
            => CompareBool(leftData.IsExtern, rightData.IsExtern);
    }

    public sealed class ConstSorter : SyntaxNodeSorter
    {
        public ConstSorter()
        { }

        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData) 
            => CompareBool(leftData.IsConst, rightData.IsConst);
    }

    public sealed class ReadonlySorter : SyntaxNodeSorter
    {
        public ReadonlySorter()
        { }

        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData) 
            => CompareBool(leftData.IsReadonly, rightData.IsReadonly);
    }

    public sealed class StaticSorter : SyntaxNodeSorter
    {
        public StaticSorter()
        { }

        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData) 
            => CompareBool(leftData.IsStatic, rightData.IsStatic);
    }

    public sealed class AccessibilitySorter : SyntaxNodeSorterWithSortConfig
    {
        public AccessibilitySorter(SortingConfiguration sortConfig) : base(sortConfig)
        { }

        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData)
        {
            if (leftData.Access == rightData.Access)
                return 0;
            return SortConfig.AccessModifierSortOrder[leftData.Access] < SortConfig.AccessModifierSortOrder[rightData.Access] ? -1 : 1;
        }
    }

    public sealed class NameSorter : SyntaxNodeSorter
    {
        public NameSorter()
        { }

        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData)
        {
            int identifierComparison = string.Compare(leftData.Identifier, rightData.Identifier);
            return identifierComparison;
        }
    }

    public sealed class MethodArgumentsSorter : SyntaxNodeSorter
    {
        public MethodArgumentsSorter()
        { }

        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData)
        {
            if (leftData.NumberOfMethodArguments < rightData.NumberOfMethodArguments)
                return -1;
            if (leftData.NumberOfMethodArguments > rightData.NumberOfMethodArguments)
                return 1;

            for(int argumentIndex = 0; argumentIndex < leftData.NumberOfMethodArguments; argumentIndex++)
            {
                ParameterSyntax leftArg = leftData.MethodArguments[argumentIndex];
                ParameterSyntax rightArg = rightData.MethodArguments[argumentIndex];

                TypeSyntax leftType = leftArg.Type;
                TypeSyntax rightType = rightArg.Type;

                int predefinedCompareResult = ComparePredefinedArgument(leftType, rightType);
                if (predefinedCompareResult != 0)
                    return predefinedCompareResult;

                int attributeCompareResult = CompareAttributedArgument(leftArg, rightArg);
                if (attributeCompareResult != 0)
                    return attributeCompareResult;

                int modifierCompareResult = CompareModifierOnArgument(leftArg, rightArg);
                if (modifierCompareResult != 0)
                    return modifierCompareResult;

                int arrayCompareResult = CompareArrayArgument(leftType, rightType);
                if (arrayCompareResult != 0)
                    return arrayCompareResult;

                int genericCompareResult = CompareGenericArgument(leftType, rightType);
                if (genericCompareResult != 0)
                    return genericCompareResult;
            }

            int argumentTypeNameCompareResult = CompareArgumentTypeNames(leftData, rightData);
            return argumentTypeNameCompareResult;
        }

        private int CompareArgumentTypeNames(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData)
        {
            for (int argumentIndex = 0; argumentIndex < leftData.NumberOfMethodArguments; argumentIndex++)
            {
                ParameterSyntax leftArg = leftData.MethodArguments[argumentIndex];
                ParameterSyntax rightArg = rightData.MethodArguments[argumentIndex];

                string leftTypeName = leftArg.Type.ToString();
                string rightTypeName = rightArg.Type.ToString();
                int comparison = leftTypeName.CompareTo(rightTypeName);
                if (comparison != 0)
                    return comparison;
            }
            return 0;
        }

        /// <summary>
        /// Compares whether the arguments based on whether they are "predefined".  Pushes predefined args upward in sorting.
        /// Predefined meaning that it is a keyword of the C# language (<c>int</c>, <c>double</c>, <c>string</c>, etc.)
        /// Note that the "class" names for these types are not considered "predefined" by the compiler,
        /// so this will return -1 if the leftArg is an <c>int</c> and the rightArg is an <c>Int32</c>.
        /// </summary>
        private int ComparePredefinedArgument(TypeSyntax leftArg, TypeSyntax rightArg)
        {
            bool leftIsPredefined = leftArg is PredefinedTypeSyntax;
            bool rightIsPredefined = rightArg is PredefinedTypeSyntax;

            return CompareBool(leftIsPredefined, rightIsPredefined);
        }

        /// <summary>
        /// Pushes an attributed arg upward in sorting.
        /// </summary>
        private int CompareAttributedArgument(ParameterSyntax leftArg, ParameterSyntax rightArg)
        {
            bool leftHasAttribute = leftArg.AttributeLists.Count > 0;
            bool rightHasAttribute = rightArg.AttributeLists.Count > 0;

            return CompareBool(leftHasAttribute, rightHasAttribute);
        }

        /// <summary>
        /// Pushes an arg with a modifier downward in sorting.
        /// </summary>
        private int CompareModifierOnArgument(ParameterSyntax leftArg, ParameterSyntax rightArg)
        {
            bool leftHasModifier = leftArg.Modifiers.Count > 0;
            bool rightHasModifier = rightArg.Modifiers.Count > 0;

            return CompareReverseBool(leftHasModifier, rightHasModifier);
        }

        /// <summary>
        /// Pushes array args downward in sorting.
        /// </summary>
        private int CompareArrayArgument(TypeSyntax leftArg, TypeSyntax rightArg)
        {
            bool leftIsArray = leftArg is ArrayTypeSyntax;
            bool rightIsArray = rightArg is ArrayTypeSyntax;

            return CompareReverseBool(leftIsArray, rightIsArray);
        }

        /// <summary>
        /// Pushes non-generic args upward in sorting.  If both are generic, the one with less generic parameters
        /// is pushed upward in sorting.  If both have the same number of generic parameters, cycles through the
        /// generic parameters and pushes predefined parameters upward in sorting.
        /// If both generic args have the same number of parameters and all parameters for both are either predefined
        /// or not predefined, then returns 0;
        /// </summary>
        /// <param name="leftArg"></param>
        /// <param name="rightArg"></param>
        /// <returns></returns>
        private int CompareGenericArgument(TypeSyntax leftArg, TypeSyntax rightArg)
        {
            // Compare both not Generic, or Generic argument vs. non-Generic argument
            var leftAsGeneric = leftArg as GenericNameSyntax;
            var rightAsGeneric = rightArg as GenericNameSyntax;

            if (leftAsGeneric == null && rightAsGeneric == null)
                return 0;
            if (leftAsGeneric != null && rightAsGeneric == null)
                return 1;
            if (leftAsGeneric == null && rightAsGeneric != null)
                return -1;

            // Both are Generic. Compare number of Generic arguments
            SeparatedSyntaxList<TypeSyntax> leftGenericArgs = leftAsGeneric.TypeArgumentList.Arguments;
            SeparatedSyntaxList<TypeSyntax> rightGenericArgs = rightAsGeneric.TypeArgumentList.Arguments;

            if (leftGenericArgs.Count < rightGenericArgs.Count)
                return -1;
            if (leftGenericArgs.Count > rightGenericArgs.Count)
                return 1;

            // Same number of Generic arguments. Compare predefined vs. non-predefined Generic arguments
            for (int genericArgIndex = 0; genericArgIndex < leftGenericArgs.Count; genericArgIndex++)
            {
                TypeSyntax leftGenericArg = leftGenericArgs[genericArgIndex];
                TypeSyntax rightGenericArg = rightGenericArgs[genericArgIndex];

                int genericArgComparison = ComparePredefinedArgument(leftGenericArg, rightGenericArg);
                if (genericArgComparison != 0)
                    return genericArgComparison;
            }

            return 0;
        }
    }
}
