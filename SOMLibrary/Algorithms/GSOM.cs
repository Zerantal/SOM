using System;
using MathLib.Matrices;
using SomLibrary.NeuronMaps;

namespace SomLibrary.Algorithms
{    
    [SOMPluginDetail("GSOM", "The Growing Self-Organising Map", typeof(GrowingRectNeuronMap))]
    [Serializable]
    public sealed partial class GSOM : ISOM
    {        
        private bool _isTraining;       
        [NonSerialized] private IInputLayer _input;
        private int _updateInterval = 100; // the number of iterations before a ProgressUpdate event occurs  
        private GrowingRectNeuronMap _mapping;
        private int _neighbourhoodSize;
        private double[] _neighbourhoodFn;
        private double _gt;      // Growth threshold    
        private double _sigmaSquared;        

        [field: NonSerialized]
        public event EventHandler ProgressUpdate;

        private const int InititialNeighbourhoodSize = 2;

        private enum GsomLearningPhase { GrowingPhase, SmoothingPhase };
        
        GsomLearningPhase _learningPhase;

        private void OnProgressUpdate(EventArgs e)
        {
            if (ProgressUpdate != null)
                ProgressUpdate(this, e);
        }

        public GSOM()
        {
            _neighbourhoodSize = InititialNeighbourhoodSize;
            _sigmaSquared = 2;

            //calculate neighbourhood function
            _neighbourhoodFn = new double[_neighbourhoodSize + 1];
            for (int kernel = 0; kernel <= _neighbourhoodSize; kernel++)
                _neighbourhoodFn[kernel] = NeighbourhoodFn(kernel);

            _mapping = new GrowingRectNeuronMap();
        }

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

        public int Simulate(Vector x, out double error)
        {
            int c = 0;
            double dist, minDist = int.MaxValue;

            for (int i = 0; i < _mapping.MapSize; i++)
            {
                dist = (_mapping[i] - x).Norm;
                if (dist < minDist)
                {
                    c = i;
                    minDist = dist;
                }
            }           

            error = minDist;
            return c;
        }
        
        public void CancelTraining()
        {
            _isTraining = false;
        }
        
        public INeuronMap Map
        {
            get { return  _mapping; }
            set
            {
                if (value.GetType() != typeof(GrowingRectNeuronMap))
                    throw new ArgumentException("Map is not a valid GrowingRectNeuronMap");
                _mapping = (GrowingRectNeuronMap)value;

                _mapping.FactorOfDistribution = _FoD;
            }

        }

        public int ProgressInterval
        {
            get { return _updateInterval; }
            set { _updateInterval = value; }
        }
        
        /// <summary>
        /// Train GSOM
        /// </summary>        
        public void Train()
        {
            int epoch;
            int c;  // winning node    
            double error;
            int updateOnIter = _updateInterval;
            int iter = 1;

            // Initialisation
            _isTraining = true;
            _mapping = new GrowingRectNeuronMap(_input.InputDimension);
            MapInitialiser.Random(_mapping);
            _gt = -_input.InputDimension * Math.Log(_SF);
            _learningPhase = GsomLearningPhase.GrowingPhase;

            /////////// Main algorithm
            for (epoch = 0; epoch < m_MaxEpoch; epoch++)
            {                
                foreach (Vector x in _input)
                {
                    if (iter == updateOnIter)  // update event (for gui)
                    {
                        OnProgressUpdate(new EventArgs());
                        updateOnIter += _updateInterval;
                        //System.Threading.Thread.Sleep(1000);
                    }

                    if (iter == _growthPeriod)
                        _learningPhase = GsomLearningPhase.SmoothingPhase;

                    c = Simulate(x, out error);         //****** Find winner                    

                    UpdateWeights(x, c);        //****** 

                    UpdateErrorDirection(x, c);

                    if (_learningPhase == GsomLearningPhase.GrowingPhase)
                        GrowNodes(c, error);
                    else
                        _alpha *= _alphaDecayRate;

                    if (!_isTraining)    // abort algorithm if isTraining is set to false;
                        break;

                    iter++;
                }
                if (!_isTraining)
                    break;
            }
        }

        private void UpdateErrorDirection(Vector x, int winner)
        {
            GRectNodeData data = _mapping.GetData(winner);
            data.ErrorDirection += (x - _mapping[winner]);            
        }

        private double NeighbourhoodFn(int kernel)
        {
            return Math.Exp(-kernel * kernel / _sigmaSquared);
        }
        
        private void UpdateWeights(Vector x, int winner)
        {  

            for (int kernel = 0; kernel <= _neighbourhoodSize; kernel++)
            {
                foreach (int node in _mapping.Neighbours(winner, kernel))
                    _mapping[node] += _alpha * _neighbourhoodFn[kernel] * (x - _mapping[node]);
            }
        }

        public void GrowNodes(int winner, double error)
        {
            GRectNodeData data = _mapping.GetData(winner);
            data.Error += error;

            if (data.Error >= _gt)
            {
                if (_mapping.IsBoundary(winner))
                {
                    _mapping.GrowNodes(winner);
                }
                else
                    _mapping.RedistributeError(winner);
            }
        }                      
    }
}
