using GeneticAlgorithmLibrary.Solutions.DataClassification;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.GeneticAlgorithmLibrary.Solutions.DataClassification
{
    [TestClass()]
    public class RealDataSetTests
    {

        #region Tests

        [TestMethod()]
        public void RealDataSetTest()
        {
            double[] input = new double[] { 0.1, 1, 0.5, 1 };
            int output = 0;
            RealDataSet dataSet = new RealDataSet(input, output);
            Assert.AreEqual(input, dataSet.Input);
            Assert.AreEqual(output, dataSet.Output);
        }

        #endregion

    }
}
