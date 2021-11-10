using System;
using System.Numerics;
using System.Diagnostics.Contracts;

using MathLib.Statistics;
using Util;

namespace MathLib.Matrices
{
    [Serializable]
    public class ComplexVector : ComplexMatrix, IVector<ComplexVector, Complex>,  IDeepCloneable<ComplexVector>
    {        
       #region Constructors

        public ComplexVector(int rows, int columns) : base(rows, columns)
        {}

        ///<summary>
        /// Initialise a new instance of the <see cref="ComplexVector"/> class
        /// with a specified dimension and orientation.
        ///</summary>
        ///<param name="dimension">Dimension of new <see cref="ComplexVector"/>.</param>
        ///<param name="orientation">Orientation of new <see cref="ComplexVector"/>.</param>
        ///<param name="initialValue">Value to initialise every element of vector to. Default is 0 + 0i.</param>
        ///<exception cref="ArgumentOutOfRangeException">Dimension is less
        /// than or equal to zero.</exception>
        public ComplexVector(int dimension, VectorType orientation = VectorType.RowVector,
                             Complex initialValue = default(Complex))
            : base(orientation == VectorType.RowVector ? 1 : dimension, orientation == VectorType.RowVector ? dimension : 1)
        {
            // Contract.Requires(dimension >= 1);

            if (!initialValue.Equals(default(Complex)))
                for (int r = 0; r < Rows; r++)
                    for (int c = 0; c < Columns; c++ )
                        ValuesData[r, c] = initialValue;            
        }

        ///<summary>
        /// Initialise a new instance of the <see cref="ComplexVector"/> class
        /// with a specific content and orientation.
        ///</summary>
        ///<param name="values">The values to initialise the 
        /// <see cref="ComplexVector"/> with.</param>
        ///<param name="orientation">Orientation of new <see cref="ComplexVector"/>.</param>
        ///<exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public ComplexVector(Complex[] values, VectorType orientation = VectorType.RowVector)
            : base(orientation == VectorType.RowVector ? 1 : values.Length, orientation == VectorType.RowVector ? values.Length : 1)
        {
            // Contract.Requires(values != null);
            // Contract.Requires(values.Length > 0);

            if (orientation == VectorType.ColumnVector)
            {
                for (int r = 0; r < Rows; r++)
                    ValuesData[r, 0] = values[r];
            }
            else
            {             
                for (int c = 0; c < Columns; c++)
                    ValuesData[0, c] = values[c];
            } 
        }

        public ComplexVector(int dimension, INumberGenerator realNumberGenerator, INumberGenerator imaginaryNumberGenerator, VectorType orientation = VectorType.RowVector)
            : base(orientation == VectorType.RowVector ? 1 : dimension, orientation == VectorType.RowVector ? dimension : 1)

        {
            // Contract.Requires(dimension >= 1);
            // Contract.Requires(realNumberGenerator != null);
            // Contract.Requires(imaginaryNumberGenerator != null);

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++ )
                    ValuesData[r, c] = new Complex(realNumberGenerator.Number, imaginaryNumberGenerator.Number);
        }
        
        #endregion 

        #region IVector<Complex> Members

        public VectorType Orientation
        {
            get {
                return Rows == 1 ? VectorType.RowVector : VectorType.ColumnVector;
            }
        }

        public int Length
        {
            get {
                return Rows == 1 ? Columns : Rows;
            }
        }

        public Complex this[int index]
        {

            get {
                return Rows == 1 ? ValuesData[0, index] : ValuesData[index, 0];
            }
            set
            {
                if (Rows == 1)
                    ValuesData[0, index] = value;

                else
                    ValuesData[index, 0] = value;
            }
        }

        public ComplexVector ArrayMultiplication(ComplexVector rhs)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDeepCloneable<ComplexVector> Members

        public new ComplexVector DeepClone()
        {
            ComplexVector clone = new ComplexVector(Rows, Columns);                                      

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    clone.ValuesData[r, c] = ValuesData[r, c];

            return clone;             
        }

        #endregion
    }
}
