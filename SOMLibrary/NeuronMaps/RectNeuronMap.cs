using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using MathLib.Matrices;

namespace SomLibrary.NeuronMaps
{    
    [SOMPluginDetail("3D Rectangular Map", "3D rectangular lattice")]
    [Serializable]
    public class RectNeuronMap : INeuronMapWithWeightEnum
    {        
        private int _xDim;
        private int _yDim;
        private int _zDim;

        static private Vector<int>[][] _neighbourOffsets;        
        
        private int _neuronsInXyPlane;    // number of neurons in axial x-y plane of lattice
        private int _maxPreCalcNeighbourhood;

        Matrix _weightings;
        int _inputDimension;

        internal const int DimensionUpperBound = 10000;

        public RectNeuronMap()
            : this(2, 100)
        {
        }

        public RectNeuronMap(int inputDimension, int xDim, int yDim = 1, int zDim = 1)
        {
            // Contract.Requires<ArgumentException>(inputDimension >= 1);
            // Contract.Requires<ArgumentException>(xDim > 0 && yDim > 0 && zDim > 0);

            _xDim = xDim;
            _yDim = yDim;
            _zDim = zDim;

            _inputDimension = inputDimension;

            InitMap();            
        }

        private void InitMap()
        {
            // Contract.Requires(_xDim > 0 && _yDim > 0 && _zDim > 0);

            _weightings = new Matrix(_xDim * _yDim * _zDim, _inputDimension);            

            _neuronsInXyPlane = _xDim * _yDim;
            // assign MaxPreCalcNeighbourhood to maximum of _xDim, _yDim, _zDim
            _maxPreCalcNeighbourhood = new[] { _xDim, _yDim, _zDim }.Max();
            PreCalcNeighbourhoodOffsets();
        }

        private void PreCalcNeighbourhoodOffsets()
        {
            _neighbourOffsets = new Vector<int>[_maxPreCalcNeighbourhood + 1][];
            _neighbourOffsets[0] = new[] { new Vector<int>(new[] { 0, 0, 0 }) }; // 0th neighbourhood offsets   


            List<Vector<int>> neighbourhoodOffsets = new List<Vector<int>>();
            for (int i = 1; i <= _maxPreCalcNeighbourhood; i++)
            {
                neighbourhoodOffsets.Clear();
                int xRange = i > _xDim ? 0 : i;
                int yRange = i > _yDim ? 0 : i;
                int zRange = i > _zDim ? 0 : i;

                // do it face by face
                //x = -i, i faces
                if (xRange != 0)
                    for (int y = -yRange; y <= yRange; y++)
                        for (int z = -zRange; z <= zRange; z++)
                        {
                            neighbourhoodOffsets.Add(new Vector<int>(new[] {-i, y, z}));
                            neighbourhoodOffsets.Add(new Vector<int>(new[] {i, y, z}));
                        }

                //y = -i, i faces               
                if (yRange != 0)
                    for (int x = -xRange; x <= xRange; x++)
                        for (int z = -zRange; z <= zRange; z++)
                        {
                            neighbourhoodOffsets.Add(new Vector<int>(new[] {x, -i, z}));
                            neighbourhoodOffsets.Add(new Vector<int>(new[] { x, i, z }));
                        }

                //z = -i, i faces                
                if (zRange != 0)
                    for (int x = -xRange; x <= xRange; x++)
                        for (int y = -yRange; y <= yRange; y++)
                        {
                            neighbourhoodOffsets.Add(new Vector<int>(new[] {x, y, -i}));
                            neighbourhoodOffsets.Add(new Vector<int>(new[] {x, y, i}));
                        }

                _neighbourOffsets[i] = neighbourhoodOffsets.Distinct().ToArray();
            }

        }

        private Vector<int> neuronPosition(int idx)
        {
            // Contract.Requires(idx >= 0 && idx < MapSize);

            int z = idx / _neuronsInXyPlane + 1;
            int tmp = idx - (z - 1) * _neuronsInXyPlane;
            int y = tmp / _xDim + 1;
            int x = tmp - (y - 1) * _xDim + 1;

            return new Vector<int>(new[] {x, y, z});
        }

        private int CalcIndex(Vector<int> v)
        {
            // Contract.Requires(v != null);
            // Contract.Requires(v.Length == 3);
            // Contract.Requires(v[0] >= 1 && v[0] <= _xDim && v[1] >= 1 && v[1] <= _yDim && v[2] >= 1 && v[2] <= _zDim);
            // Contract.Ensures(// Contract.Result<int>() >= 0);
            // Contract.Ensures(// Contract.Result<int>() < MapSize);

            int idx = ((v[2] - 1) * _neuronsInXyPlane) + ((v[1] - 1) * _xDim) + v[0] - 1;

            // Contract.Assume(idx >= 0 && idx < MapSize); // Don't know how this can be proved

            return idx;
        }

        #region INeuronMapWithWeightEnum implementation

        //-------------------- A few useful iterators ----------------------------

        // iterate over an x-span of neurons
        // yields the index of each neuron
        protected virtual IEnumerable<int> XSpan(int y, int z)
        {
            // Contract.Requires(y >= 1 && y <= this.YDim);
            // Contract.Requires(z >= 1 && z <= this.ZDim);

            for (int x = 1; x <= _xDim; x++)
                yield return CalcIndex(new Vector<int>(new[] {x, y, z}));

            yield break;
        }

        // iterate over a y-span of neurons
        // yields the index of each neuron
        protected virtual IEnumerable<int> YSpan(int x, int z)
        {
            // Contract.Requires(x >= 1 && x <= this.XDim && z >= 1 && z <= this.ZDim);

            for (int y = 1; y <= _yDim; y++)
                yield return CalcIndex(new Vector<int>(new[] {x, y, z}));

            yield break;
        }

        // iterate over an z-span of neurons
        // yields the index of each neuron
        protected virtual IEnumerable<int> ZSpan(int x, int y)
        {
            // Contract.Requires(x >= 1 && x <= this.XDim && y >= 1 && y <= this.YDim);

            for (int z = 1; z <= _zDim; z++)
                yield return CalcIndex(new Vector<int>(new[] {x, y, z}));

            yield break;
        }

                /// <summary>
        /// Iterate over the Neurons in the map. returns an array of neurons 
        /// corrosponding to each row followed by array of neurons corrosponding
        /// to the vertical columns of the map
        /// </summary>
        public IEnumerable<Point[]> WeightEnum
        {
            get
            {                
                List<Point> result = new List<Point>();
                
                // x lines
                for (int y = 1; y <= _yDim; y++)
                    for (int z = 1; z <= _zDim; z++)
                    {
                        result.AddRange(XSpan(y, z).Select(index => new Point((float) this[index][0], (float) this[index][1])));
                        yield return result.ToArray();
                        result.Clear();
                    }

                // y lines
                for (int x = 1; x <= _xDim; x++)
                    for (int z = 1; z <= _zDim; z++)
                    {
                        result.AddRange(YSpan(x, z).Select(index => new Point((float) this[index][0], (float) this[index][1])));
                        yield return result.ToArray();
                        result.Clear();
                    }

                // z lines
                for (int x = 1; x <= _xDim; x++)
                    for (int y = 1; y <= _yDim; y++)
                    {
                        result.AddRange(ZSpan(x, y).Select(index => new Point((float) this[index][0], (float) this[index][1])));
                        yield return result.ToArray();
                        result.Clear();
                    }
            }
        }

        #endregion

        #region INeuronMap Members

        public int MapSize
        {
            get { return _weightings.Rows;}
            set { _weightings = new Matrix(value, _inputDimension); }
        }

        public Vector this[int nodeIdx]
        {
            get
            {
                return _weightings.GetRow(nodeIdx);
            }
            set
            {
                _weightings.SetRow(nodeIdx, value);
            }
        }

        public int InputDimension
        {
            get { return _inputDimension;}                  
            set 
            {
                _inputDimension = value;
                _weightings = new Matrix(_weightings.Rows, _inputDimension);
            }            
        }

        public int[] Neighbours(int nodeIdx, int kernel)
        {
            if (kernel == 0)
                return new[] { nodeIdx };

            List<Vector<int>> neighbourVectors = new List<Vector<int>>(100);
            Vector<int>[] neighbourOffsets;
            Vector<int> n = neuronPosition(nodeIdx);

            if (kernel <= _maxPreCalcNeighbourhood)
                neighbourOffsets = _neighbourOffsets[kernel];
            else
                return new int[] { };            // should probably provide some kind of fallback here

            // calculate neighbourhood vectors
            neighbourVectors.AddRange(neighbourOffsets.Select(v => (n + v)));

            // remove vectors outside of defined lattice bounds
            neighbourVectors = neighbourVectors.FindAll(
                delegate(Vector<int> v)
                {
                    if (v[0] < 1 || v[0] > _xDim || v[1] < 1 || v[1] > _yDim || v[2] < 1 || v[2] > _zDim)
                        return false;
                    return true;
                });

            // evaluate indices of neighbourhood vectors
            List<int> neighbourIndices = neighbourVectors.ConvertAll(CalcIndex);

            return neighbourIndices.ToArray();
        }

        public Vector NeuronPosition(int nodeIdx)
        {
            int z = nodeIdx / _neuronsInXyPlane + 1;
            int tmp = nodeIdx - (z - 1) * _neuronsInXyPlane;
            int y = tmp / _xDim + 1;
            int x = tmp - (y - 1) * _xDim + 1;

            return new Vector(new double[] { x, y, z });
        }

        #endregion

        #region Public properties

        [SOMLibProperty("XDim", "Number of neurons in x-direction",
            1, DimensionUpperBound, 20)]
        public int XDim
        {
            get { return _xDim; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 1 && value <= DimensionUpperBound);
                
                _xDim = value;

                InitMap();
            }
        }

        [SOMLibProperty("YDim", "Number of neurons in y-direction",
            1, DimensionUpperBound, 20)]
        public int YDim
        {
            get { return _yDim; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 1 && value <= DimensionUpperBound);

                _yDim = value;

                InitMap();
            }
        }


        [SOMLibProperty("ZDim", "Number of neurons in y-direction",
            1, DimensionUpperBound, 1)]
        public int ZDim
        {
            get { return _zDim; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 1 && value < DimensionUpperBound);
                
                _zDim = value;

                InitMap();
            }
        }
        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        { 
            // Contract.Invariant(_weightings != null);
            // Contract.Invariant(_weightings.Columns == InputDimension);
            // Contract.Invariant(_weightings.Rows == MapSize);
            // Contract.Invariant(_inputDimension > 0);
            // Contract.Invariant(_xDim > 0 && _yDim > 0 && _zDim > 0);
            // Contract.Invariant(_neighbourOffsets != null);
            //// Contract.Invariant(// Contract.ForAll<Vector>(_neighbourOffsets, v => v != null)));
            // Contract.Invariant(_neuronsInXyPlane != 0);
        }
    }
}