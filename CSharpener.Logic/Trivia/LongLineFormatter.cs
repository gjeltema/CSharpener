// --------------------------------------------------------------------
// LongLineFormatter.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

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
        private readonly string IndentSpacing = "    ";
        private readonly int MaxLengthOfLine;
        private readonly string[] NewlineSeparator = new[] { Environment.NewLine };

        public LongLineFormatter(int maxLengthOfLine)
        {
            MaxLengthOfLine = maxLengthOfLine;
        }

        public override SyntaxNode VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            AssignmentExpressionSyntax newNode = SplitLongLinesOnDotToken(node);
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
            LocalDeclarationStatementSyntax newNode = SplitLongLinesOnDotToken(node);
            return base.VisitLocalDeclarationStatement(newNode);
        }

        private SyntaxToken FormatDotTokenWhitespace(SyntaxToken token, SyntaxTrivia newLeadingTrivia)
        {
            return token.WithLeadingTrivia(SyntaxFactory.CarriageReturnLineFeed, newLeadingTrivia).WithTrailingTrivia();
        }

        private SyntaxToken FormatEqualsOrArrowTokenWhitespace(SyntaxToken token, SyntaxTrivia newLeadingTrivia)
        {
            return token.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed, newLeadingTrivia);
        }

        private string GetFirstNonEmptyString(string[] nodeLines)
        {
            return nodeLines.First(x => x.Trim().Length > 0);
        }

        private SyntaxTrivia GetNewTrivia(string[] nodeLines)
        {
            string firstNonEmptyString = GetFirstNonEmptyString(nodeLines);
            return GetNewTrivia(firstNonEmptyString);
        }

        private SyntaxTrivia GetNewTrivia(string nodeLine)
        {
            int leadingWhitespaceLength = IndexOfFirstNonWhitespace(nodeLine);
            string leadingSpaces = GetSpaceString(leadingWhitespaceLength);
            string leadingSpacesWithIndent = leadingSpaces + IndentSpacing;
            SyntaxTrivia newLeadingTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, leadingSpacesWithIndent);
            return newLeadingTrivia;
        }

        private string GetSpaceString(int numberOfSpaces)
        {
            return new string(' ', numberOfSpaces);
        }

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

        private TSyntaxNode SplitLongLinesOnDotToken<TSyntaxNode>(TSyntaxNode node) where TSyntaxNode : SyntaxNode
        {
            string nodeString = node.ToFullString();
            string[] nodeLines = nodeString.Split(NewlineSeparator, StringSplitOptions.RemoveEmptyEntries);
            bool anyLineTooLong = nodeLines.Any(x => x.Length > MaxLengthOfLine);
            if (!anyLineTooLong)
                return node;

            SyntaxTrivia newLeadingTrivia = GetNewTrivia(nodeLines);

            IEnumerable<SyntaxNodeOrToken> descendants = node.DescendantNodesAndTokensAndSelf(x => !(x is LambdaExpressionSyntax));
            IList<SyntaxToken> descendantDotTokens = descendants.Where(x => x.Kind() is SyntaxKind.DotToken).Select(x => x.AsToken()).ToList();
            IDictionary<SyntaxToken, SyntaxToken> newDotTokens = descendantDotTokens.ToDictionary(x => x, x => FormatDotTokenWhitespace(x, newLeadingTrivia));
            TSyntaxNode newNode = node.ReplaceTokens(descendantDotTokens, (x, y) => newDotTokens[x]);
            return newNode;
        }
    }
}
