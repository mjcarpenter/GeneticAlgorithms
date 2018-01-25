using System;
using GeneticAlgorithmLibrary.Models;

namespace GeneticAlgorithmLibrary.Solutions.BinaryRuleClassification
{
    /// <summary>
    /// Represents a candidate solution to the binary rule classification problem.
    /// </summary>
    internal class BinaryRuleSolution : Solution
    {

        #region Variables

        /// <summary>
        /// The data to train the solution with.
        /// </summary>
        private BinaryDataSet[] m_trainingData;

        /// <summary>
        /// Whether the wildcard operator should be used.
        /// </summary>
        private bool m_useWildcard;

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
        /// Creates a new BinaryRuleSolution with the provided rules.
        /// </summary>
        /// <param name="rules">The rules to use for this solution.</param>
        /// <param name="data">The data to train the solution against.</param>
        /// <param name="useWildcard">Whether the wildcard operator should be used.</param>
        public BinaryRuleSolution(BinaryRule[] rules, BinaryDataSet[] data, bool useWildcard)
        {
            Rules = rules;
            m_trainingData = data;
            m_useWildcard = useWildcard;
            m_shouldRecalculateFitness = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Backing variable for the Rules property.
        /// </summary>
        private BinaryRule[] _rules;

        /// <summary>
        /// The rules to use for this solution.
        /// </summary>
        public BinaryRule[] Rules
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
            double fitness = GetFitnessForDataSet(m_trainingData);
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
            BinaryRuleSolution binarySolution = (BinaryRuleSolution)solution;
            m_shouldRecalculateFitness = true;
            char[] parent1Bits = GetBitString();
            char[] parent2Bits = binarySolution.GetBitString();
            int bitStringLength = parent1Bits.Length;
            int crossover = RANDOM_GENERATOR.Next(1, bitStringLength - 1);
            char[] child1Bits = new char[bitStringLength];
            char[] child2Bits = new char[bitStringLength];
            Array.Copy(parent1Bits, 0, child1Bits, 0, crossover);
            Array.Copy(parent2Bits, 0, child2Bits, 0, crossover);
            Array.Copy(parent2Bits, crossover, child1Bits, crossover, bitStringLength - crossover);
            Array.Copy(parent1Bits, crossover, child2Bits, crossover, bitStringLength - crossover);
            int ruleLength = bitStringLength / Rules.Length;
            BinaryRule[] child1Rules = BinaryRule.GetRules(child1Bits, ruleLength, m_useWildcard);
            BinaryRule[] child2Rules = BinaryRule.GetRules(child2Bits, ruleLength, m_useWildcard);
            child1 = new BinaryRuleSolution(child1Rules, m_trainingData, m_useWildcard);
            child2 = new BinaryRuleSolution(child2Rules, m_trainingData, m_useWildcard);
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
                Rules[i].Mutate(mutationRate);
            }
        }

        /// <summary>
        /// Returns the fitness of the solution when run against the provided data set.
        /// </summary>
        /// <param name="testData">The data to test the solution with.</param>
        /// <returns>The fitness when run against the data, the percentage of the data correctly identified.</returns>
        public double GetFitness(BinaryDataSet[] testData)
        {
            double fitness = GetFitnessForDataSet(testData);
            return fitness;
        }

        /// <summary>
        /// Returns the bit string representation of the solution.
        /// </summary>
        /// <returns>The bit string representation of the solution as an array of chars.</returns>
        private char[] GetBitString()
        {
            int length = (Rules.Length * (Rules[0].Input.Length + 1));
            char[] bits = new char[length];
            int bitIndex = 0;
            for (int i = 0; i < Rules.Length; i++)
            {
                for (int j = 0; j < Rules[i].Input.Length; j++)
                {
                    bits[bitIndex] = Rules[i].Input[j];
                    bitIndex++;
                }
                bits[bitIndex] = Rules[i].Output;
                bitIndex++;
            }
            return bits;
        }

        /// <summary>
        /// Calculates the fitness of the solution with the provided data.
        /// </summary>
        /// <param name="data">The data to use to calculate the fitness.</param>
        /// <returns>The fitness of the solution with the provided data.</returns>
        private double GetFitnessForDataSet(BinaryDataSet[] data)
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
                        break;
                    }
                }
            }
            return fitness;
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
