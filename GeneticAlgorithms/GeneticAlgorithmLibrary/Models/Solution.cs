
namespace GeneticAlgorithmLibrary.Models
{
    /// <summary>
    /// Provides the standard structure of a candidate solution.
    /// </summary>
    public abstract class Solution
    {

        #region Methods

        /// <summary>
        /// Gets the fitness of the solution.
        /// </summary>
        /// <returns>The fitness of the solution.</returns>
        public abstract double GetFitness();

        /// <summary>
        /// Crosses this solution over with the provided solution, setting the two out parameters to the new children.
        /// </summary>
        /// <param name="solution">The solution to cross this over with.</param>
        /// <param name="child1">The first child produced by this operation.</param>
        /// <param name="child2">The second child produced by this operation.</param>
        public abstract void Crossover(Solution solution, out Solution child1, out Solution child2);

        /// <summary>
        /// Randomly determines whether to mutate this solution.
        /// </summary>
        /// <param name="mutationRate">The value to be used when determining whether to mutate a solution or not. (1/MutationRate probability to mutate).</param>
        public abstract void Mutate(int mutationRate);

        #endregion

    }
}
