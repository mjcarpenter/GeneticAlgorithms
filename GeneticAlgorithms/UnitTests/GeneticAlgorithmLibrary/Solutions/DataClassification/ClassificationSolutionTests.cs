using GeneticAlgorithmLibrary.Models;
using GeneticAlgorithmLibrary.Solutions.DataClassification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTests.GeneticAlgorithmLibrary.Solutions.DataClassification
{
    [TestClass]
    public class ClassificationSolutionTests
    {

        #region Methods

        #region Tests

        [TestMethod]
        public void ClassificationSolutionTest()
        {
            int length = 5;
            ClassificationRule rule = GetFilledClassificationRule(0, 0.2, 0, length);
            ClassificationRule[] rules = new ClassificationRule[] { rule };
            RealDataSet dataSet = GetFilledRealDataSet(0.1, 0, length);
            RealDataSet[] dataSets = new RealDataSet[] { dataSet };
            double maximumMutationStep = 0.5;
            ClassificationSolution solution = new ClassificationSolution(rules, dataSets, maximumMutationStep);
            CollectionAssert.AreEqual(rules, solution.Rules);
        }

        [TestMethod]
        public void GetFitness_MatchingRuleAndDataTest()
        {
            int length = 5;
            ClassificationRule rule = GetFilledClassificationRule(0, 0.2, 0, length);
            ClassificationRule[] rules = new ClassificationRule[] { rule };
            RealDataSet dataSet = GetFilledRealDataSet(0.1, 0, length);
            RealDataSet[] dataSets = new RealDataSet[] { dataSet };
            double maximumMutationStep = 0.5;
            ClassificationSolution solution = new ClassificationSolution(rules, dataSets, maximumMutationStep);
            Assert.AreEqual(1, solution.GetFitness());
        }

        [TestMethod]
        public void GetFitness_NoMatchingRuleAndDataTest()
        {
            int length = 5;
            ClassificationRule rule = GetFilledClassificationRule(0, 0.2, 0, length);
            ClassificationRule[] rules = new ClassificationRule[] { rule };
            RealDataSet dataSet = GetFilledRealDataSet(0.5, 0, length);
            RealDataSet[] dataSets = new RealDataSet[] { dataSet };
            double maximumMutationStep = 0.5;
            ClassificationSolution solution = new ClassificationSolution(rules, dataSets, maximumMutationStep);
            Assert.AreEqual(0, solution.GetFitness());
        }

        [TestMethod]
        public void CrossoverTest()
        {
            int length = 5;
            ClassificationRule rule1 = GetFilledClassificationRule(0, 0.2, 0, length);
            ClassificationRule[] rules1 = new ClassificationRule[] { rule1 };
            ClassificationRule rule2 = GetFilledClassificationRule(0.8, 1, 1, length);
            ClassificationRule[] rules2 = new ClassificationRule[] { rule2 };
            RealDataSet dataSet = GetFilledRealDataSet(0.5, 0, length);
            RealDataSet[] dataSets = new RealDataSet[] { dataSet };
            double maximumMutationStep = 0.5;
            ClassificationSolution solution1 = new ClassificationSolution((ClassificationRule[])rules1.Clone(), dataSets, maximumMutationStep);
            ClassificationSolution solution2 = new ClassificationSolution((ClassificationRule[])rules2.Clone(), dataSets, maximumMutationStep);
            Solution c1;
            Solution c2;
            solution1.Crossover(solution2, out c1, out c2);
            ClassificationSolution child1 = (ClassificationSolution)c1;
            ClassificationSolution child2 = (ClassificationSolution)c2;
            CollectionAssert.AreNotEqual(rule1.ValuePairs, child1.Rules[0].ValuePairs, "First solution's rule input was unchanged.");
            Assert.AreNotEqual(rule1.Output, child1.Rules[0].Output, "First solution's rule output was unchanged.");
            CollectionAssert.AreNotEqual(rule2.ValuePairs, child2.Rules[0].ValuePairs, "Second solution's rule input was unchanged.");
            Assert.AreNotEqual(rule2.Output, child2.Rules[0].Output, "Second solution's rule output was unchanged.");
        }

        [TestMethod]
        public void MutateTest()
        {
            int length = 5;
            ClassificationRule rule = GetFilledClassificationRule(0.5, 0.5, 0, length);
            ClassificationRule[] rules = new ClassificationRule[] { rule };
            RealDataSet dataSet = GetFilledRealDataSet(0.5, 0, length);
            RealDataSet[] dataSets = new RealDataSet[] { dataSet };
            double maximumMutationStep = 0.1;
            ClassificationSolution solution = new ClassificationSolution((ClassificationRule[])rules.Clone(), dataSets, maximumMutationStep);
            solution.Mutate(1);
            Tuple<double, double>[] valuePairs = solution.Rules[0].ValuePairs;
            foreach (Tuple<double, double> tuple in valuePairs)
            {
                if (tuple.Item1 < 0.4 || tuple.Item1 > 0.6 || tuple.Item2 < 0.4 || tuple.Item2 > 0.6)
                {
                    Assert.Fail("Tuple value was outside of expected range.");
                }
                else if (tuple.Item1 > tuple.Item2)
                {
                    Assert.Fail("Tuple was not in correct order.");
                }
            }
            int expectedOutput = 1;
            Assert.AreEqual(expectedOutput, solution.Rules[0].Output, "Rule's output bit was not flipped.");
        }

        [TestMethod]
        public void GetFitness_DataSetTest()
        {
            int length = 5;
            ClassificationRule rule = GetFilledClassificationRule(0, 0.2, 0, length);
            ClassificationRule[] rules = new ClassificationRule[] { rule };
            RealDataSet dataSet = GetFilledRealDataSet(0.7, 0, length);
            RealDataSet[] dataSets = new RealDataSet[] { dataSet };
            double maximumMutationStep = 0.5;
            ClassificationSolution solution = new ClassificationSolution(rules, dataSets, maximumMutationStep);
            RealDataSet testDataSet = GetFilledRealDataSet(0.1, 0, length);
            RealDataSet[] testDataSets = new RealDataSet[] { testDataSet };
            Assert.AreEqual(1, solution.GetFitness(testDataSets));
        }

        [TestMethod]
        public void ToStringTest()
        {
            int length = 5;
            ClassificationRule rule = GetFilledClassificationRule(0, 0.2, 0, length);
            ClassificationRule[] rules = new ClassificationRule[] { rule, rule };
            RealDataSet dataSet = GetFilledRealDataSet(0.7, 0, length);
            RealDataSet[] dataSets = new RealDataSet[] { dataSet };
            double maximumMutationStep = 0.5;
            ClassificationSolution solution = new ClassificationSolution(rules, dataSets, maximumMutationStep);
            string solutionString = solution.ToString();
            Assert.AreEqual("1: <0, 0.2> <0, 0.2> <0, 0.2> <0, 0.2> 0\n2: <0, 0.2> <0, 0.2> <0, 0.2> <0, 0.2> 0", solutionString);
        }

        #endregion

        private ClassificationRule GetFilledClassificationRule(double low, double high, int output, int length)
        {
            Tuple<double, double>[] valuePairs = Enumerable.Repeat(new Tuple<double, double>(low, high), length - 1).ToArray();
            return new ClassificationRule(valuePairs, output, 0, 1);
        }

        private RealDataSet GetFilledRealDataSet(double input, int output, int length)
        {
            double[] inputArray = Enumerable.Repeat(input, length - 1).ToArray();
            return new RealDataSet(inputArray, output);
        }

        #endregion

    }
}
