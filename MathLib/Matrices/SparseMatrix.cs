using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MathLib.Matrices
{
    [ContractVerification(true)]
    [Serializable]
    public class SparseMatrix : SparseMatrixBase<SparseMatrix, SparseVector, double>
    {
        public SparseMatrix(int rows, int columns) : base(rows, columns) 
        {
            // Contract.Requires(rows > 0 && columns > 0);
            // Contract.Requires(rows < int.MaxValue);
        }

        public SparseMatrix(int rows, int columns, IEnumerable<Tuple<int, int, double>> values)
            : base(rows, columns, values)
        {
            // Contract.Requires(rows >= 1 && columns >= 1);
            // Contract.Requires(rows < int.MaxValue);
            // Contract.Requires(values != null);
            // Contract.Requires(// Contract.ForAll<Tuple<int, int, double>>
                //(values, new Predicate<Tuple<int, int, double>>(
                //             t => (t != null && t.Item1 < rows && t.Item1 >= 0 && t.Item2 < columns && t.Item2 >= 0))));
        }

        #region Overrides of MatrixBase<SparseMatrix,SparseVector,double>

        protected override SparseVector CreateVector(int rows, int columns)
        {
            if (rows == 1)
                return new SparseVector(columns, VectorType.RowVector);

            return new SparseVector(rows, VectorType.ColumnVector);            
        }

        protected override SparseMatrix CreateMatrix(int rows, int columns)
        {
            return new SparseMatrix(rows, columns);            
        }

        #endregion
    }
}
