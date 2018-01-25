using GeneticAlgorithmLibrary.Helpers;
using GeneticAlgorithmLibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.GeneticAlgorithmLibrary.TestImplemetations;

namespace UnitTests.GeneticAlgorithmLibrary.Helpers
{
    [TestClass]
    public class AlgorithmRunnerTests
    {

        #region Tests

        [TestMethod]
        public void AlgorithmRunnerTest()
        {
            GeneticAlgorithmConfig config = new GeneticAlgorithmConfigTest(100, 50, 20);
            GeneticAlgorithm algorithm = new GeneticAlgorithmTest(config);
            int runs = 10;
            AlgorithmRunner runner = new AlgorithmRunner(algorithm, runs);
            Assert.AreEqual(algorithm, runner.Algorithm);
            Assert.AreEqual(runs, runner.Runs);
        }

        [TestMethod]
        public void StartTest()
        {
            GeneticAlgorithmConfig config = new GeneticAlgorithmConfigTest(10, 10, 20);
            GeneticAlgorithm algorithm = new GeneticAlgorithmTest(config);
            int runs = 1;
            AlgorithmRunner runner = new AlgorithmRunner(algorithm, runs);
            runner.Start(true, true);
            Assert.AreNotEqual(null, runner.AverageFitnessAverage);
            Assert.AreNotEqual(null, runner.BestFitnessAverage);
        }

        [TestMethod]
        public void Start_TestableAlgorithmTest()
        {
            GeneticAlgorithmConfig config = new GeneticAlgorithmConfigTest(10, 10, 20);
            GeneticAlgorithm algorithm = new GeneticAlgorithmTestable(config);
            int runs = 1;
            AlgorithmRunner runner = new AlgorithmRunner(algorithm, runs);
            runner.Start(true, true);
            Assert.AreNotEqual(null, runner.AverageFitnessAverage);
            Assert.AreNotEqual(null, runner.BestFitnessAverage);
            Assert.AreNotEqual(null, runner.AverageTestResults);
        }

        #endregion

    }
}
