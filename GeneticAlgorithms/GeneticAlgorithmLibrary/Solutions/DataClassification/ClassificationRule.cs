using System;

namespace GeneticAlgorithmLibrary.Solutions.DataClassification
{
    /// <summary>
    /// A rule used to match against real-world data sets to indicate how the data should be classified.
    /// </summary>
    internal class ClassificationRule
    {

        #region Variables

        /// <summary>
        /// Random number generator used for crossover and mutation.
        /// </summary>
        private static Random RANDOM_GENERATOR = new Random();

        private double m_minimumValue;
        private double m_maximumValue;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new ClassificationRule with the specified value pairs and output.
        /// </summary>
        /// <param name="valuePairs">The set of value pairs that will be used to match the inputs.</param>
        /// <param name="output">The bit that will be matched against the output of a data set.</param>
        public ClassificationRule(Tuple<double, double>[] valuePairs, int output, double minimumValue, double maximumValue)
        {
            ValuePairs = valuePairs;
            Output = output;
            m_minimumValue = minimumValue;
            m_maximumValue = maximumValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The set of value pairs that will be used to match the inputs.
        /// </summary>
        public Tuple<double, double>[] ValuePairs { get; private set; }

        /// <summary>
        /// The bit that will be matched against the output of a data set.
        /// </summary>
        public int Output { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a ClassificationRule with a random set of value pairs for the input and random output bit.
        /// </summary>
        /// <param name="ruleLength">The length of the rule to generate.</param>
        /// <returns>A randomly generated rule.</returns>
        public static ClassificationRule GenerateRule(int ruleLength, double minimumValue, double maximumValue)
        {
            Tuple<double, double>[] valuePairs = new Tuple<double, double>[ruleLength - 1];
            for (int i = 0; i < valuePairs.Length; i++)
            {
                double d1 = (RANDOM_GENERATOR.NextDouble() * (maximumValue - minimumValue)) + minimumValue;
                double d2 = (RANDOM_GENERATOR.NextDouble() * (maximumValue - minimumValue)) + minimumValue;
                valuePairs[i] = CreateTuple(d1, d2);
            }
            int output = RANDOM_GENERATOR.Next(0, 2);
            return new ClassificationRule(valuePairs, output, minimumValue, maximumValue);
        }

        /// <summary>
        /// Crosses over this rule with the provided rule, setting the two out parameters to the new child rules.
        /// </summary>
        /// <param name="rule">The rule to cross this over with.</param>
        /// <param name="child1">The first rule produced by this operation.</param>
        /// <param name="child2">The second rule produced by this operation.</param>
        public void Crossover(ClassificationRule rule, out ClassificationRule child1, out ClassificationRule child2)
        {
            int ruleLength = ValuePairs.Length + 1;
            int rand = RANDOM_GENERATOR.Next(1, ruleLength);
            Tuple<double, double>[] valuePairs1 = new Tuple<double, double>[ruleLength - 1];
            Tuple<double, double>[] valuePairs2 = new Tuple<double, double>[ruleLength - 1];
            Array.Copy(ValuePairs, 0, valuePairs2, 0, rand);
            Array.Copy(rule.ValuePairs, 0, valuePairs1, 0, rand);
            if (rand != ruleLength - 1)
            {
                Array.Copy(ValuePairs, rand, valuePairs1, rand, ruleLength - 1 - rand);
                Array.Copy(rule.ValuePairs, rand, valuePairs2, rand, ruleLength - 1 - rand);
            }
            int output1 = rule.Output;
            int output2 = Output;
            child1 = new ClassificationRule(valuePairs1, output1, m_minimumValue, m_maximumValue);
            child2 = new ClassificationRule(valuePairs2, output2, m_minimumValue, m_maximumValue);
        }

        /// <summary>
        /// Randomly determines whether to mutate this rule.
        /// </summary>
        /// <param name="mutationRate">The value to be used when determining whether to mutate a solution or not. (1/MutationRate probability to mutate).</param>
        public void Mutate(int mutationRate, double mutationStep)
        {
            for (int i = 0; i < ValuePairs.Length; i++)
            {
                int rand1 = RANDOM_GENERATOR.Next(0, mutationRate);
                if (rand1 == 0)
                {
                    double d1 = ValuePairs[i].Item1;
                    double d2 = ValuePairs[i].Item2;
                    rand1 = RANDOM_GENERATOR.Next(0, 2);
                    double randStep = RANDOM_GENERATOR.NextDouble() % mutationStep;
                    if (rand1 == 0)
                    {
                        d1 = ShiftDouble(d1, randStep);
                        d1 = ConstrainDouble(d1);
                    }
                    else
                    {
                        d2 = ShiftDouble(d2, randStep);
                        d2 = ConstrainDouble(d2);
                    }
                    ValuePairs[i] = CreateTuple(d1, d2);
                }
            }
            int randOut = RANDOM_GENERATOR.Next(0, mutationRate);
            if (randOut == 0)
            {
                Output = Output == 0 ? 1 : 0;
            }
        }

        /// <summary>
        /// Checks whether value pairs of this rule match that of the provided data set.
        /// </summary>
        /// <param name="data">The data set to check against.</param>
        /// <returns>True if the inputs match; otherwise false.</returns>
        public bool InputMatches(RealDataSet data)
        {
            for (int i = 0; i < ValuePairs.Length; i++)
            {
                if (!(data.Input[i] >= ValuePairs[i].Item1 && data.Input[i] <= ValuePairs[i].Item2))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks whether the output of this rule matches that of the provided data set.
        /// </summary>
        /// <param name="data">The data set to check against.</param>
        /// <returns>True if the outputs match; otherwise false.</returns>
        public bool OutputMatches(RealDataSet data)
        {
            return Output == data.Output;
        }

        /// <summary>
        /// Creates a tuple of doubles in the correct configuration from the values provided.
        /// </summary>
        /// <param name="d1">The first double.</param>
        /// <param name="d2">The second double.</param>
        /// <returns>The generated tuple, where Item1 is always smaller than Item2.</returns>
        private static Tuple<double, double> CreateTuple(double d1, double d2)
        {
            if (d1 < d2)
            {
                return new Tuple<double, double>(d1, d2);
            }
            else
            {
                return new Tuple<double, double>(d2, d1);
            }
        }

        /// <summary>
        /// Shifts the provided double value by the provided step, in the positive or negative direction, randomly chosen.
        /// </summary>
        /// <param name="value">The value to update.</param>
        /// <param name="step">The value to add or subtract.</param>
        /// <returns>The altered value.</returns>
        private static double ShiftDouble(double value, double step)
        {
            int rand = RANDOM_GENERATOR.Next(0, 2);
            if (rand == 0)
            {
                return value + step;
            }
            else
            {
                return value - step;
            }
        }

        /// <summary>
        /// Constrains the provided double value between the minimum and maximum value bounds.
        /// </summary>
        /// <param name="value">The value to constrain.</param>
        /// <returns>A double value between the minimum and maximum values inclusive.</returns>
        private double ConstrainDouble(double value)
        {
            return Math.Max(Math.Min(value, m_maximumValue), m_minimumValue);
        }

        /// <summary>
        /// Returns the string representation of this rule.
        /// </summary>
        /// <returns>The string representation of this rule.</returns>
        public override string ToString()
        {
            string output = "";
            for (int i = 0; i < ValuePairs.Length; i++)
            {
                output += "<" + ValuePairs[i].Item1 + ", " + ValuePairs[i].Item2 + "> ";
            }
            output += Output;
            return output;
        }

        #endregion

    }
}
