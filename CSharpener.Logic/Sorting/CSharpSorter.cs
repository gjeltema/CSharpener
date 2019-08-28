// --------------------------------------------------------------------
// CSharpSorter.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic.Sorting
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public enum Sorter
    {
        Kind,
        Extern,
        Const,
        Readonly,
        Static,
        Accessibility,
        Name,
        NumberOfMethodArguments
    }

    public sealed class CSharpSorter : CSharpSyntaxRewriter
    {
        private readonly SortingConfiguration sortingConfiguration = new SortingConfiguration();

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            ClassDeclarationSyntax sortedNode = SortClassNode(node);
            return base.VisitClassDeclaration(sortedNode);
        }

        private ClassDeclarationSyntax SortClassNode(ClassDeclarationSyntax classDeclaration)
        {
            var membersOfClass = classDeclaration.Members.ToList();
            var nodeData = membersOfClass.ToDictionary(x => x, x => new CSharpSyntaxNodeData(x));

            IComparer<MemberDeclarationSyntax> comparer = new NodeSorter(nodeData, sortingConfiguration);
            membersOfClass.Sort(comparer);

            ClassDeclarationSyntax classDeclarationNodeWithoutNodes = classDeclaration.RemoveNodes(membersOfClass, SyntaxRemoveOptions.KeepNoTrivia);
            ClassDeclarationSyntax finalClassDeclarationNode = classDeclarationNodeWithoutNodes.WithMembers(new SyntaxList<MemberDeclarationSyntax>(membersOfClass));
            return finalClassDeclarationNode;
        }

        private class NodeSorter : IComparer<MemberDeclarationSyntax>
        {
            private delegate SyntaxNodeSorter SorterCreator(SortingConfiguration sortConfig);

            private static readonly IDictionary<Sorter, SorterCreator> sorterInitializer = new Dictionary<Sorter, SorterCreator>
            {
                { Sorter.Kind, x => new KindSorter(x) },
                { Sorter.Extern, x => new ExternSorter() },
                { Sorter.Const, x => new ConstSorter() },
                { Sorter.Readonly, x => new ReadonlySorter() },
                { Sorter.Static, x => new StaticSorter() },
                { Sorter.Accessibility, x => new AccessibilitySorter(x) },
                { Sorter.Name, x => new NameSorter() },
                { Sorter.NumberOfMethodArguments, x => new NumberOfMethodArgumentsSorter() }
            };
            private readonly IDictionary<MemberDeclarationSyntax, CSharpSyntaxNodeData> nodeData;
            private readonly IList<SyntaxNodeSorter> sorters = new List<SyntaxNodeSorter>();
            private readonly SortingConfiguration sortingConfiguration;

            internal NodeSorter(IDictionary<MemberDeclarationSyntax, CSharpSyntaxNodeData> nodeData, SortingConfiguration sortingConfiguration)
            {
                this.nodeData = nodeData;
                this.sortingConfiguration = sortingConfiguration;
                InitializeSortOrder();
            }

            public int Compare(MemberDeclarationSyntax left, MemberDeclarationSyntax right)
            {
                if (ReferenceEquals(left, right))
                    return 0;
                if (left == null)
                    return 1;
                if (right == null)
                    return -1;

                CSharpSyntaxNodeData leftData = nodeData[left];
                CSharpSyntaxNodeData rightData = nodeData[right];

                for (int i = 0; i < sorters.Count; i++)
                {
                    int comparisonResult = sorters[i].Compare(leftData, rightData);
                    if (comparisonResult != 0)
                        return comparisonResult;
                }

                return 0;
            }

            private void InitializeSortOrder()
            {
                for (int i = 0; i < sortingConfiguration.ModifiersSortOrder.Count; i++)
                {
                    Sorter configuredSorter = sortingConfiguration.ModifiersSortOrder[i];
                    SyntaxNodeSorter newSorter = sorterInitializer[configuredSorter](sortingConfiguration);
                    sorters.Add(newSorter);
                }
            }
        }
    }
}
