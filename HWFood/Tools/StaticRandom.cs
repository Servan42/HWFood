using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HWFood
{
    /// <summary>
    /// Static class that let you use pseudo-random number generation in a multithreaded environement.
    /// </summary>
    class StaticRandom
    {
        static int seed = Environment.TickCount;

        static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        //static readonly ThreadLocal<Random> random =
        //new ThreadLocal<Random>(() => new Random(0));

        public static int Rand()
        {
            return random.Value.Next();
        }

        public static int Rand(int aDeb, int aFin)
        {
            return random.Value.Next(aDeb, aFin);
        }
    }
}
