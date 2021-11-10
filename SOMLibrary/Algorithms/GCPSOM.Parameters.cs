using System;
using System.Collections.Generic;
using System.Text;
using Util;

namespace SomLibrary.Algorithms
{
    public partial class GCPSOM
    {/*
        private double m_SF;                        
        private double m_FD;
        private double m_Beta;
        private int m_MaxEpoch = 30;
        
        private int m_Lambda;        
        private double m_GammaStart;               
        private double m_Sigma;


        ////////////// algorithm parameters /////////////////////////////
        [SOMLibProperty("Spread Factor", "", LowerBound = 0, UpperBound = 1)]
        public double SpreadFactor
        {
            get { return m_SF; }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("SpreadFactor", "Spread Factor must be between 0 and 1");
                m_SF = value;
            }
        }   

        [SOMLibProperty("FactorOfDistribution", "Factor of Distribution for errors",
            LowerBound = Constants.EPSILON, UpperBound = 1 - Constants.EPSILON)]
        public double FactorOfDistribution
        {
            get { return m_FD; }
            set
            {
                if (value <= 0 || value >= 1)
                    throw new ArgumentOutOfRangeException("Gamma", "Gamma must be between 0 and 1");
                if (m_Map != null)
                    m_Map.FactorOfDistribution = value;
                m_FD = value;
            }
        }

        [SOMLibProperty("Gamma Start", "Initial forgetting value",
            LowerBound = 1, UpperBound = 1000d)]
        public double GammaStart
        {
            get { return m_GammaStart; }
            set
            {
                if (value < 1 || value > 1000d)
                    throw new ArgumentOutOfRangeException("GammaStart", "GammaStart should be " +
                        "greater than GammaFinal and between 0 and 1000");
                m_GammaStart = value;
                m_GammaDecrement = (m_GammaStart - 1) / 2 / m_Lambda * 3 * GammaAdjustmentInterval;
            }
        }

        [SOMLibProperty("Beta", "Annealing constant",
            LowerBound = Constants.EPSILON, UpperBound = 100000)]
        public double Beta
        {
            get { return m_Beta; }
            set
            {
                if (value <= 0 || value > 100000 )
                    throw new ArgumentOutOfRangeException("Beta", "Beta must be between 0 and 100000");
                m_Beta = value;
            }
        }
        
        [SOMLibProperty("Lambda", "Forgetting cycle length",
            LowerBound = 1, UpperBound = 10000000)]
        public int Lambda
        {
            get { return m_Lambda; }
            set
            {
                if (value < 1 || value > 10000000)
                    throw new ArgumentOutOfRangeException("Lambda", "Lambda2 should be " +
                        "between 1 and 10000000");
                m_Lambda = value;
                m_GammaDecrement = (m_GammaStart - 1) / 2 / m_Lambda * 3 * GammaAdjustmentInterval;
            }
        }

        [SOMLibProperty("Sigma", "Initial spread factor for the neighbourhood function ( >= 1)",
            LowerBound = Constants.EPSILON, UpperBound = 100000)]
        public double Sigma
        {
            get { return m_Sigma; }

            set
            {
                if (value < Constants.EPSILON)
                    throw new ArgumentOutOfRangeException("Sigma", "Sigma has to be greater than zero");
                m_Sigma = value;
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
        }                    */
    }
}
