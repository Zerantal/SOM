using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Util;

namespace MathLib.Matrices
{
    //[ContractVerification(false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes"), Serializable]
    public abstract class SparseMatrixBase<TMatrixType, TVectorType, TValueType>
        : MatrixBase<TMatrixType, TVectorType, TValueType>, IDeepCloneable<TMatrixType>
        where TMatrixType : SparseMatrixBase<TMatrixType, TVectorType, TValueType>
        where TVectorType : TMatrixType, IVector<TVectorType, TValueType>
    {
        class MatrixElementComparer : IEqualityComparer<Tuple<int, int, TValueType>>
        {
            #region IEqualityComparer<Tuple<int,int,TValueType>> Members

            bool IEqualityComparer<Tuple<int, int, TValueType>>.Equals(Tuple<int, int, TValueType> x, Tuple<int, int, TValueType> y)
            {
                // Contract.Assume(x != null && y != null);              
                return (x.Item1 == y.Item1 && x.Item2 == y.Item2);
            }

            int IEqualityComparer<Tuple<int, int, TValueType>>.GetHashCode(Tuple<int, int, TValueType> obj)
            {
                // Contract.Assume(obj != null);
                return ~obj.Item1 ^ obj.Item2;
            }

            #endregion
        }

        #region Constructors

        /// <summary>
        /// Create a SparseMatrix with the specified dimensions but no non-zero elements
        /// </summary>
        /// <param name="rows">The number of rows in the matrix</param>
        /// <param name="columns">The number of columns in the matrix</param>
        protected internal SparseMatrixBase(int rows, int columns) : base(rows, columns)
        {
            // Contract.Requires(rows >= 1 && columns >= 1);
            // Contract.Requires(rows < int.MaxValue);

            RowPtrs = new List<int>();
            RowPtrs.AddRange(new int[Rows + 1]);
            Values = new List<TValueType>();
            ColIndices = new List<int>();
        }

        /// <summary>
        /// Create SparseMatrix with the specified number of rows and columns, initialised with the specified
        /// values
        /// </summary>
        /// <param name="rows">The number of rows for the new matrix</param>
        /// <param name="columns">The number of columns for the new matrix</param>
        /// <param name="values">An array of values to initialise matrix with</param>
        protected internal SparseMatrixBase(int rows, int columns, IEnumerable<Tuple<int, int, TValueType>> values)
            : base(rows, columns)
        {
            // Contract.Requires(rows >= 1 && columns >= 1);
            // Contract.Requires(rows < int.MaxValue);
            // Contract.Requires(values != null);
            // Contract.Requires(// Contract.ForAll
                //(values, t => (t != null && t.Item1 < rows && t.Item1 >= 0 && t.Item2 < columns && t.Item2 >= 0)));

            PopulateMatrix(values);
        }
        
        #endregion

        #region SparseMatrixBase Methods

        protected internal List<int> RowPtrs { get; private set; }

        protected internal List<TValueType> Values { get; private set; }

        protected internal List<int> ColIndices { get; private set; }

        protected void PopulateMatrix(IEnumerable<Tuple<int, int, TValueType>> values)
        {
            // Contract.Requires(values != null);
            // Contract.Requires(// Contract.ForAll(values,
                                              //t =>
                                              //(t != null && t.Item1 < Rows &&
                                              // t.Item2 < Columns)));

            // order tuples by row and then by column
            var sortedValues = values.OrderBy(t => t.Item1).ThenBy(t => t.Item2);            
            
            // remove duplicate entries (if any)
            var valuesWithoutDups = sortedValues.Distinct(new MatrixElementComparer());

            // remove zero entries from Tuple array
            var finalValueList = valuesWithoutDups.Where(t => !t.Item3.Equals(default(TValueType))).ToArray();                   
            
            var valList = new List<TValueType>();
            var colIndexList = new List<int>();
            var rowPtrList = new List<int>(Rows + 1);
            foreach (Tuple<int, int, TValueType> t in finalValueList)
            {
                // Contract.Assume(t != null);
                valList.Add(t.Item3);
                colIndexList.Add(t.Item2);
            }            
            // Contract.Assert(valList.Count == colIndexList.Count);
            // may not be the most efficient way of getting row pointers ;)
            RowPtrs.Add(0);
            var rowLengths = new int[Rows];
            for (int r = 0; r < Rows; r++)
            {
                // count number of values in row
                int r1 = r;
                rowLengths[r] = finalValueList.Where(t => t.Item1 == (r1)).Count();
            }

            for (int r = 0; r < Rows; r++)
                RowPtrs.Add(RowPtrs[r] + rowLengths[r]);

            Values = valList;
            ColIndices = colIndexList;
            RowPtrs = rowPtrList;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IEnumerable<Tuple<int, int, TValueType>> ValueEnumerator
        {
            get
            {
                for (int r = 0; r < Rows; r++)
                {
                    for (int i = RowPtrs[r]; i < RowPtrs[r + 1]; i++)
                    {
                        int column = ColIndices[i];
                        if (column >= 0 && column < Columns)
                            yield return new Tuple<int, int, TValueType>(r, column, Values[i]);
                    }
                }
            }
        }

        public int NumberOfNonzeroElements 
        { 
            get
            {
                return Values.Count();                
            }
        }

        #endregion

        #region MatrixBase overrides

        public override TVectorType GetRow(int row)
        {
            TVectorType retVec = CreateVector(1, Columns);

            int firstRowIdx = RowPtrs[row];
            int lastRowIdx = RowPtrs[row + 1];
            // Contract.Assert(firstRowIdx > 0);
            // Contract.Assert(lastRowIdx >= firstRowIdx);

            for (int r = firstRowIdx; r < lastRowIdx; r++)
            {
                retVec.Values.Add(Values[r]);
                retVec.ColIndices.Add(ColIndices[r]);
            }

            retVec.RowPtrs.Clear();
            retVec.RowPtrs.Add(0);
            retVec.RowPtrs.Add(lastRowIdx - firstRowIdx);

            return retVec;
        }

        public override TVectorType GetColumn(int column)
        {
            TVectorType retVector = CreateVector(Rows, 1);

            int rowPtrCounter = 0;
            for (int r = 0; r < Rows; r++)
            {
                for (int valueIdx = RowPtrs[r]; valueIdx < RowPtrs[r + 1]; valueIdx++)
                {
                    if (ColIndices[valueIdx] != column) continue;

                    retVector.Values.Add(Values[valueIdx]);
                    rowPtrCounter++;
                    retVector.ColIndices.Add(0);
                    continue;
                }
                retVector.RowPtrs[r+1] = rowPtrCounter;
            }
            return retVector;
        }

        public override void SetRow(int row, TVectorType rowVector)
        {
            throw new NotImplementedException();
        }

        public override void SetColumn(int column, TVectorType columnVector)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TVectorType> RowEnumerator
        {
            get { throw new NotImplementedException(); }
        }

        public override IEnumerable<TVectorType> ColumnEnumerator
        {
            get { throw new NotImplementedException(); }
        }

        public override TMatrixType Transpose()
        {
            throw new NotImplementedException();
        }

        public override TMatrixType Repeat(int vertReps, int horizReps)
        {
            throw new NotImplementedException();
        }

        public override TValueType this[int row, int column]
        {
            get
            {
                int rowIdx = RowPtrs[row];
                int nextRowIdx = RowPtrs[row + 1];

                for (int i = rowIdx; i < nextRowIdx; i++)
                    if (ColIndices[i] == column)
                        return Values[i];

                return default(TValueType);                
            }
            // four scenarios have to be considered in the setter:
            // i) replacing a nonzero element with a zero
            // ii) add new non zero element into matrix
            // iii) replace existing non zero element with another
            // iv) adding a zero element to matrix (can be ignored)
            set
            {
                int valIdxOfRow = RowPtrs[row];
                int valIdxOfNextRow = RowPtrs[row + 1];
                int i;
                bool elementAlreadyExists = false;
                int effectiveColumn = column;
                for (i = valIdxOfRow; i < valIdxOfNextRow; i++)
                {
                    if (ColIndices[i] == effectiveColumn)   // replace existing non zero element of matrix
                    {
                        elementAlreadyExists = true;
                        break;
                    }
                    if (ColIndices[i] > effectiveColumn)  // insert new non zero element into matrix
                    {
                        break;
                    }
                }
                if (elementAlreadyExists)
                {
                    if (Equals(value, default(TValueType)))  // i)
                    {
                        Values.RemoveAt(i);
                        ColIndices.RemoveAt(i);
                        for (int r = row + 1; r < RowPtrs.Count(); r++)
                            RowPtrs[r]--;                        
                    }
                    else // iii)
                    {
                        Values[i] = value;
                        ColIndices[i] = effectiveColumn;
                    }
                }
                else
                {
                    if (!Equals(value, default(TValueType))) // ii)
                    {
                        Values.Insert(i, value);
                        ColIndices.Insert(i, effectiveColumn);
                        for (int r = row + 1; r < RowPtrs.Count(); r++)
                            RowPtrs[r]++;                        
                    }
                }
            }
        }

        public override TVectorType AsVector()
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(TMatrixType destMatrix, int row, int col)
        {
            throw new NotImplementedException();
        }

        public override TMatrixType ArrayMultiplication(TMatrixType lhs)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Object overrides
        
        public override bool Equals(object obj)
        {
            SparseMatrixBase<TMatrixType, TVectorType, TValueType> sm = obj as SparseMatrixBase<TMatrixType, TVectorType, TValueType>;            
            if ((object)sm == null)
                return false;

            if (Rows != sm.Rows || Columns != sm.Columns)           
                return false;

            if (NumberOfNonzeroElements != sm.NumberOfNonzeroElements)
                return false;

            // this is not that efficient!
            List<Tuple<int, int, TValueType>> valueList1 = ValueEnumerator.ToList();

            List<Tuple<int, int, TValueType>> valueList2 = sm.ValueEnumerator.ToList();

            return valueList1.SequenceEqual(valueList2);
        }

        public bool Equals(TMatrixType matrix)
        {
            if ((object)matrix == null)
                return false;

            if (matrix.Rows != Rows || matrix.Columns != Columns)
                return false;


            if (NumberOfNonzeroElements != matrix.NumberOfNonzeroElements)
                return false;

            for (int i = 0; i < Values.Count(); i++)
            {
                if (!(Values[i].Equals(matrix.Values[i])) || (ColIndices[i] != matrix.ColIndices[i]))
                    return false;
            }

            for (int r = 0; r <= Rows; r++)
                if (RowPtrs[r] != matrix.RowPtrs[r])
                    return false;

            return true;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            for (int rIdx = 0; rIdx < Rows; rIdx++)
            {
                for (int valIdx = RowPtrs[rIdx]; valIdx < RowPtrs[rIdx+1]; valIdx++)
                {
                    int column = ColIndices[valIdx];
                    if (column >= 0 && column < Columns)
                        str.AppendLine("(" + (rIdx) + ", " + column + ", " + Values[valIdx] + ")");
                }
            }

            return str.ToString();            
        }

        public override int GetHashCode()
        {
            return Rows ^ Columns;
        }

        #endregion

        #region Operator overloads
        //////////////////////////////// operator overloads //////////////////////////////////
        // basic overload operators for the following interactions:
        // SparseMatrix <--> SparseMatrix
        // SparseMatrix <--> TValueType
        public static bool operator ==(SparseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, SparseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return true;

            if ((object)lhs == null || (object)rhs == null)
                return false;

            return lhs.Equals(rhs);

        }

        public static bool operator !=(SparseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, SparseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            return !(lhs == rhs);
        }

        public static TMatrixType operator -(SparseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, SparseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires((lhs.Rows == rhs.Rows) && (lhs.Columns == rhs.Columns));

            TMatrixType result = lhs.DeepClone();

            foreach (Tuple<int, int, TValueType> val in rhs.ValueEnumerator)
                result[val.Item1, val.Item2] = OpSubtract(result[val.Item1, val.Item2], val.Item3);

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TMatrixType Subtract(SparseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, SparseMatrixBase<TMatrixType, TVectorType, TValueType> rhs) { return lhs - rhs; }

        public static TMatrixType operator +(SparseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, SparseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires((lhs.Rows == rhs.Rows) && (lhs.Columns == rhs.Columns));

            TMatrixType result = lhs.DeepClone();

            foreach (Tuple<int, int, TValueType> val in rhs.ValueEnumerator)
                result[val.Item1, val.Item2] = OpAdd(result[val.Item1, val.Item2], val.Item3);

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TMatrixType Add(SparseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, SparseMatrixBase<TMatrixType, TVectorType, TValueType> rhs) { return lhs + rhs; }

        public static TMatrixType operator -(SparseMatrixBase<TMatrixType, TVectorType, TValueType> arg)
        {
            // Contract.Requires(arg != null);

            TMatrixType result = arg.DeepClone();

            for (int i = 0; i < result.Values.Count(); i++)
                result.Values[i] = OpNegate(result.Values[i]);

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TMatrixType Negate(SparseMatrixBase<TMatrixType, TVectorType, TValueType> arg)
        {
            // Contract.Requires(arg != null);
            return -arg;
        }

        public static TMatrixType operator *(SparseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, SparseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires(lhs.Columns == rhs.Rows);

            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TMatrixType Multiply(SparseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, SparseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires(lhs.Columns == rhs.Rows);
            return lhs * rhs;
        }

        public static TMatrixType operator *(SparseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);

            TMatrixType result = lhs.DeepClone();

            for (int i = 0; i < result.Values.Count(); i++)
                result.Values[i] = OpMultiply(result.Values[i], rhs);

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TMatrixType Multiply(SparseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);
            return lhs * rhs;
        }

        public static TMatrixType operator *(TValueType lhs, SparseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            // Contract.Requires(rhs != null);

            return rhs * lhs;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TMatrixType Multiply(TValueType lhs, SparseMatrixBase<TMatrixType, TVectorType, TValueType> rhs)
        {
            // Contract.Requires(rhs != null);
            return rhs * lhs;
        }

        public static TMatrixType operator /(SparseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);

            TMatrixType result = lhs.DeepClone();

            for (int i = 0; i < result.Values.Count(); i++)
                result.Values[i] = OpDivide(result.Values[i], rhs);

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TMatrixType Divide(SparseMatrixBase<TMatrixType, TVectorType, TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);
            return lhs / rhs; 
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Object invariants
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(Values != null);
            // Contract.Invariant(RowPtrs != null);
            // Contract.Invariant(ColIndices != null);
            // Contract.Invariant(RowPtrs.Count == Rows + 1);         
            // Contract.Invariant(Values.Count == ColIndices.Count);
            // Contract.Invariant(// Contract.ForAll(RowPtrs, r => r >= 0));
            // Contract.Invariant(// Contract.ForAll(RowPtrs, r => r <= ColIndices.Count));
            // Contract.Invariant(// Contract.ForAll(0, RowPtrs.Count-2, r => (RowPtrs[r+1] >= RowPtrs[r])));
        }
        #endregion

        #region IDeepCloneable<TMatrixType> Members

        public virtual TMatrixType DeepClone()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
