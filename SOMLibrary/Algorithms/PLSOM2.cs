using System;
using System.Collections.Generic;
using MathLib;
using MathLib.Matrices;
using SomLibrary.NeuronMaps;
using Util;

namespace SomLibrary.Algorithms
{    
    [Serializable]
 //   [SOMPluginDetail("PLSOM2", "Improved PLSOM from the creators of the original PLSOM")]
    public partial class PLSOM2 : ISOM
    {        
        [Serializable]
        public class DiameterBuffer
        {
            private double _maxDiameter = -1;
            private List<Vector> _buffer = new List<Vector>();

            public double MaxDiameter { get { return _maxDiameter; } }

            public void UpdateBuffer(Vector data)
            {
                //calculate the distance from the new input to all inputs in the buffer
                //find the largest distance between the input and any entry in the buffer
                //as well as the entry in the buffer that is closest to the input
                int minDistIndex = 0;
                double minDist = Double.MaxValue;
                double maxNewDist = 0;
                for (int i = 0; i < _buffer.Count; i++)
                {
                    double tmp = (_buffer[i] - data).Norm;
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
                if (maxNewDist > _maxDiameter)
                {
                    //add the new input to the buffer
                    _maxDiameter = maxNewDist;
                    _buffer.Add(data);
                    //keep the buffer to the set size...
                    if (_buffer.Count > (data.Length + 1))
                    {
                        //...by removing the buffer entry that is closest to the new input
                        _buffer.RemoveAt(minDistIndex);
                    }
                }

            }
        }



        #region Fields (6)

        [NonSerialized] private IInputLayer _input;
        private bool _isTraining;
        private INeuronMap _mapping;
        private int _updateInterval = 100;        
        private double _lastError;
        private DiameterBuffer _diameterBuffer;

        #endregion Fields

        #region Constructors (1)

        public PLSOM2()
        {
            //set algorithms parameters to their lower bounds
            _mapping = new RectNeuronMap(2, 20, 20);
        }

        #endregion Constructors

        #region Properties (3)

        public IInputLayer InputReader
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _input = value;
            }

            get
            {
                return _input;
            }
        }

        public INeuronMap Map
        {
            get { return _mapping; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _mapping = value;
            }
        }

        public int ProgressInterval
        {
            get { return _updateInterval; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "value has to be greater than or equal to one");

                _updateInterval = value;
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
            _isTraining = false;
        }
    
        public int Simulate(Vector x, out double error)
        {
            if (x == null)
                throw new ArgumentNullException();
            if (_mapping == null)
                throw new InvalidOperationException("No mapping present");
            if (_mapping.InputDimension != x.Length)
                throw new SizeMismatchException("Input vector has the wrong dimension for map.");

            int c = 0;
            double minDist = int.MaxValue;

            for (int i = 0; i < _mapping.MapSize; i++)
            {
                double dist = (_mapping[i] - x).Norm;
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
            int updateOnIter = ProgressInterval;
            int currentIter = 1;

            // check whether algorithm is in valid state
            if (_input == null)
                throw new InvalidOperationException("No input source set");
            if (_input.InputDimension != _mapping.InputDimension)
                throw new InvalidOperationException("Mismatch between input vector length " +
                    "and map weight vector lengths");

            _isTraining = true;
            _diameterBuffer = new DiameterBuffer();

            for (epoch = 0; epoch < m_MaxEpoch; epoch++)
            {
                foreach (Vector x in _input)
                {
                    if (currentIter == updateOnIter)  // update event (for gui)
                    {
                        OnProgressUpdate(new EventArgs());
                        updateOnIter += _updateInterval;
                        //System.Threading.Thread.Sleep(1000);
                    }

                    if (!_isTraining)    // abort algorithm if isTraining is set to false;
                        break;

                    //update diameter buffer
                    _diameterBuffer.UpdateBuffer(x);
                    // find winning neuron                   
                    double error;
                    int c = Simulate(x, out error);  // index of winning node                       
                    _lastError = error;


                    UpdateWeights(x, c);        //******                     

                    currentIter++;
                }
                if (!_isTraining)
                    break;

            }
        }


        [ToDo] // this should probably use some precalculated lookup table (but i'm feeling lazy)        
        private double NeighbourhoodFn(double distance, double epsilon)
        {
            double sigma = _beta * Math.Log(1 + epsilon * (Math.E - 1));
            //double sigma = _beta * epsilon;
            return (Math.Exp(-distance * distance / (sigma * sigma)));
        }

        private void OnProgressUpdate(EventArgs e)
        {
            if (ProgressUpdate != null)
                ProgressUpdate(this, e);
        }

        double CalcEpsilon()
        {
            double epsilon;

            if (_lastError == 0)
                epsilon = 0;
            else
            {
                epsilon = _lastError / _diameterBuffer.MaxDiameter;
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
            double epsilon = CalcEpsilon();

            while (h > Constants.Epsilon)
            {
                h = NeighbourhoodFn(kernel, epsilon);

                foreach (int n in _mapping.Neighbours(winner, kernel))
                    _mapping[n] += (epsilon * h * (x - _mapping[n]));
                kernel++;
                if (kernel == _mapping.MapSize) // fail safe check
                    break;
            }
        }

        #endregion Methods        
    }
}
