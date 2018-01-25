namespace GeneticAlgorithmLibrary.Models
{
    /// <summary>
    /// Holds the configuration for a standard genetic algorithm.
    /// </summary>
    public abstract class GeneticAlgorithmConfig
    {

        #region Constructor

        /// <summary>
        /// Creates a new GeneticAlgorithmConfig to hold the provided configuration options.
        /// </summary>
        /// <param name="generations">The number of generations to evolve solutions for.</param>
        /// <param name="populationSize">The candidate solution population size.</param>
        /// <param name="mutationRate">The value to be used when determining whether to mutate a solution or not. (1/MutationRate probability to mutate).</param>
        public GeneticAlgorithmConfig(int generations, int populationSize, int mutationRate)
        {
            Generations = generations;
            PopulationSize = populationSize;
            MutationRate = mutationRate;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The number of generations to evolve solutions for.
        /// </summary>
        public int Generations { get; private set; }

        /// <summary>
        /// The candidate solution population size.
        /// </summary>
        public int PopulationSize { get; private set; }

        /// <summary>
        /// The value to be used when determining whether to mutate a solution or not. (1/MutationRate probability to mutate).
        /// </summary>
        public int MutationRate { get; private set; }

        #endregion

    }
}
