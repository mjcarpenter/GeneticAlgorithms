using GeneticAlgorithmLibrary.AlgorithmConfig;
using GeneticAlgorithmLibrary.Algorithms;
using GeneticAlgorithmLibrary.Solutions.DataClassification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTests.GeneticAlgorithmLibrary.Algorithms
{
    [TestClass]
    public class DataClassificationAlgorithmTests
    {

        #region Methods

        #region Tests

        [TestMethod]
        public void DataClassificationAlgorithm_TrainingDataOnlyTest()
        {
            double minimumInputValue = 0;
            double maximumInputValue = 1;
            double maximumMutationStep = 0.5;
            DataClassificationConfig config = new DataClassificationConfig(100, 50, 20, 10, minimumInputValue, maximumInputValue, maximumMutationStep);
            RealDataSet[] data = GetRealData(0.1, 0);
            DataClassificationAlgorithm algorithm = new DataClassificationAlgorithm(config, (RealDataSet[])data.Clone());
            Assert.AreEqual(maximumMutationStep, algorithm.MaximumMutationStep);
            CollectionAssert.AreEqual(data, algorithm.TrainingData);
            Assert.AreEqual(false, algorithm.CanTest);
        }

        [TestMethod]
        public void DataClassificationAlgorithm_TrainingAndTestingDataTest()
        {
            double minimumInputValue = 0;
            double maximumInputValue = 1;
            double maximumMutationStep = 0.5;
            DataClassificationConfig config = new DataClassificationConfig(100, 50, 20, 10, minimumInputValue, maximumInputValue, maximumMutationStep);
            RealDataSet[] trainingData = GetRealData(0.1, 0);
            RealDataSet[] testingData = GetRealData(0.9, 1);
            DataClassificationAlgorithm algorithm = new DataClassificationAlgorithm(config, (RealDataSet[])trainingData.Clone(), (RealDataSet[])testingData.Clone());
            Assert.AreEqual(maximumMutationStep, algorithm.MaximumMutationStep);
            CollectionAssert.AreEqual(trainingData, algorithm.TrainingData);
            CollectionAssert.AreEqual(testingData, algorithm.TestingData);
            Assert.AreEqual(true, algorithm.CanTest);
        }

        [TestMethod]
        public void RunAlgorithmTest()
        {
            DataClassificationConfig config = new DataClassificationConfig(100, 50, 20, 10, 0, 1, 0.5);
            RealDataSet[] data = GetRealData(0.1, 0);
            DataClassificationAlgorithm algorithm = new DataClassificationAlgorithm(config, data);
            algorithm.RunAlgorithm();
            Assert.AreNotEqual(null, algorithm.BestFitness);
            Assert.AreNotEqual(null, algorithm.AverageFitness);
        }

        [TestMethod]
        public void Test_NoTestingDataTest()
        {
            DataClassificationConfig config = new DataClassificationConfig(100, 50, 20, 10, 0, 1, 0.5);
            RealDataSet[] data = GetRealData(0.1, 0);
            DataClassificationAlgorithm algorithm = new DataClassificationAlgorithm(config, data);
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
            DataClassificationConfig config = new DataClassificationConfig(100, 50, 20, 10, 0, 1, 0.5);
            RealDataSet[] trainingData = GetRealData(0.1, 0);
            RealDataSet[] testingData = GetRealData(0.9, 1);
            DataClassificationAlgorithm algorithm = new DataClassificationAlgorithm(config, trainingData, testingData);
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

        private RealDataSet[] GetRealData(double input, int output)
        {
            RealDataSet[] data = new RealDataSet[1];
            double[] inputArray = Enumerable.Repeat(input, 4).ToArray();
            data[0] = new RealDataSet(inputArray, output);
            return data;
        }

        #endregion

    }
}
