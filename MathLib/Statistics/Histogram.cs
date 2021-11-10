using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace MathLib.Statistics
{
    /// <summary>
    /// Class for storing 1-Dimensional histogram data
    /// </summary>
    public class Histogram
    {
        /*
        private int m_NumberOfBins;
        private double m_Minimum;
        private double m_Maximum;
        private int [] m_HistogramBins;
        private int m_UnderFlow;
        private int m_OverFlow;
        private int m_DataCount;
        private double m_BinWidth;


        /// <summary>
        /// Initialise a new instance of the Histogram class, with the specified
        /// number of bins and range.
        /// </summary>
        /// <param name="numberOfBins">The number of bins in histogram.</param>
        /// <param name="minimum">The minimum value of the range covered by the histogram.</param>
        /// <param name="maximum">The maximum value of the range covered by the histogram.</param>
        public Histogram(int numberOfBins, double minimum, double maximum)
        {
            if (numberOfBins <= 0)
                throw new ArgumentOutOfRangeException("numberOfBins", "The number of bins must be greater than zero.");
            if (maximum <= minimum)
                throw new ArgumentException("Histogram minimum must be less than Histogram maximum");

            m_NumberOfBins = numberOfBins;
            m_Minimum = minimum;
            m_Maximum = maximum;
            m_HistogramBins = new int[m_NumberOfBins];
            m_UnderFlow = 0;
            m_OverFlow = 0;
            m_DataCount = 0;

            m_BinWidth = (m_Maximum - m_Minimum) / m_NumberOfBins;
        }

        /// <summary>
        /// Add numbers to histogram.
        /// </summary>
        /// <param name="number">The value to add to histogram.</param>
        public void AddValue(double number)
        {
            Bin b = RetrieveBin(number);
            if (b.IsUnderFlow)
                m_UnderFlow++;
            else if (b.IsOverFlow)
                m_OverFlow++;
            else
                m_HistogramBins[b.Index]++;

            m_DataCount++;

        }

        /// <summary>
        /// Retrieve the bin which specified number falls into.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private Bin RetrieveBin(double number)
        {
            Bin b = new Bin();

            b.IsOverFlow = false;
            b.IsUnderFlow = false;

            if (number < m_Minimum)
                b.IsUnderFlow = true;
            else if (number >= m_Maximum)
                b.IsOverFlow = true;
            else
            {
                b.Index = (int)Math.Floor((number - m_Minimum) / m_BinWidth);
            }

            return b;
        }

        /// <summary>
        /// Calculate the mean value as the average of the bin centres weighted by their occupancy.
        /// </summary>
        /// <returns></returns>
        public double Mean()
        {
            double sum = 0;
            double binCentre = m_BinWidth / 2; // bin centre of first bin

            for (int i = 0; i < m_NumberOfBins; i++)
            {
                sum += m_HistogramBins[i] * binCentre;
                binCentre += m_BinWidth;
            }

            return sum / (m_DataCount - m_OverFlow - m_UnderFlow);
        }

        /// <summary>
        /// Retrieves the total number of data points incorporated 
        /// in histogram
        /// </summary>
        public int DataCount
        {
            get { return m_DataCount; }
        }

        /// <summary>
        /// Retrieve the number of bins in the histogram.
        /// </summary>
        public int NumberOfBins
        {
            get { return m_NumberOfBins; }
        }

        /// <summary>
        /// Retrieve the minimum range for the histogram.
        /// </summary>       
        public double MinimumRange
        {
            get { return m_Minimum; }
        }

        /// <summary>
        /// Retrieve the maximum range for the histogram.
        /// </summary>
        public double MaximumRange
        {
            get { return m_Maximum; }
        }

        /// <summary>
        /// Retrieves the number of data points above the histograms
        /// maximum range.
        /// </summary>
        public int OverFlow
        {
            get { return m_OverFlow; }
        }

        /// <summary>
        /// Retrieves the number of data points below the histograms 
        /// minimum range.
        /// </summary>
        public int UnderFlow
        {
            get { return m_UnderFlow; }
        }

        public int[] BinContents
        {
            get { return m_HistogramBins; }
        }

    }

    internal struct Bin
    {
        public int Index;
        public bool IsUnderFlow;
        public bool IsOverFlow;
    }*/
    }
}