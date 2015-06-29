﻿//
using System;
using System.Text;
//
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace SlackerRunner.UnitTests
{
    [TestClass]
    public class ResultsParserTest
    {
        private static string ResultSomeFailed()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Beachcomber ((local))");
            sb.AppendLine(".F.*");
            sb.AppendLine(" ");
            sb.AppendLine("Finished in 0.11845 seconds");
            sb.AppendLine("3 examples, 1 failure");
            sb.AppendLine("");
            return sb.ToString();
        }

        private static string ResultAllPassed()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Beachcomber ((local))");
            sb.AppendLine("...");
            sb.AppendLine(" ");
            sb.AppendLine("Finished in 0.11845 seconds");
            sb.AppendLine("3 examples, 0 failures");
            sb.AppendLine("");
            return sb.ToString();
        }

        private static string UnforseenError()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Beachcomber ((local))");
            sb.AppendLine("...");
            sb.AppendLine(" ");
            sb.AppendLine("ODBC::Error: 28000 (18456) [Microsoft][ODBC SQL Server Driver][SQL Server]Login failed for user 'something'.");
            sb.AppendLine("");
            return sb.ToString();
        }

        [TestMethod]
        public void ParseUnforseenError()
        {
            var resultsParser = new ResultsParser();
            SlackerResults res = resultsParser.Parse(UnforseenError(), null);
            // advertise what what the score is 
            Console.WriteLine(res.getString());
            // Proof it 
            Assert.IsTrue(res.FailedSpecs > 0);
            Assert.IsFalse( res.Passed );
        }

        [TestMethod]
        public void ParseReturnsFailedSpecsWhenAllPassed()
        {
            var resultsParser = new ResultsParser();
            SlackerResults res = resultsParser.Parse(ResultAllPassed(), null);
            // advertise what what the score is 
            Console.WriteLine(res.getString());
            // Proof it 
            Assert.AreEqual(res.FailedSpecs, 0);
            Assert.AreEqual(res.PassedSpecs, 3);
        }

        [TestMethod]
        public void ParseReturnsFailedSpecsWhenSomeFailed()
        {
            var resultsParser = new ResultsParser();
            SlackerResults res = resultsParser.Parse(ResultSomeFailed(), null);
            // advertise what what the score is 
            Console.WriteLine(res.getString());
            // Proof it 
            Assert.AreEqual(res.FailedSpecs, 1);
            Assert.AreEqual(res.PassedSpecs, 2);
        }

        [TestMethod]
        public void ParseReturnsFalseWhenStandardErrorNotNull()
        {
            var resultsParser = new ResultsParser();
            SlackerResults SlackerResults = resultsParser.Parse(ResultAllPassed(), "StandardError");
            Assert.IsFalse(SlackerResults.Passed);
        }

        [TestMethod]
        public void ValidateReturnsFalseWhenOutResultsHaveFailedScenario()
        {
            var resultsParser = new ResultsParser();
            SlackerResults SlackerResults = resultsParser.Parse(ResultSomeFailed(), "StandardError");
            Assert.IsFalse(SlackerResults.Passed);
        }

        [TestMethod]
        public void ValidateReturnsTrueWhenOutputResultsPassedAndErrorIsEmpty()
        {
            var resultsParser = new ResultsParser();
            SlackerResults SlackerResults = resultsParser.Parse(ResultAllPassed(), "");
            Assert.IsTrue(SlackerResults.Passed);
        }

        [TestMethod]
        public void ValidateReturnsTrueWhenOutputResultsPassedAndErrorIsNull()
        {
            var resultsParser = new ResultsParser();
            SlackerResults SlackerResults = resultsParser.Parse(ResultAllPassed(), null);
            Assert.IsTrue(SlackerResults.Passed);
        }
    }
}