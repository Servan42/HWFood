using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWFood
{
    public static class Tools
    {
        /// <summary>
        /// Writes a message in the console in the specified color.
        /// </summary>
        /// <param name="aOutput"></param>
        /// <param name="aColor"></param>
        public static void ConsoleWriteColor(string aOutput, ConsoleColor aColor)
        {
            Console.ForegroundColor = aColor;
            Console.Write(aOutput);
            Console.ResetColor();
        }
    }
}
