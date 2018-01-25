namespace GeneticAlgorithmLibrary.Solutions.BinaryRuleClassification
{
    /// <summary>
    /// Holds a set of binary data that the BinaryClassification algorithm can train against.
    /// </summary>
    public class BinaryDataSet
    {

        #region Constructor

        /// <summary>
        /// Creates a new BinaryDataSet with the provided input and output values.
        /// </summary>
        /// <param name="input">An array of bits that represents a binary input.</param>
        /// <param name="output">A bit that represents the value of the classified data.</param>
        public BinaryDataSet(int[] input, int output)
        {
            Input = input;
            Output = output;
        }

        #endregion

        #region Properties

        /// <summary>
        /// An array of bits that represents a binary input.
        /// </summary>
        public int[] Input { get; set; }

        /// <summary>
        /// A bit that represents the value of the classified data.
        /// </summary>
        public int Output { get; set; }

        #endregion

    }
}
