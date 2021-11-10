using System;
using System.Numerics;
using System.Diagnostics.Contracts;

namespace MathLib.Matrices
{
    public class ComplexSparseMatrix : SparseMatrixBase<ComplexSparseMatrix, ComplexSparseVector, Complex>
    {
        public ComplexSparseMatrix(int rows, int columns) : base(rows, columns) 
        {
            // Contract.Requires(rows >= 1 && columns >= 1);
            // Contract.Requires(rows < int.MaxValue);
        }        

        public ComplexSparseMatrix(int rows, int columns, Tuple<int, int, Complex>[] values)
            : base(rows, columns, values)
        {
            // Contract.Requires(rows >= 1 && columns >= 1);
            // Contract.Requires(rows < int.MaxValue);
            // Contract.Requires(values != null);
            // Contract.Requires(// Contract.ForAll
                //(values, t => (t != null && t.Item1 < rows && t.Item1 >= 0 && t.Item2 < columns && t.Item2 >= 0)));
        }

        #region Overrides of MatrixBase<ComplexSparseMatrix,ComplexSparseVector,Complex>

        protected override ComplexSparseVector CreateVector(int rows, int columns)
        {
            if (rows == 1)            
                return new ComplexSparseVector(columns, VectorType.RowVector);
            
            return new ComplexSparseVector(rows, VectorType.ColumnVector);
        }

        protected override ComplexSparseMatrix CreateMatrix(int rows, int columns)
        {
            return new ComplexSparseMatrix(rows, columns);            
        }

        #endregion
    }
}