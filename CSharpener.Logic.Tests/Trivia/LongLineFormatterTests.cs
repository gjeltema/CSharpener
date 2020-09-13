// --------------------------------------------------------------------
// LongLineFormatterTests.cs Copyright 2020 Craig Gjeltema
// --------------------------------------------------------------------

namespace CSharpener.Logic.Tests.Trivia
{
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic;
    using Gjeltema.CSharpener.Logic.Trivia;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    [TestFixture]
    public class LongLineFormatterTests
    {
        [TestCase(TestData.ClassWithLongLines, TestData.ClassWithLongLinesAfterFormat)]
        public void CodeText_WhenHeaderFormatterRun_OutputsExpectedText(string inputString, string expectedOutput)
        {
            TestHelper.InitializeConfig(TestData.TestConfig);
            CSharpSyntaxNode root = TestHelper.CreateCSharpSyntaxRoot(inputString);

            var llf = new LongLineFormatter(CSharpenerConfigSettings.LengthOfLineToBreakOn);
            SyntaxNode llfRoot = llf.Visit(root);

            string actualOutput = llfRoot.ToFullString();
            Debug.WriteLine(actualOutput);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
