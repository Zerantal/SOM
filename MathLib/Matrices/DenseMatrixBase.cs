#region

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Text;
using Util;

#endregion

namespace MathLib.Matrices
{
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes"), Serializable]
    public abstract class DenseMatrixBase<TMatrixType, TVectorType, TValueType>
        : MatrixBase<TMatrixType, TVectorType, TValueType>, IDeepCloneable<TMatrixType>
        where TMatrixType : DenseMatrixBase<TMatrixType, TVectorType, TValueType>
        where TVectorType : TMatrixType, IVector<TVectorType, TValueType>
    {
        #region Constructors        

        internal DenseMatrixBase(int rows, int columns) : base(rows, columns)
        {
            // Contract.Requires(rows > 0);
            // Contract.Requires(columns > 0);            
            // Contract.Ensures(Rows == rows);
            // Contract.Ensures(Columns == columns);

            _values = new TValueType[rows,columns];
        }

        internal DenseMatrixBase(int rows, int columns, TValueType[,] values) : base(rows, columns)
        {
            // Contract.Requires(rows > 0);
            // Contract.Requires(columns > 0);
            // Contract.Requires(values != null);
            // Contract.Ensures(Rows == rows);
            // Contract.Ensures(Columns == columns);
      
            _values = values;            
        }

        protected DenseMatrixBase(int rows, int columns, TValueType initialValue) : base(rows, columns)
        {
            // Contract.Requires(rows > 0);
            // Contract.Requires(columns > 0);
            // Contract.Ensures(this.Columns == columns);
            // Contract.Ensures(this.Rows == rows);
            
            _values = new TValueType[Rows,Columns];

            if (!initialValue.Equals(default(TValueType)))
                for (int r = 0; r < Rows; r++)
                    for (int c = 0; c < Columns; c++)
                        ValuesData[r, c] = initialValue;
        }

        protected DenseMatrixBase(TValueType[,] values) : base(values.GetLength(0), values.GetLength(1))
        {
            // Contract.Requires(values != null);
            // Contract.Requires(values.GetLength(0) > 0 && values.GetLength(1) > 0);
            
            _values = new TValueType[Rows,Columns];

            Array.Copy(values, ValuesData, values.Length);
        }

        #endregion

        #region object override

        public override string ToString()
        {
            return ToString(true, " ");
        }

        public string ToString(bool displayBrackets, string delimiter)
        {
            StringBuilder retString = new StringBuilder();

            int rIdx = 0;
            for (int r = 0; r < Rows; r++, rIdx++)
            {
                int cIdx = 0;

                if (displayBrackets)
                    retString.Append("|");
                for (int c = 0; c < Columns - 1; c++, cIdx++)
                {
                    retString.AppendFormat("{0, -10:0.0000}", ValuesData[rIdx, cIdx]);
                    retString.Append(delimiter);
                }
                retString.AppendFormat("{0, -10:0.0000}", ValuesData[rIdx, Columns - 1]);

                if (displayBrackets)
                    retString.Append("|");

                if (r != Rows - 1) // not last row
                    retString.AppendLine();
            }

            return retString.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            TMatrixType m = obj as TMatrixType;

            if ((Object) m == null)
                return false;

            if (Rows != m.Rows || Columns != m.Columns)
                return false;

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (!(ValuesData[r, c].Equals(m.ValuesData[r, c])))
                        return false;

            return true;
        }

        public bool Equals(TMatrixType m)
        {
            if ((object) m == null)
                return false;

            if (Rows != m.Rows || Columns != m.Columns)
                return false;

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (!(ValuesData[r, c].Equals(m.ValuesData[r, c])))
                        return false;

            return true;
        }

        public override int GetHashCode()
        {
            return Rows ^ Columns;
        }

        #endregion        

        #region MatrixBase overrides        

        public override IEnumerable<TVectorType> RowEnumerator
        {
            get
            {
                for (int r = 0; r < Rows; r++)
                    yield return GetRow(r);
            }
        }

        public override IEnumerable<TVectorType> ColumnEnumerator
        {
            get
            {
                for (int c = 0; c < Columns; c++)
                    yield return GetColumn(c);
            }
        }

        public override TValueType this[int row, int column]
        {
            get { return ValuesData[row, column]; }
            set { ValuesData[row, column] = value; }
        }

        public override TVectorType GetRow(int row)
        {          
            TVectorType retVec = CreateVector(1, Columns);

            for (int c = 0; c < Columns; c++)
                retVec.ValuesData[0,c] = ValuesData[row, c];

            return retVec;
        }        

        public override TVectorType GetColumn(int column)
        {
            TVectorType retVec = CreateVector(Rows, 1);            

            for (int r = 0; r < Rows; r++)
                retVec.ValuesData[r, 0] = ValuesData[r, column];
            
            return retVec;
        }

        public override void SetRow(int row, TVectorType rowVector)
        {
            int r = row;            

            for (int c = 0; c < Columns; c++)
                ValuesData[r, c] = rowVector.ValuesData[0, c];
        }

        public override void SetColumn(int column, TVectorType columnVector)
        {
            int c = column;            

            for (int r = 0; r < Columns; r++)
                ValuesData[r, c] = columnVector.ValuesData[r, 0];
        }

        public override TMatrixType Transpose()
        {
            TMatrixType result = CreateMatrix(Columns, Rows);

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    result.ValuesData[c, r] = ValuesData[r, c];

            return result;
        }

        public override TMatrixType Repeat(int vertReps, int horizReps)
        {
            TMatrixType result = CreateMatrix(vertReps*Rows, horizReps*Columns);
            
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    for (int row = r; row < result.Rows; row += Rows)
                    {
                        for (int col = c; col < result.Columns; col += Columns)
                        {
                            result.ValuesData[row, col] = ValuesData[r, c];
                        }
                    }
                }
            }

            return result;
        }


        public override TVectorType AsVector()
        {
            TVectorType retVec = CreateVector(Rows, Columns);

            retVec._values = ValuesData;                 

            return retVec;
        }

        public override void CopyTo(TMatrixType destMatrix, int startRow, int startColumn)
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    destMatrix.ValuesData[startRow + r, startColumn + c] = ValuesData[r, c];
        }

        public override TMatrixType ArrayMultiplication(TMatrixType rhs)
        {
            TMatrixType retMatrix = CreateMatrix(Rows, Columns);

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    retMatrix.ValuesData[r, c] = OpMultiply(ValuesData[r, c], rhs.ValuesData[r, c]);

            return retMatrix;
        }

        #endregion

        #region internal methods / properties

        private TValueType[,] _values;
        protected internal TValueType[,] ValuesData
        {
            get
            {
                // Contract.Ensures(// Contract.Result<TValueType[,]>() != null);
                return _values;
            }
        }

        #endregion

        #region Operator overloads

        //////////////////////////////// operator overloads //////////////////////////////////
        public static bool operator ==(
            DenseMatrixBase<TMatrixType, TVectorType, TValueType> lhs,
            DenseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return true;

            if ((object) lhs == null || (object) rhs == null)
                return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(
            DenseMatrixBase<TMatrixType, TVectorType, TValueType> lhs,
            DenseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            return !(lhs == rhs);
        }

        public static TMatrixType operator -(
            DenseMatrixBase<TMatrixType, TVectorType, TValueType> lhs,
            DenseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires((lhs.Rows == rhs.Rows) && (lhs.Columns == rhs.Columns));

            TMatrixType result = lhs.CreateMatrix(lhs.Rows, lhs.Columns);
            
            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                {
                    result.ValuesData[r, c] = OpSubtract(lhs.ValuesData[r, c],
                                                      rhs.ValuesData[r, c]);
                }
            return result;
        }

        public static TMatrixType operator +(
            DenseMatrixBase<TMatrixType, TVectorType, TValueType> lhs,
            DenseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires((lhs.Rows == rhs.Rows) && (lhs.Columns == rhs.Columns));

            TMatrixType result = lhs.CreateMatrix(lhs.Rows,lhs.Columns);            

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    result.ValuesData[r, c] = OpAdd(lhs.ValuesData[r, c],
                                                 rhs.ValuesData[r, c]);

            return result;
        }

        public static TMatrixType operator -(DenseMatrixBase<TMatrixType, TVectorType, TValueType> arg)
        {
            // Contract.Requires(arg != null);

            TMatrixType result = arg.CreateMatrix(arg.Rows, arg.Columns);            

            for (int r = 0; r < arg.Rows; r++)
                for (int c = 0; c < arg.Columns; c++)
                    result.ValuesData[r, c] = OpNegate(arg.ValuesData[r, c]);

            return result;
        }

        public static TMatrixType operator *(
            DenseMatrixBase<TMatrixType, TVectorType, TValueType> lhs,
            DenseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires(lhs.Columns == rhs.Rows);

            TMatrixType result = lhs.CreateMatrix(lhs.Rows, rhs.Columns);            

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < rhs.Columns; c++)
                {
                    TValueType sum = default(TValueType);
                    int i;
                    int r2 = 0, c2 = 0;                        
                    for (i = 0; i < lhs.Columns; i++, r2++, c2++)
                        sum = OpAddAssign(sum,
                                           OpMultiply(lhs.ValuesData[r, c2],
                                                       rhs.ValuesData[r2, c]));
                    result.ValuesData[r, c] = sum;
                }

            return result;
        }

        public static TMatrixType operator *(DenseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);

            TMatrixType result = lhs.CreateMatrix(lhs.Rows, lhs.Columns);            

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    result.ValuesData[r, c] = OpMultiply(lhs.ValuesData[r, c], rhs);

            return result;
        }

        public static TMatrixType operator *(TValueType lhs, DenseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            // Contract.Requires(rhs != null);

            return rhs*lhs;
        }

        public static TMatrixType operator /(DenseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);

            TMatrixType result = lhs.CreateMatrix(lhs.Rows, lhs.Columns);
            result._values = new TValueType[result.Rows,result.Columns];

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    result.ValuesData[r, c] = OpDivide(lhs.ValuesData[r, c], rhs);

            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////////////

        #endregion

        #region IDeepCloneable<TMatrixType> Members

        public virtual TMatrixType DeepClone()
        {
            Type t = typeof (TValueType);
            IDeepCloneable<TValueType> element;
            TMatrixType clone = CreateMatrix(Rows, Columns);

            if (t.IsValueType)
            {
                for (int r = 0; r < Rows; r++)
                    for (int c = 0; c < Columns; c++)
                        clone.ValuesData[r, c] = ValuesData[r, c];
            }
            else if (t.GetInterface("IDeepCloneable") != null)
            {
                for (int r = 0; r < Rows; r++)
                    for (int c = 0; c < Columns; c++)
                    {
                        element = ValuesData[r, c] as IDeepCloneable<TValueType>;
                        // Contract.Assume(element != null);
                        clone.ValuesData[r, c] = element.DeepClone();
                    }
                       
            }
            else
            {
                throw new InvalidOperationException("The generic parameter of matrix doesn't implement " +
                                                    "the IDeepCloneable interface.");
            }

            return clone;
        }

        #endregion

        #region Contract Invariants

        [ContractInvariantMethod]
// ReSharper disable UnusedMember.Local
        private void ObjectInvariants()
// ReSharper restore UnusedMember.Local
        {
            // Contract.Invariant(_values != null);
            // Contract.Invariant(_values.GetLength(0) == Rows);
            // Contract.Invariant(_values.GetLength(1) == Columns);
        }

        #endregion
    }
}