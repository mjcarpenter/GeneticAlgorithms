using GeneticAlgorithmLibrary.AlgorithmConfig;
using GeneticAlgorithmLibrary.Models;
using GeneticAlgorithmLibrary.Solutions.BinaryRuleClassification;
using System;

namespace GeneticAlgorithmLibrary.Algorithms
{
    /// <summary>
    /// An algorithm that can be used to classify sets of binary data.
    /// </summary>
    public class BinaryClassificationAlgorithm : RuleClassificationAlgorithm
    {

        #region Variables

        /// <summary>
        /// The length of each rule.
        /// </summary>
        private int m_ruleLength;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new BinaryClassificationAlgorithm with the provided configuration and training data.
        /// </summary>
        /// <param name="config">The configuration for the algorithm.</param>
        /// <param name="data">The binary data to train the algorithm with.</param>
        public BinaryClassificationAlgorithm(BinaryClassificationConfig config, BinaryDataSet[] data) : base(config)
        {
            UseWildcard = config.UseWildcard;
            TrainingData = data;
            m_ruleLength = data[0].Input.Length + 1;
        }

        /// <summary>
        /// Creates a new BinaryClassificationAlgorithm with the provided configuration and training and testing data.
        /// </summary>
        /// <param name="config">The configuration for the algorithm.</param>
        /// <param name="trainingData">The binary data to train the algorithm with.</param>
        /// <param name="testingData">The binary data to test the algorithm with.</param>
        public BinaryClassificationAlgorithm(BinaryClassificationConfig config, BinaryDataSet[] trainingData, BinaryDataSet[] testingData) : this(config, trainingData)
        {
            TestingData = testingData;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Whether the wildcard operator should be used.
        /// </summary>
        public bool UseWildcard { get; private set; }

        /// <summary>
        /// The binary data to train the algorithm with.
        /// </summary>
        public BinaryDataSet[] TrainingData { get; private set; }

        /// <summary>
        /// The binary data to test the algorithm with.
        /// </summary>
        public BinaryDataSet[] TestingData { get; private set; }

        /// <summary>
        /// Whether the algorithm can be tested using test data or not.
        /// </summary>
        public override bool CanTest
        {
            get
            {
                return TestingData != null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tests the algorithm if testing data has been provided.
        /// </summary>
        /// <returns>The accuracy of the algorithm against the test data.</returns>
        public override double Test()
        {
            if (TestingData == null)
            {
                throw new NotSupportedException();
            }
            BinaryRuleSolution bestSolution = (BinaryRuleSolution)GetBestSolution(Population);
            double fitness = bestSolution.GetFitness(TestingData);
            return (fitness / TestingData.Length) * 100; ;
        }

        /// <summary>
        /// Generates a random solution for the problem.
        /// </summary>
        /// <returns>A randomly generated solution.</returns>
        internal override Solution GenerateRandomSolution()
        {
            BinaryRule[] rules = new BinaryRule[RuleCount];
            for (int i = 0; i < RuleCount; i++)
            {
                rules[i] = BinaryRule.GenerateRule(m_ruleLength, UseWildcard);
            }
            return new BinaryRuleSolution(rules, TrainingData, UseWildcard);
        }

        #endregion

    }
}
