using HWFood.Stats;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWFood
{
    class Program
    {
        static void Main(string[] args)
        {
            
            try {
                // Load the base from the CSV.
                FoodBase foodBase = new FoodBase(ConfigurationManager.AppSettings.Get("FoodBasePath"));
                // Remove all the food that do not have a stat to 10.
                foodBase.PruneWeakFood();
                // Bost the food depending on the element
                foodBase.AddElementBonus("Tenebre");
                //foodBase.PrintFoodMean();

                FoodSample sample = new FoodSample(foodBase);
                sample.GenSD(int.Parse(ConfigurationManager.AppSettings.Get("BaseStatAdmirationPlusEssenceScore")),
                    int.Parse(ConfigurationManager.AppSettings.Get("BaseStatClassePlusEssenceScore")),
                    int.Parse(ConfigurationManager.AppSettings.Get("BaseStatEsquivenPlusEssenceScore")),
                    int.Parse(ConfigurationManager.AppSettings.Get("BaseStatPassionPlusEssenceScore")),
                    int.Parse(ConfigurationManager.AppSettings.Get("BaseStatVoluptePlusEssenceScore")),
                    int.Parse(ConfigurationManager.AppSettings.Get("NumberOfEssenceGiven")),
                    int.Parse(ConfigurationManager.AppSettings.Get("GenSDLookAhead")));
                sample.PrintSample();

                //Statistics.BestSD(foodBase, 1000000);
                //Statistics.BestSD(foodBase, 1000000, 10);
                //Statistics.Constraints(foodBase, 1000000, 200, 200, 200, 200, 200);
                //Statistics.BestMeanAndSD(foodBase, 10000000);
                //Statistics.BestMeanAndSD(foodBase, new DateTime(2020,02,05,17,0,0));
                //Statistics.BestMeanAndSDThreaded(foodBase, new DateTime(2020,02,07,9,00,0),2);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }
    }
}
