using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;

namespace SomLibrary.Algorithms
{
    public partial class SOM
    {
        private double m_SigmaSquared;
        private double m_RSigmaSquared;
        private double m_Alpha;
        private double m_Ralpha;
        protected int m_MaxEpoch = 5;

        #region Algorithm Parameters
        [SOMLibProperty("Alpha", "The initial training gain (valid values are between 0 and 1",
            0, 1d, 0.4)]
        public double Alpha
        {
            get { return m_Alpha; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 0, "Alpha must be greater than or equal to zero");
                // Contract.Requires<ArgumentOutOfRangeException>(value <= 1d, "Alpha must be less than or equal to 1");
                
                m_Alpha = value;
            }
        }

        [SOMLibProperty("Alpha Decay Factor", "Alpha Decay Factor should be between 0 and 1. Setting to 1 results in " +
            "no decay while setting to 0 results in instantaneous decay",
            0, 1d, 0.9995)]
        public double AlphaDecayFactor
        {
            get { return m_Ralpha; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 0, 
                    //"AlphaDecayFactor must be greater than or equal to zero");
                // Contract.Requires<ArgumentOutOfRangeException>(value <= 1d, 
                    //"AlphaDecayFactor must be less than or equal to 1");

                m_Ralpha = value;
            }
        }

        [SOMLibProperty("Maximum Epoch", "The maximum training epoch is ignored for dynamic data sets",
            1, 1000000, 5)]
        public int MaxEpoch
        {
            get { return m_MaxEpoch; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 1, "MaxEpoch must be greater than or equal to 1");
                
                m_MaxEpoch = value;
            }
        }

        [SOMLibProperty("Sigma", "Initial Radius of neighbourhood function. Sigma should be greater than 1",
            0, 100000, 20)]
        public double Sigma
        {
            get { return Math.Sqrt(m_SigmaSquared / 2); }

            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 1, 
                    //"Sigma must be greater than or equal to 0");
                
                m_SigmaSquared = 2 * value * value;
            }
        }

        [SOMLibProperty("Sigma Decay Factor", "Sigma Decay Factor should be between 0 and 1. Setting to 1 results in " +
            "no decay while setting to 0 results in instantaneous decay",
            0, 1, 0.999)]
        public double SigmaDecayFactor
        {
            get { return Math.Sqrt(m_RSigmaSquared); }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 0,
                    //"SigmaDecayFactor must be greater than or equal to zero");
                // Contract.Requires<ArgumentOutOfRangeException>(value <= 1,
                    //"SigmaDecayFactor must be less than or equal to 1");
                
                m_RSigmaSquared = value * value;
            }
        }

        #endregion
    }
}
