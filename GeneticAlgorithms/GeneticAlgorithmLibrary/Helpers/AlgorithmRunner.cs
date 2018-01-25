using GeneticAlgorithmLibrary.Models;
using System;

namespace GeneticAlgorithmLibrary.Helpers
{
    /// <summary>
    /// Can be used to run an algorithm multiple times to calculate averages for the best and average fitness, as well as test results.
    /// </summary>
    public class AlgorithmRunner
    {

        #region Constructor

        /// <summary>
        /// Creates a new AlgorithmRunner to run the provided algorithm the number of times specified.
        /// </summary>
        /// <param name="algorithm">The algorithm to run.</param>
        /// <param name="runs">The number of times to run the algorithm.</param>
        public AlgorithmRunner(GeneticAlgorithm algorithm, int runs)
        {
            Algorithm = algorithm;
            Runs = runs;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The algorithm to run.
        /// </summary>
        public GeneticAlgorithm Algorithm { get; private set; }

        /// <summary>
        /// The number of times to run the algorithm.
        /// </summary>
        public int Runs { get; private set; }

        /// <summary>
        /// The average of the fitness of the best candidate solution for each generation.
        /// </summary>
        public double[] BestFitnessAverage { get; private set; }

        /// <summary>
        /// The average of the average fitness of the candidate solutions for each generation.
        /// </summary>
        public double[] AverageFitnessAverage { get; private set; }

        /// <summary>
        /// The average of the test results for each generation, if the algorithm can be tested.
        /// </summary>
        public double[] AverageTestResults { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Begins the running of the algorithm.
        /// </summary>
        /// <param name="outputGenerationToConsole">Whether the algorithm should output the best and average fitnesses each generation.</param>
        /// <param name="outputRunToConsole">Whether the runner should output the finishing of a run.</param>
        public void Start(bool outputGenerationToConsole = false, bool outputRunToConsole = false)
        {
            BestFitnessAverage = new double[Algorithm.Generations];
            AverageFitnessAverage = new double[Algorithm.Generations];
            AverageTestResults = new double[Algorithm.Generations];
            double[][] bestFitnessTracker = new double[Runs][];
            double[][] averageFitnessTracker = new double[Runs][];
            double[][] testResults = new double[Runs][];
            for (int i = 0; i < Runs; i++)
            {
                Algorithm.RunAlgorithm(outputGenerationToConsole);
                bestFitnessTracker[i] = Algorithm.BestFitness;
                averageFitnessTracker[i] = Algorithm.AverageFitness;
                if (Algorithm.CanTest)
                {
                    testResults[i] = Algorithm.TestResults;
                }
                if (outputRunToConsole)
                {
                    if (Algorithm.CanTest)
                    {
                        Console.WriteLine("Run " + (i + 1) + " completed. Accuracy of final solution was " + testResults[i][Algorithm.Generations - 1] + ".");
                    }
                    else
                    {
                        Console.WriteLine("Run " + (i + 1) + " completed.");
                    }
                }
            }
            for (int i = 0; i < Algorithm.Generations; i++)
            {
                double bestTotal = 0;
                double averageTotal = 0;
                double testTotal = 0;
                for (int j = 0; j < Runs; j++)
                {
                    bestTotal += bestFitnessTracker[j][i];
                    averageTotal += averageFitnessTracker[j][i];
                    if (Algorithm.CanTest)
                    {
                        testTotal += testResults[j][i];
                    }
                }
                double best = bestTotal / Runs;
                double average = averageTotal / Runs;
                double test = testTotal / Runs;
                BestFitnessAverage[i] = best;
                AverageFitnessAverage[i] = average;
                if (Algorithm.CanTest)
                {
                    AverageTestResults[i] = test;
                }
            }
        }

        #endregion

    }
}
