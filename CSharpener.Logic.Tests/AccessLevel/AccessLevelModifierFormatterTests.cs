// -----------------------------------------------------------------------
// AccessLevelModifierFormatterTests.cs Copyright 2021 Craig Gjeltema
// -----------------------------------------------------------------------

namespace CSharpener.Logic.Tests.AccessLevel
{
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic.AccessLevel;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    [TestFixture]
    public class AccessLevelModifierFormatterTests
    {
        [TestCase(TestData.ClassBeforeAccessLevelModifierFormat, TestData.ClassAfterAccessLevelModifierFormat)]
        public void CodeText_WhenAccessLevelFormatterIsRun_OutputsFormattedCodeText(string inputString, string expectedOutput)
        {
            TestHelper.InitializeConfig(TestData.TestConfig);
            CSharpSyntaxNode root = TestHelper.CreateCSharpSyntaxRoot(inputString);

            var alm = new AccessLevelModifierFormatter();
            SyntaxNode output = alm.Visit(root);

            string actualOutput = output.ToFullString();
            Debug.WriteLine(actualOutput);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
