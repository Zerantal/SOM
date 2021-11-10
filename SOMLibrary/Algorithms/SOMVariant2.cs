using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections.Specialized;
using System.ComponentModel;
using MathLib;
using MathLib.Matrices;
using Util;

namespace SomLibrary.Algorithms
{
    //[AlgorithmDetail("SOM", "The Original self-organising map")]
    [ToDo("Create an AlgorithmDetail Attribute")]
    [Serializable]
    public partial class SOMVariant2 //: ISOM
    {
        /*
        #region Fields (6)

        private const int DefaultDimension = 20;
        private IInputLayer m_Input = null;
        private bool m_IsTraining = false;
        private NeuronMap m_Mapping = null;
        private int m_UpdateInterval = 100;

        #endregion Fields

        #region Constructors (1)

        public SOMVariant2()
        {
            //set algorithms parameters to their lower bounds
            Alpha = 0;
            AlphaDecayFactor = 0;
            Sigma = 1;
            SigmaDecayFactor = 0;
            OrderingDuration = 0;
            new RectNeuronMap(2, 20, 20);
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
            Alpha = 0.4;
            Sigma = Math.Sqrt(m_Mapping.MapSize);
            MaxEpoch = 5;
            if (m_Input != null)
            {
                SigmaDecayFactor = Math.Pow(1d / m_SigmaSquared, 2d / 4000); // decay sigma to 1 after one epoch
                AlphaDecayFactor = Math.Pow(0.1d, 2d / 4000);  // decay alpha to 0.1 after one epoch
                OrderingDuration = 1000;
            }
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

                    AdjustParameters(currentIter);  // adjust parameters

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
        [ToDo] // this should probably use some precalculated lookup table (but i'm feeling lazy)        
        private double NeighbourhoodFn(double distance)
        {
            return (Math.Exp(-distance * distance / m_SigmaSquared));
        }

        private void OnProgressUpdate(System.EventArgs e)
        {
            if (ProgressUpdate != null)
                ProgressUpdate(this, e);
        }

        private void AdjustParameters(int iter)
        {
            if (iter <= m_OrderingTime)
                m_SigmaSquared *= m_RSigmaSquared;            // ordering phase
            else
                m_Alpha *= m_Ralpha;        // convergence phase
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

            while (h > Constants.Epsilon)
            {
                h = NeighbourhoodFn(kernel);

                foreach (int n in m_Mapping.Neighbours(winner, kernel))
                    m_Mapping[n] += (m_Alpha * h * (x - m_Mapping[n]));
                kernel++;
                if (kernel == m_Mapping.MapSize) // fail safe check
                    break;
            }
        }

 //*************************** new training and simulation regime *********
        // NOTE: This code is a bit hackish in places
        public void Train2()
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
                    c = Simulate2(x, out error);

                    UpdateWeights2(x, c);        //****** 

                    AdjustParameters(currentIter);  // adjust parameters

                    currentIter++;
                }
                if (!m_IsTraining)
                    break;

            }
        }

        public int Simulate2(Vector x, out double error)
        {
            if (x == null)
                throw new ArgumentNullException();
            if (m_Mapping == null)
                throw new InvalidOperationException("No mapping present");
             if (x[0] > x[1])
                throw new SOMLibException("First value in input range has to be less than" +
                    "or equal to second value.");
            if (x[0] < 0 || x[1] >= m_Mapping.InputDimension)
                throw new SOMLibException("range is incorrectly specified.");
            if ((x[1] - x[0]) != (x.Length - 3))
                throw new SOMLibException("Input vector is insufficiently sized.");


            int min = Convert.ToInt32(x[0]);
            int max = Convert.ToInt32(x[1]);
            int c = 0;
            double dist, minDist = int.MaxValue;
            int k;

            for (int i = 0; i < m_Mapping.MapSize; i++)
            {
                dist = 0;
                k = 2;
                for (int j = min; j <= max; j++)
                {
                    dist +=  BasicMath.Sqr(m_Mapping[i][j] - x[k]);
                    k++;
                }
                
                if (dist < minDist)
                {
                    c = i;
                    minDist = dist;
                }
            }

            error = Math.Sqrt(minDist);
            return c;
        }

        protected virtual void UpdateWeights2(Vector x, int winner)
        {/*
            int kernel = 0; // nieghbourhood size
            double h = 1; // result of neighbourhood function
            int min = Convert.ToInt32(x[0]);
            int max = Convert.ToInt32(x[1]);
            ViewVector X = new ViewVector(2, x.Length-1, x);
            ViewVector w;
            Vector updateVector;
            int k;

            while (h > Constants.EPSILON)
            {
                h = NeighbourhoodFn(kernel);

                foreach (int n in m_Mapping.Neighbours(winner, kernel))
                {
                    w = new ViewVector(min, max, m_Mapping[n]);
                    updateVector = (m_Alpha * h * (X - w));
                    k = 0;
                    for (int j = min; j <= max; j++)
                    {
                        m_Mapping[n][j] = updateVector[k];
                        k++;
                    }
                }
                kernel++;
                if (kernel == m_Mapping.MapSize) // fail safe check
                    break;
            }*
        }


        #endregion Methods
        */
    }
}
