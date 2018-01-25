using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeneticAlgorithmLibrary.Solutions.BinaryRuleClassification;

namespace UnitTests.GeneticAlgorithmLibrary.Solutions.BinaryRuleClassification
{
    [TestClass()]
    public class BinaryDataSetTests
    {

        #region Tests

        [TestMethod()]
        public void BinaryDataSetTest()
        {
            int[] input = new int[] { 0, 1, 0, 1 };
            int output = 0;
            BinaryDataSet dataSet = new BinaryDataSet(input, output);
            Assert.AreEqual(input, dataSet.Input);
            Assert.AreEqual(output, dataSet.Output);
        }

        #endregion

    }
}