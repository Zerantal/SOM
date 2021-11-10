using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;

using Util;

namespace MathLib.Statistics
{
    public class UniformRandomGenerator : INumberGenerator
    {
        readonly double _min;
        readonly double _range;
        private readonly double _max;

        public UniformRandomGenerator(double min, double max)
        {
            // Contract.Requires(min <= max);
            // Contract.Requires((int)Math.Ceiling(min) <= (int)Math.Floor(max));
            
            _min = min;
            _max = max;
            _range = max - min;
        }

        public double Number
        {
            get 
            {
                return (StaticRandom.NextDouble()*_range) + _min;                 
            }
        }



        #region IDeepCloneable<INumberGenerator> Members

        public INumberGenerator DeepClone()
        {
            return new UniformRandomGenerator(_min, _max);
        }

        #endregion
    }
}
