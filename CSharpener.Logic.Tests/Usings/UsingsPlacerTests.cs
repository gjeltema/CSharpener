// -----------------------------------------------------------------------
// UsingsPlacerTests.cs Copyright 2020 Craig Gjeltema
// -----------------------------------------------------------------------

namespace CSharpener.Logic.Tests.Usings
{
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic.Trivia;
    using Gjeltema.CSharpener.Logic.Usings;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    public class UsingsPlacerTests
    {
        [TestCase(TestData.UsingsInAndOutsideNamespace, TestData.UsingInAndOutAfterUsingsPlacer)]
        [TestCase(TestData.UsingsOnlyOutsideNamespace, TestData.UsingsOnlyOutAfterUsingsPlacer)]
        [TestCase(TestData.UsingsInAndOutsideFileScopedNamespace, TestData.UsingsInAndOutsideFileScopedNamespaceAfterUsingsPlacer)]
        public void CodeText_WhenUsingsFormatterIsRun_OutputsCodeTextWithUsingsPlacedInNamespace(string inputString, string expectedOutput)
        {
            CSharpSyntaxNode root = TestHelper.CreateCSharpSyntaxRoot(inputString);

            var usingsHelper = new UsingsPlacer();
            SyntaxNode usingsRoot = usingsHelper.ProcessUsings(root);

            string actualOutput = usingsRoot.ToFullString();
            Debug.WriteLine(actualOutput);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }

        [TestCase(TestData.UsingsInAndOutsideFileScopedNamespace, "ThirdTestFile.cs", TestData.UsingsInAndOutsideFileScopedNamespaceAfterUsingsPlacerAndHeader)]
        public void CodeText_WhenUsingsFormatterAndHeaderAreRun_OutputsCodeTextWithUsingsPlacedInNamespace(string inputString, string fileName, string expectedOutput)
        {
            TestHelper.InitializeConfig(TestData.TestConfig);

            CSharpSyntaxNode root = TestHelper.CreateCSharpSyntaxRoot(inputString);

            var usingsHelper = new UsingsPlacer();
            SyntaxNode usingsRoot = usingsHelper.ProcessUsings(root);

            var fhf = new FileHeaderFormatter();
            SyntaxNode fhfRoot = fhf.AddHeader(usingsRoot, fileName);

            string actualOutput = fhfRoot.ToFullString();
            Debug.WriteLine(actualOutput);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
