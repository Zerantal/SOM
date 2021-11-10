using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using MathLib.Matrices;

namespace SomLibrary.NeuronMaps
{
    [Serializable]
    public class GRectNodeData
    {
        private double _error;       // accumulated error for node
        private int _hits;           // number of times neuron has won
        private int _growthLevel = 1;    // How old the neuron is 
        private int _row;                 // row position in map (zero based)
        private int _col;                 // column position in map (zero based)

        public static int MaxRows = 10000;      
        public static int MaxCols = 10000;

        [OptionalField]
        private Vector _averageErrorDirection; // direction that errors are coming from on average
        
        public GRectNodeData(int r, int c, int inputDim)
        {
            // Contract.Requires(r > 0 && r < MaxRows - 1);
            // Contract.Requires(c > 0 && c < MaxCols - 1);

            _row = r;
            _col = c;
            _averageErrorDirection = new Vector(inputDim);
        }

        public Vector ErrorDirection
        {
            get { return _averageErrorDirection; }
            set { _averageErrorDirection = value; }
        }

        public double Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public int HitCount
        {
            get { return _hits; }
            set { _hits = value; }
        }

        public int GrowthLevel
        {
            get { return _growthLevel; }
            set { _growthLevel = value; }
        }

        public int Row
        {
            get 
            {
                // Contract.Ensures(// Contract.Result<int>() > 0);
                // Contract.Ensures(// Contract.Result<int>() < MaxRows-1);
                return _row; 
            }
            set 
            {
                // Contract.Requires(value > 0);
                // Contract.Requires(value < MaxRows-1);
                _row = value; 
            }
        }

        public int Column
        {
            get 
            {
                // Contract.Ensures(// Contract.Result<int>() > 0);
                // Contract.Ensures(// Contract.Result<int>() < MaxCols-1);
                return _col; 
            }
            set 
            {
                // Contract.Requires(value > 0);
                // Contract.Requires(value < MaxCols-1);
                _col = value; 
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(_row > 0);
            // Contract.Invariant(_row < MaxRows - 1);
            // Contract.Invariant(_col > 0);
            // Contract.Invariant(_col < MaxCols - 1);
        }


    }
}
