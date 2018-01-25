using System;

namespace GeneticAlgorithmLibrary.Solutions.BinaryRuleClassification
{
    /// <summary>
    /// A rule used to match against binary data sets to indicate how the data should be classified.
    /// </summary>
    internal class BinaryRule
    {

        #region Variables

        /// <summary>
        /// Random number generator used for crossover and mutation.
        /// </summary>
        private static Random RANDOM_GENERATOR = new Random();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new BinaryRule with the specified parameters.
        /// </summary>
        /// <param name="input">An array of bits that will be matched against the input of a data set.</param>
        /// <param name="output">The bit that will be matched against the output of a data set.</param>
        /// <param name="useWildcard">Whether the wildcard operator should be used.</param>
        public BinaryRule(char[] input, char output, bool useWildcard)
        {
            Input = input;
            if (output == '#')
            {
                Output = RANDOM_GENERATOR.Next(0, 2) == 0 ? '0' : '1';
            }
            else
            {
                Output = output;
            }
            UseWildcard = useWildcard;
        }

        #endregion

        #region Properties

        /// <summary>
        /// An array of bits that will be matched against the input of a data set.
        /// </summary>
        public char[] Input { get; private set; }

        /// <summary>
        /// The bit that will be matched against the output of a data set.
        /// </summary>
        public char Output { get; private set; }

        /// <summary>
        /// Whether the wildcard operator should be used.
        /// </summary>
        public bool UseWildcard { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a BinaryRule with a random bit sequence for the input and random output bit.
        /// </summary>
        /// <param name="ruleLength">The length of the rule to generate.</param>
        /// <param name="useWildcard">Whether the wildcard operator should be used.</param>
        /// <returns>A randomly generated rule.</returns>
        public static BinaryRule GenerateRule(int ruleLength, bool useWildcard)
        {
            char[] input = new char[ruleLength - 1];
            for (int i = 0; i < input.Length; i++)
            {
                int maxRand = useWildcard ? 5 : 4;
                int rand = RANDOM_GENERATOR.Next(0, maxRand);
                if (rand == 0 || rand == 1)
                {
                    input[i] = '0';
                }
                else if (rand == 2 || rand == 3)
                {
                    input[i] = '1';
                }
                else
                {
                    input[i] = '#';
                }
            }
            char output = RANDOM_GENERATOR.Next(0, 2) == 0 ? '0' : '1';
            return new BinaryRule(input, output, useWildcard);
        }

        /// <summary>
        /// Generates an array of BinaryRules from the provided bit string.
        /// </summary>
        /// <param name="bitString">The array of chars from which to generate the rules.</param>
        /// <param name="ruleLength">The length of the rules to generate.</param>
        /// <param name="useWildcard">Whether the wildcard operator should be used.</param>
        /// <returns></returns>
        public static BinaryRule[] GetRules(char[] bitString, int ruleLength, bool useWildcard)
        {
            BinaryRule[] rules = new BinaryRule[bitString.Length / ruleLength];
            for (int i = 0; i < bitString.Length; i += ruleLength)
            {
                char[] input = new char[ruleLength - 1];
                for (int j = 0; j < input.Length; j++)
                {
                    input[j] = bitString[i + j];
                }
                char output = bitString[i + input.Length];
                rules[i / ruleLength] = new BinaryRule(input, output, useWildcard);
            }
            return rules;
        }

        /// <summary>
        /// Crosses over this rule with the provided rule, setting the two out parameters to the new child rules.
        /// </summary>
        /// <param name="rule">The rule to cross this over with.</param>
        /// <param name="child1">The first rule produced by this operation.</param>
        /// <param name="child2">The second rule produced by this operation.</param>
        public void Crossover(BinaryRule rule, out BinaryRule child1, out BinaryRule child2)
        {
            int ruleLength = Input.Length + 1;
            char[] bits1 = new char[ruleLength];
            char[] bits2 = new char[ruleLength];
            Array.Copy(Input, bits1, ruleLength - 1);
            Array.Copy(rule.Input, bits2, ruleLength - 1);
            bits1[ruleLength - 1] = Output;
            bits2[ruleLength - 1] = rule.Output;
            int crossover = RANDOM_GENERATOR.Next(1, ruleLength - 1);
            char[] child1Bits = new char[ruleLength];
            char[] child2Bits = new char[ruleLength];
            Array.Copy(bits1, 0, child1Bits, 0, crossover);
            Array.Copy(bits2, 0, child2Bits, 0, crossover);
            Array.Copy(bits2, crossover, child1Bits, crossover, ruleLength - crossover);
            Array.Copy(bits1, crossover, child2Bits, crossover, ruleLength - crossover);
            char[] child1Input = new char[ruleLength - 1];
            char[] child2Input = new char[ruleLength - 1];
            Array.Copy(child1Bits, child1Input, ruleLength - 1);
            Array.Copy(child2Bits, child2Input, ruleLength - 1);
            child1 = new BinaryRule(child1Input, child1Bits[ruleLength - 1], UseWildcard);
            child2 = new BinaryRule(child2Input, child2Bits[ruleLength - 1], UseWildcard);
        }

        /// <summary>
        /// Randomly determines whether to mutate this rule.
        /// </summary>
        /// <param name="mutationRate">The value to be used when determining whether to mutate a solution or not. (1/MutationRate probability to mutate).</param>
        public void Mutate(int mutationRate)
        {
            for (int i = 0; i < Input.Length; i++)
            {
                int rand = RANDOM_GENERATOR.Next(0, mutationRate);
                if (rand == 0)
                {
                    if (Input[i] != '#')
                    {
                        int maxRand = UseWildcard ? 2 : 1;
                        rand = RANDOM_GENERATOR.Next(0, maxRand);
                        if (rand == 0)
                        {
                            Input[i] = Input[i] == '0' ? '1' : '0';
                        }
                        else
                        {
                            Input[i] = '#';
                        }
                    }
                    else
                    {
                        rand = RANDOM_GENERATOR.Next(0, 2);
                        Input[i] = rand == 0 ? '0' : '1';
                    }
                }
            }
            int randOut = RANDOM_GENERATOR.Next(0, mutationRate);
            if (randOut == 0)
            {
                Output = Output == '0' ? '1' : '0';
            }
        }

        /// <summary>
        /// Checks whether the input sequence of this rule matches that of the provided data set.
        /// </summary>
        /// <param name="data">The data set to check against.</param>
        /// <returns>True if the inputs match; otherwise false.</returns>
        public bool InputMatches(BinaryDataSet data)
        {
            for (int i = 0; i < data.Input.Length; i++)
            {
                if (!CharacterMatches(Input[i], data.Input[i]))
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
        public bool OutputMatches(BinaryDataSet data)
        {
            return CharacterMatches(Output, data.Output);
        }

        /// <summary>
        /// Checks whether the provided char and bit match, accounting for the wildcard operator if it is present.
        /// </summary>
        /// <param name="rule">The char from the rule.</param>
        /// <param name="data">The bit from the data set.</param>
        /// <returns>True if the two pieces of data match; otherwise false.</returns>
        private static bool CharacterMatches(char rule, int data)
        {
            int ruleInt;
            if (int.TryParse(rule.ToString(), out ruleInt))
            {
                return ruleInt == data;
            }
            else if (rule == '#')
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the string representation of this rule.
        /// </summary>
        /// <returns>The string representation of this rule.</returns>
        public override string ToString()
        {
            string output = "";
            for (int i = 0; i < Input.Length; i++)
            {
                output += Input[i];
            }
            output += " ";
            output += Output;
            return output;
        }

        #endregion

    }
}
