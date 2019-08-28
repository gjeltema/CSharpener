// --------------------------------------------------------------------
// UsingsPlacerTests.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace CSharpener.Logic.Tests.Usings
{
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic.Usings;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    public class UsingsPlacerTests
    {
        [TestCase(ComplexFileTestData.UsingsInAndOutsideNamespace, ComplexFileTestData.UsingsInAndOutsideAfterUsingsPlacement)]
        [TestCase(ComplexFileTestData.UsingsOnlyOutsideNamespace, ComplexFileTestData.UsingsOnlyOutsideAfterUsingsPlacement)]
        public void CodeText_WhenUsingsFormatterIsRun_OutputsCodeTextWithUsingsPlacedInNamespace(string inputString, string expectedOutput)
        {
            CSharpSyntaxNode root = TestHelper.CreateCSharpSyntaxRoot(inputString);

            var usingsHelper = new UsingsPlacer();
            SyntaxNode usingsRoot = usingsHelper.ProcessUsings(root);

            string actualOutput = usingsRoot.ToFullString();
            Debug.WriteLine(actualOutput);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
