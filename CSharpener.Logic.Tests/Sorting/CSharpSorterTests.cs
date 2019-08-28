// --------------------------------------------------------------------
// CSharpSorterTests.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace CSharpener.Logic.Tests.Sorting
{
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic.Sorting;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    [TestFixture]
    public class CSharpSorterTests
    {
        [TestCase(ComplexFileTestData.UsingsInAndOutsideNamespace, ComplexFileTestData.UsingsInAndOutsideNamespaceAfterSorting)]
        public void CodeText_WhenSorterIsRun_OutputsSortedCodeText(string inputString, string expectedOutput)
        {
            TestHelper.InitializeConfig(TestData.TestConfig);
            CSharpSyntaxNode root = TestHelper.CreateCSharpSyntaxRoot(inputString);

            var sh = new CSharpSorter();
            SyntaxNode output = sh.Visit(root);

            string actualOutput = output.ToFullString();
            Debug.WriteLine(actualOutput);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
