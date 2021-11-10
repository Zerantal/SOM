using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Util
{
    // static, threadsafe wrapper for Random class
    static public class StaticRandom
    {
        static private readonly Random R = new Random();
        static private readonly object ObjLock = new object();

        public static int Next()
        {
            // Contract.Ensures(// Contract.Result<int>() >= 0);

            lock (ObjLock)
            {
                return R.Next();
            }
        }

        public static int Next(int maxValue)
        {
            // Contract.Requires(maxValue >= 0);
            // Contract.Ensures(// Contract.Result<int>() >= 0);
            // Contract.Ensures(// Contract.Result<int>() < maxValue || maxValue == 0 && // Contract.Result<int>() == 0);
            
            lock (ObjLock)
            {
                return R.Next(maxValue);
            }
        }

        public static int Next(int minValue, int maxValue)
        {
            // Contract.Requires(minValue <= maxValue);
            // Contract.Ensures(// Contract.Result<int>() >= minValue);
            // Contract.Ensures(// Contract.Result<int>() < maxValue ||
                             //maxValue == minValue && // Contract.Result<int>() == minValue);
            lock (ObjLock)
            {
                return R.Next(minValue, maxValue);
            }
        }

        public static void NextBytes(byte[] buffer)
        {
            // Contract.Requires(buffer != null);

            lock (ObjLock)
            {
                R.NextBytes(buffer);
            }
        }

        public static double NextDouble()
        {
            lock (ObjLock)
            {
                return R.NextDouble();
            }
        }
    }
}
