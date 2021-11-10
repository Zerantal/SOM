using System;
using System.Collections.Generic;
using System.Text;

namespace SomLibrary.Algorithms
{
    public partial class SOMVariant2
    {
        /*
        private double m_SigmaSquared;
        private double m_RSigmaSquared;
        private double m_Alpha;
        private double m_Ralpha;
        protected int m_MaxEpoch = 5;
        private int m_OrderingTime;

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

        [SOMLibProperty("Alpha Decay Factor", "Alpha Decay Factor should be between 0 and 1. Setting to 1 results in " +
            "no decay while setting to 0 results in instantaneous decay", LowerBound = 0, UpperBound = 1d)]
        public double AlphaDecayFactor
        {
            get { return m_Ralpha; }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("AlphaDecayRate", "AlphaDecayRate should be between 0 and 1");
                m_Ralpha = value;
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

        [SOMLibProperty("Ordering Duration", "The duration (in iterations)" +
            " of the convergence and ordering phase",
            LowerBound = 0, UpperBound = 1000000)]
        public int OrderingDuration
        {
            get { return m_OrderingTime; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("OrderingDuration", "OrderingDuration should be be greater than zero");
                m_OrderingTime = value;
            }
        }

        [SOMLibProperty("Sigma", "Initial Radius of neighbourhood function. Sigma should be greater than 1",
            LowerBound = 1, UpperBound = 100000)]
        public double Sigma
        {
            get { return Math.Sqrt(m_SigmaSquared / 2); }

            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("Sigma", "Sigma should be greater than or equal to 1");
                m_SigmaSquared = 2 * value * value;
            }
        }

        [SOMLibProperty("Sigma Decay Factor", "Sigma Decay Factor should be between 0 and 1. Setting to 1 results in " +
            "no decay while setting to 0 results in instantaneous decay", LowerBound = 0, UpperBound = 1)]
        public double SigmaDecayFactor
        {
            get { return Math.Sqrt(m_RSigmaSquared); }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("SigmaDecayRate", "Sigma Decay Rate should be between 0 and 1");
                m_RSigmaSquared = value * value;
            }
        }

        #endregion
         * */
    }
}
