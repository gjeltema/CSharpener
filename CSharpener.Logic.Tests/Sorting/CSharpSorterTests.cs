﻿// -----------------------------------------------------------------------
// CSharpSorterTests.cs Copyright 2021 Craig Gjeltema
// -----------------------------------------------------------------------

namespace CSharpener.Logic.Tests.Sorting
{
    using System.Diagnostics;
    using Gjeltema.CSharpener.Logic.Sorting;
    using Gjeltema.CSharpener.Logic.Trivia;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    [TestFixture]
    public class CSharpSorterTests
    {
        [TestCase(TestData.ClassWithMultipleConstructors, TestData.ClassWithMultipleConstructorsAfterSort)]
        [TestCase(TestData.InterfaceBeforeSorting, TestData.InterfaceAfterSorting)]
        [TestCase(TestData.RecordBeforeSorting, TestData.RecordAfterSorting)]
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

        [TestCase(TestData.InterfaceBeforeSorting, TestData.InterfaceAfterSortingAndFormattingNewlines)]
        [TestCase(TestData.RecordBeforeSorting, TestData.RecordAfterSortingAndFormattingNewlines)]
        [TestCase(TestData.StructBeforeSorting, TestData.StructAfterSorting)]
        public void CodeText_WhenSorterIsRunWithWhitespaceFormatter_OutputsSortedCodeText(string inputString, string expectedOutput)
        {
            TestHelper.InitializeConfig(TestData.TestConfig);
            CSharpSyntaxNode root = TestHelper.CreateCSharpSyntaxRoot(inputString);

            var sh = new CSharpSorter();
            SyntaxNode sortedOutput = sh.Visit(root);

            var newlineFormatter = new NewlineFormatter();
            SyntaxNode formattedOutput = newlineFormatter.Visit(sortedOutput);

            string actualOutput = formattedOutput.ToFullString();
            Debug.WriteLine(actualOutput);
            Assert.That(actualOutput, Is.EqualTo(expectedOutput));
        }
    }
}
