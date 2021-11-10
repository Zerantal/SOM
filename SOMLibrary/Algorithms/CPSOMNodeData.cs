using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;

using Util;

namespace SomLibrary
{
    public class CPSOMNodeData
    {
        /*
        [ToDo("Remove quantization error from class")]
        private double m_ProbAssignment = 0;

        // temparature annealing parameter
        private double m_B = 0;
 
        // array to store a pre calculated neighbourhood function (index = neighbourhood radius)
        private double[] m_H = null;

        private double m_NormFactor = 1;

        public CPSOMNodeData()
        {
            throw new NotImplementedException();
        }

        public double Probability
        {
            get { return m_ProbAssignment; }
            set { m_ProbAssignment = value; }
        }

        public double B
        {
            get { return m_B; }
            set { m_B = value; }
        }
        /*
        public void SetNeighbourhoodFn(double[] H)
        {
            // Contract.Requires<ArgumentNullException>(H != null);            
            m_H = H;
            m_NormalisedH = new double[m_H.Length];
            Renormalise();            
        }

        public void Renormalise()
        {   
            for (int i = 0; i < m_NormalisedH.Length; i++)         
                m_NormalisedH[i] = m_H[i] / m_NormFactor;          
        }

        public double H(int size)
        {
            return m_NormalisedH[size];            
        }
*
        public double NormFactor
        {
            get { return m_NormFactor; }
            set { m_NormFactor = value; }
        }
         */
    }
}
