// --------------------------------------------------------------------
// CSharpSyntaxNodeDataComparers.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic.Sorting
{
    using System.Collections.Generic;

    public abstract class SyntaxNodeSorter : IComparer<CSharpSyntaxNodeData>
    {
        protected SyntaxNodeSorter()
        {
        }

        public abstract int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData);
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
        {
            if (leftData.IsExtern && !rightData.IsExtern)
                return 1;
            else if (!leftData.IsExtern && rightData.IsExtern)
                return -1;
            return 0;
        }
    }

    public sealed class ConstSorter : SyntaxNodeSorter
    {
        public ConstSorter()
        { }

        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData)
        {
            if (leftData.IsConst && !rightData.IsConst)
                return -1;
            else if (!leftData.IsConst && rightData.IsConst)
                return 1;
            return 0;
        }
    }

    public sealed class ReadonlySorter : SyntaxNodeSorter
    {
        public ReadonlySorter()
        { }

        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData)
        {
            if (leftData.IsReadonly && !rightData.IsReadonly)
                return -1;
            else if (!leftData.IsReadonly && rightData.IsReadonly)
                return 1;
            return 0;
        }
    }

    public sealed class StaticSorter : SyntaxNodeSorter
    {
        public StaticSorter()
        { }

        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData)
        {
            if (leftData.IsStatic && !rightData.IsStatic)
                return -1;
            else if (!leftData.IsStatic && rightData.IsStatic)
                return 1;
            return 0;
        }
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

    public sealed class NumberOfMethodArgumentsSorter : SyntaxNodeSorter
    {
        public NumberOfMethodArgumentsSorter()
        { }

        public override int Compare(CSharpSyntaxNodeData leftData, CSharpSyntaxNodeData rightData)
        {
            if (leftData.NumberOfMethodArguments == rightData.NumberOfMethodArguments)
                return 0;
            return leftData.NumberOfMethodArguments < rightData.NumberOfMethodArguments ? -1 : 1;
        }
    }
}
