// --------------------------------------------------------------------
// NewlineFormatterTests.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace CSharpener.Logic.Tests.Trivia
{
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic.Trivia;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    [TestFixture]
    public class NewlineFormatterTests
    {
        [TestCase(ComplexFileTestData.UsingsInAndOutsideNamespace, ComplexFileTestData.UsingsInAndOutsideNamespaceAfterNewlineFormatting)]
        [TestCase(TestData.ClassWithAttributes, TestData.ClassWithAttributesAfterNewline)]
        public void CodeText_WhenRegionAndNewlineFormattingsRun_OutputsExpectedText(string inputString, string expectedOutput)
        {
            TestHelper.InitializeConfig(TestData.TestConfig);
            CSharpSyntaxNode root = TestHelper.CreateCSharpSyntaxRoot(inputString);

            var regionRemover = new RegionRemover();
            SyntaxNode removedRegionsNode = regionRemover.Visit(root);

            var newLineFormatter = new NewlineFormatter();
            SyntaxNode newLineRoot = newLineFormatter.Visit(removedRegionsNode);

            string actualOutput = newLineRoot.ToFullString();
            Debug.WriteLine(actualOutput);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
