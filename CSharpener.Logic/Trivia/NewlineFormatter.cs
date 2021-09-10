// -----------------------------------------------------------------------
// NewlineFormatter.cs Copyright 2021 Craig Gjeltema
// -----------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic.Trivia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public sealed class NewlineFormatter : CSharpSyntaxRewriter
    {
        private static readonly SyntaxTrivia doubleNewLine = SyntaxFactory.SyntaxTrivia(SyntaxKind.EndOfLineTrivia, Environment.NewLine + Environment.NewLine);
        private static readonly SyntaxTrivia emptyTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, "");
        private static readonly int newlineLength = Environment.NewLine.Length;

        public NewlineFormatter() : base(true)
        { }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var formattedNode = FormatNewlines(node) as ClassDeclarationSyntax;
            return base.VisitClassDeclaration(formattedNode);
        }

        public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            var formattedNode = FormatNewlines(node) as InterfaceDeclarationSyntax;
            return base.VisitInterfaceDeclaration(formattedNode);
        }

        public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            NamespaceDeclarationSyntax formattedNode = FormatLeadingWhitespace(node);
            formattedNode = FormatNewlines(formattedNode) as NamespaceDeclarationSyntax;
            return base.VisitNamespaceDeclaration(formattedNode);
        }

        public override SyntaxNode VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            var formattedNode = FormatNewlines(node) as RecordDeclarationSyntax;
            return base.VisitRecordDeclaration(formattedNode);
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            var formattedNode = FormatNewlines(node) as StructDeclarationSyntax;
            return base.VisitStructDeclaration(formattedNode);
        }

        private static SyntaxNode CleanLastTokenLeadingTrivia(SyntaxNode nodeToBeUpdated)
        {
            SyntaxToken lastToken = nodeToBeUpdated.GetLastToken();
            string currentLeadingTrivia = TrimWhitespaceToNewline(lastToken.LeadingTrivia.ToString());
            SyntaxTrivia cleanedLeadingTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, currentLeadingTrivia);
            return nodeToBeUpdated.ReplaceToken(lastToken, lastToken.WithLeadingTrivia(cleanedLeadingTrivia));
        }

        private static IDictionary<SyntaxNode, SyntaxNode> CreateFormattedNodes(ICollection<SyntaxNode> newRootNodeChildren)
        {
            IDictionary<SyntaxNode, SyntaxNode> updatedNodes = new Dictionary<SyntaxNode, SyntaxNode>();
            SyntaxNode currentNode = newRootNodeChildren.First();
            foreach (SyntaxNode nextNode in newRootNodeChildren.Skip(1))
            {
                SyntaxNode newCurrentNode = UpdateNodeNewlines(currentNode, nextNode);
                updatedNodes.Add(currentNode, newCurrentNode);
                currentNode = nextNode;
            }

            SyntaxNode newLastNode = UpdateNodeNewlines(currentNode, SyntaxFactory.CarriageReturnLineFeed);
            updatedNodes.Add(currentNode, newLastNode);
            return updatedNodes;
        }

        private static SyntaxKind DetermineTriviaKind(string trivia)
        {
            string trimmedTrivia = trivia.TrimStart();
            bool isDocumentationTrivia = trimmedTrivia.StartsWith(@"\\\");
            return isDocumentationTrivia ? SyntaxKind.SingleLineDocumentationCommentTrivia : SyntaxKind.SingleLineCommentTrivia;
        }

        private static NamespaceDeclarationSyntax FormatLeadingWhitespace(NamespaceDeclarationSyntax namespaceNode)
        {
            SyntaxTriviaList leadingTrivia = namespaceNode.GetLeadingTrivia();
            string trimmedLeadingTrivia = leadingTrivia.ToString().TrimEnd();
            SyntaxTrivia desiredNewlines = GetNewlinesForNamespaceLeadingTrivia(namespaceNode);
            SyntaxTrivia newLeadingTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, trimmedLeadingTrivia);
            SyntaxTriviaList updatedLeadingTrivia = SyntaxFactory.TriviaList(newLeadingTrivia, desiredNewlines);
            return namespaceNode.WithLeadingTrivia(updatedLeadingTrivia);
        }

        private static SyntaxNode FormatNewlines(SyntaxNode root)
        {
            SyntaxNode newRoot = root;
            IEnumerable<SyntaxNode> childNodes = newRoot.ChildNodes();
            ICollection<SyntaxNode> newRootNodeChildren = GetChildNodesToFormat(childNodes);
            if (newRootNodeChildren.Count == 0)
                return newRoot;

            IDictionary<SyntaxNode, SyntaxNode> updatedNodes = CreateFormattedNodes(newRootNodeChildren);

            newRoot = newRoot.ReplaceNodes(newRootNodeChildren, (n, _) => updatedNodes[n]);
            newRoot = CleanLastTokenLeadingTrivia(newRoot);
            return newRoot;
        }

        private static ICollection<SyntaxNode> GetChildNodesToFormat(IEnumerable<SyntaxNode> childNodes)
        {
            // Some namespace and class declaration nodes include parts of the declaration items in the ChildNodes list.
            // Skip over these as we do not want newlines before the opening bracket for example.
            ICollection<SyntaxNode> nodesToBeFormatted = childNodes.SkipWhile(IsNodePartOfEnclosingDeclaration).ToList();
            return nodesToBeFormatted;
        }

        private static SyntaxTrivia GetDesiredNewlines(SyntaxKind currentKind, SyntaxKind nextKind)
        {
            if (currentKind != nextKind)
                return doubleNewLine;

            switch (currentKind)
            {
                // Single newline after these types.  May want to add a configuration property for this.
                case SyntaxKind.FieldDeclaration:
                case SyntaxKind.EventFieldDeclaration:
                case SyntaxKind.DelegateDeclaration:
                case SyntaxKind.UsingDirective:
                    return SyntaxFactory.CarriageReturnLineFeed;
                default:
                    return doubleNewLine;
            }
        }

        private static SyntaxTrivia GetNewlinesForNamespaceLeadingTrivia(SyntaxNode namespaceNode)
        {
            // Presumably this is the Document root node
            SyntaxNode rootNode = namespaceNode.Parent;
            if (rootNode == null)
                return doubleNewLine;

            SyntaxNode previousNode = null;
            foreach (SyntaxNode node in rootNode.ChildNodes())
            {
                if (node is NamespaceDeclarationSyntax)
                {
                    if (previousNode is UsingDirectiveSyntax)
                        return SyntaxFactory.CarriageReturnLineFeed;
                    else if (previousNode is ClassDeclarationSyntax)
                        return emptyTrivia;
                    else
                        return doubleNewLine;
                }
                previousNode = node;
            }

            return doubleNewLine;
        }

        private static bool IsNodePartOfEnclosingDeclaration(SyntaxNode node)
        {
            SyntaxKind kind = node.Kind();
            return kind == SyntaxKind.QualifiedName || // Namespace name
                kind == SyntaxKind.IdentifierName || // Also namespace name
                kind == SyntaxKind.BaseList || // Class inheritance list
                kind == SyntaxKind.TypeParameterList || // Generic parameter list
                kind == SyntaxKind.TypeParameterConstraintClause || // Generic constraint 'where' clauses
                kind == SyntaxKind.AttributeList || // Attributes decorating a class
                kind == SyntaxKind.ParameterList; // record parameter list when using "default constructor" syntax
        }

        /// <summary>
        /// Trim all whitespace up through the last newline before non-whitespace characters start.
        /// </summary>
        private static string TrimWhitespaceToNewline(string toBeTrimmed)
        {
            // If no newlines, then there is nothing to be trimmed.
            int lastNewlineIndex = toBeTrimmed.LastIndexOf(Environment.NewLine);
            if (lastNewlineIndex == -1)
                return toBeTrimmed;

            // Find where the whitespace ends, up through at most the last newline in the string.
            int lastIndexToTrimTo = 0;
            for (int i = 0; i <= lastNewlineIndex + newlineLength - 1; i++)
            {
                if (!char.IsWhiteSpace(toBeTrimmed[i]))
                {
                    break;
                }
                lastIndexToTrimTo = i;
            }

            // If there are no newlines in the leading whitespace before non-whitespace characters start, then dont trim.
            int indexOfNewlineToTrimTo = toBeTrimmed.LastIndexOf(Environment.NewLine, lastIndexToTrimTo);
            if (indexOfNewlineToTrimTo == -1)
                return toBeTrimmed;

            // Need to add the newlineLength to ensure that the trimming starts after the Environment.Newline string.
            return toBeTrimmed.Substring(indexOfNewlineToTrimTo + newlineLength);
        }

        private static SyntaxNode UpdateNodeNewlines(SyntaxNode currentNode, SyntaxNode nextNode)
        {
            SyntaxTrivia newlines = GetDesiredNewlines(currentNode.Kind(), nextNode.Kind());
            return UpdateNodeNewlines(currentNode, newlines);
        }

        private static SyntaxNode UpdateNodeNewlines(SyntaxNode currentNode, SyntaxTrivia newlines)
        {
            SyntaxToken lastToken = currentNode.GetLastToken();
            string cleanedTrailingTrivia = lastToken.TrailingTrivia.ToString().TrimEnd();
            SyntaxKind trailingTriviaKind = DetermineTriviaKind(cleanedTrailingTrivia);
            SyntaxTrivia trailingTriviaWithoutNewlines = SyntaxFactory.SyntaxTrivia(trailingTriviaKind, cleanedTrailingTrivia);
            SyntaxTriviaList newTrailingTrivia = SyntaxFactory.TriviaList(trailingTriviaWithoutNewlines, newlines);

            string cleanedLeadingTrivia = TrimWhitespaceToNewline(currentNode.GetLeadingTrivia().ToString());
            SyntaxKind leadingTriviaKind = DetermineTriviaKind(cleanedLeadingTrivia);
            SyntaxTrivia newLeadingTrivia = SyntaxFactory.SyntaxTrivia(leadingTriviaKind, cleanedLeadingTrivia);

            return currentNode.ReplaceToken(lastToken, lastToken.WithTrailingTrivia(newTrailingTrivia)).WithLeadingTrivia(newLeadingTrivia);
        }
    }
}
