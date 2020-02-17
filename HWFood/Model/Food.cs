using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWFood
{
    /// <summary>
    /// Describe a piece of food.
    /// </summary>
    class Food
    {
        public string Nom { get; private set; }
        public string Element { get; private set; }
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
        public Food(string aNom, string aElement, int aAdmiration, int aClasse, int aEsquive, int aPassion, int aVolupte)
        {
            Nom = aNom;
            Element = aElement;
            Admiration = aAdmiration;
            Classe = aClasse;
            Esquive = aEsquive;
            Passion = aPassion;
            Volupte = aVolupte;
        }

        /// <summary>
        /// Reads a food list from a CSV file.
        /// </summary>
        /// <param name="aCSVLine">Path of the CSV file.</param>
        /// <exception cref="FormatException"></exception>
        public Food(string aCSVLine)
        {
            string[] splittedLine = aCSVLine.Split(',');

            if (splittedLine.Length != 7)
            {
                throw new FormatException("The CSV line number is incomplete, it must contain 7 members.");
            }

            foreach (string member in splittedLine)
            {
                if (String.IsNullOrWhiteSpace(member) || String.IsNullOrEmpty(member))
                {
                    throw new FormatException("ERROR: CSV line contains an empty (or whitespace) member.");
                }
            }

            Nom = splittedLine[0];
            Element = splittedLine[1];
            Admiration = int.Parse(splittedLine[2]);
            Classe = int.Parse(splittedLine[3]);
            Esquive = int.Parse(splittedLine[4]);
            Passion = int.Parse(splittedLine[5]);
            Volupte = int.Parse(splittedLine[6]);
        }

        /// <summary>
        /// Writes in the console the name of the food, with a color matching its element.
        /// </summary>
        public void WriteColoredName()
        {
            switch (Element)
            {
                case "Lumiere":
                    Tools.ConsoleWriteColor(Nom, ConsoleColor.Yellow);
                    break;
                case "Tenebre":
                    Tools.ConsoleWriteColor(Nom, ConsoleColor.DarkMagenta);
                    break;
                case "Eau":
                    Tools.ConsoleWriteColor(Nom, ConsoleColor.Blue);
                    break;
                case "Feu":
                    Tools.ConsoleWriteColor(Nom, ConsoleColor.Red);
                    break;
                case "Electricite":
                    Tools.ConsoleWriteColor(Nom, ConsoleColor.DarkYellow);
                    break;
                default:
                    Console.Write(Nom);
                    break;
            }
        }

        /// <summary>
        /// Returns the mean value of the stats of the food.
        /// </summary>
        public double Mean()
        {
            return ((double)Admiration + (double)Classe + (double)Esquive + (double)Passion + (double)Volupte) / 5.0;
        }

        public override string ToString()
        {
            return $"{Nom} {Element} {Admiration} {Classe} {Esquive} {Passion} {Volupte}";
        }

        /// <summary>
        /// Boost the all the stats of the food.
        /// </summary>
        public void ElementBoost()
        {
            Admiration = ElementBoost(Admiration);
            Classe = ElementBoost(Classe);
            Esquive = ElementBoost(Esquive);
            Passion = ElementBoost(Passion);
            Volupte = ElementBoost(Volupte);
        }

        /// <summary>
        /// Boost a specific stat of the food.
        /// </summary>
        /// <param name="aTrait">The stat to be boosted</param>
        /// <returns>The new value of the stat</returns>
        private int ElementBoost(int aTrait)
        {
            switch (aTrait)
            {
                case -5:
                    return -2;
                case -2:
                    return 1;
                case -1:
                    return 2;
                case 2:
                    return 3;
                case 5:
                    return 6;
                case 10:
                    return 12;
                default:
                    return aTrait;
            }
        }
    }
}
