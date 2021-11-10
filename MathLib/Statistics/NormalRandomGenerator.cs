using System;
using System.Collections.Generic;
using System.Text;

using Util;

namespace MathLib.Statistics
{
    public class NormalRandomGenerator : INumberGenerator
    {
        private readonly double _mean;
        private readonly double _standardDev;
        
        public NormalRandomGenerator(double mean = 0, double stddev = 1.0)
        {
            _mean = mean;
            _standardDev = stddev;            
        }

        public double Number
        {
            get
            {
                double u1 = StaticRandom.NextDouble();
                double u2 = StaticRandom.NextDouble();

                double z0 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
                return (_mean + z0 * _standardDev);
            }


        }

        #region IDeepCloneable<INumberGenerator> Members

        public INumberGenerator DeepClone()
        {
            return new NormalRandomGenerator(_mean, _standardDev);
        }

        #endregion
    }
}
