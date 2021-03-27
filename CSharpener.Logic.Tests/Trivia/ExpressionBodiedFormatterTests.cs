// -----------------------------------------------------------------------
// ExpressionBodiedFormatterTests.cs Copyright 2021 Craig Gjeltema
// -----------------------------------------------------------------------

namespace CSharpener.Logic.Tests.Trivia
{
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic.Trivia;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    [TestFixture]
    public class ExpressionBodiedFormatterTests
    {
        [TestCase(TestData.ExpressionBodiedMethod, TestData.ExpressionBodiedMethodAfterFormatting)]
        public void CodeText_WhenHeaderFormatterRun_OutputsExpectedText(string inputString, string expectedOutput)
        {
            TestHelper.InitializeConfig(TestData.TestConfig);
            CSharpSyntaxNode root = TestHelper.CreateCSharpSyntaxRoot(inputString);

            var ebf = new ExpressionBodiedFormatter();
            SyntaxNode ebfResult = ebf.Visit(root);

            string actualOutput = ebfResult.ToFullString();
            Debug.WriteLine(actualOutput);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
