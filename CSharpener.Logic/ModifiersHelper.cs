// --------------------------------------------------------------------
// ModifiersHelper.cs Copyright 2020 Craig Gjeltema
// --------------------------------------------------------------------

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
        };

        public static AccessModifier GetAccessibility(SyntaxKind nodeKind, SyntaxTokenList modifiers)
        {
            AccessModifier modifier = AccessModifier.NotExplicit;
            IEnumerable<SyntaxKind> accessModifiers = modifiers.Select(st => st.Kind()).Where(sk => accessibilityModifiersForSyntaxKind.Keys.Contains(sk));
            foreach (SyntaxKind accessModifier in accessModifiers)
            {
                modifier = DetermineAccessModifier(accessModifier, modifier);
            }

            if (modifier == AccessModifier.NotExplicit)
            {
                modifier = SetDefaultAccessibility(nodeKind);
            }

            return modifier;
        }

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

        private static AccessModifier SetDefaultAccessibility(SyntaxKind kind)
        {
            if (defaultAccessForSyntaxKind.TryGetValue(kind, out AccessModifier defaultAccess))
            {
                return defaultAccess;
            }

            return AccessModifier.Private;
        }
    }
}
