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
    [TODO("remove map dimensions parameter from algorithm")]
    public partial class ProtoAlg //: ISOM
    {

        #region Fields (6)

        protected const int StartingDimension = 3;
        protected IInputLayer m_Input = null;
        protected bool m_IsTraining = false;
        protected NeuronMap m_Mapping = null;
        protected int m_MaxKernel = 1000;
        protected int m_UpdateInterval = 100;

        #endregion Fields

        #region Constructors (1)

        public ProtoAlg()
        {
            //set algorithms parameters to their lower bounds
            Alpha = 0;           
            Sigma = 1;                                                 
            InitialiseMap();
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

        public event EventHandler ProgressUpdate;


        #endregion Delegates and Events

        #region Methods (9)


        // Public Methods (5) 

        public void CancelTraining()
        {
            m_IsTraining = false;
        }

        public void InitialiseMap()
        {
            /*
            if (m_Input != null)
            {
                if (m_Mapping != null)
                    m_Mapping.RedimensionMap(m_Input.InputDimension, new int [] {StartingDimension, StartingDimension});
                else
                    m_Mapping = new RectNeuronMap(m_Input.InputDimension, new int [] {StartingDimension, StartingDimension});

                Init.Random(m_Mapping); // clear map
            }*/
        }

        public void ResetParameters()
        {/*
            double tmp = 0;

            Alpha = 0.4;
            foreach (int d in m_Mapping.MapDimensions)
                tmp += d * d;
            Sigma = Math.Sqrt(tmp);
            MaxEpoch = 5;
            if (m_Input != null)
            {
                SigmaDecayFactor = Math.Pow(1d / m_SigmaSquared, 2d / 4000); // decay sigma to 1 after one epoch
                AlphaDecayFactor = Math.Pow(0.1d, 2d / 4000);  // decay alpha to 0.1 after one epoch
                OrderingDuration = 1000;
            }*/
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns>Node id of winning neuron</returns>      
        public virtual Neuron Simulate(Vector x, out double error)
        {
            Neuron c = null;
            double dist, minDist = int.MaxValue;

            foreach (Neuron n in Map)
            {
                dist = m_Mapping.ComparitiveDistance(n, x);
                if (dist < minDist)
                {
                    c = n;
                    minDist = dist;
                }
            }
            error = m_Mapping.Distance(c, x);
            return c;
        }

        /// <summary>
        /// Train SOM
        /// </summary>        
        public void Train()
        {
            int epoch;
            Neuron c;  // index of winning node           
            double squaredError = double.MaxValue; // Squared Error of neuron
            int updateOnIter = 1;
            int currentIter = 1;

            // check whether algorithm is in valid state
            if (m_Input == null)
                throw new InvalidOperationException("No input source set");
            if (m_Mapping == null)
                throw new InvalidOperationException("No map set");
            if (m_Input.InputDimension != m_Mapping.WeightDimension)
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

                    c = Simulate(x, out squaredError);         //****** Find winner

                    UpdateWeights(x, c);        //****** 

                    PostProcessing(currentIter);  // adjust parameters

                    currentIter++;
                }
                if (!m_IsTraining)
                    break;

            }
        }



        // Protected Methods (4) 

        /// <summary>
        /// Simple gaussian function using 1 parameter to control the spread 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [TODO] // this should probably use some precalculated lookup table (but i'm feeling lazy)        
        protected virtual double NeighbourhoodFn(double dist)
        {
            return m_Alpha * Math.Exp(-dist * dist / m_SigmaSquared);
        }

        protected virtual void OnProgressUpdate(System.EventArgs e)
        {
            if (ProgressUpdate != null)
                ProgressUpdate(this, e);
        }

        protected virtual void PostProcessing(int iter)
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
        [TODO("Fail safe check needs fixing: leads to premature truncation of the neighbourhood function")]
        protected virtual void UpdateWeights(Vector x, Neuron winner)
        {
            int kernel = 0; // nieghbourhood size
            double h = 1; // result of neighbourhood function
            List<Neuron> neighbours;

            while ((h = NeighbourhoodFn(kernel)) > Constants.EPSILON)
            {
                neighbours = m_Mapping.Neighbours(winner, kernel);
                foreach (Neuron n in neighbours)
                    n.W = n.W + (h * (x - n.W));
                kernel++;

                if (kernel == m_MaxKernel || neighbours.Count == 0)
                    break;
            }
        }


        #endregion Methods

    }
}
