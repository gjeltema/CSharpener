// -----------------------------------------------------------------------
// AccessLevelModifierFormatter.cs Copyright 2021 Craig Gjeltema
// -----------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic.AccessLevel
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public sealed class AccessLevelModifierFormatter : CSharpSyntaxRewriter
    {
        private static readonly SyntaxTrivia SingleWhitespace = SyntaxFactory.Whitespace(" ");

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var updatedClassNode = AddAccessModifierIfNotPresent(node) as ClassDeclarationSyntax;
            return base.VisitClassDeclaration(updatedClassNode);
        }

        public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            var updatedConstructorNode = AddAccessModifierIfNotPresent(node) as ConstructorDeclarationSyntax;
            return base.VisitConstructorDeclaration(updatedConstructorNode);
        }

        public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            var updatedEnumNode = AddAccessModifierIfNotPresent(node) as EnumDeclarationSyntax;
            return base.VisitEnumDeclaration(updatedEnumNode);
        }

        public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            var updatedFieldNode = AddAccessModifierIfNotPresent(node) as FieldDeclarationSyntax;
            return base.VisitFieldDeclaration(updatedFieldNode);
        }

        public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            var updatedInterfaceNode = AddAccessModifierIfNotPresent(node) as InterfaceDeclarationSyntax;
            return base.VisitInterfaceDeclaration(updatedInterfaceNode);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node.ExplicitInterfaceSpecifier != null)
                return node;

            if (node.Parent is InterfaceDeclarationSyntax)
                return node;

            var updatedMethodNode = AddAccessModifierIfNotPresent(node) as MethodDeclarationSyntax;
            return base.VisitMethodDeclaration(updatedMethodNode);
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (node.ExplicitInterfaceSpecifier != null)
                return node;

            if (node.Parent is InterfaceDeclarationSyntax)
                return node;

            var updatedPropertyNode = AddAccessModifierIfNotPresent(node) as PropertyDeclarationSyntax;
            return base.VisitPropertyDeclaration(updatedPropertyNode);
        }

        public override SyntaxNode VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            var updatedRecordNode = AddAccessModifierIfNotPresent(node) as RecordDeclarationSyntax;
            return base.VisitRecordDeclaration(updatedRecordNode);
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            var updatedStructNode = AddAccessModifierIfNotPresent(node) as StructDeclarationSyntax;
            return base.VisitStructDeclaration(updatedStructNode);
        }

        private static SyntaxNode AddAccessModifierIfNotPresent(BaseTypeDeclarationSyntax node)
        {
            SyntaxTokenList modifiers = node.Modifiers;
            AccessModifier currentAccessibility = ModifiersHelper.GetAccessibility(modifiers);
            if (currentAccessibility != AccessModifier.NotExplicit)
                return node;

            AccessModifier defaultAccessibility = ModifiersHelper.GetDefaultAccessibility(node.Kind());
            SyntaxTriviaList leadingTrivia = node.GetLeadingTrivia();
            SyntaxTokenList newModifiers = FormatModifiers(modifiers, defaultAccessibility);
            return node.WithoutLeadingTrivia().WithModifiers(newModifiers).WithLeadingTrivia(leadingTrivia);
        }

        private static SyntaxNode AddAccessModifierIfNotPresent(MemberDeclarationSyntax node)
        {
            SyntaxTokenList modifiers = node.Modifiers;
            AccessModifier currentAccessibility = ModifiersHelper.GetAccessibility(modifiers);
            if (currentAccessibility != AccessModifier.NotExplicit)
                return node;

            AccessModifier defaultAccessibility = ModifiersHelper.GetDefaultAccessibility(node.Kind());
            SyntaxTriviaList leadingTrivia = node.GetLeadingTrivia();
            SyntaxTokenList newModifiers = FormatModifiers(modifiers, defaultAccessibility);
            return node.WithoutLeadingTrivia().WithModifiers(newModifiers).WithLeadingTrivia(leadingTrivia);
        }

        private static SyntaxTokenList FormatModifiers(SyntaxTokenList existingModifiers, AccessModifier access)
        {
            // Get First since all current default modifiers have only one modifier (public, internal or private)
            SyntaxToken newAccessibility = ModifiersHelper.GetModifiers(access).First();
            ICollection<SyntaxToken> newModifiers = new List<SyntaxToken>() { newAccessibility.WithTrailingTrivia(SingleWhitespace) };

            foreach (SyntaxToken token in existingModifiers)
            {
                SyntaxToken newToken = token.WithoutTrivia().WithTrailingTrivia(SingleWhitespace);
                newModifiers.Add(newToken);
            }

            return SyntaxFactory.TokenList(newModifiers);
        }
    }
}
