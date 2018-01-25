using GeneticAlgorithmLibrary.Models;

namespace UnitTests.GeneticAlgorithmLibrary.TestImplemetations
{
    public class RuleClassificationConfigTest : RuleClassificationConfig
    {

        #region Constructor

        public RuleClassificationConfigTest(int generations, int populationSize, int mutationRate, int ruleCount) : base(generations, populationSize, mutationRate, ruleCount) { }

        #endregion

    }
}
