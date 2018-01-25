namespace GeneticAlgorithmLibrary.Solutions.DataClassification
{
    /// <summary>
    /// Holds a set of real-world data that the DataClassification algorithm can train against.
    /// </summary>
    public class RealDataSet
    {

        #region Constructor

        /// <summary>
        /// Creates a new RealDataSet with the provided input and output values.
        /// </summary>
        /// <param name="input">An array of doubles that represent data inputs.</param>
        /// <param name="output">A bit that represents the value of the classified data.</param>
        public RealDataSet(double[] input, int output)
        {
            Input = input;
            Output = output;
        }

        #endregion

        #region Properties

        /// <summary>
        /// An array of doubles that represent data inputs.
        /// </summary>
        public double[] Input { get; set; }

        /// <summary>
        /// A bit that represents the value of the classified data.
        /// </summary>
        public int Output { get; set; }

        #endregion

    }
}
