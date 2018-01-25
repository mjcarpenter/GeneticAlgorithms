using GeneticAlgorithmLibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.GeneticAlgorithmLibrary.TestImplemetations;

namespace UnitTests.GeneticAlgorithmLibrary.Models
{
    [TestClass]
    public class RuleClassificationAlgorithmTests
    {

        #region Tests

        [TestMethod]
        public void RuleClassificationAlgorithmTest()
        {
            int ruleCount = 10;
            RuleClassificationConfig config = new RuleClassificationConfigTest(100, 50, 20, ruleCount);
            RuleClassificationAlgorithm algorithm = new RuleClassificationAlgorithmTest(config);
            Assert.AreEqual(ruleCount, algorithm.RuleCount);
        }

        #endregion

    }
}
