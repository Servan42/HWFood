using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HWFood.Stats
{
    /// <summary>
    /// Static methods to do statistics.
    /// </summary>
    class Statistics
    {
        /// <summary>
        /// Writes in the console the best food sample based on the standard deviantion.
        /// </summary>
        /// <param name="aFoodbase">Food data.</param>
        /// <param name="aNb">Number of iterations.</param>
        public static void BestSD(FoodBase aFoodbase, int aNb)
        {
            FoodSample workingFoodSample = new FoodSample(aFoodbase);
            FoodSample bestFoodSample = null;
            double bestSD = Double.MaxValue;
            double SD;
            for (int i = 0; i < aNb; i++)
            {
                GenFoodSample(workingFoodSample);
                SD = workingFoodSample.StandardDeviation();
                if (Math.Abs(SD) < Math.Abs(bestSD))
                {
                    bestSD = SD;
                    bestFoodSample = new FoodSample(workingFoodSample);
                }
            }
            bestFoodSample.PrintSample();
        }

        /// <summary>
        /// Writes in the console the best food sample based on the standard deviantion.
        /// The minimum mean of the food to use can be specified.
        /// </summary>
        /// <param name="aFoodbase">Food data.</param>
        /// <param name="aNb">Number of iterations.</param>
        /// <param name="aMean">Minimum mean of the food to use.</param>
        public static void BestSD(FoodBase aFoodbase, int aNb, double aMean)
        {
            FoodSample workingFoodSample = new FoodSample(aFoodbase);
            FoodSample bestFoodSample = null;
            double bestSD = Double.MaxValue;
            double SD;
            for (int i = 0; i < aNb; i++)
            {
                workingFoodSample.GenMean(aMean);
                SD = workingFoodSample.StandardDeviation();
                if (Math.Abs(SD) < Math.Abs(bestSD))
                {
                    bestSD = SD;
                    bestFoodSample = new FoodSample(workingFoodSample);
                }
            }
            bestFoodSample.PrintSample();
        }

        /// <summary>
        /// Writes in the console the foods sample that respects the constraints in parameter.
        /// If aMaxNb is not 0, sets a number of iterations before timeout.
        /// </summary>
        /// <param name="aFoodbase">Food data</param>
        /// <param name="aMaxNb">Number of iterations before timeout. 0 for infinite.</param>
        /// <param name="aAdmiration"></param>
        /// <param name="aClasse"></param>
        /// <param name="aEsquive"></param>
        /// <param name="aPassion"></param>
        /// <param name="aVolupte"></param>
        public static void Constraints(FoodBase aFoodbase, int aMaxNb, int aAdmiration, int aClasse, int aEsquive, int aPassion, int aVolupte)
        {
            FoodSample workingFoodSample = new FoodSample(aFoodbase);
            bool timeout = false;
            int i = 0;
            do
            {
                GenFoodSample(workingFoodSample);
                i++;
                if (aMaxNb != 0 && i > aMaxNb) timeout = true;
            } while ((workingFoodSample.Admiration <= aAdmiration
            || workingFoodSample.Classe <= aClasse
            || workingFoodSample.Esquive <= aEsquive
            || workingFoodSample.Passion <= aPassion
            || workingFoodSample.Volupte <= aVolupte)
            && !timeout);
            workingFoodSample.PrintSample();
        }

        /// <summary>
        /// Writes in the console the best sample based on higher mean and lower standard deviation.
        /// </summary>
        /// <param name="aFoodbase">Food data</param>
        /// <param name="aNb">Iterations number before sropping.</param>
        public static void BestMeanAndSD(FoodBase aFoodbase, int aNb)
        {
            FoodSample workingFoodSample = new FoodSample(aFoodbase);
            FoodSample bestFoodSample = null;
            double bestSD = Double.MaxValue;
            double bestMean = 0;
            double SD;
            double mean;
            for (int i = 0; i < aNb; i++)
            {
                GenFoodSample(workingFoodSample);
                SD = workingFoodSample.StandardDeviation();
                mean = workingFoodSample.Mean();
                if ((Math.Abs(SD) < Math.Abs(bestSD)) && (mean > bestMean))
                {
                    bestSD = SD;
                    bestMean = mean;
                    Console.WriteLine($"{SD} {mean}");
                    bestFoodSample = new FoodSample(workingFoodSample);
                }
            }
            Console.Clear();
            bestFoodSample.PrintSample();
        }

        /// <summary>
        /// Writes in the console the best sample based on higher mean and lower standard deviation.
        /// </summary>
        /// <param name="aFoodbase">Food data.</param>
        /// <param name="aEndTime">Date when to stop processing.</param>
        public static void BestMeanAndSD(FoodBase aFoodbase, DateTime aEndTime)
        {
            FoodSample workingFoodSample = new FoodSample(aFoodbase);
            FoodSample bestFoodSample = null;
            double bestSD = Double.MaxValue;
            double bestMean = 0;
            double SD;
            double mean;
            while (DateTime.Now < aEndTime)
            {
                GenFoodSample(workingFoodSample);
                SD = workingFoodSample.StandardDeviation();
                mean = workingFoodSample.Mean();
                if ((Math.Abs(SD) < Math.Abs(bestSD)) && (mean > bestMean))
                {
                    bestSD = SD;
                    bestMean = mean;
                    Console.WriteLine($"{SD} {mean}");
                    bestFoodSample = new FoodSample(workingFoodSample);
                }
            }
            Console.Clear();
            bestFoodSample.PrintSample();
        }

        /// <summary>
        /// Writes in the console the best sample based on higher mean and lower standard deviation.
        /// The search is multithrreaded. Sepcify 0 to let the computer take the maximum efficient number of threads based on the CPU. 
        /// </summary>
        /// <param name="aFoodBase">Food data</param>
        /// <param name="aEndTime">Date when to stop processing.</param>
        /// <param name="aThreadNumber">Numer of threads. 0 = auto</param>
        public static void BestMeanAndSDThreaded(FoodBase aFoodBase, DateTime aEndTime, int aThreadNumber)
        {
            int threadNumber = aThreadNumber == 0 ? Environment.ProcessorCount : aThreadNumber;
            List<Thread> threadList = new List<Thread>();
            List<FoodSample> bestFoodSampleList = new List<FoodSample>();
            object lockBestFoodSampleList = new object();
            object lockConsole = new object();

            FoodSample bestFoodSampleEnd = null;
            double bestSDEnd = Double.MaxValue;
            double bestMeanEnd = 0;
            double SDEnd;
            double meanEnd;

            Console.CursorVisible = false;
            Console.SetWindowSize(threadNumber*15, Console.WindowHeight);

            // Thread code
            Action<object> threadCode = (object aThreadId) =>
            {
                FoodSample workingFoodSample = new FoodSample(aFoodBase);
                FoodSample bestFoodSample = null;
                double bestSD = Double.MaxValue;
                double bestMean = 0;
                double SD;
                double mean;
                int consoleLineNumber = 0;
                int consoleColumnNumber = (int)aThreadId;

                // Search for the best sample until time is over
                while (DateTime.Now < aEndTime)
                {
                    GenFoodSample(workingFoodSample);
                    SD = workingFoodSample.StandardDeviation();
                    mean = workingFoodSample.Mean();
                    if ((Math.Abs(SD) < Math.Abs(bestSD)) && (mean > bestMean))
                    {
                        bestSD = SD;
                        bestMean = mean;
                        lock (lockConsole)
                        {
                            Console.SetCursorPosition((consoleColumnNumber*15), consoleLineNumber);
                            Console.Write($"{SD.ToString("N2")} {mean.ToString("N2")}  ");
                            consoleLineNumber++;
                        }
                        bestFoodSample = new FoodSample(workingFoodSample);
                    }
                }

                // Save the best sample for this thread
                lock (lockBestFoodSampleList)
                {
                    bestFoodSampleList.Add(bestFoodSample);
                }
            };

            // Create and start the threads
            for (int i = 0; i < threadNumber; i++) threadList.Add(new Thread(new ParameterizedThreadStart(threadCode)));
            for (int i = 0; i < threadNumber; i++) threadList[i].Start((object)i);
            foreach (Thread t in threadList) t.Join();

            // Select the best sample of all the threads
            foreach(FoodSample fs in bestFoodSampleList)
            {
                SDEnd = fs.StandardDeviation();
                meanEnd = fs.Mean();
                if ((Math.Abs(SDEnd) < Math.Abs(bestSDEnd)) && (meanEnd > bestMeanEnd))
                {
                    bestSDEnd = SDEnd;
                    bestMeanEnd = meanEnd;
                    bestFoodSampleEnd = new FoodSample(fs);
                }
            }
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = true;
            Console.Clear();
            bestFoodSampleEnd.PrintSample();

        }

        /// <summary>
        /// Calls generation function (can be modified within the function)
        /// </summary>
        /// <param name="aWorkingFoodSample"></param>
        private static void GenFoodSample(FoodSample aWorkingFoodSample)
        {
            //aworkingFoodSample.GenRandom();
            aWorkingFoodSample.GenFair();
        }
    }
}
