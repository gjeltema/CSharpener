// --------------------------------------------------------------------
// UsingsPlacer.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic.Usings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public sealed class UsingsPlacer
    {
        public SyntaxNode ProcessUsings(SyntaxNode root)
        {
            IList<UsingDirectiveSyntax> usingsThatNeedToBeMoved = GetUsingsInWrongPlace(root);
            if (usingsThatNeedToBeMoved.Count == 0)
                return root;
            SyntaxNode newRootNode = PutUsingsInRightPlace(root, usingsThatNeedToBeMoved);
            return newRootNode;
        }

        private ICollection<UsingDirectiveSyntax> FormatUsings(IList<UsingDirectiveSyntax> usingsThatNeedToBeMoved)
        {
            var newList = new List<UsingDirectiveSyntax>(usingsThatNeedToBeMoved.Count);
            for (int i = 0; i < usingsThatNeedToBeMoved.Count; i++)
            {
                UsingDirectiveSyntax usingNode = usingsThatNeedToBeMoved[i];
                if (i == 0)
                    usingNode = usingNode.WithoutLeadingTrivia();

                UsingDirectiveSyntax newNode = usingNode.NormalizeWhitespace().WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

                if (i == usingsThatNeedToBeMoved.Count - 1)
                    newNode = newNode.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed, SyntaxFactory.CarriageReturnLineFeed);

                newList.Add(newNode);
            }
            return newList;
        }

        private IList<UsingDirectiveSyntax> GetUsingsInWrongPlace(SyntaxNode rootNode)
        {
            bool namespaceNodeFound = false;
            IList<UsingDirectiveSyntax> usingNodes = new List<UsingDirectiveSyntax>();
            foreach (SyntaxNode node in rootNode.ChildNodes())
            {
                if (node is UsingDirectiveSyntax usingNode)
                    usingNodes.Add(usingNode);
                else if (node is NamespaceDeclarationSyntax)
                {
                    namespaceNodeFound = true;
                    break;
                }
            }

            if (!namespaceNodeFound)
                return new List<UsingDirectiveSyntax>();
            return usingNodes;
        }

        private SyntaxNode PutUsingsInRightPlace(SyntaxNode rootNode, IList<UsingDirectiveSyntax> usingsThatNeedToBeMoved)
        {
            try
            {
                ICollection<UsingDirectiveSyntax> clonedNodes = FormatUsings(usingsThatNeedToBeMoved);
                SyntaxNode intermediateRootNode = rootNode.RemoveNodes(usingsThatNeedToBeMoved, SyntaxRemoveOptions.KeepNoTrivia);
                var namespaceNode = intermediateRootNode.ChildNodes().First(x => x is NamespaceDeclarationSyntax) as NamespaceDeclarationSyntax;
                NamespaceDeclarationSyntax newNamespaceNode = namespaceNode.AddUsings(clonedNodes.ToArray());

                SyntaxTriviaList firstUsingLeadingTrivia = usingsThatNeedToBeMoved[0].GetLeadingTrivia();
                SyntaxTriviaList newNamespaceLeadingTrivia = firstUsingLeadingTrivia.AddRange(newNamespaceNode.GetLeadingTrivia());
                newNamespaceNode = newNamespaceNode.WithLeadingTrivia(newNamespaceLeadingTrivia);
                SyntaxNode newRootNode = intermediateRootNode.ReplaceNode(namespaceNode, newNamespaceNode);
                return newRootNode;
            }
            catch (Exception)
            {
                return rootNode;
            }
        }
    }
}
