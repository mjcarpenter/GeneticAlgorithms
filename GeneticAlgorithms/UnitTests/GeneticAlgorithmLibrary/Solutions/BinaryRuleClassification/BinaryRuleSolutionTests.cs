using GeneticAlgorithmLibrary.Models;
using GeneticAlgorithmLibrary.Solutions.BinaryRuleClassification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTests.GeneticAlgorithmLibrary.Solutions.BinaryRuleClassification
{
    [TestClass]
    public class BinaryRuleSolutionTests
    {

        #region Methods

        #region Tests

        [TestMethod]
        public void BinaryRuleSolutionTest()
        {
            int length = 5;
            BinaryRule rule = GetFilledBinaryRule('0', length, true);
            BinaryRule[] rules = new BinaryRule[] { rule };
            BinaryDataSet dataSet = GetFilledBinaryDataSet(0, length);
            BinaryDataSet[] dataSets = new BinaryDataSet[] { dataSet };
            bool useWildcard = true;
            BinaryRuleSolution solution = new BinaryRuleSolution((BinaryRule[])rules.Clone(), (BinaryDataSet[])dataSets.Clone(), useWildcard);
            CollectionAssert.AreEqual(rules, solution.Rules);
        }

        [TestMethod]
        public void GetFitness_MatchingRuleAndDataTest()
        {
            int length = 5;
            BinaryRule rule = GetFilledBinaryRule('0', length, true);
            BinaryRule[] rules = new BinaryRule[] { rule };
            BinaryDataSet dataSet = GetFilledBinaryDataSet(0, length);
            BinaryDataSet[] dataSets = new BinaryDataSet[] { dataSet };
            bool useWildcard = true;
            BinaryRuleSolution solution = new BinaryRuleSolution(rules, dataSets, useWildcard);
            Assert.AreEqual(1, solution.GetFitness());
        }

        [TestMethod]
        public void GetFitness_NoMatchingRuleAndDataTest()
        {
            int length = 5;
            BinaryRule rule = GetFilledBinaryRule('0', length, true);
            BinaryRule[] rules = new BinaryRule[] { rule };
            BinaryDataSet dataSet = GetFilledBinaryDataSet(1, length);
            BinaryDataSet[] dataSets = new BinaryDataSet[] { dataSet };
            bool useWildcard = true;
            BinaryRuleSolution solution = new BinaryRuleSolution(rules, dataSets, useWildcard);
            Assert.AreEqual(0, solution.GetFitness());
        }

        [TestMethod]
        public void CrossoverTest()
        {
            int length = 5;
            BinaryRule rule1 = GetFilledBinaryRule('0', length, true);
            BinaryRule[] rules1 = new BinaryRule[] { rule1 };
            BinaryRule rule2 = GetFilledBinaryRule('1', length, true);
            BinaryRule[] rules2 = new BinaryRule[] { rule2 };
            BinaryDataSet dataSet = GetFilledBinaryDataSet(1, length);
            BinaryDataSet[] dataSets = new BinaryDataSet[] { dataSet };
            bool useWildcard = true;
            BinaryRuleSolution solution1 = new BinaryRuleSolution((BinaryRule[])rules1.Clone(), dataSets, useWildcard);
            BinaryRuleSolution solution2 = new BinaryRuleSolution((BinaryRule[])rules2.Clone(), dataSets, useWildcard);
            Solution c1;
            Solution c2;
            solution1.Crossover(solution2, out c1, out c2);
            BinaryRuleSolution child1 = (BinaryRuleSolution)c1;
            BinaryRuleSolution child2 = (BinaryRuleSolution)c2;
            CollectionAssert.AreNotEqual(rule1.Input, child1.Rules[0].Input, "First solution's rule input was unchanged.");
            Assert.AreNotEqual(rule1.Output, child1.Rules[0].Output, "First solution's rule output was unchanged.");
            CollectionAssert.AreNotEqual(rule2.Input, child2.Rules[0].Input, "Second solution's rule input was unchanged.");
            Assert.AreNotEqual(rule2.Output, child2.Rules[0].Output, "Second solution's rule output was unchanged.");
        }

        [TestMethod]
        public void Mutate_UseWildcardFalseTest()
        {
            int length = 5;
            BinaryRule rule = GetFilledBinaryRule('0', length, false);
            BinaryRule[] rules = new BinaryRule[] { rule };
            BinaryDataSet dataSet = GetFilledBinaryDataSet(0, length);
            BinaryDataSet[] dataSets = new BinaryDataSet[] { dataSet };
            bool useWildcard = false;
            BinaryRuleSolution solution = new BinaryRuleSolution(rules, dataSets, useWildcard);
            solution.Mutate(1);
            char[] expectedInput = new char[] { '1', '1', '1', '1' };
            char expectedOutput = '1';
            CollectionAssert.AreEqual(expectedInput, solution.Rules[0].Input, "Rule's input bits were not flipped.");
            Assert.AreEqual(expectedOutput, solution.Rules[0].Output, "Rule's output bit was not flipped.");
        }

        [TestMethod]
        public void Mutate_UseWildcardTrueTest()
        {
            int length = 1001;
            BinaryRule rule = GetFilledBinaryRule('0', length, true);
            BinaryRule[] rules = new BinaryRule[] { rule };
            BinaryDataSet dataSet = GetFilledBinaryDataSet(0, length);
            BinaryDataSet[] dataSets = new BinaryDataSet[] { dataSet };
            bool useWildcard = false;
            BinaryRuleSolution solution = new BinaryRuleSolution(rules, dataSets, useWildcard);
            solution.Mutate(1);
            bool wildcard = false;
            foreach (char c in solution.Rules[0].Input)
            {
                if (c == '#')
                {
                    wildcard = true;
                    break;
                }
            }
            Assert.IsTrue(wildcard, "There was not a single wildcard operator in the solution's mutated rule. There is a random chance of this happening and the code still functioning correctly.");
        }

        [TestMethod]
        public void Mutate_MutateWildcardTest()
        {
            int length = 5;
            BinaryRule rule = GetFilledBinaryRule('#', length, true);
            BinaryRule[] rules = new BinaryRule[] { rule };
            BinaryDataSet dataSet = GetFilledBinaryDataSet(0, length);
            BinaryDataSet[] dataSets = new BinaryDataSet[] { dataSet };
            bool useWildcard = false;
            BinaryRuleSolution solution = new BinaryRuleSolution(rules, dataSets, useWildcard);
            solution.Mutate(1);
            bool wildcard = false;
            foreach (char c in solution.Rules[0].Input)
            {
                if (c == '#')
                {
                    wildcard = true;
                    break;
                }
            }
            Assert.IsFalse(wildcard, "There was a wildcard operator in the solution's mutated rule.");
        }

        [TestMethod]
        public void GetFitness_DataSetTest()
        {
            int length = 5;
            BinaryRule rule = GetFilledBinaryRule('0', length, false);
            BinaryRule[] rules = new BinaryRule[] { rule };
            BinaryDataSet dataSet = GetFilledBinaryDataSet(1, length);
            BinaryDataSet[] dataSets = new BinaryDataSet[] { dataSet };
            bool useWildcard = false;
            BinaryRuleSolution solution = new BinaryRuleSolution(rules, dataSets, useWildcard);
            BinaryDataSet testDataSet = GetFilledBinaryDataSet(0, length);
            BinaryDataSet[] testDataSets = new BinaryDataSet[] { testDataSet };
            Assert.AreEqual(1, solution.GetFitness(testDataSets));
        }

        [TestMethod]
        public void ToStringTest()
        {
            int length = 5;
            BinaryRule rule = GetFilledBinaryRule('0', length, false);
            BinaryRule[] rules = new BinaryRule[] { rule, rule };
            BinaryDataSet dataSet = GetFilledBinaryDataSet(1, length);
            BinaryDataSet[] dataSets = new BinaryDataSet[] { dataSet };
            bool useWildcard = false;
            BinaryRuleSolution solution = new BinaryRuleSolution(rules, dataSets, useWildcard);
            string solutionString = solution.ToString();
            Assert.AreEqual("1: 0000 0\n2: 0000 0", solutionString);
        }

        #endregion

        private BinaryRule GetFilledBinaryRule(char value, int length, bool useWildcard)
        {
            char[] input = Enumerable.Repeat(value, length - 1).ToArray();
            char output = value;
            return new BinaryRule(input, output, useWildcard);
        }

        private BinaryDataSet GetFilledBinaryDataSet(int value, int length)
        {
            int[] input = Enumerable.Repeat(value, length - 1).ToArray();
            int output = value;
            return new BinaryDataSet(input, output);
        }

        #endregion

    }
}
