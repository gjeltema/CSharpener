// -----------------------------------------------------------------------
// SyntaxNodeData.cs Copyright 2020 Craig Gjeltema
// -----------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic.Sorting
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public sealed class CSharpSyntaxNodeData
    {
        public CSharpSyntaxNodeData(CSharpSyntaxNode node)
        {
            Kind = node.Kind();
            SyntaxTokenList modifiers = GetSyntaxPropertiesForNode(node);
            Modifiers = ModifiersHelper.GetNonAccessibilityModifiers(modifiers);
            Access = ModifiersHelper.GetAccessibility(Kind, modifiers);
        }

        public AccessModifier Access { get; }

        public string Identifier { get; private set; }

        public bool IsConst => Modifiers.Contains(SyntaxKind.ConstKeyword);

        public bool IsExtern => Modifiers.Contains(SyntaxKind.ExternKeyword);

        public bool IsReadonly => Modifiers.Contains(SyntaxKind.ReadOnlyKeyword);

        public bool IsStatic => Modifiers.Contains(SyntaxKind.StaticKeyword);

        public SyntaxKind Kind { get; }

        public IImmutableList<ParameterSyntax> MethodArguments { get; private set; } = ImmutableList<ParameterSyntax>.Empty;

        public int NumberOfMethodArguments { get; private set; } = 0;

        private ICollection<SyntaxKind> Modifiers { get; }

        private SyntaxTokenList GetSyntaxPropertiesForNode(CSharpSyntaxNode node)
        {
            switch (node)
            {
                case MethodDeclarationSyntax mds:
                    Identifier = mds.Identifier.ValueText;
                    IList<ParameterSyntax> methodArgs = mds.ParameterList.Parameters.ToList();
                    NumberOfMethodArguments = methodArgs.Count;
                    MethodArguments = methodArgs.ToImmutableList();
                    return mds.Modifiers;
                case PropertyDeclarationSyntax pds:
                    Identifier = pds.Identifier.ValueText;
                    return pds.Modifiers;
                case FieldDeclarationSyntax fds:
                    Identifier = fds.Declaration.Variables[0].Identifier.ValueText;
                    return fds.Modifiers;
                case ConstructorDeclarationSyntax cnds:
                    Identifier = cnds.Identifier.ValueText;
                    return cnds.Modifiers;
                case DelegateDeclarationSyntax dds:
                    Identifier = dds.Identifier.ValueText;
                    return dds.Modifiers;
                case EventFieldDeclarationSyntax efds:
                    Identifier = efds.Declaration.Variables[0].Identifier.ValueText;
                    return efds.Modifiers;
                case IndexerDeclarationSyntax ids:
                    Identifier = "this";
                    return ids.Modifiers;
                case OperatorDeclarationSyntax ods:
                    Identifier = ods.OperatorToken.ValueText;
                    return ods.Modifiers;
                case DestructorDeclarationSyntax dsds:
                    Identifier = dsds.Identifier.ValueText;
                    return dsds.Modifiers;
                case EventDeclarationSyntax evds:
                    Identifier = evds.Identifier.ValueText;
                    return evds.Modifiers;
                case EnumDeclarationSyntax eds:
                    Identifier = eds.Identifier.ValueText;
                    return eds.Modifiers;
                case ClassDeclarationSyntax cds:
                    Identifier = cds.Identifier.ValueText;
                    return cds.Modifiers;
                case StructDeclarationSyntax sds:
                    Identifier = sds.Identifier.ValueText;
                    return sds.Modifiers;
                case InterfaceDeclarationSyntax inds:
                    Identifier = inds.Identifier.ValueText;
                    return inds.Modifiers;
                case RecordDeclarationSyntax rds:
                    Identifier = rds.Identifier.ValueText;
                    return rds.Modifiers;
                default:
                    Identifier = "zzz";
                    return new SyntaxTokenList();
            }
        }
    }
}
