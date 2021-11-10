using System;
using System.Collections.Generic;
using System.Text;

namespace SomLibrary.Algorithms
{
    public partial class ExperimentalSOM
    {
        /*
        private double m_SigmaSquared;        
        private double m_Alpha;        
        protected int m_MaxEpoch = 5;

        #region Algorithm Parameters
        [SOMLibProperty("Alpha", "The initial training gain (valid values are between 0 and 1",
            LowerBound = 0, UpperBound = 1d)]
        public double Alpha
        {
            get { return m_Alpha; }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("Alpha", "Alpha value should be between 0 and 1");
                m_Alpha = value;
            }
        }

        [SOMLibProperty("Maximum Epoch", "The maximum training epoch is ignored for dynamic data sets",
    LowerBound = 1, UpperBound = 1000000)]
        public int MaxEpoch
        {
            get { return m_MaxEpoch; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "Max epoch has to be at least 1");
                m_MaxEpoch = value;
            }
        }


        [SOMLibProperty("Sigma", "Initial Radius of neighbourhood function. Sigma should be greater than 1",
            LowerBound = 0, UpperBound = 100000)]
        public double Sigma
        {
            get { return Math.Sqrt(m_SigmaSquared / 2); }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Sigma", "Sigma should be greater than or equal to 1");
                m_SigmaSquared = 2 * value * value;
            }
        }

        #endregion*/
    }
}
