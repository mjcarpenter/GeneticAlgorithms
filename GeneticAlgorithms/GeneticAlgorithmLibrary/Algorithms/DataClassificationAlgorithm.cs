using GeneticAlgorithmLibrary.AlgorithmConfig;
using GeneticAlgorithmLibrary.Models;
using GeneticAlgorithmLibrary.Solutions.DataClassification;
using System;

namespace GeneticAlgorithmLibrary.Algorithms
{
    /// <summary>
    /// An algorithm that can be used to classify sets of real-world data with a bit.
    /// </summary>
    public class DataClassificationAlgorithm : RuleClassificationAlgorithm
    {

        #region Variables

        /// <summary>
        /// The length of each rule.
        /// </summary>
        private int m_ruleLength;

        /// <summary>
        /// The minimum value that an input value can be.
        /// </summary>
        private double m_minimumInputValue;

        /// <summary>
        /// The maximum value that an input value can be.
        /// </summary>
        private double m_maximumInputValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new DataClassificationAlgorithm with the provided configuration and training data.
        /// </summary>
        /// <param name="config">The configuration for the algorithm.</param>
        /// <param name="data">The real-world data to train the algorithm with.</param>
        public DataClassificationAlgorithm(DataClassificationConfig config, RealDataSet[] data) : base(config)
        {
            MaximumMutationStep = config.MaximumMutationStep;
            if (data != null)
            {
                TrainingData = data;
                m_ruleLength = data[0].Input.Length + 1;
            }
            m_minimumInputValue = config.MinimumInputValue;
            m_maximumInputValue = config.MaximumInputValue;
        }

        /// <summary>
        /// Creates a new DataClassificationAlgorithm with the provided configuration and training and testing data.
        /// </summary>
        /// <param name="config">The configuration for the algorithm.</param>
        /// <param name="trainingData">The real-world data to train the algorithm with.</param>
        /// <param name="testingData">The real-world data to test the algorithm with.</param>
        public DataClassificationAlgorithm(DataClassificationConfig config, RealDataSet[] trainingData, RealDataSet[] testingData) : this(config, trainingData)
        {
            TestingData = testingData;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The maximum jump that the rule bounds can make in a mutation.
        /// </summary>
        public double MaximumMutationStep { get; private set; }

        /// <summary>
        /// The real-world data to train the algorithm with.
        /// </summary>
        public RealDataSet[] TrainingData { get; private set; }

        /// <summary>
        /// The real-world data to test the algorithm with.
        /// </summary>
        public RealDataSet[] TestingData { get; private set; }

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
            ClassificationSolution bestSolution = (ClassificationSolution)GetBestSolution(Population);
            double fitness = bestSolution.GetFitness(TestingData);
            return (fitness / TestingData.Length) * 100; ;
        }

        /// <summary>
        /// Generates a random solution for the problem.
        /// </summary>
        /// <returns>A randomly generated solution.</returns>
        internal override Solution GenerateRandomSolution()
        {
            ClassificationRule[] rules = new ClassificationRule[RuleCount];
            for (int i = 0; i < RuleCount; i++)
            {
                rules[i] = ClassificationRule.GenerateRule(m_ruleLength, m_minimumInputValue, m_maximumInputValue);
            }
            return new ClassificationSolution(rules, TrainingData, MaximumMutationStep);
        }

        #endregion

    }
}
