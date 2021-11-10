using System;
using System.Diagnostics.Contracts;

namespace MathLib.Matrices
{
    [Serializable]
    public class Matrix<TValueType> : DenseMatrixBase<Matrix<TValueType>, Vector<TValueType>, TValueType>
    {
        #region Constructors

        internal Matrix(int rows, int columns)
            : base(rows, columns)
        {
            // Contract.Requires(rows > 0);
            // Contract.Requires(columns > 0);            
        }

        public Matrix(int rows, int columns, TValueType initialValue = default(TValueType)) :
            base(rows, columns, initialValue) 
        {
            // Contract.Requires(rows> 0);
            // Contract.Requires(columns > 0);
        }


        public Matrix(TValueType[,] values) : base(values) 
        {
            // Contract.Requires(values != null);  
        }

        #endregion

        #region Overrides of MatrixBase<Matrix<TValueType>,Vector<TValueType>,TValueType>

        protected override Vector<TValueType> CreateVector(int rows, int columns)
        {
            return rows == 1 ? new Vector<TValueType>(columns) : new Vector<TValueType>(rows, VectorType.ColumnVector);
        }

        protected override Matrix<TValueType> CreateMatrix(int rows, int columns)
        {
            return new Matrix<TValueType>(rows, columns);            
        }

        #endregion
    }
}
