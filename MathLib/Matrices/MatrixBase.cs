using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MathLib.Matrices
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes"), Serializable]
    [ContractClass(typeof(MatrixBaseContract<,,>))]
    public abstract class MatrixBase<TMatrixType, TVectorType, TValueType>
        where TMatrixType : MatrixBase<TMatrixType, TVectorType, TValueType>
        where TVectorType : TMatrixType, IVector<TVectorType, TValueType>
    {

        protected static readonly Func<TValueType, TValueType, TValueType> OpAdd = GenericOperators.AddDelegate<TValueType>();
        protected static readonly Func<TValueType, TValueType, TValueType> OpSubtract = GenericOperators.SubtractDelegate<TValueType>();
        protected static readonly Func<TValueType, TValueType, TValueType> OpMultiply = GenericOperators.MultiplyDelegate<TValueType>();
        protected static readonly Func<TValueType, TValueType, TValueType> OpDivide = GenericOperators.DivideDelegate<TValueType>();
        protected static readonly Func<TValueType, TValueType, TValueType> OpAddAssign = GenericOperators.AddAssignDelegate<TValueType>();
        protected static readonly Func<TValueType, TValueType> OpNegate = GenericOperators.NegateDelegate<TValueType>();

        protected MatrixBase(int rows, int columns)
        {
            // Contract.Requires(rows > 0 && columns > 0);
            // Contract.Requires(rows < int.MaxValue && columns < int.MaxValue);
            // Contract.Ensures(Rows == rows);
            // Contract.Ensures(Columns == columns);

            _rows = rows;
            _columns = columns;            
        }        

        private int _rows;
        private int _columns;
        public int Rows { get { return _rows; } }
        public int Columns { get { return _columns; } }

        public bool IsSquare { get { return (Rows == Columns); } }

        protected abstract TVectorType CreateVector(int rows, int columns);
        protected abstract TMatrixType CreateMatrix(int rows, int columns);

        public abstract TVectorType GetRow(int row);
        public abstract TVectorType GetColumn(int column);
        public abstract void SetRow(int row, TVectorType rowVector);
        public abstract void SetColumn(int column, TVectorType columnVector);
        public abstract IEnumerable<TVectorType> RowEnumerator { get; }
        public abstract IEnumerable<TVectorType> ColumnEnumerator { get; }
        public abstract TMatrixType Transpose();
        public abstract TMatrixType Repeat(int vertReps, int horizReps);
        public abstract TValueType this[int row, int column] { get; set; }
        public abstract TVectorType AsVector();
        public abstract void CopyTo(TMatrixType destMatrix, int row, int col);

        public abstract TMatrixType ArrayMultiplication(TMatrixType rhs);

        [ContractInvariantMethod]
// ReSharper disable UnusedMember.Local
        private void ObjectInvariant()
// ReSharper restore UnusedMember.Local
        {
            // Contract.Invariant(Rows > 0);
            // Contract.Invariant(Columns > 0);
            // Contract.Invariant(Rows < int.MaxValue);
            // Contract.Invariant(Columns < int.MaxValue);
        }
    }
}
