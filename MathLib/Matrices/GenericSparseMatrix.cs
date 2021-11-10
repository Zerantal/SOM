using System;
using System.Diagnostics.Contracts;

namespace MathLib.Matrices
{
    [Serializable]
    public class SparseMatrix<TValueType> : SparseMatrixBase<SparseMatrix<TValueType>, SparseVector<TValueType>, TValueType>
    {
        #region constructors

        public SparseMatrix(int rows, int columns) : base(rows, columns) 
        {
            // Contract.Requires(rows > 0 && columns > 0);
            // Contract.Requires(rows < int.MaxValue);
        }        

        public SparseMatrix(int rows, int columns, Tuple<int, int, TValueType>[] values)
            : base(rows, columns, values)
        {
            // Contract.Requires(rows >= 1 && columns >= 1);
            // Contract.Requires(rows < int.MaxValue);
            // Contract.Requires(values != null);
            // Contract.Requires(// Contract.ForAll<Tuple<int, int, TValueType>>
                //(values, new Predicate<Tuple<int, int, TValueType>>(
                //             t => (t != null && t.Item1 < rows && t.Item1 >= 0 && t.Item2 < columns && t.Item2 >= 0))));
        }        

        #endregion

        #region Overrides of MatrixBase<SparseMatrix<TValueType>,SparseVector<TValueType>,TValueType>

        protected override SparseVector<TValueType> CreateVector(int rows, int columns)
        {
            return rows == 1 ? new SparseVector<TValueType>(columns) : new SparseVector<TValueType>(rows, VectorType.ColumnVector);
        }

        protected override SparseMatrix<TValueType> CreateMatrix(int rows, int columns)
        {
            return new SparseMatrix<TValueType>(rows, columns);            
        }

        #endregion
    }
}
