using GeneticAlgorithmLibrary.Models;
using System;

namespace UnitTests.GeneticAlgorithmLibrary.TestImplemetations
{
    public class GeneticAlgorithmTestable : GeneticAlgorithm
    {

        #region Constructor

        public GeneticAlgorithmTestable(GeneticAlgorithmConfig config) : base(config) { }

        #endregion

        #region Properties

        public override bool CanTest
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Methods

        public override double Test()
        {
            return new Random().NextDouble();
        }

        internal override Solution GenerateRandomSolution()
        {
            return new SolutionTest();
        }

        #endregion

    }
}
