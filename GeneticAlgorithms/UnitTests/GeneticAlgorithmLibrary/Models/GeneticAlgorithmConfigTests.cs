using GeneticAlgorithmLibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.GeneticAlgorithmLibrary.TestImplemetations;

namespace UnitTests.GeneticAlgorithmLibrary.Models
{
    [TestClass]
    public class GeneticAlgorithmConfigTests
    {

        #region Tests

        [TestMethod]
        public void GeneticAlgorithmConfigTest()
        {
            int generations = 100;
            int populationSize = 50;
            int mutationRate = 20;
            GeneticAlgorithmConfig config = new GeneticAlgorithmConfigTest(generations, populationSize, mutationRate);
            Assert.AreEqual(generations, config.Generations);
            Assert.AreEqual(populationSize, config.PopulationSize);
            Assert.AreEqual(mutationRate, config.MutationRate);
        }

        #endregion

    }
}
