using GeneticAlgorithmLibrary.Models;
using System;

namespace UnitTests.GeneticAlgorithmLibrary.TestImplemetations
{
    public class RuleClassificationAlgorithmTest : RuleClassificationAlgorithm
    {

        #region Constructor

        public RuleClassificationAlgorithmTest(RuleClassificationConfig config) : base(config) { }

        #endregion

        #region Properties

        public override bool CanTest
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Methods

        public override double Test()
        {
            throw new NotImplementedException();
        }

        internal override Solution GenerateRandomSolution()
        {
            return new SolutionTest();
        }

        #endregion

    }
}
