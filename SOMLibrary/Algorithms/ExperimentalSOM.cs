/*
using System;
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
    public partial class ExperimentalSOM : ISOM
    {

        #region Fields (6)

        private const int DefaultDimension = 20;
        private IInputLayer m_Input = null;
        private bool m_IsTraining = false;
        private NeuronMap m_Mapping = null;
        private int m_UpdateInterval = 100;

        #endregion Fields

        #region Constructors (1)

        public ExperimentalSOM()
        {
            //set algorithms parameters to their lower bounds
            Alpha = 0;           
            Sigma = 1;          
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
            Alpha = 0.1;
            Sigma = 0.5;
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

            // create initial weightings
            
            RectNeuronMap map = m_Mapping as RectNeuronMap;
            int i = 0;
            for (int y = 0; y < map.YDim; y++)
                for (int x = 0; x < map.XDim; x++,i++)
                {
                    map[i] = new Vector(new double[] { (double)x / map.XDim, (double) y / map.YDim });
                }
            
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


                    // find winning neuron                   
                    c = Simulate(x, out error);

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
        private double NeighbourhoodFn(double distance)
        {
            return (Math.Exp(-distance * distance / m_SigmaSquared));
        }

        private void OnProgressUpdate(System.EventArgs e)
        {
            if (ProgressUpdate != null)
                ProgressUpdate(this, e);
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

            while (h > Constants.EPSILON)
            {
                h = NeighbourhoodFn(kernel);

                foreach (int n in m_Mapping.Neighbours(winner, kernel))
                    m_Mapping[n] += (m_Alpha * h * (x - m_Mapping[n]));
                kernel++;
                if (kernel == m_Mapping.MapSize) // fail safe check
                    break;
            }
        }

        #endregion Methods

    }
}
*/