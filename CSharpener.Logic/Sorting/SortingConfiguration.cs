// --------------------------------------------------------------------
// SortingConfiguration.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace Gjeltema.CSharpener.Logic.Sorting
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;

    public sealed class SortingConfiguration
    {
        private static readonly IImmutableDictionary<string, AccessModifier> accessModifierConfigurationMapping;
        private static readonly IImmutableDictionary<string, SyntaxKind> kindSortOrderConfigurationMapping;
        private static readonly IImmutableDictionary<string, Sorter> modifiersConfigurationMapping;

        public SortingConfiguration()
        {
            IList<string> kindSortOrderConfigItems = CSharpenerConfigSettings.GetConfigItemsForSection(CSharpenerConfigSettings.KindSortOrderSectionName);
            KindSortOrder = InitializeKindSortOrder(kindSortOrderConfigItems);

            IList<string> accessModifierConfigItems = CSharpenerConfigSettings.GetConfigItemsForSection(CSharpenerConfigSettings.AccessModifierSectionName);
            AccessModifierSortOrder = InitializeAccessModifierSortOrder(accessModifierConfigItems);

            IList<string> modifierConfigItems = CSharpenerConfigSettings.GetConfigItemsForSection(CSharpenerConfigSettings.ModifierSectionName);
            ModifiersSortOrder = InitializeModifiersSortOrder(modifierConfigItems);
        }

        static SortingConfiguration()
        {
            ImmutableDictionary<string, SyntaxKind>.Builder kindConfigMappingBuilder = ImmutableDictionary.CreateBuilder<string, SyntaxKind>();
            kindConfigMappingBuilder.Add("enum", SyntaxKind.EnumDeclaration);
            kindConfigMappingBuilder.Add("delegate", SyntaxKind.DelegateDeclaration);
            kindConfigMappingBuilder.Add("field", SyntaxKind.FieldDeclaration);
            kindConfigMappingBuilder.Add("event", SyntaxKind.EventDeclaration); // Events declared as fields
            kindConfigMappingBuilder.Add("eventField", SyntaxKind.EventFieldDeclaration); // Events declared using the property-like syntax (with add and remove blocks)
            kindConfigMappingBuilder.Add("constructor", SyntaxKind.ConstructorDeclaration);
            kindConfigMappingBuilder.Add("destructor", SyntaxKind.DestructorDeclaration);
            kindConfigMappingBuilder.Add("indexer", SyntaxKind.IndexerDeclaration);
            kindConfigMappingBuilder.Add("property", SyntaxKind.PropertyDeclaration);
            kindConfigMappingBuilder.Add("operator", SyntaxKind.OperatorDeclaration);
            kindConfigMappingBuilder.Add("method", SyntaxKind.MethodDeclaration);
            kindConfigMappingBuilder.Add("struct", SyntaxKind.StructDeclaration);
            kindConfigMappingBuilder.Add("class", SyntaxKind.ClassDeclaration);
            kindConfigMappingBuilder.Add("interface", SyntaxKind.InterfaceDeclaration);
            kindSortOrderConfigurationMapping = kindConfigMappingBuilder.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

            ImmutableDictionary<string, AccessModifier>.Builder accessModifierConfigMappingBuilder = ImmutableDictionary.CreateBuilder<string, AccessModifier>();
            accessModifierConfigMappingBuilder.Add("public", AccessModifier.Public);
            accessModifierConfigMappingBuilder.Add("internal", AccessModifier.Internal);
            accessModifierConfigMappingBuilder.Add("protected", AccessModifier.Protected);
            accessModifierConfigMappingBuilder.Add("protectedinternal", AccessModifier.ProtectedInternal);
            accessModifierConfigMappingBuilder.Add("private", AccessModifier.Private);
            accessModifierConfigMappingBuilder.Add("privateprotected", AccessModifier.PrivateProtected);
            accessModifierConfigurationMapping = accessModifierConfigMappingBuilder.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

            ImmutableDictionary<string, Sorter>.Builder modifiersConfigMappingBuilder = ImmutableDictionary.CreateBuilder<string, Sorter>();
            modifiersConfigMappingBuilder.Add("kind", Sorter.Kind);
            modifiersConfigMappingBuilder.Add("extern", Sorter.Extern);
            modifiersConfigMappingBuilder.Add("const", Sorter.Const);
            modifiersConfigMappingBuilder.Add("readonly", Sorter.Readonly);
            modifiersConfigMappingBuilder.Add("static", Sorter.Static);
            modifiersConfigMappingBuilder.Add("accessibility", Sorter.Accessibility);
            modifiersConfigMappingBuilder.Add("name", Sorter.Name);
            modifiersConfigMappingBuilder.Add("numberofmethodargs", Sorter.NumberOfMethodArguments);
            modifiersConfigurationMapping = modifiersConfigMappingBuilder.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);
        }

        internal IDictionary<AccessModifier, int> AccessModifierSortOrder { get; }

        internal IDictionary<SyntaxKind, int> KindSortOrder { get; }

        internal IList<Sorter> ModifiersSortOrder { get; }

        private IDictionary<AccessModifier, int> InitializeAccessModifierSortOrder(IList<string> configurationItems)
        {
            IDictionary<AccessModifier, int> accessModifierFromConfig = new Dictionary<AccessModifier, int>();
            int sortOrder = 0;
            foreach (string configItem in configurationItems)
            {
                sortOrder++;
                AccessModifier configAccessModifier = accessModifierConfigurationMapping[configItem];
                accessModifierFromConfig.Add(configAccessModifier, sortOrder);
            }

            IEnumerable<AccessModifier> unspecified = accessModifierConfigurationMapping.Values.Except(accessModifierFromConfig.Keys);
            foreach (AccessModifier missedAccessModifier in unspecified)
            {
                sortOrder++;
                accessModifierFromConfig.Add(missedAccessModifier, sortOrder);
            }

            return accessModifierFromConfig;
        }

        private IDictionary<SyntaxKind, int> InitializeKindSortOrder(IList<string> configurationItems)
        {
            IDictionary<SyntaxKind, int> kindSortOrderFromConfig = new Dictionary<SyntaxKind, int>();
            int sortOrder = 0;
            foreach (string configItem in configurationItems)
            {
                sortOrder++;
                SyntaxKind configLineKind = kindSortOrderConfigurationMapping[configItem];
                kindSortOrderFromConfig.Add(configLineKind, sortOrder);
            }

            IEnumerable<SyntaxKind> unspecified = kindSortOrderConfigurationMapping.Values.Except(kindSortOrderFromConfig.Keys);
            foreach (SyntaxKind missedKind in unspecified)
            {
                sortOrder++;
                kindSortOrderFromConfig.Add(missedKind, sortOrder);
            }

            return kindSortOrderFromConfig;
        }

        private IList<Sorter> InitializeModifiersSortOrder(IList<string> configurationItems)
        {
            IList<Sorter> modifierFromConfig = new List<Sorter>();
            foreach (string configItem in configurationItems)
            {
                Sorter configModifier = modifiersConfigurationMapping[configItem];
                modifierFromConfig.Add(configModifier);
            }

            IEnumerable<Sorter> unspecified = modifiersConfigurationMapping.Values.Except(modifierFromConfig);
            foreach (Sorter missedModifier in unspecified)
            {
                modifierFromConfig.Add(missedModifier);
            }

            return modifierFromConfig;
        }
    }
}
