using GeneticAlgorithmLibrary.AlgorithmConfig;
using GeneticAlgorithmLibrary.Algorithms;
using GeneticAlgorithmLibrary.Helpers;
using GeneticAlgorithmLibrary.Models;
using GeneticAlgorithmLibrary.Solutions.BinaryRuleClassification;
using GeneticAlgorithmLibrary.Solutions.DataClassification;
using System;
using System.IO;
using System.Windows.Forms;

namespace GeneticAlgorithmConsole
{
    public class Program
    {

        #region Variables

        /// <summary>
        /// List of all available algorithms.
        /// </summary>
        private static string[] s_algorithms = new string[] { "Binary Rule Classification | Data Format: [00000 0]", "Data Classification | Data Format: [0.1 0.2 0.3 0.4 0.5 0]" };

        /// <summary>
        /// Currently selected algorithm, represented as an index.
        /// </summary>
        private static int s_selectedAlgorithm;

        /// <summary>
        /// Entered value for number of runs to complete.
        /// </summary>
        private static int s_runs;

        /// <summary>
        /// Entered value for number of generations for the algorithm.
        /// </summary>
        private static int s_generations;

        /// <summary>
        /// Entered value for population size for the algorithm.
        /// </summary>
        private static int s_populationSize;

        /// <summary>
        /// Entered value for the mutation rate for the algorithm.
        /// </summary>
        private static int s_mutationRate;

        /// <summary>
        /// Holds the result of the best fitness per generation.
        /// </summary>
        private static double[] s_bestFitnessResults;

        /// <summary>
        /// Holds the result of the average fitness per generation.
        /// </summary>
        private static double[] s_averageFitnessResults;

        /// <summary>
        /// Holds the result of the algorithm test result per generation.
        /// </summary>
        private static double[] s_testResults;

        #endregion

        #region Methods

        [STAThread]
        public static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("==========================================");
                Console.WriteLine("Genetic Algorithm Console");
                if (GetParameters())
                {
                    object[] specificParameters;
                    if (GetAlgorithmSpecificParameters(out specificParameters))
                    {
                        RunAlgorithm(specificParameters);
                        OutputResults();
                        OfferFileOutput();
                        Console.Write("Run again? Y/N: ");
                        string runAgain = Console.ReadLine().ToUpper();
                        switch (runAgain)
                        {
                            case "N":
                                exit = true;
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid parameter entered, restarting.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid parameter entered, restarting.");
                }
            }
        }

        /// <summary>
        /// Asks the user for the standard genetic algorithm parameters.
        /// </summary>
        /// <returns>True if all values entered are valid; otherwise false.</returns>
        private static bool GetParameters()
        {
            Console.Write(GetAlgorithmOptions());
            Console.Write("Select algorithm to run: ");
            string selectedAlgorithm = Console.ReadLine();
            if (!int.TryParse(selectedAlgorithm, out s_selectedAlgorithm) || s_selectedAlgorithm > s_algorithms.Length)
            {
                return false;
            }
            Console.Write("Enter number of runs for algorithm: ");
            string runs = Console.ReadLine();
            if (!int.TryParse(runs, out s_runs))
            {
                return false;
            }
            Console.Write("Enter generations to evolve: ");
            string generations = Console.ReadLine();
            if (!int.TryParse(generations, out s_generations))
            {
                return false;
            }
            Console.Write("Enter population size to use: ");
            string populationSize = Console.ReadLine();
            if (!int.TryParse(populationSize, out s_populationSize))
            {
                return false;
            }
            Console.Write("Enter mutation rate to use in the format 1/x: ");
            string mutationRate = Console.ReadLine();
            if (!int.TryParse(mutationRate, out s_mutationRate))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Asks the user for the parameters specific to the selected algorithm.
        /// </summary>
        /// <param name="parameters">The array in which the entered values will be stored.</param>
        /// <returns>True if all values entered are valid; otherwise false.</returns>
        private static bool GetAlgorithmSpecificParameters(out object[] parameters)
        {
            switch (s_selectedAlgorithm)
            {
                case 1:
                    int binaryRuleCount;
                    bool useWildcard;
                    BinaryDataSet[] binaryTrainingData;
                    BinaryDataSet[] binaryTestingData;
                    bool binarySuccess = GetBinaryRuleParameters(out binaryRuleCount, out useWildcard, out binaryTrainingData, out binaryTestingData);
                    parameters = new object[] { binaryRuleCount, useWildcard, binaryTrainingData, binaryTestingData };
                    return binarySuccess;
                case 2:
                    int realRuleCount;
                    double minimumValue;
                    double maximumValue;
                    double maximumMutationStep;
                    RealDataSet[] realTrainingData;
                    RealDataSet[] realTestingData;
                    bool dataSuccess = GetDataClassificationParameters(out realRuleCount, out minimumValue, out maximumValue, out maximumMutationStep, out realTrainingData, out realTestingData);
                    parameters = new object[] { realRuleCount, minimumValue, maximumValue, maximumMutationStep, realTrainingData, realTestingData };
                    return dataSuccess;
            }
            parameters = null;
            return false;
        }

        /// <summary>
        /// Asks the user for the parameters specific to the binary rule algorithm.
        /// </summary>
        /// <param name="ruleCount">The value the user entered for rule count.</param>
        /// <param name="useWildcard">The option the user selected for using the wildcard operator.</param>
        /// <param name="trainingData">The data the user selected for training.</param>
        /// <param name="testingData">The data the user selected for testing; empty if they did not select to train and test.</param>
        /// <returns>True if all values entered are valid and the selected data file is valid; otherwise false.</returns>
        private static bool GetBinaryRuleParameters(out int ruleCount, out bool useWildcard, out BinaryDataSet[] trainingData, out BinaryDataSet[] testingData)
        {
            ruleCount = 0;
            useWildcard = false;
            trainingData = null;
            testingData = null;
            Console.Write("Enter rules per solution: ");
            string rules = Console.ReadLine();
            if (!int.TryParse(rules, out ruleCount))
            {
                return false;
            }
            Console.Write("Use wildcard operator (#)? Y/N: ");
            string wildcard = Console.ReadLine().ToUpper();
            switch (wildcard)
            {
                case "N":
                    useWildcard = false;
                    break;
                case "Y":
                    useWildcard = true;
                    break;
                default:
                    return false;
            }
            Console.Write("Select data file to use: ");
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            string fileName = ofd.FileName;
            Console.WriteLine(fileName);
            Console.Write("Split data into training and testing? Y/N: ");
            string trainAndTest = Console.ReadLine().ToUpper();
            bool splitData;
            switch (trainAndTest)
            {
                case "N":
                    splitData = false;
                    break;
                case "Y":
                    splitData = true;
                    break;
                default:
                    return false;
            }
            if (!ReadBinaryData(fileName, splitData, out trainingData, out testingData))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Asks the user for the parameters specific to the data classification algorithm.
        /// </summary>
        /// <param name="ruleCount">The value the user entered for rule count.</param>
        /// <param name="minimumValue">The minimum value of all the input values read in.</param>
        /// <param name="maximumValue">The maximum value of all the input values read in.</param>
        /// <param name="maxMutationStep">The value the user entered for maximum mutation step.</param>
        /// <param name="trainingData">The data the user selected for training.</param>
        /// <param name="testingData">The data the user selected for testing; empty if they did not select to train and test.</param>
        /// <returns>True if all values entered are valid and the selected data file is valid; otherwise false.</returns>
        private static bool GetDataClassificationParameters(out int ruleCount, out double minimumValue, out double maximumValue, out double maximumMutationStep, out RealDataSet[] trainingData, out RealDataSet[] testingData)
        {
            ruleCount = 0;
            minimumValue = 0;
            maximumValue = 0;
            maximumMutationStep = 0;
            trainingData = null;
            testingData = null;
            Console.Write("Enter rules per solution: ");
            string rules = Console.ReadLine();
            if (!int.TryParse(rules, out ruleCount))
            {
                return false;
            }
            Console.Write("Enter max mutation step: ");
            string wildcard = Console.ReadLine().ToUpper();
            if (!double.TryParse(wildcard, out maximumMutationStep))
            {
                return false;
            }
            Console.Write("Select data file to use: ");
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            string fileName = ofd.FileName;
            Console.WriteLine(fileName);
            Console.Write("Split data into training and testing? Y/N: ");
            string trainAndTest = Console.ReadLine().ToUpper();
            bool splitData;
            switch (trainAndTest)
            {
                case "N":
                    splitData = false;
                    break;
                case "Y":
                    splitData = true;
                    break;
                default:
                    return false;
            }
            if (!ReadRealData(fileName, splitData, out minimumValue, out maximumValue, out trainingData, out testingData))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Runs the selected algorithm with the entered parameters.
        /// </summary>
        /// <param name="parameters">The specific paramters for the selected algorithm.</param>
        private static void RunAlgorithm(object[] parameters)
        {
            GeneticAlgorithm algorithm = CreateGeneticAlgorithm(parameters);
            AlgorithmRunner runner = new AlgorithmRunner(algorithm, s_runs);
            runner.Start(true, true);
            s_bestFitnessResults = runner.BestFitnessAverage;
            s_averageFitnessResults = runner.AverageFitnessAverage;
            if (algorithm.CanTest)
            {
                s_testResults = runner.AverageTestResults;
            }
            else
            {
                s_testResults = null;
            }
        }

        /// <summary>
        /// Outputs the final results of the algorithm run to the console.
        /// </summary>
        private static void OutputResults()
        {
            Console.WriteLine("Over an average of " + s_runs + " runs, after " + s_generations + " generations:");
            Console.WriteLine("The best fitness was: " + s_bestFitnessResults[s_generations - 1]);
            Console.WriteLine("The average fitness was: " + s_averageFitnessResults[s_generations - 1]);
            if (s_testResults != null)
            {
                Console.WriteLine("The average accuracy of the algorithm on the final generation was: " + s_testResults[s_generations - 1]);
            }
        }

        /// <summary>
        /// Offers the option to the user to save the results to a text file.
        /// </summary>
        private static void OfferFileOutput()
        {
            Console.Write("Would you like to export the results to file? Y/N: ");
            string exportString = Console.ReadLine().ToUpper();
            bool export;
            switch (exportString)
            {
                case "N":
                    export = false;
                    break;
                case "Y":
                    export = true;
                    break;
                default:
                    export = true;
                    break;
            }
            if (export)
            {
                string[] output = GetExportData();
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.RestoreDirectory = true;
                sfd.FileName = "*.txt";
                sfd.Filter = "Text Documents (*.txt)|*.txt";
                sfd.FilterIndex = 2;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllLines(sfd.FileName, output);
                }
            }
        }

        /// <summary>
        /// Returns a string of all valid algorithms and their indexes.
        /// </summary>
        /// <returns>A string of all valid algorithms and their indexes.</returns>
        private static string GetAlgorithmOptions()
        {
            string result = "";
            for (int i = 0; i < s_algorithms.Length; i++)
            {
                result += (i + 1) + ": " + s_algorithms[i] + "\n";
            }
            return result;
        }

        /// <summary>
        /// Reads the binary data in from the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to read data from.</param>
        /// <param name="splitData">Whether to split the file into training and testing data.</param>
        /// <param name="trainingData">The data the user selected for training.</param>
        /// <param name="testingData">The data the user selected for testing; empty if they did not select to train and test.</param>
        /// <returns>True if the file is a valid data file; otherwise false.</returns>
        private static bool ReadBinaryData(string fileName, bool splitData, out BinaryDataSet[] trainingData, out BinaryDataSet[] testingData)
        {
            string[] lines = File.ReadAllLines(fileName);
            int trainDataSize = lines.Length;
            if (splitData)
            {
                trainDataSize /= 2;
            }
            trainingData = new BinaryDataSet[trainDataSize];
            testingData = new BinaryDataSet[lines.Length - trainDataSize];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] lineParts = lines[i].Split(' ');
                string inputString = lineParts[0];
                int[] input = new int[inputString.Length];
                for (int j = 0; j < input.Length; j++)
                {
                    int bit;
                    if (!int.TryParse(inputString.Substring(j, 1), out bit))
                    {
                        return false;
                    }
                    input[j] = bit;
                }
                int output;
                if (!int.TryParse(lineParts[1], out output))
                {
                    return false;
                }
                BinaryDataSet data = new BinaryDataSet(input, output);
                if (splitData)
                {
                    if (i % 2 == 0)
                    {
                        trainingData[i / 2] = data;
                    }
                    else
                    {
                        testingData[i / 2] = data;
                    }
                }
                else
                {
                    trainingData[i] = data;
                }
            }
            return true;
        }

        /// <summary>
        /// Reads the real-world data in from the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to read data from.</param>
        /// <param name="splitData">Whether to split the file into training and testing data.</param>
        /// <param name="minimumValue">The minimum value of all the input values read in.</param>
        /// <param name="maximumValue">The maximum value of all the input values read in.</param>
        /// <param name="trainingData">The data the user selected for training.</param>
        /// <param name="testingData">The data the user selected for testing; empty if they did not select to train and test.</param>
        /// <returns>True if the file is a valid data file; otherwise false.</returns>
        private static bool ReadRealData(string fileName, bool splitData, out double minimumValue, out double maximumValue, out RealDataSet[] trainingData, out RealDataSet[] testingData)
        {
            string[] lines = File.ReadAllLines(fileName);
            int trainDataSize = lines.Length;
            if (splitData)
            {
                trainDataSize /= 2;
            }
            trainingData = new RealDataSet[trainDataSize];
            testingData = new RealDataSet[lines.Length - trainDataSize];
            minimumValue = double.MaxValue;
            maximumValue = double.MinValue;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] lineParts = lines[i].Split(' ');
                double[] input = new double[lineParts.Length - 1];
                for (int j = 0; j < input.Length; j++)
                {
                    double value;
                    if (!double.TryParse(lineParts[j], out value))
                    {
                        return false;
                    }
                    if (value < 100)
                    {

                    }
                    if (value < minimumValue)
                    {
                        minimumValue = value;
                    }
                    if (value > maximumValue)
                    {
                        maximumValue = value;
                    }
                    input[j] = value;
                }
                int output;
                if (!int.TryParse(lineParts[lineParts.Length - 1], out output))
                {
                    return false;
                }
                RealDataSet data = new RealDataSet(input, output);
                if (splitData)
                {
                    if (i % 2 == 0)
                    {
                        trainingData[i / 2] = data;
                    }
                    else
                    {
                        testingData[i / 2] = data;
                    }
                }
                else
                {
                    trainingData[i] = data;
                }
            }
            return true;
        }

        /// <summary>
        /// Creates the genetic algorithm with the entered parameters.
        /// </summary>
        /// <param name="parameters">The specific parameters for the selected algorithm.</param>
        /// <returns>The created genetic algorithm.</returns>
        private static GeneticAlgorithm CreateGeneticAlgorithm(object[] parameters)
        {
            switch (s_selectedAlgorithm)
            {
                case 1:
                    bool testBinaryData = ((BinaryDataSet[])parameters[3]).Length > 0;
                    BinaryClassificationConfig binaryConfig = new BinaryClassificationConfig(s_generations, s_populationSize, s_mutationRate, (int)parameters[0], (bool)parameters[1]);
                    if (testBinaryData)
                    {
                        return new BinaryClassificationAlgorithm(binaryConfig, (BinaryDataSet[])parameters[2], (BinaryDataSet[])parameters[3]);
                    }
                    return new BinaryClassificationAlgorithm(binaryConfig, (BinaryDataSet[])parameters[2]);
                case 2:
                    bool testRealData = ((RealDataSet[])parameters[5]).Length > 0;
                    DataClassificationConfig realConfig = new DataClassificationConfig(s_generations, s_populationSize, s_mutationRate, (int)parameters[0], (double)parameters[1], (double)parameters[2], (double)parameters[3]);
                    if (testRealData)
                    {
                        return new DataClassificationAlgorithm(realConfig, (RealDataSet[])parameters[4], (RealDataSet[])parameters[5]);
                    }
                    return new DataClassificationAlgorithm(realConfig, (RealDataSet[])parameters[4]);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Generates an array of strings from the algorithm output to write to file.
        /// </summary>
        /// <returns>The lines to write to file.</returns>
        private static string[] GetExportData()
        {
            string[] lines = new string[s_generations];
            for (int i = 0; i < lines.Length; i++)
            {
                if (s_testResults == null)
                {
                    lines[i] = s_bestFitnessResults[i] + " " + s_averageFitnessResults[i];
                }
                else
                {
                    lines[i] = s_bestFitnessResults[i] + " " + s_averageFitnessResults[i] + " " + s_testResults[i];
                }
            }
            return lines;
        }

        #endregion

    }
}
