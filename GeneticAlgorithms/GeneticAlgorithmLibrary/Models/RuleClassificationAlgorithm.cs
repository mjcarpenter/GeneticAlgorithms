namespace GeneticAlgorithmLibrary.Models
{
    /// <summary>
    /// Provides the standard shell and process of a rule-based genetic algorithm.
    /// </summary>
    public abstract class RuleClassificationAlgorithm : GeneticAlgorithm
    {

        #region Constructor

        /// <summary>
        /// Creates a new RuleClassificationAlgorithm with the specified configuration.
        /// </summary>
        /// <param name="config">The configuration for the algorithm.</param>
        public RuleClassificationAlgorithm(RuleClassificationConfig config) : base(config)
        {
            RuleCount = config.RuleCount;
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
