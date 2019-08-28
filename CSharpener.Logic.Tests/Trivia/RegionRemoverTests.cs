// --------------------------------------------------------------------
// RegionRemoverTests.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace CSharpener.Logic.Tests.Trivia
{
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic.Trivia;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    [TestFixture]
    public class RegionRemoverTests
    {
        [TestCase(ComplexFileTestData.UsingsInAndOutsideNamespace, ComplexFileTestData.UsingsInAndOutsideNamespaceAfterRegionRemover)]
        public void CodeText_WhenRegionFormatterIsRun_OutputsTextWithoutRegions(string inputString, string expectedOutput)
        {
            TestHelper.InitializeConfig(TestData.TestConfig);
            CSharpSyntaxNode root = TestHelper.CreateCSharpSyntaxRoot(inputString);

            var regionRemover = new RegionRemover();
            SyntaxNode newNode = regionRemover.Visit(root);

            string actualOutput = newNode.ToFullString();
            Debug.WriteLine(actualOutput);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
