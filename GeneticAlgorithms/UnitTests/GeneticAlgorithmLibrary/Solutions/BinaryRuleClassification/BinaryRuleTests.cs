using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeneticAlgorithmLibrary.Solutions.BinaryRuleClassification;
using System;

namespace UnitTests.GeneticAlgorithmLibrary.Solutions.BinaryRuleClassification
{
    [TestClass]
    public class BinaryRuleTests
    {

        #region Tests

        [TestMethod]
        public void BinaryRuleTest()
        {
            char[] input = new char[] { '0', '1', '0', '1' };
            char output = '0';
            bool useWildcard = true;
            BinaryRule rule = new BinaryRule((char[])input.Clone(), output, useWildcard);
            CollectionAssert.AreEqual(input, rule.Input, "Input did not equal expected value.");
            Assert.AreEqual(output, rule.Output, "Output did not equal expected value.");
            Assert.AreEqual(useWildcard, rule.UseWildcard, "UseWildcard did not equal expected value.");
        }

        [TestMethod]
        public void BinaryRule_WildcardOutputTest()
        {
            char[] input = new char[] { '0', '1', '0', '1' };
            char output = '#';
            bool useWildcard = true;
            for (int i = 0; i < 100; i++)
            {
                BinaryRule rule = new BinaryRule((char[])input.Clone(), output, useWildcard);
                CollectionAssert.AreEqual(input, rule.Input, "Input did not equal expected value.");
                Assert.AreNotEqual(output, rule.Output, "Output did not equal expected value.");
                Assert.AreEqual(useWildcard, rule.UseWildcard, "UseWildcard did not equal expected value.");
            }
        }

        [TestMethod]
        public void GenerateRule_UseWildcardFalseTest()
        {
            BinaryRule rule = BinaryRule.GenerateRule(5, false);
            char[] input = rule.Input;
            foreach (char c in input)
            {
                Assert.AreNotEqual('#', c, "Input character was a wildcard operator.");
            }
            char output = rule.Output;
            Assert.AreNotEqual('#', output, "Output character was a wildcard operator.");
        }

        [TestMethod]
        public void GenerateRule_UseWildcardTrueTest()
        {
            BinaryRule rule = BinaryRule.GenerateRule(1001, true);
            bool wildcard = false;
            char[] input = rule.Input;
            foreach (char c in input)
            {
                if (c == '#')
                {
                    wildcard = true;
                    break;
                }
            }
            Assert.IsTrue(wildcard, "There was not a single wildcard operator in the generated rule. There is a random chance of this happening and the code still functioning correctly.");
        }

        [TestMethod]
        public void GetRulesTest()
        {
            int ruleLength = 5;
            char[] bitString = new char[ruleLength * 2];
            Random rand = new Random();
            for (int i = 0; i < bitString.Length; i++)
            {
                bitString[i] = char.Parse(rand.Next(0, 2).ToString());
            }
            bool useWildcard = true;
            char[] rule1Input = new char[ruleLength - 1];
            char[] rule2Input = new char[ruleLength - 1];
            char rule1Output;
            char rule2Output;
            Array.Copy(bitString, 0, rule1Input, 0, ruleLength - 1);
            rule1Output = bitString[ruleLength - 1];
            Array.Copy(bitString, ruleLength, rule2Input, 0, ruleLength - 1);
            rule2Output = bitString[bitString.Length - 1];
            BinaryRule[] rules = BinaryRule.GetRules(bitString, ruleLength, useWildcard);
            Assert.AreEqual(2, rules.Length, "Returned array was wrong length.");
            CollectionAssert.AreEqual(rule1Input, rules[0].Input, "First rule's input was incorrect.");
            Assert.AreEqual(rule1Output, rules[0].Output, "First rule's output was incorrect.");
            CollectionAssert.AreEqual(rule2Input, rules[1].Input, "Second rule's input was incorrect.");
            Assert.AreEqual(rule2Output, rules[1].Output, "Second rule's output was incorrect.");
        }

        [TestMethod]
        public void CrossoverTest()
        {
            char[] input1 = new char[] { '0', '0', '0', '0' };
            char output1 = '0';
            char[] input2 = new char[] { '1', '1', '1', '1' };
            char output2 = '1';
            bool useWildcard = true;
            BinaryRule rule1 = new BinaryRule((char[])input1.Clone(), output1, useWildcard);
            BinaryRule rule2 = new BinaryRule((char[])input2.Clone(), output2, useWildcard);
            BinaryRule child1;
            BinaryRule child2;
            rule1.Crossover(rule2, out child1, out child2);
            CollectionAssert.AreNotEqual(input1, child1.Input, "First rule's input was unchanged.");
            Assert.AreNotEqual(output1, child1.Output, "First rule's output was unchanged.");
            CollectionAssert.AreNotEqual(input2, child2.Input, "Second rule's input was unchanged.");
            Assert.AreNotEqual(output2, child2.Output, "Second rule's output was unchanged.");
        }

        [TestMethod]
        public void Mutate_UseWildcardFalseTest()
        {
            char[] input = new char[] { '0', '0', '0', '0' };
            char output = '0';
            bool useWildcard = false;
            BinaryRule rule = new BinaryRule(input, output, useWildcard);
            rule.Mutate(1);
            char[] expectedInput = new char[] { '1', '1', '1', '1' };
            char expectedOutput = '1';
            CollectionAssert.AreEqual(expectedInput, rule.Input, "Input bits were not flipped.");
            Assert.AreEqual(expectedOutput, rule.Output, "Output bit was not flipped.");
        }

        [TestMethod]
        public void Mutate_UseWildcardTrueTest()
        {
            char[] input = new char[1000];
            char output = '0';
            bool useWildcard = true;
            BinaryRule rule = new BinaryRule(input, output, useWildcard);
            rule.Mutate(1);
            bool wildcard = false;
            foreach (char c in rule.Input)
            {
                if (c == '#')
                {
                    wildcard = true;
                    break;
                }
            }
            Assert.IsTrue(wildcard, "There was not a single wildcard operator in the mutated rule. There is a random chance of this happening and the code still functioning correctly.");
        }

        [TestMethod]
        public void Mutate_MutateWildcardTest()
        {
            char[] input = new char[] { '#', '#', '#', '#' };
            char output = '0';
            bool useWildcard = true;
            BinaryRule rule = new BinaryRule(input, output, useWildcard);
            rule.Mutate(1);
            bool wildcard = false;
            foreach (char c in rule.Input)
            {
                if (c == '#')
                {
                    wildcard = true;
                    break;
                }
            }
            Assert.IsFalse(wildcard, "There was a wildcard operator in the mutated rule.");
        }

        [TestMethod]
        public void InputMatches_True_NoWildcardTest()
        {
            char[] input = new char[] { '0', '1', '0', '1' };
            char output = '0';
            bool useWildcard = false;
            BinaryRule rule = new BinaryRule(input, output, useWildcard);
            int[] dataInput = new int[] { 0, 1, 0, 1 };
            int dataOutput = 0;
            BinaryDataSet dataSet = new BinaryDataSet(dataInput, dataOutput);
            Assert.IsTrue(rule.InputMatches(dataSet), "Method did not return correctly.");
        }

        [TestMethod]
        public void InputMatches_True_WildcardTest()
        {
            char[] input = new char[] { '0', '#', '0', '1' };
            char output = '0';
            bool useWildcard = true;
            BinaryRule rule = new BinaryRule(input, output, useWildcard);
            int[] dataInput = new int[] { 0, 0, 0, 1 };
            int dataOutput = 0;
            BinaryDataSet dataSet = new BinaryDataSet(dataInput, dataOutput);
            Assert.IsTrue(rule.InputMatches(dataSet), "Method did not return correctly.");
        }

        [TestMethod]
        public void InputMatches_False_NoWildcardTest()
        {
            char[] input = new char[] { '0', '0', '0', '1' };
            char output = '0';
            bool useWildcard = false;
            BinaryRule rule = new BinaryRule(input, output, useWildcard);
            int[] dataInput = new int[] { 0, 1, 0, 1 };
            int dataOutput = 0;
            BinaryDataSet dataSet = new BinaryDataSet(dataInput, dataOutput);
            Assert.IsFalse(rule.InputMatches(dataSet), "Method did not return correctly.");
        }

        [TestMethod]
        public void InputMatches_False_WildcardTest()
        {
            char[] input = new char[] { '0', '#', '0', '1' };
            char output = '0';
            bool useWildcard = true;
            BinaryRule rule = new BinaryRule(input, output, useWildcard);
            int[] dataInput = new int[] { 1, 0, 0, 1 };
            int dataOutput = 0;
            BinaryDataSet dataSet = new BinaryDataSet(dataInput, dataOutput);
            Assert.IsFalse(rule.InputMatches(dataSet), "Method did not return correctly.");
        }

        [TestMethod]
        public void InputMatches_InvalidCharacterTest()
        {
            char[] input = new char[] { 'f', 'q', 'h', ']' };
            char output = '0';
            bool useWildcard = true;
            BinaryRule rule = new BinaryRule(input, output, useWildcard);
            int[] dataInput = new int[] { 1, 0, 0, 1 };
            int dataOutput = 0;
            BinaryDataSet dataSet = new BinaryDataSet(dataInput, dataOutput);
            Assert.IsFalse(rule.InputMatches(dataSet), "Method did not return correctly.");
        }

        [TestMethod]
        public void OutputMatches_TrueTest()
        {
            char[] input = new char[] { '0', '1', '0', '1' };
            char output = '0';
            bool useWildcard = true;
            BinaryRule rule = new BinaryRule(input, output, useWildcard);
            int[] dataInput = new int[] { 0, 1, 0, 1 };
            int dataOutput = 0;
            BinaryDataSet dataSet = new BinaryDataSet(dataInput, dataOutput);
            Assert.IsTrue(rule.OutputMatches(dataSet), "Method did not return correctly.");
        }

        [TestMethod]
        public void OutputMatches_FalseTest()
        {
            char[] input = new char[] { '0', '1', '0', '1' };
            char output = '1';
            bool useWildcard = true;
            BinaryRule rule = new BinaryRule(input, output, useWildcard);
            int[] dataInput = new int[] { 0, 1, 0, 1 };
            int dataOutput = 0;
            BinaryDataSet dataSet = new BinaryDataSet(dataInput, dataOutput);
            Assert.IsFalse(rule.OutputMatches(dataSet), "Method did not return correctly.");
        }

        [TestMethod]
        public void ToStringTest()
        {
            char[] input = new char[] { '0', '1', '0', '1' };
            char output = '1';
            bool useWildcard = false;
            BinaryRule rule = new BinaryRule(input, output, useWildcard);
            string expected = "0101 1";
            string result = rule.ToString();
            Assert.AreEqual(expected, result, "String was not the same as expected.");
        }

        #endregion

    }
}
