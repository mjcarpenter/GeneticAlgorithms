using GeneticAlgorithmLibrary.Models;
using System;

namespace UnitTests.GeneticAlgorithmLibrary.TestImplemetations
{
    public class SolutionTest : Solution
    {

        #region Variables

        private int mutateCount = 0;

        private static Random RANDOM_GENERATOR = new Random();

        #endregion

        #region Methods

        public override double GetFitness()
        {
            return mutateCount * RANDOM_GENERATOR.Next(0, 10);
        }

        public override void Crossover(Solution solution, out Solution child1, out Solution child2)
        {
            child1 = new SolutionTest();
            child2 = new SolutionTest();
        }

        public override void Mutate(int mutationRate)
        {
            mutateCount++;
        }

        #endregion

    }
}
