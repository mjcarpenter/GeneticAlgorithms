using GeneticAlgorithmLibrary.Models;
using System;

namespace GeneticAlgorithmLibrary.Solutions.DataClassification
{
    /// <summary>
    /// Represents a candidate solution to the data classification problem.
    /// </summary>
    internal class ClassificationSolution : Solution
    {

        #region Variables

        /// <summary>
        /// The data to train the solution with.
        /// </summary>
        private RealDataSet[] m_trainingData;

        /// <summary>
        /// The maximum jump that the rule bounds can make in a mutation.
        /// </summary>
        private double m_maximumMutationStep;

        /// <summary>
        /// The current fitness of this solution.
        /// </summary>
        private double m_currentFitness;

        /// <summary>
        /// Whether the current fitness of this solution should be recalculated.
        /// </summary>
        private bool m_shouldRecalculateFitness;

        /// <summary>
        /// Random number generator used for crossover and mutation.
        /// </summary>
        private static Random RANDOM_GENERATOR = new Random();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new ClassificationSolution with the provided rules.
        /// </summary>
        /// <param name="rules">The rules to use for this solution.</param>
        /// <param name="data">The data to train the solution against.</param>
        /// <param name="maxMutationStep">The maximum jump that the rule bounds can make in a mutation.</param>
        public ClassificationSolution(ClassificationRule[] rules, RealDataSet[] data, double maximumMutationStep)
        {
            Rules = rules;
            m_trainingData = data;
            m_maximumMutationStep = maximumMutationStep;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Backing variable for the Rules property.
        /// </summary>
        private ClassificationRule[] _rules;

        /// <summary>
        /// The rules to use for this solution.
        /// </summary>
        public ClassificationRule[] Rules
        {
            get
            {
                return _rules;
            }
            set
            {
                _rules = value;
                m_shouldRecalculateFitness = true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the fitness of the solution.
        /// </summary>
        /// <returns>The fitness of the solution.</returns>
        public override double GetFitness()
        {
            if (!m_shouldRecalculateFitness)
            {
                return m_currentFitness;
            }
            double fitness = GetFitnessForDataSet(m_trainingData, true);
            m_currentFitness = fitness;
            m_shouldRecalculateFitness = false;
            return fitness;
        }

        /// <summary>
        /// Crosses this solution over with the provided solution, setting the two out parameters to the new children.
        /// </summary>
        /// <param name="solution">The solution to cross this over with.</param>
        /// <param name="child1">The first child produced by this operation.</param>
        /// <param name="child2">The second child produced by this operation.</param>
        public override void Crossover(Solution solution, out Solution child1, out Solution child2)
        {
            ClassificationSolution ruleSolution = (ClassificationSolution)solution;
            ClassificationRule[] rules1 = new ClassificationRule[Rules.Length];
            ClassificationRule[] rules2 = new ClassificationRule[Rules.Length];
            for (int i = 0; i < Rules.Length; i++)
            {
                ClassificationRule rule1;
                ClassificationRule rule2;
                Rules[i].Crossover(ruleSolution.Rules[i], out rule1, out rule2);
                rules1[i] = rule1;
                rules2[i] = rule2;
            }
            child1 = new ClassificationSolution(rules1, m_trainingData, m_maximumMutationStep);
            child2 = new ClassificationSolution(rules2, m_trainingData, m_maximumMutationStep);
        }

        /// <summary>
        /// Randomly determines whether to mutate this solution.
        /// </summary>
        /// <param name="mutationRate">The value to be used when determining whether to mutate a solution or not. (1/MutationRate probability to mutate).</param>
        public override void Mutate(int mutationRate)
        {
            m_shouldRecalculateFitness = true;
            for (int i = 0; i < Rules.Length; i++)
            {
                Rules[i].Mutate(mutationRate, m_maximumMutationStep);
            }
        }

        /// <summary>
        /// Returns the fitness of the solution when run against the provided data set.
        /// </summary>
        /// <param name="testData">The data to test the solution with.</param>
        /// <returns>The fitness when run against the data, the percentage of the data correctly identified.</returns>
        public double GetFitness(RealDataSet[] testData)
        {
            return GetFitnessForDataSet(testData, false);
        }

        /// <summary>
        /// Calculates the fitness of the solution with the provided data.
        /// </summary>
        /// <param name="data">The data to use to calculate the fitness.</param>
        /// <param name="reduceForIncorrectOutput">Whether any incorrectly identified sets of data should be detrimental to the fitness value.</param>
        /// <returns>The fitness of the solution with the provided data.</returns>
        private double GetFitnessForDataSet(RealDataSet[] data, bool reduceForIncorrectOutput)
        {
            double fitness = 0;
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < Rules.Length; j++)
                {
                    if (Rules[j].InputMatches(data[i]))
                    {
                        if (Rules[j].OutputMatches(data[i]))
                        {
                            fitness++;
                        }
                        else if (reduceForIncorrectOutput)
                        {
                            fitness--;
                        }
                        break;
                    }
                }
            }
            return Math.Max(fitness, 0);
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < Rules.Length; i++)
            {
                result += (i + 1) + ": " + Rules[i].ToString();
                if (i != Rules.Length - 1)
                {
                    result += "\n";
                }
            }
            return result;
        }

        #endregion

    }
}
