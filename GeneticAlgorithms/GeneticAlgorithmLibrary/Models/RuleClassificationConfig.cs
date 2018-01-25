namespace GeneticAlgorithmLibrary.Models
{
    /// <summary>
    /// Holds the configuration for a rule-based genetic algorithm.
    /// </summary>
    public abstract class RuleClassificationConfig : GeneticAlgorithmConfig
    {

        #region Constructor

        /// <summary>
        /// Creates a new RuleClassificationConfig to hold the provided configuration options.
        /// </summary>
        /// <param name="generations">The number of generations to evolve solutions for.</param>
        /// <param name="populationSize">The candidate solution population size.</param>
        /// <param name="mutationRate">The value to be used when determining whether to mutate a solution or not. (1/MutationRate probability to mutate).</param>
        /// <param name="ruleCount">The number of rules that each solution has.</param>
        public RuleClassificationConfig(int generations, int populationSize, int mutationRate, int ruleCount) : base(generations, populationSize, mutationRate)
        {
            RuleCount = ruleCount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The number of rules that each solution has.
        /// </summary>
        public int RuleCount { get; private set; }

        #endregion

    }
}
