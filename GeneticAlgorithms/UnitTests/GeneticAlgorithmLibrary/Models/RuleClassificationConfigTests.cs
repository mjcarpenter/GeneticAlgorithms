using GeneticAlgorithmLibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.GeneticAlgorithmLibrary.TestImplemetations;

namespace UnitTests.GeneticAlgorithmLibrary.Models
{
    [TestClass]
    public class RuleClassificationConfigTests
    {

        #region Tests

        [TestMethod]
        public void RuleClassificationConfigTest()
        {
            int ruleCount = 10;
            RuleClassificationConfig config = new RuleClassificationConfigTest(100, 50, 20, ruleCount);
            Assert.AreEqual(ruleCount, config.RuleCount);
        }

        #endregion

    }
}
