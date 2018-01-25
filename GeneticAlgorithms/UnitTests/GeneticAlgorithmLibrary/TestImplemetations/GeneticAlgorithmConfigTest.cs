using GeneticAlgorithmLibrary.Models;

namespace UnitTests.GeneticAlgorithmLibrary.TestImplemetations
{
    public class GeneticAlgorithmConfigTest : GeneticAlgorithmConfig
    {

        #region Constructor

        public GeneticAlgorithmConfigTest(int generations, int populationSize, int mutationRate) : base(generations, populationSize, mutationRate) { }

        #endregion

    }
}
