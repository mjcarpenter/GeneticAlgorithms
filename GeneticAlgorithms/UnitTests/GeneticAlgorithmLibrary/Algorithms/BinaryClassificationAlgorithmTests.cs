using GeneticAlgorithmLibrary.AlgorithmConfig;
using GeneticAlgorithmLibrary.Algorithms;
using GeneticAlgorithmLibrary.Solutions.BinaryRuleClassification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTests.GeneticAlgorithmLibrary.Algorithms
{
    [TestClass]
    public class BinaryClassificationAlgorithmTests
    {

        #region Methods

        #region Tests

        [TestMethod]
        public void BinaryClassificationAlgorithm_TrainingDataOnlyTest()
        {
            bool useWildcard = true;
            BinaryClassificationConfig config = new BinaryClassificationConfig(100, 50, 20, 10, useWildcard);
            BinaryDataSet[] data = GetBinaryData(0);
            BinaryClassificationAlgorithm algorithm = new BinaryClassificationAlgorithm(config, (BinaryDataSet[])data.Clone());
            Assert.AreEqual(useWildcard, algorithm.UseWildcard);
            CollectionAssert.AreEqual(data, algorithm.TrainingData);
            Assert.AreEqual(false, algorithm.CanTest);
        }

        [TestMethod]
        public void BinaryClassificationAlgorithm_TrainingAndTestingDataTest()
        {
            bool useWildcard = true;
            BinaryClassificationConfig config = new BinaryClassificationConfig(100, 50, 20, 10, useWildcard);
            BinaryDataSet[] trainingData = GetBinaryData(0);
            BinaryDataSet[] testingData = GetBinaryData(1);
            BinaryClassificationAlgorithm algorithm = new BinaryClassificationAlgorithm(config, (BinaryDataSet[])trainingData.Clone(), (BinaryDataSet[])testingData.Clone());
            Assert.AreEqual(useWildcard, algorithm.UseWildcard);
            CollectionAssert.AreEqual(trainingData, algorithm.TrainingData);
            CollectionAssert.AreEqual(testingData, algorithm.TestingData);
            Assert.AreEqual(true, algorithm.CanTest);
        }

        [TestMethod]
        public void RunAlgorithmTest()
        {
            BinaryClassificationConfig config = new BinaryClassificationConfig(100, 50, 20, 10, true);
            BinaryDataSet[] data = GetBinaryData(0);
            BinaryClassificationAlgorithm algorithm = new BinaryClassificationAlgorithm(config, data);
            algorithm.RunAlgorithm();
            Assert.AreNotEqual(null, algorithm.BestFitness);
            Assert.AreNotEqual(null, algorithm.AverageFitness);
        }

        [TestMethod]
        public void Test_NoTestingDataTest()
        {
            BinaryClassificationConfig config = new BinaryClassificationConfig(100, 50, 20, 10, true);
            BinaryDataSet[] data = GetBinaryData(0);
            BinaryClassificationAlgorithm algorithm = new BinaryClassificationAlgorithm(config, data);
            algorithm.RunAlgorithm();
            bool exceptionCaught = false;
            try
            {
                algorithm.Test();
            }
            catch (NotSupportedException)
            {
                exceptionCaught = true;
            }
            Assert.IsTrue(exceptionCaught);
        }

        [TestMethod]
        public void Test_TestingDataTest()
        {
            BinaryClassificationConfig config = new BinaryClassificationConfig(100, 50, 20, 10, true);
            BinaryDataSet[] trainingData = GetBinaryData(0);
            BinaryDataSet[] testingData = GetBinaryData(1);
            BinaryClassificationAlgorithm algorithm = new BinaryClassificationAlgorithm(config, trainingData, testingData);
            algorithm.RunAlgorithm();
            bool exceptionCaught = false;
            try
            {
                double result = algorithm.Test();
                bool inPercentageRange = result >= 0 && result <= 100;
                Assert.IsTrue(inPercentageRange);
            }
            catch (NotSupportedException)
            {
                exceptionCaught = true;
            }
            Assert.IsFalse(exceptionCaught);
        }

        #endregion

        private BinaryDataSet[] GetBinaryData(int value)
        {
            BinaryDataSet[] data = new BinaryDataSet[1];
            int[] input = Enumerable.Repeat(value, 4).ToArray();
            data[0] = new BinaryDataSet(input, value);
            return data;
        }

        #endregion

    }
}
