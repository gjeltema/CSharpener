// --------------------------------------------------------------------
// FileHeaderFormatterTests.cs Copyright 2020 Craig Gjeltema
// --------------------------------------------------------------------

namespace CSharpener.Logic.Tests.Trivia
{
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic.Trivia;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    [TestFixture]
    public class FileHeaderFormatterTests
    {
        [TestCase(ComplexFileTestData.UsingsInAndOutsideNamespace, "TestFile.cs", ComplexFileTestData.UsingsInAndOutsideNamespaceAfterFileHeader)]
        [TestCase(ComplexFileTestData.UsingsOnlyOutsideNamespace, "AnotherTestFile.cs", ComplexFileTestData.UsingsOnlyOutsideNamespaceAfterFileHeader)]
        public void CodeText_WhenHeaderFormatterRun_OutputsExpectedText(string inputString, string fileName, string expectedOutput)
        {
            TestHelper.InitializeConfig(TestData.TestConfig);
            var root = TestHelper.CreateCSharpSyntaxRoot(inputString) as CSharpSyntaxNode;

            var fhh = new FileHeaderFormatter();
            SyntaxNode fhhRoot = fhh.AddHeader(root, fileName);

            string actualOutput = fhhRoot.ToFullString();
            Debug.WriteLine(actualOutput);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
