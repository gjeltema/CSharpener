using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Gjeltema.CSharpener.Logic.Trivia
{
    public sealed class LongLineFormatter : CSharpSyntaxRewriter
    {
        private int testVal;

        public int test => 5;

        public int test2() => testVal = 5;

        // Test these out next time.  Might be a better solution than Constructor/MethodDeclarationSyntax.
        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            return base.VisitLocalDeclarationStatement(node);
        }

        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            return base.VisitInvocationExpression(node);
        }



        public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            int lengthOfLine = CSharpenerConfigSettings.
                LengthOfLineToBreakOn;

            BreakLongLines(node);
            return base.VisitConstructorDeclaration(node);
        }

        //public override SyntaxNode VisitBlock(BlockSyntax node)
        //{
        //    //int lengthOfLine = CSharpenerConfigSettings.LengthOfLineToBreakOn;
        //    //string startingNodeText = node.ToFullString();
        //    return base.VisitBlock(node);
        //}

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            BreakLongLines(node);
            return base.VisitMethodDeclaration(node);
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            return base.VisitClassDeclaration(node);
        }

        private SyntaxNode BreakLongLines(BaseMethodDeclarationSyntax rootNode)
        {
            string startingNodeText = rootNode.ToFullString();

            BlockSyntax body = rootNode.Body;

            var childNodes = body.ChildNodes().ToList();

            return rootNode;

            //BaseMethodDeclarationSyntax finalNode = rootNode.WithBody(body);
            //return finalNode;
        }
    }
}
