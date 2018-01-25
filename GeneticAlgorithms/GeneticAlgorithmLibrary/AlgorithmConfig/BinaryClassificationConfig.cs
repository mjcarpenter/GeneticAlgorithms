using GeneticAlgorithmLibrary.Models;

namespace GeneticAlgorithmLibrary.AlgorithmConfig
{
    /// <summary>
    /// Holds the configuration for a binary classification rule-based algorithm.
    /// </summary>
    public class BinaryClassificationConfig : RuleClassificationConfig
    {

        #region Constructor

        /// <summary>
        /// Creates a new BinaryClassificationConfig to hold the provided configuration options.
        /// </summary>
        /// <param name="generations">The number of generations to evolve solutions for.</param>
        /// <param name="populationSize">The candidate solution population size.</param>
        /// <param name="mutationRate">The value to be used when determining whether to mutate a solution or not. (1/MutationRate probability to mutate).</param>
        /// <param name="ruleCount">The number of rules that each solution has.</param>
        /// <param name="useWildcard">Whether the wildcard operator should be used.</param>
        public BinaryClassificationConfig(int generations, int populationSize, int mutationRate, int ruleCount, bool useWildcard) : base(generations, populationSize, mutationRate, ruleCount)
        {
            UseWildcard = useWildcard;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Whether the wildcard operator should be used.
        /// </summary>
        public bool UseWildcard { get; private set; }

        #endregion

    }
}
