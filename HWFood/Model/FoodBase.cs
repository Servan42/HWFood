using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWFood
{
    /// <summary>
    /// The list of all the kind of foods.
    /// </summary>
    class FoodBase : List<Food>
    {
        /// <summary>
        /// Create the object from the data found in a food CSV file.
        /// </summary>
        /// <param name="aPath">The path to the CSV food file.</param>
        public FoodBase(string aPath)
        {
            FillFromCSV(aPath);
        }

        /// <summary>
        /// Boost the stats of all the foods of a specific element.
        /// </summary>
        /// <param name="aElement">The element of the food type to boost.</param>
        public void AddElementBonus(string aElement)
        {
            foreach(Food f in this)
            {
                if(f.Element == aElement)
                {
                    f.ElementBoost();
                }
            }
        }

        /// <summary>
        /// Writes in the console the food names and the mean of their stats.
        /// </summary>
        public void PrintFoodMean()
        {
            foreach (Food f in this.OrderBy(x => x.Mean()))
            {
                f.WriteColoredName();
                Console.SetCursorPosition(25, Console.CursorTop);
                Console.WriteLine(f.Mean());
            }
        }

        /// <summary>
        /// Remove from the base the foods that do not a have a 10 stat.
        /// </summary>
        public void PruneWeakFood()
        {
            List<Food> foodToKeep = new List<Food>();
            foreach (Food f in this)
            {
                if(!(f.Admiration != 10 && f.Classe != 10 && f.Esquive != 10 && f.Passion != 10 && f.Volupte != 10))
                {
                    foodToKeep.Add(f);
                }
            }
            this.Clear();
            this.AddRange(foodToKeep);
        }

        /// <summary>
        /// Fills the object with the food data from the CSV.
        /// </summary>
        /// <param name="aPath">Path of the CSV File.</param>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="Exception"></exception>
        private void FillFromCSV(string aPath)
        {
            using (StreamReader sr = new StreamReader(aPath))
            {
                string currentLine;
                int lineNumber = 1;
                currentLine = sr.ReadLine();
                // Checking if the file is a CSV file.
                if (currentLine != null && !currentLine.Contains("Nom,Element,Admiration,Classe,Esquive,Passion,Volupte"))
                {
                    throw new FormatException("ERROR: The file is not a HW CSV file. Invalid header.");
                }
                // Reading the lines and adding them to the list.
                while ((currentLine = sr.ReadLine()) != null)
                {
                    // Skip empty lines & comments
                    if (String.IsNullOrEmpty(currentLine) || (currentLine != null && currentLine[0] == '#')) continue;
                    lineNumber++;
                    try
                    {
                        this.Add(new Food(currentLine));
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"ERROR while parsing the CSV on line {lineNumber}:");
                        throw e;
                    }
                }
            }
        }
    }
}
