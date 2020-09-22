using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWFood
{
    static class GeneticAlgorithm
    {
        static readonly int GenNumber = int.Parse(ConfigurationManager.AppSettings.Get("GA_GenerationNumber"));
        static readonly int PopSize = int.Parse(ConfigurationManager.AppSettings.Get("GA_PopulationSize"));
        static readonly float PerOfBestToKeep = float.Parse(ConfigurationManager.AppSettings.Get("GA_PercentageOfBestsToKeep"));
        static readonly float StableStop = float.Parse(ConfigurationManager.AppSettings.Get("GA_StableStop"));

        /// <summary>
        /// Main loop of the genetic algorithm.
        /// </summary>
        /// <param name="aFoodBase"></param>
        public static void GeneticAlgorithmMain(FoodBase aFoodBase)
        {
            List<FoodSample> foodSampleList = new List<FoodSample>();
            FoodSample foodsample;
            double SD;
            double lastSD = double.MaxValue;
            int stableCount = 0;

            // Generate initial food sample
            for (int i = 0; i < PopSize; i++)
            {
                foodsample = new FoodSample(aFoodBase);
                foodsample.GenRandom();
                foodSampleList.Add(foodsample);
            }

            // Run the algorithm
            for (int i = 0; i < GenNumber; i++)
            {
                foodSampleList = SelectBest(foodSampleList, (int)(PopSize * PerOfBestToKeep));
                foodSampleList = CombineAndMutate(foodSampleList);

                // Display
                Console.SetCursorPosition(0, 0);
                SD = SelectBest(foodSampleList, 1)[0].StandardDeviation();
                Console.Write($"Generation {i}\t| SD: {SD}");
                //if (SD == 0) break;

                // Check for a stable behavior
                if (SD == lastSD) stableCount++;
                if (StableStop !=0 && stableCount >= StableStop) break;
                lastSD = SD;
            }

            // Display Results
            Console.WriteLine("\nDONE\n");
            SelectBest(foodSampleList, 1)[0].PrintSample();
        }

        /// <summary>
        /// From a list of food samples, selects and return the n best ones.
        /// </summary>
        /// <param name="aFoodSampleList"></param>
        /// <param name="nBest"></param>
        /// <returns></returns>
        private static List<FoodSample> SelectBest(List<FoodSample> aFoodSampleList, int nBest)
        {
            //int size = aFoodSampleList.Count - nBest;
            List<FoodSample> foodSamples;
            // Minimize the standard deviation
            foodSamples = aFoodSampleList.OrderBy(a => a.StandardDeviation()).ToList();
            foodSamples.RemoveRange(nBest, aFoodSampleList.Count - nBest);
            return foodSamples;
        }

        /// <summary>
        /// Breed random parents from the population and returns a list of mutated offsprings.
        /// </summary>
        /// <param name="aParents"></param>
        /// <returns></returns>
        private static List<FoodSample> CombineAndMutate(List<FoodSample> aParents)
        {
            List<FoodSample> offsprings = new List<FoodSample>();
            FoodSample parent1;
            FoodSample parent2;
            FoodSample offspring;

            while (offsprings.Count < PopSize)
            {
                parent1 = aParents[StaticRandom.Rand(0, aParents.Count)];
                parent2 = aParents[StaticRandom.Rand(0, aParents.Count)];
                offspring = parent1.UniformCrossover(parent2);
                offspring.Mutate();
                offsprings.Add(offspring);
            }

            return offsprings;
        }
    }
}
