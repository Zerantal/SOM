using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using MathLib.Matrices;

namespace SomLibrary.NeuronMaps
{    
    /// <summary>
    /// 3d map using a hexagonal lattice.    
    /// </summary>
    [Serializable]
    //[SOMPluginDetail("3D Hexagonal Map", "3D Hexagonal lattice")]
    public class HexNeuronMap : INeuronMapWithWeightEnum 
    {
        protected int XDim;
        protected int YDim;
        protected int ZDim;

        static protected Vector<int>[][] NeighbourOffsets;

        protected int NeuronsInXyPlane;    // number of neurons in axial x-y plane of lattice
        protected int MaxPreCalcNeighbourhood;

        private Matrix _weightings;
        private readonly int _inputDimension;

        public HexNeuronMap()
            : this(2)
        {}

        public HexNeuronMap(int weightDimension, int xDim = 20, int yDim = 20, int zDim = 1)
        {
            // Contract.Requires(weightDimension >= 1);
            // Contract.Requires(xDim >= 1 && yDim >= 1 && zDim >= 1);

            XDim = xDim;
            YDim = yDim;
            ZDim = zDim;

            _inputDimension = weightDimension;

            InitMap();
            
        }

        private void InitMap()
        {
            // Contract.Requires(XDim > 0 && YDim > 0 && ZDim > 0);

            _weightings = new Matrix(XDim * YDim * ZDim, _inputDimension);
            
            NeuronsInXyPlane = XDim * YDim;
            // assign MaxPreCalcNeighbourhood to maximum of _xDim, _yDim, _zDim
            MaxPreCalcNeighbourhood = new[] { XDim, YDim, ZDim }.Max();
            preCalcNeighbourhoodOffsets();
        }



        private static void preCalcNeighbourhoodOffsets()
        {

            //throw new NotImplementedException();
            //base.preCalcNeighbourhoodOffsets();
        }
        /*
        // neuron map layout in memory
        //  o   o   o   o   o
        //    o   o   o   o   o
        //  o   o   o   o   o  
        //    o   o   o   o   o
        //  o   o   o   o   o
        //  1   2   3   4   5
        public override int[] Neighbours(int i, int kernel)
        {
            if (kernel == 0)
                return new int[] { i };

            List<int> neighbours = new List<int>(6*kernel);
            int x, y, j, xoff = 0;

            _LatticePosition(i, out x, out y);

            if ((y % 2) == 0)
                xoff = 1;


            x = x - kernel;

            if (x >= 1 && x <= XDim && y >= 1 && y <= YDim)
                neighbours.Add(_CalcIndex(x, y));

            // side 1
            for (j = 1; j <= kernel; j++)
            {
                x += (j + xoff) % 2;
                y--;
                if (x >= 1 && x <= XDim && y >= 1 && y <= YDim)
                    neighbours.Add(_CalcIndex(x, y));
            }

            // side 2
            for (j = 0; j < kernel; j++)
            {
                x++;
                if (x >= 1 && x <= XDim && y >= 1 && y <= YDim)
                    neighbours.Add(_CalcIndex(x, y));
            }
            
            // size 3
            for (j = 1; j <= kernel; j++)
            {
                x += (j + kernel + xoff) % 2;
                y++;
                if (x >= 1 && x <= XDim && y >= 1 && y <= YDim) 
                    neighbours.Add(_CalcIndex(x, y));
            }

            // side 4
            for (j = 0; j < kernel; j++)
            {
                x -= (j + xoff) % 2;
                y++;
                if (x >= 1 && x <= XDim && y >= 1 && y <= YDim)
                    neighbours.Add(_CalcIndex(x, y));
            }

            // side 5
            for (j = 0; j < kernel; j++)
            {
                x--;
                if (x >= 1 && x <= XDim && y >= 1 && y <= YDim) 
                    neighbours.Add(_CalcIndex(x, y));
            }

            // side 6
            for (j = 0; j < kernel - 1; j++)
            {
                x -= (j + kernel + xoff) % 2;
                y--;
                if (x >= 1 && x <= XDim && y >= 1 && y <= YDim)
                    neighbours.Add(_CalcIndex(x, y));
            }



            return neighbours.ToArray();
        }

        public override Vector NeuronPosition(int i)
        {
            Vector position = new Vector(2);
            int x, y;
            _LatticePosition(i, out x, out y);
            
            position[0] = (double)(1 + 2 * x + (x % 2)) / m_ScaleFactor;
            position[1] = (double)(1 + 1.5 * y) / m_ScaleFactor;

            return position;
        }


        //-------------------- A few useful iterators ----------------------------

        // iterate over a horizontal span of neurons (ensure that x1 >= x2)
        // yields the index of each neuron
        protected override IEnumerable<int> HorizontalSpan(int y)
        {
            int idx;
           
            idx = _CalcIndex(1, y);
            for (int x = 1; x <= XDim; x++)
                yield return idx++;        
            
            yield break;
        }

        // iterate over a vertical span of neurons (ensure that y1 >= y2)
        // yields the index of each neuron
        protected virtual IEnumerable<int> ZigZagSpan(int y)
        {
            int y2;
            if ((y % 2) != 0)
            {
                y2 = y;
                y--;
            }
            else
                y2 = y - 1;           

            for (int x = 1; x <= XDim; x++)
            {                
                yield return _CalcIndex(x, y);                
                yield return _CalcIndex(x, y2);
            }
         /*   int idx;

            if (y1 < 0) y1 = 0;
            if (y2 >= m_YDim) y2 = m_YDim - 1;

            if (x >= 0 && x < m_XDim)
            {
                idx = CalcIndex(x, y1);
                for (int y = y1; y <= y2; y++)
                {
                    yield return idx;
                    idx += m_XDim;
                }
            }*
            yield break;
        }


        /// <summary>
        /// Iterate over the Neurons in the map. returns an array of neurons 
        /// corrosponding to each row followed by array of neurons corrosponding
        /// to the vertical columns of the map
        /// </summary>
        public override IEnumerable<Point[]> WeightEnum
        {
            get
            {
                List<Point> result = new List<Point>();

                for (int y = 1; y <= YDim; y++)
                {
                    foreach (int index in HorizontalSpan(y))
                        result.Add(new Point((float)this[index][1],
                                              (float)this[index][2]));
                    yield return result.ToArray();
                    result.Clear();
                }
                
                for (int y = 2; y <= YDim; y++)
                {
                    foreach (int index in ZigZagSpan(y))
                        result.Add(new Point((float)this[index][1],
                                              (float)this[index][2]));
                    yield return result.ToArray();
                    result.Clear();
                }
            }
        }*/

        #region INeuronMapWithWeightEnum Implementation
        public IEnumerable<Point[]> WeightEnum
        {
            get
            {
               throw new NotImplementedException();                
            }
        }
        #endregion

        #region INeuronMap Members

        public int MapSize
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Vector this[int nodeIdx]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int InputDimension
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Vector NeuronPosition(int idx)
        {
            double xRet;

            int z = idx / NeuronsInXyPlane + 1;
            int tmp = idx - (z - 1) * NeuronsInXyPlane;
            int y = tmp / XDim + 1;
            int x = tmp - (y - 1) * XDim + 1;

            double yRet = (y - 1) * Math.Sqrt(3.0) / 2.0;
            if (z % 2 == 1)    // is odd plane
            {
                xRet = x - 1 + 0.5 * (1 - (y % 2));
            }
            else
            {
                xRet = x - 1 + 0.5 * (y % 2);
                yRet += Math.Sqrt(3.0) / 6.0;
            }

            double zRet = (z - 1) * Math.Sqrt(2.0 / 3.0);

            return new Vector(new[] { xRet, yRet, zRet });
        }

        public int[] Neighbours(int n, int kernel)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
