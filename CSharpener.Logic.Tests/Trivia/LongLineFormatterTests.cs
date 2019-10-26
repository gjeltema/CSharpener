// --------------------------------------------------------------------
// LongLineFormatterTests.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace CSharpener.Logic.Tests.Trivia
{
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic.Trivia;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    [TestFixture]
    public class LongLineFormatterTests
    {
        [TestCase(TestData.ClassWithLongLines)]
        public void CodeText_WhenHeaderFormatterRun_OutputsExpectedText(string inputString)
        {
            TestHelper.InitializeConfig(TestData.TestConfig);
            var root = TestHelper.CreateCSharpSyntaxRoot(inputString) as CSharpSyntaxNode;

            var llf = new LongLineFormatter();
            SyntaxNode llfRoot = llf.Visit(root);

            string actualOutput = llfRoot.ToFullString();
            Debug.WriteLine(actualOutput);
            //Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
