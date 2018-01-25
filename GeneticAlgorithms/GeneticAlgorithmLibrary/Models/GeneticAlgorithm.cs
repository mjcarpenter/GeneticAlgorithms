using System;

namespace GeneticAlgorithmLibrary.Models
{
    /// <summary>
    /// Provides the standard shell and process of a genetic algorithm.
    /// </summary>
    public abstract class GeneticAlgorithm
    {

        #region Variables

        /// <summary>
        /// The current candidate solution population.
        /// </summary>
        protected Solution[] Population;

        /// <summary>
        /// The current candidate solution parent population.
        /// </summary>
        private Solution[] Parents;

        /// <summary>
        /// The current candidate solution offspring population.
        /// </summary>
        private Solution[] Offspring;

        /// <summary>
        /// The current generation.
        /// </summary>
        private int currentGeneration;

        /// <summary>
        /// Random number generator used for crossover and mutation.
        /// </summary>
        private static Random RANDOM_GENERATOR = new Random();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new GeneticAlgorithm with the provided configuration.
        /// </summary>
        /// <param name="config">The configuration for the algorithm.</param>
        public GeneticAlgorithm(GeneticAlgorithmConfig config)
        {
            Generations = config.Generations;
            PopulationSize = config.PopulationSize;
            MutationRate = config.MutationRate;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The number of generations to evolve solutions for.
        /// </summary>
        public int Generations { get; private set; }

        /// <summary>
        /// The candidate solution population size.
        /// </summary>
        public int PopulationSize { get; private set; }

        /// <summary>
        /// The value to be used when determining whether to mutate a solution or not. (1/MutationRate probability to mutate).
        /// </summary>
        public int MutationRate { get; private set; }

        /// <summary>
        /// The fitness of the best solution for each generation.
        /// </summary>
        public double[] BestFitness { get; private set; }

        /// <summary>
        /// The average fitness of the solution population for each generation.
        /// </summary>
        public double[] AverageFitness { get; private set; }

        /// <summary>
        /// The percentage of the testing data classified correctly for each generation.
        /// </summary>
        public double[] TestResults { get; private set; }

        /// <summary>
        /// Whether the algorithm can be tested using test data or not.
        /// </summary>
        public abstract bool CanTest { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Runs the algorithm with the provided parameters, setting the historic values when finished.
        /// </summary>
        /// <param name="outputToConsole">Whether the algorithm should output the best and average fitnesses each generation.</param>
        public void RunAlgorithm(bool outputToConsole = false)
        {
            Populate();
            Record(outputToConsole);
            for (int i = 0; i < Generations; i++)
            {
                currentGeneration++;
                SelectParents();
                Crossover();
                Mutation();
                Survival();
                Record(outputToConsole);
            }
            if (outputToConsole)
            {
                ShowBestSolution();
            }
        }

        /// <summary>
        /// Tests the algorithm if testing data has been provided.
        /// </summary>
        /// <returns>The accuracy of the algorithm against the test data.</returns>
        public abstract double Test();

        /// <summary>
        /// Populates the population of candidate solutions through random generation.
        /// </summary>
        private void Populate()
        {
            currentGeneration = 0;
            Population = new Solution[PopulationSize];
            BestFitness = new double[Generations + 1];
            AverageFitness = new double[Generations + 1];
            if (CanTest)
            {
                TestResults = new double[Generations + 1];
            }
            for (int i = 0; i < PopulationSize; i++)
            {
                Population[i] = GenerateRandomSolution();
            }
        }

        /// <summary>
        /// Generates a random solution for the problem.
        /// </summary>
        /// <returns>A randomly generated solution.</returns>
        internal abstract Solution GenerateRandomSolution();

        /// <summary>
        /// Selects sets of parents from the population through tournament selection.
        /// </summary>
        private void SelectParents()
        {
            Parents = new Solution[PopulationSize];
            Offspring = new Solution[PopulationSize];
            for (int i = 0; i < PopulationSize; i++)
            {
                Parents[i] = TournamentSelect(Population);
            }
        }

        /// <summary>
        /// Creates new candidate solutions from the parent solutions through crossover.
        /// </summary>
        private void Crossover()
        {
            for (int i = 0; i < PopulationSize; i += 2)
            {
                Solution parent1 = Parents[i];
                Solution parent2;
                bool oddPopulation = i + 1 == PopulationSize;
                if (!oddPopulation && i != PopulationSize - 1)
                {
                    parent2 = Parents[i + 1];
                }
                else
                {
                    parent2 = Parents[0];
                }
                Solution child1;
                Solution child2;
                parent1.Crossover(parent2, out child1, out child2);
                Offspring[i] = child1;
                if (!oddPopulation && i != PopulationSize - 1)
                {
                    Offspring[i + 1] = child2;
                }
            }
        }

        /// <summary>
        /// Alters the newly created offspring through the random process of mutation.
        /// </summary>
        private void Mutation()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Offspring[i].Mutate(MutationRate);
            }
        }

        /// <summary>
        /// Creates a new population from the generated offspring and current best solution.
        /// </summary>
        private void Survival()
        {
            Solution[] solutions = CombinePopulation();
            Solution best = GetBestSolution(solutions);
            for (int i = 0; i < PopulationSize; i++)
            {
                Population[i] = TournamentSelect(solutions);
            }
            Population[GetWorstSolutionIndex(Population)] = best;
        }

        /// <summary>
        /// Updates the records for the generation.
        /// </summary>
        /// <param name="outputToConsole">Whether the algorithm should output the best and average fitnesses.</param>
        private void Record(bool outputToConsole)
        {
            Solution best = GetBestSolution(Population);
            double average = GetAverageFitness();
            BestFitness[currentGeneration] = best.GetFitness();
            AverageFitness[currentGeneration] = average;
            if (CanTest)
            {
                TestResults[currentGeneration] = Test();
            }
            if (outputToConsole)
            {
                string outputString = "Generation " + currentGeneration + ": Best: " + best.GetFitness() + ", Average: " + average;
                if (CanTest)
                {
                    outputString += ", Test Result: " + TestResults[currentGeneration] + ".";
                }
                else
                {
                    outputString += ".";
                }
                Console.WriteLine(outputString);
            }
        }

        private void ShowBestSolution()
        {
            string solutionString = GetBestSolution(Population).ToString();
            Console.WriteLine(solutionString);
        }

        /// <summary>
        /// Selects two random solutions from the provided array and returns the one with the higher fitness.
        /// </summary>
        /// <param name="solutions">The array of candidate solutions to select from.</param>
        /// <returns></returns>
        private Solution TournamentSelect(Solution[] solutions)
        {
            int rand1 = RANDOM_GENERATOR.Next(0, solutions.Length);
            int rand2 = RANDOM_GENERATOR.Next(0, solutions.Length);
            Solution s1 = solutions[rand1];
            Solution s2 = solutions[rand2];
            return s1.GetFitness() > s2.GetFitness() ? s1 : s2;
        }

        /// <summary>
        /// Combines the current population and new offspring into one array.
        /// </summary>
        /// <returns>The combined array of population and offspring.</returns>
        private Solution[] CombinePopulation()
        {
            Solution[] combined = new Solution[PopulationSize * 2];
            Array.Copy(Population, combined, PopulationSize);
            Array.Copy(Offspring, 0, combined, PopulationSize, PopulationSize);
            return combined;
        }

        /// <summary>
        /// Gets the solution with the highest fitness in the provided array.
        /// </summary>
        /// <param name="solutions">The array of candidate solutions to select from.</param>
        /// <returns>The solution with the highest fitness.</returns>
        protected static Solution GetBestSolution(Solution[] solutions)
        {
            double bestValue = 0;
            int bestIndex = 0;
            for (int i = 0; i < solutions.Length; i++)
            {
                Solution s = solutions[i];
                double fitness = s.GetFitness();
                if (fitness > bestValue)
                {
                    bestValue = fitness;
                    bestIndex = i;
                }
            }
            return solutions[bestIndex];
        }

        /// <summary>
        /// Gets the index of the solution with the lowest fitness in the provided array.
        /// </summary>
        /// <param name="solutions">The array of candidate solutions to select from.</param>
        /// <returns>The index of the solution with the lowest fitness.</returns>
        private static int GetWorstSolutionIndex(Solution[] solutions)
        {
            double worstValue = double.MaxValue;
            int worstIndex = 0;
            for (int i = 0; i < solutions.Length; i++)
            {
                Solution s = solutions[i];
                double fitness = s.GetFitness();
                if (fitness < worstValue)
                {
                    worstValue = fitness;
                    worstIndex = i;
                }
            }
            return worstIndex;
        }

        /// <summary>
        /// Calculates the average fitness of the current population.
        /// </summary>
        /// <returns>The average fitness of the current population.</returns>
        private double GetAverageFitness()
        {
            double total = 0;
            for (int i = 0; i < PopulationSize; i++)
            {
                Solution s = Population[i];
                total += s.GetFitness();
            }
            return total / PopulationSize;
        }

        #endregion

    }
}
