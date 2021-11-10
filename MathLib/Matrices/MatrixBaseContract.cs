using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MathLib.Matrices
{
    [ContractClassFor(typeof(MatrixBase<,,>))]
    abstract class MatrixBaseContract<TMatrixType, TVectorType, TValueType> 
        : MatrixBase<TMatrixType, TVectorType, TValueType>
        where TMatrixType : MatrixBase<TMatrixType, TVectorType, TValueType>
        where TVectorType : TMatrixType, IVector<TVectorType, TValueType>
    {
        private MatrixBaseContract(int rows, int columns) : base(rows, columns)
        {
        }

        #region MatrixBase overrides

        public override TVectorType GetRow(int row)
        {
            // Contract.Requires(row >= 0);
            // Contract.Requires(row < this.Rows);
            // Contract.Ensures(// Contract.Result<TVectorType>() != null);
            // Contract.Ensures(// Contract.Result<TVectorType>().Rows == 1);
            // Contract.Ensures(// Contract.Result<TVectorType>().Columns == this.Columns);            
            throw new NotImplementedException();
        }

        public override TVectorType GetColumn(int column)
        {
            // Contract.Requires(column >= 0);
            // Contract.Requires(column < this.Columns);
            // Contract.Ensures(// Contract.Result<TVectorType>() != null);
            // Contract.Ensures(// Contract.Result<TVectorType>().Rows == this.Rows);
            // Contract.Ensures(// Contract.Result<TVectorType>().Columns == 1);  
            throw new NotImplementedException();
        }
        
        public override void SetRow(int row, TVectorType rowVector)
        {
            // Contract.Requires(rowVector != null);
            // Contract.Requires(row >= 0);
            // Contract.Requires(row < this.Rows);
            // Contract.Requires(rowVector.Rows == 1);
            // Contract.Requires(rowVector.Columns == this.Columns);
            throw new NotImplementedException();
        }

        public override void SetColumn(int column, TVectorType columnVector)
        {
            // Contract.Requires(columnVector != null);
            // Contract.Requires(column >= 0);
            // Contract.Requires(column < this.Columns);
            // Contract.Requires(columnVector.Rows == this.Rows);
            // Contract.Requires(columnVector.Columns == 1);
            throw new NotImplementedException();
        }

        public override IEnumerable<TVectorType> RowEnumerator
        {
            get
            {
                // Contract.Ensures(// Contract.ForAll(// Contract.Result<IEnumerable<TVectorType>>(),
                    //vector => vector != null));
                throw new NotImplementedException();
            }
        }

        public override IEnumerable<TVectorType> ColumnEnumerator
        {
            get
            {
                // Contract.Ensures(// Contract.ForAll(// Contract.Result<IEnumerable<TVectorType>>(),
                    //vector => vector != null));
                throw new NotImplementedException();
            }
        }

        public override TMatrixType Transpose()
        {
            // Contract.Ensures(// Contract.Result<TMatrixType>() != null);
            throw new NotImplementedException();
        }

        public override TValueType this[int row, int column]
        {
            get
            {
                // Contract.Requires(row >= 0 && row < this.Rows);
                // Contract.Requires(column >= 0 && column < this.Columns);
                throw new NotImplementedException();
            }
            set
            {
                // Contract.Requires(row >= 0 && row < this.Rows);
                // Contract.Requires(column >= 0 && column < this.Columns);
                throw new NotImplementedException();
            }
        }

        public override TVectorType AsVector()
        {
            // Contract.Requires(this.Rows == 1 || this.Columns == 1);
            // Contract.Ensures(// Contract.Result<TVectorType>() != null);
            throw new NotImplementedException();
        }

        public override TMatrixType Repeat(int vertReps, int horizReps)
        {
            // Contract.Requires(horizReps > 0);
            // Contract.Requires(vertReps > 0);
            // Contract.Ensures(// Contract.Result<TMatrixType>() != null);
            // Contract.Ensures(// Contract.Result<TMatrixType>().Rows == this.Rows * vertReps);
            // Contract.Ensures(// Contract.Result<TMatrixType>().Columns == this.Columns * horizReps);

            throw new NotImplementedException();
        }

        public override void CopyTo(TMatrixType destMatrix, int row, int column)
        {           
            // Contract.Requires(destMatrix != null);
            // Contract.Requires(row >= 0);
            // Contract.Requires(column >= 0);
            // Contract.Requires(destMatrix.Rows - row >= this.Rows);
            // Contract.Requires(destMatrix.Columns - column >= this.Columns);
            throw new NotImplementedException();
        }

        public override TMatrixType ArrayMultiplication(TMatrixType lhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Requires(lhs.Rows == this.Rows);
            // Contract.Requires(lhs.Columns == this.Columns);
            // Contract.Ensures(// Contract.Result<TMatrixType>().Rows == this.Rows);
            // Contract.Ensures(// Contract.Result<TMatrixType>().Columns == this.Columns);
            throw new NotImplementedException();
        }

        protected override TMatrixType CreateMatrix(int rows, int columns)
        {
            // Contract.Requires(rows > 0 && columns > 0);
            // Contract.Requires(rows < int.MaxValue && columns < int.MaxValue);
            // Contract.Ensures(// Contract.Result<TMatrixType>() != null);
            throw new NotImplementedException();
        }

        protected override TVectorType CreateVector(int rows, int columns)
        {
            // Contract.Requires(rows == 1 || columns == 1);
            // Contract.Requires(rows > 0 && columns > 0);
            // Contract.Requires(rows < int.MaxValue && columns < int.MaxValue);
            // Contract.Ensures(// Contract.Result<TVectorType>() != null);
            throw new NotImplementedException();
        }

        #endregion

    }
}
