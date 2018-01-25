using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeneticAlgorithmLibrary.AlgorithmConfig;

namespace UnitTests.GeneticAlgorithmLibrary.AlgorithmConfig
{
    [TestClass]
    public class DataClassificationConfigTests
    {

        #region Tests

        [TestMethod]
        public void DataClassificationConfigTest()
        {
            double minimumInputValue = 0;
            double maximumInputValue = 1;
            double maximumMutationStep = 0.5;
            DataClassificationConfig config = new DataClassificationConfig(100, 50, 20, 10, minimumInputValue, maximumInputValue, maximumMutationStep);
            Assert.AreEqual(minimumInputValue, config.MinimumInputValue);
            Assert.AreEqual(maximumInputValue, config.MaximumInputValue);
            Assert.AreEqual(maximumMutationStep, config.MaximumMutationStep);
        }

        #endregion

    }

}
