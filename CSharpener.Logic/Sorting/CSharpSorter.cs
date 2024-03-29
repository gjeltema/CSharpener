﻿// -----------------------------------------------------------------------
// CSharpSorter.cs Copyright 2021 Craig Gjeltema
// -----------------------------------------------------------------------

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
        MethodArguments
    }

    public sealed class CSharpSorter : CSharpSyntaxRewriter
    {
        private readonly SortingConfiguration sortingConfiguration = new SortingConfiguration();

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            ClassDeclarationSyntax sortedNode = SortClassNode(node);
            return base.VisitClassDeclaration(sortedNode);
        }

        public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            InterfaceDeclarationSyntax sortedInterfaceNode = SortInterfaceNode(node);
            return base.VisitInterfaceDeclaration(sortedInterfaceNode);
        }

        public override SyntaxNode VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            RecordDeclarationSyntax sortedRecordNode = SortRecordNode(node);
            return base.VisitRecordDeclaration(sortedRecordNode);
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            StructDeclarationSyntax sortedStructNode = SortStructNode(node);
            return base.VisitStructDeclaration(sortedStructNode);
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

        private InterfaceDeclarationSyntax SortInterfaceNode(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            var membersOfInterface = interfaceDeclaration.Members.ToList();
            var nodeData = membersOfInterface.ToDictionary(x => x, x => new CSharpSyntaxNodeData(x));

            IComparer<MemberDeclarationSyntax> comparer = new NodeSorter(nodeData, sortingConfiguration);
            membersOfInterface.Sort(comparer);

            InterfaceDeclarationSyntax interfaceDeclarationNodeWithoutNodes = interfaceDeclaration.RemoveNodes(membersOfInterface, SyntaxRemoveOptions.KeepNoTrivia);
            InterfaceDeclarationSyntax finalInterfaceDeclarationNode = interfaceDeclarationNodeWithoutNodes.WithMembers(new SyntaxList<MemberDeclarationSyntax>(membersOfInterface));
            return finalInterfaceDeclarationNode;
        }

        private RecordDeclarationSyntax SortRecordNode(RecordDeclarationSyntax recordDeclaration)
        {
            var membersOfRecord = recordDeclaration.Members.ToList();
            var nodeData = membersOfRecord.ToDictionary(x => x, x => new CSharpSyntaxNodeData(x));

            IComparer<MemberDeclarationSyntax> comparer = new NodeSorter(nodeData, sortingConfiguration);
            membersOfRecord.Sort(comparer);

            RecordDeclarationSyntax recordDeclarationNodeWithoutNodes = recordDeclaration.RemoveNodes(membersOfRecord, SyntaxRemoveOptions.KeepNoTrivia);
            RecordDeclarationSyntax finalRecordDeclarationNode = recordDeclarationNodeWithoutNodes.WithMembers(new SyntaxList<MemberDeclarationSyntax>(membersOfRecord));
            return finalRecordDeclarationNode;
        }

        private StructDeclarationSyntax SortStructNode(StructDeclarationSyntax structDeclaration)
        {
            var membersOfStruct = structDeclaration.Members.ToList();
            var nodeData = membersOfStruct.ToDictionary(x => x, x => new CSharpSyntaxNodeData(x));

            IComparer<MemberDeclarationSyntax> comparer = new NodeSorter(nodeData, sortingConfiguration);
            membersOfStruct.Sort(comparer);

            StructDeclarationSyntax structDeclarationNodeWithoutNodes = structDeclaration.RemoveNodes(membersOfStruct, SyntaxRemoveOptions.KeepNoTrivia);
            StructDeclarationSyntax finalStructDeclarationNode = structDeclarationNodeWithoutNodes.WithMembers(new SyntaxList<MemberDeclarationSyntax>(membersOfStruct));
            return finalStructDeclarationNode;
        }

        private sealed class NodeSorter : IComparer<MemberDeclarationSyntax>
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
                { Sorter.MethodArguments, x => new MethodArgumentsSorter() }
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
