using GeneticAlgorithmLibrary.Models;

namespace GeneticAlgorithmLibrary.AlgorithmConfig
{
    /// <summary>
    /// Holds the configuration for a data classification rule-based algorithm.
    /// </summary>
    public class DataClassificationConfig : RuleClassificationConfig
    {

        #region Constructor

        /// <summary>
        /// Creates a new DataClassificationConfig to hold the provided configuration options.
        /// </summary>
        /// <param name="generations">The number of generations to evolve solutions for.</param>
        /// <param name="populationSize">The candidate solution population size.</param>
        /// <param name="mutationRate">The value to be used when determining whether to mutate a solution or not. (1/MutationRate probability to mutate).</param>
        /// <param name="ruleCount">The number of rules that each solution has.</param>
        /// <param name="minimumInputValue">The minimum value that an input value can be.</param>
        /// <param name="maximumInputValue">The maximum value that an input value can be.</param>
        /// <param name="maximumMutationStep">The maximum jump that the rule bounds can make in a mutation.</param>
        public DataClassificationConfig(int generations, int populationSize, int mutationRate, int ruleCount, double minimumInputValue, double maximumInputValue, double maximumMutationStep) : base(generations, populationSize, mutationRate, ruleCount)
        {
            MinimumInputValue = minimumInputValue;
            MaximumInputValue = maximumInputValue;
            MaximumMutationStep = maximumMutationStep;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The minimum value that an input value can be.
        /// </summary>
        public double MinimumInputValue { get; private set; }

        /// <summary>
        /// The maximum value that an input value can be.
        /// </summary>
        public double MaximumInputValue { get; private set; }

        /// <summary>
        /// The maximum jump that the rule bounds can make in a mutation.
        /// </summary>
        public double MaximumMutationStep { get; private set; }

        #endregion

    }
}
