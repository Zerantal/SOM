using System;
using System.Collections.Generic;
using System.Text;

namespace MathLib.Statistics
{
    public class UniformRandomGenerator : INumberGenerator
    {
        double Min,range;
        int MinInt, MaxInt;
        Random r;

        public UniformRandomGenerator(double min, double max)
        {
            Min = min;            
            range = max - min;
            MinInt = (int)Math.Ceiling(min);
            MaxInt = (int)Math.Floor(max);
            r = new Random();            
        }

        public Int32 Int32Number
        {
            get { return r.Next(MinInt, MaxInt); }
        }

        public double Number
        {
            get 
            {
                return (r.NextDouble()*range) + Min;                 
            }
        }


    }
}
