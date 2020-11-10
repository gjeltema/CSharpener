// -----------------------------------------------------------------------
// FileHeaderFormatter.cs Copyright 2020 Craig Gjeltema
// -----------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic.Trivia
{
    using System;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public sealed class FileHeaderFormatter
    {
        private const string FilenameFormat = "{filename}";
        private const string YearFormat = "{year}";

        public SyntaxNode AddHeader(SyntaxNode root, string fileName)
        {
            string headerTemplate = CSharpenerConfigSettings.GetHeaderTemplate();
            if (string.IsNullOrWhiteSpace(headerTemplate))
                return root;

            string formattedHeader = FormatHeader(headerTemplate, fileName);

            SyntaxNode firstNode = GetFirstNode(root);
            if (firstNode == null)
                return root;
            SyntaxTriviaList leadingTrivia = GetLeadingTrivia(firstNode);
            bool triviaContainsHeader = TriviaContainsHeader(leadingTrivia, formattedHeader);
            if (triviaContainsHeader)
                return root;

            SyntaxTrivia headerTrivia = SyntaxFactory.SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, formattedHeader);
            SyntaxNode newFirstNode = firstNode.WithLeadingTrivia(headerTrivia);
            SyntaxNode newRoot = root.ReplaceNode(firstNode, newFirstNode);
            return newRoot;
        }

        private string FormatHeader(string header, string fileName)
        {
            string cleanedHeader = header.Trim() + Environment.NewLine + Environment.NewLine;
            string headerWithYear = cleanedHeader.Replace(YearFormat, DateTime.Today.Year.ToString());
            string headerWithFilename = headerWithYear.Replace(FilenameFormat, fileName);
            return headerWithFilename;
        }

        private SyntaxNode GetFirstNode(SyntaxNode root)
            => root.ChildNodes().FirstOrDefault();

        private SyntaxTriviaList GetLeadingTrivia(SyntaxNode firstNode)
        {
            SyntaxTriviaList leadingTrivia = firstNode.GetLeadingTrivia();
            return leadingTrivia;
        }

        private bool TriviaContainsHeader(SyntaxTriviaList triviaList, string header)
        {
            string triviaString = triviaList.ToFullString();
            return triviaString.StartsWith(header);
        }
    }
}
