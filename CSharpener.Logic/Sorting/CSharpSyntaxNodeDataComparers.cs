// -----------------------------------------------------------------------
// CSharpSyntaxNodeDataComparers.cs Copyright 2020 Craig Gjeltema
// -----------------------------------------------------------------------

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

        protected int CompareBool(bool left, bool right)
        {
            if (left && !right)
                return -1;
            return !left & right ? 1 : 0;
        }

        protected int CompareReverseBool(bool left, bool right)
        {
            if (left && !right)
                return 1;
            return !left & right ? -1 : 0;
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

            if (!SortConfig.KindSortOrder.TryGetValue(leftData.Kind, out int leftSortOrder))
            {
                return 1;
            }

            if (!SortConfig.KindSortOrder.TryGetValue(rightData.Kind, out int rightSortOrder))
            {
                return -1;
            }

            return leftSortOrder < rightSortOrder ? -1 : 1;
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
        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData)
        {
            if (leftData.NumberOfMethodArguments < rightData.NumberOfMethodArguments)
                return -1;
            if (leftData.NumberOfMethodArguments > rightData.NumberOfMethodArguments)
                return 1;

            for (int index = 0; index < leftData.NumberOfMethodArguments; ++index)
            {
                ParameterSyntax leftMethodArgs = leftData.MethodArguments[index];
                ParameterSyntax rightMethodArgs = rightData.MethodArguments[index];
                TypeSyntax leftType = leftMethodArgs.Type;
                TypeSyntax rightType = rightMethodArgs.Type;

                int predefinedCompareResult = ComparePredefinedArgument(leftType, rightType);
                if (predefinedCompareResult != 0)
                    return predefinedCompareResult;

                int attributeCompareResult = CompareAttributedArgument(leftMethodArgs, rightMethodArgs);
                if (attributeCompareResult != 0)
                    return attributeCompareResult;

                int modifierCompareResult = CompareModifierOnArgument(leftMethodArgs, rightMethodArgs);
                if (modifierCompareResult != 0)
                    return modifierCompareResult;

                int arrayCompareResult = CompareArrayArgument(leftType, rightType);
                if (arrayCompareResult != 0)
                    return arrayCompareResult;

                int genericCompareResult = CompareGenericArgument(leftType, rightType);
                if (genericCompareResult != 0)
                    return genericCompareResult;
            }
            return CompareArgumentTypeNames(leftData, rightData);
        }

        private int CompareArgumentTypeNames(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData)
        {
            for (int index = 0; index < leftData.NumberOfMethodArguments; ++index)
            {
                ParameterSyntax leftArgs = leftData.MethodArguments[index];
                ParameterSyntax rightArgs = rightData.MethodArguments[index];
                int compareResult = leftArgs.Type.ToString().CompareTo(rightArgs.Type.ToString());
                if (compareResult != 0)
                    return compareResult;
            }
            return 0;
        }

        private int CompareArrayArgument(TypeSyntax leftArg, TypeSyntax rightArg)
            => CompareReverseBool(leftArg is ArrayTypeSyntax, rightArg is ArrayTypeSyntax);

        private int CompareAttributedArgument(ParameterSyntax leftArg, ParameterSyntax rightArg)
        {
            bool left = leftArg.AttributeLists.Count > 0;
            bool right = rightArg.AttributeLists.Count > 0;
            return CompareBool(left, right);
        }

        private int CompareGenericArgument(TypeSyntax leftArg, TypeSyntax rightArg)
        {
            GenericNameSyntax leftNameSyntax = leftArg as GenericNameSyntax;
            GenericNameSyntax rightNameSyntax = rightArg as GenericNameSyntax;
            if (leftNameSyntax == null && rightNameSyntax == null)
                return 0;
            if (leftNameSyntax != null && rightNameSyntax == null)
                return 1;
            if (leftNameSyntax == null && rightNameSyntax != null)
                return -1;

            SeparatedSyntaxList<TypeSyntax> leftArguments = leftNameSyntax.TypeArgumentList.Arguments;
            SeparatedSyntaxList<TypeSyntax> rightArguments = rightNameSyntax.TypeArgumentList.Arguments;
            if (leftArguments.Count < rightArguments.Count)
                return -1;
            if (leftArguments.Count > rightArguments.Count)
                return 1;

            for (int index = 0; index < leftArguments.Count; ++index)
            {
                int predefinedCompareResult = ComparePredefinedArgument(leftArguments[index], rightArguments[index]);
                if (predefinedCompareResult != 0)
                    return predefinedCompareResult;
            }
            return 0;
        }

        private int CompareModifierOnArgument(ParameterSyntax leftArg, ParameterSyntax rightArg)
        {
            bool left = leftArg.Modifiers.Count > 0;
            bool right = rightArg.Modifiers.Count > 0;
            return CompareReverseBool(left, right);
        }

        private int ComparePredefinedArgument(TypeSyntax leftArg, TypeSyntax rightArg)
            => CompareBool(leftArg is PredefinedTypeSyntax, rightArg is PredefinedTypeSyntax);
    }
}
