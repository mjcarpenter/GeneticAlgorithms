using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeneticAlgorithmLibrary.Models;
using UnitTests.GeneticAlgorithmLibrary.TestImplemetations;

namespace UnitTests.GeneticAlgorithmLibrary.Models
{

    [TestClass]
    public class GeneticAlgorithmTests
    {

        #region Tests

        [TestMethod]
        public void GeneticAlgorithmTest()
        {
            int generations = 100;
            int populationSize = 50;
            int mutationRate = 20;
            GeneticAlgorithmConfig config = new GeneticAlgorithmConfigTest(generations, populationSize, mutationRate);
            GeneticAlgorithm algorithm = new GeneticAlgorithmTest(config);
            Assert.AreEqual(generations, algorithm.Generations);
            Assert.AreEqual(populationSize, algorithm.PopulationSize);
            Assert.AreEqual(mutationRate, algorithm.MutationRate);
        }

        [TestMethod]
        public void RunAlgorithmTest()
        {
            int generations = 1;
            int populationSize = 10;
            int mutationRate = 20;
            GeneticAlgorithmConfig config = new GeneticAlgorithmConfigTest(generations, populationSize, mutationRate);
            GeneticAlgorithm algorithm = new GeneticAlgorithmTest(config);
            algorithm.RunAlgorithm();
            Assert.AreNotEqual(null, algorithm.BestFitness);
            Assert.AreNotEqual(null, algorithm.AverageFitness);
        }

        [TestMethod]
        public void RunAlgorithm_OddPopulationTest()
        {
            int generations = 1;
            int populationSize = 11;
            int mutationRate = 20;
            GeneticAlgorithmConfig config = new GeneticAlgorithmConfigTest(generations, populationSize, mutationRate);
            GeneticAlgorithm algorithm = new GeneticAlgorithmTest(config);
            algorithm.RunAlgorithm();
            Assert.AreNotEqual(null, algorithm.BestFitness);
            Assert.AreNotEqual(null, algorithm.AverageFitness);
        }

        #endregion

    }
}
