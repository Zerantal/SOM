using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

using MathLib;

namespace SomLibrary.Algorithms
{
    public sealed partial class GSOM
    {                
        private double _alpha = 0.4;
        private double _SF = 0.3;
        private int m_MaxEpoch = 30;        // Stopping epoch
        private double _FoD = 0.4;
        private int _growthPeriod = 5000;          // # iterations for growth period

        [OptionalField]
        private double _alphaDecayRate = 0.999;

        [SOMLibProperty("Alpha", "Initial training gain", 0, 1, 0.4)]
        public double Alpha
        {
            get { return _alpha; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value <= 1, 
                    //"Alpha should be between 0 and 1 inclusive.");
                
                _alpha = value;
            }
        }

        [SOMLibProperty("AlphaDecayRate", "Decay rate of alpha (used during the smoothing phase)", 0, 1, 0.999)]
        public double AlphaDecayRate
        {
            get { return _alphaDecayRate; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value <= 1,
                    //"Alpha should be between 0 and 1 inclusive.");

                _alphaDecayRate = value;
            }
        }

        [SOMLibProperty("Gamma", "Factor of Distribution for errors",
            0, 1, 0.4)]
        public double FactorOfDistribution
        {
            get { return _FoD; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value <= 1, 
                    //"FactorOfDistribution should be between 0 and 1 inclusive.");                
                if (_mapping != null)
                    _mapping.FactorOfDistribution = value;
                _FoD = value;
            }
        }
        
        [SOMLibProperty("Spread Factor", "", 0, 1, 0.3)]
        public double SpreadFactor
        {
            get { return _SF; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value <= 1,
                    //"SpreadFactor value must be between zero and one");

                _SF = value;                
            }
        }
        
        [SOMLibProperty("Growth Period", "", 1, 1000000, 5000)]
        public int GrowthPeriod
        {
            get { return _growthPeriod; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 1 && value <= 1000000,
                    //"SpreadFactor value must be between zero and one");

                _growthPeriod = value;
            }
        }
        
        [SOMLibProperty("Maximum Epoch", "The maximum training epoch is ignored for dynamic data sets",
            1, 1000000, 30)]
        public int MaxEpoch
        {
            get { return m_MaxEpoch; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 1,
                    //"Max epoch has to be at least 1");

                m_MaxEpoch = value;
            }
        }      
    }
}
