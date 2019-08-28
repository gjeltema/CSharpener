// --------------------------------------------------------------------
// RegionRemover.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic.Trivia
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public sealed class RegionRemover : CSharpSyntaxRewriter
    {
        public RegionRemover() : base(true)
        { }

        public override SyntaxNode VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
            => SyntaxFactory.SkippedTokensTrivia();

        public override SyntaxNode VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
            => SyntaxFactory.SkippedTokensTrivia();
    }
}
