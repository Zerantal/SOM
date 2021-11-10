/*using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections.Specialized;
using System.ComponentModel;
using MathLib.Matrices;
using Util;

namespace SomLibrary.Algorithms
{


    //[AlgorithmDetail("SOM", "The Original self-organising map")]
    [TODO("Create an AlgorithmDetail Attribute")]
    [Serializable]
    public partial class ModifiedPLSOM2 //: ISOM
    {

        [Serializable]
        public class DiameterBuffer
        {
            private double m_MaxDiameter = -1;
            private List<Vector> m_Buffer = new List<Vector>();

            public double MaxDiameter { get { return m_MaxDiameter; } }

            public void UpdateBuffer(Vector data)
            {
                //calculate the distance from the new input to all inputs in the buffer
                //find the largest distance between the input and any entry in the buffer
                //as well as the entry in the buffer that is closest to the input
                int minDistIndex = 0;
                double minDist = Double.MaxValue;
                double maxNewDist = 0;
                for (int i = 0; i < m_Buffer.Count; i++)
                {
                    double tmp = (m_Buffer[i] - data).Norm;
                    if (tmp < minDist)
                    {
                        //closest buffer entry so far
                        minDist = tmp;
                        minDistIndex = i;
                    }
                    if (tmp > maxNewDist)
                    {
                        //largest distance so far
                        maxNewDist = tmp;
                    }
                }
                //check if we've received a 'distant' input
                if (maxNewDist > m_MaxDiameter)
                {
                    //add the new input to the buffer
                    m_MaxDiameter = maxNewDist;
                    m_Buffer.Add(data);
                    //keep the buffer to the set size...
                    if (m_Buffer.Count > (data.Length + 1))
                    {
                        //...by removing the buffer entry that is closest to the new input
                        m_Buffer.RemoveAt(minDistIndex);
                    }
                }

            }
        }



        #region Fields (6)

        private const int DefaultDimension = 20;
        private IInputLayer m_Input = null;
        private bool m_IsTraining = false;
        private NeuronMap m_Mapping = null;
        private int m_UpdateInterval = 100;
        private double m_LastError;
        private DiameterBuffer m_DiameterBuffer;

        #endregion Fields

        #region Constructors (1)

        public ModifiedPLSOM2()
        {
            //set algorithms parameters to their lower bounds
            m_Mapping = new RectNeuronMap(2, 20, 20);
        }

        #endregion Constructors

        #region Properties (3)

        public IInputLayer InputReader
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                m_Input = value;
            }

            get
            {
                return m_Input;
            }
        }

        public NeuronMap Map
        {
            get { return m_Mapping; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                m_Mapping = value;
            }
        }

        public int ProgressInterval
        {
            get { return m_UpdateInterval; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "value has to be greater than or equal to one");

                m_UpdateInterval = value;
            }
        }

        #endregion Properties

        #region Delegates and Events (1)


        // Events (1) 

        [field: NonSerialized]
        public event EventHandler ProgressUpdate;


        #endregion Delegates and Events

        #region Methods (9)


        // Public Methods (5) 

        public void CancelTraining()
        {
            m_IsTraining = false;
        }

        public void Reset()
        {
            MaxEpoch = 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns>Node id of winning neuron</returns>      
        public int Simulate(Vector x, out double error)
        {
            if (x == null)
                throw new ArgumentNullException();
            if (m_Mapping == null)
                throw new InvalidOperationException("No mapping present");
            if (m_Mapping.InputDimension != x.Length)
                throw new SizeMismatchException("Input vector has the wrong dimension for map.");

            int c = 0;
            double dist, minDist = int.MaxValue;

            for (int i = 0; i < m_Mapping.MapSize; i++)
            {
                dist = (m_Mapping[i] - x).Norm;
                if (dist < minDist)
                {
                    c = i;
                    minDist = dist;
                }
            }

            error = minDist;
            return c;
        }


        /// <summary>
        /// Train SOM
        /// </summary>        
        public void Train()
        {
            int epoch;
            int c;  // index of winning node                       
            int updateOnIter = 1;
            int currentIter = 1;
            double error;

            // check whether algorithm is in valid state
            if (m_Input == null)
                throw new InvalidOperationException("No input source set");
            if (m_Input.InputDimension != m_Mapping.InputDimension)
                throw new InvalidOperationException("Mismatch between input vector length " +
                    "and map weight vector lengths");

            m_IsTraining = true;
            m_DiameterBuffer = new DiameterBuffer();

            for (epoch = 0; epoch < m_MaxEpoch; epoch++)
            {
                foreach (Vector x in m_Input)
                {
                    if (currentIter == updateOnIter)  // update event (for gui)
                    {
                        OnProgressUpdate(new EventArgs());
                        updateOnIter += m_UpdateInterval;
                        //System.Threading.Thread.Sleep(1000);
                    }

                    if (!m_IsTraining)    // abort algorithm if isTraining is set to false;
                        break;

                    //update diameter buffer
                    m_DiameterBuffer.UpdateBuffer(x);
                    // find winning neuron                   
                    c = Simulate(x, out error);
                    m_LastError = error;


                    UpdateWeights(x, c);        //******                     

                    currentIter++;
                }
                if (!m_IsTraining)
                    break;

            }
        }



        // Private Methods (4) 

        /// <summary>
        /// Simple gaussian function using 1 parameter to control the spread 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [TODO] // this should probably use some precalculated lookup table (but i'm feeling lazy)        
        private double NeighbourhoodFn(double distance, double epsilon)
        {
            double sigma = _beta * Math.Log(1 + epsilon * (Math.E - 1));
            //double sigma = _beta * epsilon;
            return (Math.Exp(-distance * distance / (sigma * sigma)));
        }

        private void OnProgressUpdate(System.EventArgs e)
        {
            if (ProgressUpdate != null)
                ProgressUpdate(this, e);
        }

        double CalcEpsilon(Vector x, int c)
        {
            double epsilon;

            if (m_LastError == 0)
                epsilon = 0;
            else
            {
                epsilon = m_LastError / m_DiameterBuffer.MaxDiameter;
                if (epsilon > 1)
                    epsilon = 1;
            }

            return epsilon;
        }

        /// <summary>      
        /// Neighbourhood of the winning neuron is bounded by a 2n sided regular polytope (where n 
        /// is the map dimensionality)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="winner"></param>                
        protected virtual void UpdateWeights(Vector x, int winner)
        {
            int kernel = 0; // nieghbourhood size
            double h = 1; // result of neighbourhood function
            double epsilon = CalcEpsilon(x, winner);

            while (h > Constants.EPSILON)
            {
                h = NeighbourhoodFn(kernel, epsilon);

                foreach (int n in m_Mapping.Neighbours(winner, kernel))
                    m_Mapping[n] += (epsilon * h * (x - m_Mapping[n]));
                kernel++;
                if (kernel == m_Mapping.MapSize) // fail safe check
                    break;
            }
        }

        #endregion Methods

    }
}
*/