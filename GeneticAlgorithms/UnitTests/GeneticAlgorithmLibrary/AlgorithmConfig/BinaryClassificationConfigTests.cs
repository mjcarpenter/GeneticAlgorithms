using GeneticAlgorithmLibrary.AlgorithmConfig;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.GeneticAlgorithmLibrary.AlgorithmConfig
{
    [TestClass]
    public class BinaryClassificationConfigTests
    {

        #region Tests

        [TestMethod]
        public void BinaryClassificationConfigTest()
        {
            bool useWildcard = true;
            BinaryClassificationConfig config = new BinaryClassificationConfig(100, 50, 20, 10, useWildcard);
            Assert.AreEqual(useWildcard, config.UseWildcard);
        }

        #endregion

    }
}
