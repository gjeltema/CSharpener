// -----------------------------------------------------------------------
// ModifiersHelper.cs Copyright 2021 Craig Gjeltema
// -----------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public enum AccessModifier
    {
        NotExplicit,
        Public,
        Internal,
        Protected,
        ProtectedInternal,
        Private,
        PrivateProtected
    }

    internal static class ModifiersHelper
    {
        private static readonly IDictionary<SyntaxKind, AccessModifier> accessibilityModifiersForSyntaxKind = new Dictionary<SyntaxKind, AccessModifier>()
        {
            { SyntaxKind.PublicKeyword, AccessModifier.Public },
            { SyntaxKind.InternalKeyword, AccessModifier.Internal },
            { SyntaxKind.ProtectedKeyword, AccessModifier.Protected },
            { SyntaxKind.PrivateKeyword, AccessModifier.Private },
        };
        private static readonly IDictionary<SyntaxKind, AccessModifier> defaultAccessForSyntaxKind = new Dictionary<SyntaxKind, AccessModifier>
        {
            { SyntaxKind.EnumDeclaration, AccessModifier.Public },
            { SyntaxKind.ClassDeclaration, AccessModifier.Internal },
            { SyntaxKind.InterfaceDeclaration, AccessModifier.Internal },
            { SyntaxKind.StructDeclaration, AccessModifier.Internal },
            { SyntaxKind.RecordDeclaration, AccessModifier.Internal }
        };
        private static readonly IDictionary<AccessModifier, SyntaxTokenList> syntaxTokensForAccessibility = new Dictionary<AccessModifier, SyntaxTokenList>()
        {
            { AccessModifier.Public, SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)) },
            { AccessModifier.Internal, SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.InternalKeyword)) },
            { AccessModifier.Protected, SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword)) },
            { AccessModifier.ProtectedInternal, SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword), SyntaxFactory.Token(SyntaxKind.InternalKeyword)) },
            { AccessModifier.Private, SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)) },
            { AccessModifier.PrivateProtected, SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword), SyntaxFactory.Token(SyntaxKind.ProtectedKeyword)) },
            { AccessModifier.NotExplicit, SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)) },
        };

        public static AccessModifier GetAccessibility(SyntaxTokenList modifiers)
        {
            AccessModifier modifier = AccessModifier.NotExplicit;
            IEnumerable<SyntaxKind> accessModifiers = modifiers.Select(st => st.Kind()).Where(sk => accessibilityModifiersForSyntaxKind.Keys.Contains(sk));
            foreach (SyntaxKind accessModifier in accessModifiers)
            {
                modifier = DetermineAccessModifier(accessModifier, modifier);
            }

            return modifier;
        }

        public static AccessModifier GetAccessibilityOrDefault(SyntaxKind nodeKind, SyntaxTokenList modifiers)
        {
            AccessModifier modifier = GetAccessibility(modifiers);

            if (modifier == AccessModifier.NotExplicit)
            {
                modifier = GetDefaultAccessibility(nodeKind);
            }

            return modifier;
        }

        public static AccessModifier GetDefaultAccessibility(SyntaxKind kind)
        {
            if (defaultAccessForSyntaxKind.TryGetValue(kind, out AccessModifier defaultAccess))
            {
                return defaultAccess;
            }

            return AccessModifier.Private;
        }

        public static SyntaxTokenList GetModifiers(AccessModifier modifier)
            => syntaxTokensForAccessibility[modifier];

        public static ICollection<SyntaxKind> GetNonAccessibilityModifiers(SyntaxTokenList modifiers)
        {
            var filteredModifierList = modifiers.Select(st => st.Kind()).Where(sk => !accessibilityModifiersForSyntaxKind.Keys.Contains(sk)).ToList();
            return filteredModifierList;
        }

        private static AccessModifier DetermineAccessModifier(SyntaxKind modifierBeingEvaluated, AccessModifier existingModifier)
        {
            if (accessibilityModifiersForSyntaxKind.TryGetValue(modifierBeingEvaluated, out AccessModifier modifier))
            {
                // Handling for 'protected internal' and 'private protected'
                if (existingModifier == AccessModifier.Protected && modifier == AccessModifier.Internal)
                {
                    return AccessModifier.ProtectedInternal;
                }
                else if (existingModifier == AccessModifier.Private && modifier == AccessModifier.Protected)
                {
                    return AccessModifier.PrivateProtected;
                }
                else
                {
                    // If it's a modifier, and isn't one of the above cases, then it should be a "single" modifier
                    return modifier;
                }
            }

            return existingModifier;
        }
    }
}
