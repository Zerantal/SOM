using System.Diagnostics.Contracts;

using MathLib.Matrices;

namespace SomLibrary
{
    [ContractClass(typeof(INeuronMapContract))]
    public interface INeuronMap
    {
        int MapSize { get; }

        Vector this[int nodeIdx] { get; set; }

        int InputDimension { get; set; }

        int[] Neighbours(int nodeIdx, int kernel);

        Vector NeuronPosition(int nodeIdx);


        /*
		#region Fields (3) 
                
        //protected List<Vector> _Weights;
        protected Matrix _Weights;
        protected int _inputDim = 2;   // make default = 2D

		#endregion Fields 

		#region Constructors (1) 

        public NeuronMap() : this(2, 100) { }
       
        public NeuronMap(int inputDimension, int mapSize)
        {
            // Contract.Requires<ArgumentOutOfRangeException>(mapSize > 0);
            // Contract.Requires<ArgumentOutOfRangeException>(inputDimension > 0);

            _inputDim = inputDimension;
            _Weights = new Matrix(mapSize, _inputDim);                                 
        }
        
		#endregion Constructors 

		#region Properties (4) 

        public int MapSize
        { 
            get { return _Weights.Rows; }
            protected set             
            {
                // Contract.Requires(value >= 1);

                _Weights = new Matrix(value, _inputDim);                
            }
        }

        public Vector this[int i]
        {
            get 
            {
                // Contract.Requires<ArgumentOutOfRangeException>(i >= 0 && i < MapSize);
                // Contract.Ensures(// Contract.Result<Vector>() != null);
                // Contract.Ensures(// Contract.Result<Vector>().Orientation == VectorType.RowVector);  // must return row vector                                
                return _Weights.GetRow(i);
            }

            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(i >= 0 && i < MapSize);
                // Contract.Requires<ArgumentNullException>(value != null);
                // Contract.Requires<ArgumentException>(value.Orientation == VectorType.RowVector);
                // Contract.Requires<SizeMismatchException>(value.Length == InputDimension, "Weight vectors is incorrectly sized.");
                _Weights.SetRow(i, value);  // Contract can't be proven for some reason?
            }
        }

        public int InputDimension
        {
            get { return _inputDim; }

            set 
            {
                // Contract.Requires<ArgumentException>(value >= 1, "Dimension of weight vectors must be greater than or equal to one");

                _inputDim = value;
                int mapSize = _Weights.Rows;
                _Weights = new Matrix(mapSize, _inputDim);                                          
            }
        }

		#endregion Properties 

		#region Methods (1) 


		// Public Methods (6) 

        /// <summary>
        /// Return all the neighbours of a neuron which are in its specified discrete neighbourhood ring
        /// </summary>
        /// <param name="node"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        abstract public int[] Neighbours(int n, int kernel);             

        abstract public Vector NeuronPosition(int i);

        #endregion Methods

        // define implicit Digit-to-byte conversion operator:
        public static implicit operator Matrix(NeuronMap map)
        {
            // Contract.Requires(map != null);

            return map._Weights;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(_Weights != null);
            // Contract.Invariant(_Weights.Columns == _inputDim);
            // Contract.Invariant(_Weights.Rows == MapSize);
            // Contract.Invariant(_inputDim > 0);
        }
         * */
    }
}
