using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using MathLib.Matrices;

namespace SomLibrary.NeuronMaps
{

    [Serializable]
 //   [SOMPluginDetail("2D Growing Rectangular Map", "2D rectangular growing map used by the GSOM")]
    public class GrowingRectNeuronMap : INeuronMapWithWeightEnum
    {        
        
        // Maximum dimension of _mapGrid.  initial nodes will be position in the centre.
        private const int MaxRows = 10000;
        private const int MaxCols = 10000;
        
        // sparse array containing the indices of each map node (incremented by 1, 0 represents a vacant position)
        private SparseMatrix<int> _mapGrid;

        private List<GRectNodeData> _nodeData;
        private double _fod = 0.3f;
        private int _nextNewNodeIdx;        // should also equal _nodeData.Count and _weights.Count                                   

        private List<Vector> _weightings;
        private int _inputDimension;

        public GrowingRectNeuronMap() : this(2) { }

        public GrowingRectNeuronMap(int weightDimension)           
        {
            // Contract.Requires<ArgumentOutOfRangeException>(weightDimension > 0);

            GRectNodeData.MaxCols = MaxCols;    // Ensure GRectNodeData class is aware of map limits
            GRectNodeData.MaxRows = MaxRows;
            _inputDimension = weightDimension;

            InitMap();
           
        }        

        private void InitMap()
        {
            // Initialise neurons            
            _nodeData = new List<GRectNodeData>();
            _weightings = new List<Vector>();
            _mapGrid = new SparseMatrix<int>(MaxRows, MaxCols);

            // add first 4 neurons    
            int r = MaxRows / 2;   // x and y positions of first node
            int c = MaxCols / 2;
            AddNode(r, c);
            AddNode(r + 1, c);
            AddNode(r + 1, c + 1);
            AddNode(r, c + 1); 
        }

        protected internal int AddNode(int r, int c)
        {
            // Contract.Requires(r > 0 && r < MaxRows - 1);
            // Contract.Requires(c > 0 && c < MaxCols - 1);

            // Contract.Assert(_mapGrid[r, c] == 0);

            int newIdx = _nextNewNodeIdx++;            
            GRectNodeData newNodeData = new GRectNodeData(r, c, _inputDimension);

            SetIndexAtLocation(r, c, newIdx);                             
            _nodeData.Add(newNodeData);

            _weightings.Add(new Vector(_inputDimension));            

            return newIdx;
        }

        public bool IsBoundary(int nodeIdx)
        {
            // Contract.Requires<ArgumentOutOfRangeException>(nodeIdx >= 0);
            // Contract.Requires<ArgumentOutOfRangeException>(nodeIdx < MapSize);

            // lattice positions of node
            int r = _nodeData[nodeIdx].Row;
            int c = _nodeData[nodeIdx].Column;

            if (GetIndexAtLocation(r + 1, c) == -1)
                return true;
            if (GetIndexAtLocation(r - 1, c) == -1)
                return true;
            if (GetIndexAtLocation(r, c + 1) == -1)
                return true;
            if (GetIndexAtLocation(r, c - 1) == -1)
                return true;

            return false;
        }

        protected int GetIndexAtLocation(int row, int column)
        {
            return _mapGrid[row, column] - 1;
        }

        protected void SetIndexAtLocation(int row, int column, int idx)
        {
            _mapGrid[row, column] = idx + 1;
        }

        public void GrowNodes(int nodeIdx)
        {
            // Contract.Requires<ArgumentOutOfRangeException>(nodeIdx >= 0 && nodeIdx < MapSize);

            int r = _nodeData[nodeIdx].Row;
            int c = _nodeData[nodeIdx].Column;
            int[][] newNodeLocales = new int[4][];
            newNodeLocales[0] = new[] { r + 1, c };
            newNodeLocales[1] = new[] { r - 1, c };
            newNodeLocales[2] = new[] { r, c + 1 };
            newNodeLocales[3] = new[] { r, c - 1 };

            for (int i = 0; i < 4; i++)
            {
                if (GetIndexAtLocation(newNodeLocales[i][0], newNodeLocales[i][1]) != -1) continue;

                int newNodeIdx = AddNode(newNodeLocales[i][0], newNodeLocales[i][1]);
                _nodeData[nodeIdx].GrowthLevel++;

                // set weight of new node
                _weightings[newNodeIdx] = _weightings[nodeIdx].DeepClone();
            }            
        }

        public void RedistributeError(int nodeIdx)
        {
            // Contract.Requires<ArgumentOutOfRangeException>(nodeIdx >= 0);
            // Contract.Requires<ArgumentOutOfRangeException>(nodeIdx < MapSize);          

            int r = _nodeData[nodeIdx].Row;
            int c = _nodeData[nodeIdx].Column;
            
            int[] neighIndices = new int[4];
            neighIndices[0] = GetIndexAtLocation(r, c-1);
            neighIndices[1] =  GetIndexAtLocation(r, c + 1);
            neighIndices[2] = GetIndexAtLocation(r + 1, c);
            neighIndices[3] = GetIndexAtLocation(r - 1, c);

            Vector neighbourDirection;
            foreach (int idx in neighIndices)
                if (idx != -1)
                {
                    // Contract.Assume(idx > 0 && idx < MapSize);
                    neighbourDirection = (this[idx] - this[nodeIdx]);
                    double cosineDistance = Vector.DotProduct(neighbourDirection, _nodeData[nodeIdx].ErrorDirection)/
                                            neighbourDirection.Norm/_nodeData[nodeIdx].ErrorDirection.Norm;
                    if (cosineDistance < 0) cosineDistance = 0;

                    _nodeData[idx].Error += _fod * _nodeData[nodeIdx].Error * cosineDistance;
                }
                else
                    Debug.WriteLine("RedistributeError called on a boundary node.");

            _nodeData[nodeIdx].Error /= 2;
        }
        
        public double FactorOfDistribution
        {
            get { return _fod; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value <= 1, "FactorOfDistribution must be between 0 and 1 exclusive.");
                _fod = value;
            }
        }
                
        public GRectNodeData GetData(int nodeIdx)
        {
            // Contract.Requires<ArgumentOutOfRangeException>(nodeIdx >= 0);
            // Contract.Requires<ArgumentOutOfRangeException>(nodeIdx < MapSize);
            return _nodeData[nodeIdx];
        }

        #region INeuronMapWithWeightEnum implementation
   
        public IEnumerable<Point[]> WeightEnum
        {
            get
            {
                List<Point[]> result = new List<Point[]>();

                // sort nodes by row then by columns
                IOrderedEnumerable<GRectNodeData> sortedNodes = _nodeData.OrderBy(r => r.Row).ThenBy(c => c.Column);

                foreach (GRectNodeData d in sortedNodes)
                {
                    int idx1 = GetIndexAtLocation(d.Row, d.Column);
                    int idx2 = GetIndexAtLocation(d.Row + 1, d.Column);
                    //Debug.Write("idx1 = " + idx2 + ", idx2 = " + idx2);
                    if (idx2 != -1)
                        result.Add(new[] {
                            new Point((float)this[idx1][0], (float)this[idx1][1]),
                            new Point((float)this[idx2][0], (float)this[idx2][1])});
                    idx2 = GetIndexAtLocation(d.Row, d.Column + 1);
                   // Debug.WriteLine(", " + idx2);
                    if (idx2 != -1)
                        result.Add(new[] {
                            new Point((float)this[idx1][0], (float)this[idx1][1]),
                            new Point((float)this[idx2][0], (float)this[idx2][1])});
                }
     
                return result;
            }
        }

        #endregion

        public IEnumerable<Vector[]> LatticeEnumerator
        {
            get
            {
                List<Vector[]> result = new List<Vector[]>();

                // sort nodes by row then by columns
                IOrderedEnumerable<GRectNodeData> sortedNodes = _nodeData.OrderBy(r => r.Row).ThenBy(c => c.Column);

                foreach (GRectNodeData d in sortedNodes)
                {
                    int idx1 = GetIndexAtLocation(d.Row, d.Column);
                    int idx2 = GetIndexAtLocation(d.Row + 1, d.Column);
                    //Debug.Write("idx1 = " + idx2 + ", idx2 = " + idx2);
                    if (idx2 != -1)
                        result.Add(new[] {
                            this[idx1], this[idx2]
                        });                    
                    idx2 = GetIndexAtLocation(d.Row, d.Column + 1);
                    // Debug.WriteLine(", " + idx2);
                    if (idx2 != -1)
                        result.Add(new[] {
                            this[idx1], this[idx2]
                        });                        
                }

                return result;
            }
        }

        #region INeuronMap Members

        public int MapSize
        {
            get 
            {
                return _weightings.Count();                 
            }
        }

        public Vector this[int nodeIdx]
        {
            get
            {
                return _weightings[nodeIdx];
            }
            set
            {
                _weightings[nodeIdx] = value;
            }
        }

        public int InputDimension
        {
            get { return _inputDimension; }
            set 
            {
                _inputDimension = value;

                InitMap();       
            }
        }

        public int[] Neighbours(int n, int kernel)
        {
            if (kernel == 0)
                return new[] { n };

            List<int> neighbours = new List<int>(100);
            SparseVector<int> v;
            int r = _nodeData[n].Row;
            int c = _nodeData[n].Column;

            // left column
            if (c - kernel > 0)
            {
                v = _mapGrid.GetColumn(c - kernel);
                for (int i = r - kernel; i <= r + kernel; i++)
                    neighbours.Add(v[i]-1);
            }

            // right column
            if (c + kernel < MaxCols - 1)
            {
                v = _mapGrid.GetColumn(c + kernel);
                for (int i = r - kernel; i <= r + kernel; i++)
                    neighbours.Add(v[i]-1);
            }

            // top row
            if (r - kernel > 0)
            {
                v = _mapGrid.GetRow(r - kernel);
                for (int i = c - kernel; i <= c + kernel; i++)
                    neighbours.Add(v[i]-1);
            }

            // bottom row
            if (r + kernel < MaxRows - 1)
            {
                v = _mapGrid.GetRow(r + kernel);
                for (int i = c - kernel; i <= c + kernel; i++)
                    neighbours.Add(v[i]-1);
            }

            // Remove all all zeros
            neighbours.RemoveAll(i => i < 0);

            // return distinct elements as array            
            return neighbours.Distinct().ToArray();
        }

        public Vector NeuronPosition(int nodeIdx)
        {
            return new Vector(new double[] { _nodeData[nodeIdx].Row, _nodeData[nodeIdx].Column });
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(_mapGrid != null);
            // Contract.Invariant(_mapGrid.Rows == MaxRows);
            // Contract.Invariant(_mapGrid.Columns == MaxCols);
            // Contract.Invariant(_nodeData != null);
            // Contract.Invariant(_weightings != null);
            // Contract.Invariant(_weightings.Count() > 0);
            // Contract.Invariant(// Contract.ForAll(_weightings, v => v != null && v.Rows == 1));
            // Contract.Invariant(_nodeData.Count() == _weightings.Count());
            // Contract.Invariant(_inputDimension > 0);
            // Contract.Invariant(MaxRows == GRectNodeData.MaxRows);
            // Contract.Invariant(MaxCols == GRectNodeData.MaxCols);
        }
    }
}