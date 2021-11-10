using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MathLib.Matrices;

namespace SomLibrary
{
    public enum IteratorType { Random, Sequential };

    /// <summary>
    /// abstract base class for classes implementing an offline InputLayer.
    /// (currently, it is assumed that any derived classes will load data sets 
    /// into the buffer in their entirety)
    /// </summary>
    public abstract class BufferedInputLayer : IInputLayer
    {

		#region Fields (5) 

        protected int m_CurrentVector = 0;
        protected Matrix m_Data;
        protected String[] m_Labels;
        protected int m_NumberOfVectors = 0;
        private IteratorType m_VectorEnum = IteratorType.Random;

		#endregion Fields 

		#region Properties (4) 

        public int CurrentPosition
        {
            get { return m_CurrentVector; }
        }

        public IEnumerator<Vector> RandomEnum
        {
            get 
            {
                // iterate randomly over the input patterns
                Random r = new Random();
                int[] randomIndices = new int[m_Data.ColumnSize];
                int i, newIndex, tmp;
                for (i = 0; i < m_Data.ColumnSize; i++)
                    randomIndices[i] = i;
                for (i = 0; i < m_Data.ColumnSize; i++)
                {
                    //swap random indices
                    newIndex = r.Next(m_Data.ColumnSize);
                    tmp = randomIndices[i];
                    randomIndices[i] = randomIndices[newIndex];
                    randomIndices[newIndex] = tmp;                    
                }

                for (i = 0; i < m_Data.ColumnSize; i++)
                    yield return m_Data[randomIndices[i]];
            } 
        }

        /// <summary>
        /// Return data in the order that they appear in the input buffer
        /// </summary>
        public IEnumerator<Vector> SequentialEnum
        {
            get
            {
                m_CurrentVector = 0;
                foreach (Vector v in m_Data.ColEnum)
                {
                    yield return v;
                    m_CurrentVector++;
                }
            }
        }

        public IteratorType VectorIter
        {
            get { return m_VectorEnum; }
            set { m_VectorEnum = value; }
        }

		#endregion Properties 

		#region Methods (3) 


		// Public Methods (2) 

        public IEnumerator<Vector> GetEnumerator()
        {
            if (m_VectorEnum == IteratorType.Random)
                return RandomEnum;
            else
                return SequentialEnum; 
        }

        public Vector[] Sample(int startPos, int lastPos)
        {
            if (startPos > lastPos)
                return null;
            if (startPos < 0) startPos = 0;
            if (lastPos >= m_NumberOfVectors) lastPos =  m_NumberOfVectors - 1;

            Vector[] retSample = new Vector[lastPos - startPos+1];

            int j = 0;
            for (int i = startPos; i <= lastPos; i++)
            {
                retSample[j++] = new Vector(m_Data[i]);
            }

            return retSample;
        }



		// Private Methods (1) 

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(); 
        }


		#endregion Methods 

        int IInputLayer.InputDimension
        {
            get { return m_Data.RowSize; }
        }

    }
}
