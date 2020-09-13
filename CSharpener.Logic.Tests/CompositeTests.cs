// --------------------------------------------------------------------
// CompositeTests.cs Copyright 2020 Craig Gjeltema
// --------------------------------------------------------------------

namespace CSharpener.Logic.Tests
{
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic.Sorting;
    using Gjeltema.CSharpener.Logic.Trivia;
    using Gjeltema.CSharpener.Logic.Usings;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    [TestFixture]
    public class CompositeTests
    {
        [TestCase(ComplexFileTestData.UsingsInAndOutsideNamespace, "TestFile.cs", ComplexFileTestData.UsingsInAndOutsideNamespaceAfterCompositeRun)]
        [TestCase(ComplexFileTestData.UsingsOnlyOutsideNamespace, "AnotherTestFile.cs", ComplexFileTestData.UsingsOnlyOutsideNamespaceAfterCompositeRun)]
        public void CodeText_WhenAllFormattingsRun_OutputsExpectedCodeText(string inputString, string fileName, string expectedOutput)
        {
            TestHelper.InitializeConfig(TestData.TestConfig);
            CSharpSyntaxNode root = TestHelper.CreateCSharpSyntaxRoot(inputString);

            var regionRemover = new RegionRemover();
            SyntaxNode regionRoot = regionRemover.Visit(root);

            var usingsHelper = new UsingsPlacer();
            SyntaxNode usingsRoot = usingsHelper.ProcessUsings(regionRoot);

            var fhf = new FileHeaderFormatter();
            SyntaxNode fhfRoot = fhf.AddHeader(usingsRoot, fileName);

            var sorter = new CSharpSorter();
            SyntaxNode sorterRoot = sorter.Visit(fhfRoot);

            var newLineFormatter = new NewlineFormatter();
            SyntaxNode formattedRoot = newLineFormatter.Visit(sorterRoot);

            string actualOutput = formattedRoot.ToFullString();
            Debug.WriteLine(actualOutput);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
