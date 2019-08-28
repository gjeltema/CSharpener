// --------------------------------------------------------------------
// TestHelper.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace CSharpener.Logic.Tests
{
    using System;
    using System.Collections.Generic;
    using Gjeltema.CSharpener.Logic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public static class TestHelper
    {
        public static CSharpSyntaxNode CreateCSharpSyntaxRoot(string codeText)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(codeText);
            var root = syntaxTree.GetRoot() as CSharpSyntaxNode;
            return root;
        }

        public static void InitializeConfig(string testConfig)
        {
            IList<string> configLines = GetConfigLines(testConfig);
            CSharpenerConfigSettings.InitializeSettings(configLines);
        }

        private static IList<string> GetConfigLines(string testConfig)
            => testConfig.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
    }
}
