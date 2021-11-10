using System;
using MathLib;
using MathLib.Matrices;
using SomLibrary.NeuronMaps;
using Util;

namespace SomLibrary.Algorithms
{
    [Serializable]
 //   [SOMPluginDetail("PLSOM", "Parameterless SOM: Algorithm developed by Erik Berglund and Joaquin Sitte")]
    public partial class PLSOM : ISOM
    {        
        #region Fields (6)

        private IInputLayer _input;
        private bool _isTraining;
        private INeuronMap _mapping;
        private int _updateInterval = 100;
        private double _rho;
        double _lastError;
        #endregion Fields

        #region Constructors (1)

        public PLSOM()
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

            for (epoch = 0; epoch < MMaxEpoch; epoch++)
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
            return (Math.Exp(-distance * distance / (sigma*sigma)));
        }

        private void OnProgressUpdate(EventArgs e)
        {
            if (ProgressUpdate != null)
                ProgressUpdate(this, e);
        }

        double CalcEpsilon()
        {
            double epsilon = _lastError / _rho;

            if (epsilon > 1)
            {
                _rho = _lastError;
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
