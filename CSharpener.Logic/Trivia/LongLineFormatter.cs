// -----------------------------------------------------------------------
// LongLineFormatter.cs Copyright 2020 Craig Gjeltema
// -----------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic.Trivia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public sealed class LongLineFormatter : CSharpSyntaxRewriter
    {
        private static readonly SyntaxTrivia EmptyTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, "");
        private readonly string IndentSpacing = "    ";
        private readonly int MaxLengthOfLine;
        private readonly string[] NewlineSeparator = new[] { Environment.NewLine };

        public LongLineFormatter(int maxLengthOfLine)
        {
            MaxLengthOfLine = maxLengthOfLine;
        }

        public override SyntaxNode VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            (bool anyLineTooLong, SyntaxTrivia newLeadingTrivia) = IsAnyLineTooLong(node);
            if (!anyLineTooLong)
                return base.VisitAssignmentExpression(node);

            IList<SyntaxToken> descendentTokens = GetDescendentTokens(node, SyntaxKind.DotToken);
            IList<SyntaxNode> argumentNodes = GetArgumentNodes(node);
            AssignmentExpressionSyntax newNode = descendentTokens.Count <= (argumentNodes).Count
                ? SplitLongLinesOnCommaToken(node, argumentNodes, newLeadingTrivia)
                : SplitLongLinesOnDotToken(node, descendentTokens, newLeadingTrivia);
            return base.VisitAssignmentExpression(newNode);
        }

        public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            string nodeString = node.ToFullString();
            string[] nodeLines = nodeString.Split(NewlineSeparator, StringSplitOptions.RemoveEmptyEntries);
            bool anyLineTooLong = nodeLines.Any(x => x.Length > MaxLengthOfLine);
            if (!anyLineTooLong)
                return base.VisitFieldDeclaration(node);

            SyntaxToken firstEqualsToken = node.DescendantTokens().FirstOrDefault(x => x.Kind() is SyntaxKind.EqualsToken);
            if (firstEqualsToken.IsKind(SyntaxKind.None))
                return base.VisitFieldDeclaration(node);

            SyntaxTrivia newLeadingTrivia = GetNewTrivia(nodeLines);
            SyntaxToken newEqualsToken = FormatEqualsOrArrowTokenWhitespace(firstEqualsToken, newLeadingTrivia);
            FieldDeclarationSyntax newNode = node.ReplaceToken(firstEqualsToken, newEqualsToken);
            return base.VisitFieldDeclaration(newNode);
        }

        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            (bool anyLineTooLong, SyntaxTrivia newLeadingTrivia) = IsAnyLineTooLong(node);
            if (!anyLineTooLong)
                return base.VisitLocalDeclarationStatement(node);
            IList<SyntaxToken> descendentTokens = GetDescendentTokens(node, SyntaxKind.DotToken);
            IList<SyntaxNode> argumentNodes = GetArgumentNodes(node);
            LocalDeclarationStatementSyntax newNode =
                descendentTokens.Count <= argumentNodes.Count
                ? SplitLongLinesOnCommaToken(node, argumentNodes, newLeadingTrivia)
                : SplitLongLinesOnDotToken(node, descendentTokens, newLeadingTrivia);
            return base.VisitLocalDeclarationStatement(newNode);
        }

        private SyntaxNode FormatArgumentWhitespace(SyntaxNode node, SyntaxTrivia newLeadingTrivia)
            => node.WithLeadingTrivia(SyntaxFactory.CarriageReturnLineFeed, newLeadingTrivia).WithTrailingTrivia();

        private SyntaxToken FormatEqualsOrArrowTokenWhitespace(SyntaxToken token, SyntaxTrivia newLeadingTrivia)
            => token.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed, newLeadingTrivia);

        private SyntaxToken FormatTokenWhitespace(SyntaxToken token, SyntaxTrivia newLeadingTrivia)
            => token.WithLeadingTrivia(SyntaxFactory.CarriageReturnLineFeed, newLeadingTrivia).WithTrailingTrivia();

        private IList<SyntaxNode> GetArgumentNodes(SyntaxNode node)
            => node.DescendantNodes(x => !(x is LambdaExpressionSyntax)).Where(x => x.Kind() == SyntaxKind.Argument).ToList();

        private IList<SyntaxToken> GetDescendentTokens(SyntaxNode node, SyntaxKind tokenKind)
            => node.DescendantNodesAndTokensAndSelf(x => !(x is LambdaExpressionSyntax)).Where(x => x.Kind() == tokenKind).Select(x => x.AsToken()).ToList();

        private string GetFirstNonEmptyString(string[] nodeLines)
            => nodeLines.First(x => x.Trim().Length > 0);

        private SyntaxTrivia GetNewTrivia(string nodeLine)
        {
            int leadingWhitespaceLength = IndexOfFirstNonWhitespace(nodeLine);
            string leadingSpaces = GetSpaceString(leadingWhitespaceLength);
            string leadingSpacesWithIndent = leadingSpaces + IndentSpacing;
            SyntaxTrivia newLeadingTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, leadingSpacesWithIndent);
            return newLeadingTrivia;
        }

        private SyntaxTrivia GetNewTrivia(string[] nodeLines)
        {
            string firstNonEmptyString = GetFirstNonEmptyString(nodeLines);
            return GetNewTrivia(firstNonEmptyString);
        }

        private string GetSpaceString(int numberOfSpaces) => new string(' ', numberOfSpaces);

        private int IndexOfFirstNonWhitespace(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];
                if (!char.IsWhiteSpace(currentChar))
                    return i;
            }
            return -1;
        }

        private (bool anyLineTooLong, SyntaxTrivia newLeadingTrivia) IsAnyLineTooLong(SyntaxNode node)
        {
            string[] nodeLines = node.ToFullString().Split(NewlineSeparator, StringSplitOptions.RemoveEmptyEntries);
            return nodeLines.Any(x => x.Length > MaxLengthOfLine) ? (true, GetNewTrivia(nodeLines)) : (false, EmptyTrivia);
        }

        private TSyntaxNode SplitLongLinesOnCommaToken<TSyntaxNode>(TSyntaxNode node, IList<SyntaxNode> argumentNodes, SyntaxTrivia newLeadingTrivia) where TSyntaxNode : SyntaxNode
        {
            IDictionary<SyntaxNode, SyntaxNode> newCommaNodes = argumentNodes.ToDictionary(x => x, x => FormatArgumentWhitespace(x, newLeadingTrivia));
            return SyntaxNodeExtensions.ReplaceNodes(node, argumentNodes, (x, y) => newCommaNodes[x]);
        }

        private TSyntaxNode SplitLongLinesOnDotToken<TSyntaxNode>(TSyntaxNode node, IList<SyntaxToken> dotTokens, SyntaxTrivia newLeadingTrivia) where TSyntaxNode : SyntaxNode
        {
            IDictionary<SyntaxToken, SyntaxToken> newTokens = dotTokens.ToDictionary(x => x, x => FormatTokenWhitespace(x, newLeadingTrivia));
            return SyntaxNodeExtensions.ReplaceTokens(node, dotTokens, (x, y) => newTokens[x]);
        }
    }
}
