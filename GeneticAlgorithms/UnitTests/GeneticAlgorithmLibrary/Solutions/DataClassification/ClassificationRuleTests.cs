using GeneticAlgorithmLibrary.Solutions.DataClassification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTests.GeneticAlgorithmLibrary.Solutions.DataClassification
{
    [TestClass()]
    public class ClassificationRuleTests
    {

        #region Tests

        [TestMethod]
        public void ClassificationRuleTest()
        {
            Tuple<double, double>[] valuePairs = new Tuple<double, double>[] { new Tuple<double, double>(0.2, 0.6) };
            int output = 0;
            ClassificationRule rule = new ClassificationRule((Tuple<double, double>[])valuePairs.Clone(), output, 0, 1);
            CollectionAssert.AreEqual(valuePairs, rule.ValuePairs, "Input did not equal expected value.");
            Assert.AreEqual(output, rule.Output, "Output did not equal expected value.");
        }

        [TestMethod]
        public void GenerateRuleTest()
        {
            ClassificationRule rule = ClassificationRule.GenerateRule(5, 0, 1);
            Assert.AreEqual(4, rule.ValuePairs.Length, "Generated rule did not generate correct number of value pairs.");
        }

        [TestMethod]
        public void CrossoverTest()
        {
            Tuple<double, double>[] input1 = new Tuple<double, double>[] { new Tuple<double, double>(0.2, 0.6), new Tuple<double, double>(0.3, 0.7), new Tuple<double, double>(0.4, 0.8) };
            Tuple<double, double>[] input2 = new Tuple<double, double>[] { new Tuple<double, double>(0.1, 0.3), new Tuple<double, double>(0.2, 0.4), new Tuple<double, double>(0.3, 0.5) };
            int output1 = 0;
            int output2 = 1;
            ClassificationRule rule1 = new ClassificationRule((Tuple<double, double>[])input1.Clone(), output1, 0, 1);
            ClassificationRule rule2 = new ClassificationRule((Tuple<double, double>[])input2.Clone(), output2, 0, 1);
            ClassificationRule child1;
            ClassificationRule child2;
            rule1.Crossover(rule2, out child1, out child2);
            CollectionAssert.AreNotEqual(input1, child1.ValuePairs, "First rule's input was unchanged.");
            Assert.AreNotEqual(output1, child1.Output, "First rule's output was unchanged.");
            CollectionAssert.AreNotEqual(input2, child2.ValuePairs, "Second rule's input was unchanged.");
            Assert.AreNotEqual(output2, child2.Output, "Second rule's output was unchanged.");
        }

        [TestMethod]
        public void MutateTest()
        {
            Tuple<double, double>[] input = new Tuple<double, double>[] { new Tuple<double, double>(0.2, 0.6), new Tuple<double, double>(0.3, 0.7), new Tuple<double, double>(0.4, 0.8) };
            int output = 0;
            ClassificationRule rule = new ClassificationRule((Tuple<double, double>[])input.Clone(), output, 0, 1);
            rule.Mutate(1, 1);
            int expectedOutput = 1;
            CollectionAssert.AreNotEqual(input, rule.ValuePairs, "Value pairs went unchanged.");
            Assert.AreEqual(expectedOutput, rule.Output, "Output bit was not flipped.");
        }

        [TestMethod]
        public void Mutate_CheckMutationStepTest()
        {
            Tuple<double, double>[] input = Enumerable.Repeat(new Tuple<double, double>(0, 0), 1000).ToArray();
            int output = 0;
            ClassificationRule rule = new ClassificationRule((Tuple<double, double>[])input.Clone(), output, 0, 1);
            rule.Mutate(1, 0.1);
            Tuple<double, double>[] newInput = rule.ValuePairs;
            foreach (Tuple<double, double> tuple in newInput)
            {
                if (tuple.Item1 < 0 || tuple.Item1 > 0.1 || tuple.Item2 < 0 || tuple.Item2 > 0.1)
                {
                    Assert.Fail("Tuple value was outside of expected range.");
                }
                else if (tuple.Item1 > tuple.Item2)
                {
                    Assert.Fail("Tuple was not in correct order.");
                }
            }
        }

        [TestMethod]
        public void InputMatches_TrueTest()
        {
            Tuple<double, double>[] input = new Tuple<double, double>[] { new Tuple<double, double>(0.2, 0.6), new Tuple<double, double>(0.3, 0.7), new Tuple<double, double>(0.4, 0.8) };
            int output = 0;
            ClassificationRule rule = new ClassificationRule((Tuple<double, double>[])input.Clone(), output, 0, 1);
            double[] dataInput = new double[] { 0.5, 0.35, 0.67 };
            int dataOutput = 1;
            RealDataSet dataSet = new RealDataSet(dataInput, dataOutput);
            Assert.IsTrue(rule.InputMatches(dataSet));
        }

        [TestMethod]
        public void InputMatches_FalseTest()
        {
            Tuple<double, double>[] input = new Tuple<double, double>[] { new Tuple<double, double>(0.2, 0.6), new Tuple<double, double>(0.3, 0.7), new Tuple<double, double>(0.4, 0.8) };
            int output = 0;
            ClassificationRule rule = new ClassificationRule((Tuple<double, double>[])input.Clone(), output, 0, 1);
            double[] dataInput = new double[] { 0.5, 0.35, 1 };
            int dataOutput = 1;
            RealDataSet dataSet = new RealDataSet(dataInput, dataOutput);
            Assert.IsFalse(rule.InputMatches(dataSet));
        }

        [TestMethod]
        public void OutputMatches_TrueTest()
        {
            Tuple<double, double>[] input = new Tuple<double, double>[] { new Tuple<double, double>(0.2, 0.6), new Tuple<double, double>(0.3, 0.7), new Tuple<double, double>(0.4, 0.8) };
            int output = 0;
            ClassificationRule rule = new ClassificationRule((Tuple<double, double>[])input.Clone(), output, 0, 1);
            double[] dataInput = new double[] { 1, 1, 0.67 };
            int dataOutput = 0;
            RealDataSet dataSet = new RealDataSet(dataInput, dataOutput);
            Assert.IsTrue(rule.OutputMatches(dataSet));
        }

        [TestMethod]
        public void OutputMatches_FalseTest()
        {
            Tuple<double, double>[] input = new Tuple<double, double>[] { new Tuple<double, double>(0.2, 0.6), new Tuple<double, double>(0.3, 0.7), new Tuple<double, double>(0.4, 0.8) };
            int output = 0;
            ClassificationRule rule = new ClassificationRule((Tuple<double, double>[])input.Clone(), output, 0, 1);
            double[] dataInput = new double[] { 1, 1, 0.67 };
            int dataOutput = 1;
            RealDataSet dataSet = new RealDataSet(dataInput, dataOutput);
            Assert.IsFalse(rule.OutputMatches(dataSet));
        }

        [TestMethod]
        public void ToStringTest()
        {
            Tuple<double, double>[] input = new Tuple<double, double>[] { new Tuple<double, double>(0.2, 0.6), new Tuple<double, double>(0.3, 0.7), new Tuple<double, double>(0.4, 0.8) };
            int output = 0;
            ClassificationRule rule = new ClassificationRule((Tuple<double, double>[])input.Clone(), output, 0, 1);
            string ruleString = rule.ToString();
            Assert.AreEqual("<0.2, 0.6> <0.3, 0.7> <0.4, 0.8> 0", ruleString);
        }

        #endregion

    }
}
