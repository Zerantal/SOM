using System;
using System.Collections.Generic;
using System.Text;

namespace MathLib.Statistics
{
    public class NormalRandomGenerator : INumberGenerator
    {
        private double Mean = 0;
        private double StandardDev = 1;
        Random r;

        public NormalRandomGenerator(double mean, double stddev)
        {
            Mean = mean;
            StandardDev = stddev;
            r = new Random();
        }

        public Int32 Int32Number
        {
            get { return (Int32)Number; }
        }

        public double Number
        {
            get
            {
                double z0;
                double u1, u2;
                u1 = r.NextDouble();
                u2 = r.NextDouble();

                z0 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
                return Mean + z0*StandardDev;
            }


        }
    }
}
