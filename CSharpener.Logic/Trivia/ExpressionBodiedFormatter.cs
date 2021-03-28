// -----------------------------------------------------------------------
// ExpressionBodiedFormatter.cs Copyright 2021 Craig Gjeltema
// -----------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic.Trivia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public sealed class ExpressionBodiedFormatter : CSharpSyntaxRewriter
    {
        private const string IndentSpacing = "    ";
        private static readonly SyntaxTrivia SingleNewline = SyntaxFactory.SyntaxTrivia(SyntaxKind.EndOfLineTrivia, Environment.NewLine);

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            ArrowExpressionClauseSyntax arrowExpressionClause = null;
            SyntaxNode nodeBeforeArrowExpression = null;
            foreach (SyntaxNode childNode in node.ChildNodes())
            {
                if (childNode.Kind() == SyntaxKind.ArrowExpressionClause)
                {
                    arrowExpressionClause = childNode as ArrowExpressionClauseSyntax;
                    break;
                }
                nodeBeforeArrowExpression = childNode;
            }

            if (arrowExpressionClause == null || nodeBeforeArrowExpression == null)
                return base.VisitMethodDeclaration(node);

            if (!ShouldFormat(arrowExpressionClause, nodeBeforeArrowExpression))
                return base.VisitMethodDeclaration(node);

            SyntaxNode newNode = FormatMethodNode(node, nodeBeforeArrowExpression, arrowExpressionClause);
            return base.VisitMethodDeclaration(newNode as MethodDeclarationSyntax);
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var arrowExpressionClause = node.ChildNodes().Single(x => x.Kind() == SyntaxKind.ArrowExpressionClause) as ArrowExpressionClauseSyntax;
            if (arrowExpressionClause == null)
                return base.VisitPropertyDeclaration(node);

            SyntaxToken identifierToken = node.ChildTokens().Single(x => x.Kind() == SyntaxKind.IdentifierToken);

            string arrowLeadingTrivia = arrowExpressionClause.GetLeadingTrivia().ToString();
            string identifierTrailingTrivia = identifierToken.TrailingTrivia.ToString();

            if (!ShouldFormat(arrowExpressionClause, identifierToken))
                return base.VisitPropertyDeclaration(node);

            SyntaxNode newNode = FormatPropertyNode(node, identifierToken, arrowExpressionClause);

            string newNodeString = newNode.ToString();

            return base.VisitPropertyDeclaration(newNode as PropertyDeclarationSyntax);
        }

        private static SyntaxNode FormatMethodNode(SyntaxNode node, SyntaxNode lastNodeBeforeArrow, ArrowExpressionClauseSyntax arrowExpressionClause)
        {
            SyntaxNode newParameterListNode = lastNodeBeforeArrow.WithTrailingTrivia(SingleNewline);
            SyntaxNode newArrowExpressionClause = GetNewArrowExpressionClause(node, arrowExpressionClause);

            IDictionary<SyntaxNode, SyntaxNode> newNodes = new Dictionary<SyntaxNode, SyntaxNode>
            {
                { lastNodeBeforeArrow, newParameterListNode },
                { arrowExpressionClause, newArrowExpressionClause }
            };

            SyntaxNode newMethodNode = node.ReplaceNodes(newNodes.Keys, (x, y) => newNodes[x]);
            return newMethodNode;
        }

        private static SyntaxNode FormatPropertyNode(PropertyDeclarationSyntax node, SyntaxToken identifierToken, ArrowExpressionClauseSyntax arrowExpressionClause)
        {
            SyntaxToken newIdentifierToken = identifierToken.WithTrailingTrivia(SingleNewline);
            ArrowExpressionClauseSyntax newArrowExpressionClause = GetNewArrowExpressionClause(node, arrowExpressionClause);

            SyntaxNode newPropertyNode = node.WithIdentifier(newIdentifierToken).WithExpressionBody(newArrowExpressionClause);
            return newPropertyNode;
        }

        private static ArrowExpressionClauseSyntax GetNewArrowExpressionClause(SyntaxNode node, ArrowExpressionClauseSyntax arrowExpressionClause)
        {
            SyntaxTrivia newArrowExpressionLeadingTrivia = GetNewArrowLeadingTrivia(node);

            SyntaxToken arrowToken = arrowExpressionClause.ArrowToken;
            if (arrowToken.TrailingTrivia.ToString().Contains(Environment.NewLine))
            {
                arrowToken = UpdateArrowTokenTrailingTrivia(arrowToken);
                string expressionLeadingTrivia = arrowExpressionClause.Expression.GetLeadingTrivia().ToString();
                ExpressionSyntax newExpression = arrowExpressionClause.Expression.WithLeadingTrivia(SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, expressionLeadingTrivia.TrimStart()));
                arrowExpressionClause = arrowExpressionClause.WithArrowToken(arrowToken).WithExpression(newExpression);
            }

            ArrowExpressionClauseSyntax newArrowExpressionClause = arrowExpressionClause.WithLeadingTrivia(newArrowExpressionLeadingTrivia);
            return newArrowExpressionClause;
        }

        private static SyntaxTrivia GetNewArrowLeadingTrivia(SyntaxNode node)
        {
            SyntaxTriviaList leadingTrivia = node.GetLeadingTrivia();
            string leadingTriviaString = leadingTrivia.ToString();
            int indexOfLastNewline = leadingTriviaString.LastIndexOf(Environment.NewLine);

            int lengthOfLastLineleadingTrivia = leadingTriviaString.Length - indexOfLastNewline - Environment.NewLine.Length;

            // If the method is not declared on the next line after the preceding statement (for some odd formatting reason), then just use the existing trivia.
            if (indexOfLastNewline == -1 || lengthOfLastLineleadingTrivia == 0)
                return SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, IndentSpacing);

            return SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, GetSpaceString(lengthOfLastLineleadingTrivia) + IndentSpacing);
        }

        /// <summary>
        /// Returns 0, 1 or 2.  2 signifies "more than 1" which is all that is needed for this.
        /// </summary>
        private static int GetNumberOfNewlines(string input)
        {
            int indexOfFirstNewline = input.IndexOf(Environment.NewLine);
            if (indexOfFirstNewline == -1)
                return 0;

            int indexOfSecondNewline = input.Substring(indexOfFirstNewline + 2).IndexOf(Environment.NewLine);
            return indexOfSecondNewline == -1 ? 1 : 2;
        }

        private static string GetSpaceString(int numberOfSpaces)
            => new(' ', numberOfSpaces);

        private static bool ShouldFormat(SyntaxNode arrowExpression, SyntaxNodeOrToken nodeBeforeArrow)
        {
            // Should format if: trimmed arrow leading trivia is empty, and trimmed parameter list trailing trivia 
            // is empty and it does not end in a newline or contains more than 1 newline.
            // Otherwise it's already formatted, or there is a comment in between the 
            // method declaration and the arrow.
            string arrowLeadingTrivia = arrowExpression.GetLeadingTrivia().ToString();
            if (arrowLeadingTrivia.Trim() != string.Empty)
                return false;

            string nodeBeforeArrowStringTrailingTrivia = nodeBeforeArrow.GetTrailingTrivia().ToString();
            if (nodeBeforeArrowStringTrailingTrivia.Trim() != string.Empty)
                return false;

            int numberOfArrowNewlines = GetNumberOfNewlines(arrowLeadingTrivia);
            if (numberOfArrowNewlines > 1)
                return true;

            int numberOfBeforeNodeNewlines = GetNumberOfNewlines(nodeBeforeArrowStringTrailingTrivia);
            if (numberOfBeforeNodeNewlines > 1)
                return true;

            // Only one of the trivias is allowed to have a single newline.  If neither have a newline, need to format. If both have a newline, need to format.
            return !((numberOfArrowNewlines == 1 && numberOfBeforeNodeNewlines == 0) || (numberOfArrowNewlines == 0 && numberOfBeforeNodeNewlines == 1));
        }

        private static SyntaxToken UpdateArrowTokenTrailingTrivia(SyntaxToken arrowToken)
        {
            string arrowTokenTrailingTrivia = arrowToken.TrailingTrivia.ToString();
            return arrowToken.WithTrailingTrivia(SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, " " + arrowTokenTrailingTrivia.TrimStart()));
        }
    }
}
