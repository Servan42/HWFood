using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWFood
{
    /// <summary>
    /// Describes a food sample used to get lvl 99.
    /// </summary>
    class FoodSample
    {
        private readonly FoodBase _foodBase;
        private Food _bestFood;

        public List<Food> FoodList { get; private set; }
        /// <summary>
        /// Aspiring
        /// </summary>
        public int Admiration { get; private set; }
        /// <summary>
        /// Valiant
        /// </summary>
        public int Classe { get; private set; }
        /// <summary>
        /// Shrewd
        /// </summary>
        public int Esquive { get; private set; }
        /// <summary>
        /// Eager
        /// </summary>
        public int Passion { get; private set; }
        /// <summary>
        /// Relaxed
        /// </summary>
        public int Volupte { get; private set; }
        /// <summary>
        /// Number of essences already given to the fairy.
        /// </summary>
        public int EssenceAlreadyGiven { get; private set; }

        /// <summary>
        /// Creates the sample.
        /// </summary>
        /// <param name="aFoodBase">Data about the food.</param>
        public FoodSample(FoodBase aFoodBase)
        {
            _foodBase = aFoodBase;
            FoodList = new List<Food>();
            Admiration = 0;
            Classe = 0;
            Esquive = 0;
            Passion = 0;
            Volupte = 0;
            AddBaseStat();
        }

        /// <summary>
        /// Creates the sample from anoter one. (Copy).
        /// </summary>
        /// <param name="aFoodSample">The food sample to copy.</param>
        public FoodSample(FoodSample aFoodSample)
        {
            _foodBase = aFoodSample._foodBase;
            FoodList = new List<Food>(aFoodSample.FoodList);
            Admiration = aFoodSample.Admiration;
            Classe = aFoodSample.Classe;
            Esquive = aFoodSample.Esquive;
            Passion = aFoodSample.Passion;
            Volupte = aFoodSample.Volupte;
            EssenceAlreadyGiven = aFoodSample.EssenceAlreadyGiven;
        }

        /// <summary>
        /// Creates a sample from a list of food.
        /// </summary>
        /// <param name="aFoodBase"></param>
        /// <param name="aFoodList"></param>
        public FoodSample(FoodBase aFoodBase, List<Food> aFoodList)
        {
            _foodBase = aFoodBase;
            FoodList = new List<Food>(aFoodList);
            AddBaseStat();
            ComputeStats();
        }

        private void AddBaseStat()
        {
            Admiration += int.Parse(ConfigurationManager.AppSettings.Get("BaseStatAdmirationPlusEssenceScore"));
            Classe += int.Parse(ConfigurationManager.AppSettings.Get("BaseStatClassePlusEssenceScore"));
            Esquive += int.Parse(ConfigurationManager.AppSettings.Get("BaseStatEsquivenPlusEssenceScore"));
            Passion += int.Parse(ConfigurationManager.AppSettings.Get("BaseStatPassionPlusEssenceScore"));
            Volupte += int.Parse(ConfigurationManager.AppSettings.Get("BaseStatVoluptePlusEssenceScore"));
            EssenceAlreadyGiven = int.Parse(ConfigurationManager.AppSettings.Get("NumberOfEssenceGiven"));
        }
        
        /// <summary>
        /// Generates a sample of 33 food at random.
        /// Perf: ~ 500.000 samples/sec
        /// </summary>
        public void GenRandom()
        {
            this.Reset();

            for (int i = 0; i < 33 - EssenceAlreadyGiven; i++)
            {
                FoodList.Add(_foodBase[StaticRandom.Rand(0, _foodBase.Count)]);
            }
            ComputeStats();
        }

        /// <summary>
        /// Generates a sample of 33 food at random, but you can specify the minimum mean of the food stats.
        /// </summary>
        /// <param name="minMean">Mean of the food to not be under.</param>
        public void GenMean(double minMean)
        {
            this.Reset();
            double mean = -1;
            int rand = 0;
            for (int i = 0; i < 33 - EssenceAlreadyGiven; i++)
            {
                while (mean < minMean)
                {
                    rand = StaticRandom.Rand(0, _foodBase.Count);
                    mean = _foodBase[rand].Mean();
                }
                FoodList.Add(_foodBase[rand]);
            }
            ComputeStats();
        }

        /// <summary>
        /// DEBUG : Generates a perfect sample from Elixirs.
        /// </summary>
        public void GenEssence()
        {
            this.Reset();
            for (int i = 0; i < 33 - EssenceAlreadyGiven; i++)
            {
                FoodList.Add(new Food("Elixir", "Feu", 10, 10, 10, 10, 10));
            }
            ComputeStats();
        }

        /// <summary>
        /// Generates a sample of 33 foods with (about) the same amout of +10(+12) in each stats.
        /// Perf: ~ 90.000 samples/sec (175.000 samples/sec on a pruned base).
        /// </summary>
        public void GenFair()
        {
            bool admirationAdded = false;
            bool classeAdded = false;
            bool esquiveAdded = false;
            bool passionAdded = false;
            bool volupteAdded = false;
            int rand = 0;
            int i = 0;

            this.Reset();
            while (i < 33 - EssenceAlreadyGiven)
            {
                rand = StaticRandom.Rand(0, _foodBase.Count);
                if (!admirationAdded & _foodBase[rand].Admiration >= 10)
                {
                    admirationAdded = true;
                    FoodList.Add(_foodBase[rand]);
                    i++;
                    continue;
                }

                if (!classeAdded & _foodBase[rand].Classe >= 10)
                {
                    classeAdded = true;
                    FoodList.Add(_foodBase[rand]);
                    i++;
                    continue;
                }

                if (!esquiveAdded & _foodBase[rand].Esquive >= 10)
                {
                    esquiveAdded = true;
                    FoodList.Add(_foodBase[rand]);
                    i++;
                    continue;
                }

                if (!passionAdded & _foodBase[rand].Passion >= 10)
                {
                    passionAdded = true;
                    FoodList.Add(_foodBase[rand]);
                    i++;
                    continue;
                }

                if (!volupteAdded & _foodBase[rand].Volupte >= 10)
                {
                    volupteAdded = true;
                    FoodList.Add(_foodBase[rand]);
                    i++;
                    continue;
                }

                if (admirationAdded && classeAdded && esquiveAdded && passionAdded && volupteAdded)
                {
                    admirationAdded = false;
                    classeAdded = false;
                    esquiveAdded = false;
                    passionAdded = false;
                    volupteAdded = false;
                }
            }
            this.ComputeStats();
        }

        /// <summary>
        /// Generates a sample of 33 foods, adding each time the food that minimize the standard deviation of the sample.
        /// Best to use with a prunded food base.
        /// </summary>
        /// <param name="aAdmirationBase"></param>
        /// <param name="aClasseBase"></param>
        /// <param name="aEsquiveBase"></param>
        /// <param name="aPassionBase"></param>
        /// <param name="aVolupteBase"></param>
        public void GenSD(int aLookAhead = 0)
        {
            Food foodToAdd = null;
            double bestSD;
            int lookAhead = aLookAhead;
            int nbFoodToGenerate = 33 - EssenceAlreadyGiven;

            this.Reset();

            DateTime startTime = DateTime.Now;

            for (int i = 0; i < nbFoodToGenerate; i++)
            {
                // Reduce the lookahead near the end of the sample
                if (lookAhead != 0 && lookAhead > nbFoodToGenerate - i - 1) lookAhead--;
                bestSD = TestSampleWithLookahead(lookAhead);
                foodToAdd = _bestFood;

                // Add the best food to the sample
                Admiration += foodToAdd.Admiration;
                Classe += foodToAdd.Classe;
                Esquive += foodToAdd.Esquive;
                Passion += foodToAdd.Passion;
                Volupte += foodToAdd.Volupte;
                FoodList.Add(foodToAdd);

                Console.Write(foodToAdd.Nom);

                // Calculate and display the estimated remamining time
                if (i == 0)
                {
                    TimeSpan t = DateTime.Now - startTime;
                    TimeSpan t2 = new TimeSpan();
                    for (int j = 0; j < nbFoodToGenerate - 1; j++) t2 = t2.Add(t);
                    Console.SetCursorPosition(45, Console.CursorTop);
                    Console.Write($"Estimated time: {t2.Hours}h {t2.Minutes}m {t2.Seconds}s {t2.Milliseconds}ms");
                }

                Console.SetCursorPosition(25, Console.CursorTop);
                Console.WriteLine(bestSD);
            }
            // DONT COMPUTE THE STATS, THEY WERE ADDED IN THE LOOP
            Console.Clear();
        }

        /// <summary>
        /// Recursive function that add food to the sample and tests the new standard deviation, then remove the food.
        /// Stores the best food object into the class attribute _bestfood.
        /// </summary>
        /// <param name="aLookahead">How many food to lookahead to search the best SD</param>
        /// <returns>The best SD found.</returns>
        private double TestSampleWithLookahead(int aLookahead)
        {
            double returnSD = double.MaxValue;
            double currentSD;
            Food bestFood = null;

            foreach (Food f in _foodBase)
            {
                // Simulation add of the food to the sample
                Admiration += f.Admiration;
                Classe += f.Classe;
                Esquive += f.Esquive;
                Passion += f.Passion;
                Volupte += f.Volupte;

                // Compute the new SD
                if (aLookahead <= 0)
                {
                    currentSD = this.StandardDeviation();
                }
                else
                {
                    currentSD = TestSampleWithLookahead(aLookahead - 1);
                }

                // Remind the food if its the best
                if (currentSD < returnSD)
                {
                    returnSD = currentSD;
                    bestFood = f;
                }

                // Removing the food stat from the sample to test the next one
                Admiration -= f.Admiration;
                Classe -= f.Classe;
                Esquive -= f.Esquive;
                Passion -= f.Passion;
                Volupte -= f.Volupte;
            }

            if (aLookahead == 0) _bestFood = bestFood;
            return returnSD;
        }

        /// <summary>
        /// For the genetic algorithm, performs an uniform crossover between this and the parameter. Returns the child.
        /// </summary>
        /// <param name="parent2"></param>
        /// <returns></returns>
        public FoodSample UniformCrossover(FoodSample parent2)
        {
            List<Food> foodlist = new List<Food>();

            for (int i = 0; i < FoodList.Count; i++)
            {
                if (StaticRandom.Rand(0, 2) == 0)
                {
                    foodlist.Add(this.FoodList[i]);
                }
                else
                {
                    foodlist.Add(parent2.FoodList[i]);
                }
            }

            return new FoodSample(_foodBase, foodlist);
        }

        /// <summary>
        /// For the genetic algorithm, mutates the sample and compute the stats if needed.
        /// </summary>
        public void Mutate()
        {
            bool mutated = false;
            float mutationChance = float.Parse(ConfigurationManager.AppSettings.Get("GA_MutationChancePerLocus"));
            
            for (int i = 0; i < FoodList.Count; i++)
            {
                if ((float)(StaticRandom.Rand()) / int.MaxValue <= mutationChance)
                {
                    mutated = true;
                    FoodList[i] = _foodBase[StaticRandom.Rand(0, _foodBase.Count)];
                }
            }

            if (mutated)
            {
                Admiration = 0;
                Classe = 0;
                Esquive = 0;
                Passion = 0;
                Volupte = 0;
                AddBaseStat();
                ComputeStats();
            }
        }

        /// <summary>
        /// Returns the mean of the stats of the foods of the sample.
        /// </summary>
        public double Mean()
        {
            return ((double)Admiration + (double)Classe + (double)Esquive + (double)Passion + (double)Volupte) / 5.0;
        }

        /// <summary>
        /// Returns the standard deviation of the stats of the foods of the sample.
        /// </summary>
        public double StandardDeviation()
        {
            double mean = this.Mean();
            double variance = (Math.Pow((double)Admiration - mean, 2)
                + Math.Pow((double)Classe - mean, 2)
                + Math.Pow((double)Esquive - mean, 2)
                + Math.Pow((double)Passion - mean, 2)
                + Math.Pow((double)Volupte - mean, 2)
            ) / 4.0;
            return Math.Sqrt(variance);
        }

        /// <summary>
        /// Writes in the console the mean, the standard deviation of the sample, and the name of each food contained inside.
        /// </summary>
        public void PrintSample()
        {
            Console.WriteLine($"Admiration: {Admiration}");
            Console.WriteLine($"Classe: {Classe}");
            Console.WriteLine($"Esquive: {Esquive}");
            Console.WriteLine($"Passion: {Passion}");
            Console.WriteLine($"Admiration: {Volupte}");
            Console.WriteLine($"\nMean: {this.Mean()}");
            Console.WriteLine($"SD: {this.StandardDeviation()}\n");
            foreach (Food food in FoodList.OrderBy(x => x.Nom))
            {
                food.WriteColoredName();
                Console.WriteLine();
            }

        }

        public override string ToString()
        {
            return $"{Admiration} {Classe} {Esquive} {Passion} {Volupte}";
        }

        /// <summary>
        /// Updates the stats of the sample from the food it contains.
        /// </summary>
        private void ComputeStats()
        {
            foreach (Food f in FoodList)
            {
                Admiration += f.Admiration;
                Classe += f.Classe;
                Esquive += f.Esquive;
                Passion += f.Passion;
                Volupte += f.Volupte;
            }
        }

        /// <summary>
        /// Resets the sample (food list and mean/sd).
        /// </summary>
        private void Reset()
        {
            FoodList.Clear();
            Admiration = 0;
            Classe = 0;
            Esquive = 0;
            Passion = 0;
            Volupte = 0;
            AddBaseStat();
        }
    }
}
