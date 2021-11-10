using System;
using System.Collections.Generic;
using System.Text;

using MathLib;

namespace SomLibrary.Algorithms
{
    public partial class CPSOM 
    {
        /*
        // algorithm parameters (with reasonable defaults)
        private double m_BetaStart = 100;
        private double m_BetaFinal = 1000;
        private double m_Delta1 = 20;
        private double m_Delta2 = 0.1;
        private double m_GammaFinal = 1;
        private double m_GammaStart = 10;
        private int m_Lambda1 = 100;
        private int m_Lambda2 = 100;
        private int m_Lambda3 = 100000;
        private int m_MaxEpoch = 30;
        private double m_Sigma = 0.5;
        private int[] m_MapDims = { DefaultDimension, DefaultDimension };       

        #region Algorithm Parameters

        [SOMLibProperty("Beta Final", "Final inverse temperature value",
            LowerBound = 1, UpperBound = 100000d)]
        public double BetaFinal
        {
            get { return m_BetaFinal; }
            set
            {
                if (value < 1 || value > 100000d || value < m_BetaStart)
                    throw new ArgumentOutOfRangeException("BetaFinal", "Beta Final should be " +
                        "greater than BetaStart and between 1 and 100000");

                m_BetaFinal = value;
            }
        }

        // properties to control the algorithm
        [SOMLibProperty("Beta Start", "Initial inverse temperature value",
            LowerBound = 1, UpperBound = 100000d)]
        public double BetaStart
        {
            get { return m_BetaStart; }
            set
            {
                if (value < 1 || value > 100000d || value > m_BetaFinal)
                    throw new ArgumentOutOfRangeException("BetaStart", "BetaStart should be " +
                        "less than BetaFinal and between 1 and 100000");
                m_BetaStart = value;
            }
        }

        [SOMLibProperty("Delta1", "Beta increment value",
            LowerBound = 0.000001, UpperBound = 10000d)]
        public double Delta1
        {
            get { return m_Delta1; }
            set
            {
                if (value < 0.000001d || value > 10000d)
                    throw new ArgumentOutOfRangeException("Delta1", "Delta1 should be greater than zero " +
                        "and less than 10000");
                m_Delta1 = value;
            }
        }

        [SOMLibProperty("Delta2", "Gamma decrement value",
            LowerBound = 0.000001, UpperBound = 10000d)]
        public double Delta2
        {
            get { return m_Delta2; }
            set
            {
                if (value < 0.000001d || value > 10000d)
                    throw new ArgumentOutOfRangeException("Delta2", "Delta2 should be greater than zero " +
                        "and less than 10000");
                m_Delta2 = value;
            }
        }

        [SOMLibProperty("Gamma Final", "Final forgetting value",
            LowerBound = 1, UpperBound = 1000d)]
        public double GammaFinal
        {
            get { return m_GammaFinal; }
            set
            {
                if (value < 1 || value > 1000d || value > m_GammaStart)
                    throw new ArgumentOutOfRangeException("GammaFinal", "GammaFinal should be " +
                    "less than GammaStart and between 0 and 1000");
                m_GammaFinal = value;
            }
        }

        [SOMLibProperty("Gamma Start", "Initial forgetting value",
            LowerBound = 1, UpperBound = 1000d)]
        public double GammaStart
        {
            get { return m_GammaStart; }
            set
            {
                if (value < 1 || value > 1000d || value < m_GammaFinal)
                    throw new ArgumentOutOfRangeException("GammaStart", "GammaStart should be " +
                        "greater than GammaFinal and between 0 and 1000");
                m_GammaStart = value;
            }
        }

        [SOMLibProperty("Lambda1", "Beta increment interval",
            LowerBound = 1, UpperBound = 10000)]
        public int Lambda1
        {
            get { return m_Lambda1; }
            set
            {
                if (value < 1 || value > 10000)
                    throw new ArgumentOutOfRangeException("Lambda1", "Lambda1 should be " +
                        "between 0 and 10000");
                m_Lambda1 = value;
            }
        }

        [SOMLibProperty("Lambda2", "Gamma decrement interval",
            LowerBound = 1, UpperBound = 10000)]
        public int Lambda2
        {
            get { return m_Lambda2; }
            set
            {
                if (value < 1 || value > 10000)
                    throw new ArgumentOutOfRangeException("Lambda2", "Lambda2 should be " +
                        "between 0 and 10000");
                m_Lambda2 = value;
            }
        }

        [SOMLibProperty("Lambda3", "Gamma reset interval",
            LowerBound = 1, UpperBound = 10000000)]
        public int Lambda3
        {
            get { return m_Lambda3; }
            set
            {
                if (value < 1 || value > 10000000)
                    throw new ArgumentOutOfRangeException("Lambda3", "Lambda3 should be " +
                        "between 1 and 10000000");
                m_Lambda3 = value;
            }
        }

        [SOMLibProperty("Maximum Epoch", "Maximum epoch",
            LowerBound = 1, UpperBound = 1000000)]
        public int MaxEpoch
        {
            get { return m_MaxEpoch; }
            set
            {
                if (value < 1 || value > 1000000)
                    throw new ArgumentOutOfRangeException("value", "Max epoch has to be " +
                        "between 1 and 1000000");
                m_MaxEpoch = value;
            }
        }

        [SOMLibProperty("Sigma", "Initial spread factor for the neighbourhood function ( >= 1)",
            LowerBound = Constants.Epsilon, UpperBound = 100000)]
        public double Sigma
        {
            get { return m_Sigma; }

            set
            {
                if (value < Constants.Epsilon)
                    throw new ArgumentOutOfRangeException("Sigma", "Sigma has to be greater than zero");
                m_Sigma = value;
            }
        }     
        #endregion
      */
    }
}
