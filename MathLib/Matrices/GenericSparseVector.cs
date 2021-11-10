using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;

namespace MathLib.Matrices
{
    [Serializable]
    public class SparseVector<TValueType> : SparseMatrix<TValueType>, 
        IVector<SparseVector<TValueType>, TValueType>
    {
        #region Constructors

        public SparseVector(int dimension, VectorType orientation = VectorType.RowVector)
            : base(orientation == VectorType.RowVector ? 1 : dimension, orientation == VectorType.RowVector ? dimension : 1)
        {
            // Contract.Requires(dimension > 0);
            // Contract.Requires(dimension < int.MaxValue);
        }

        public SparseVector(int dimension, VectorType orientation, IEnumerable<Tuple<int, TValueType>> initialValues)
            : base(orientation == VectorType.RowVector ? 1 : dimension, orientation == VectorType.RowVector ? dimension : 1)

        {
            // Contract.Requires(dimension > 0);
            // Contract.Requires(dimension < int.MaxValue);
            // Contract.Requires(initialValues != null);
            // Contract.Requires(// Contract.ForAll<Tuple<int, TValueType>>
                //(initialValues, new Predicate<Tuple<int, TValueType>>(
                //                    t => (t != null && t.Item1 < dimension && t.Item1 >= 0))));

            Tuple<int, int, TValueType>[] values;
            
            if (orientation == VectorType.RowVector)
            {
                values = initialValues.Select(
                    element => new Tuple<int, int, TValueType>(0, element.Item1, element.Item2)).ToArray();
            }
            else
            {             
                values = initialValues.Select(
                    element => new Tuple<int, int, TValueType>(element.Item1, 0, element.Item2)).ToArray();
            }

            PopulateMatrix(values);
        }

        #endregion

        #region IVector<TValueType> Members

        public VectorType Orientation
        {
            get
            {
                return Rows == 1 ? VectorType.RowVector : VectorType.ColumnVector;
            }
        }

        public int Length
        {
            get
            {
                return Rows == 1 ? Columns : Rows;
            }
        }

        public TValueType this[int index]
        {
            get {
                return Rows == 1 ? this[0, index] : this[index, 0];
            }
            set
            {
                if (Rows == 1)
                    this[0, index] = value;                
                else
                    this[index, 0] = value;
            }
        }

        public SparseVector<TValueType> ArrayMultiplication(SparseVector<TValueType> rhs)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region GenericSparseVector methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public new IEnumerable<Tuple<int, TValueType>> ValueEnumerator
        {
            get
            {                
                if (Orientation == VectorType.RowVector)
                {                    
                    for (int i = RowPtrs[0]; i < RowPtrs[0+1]; i++)
                        yield return new Tuple<int, TValueType>(ColIndices[i], Values[i]);                        
                }
                else
                {
                    for (int i = RowPtrs[0]; i < RowPtrs[Rows]; i++)
                    {
                        int column = ColIndices[i];
                        if (column == 0)
                            yield return new Tuple<int, TValueType>(column, Values[i]);
                    }
                }
            }
        }

        #endregion
        #region Operator overloads
        //////////////////////////////// operator overloads //////////////////////////////////
        // basic overload operators for the following interactions:
        // SparseVector <--> SparseVector
        // SparseVector <--> TValueType

        public static SparseVector<TValueType> operator -(SparseVector<TValueType> lhs, SparseVector<TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires((lhs.Rows == rhs.Rows) && (lhs.Columns == rhs.Columns));
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Columns == lhs.Columns);

            SparseVector<TValueType> result = (SparseVector<TValueType>)lhs.DeepClone();

            foreach (Tuple<int, TValueType> val in rhs.ValueEnumerator)
                result[val.Item1] = OpSubtract(result[val.Item1], val.Item2);

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static SparseVector<TValueType> Subtract(SparseVector<TValueType> lhs, SparseVector<TValueType> rhs) { return lhs - rhs; }

        public static SparseVector<TValueType> operator +(SparseVector<TValueType> lhs, SparseVector<TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires((lhs.Rows == rhs.Rows) && (lhs.Columns == rhs.Columns));
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Columns == lhs.Columns);

            SparseVector<TValueType> result = (SparseVector<TValueType>)lhs.DeepClone();

            foreach (Tuple<int, TValueType> val in rhs.ValueEnumerator)
                result[val.Item1] = OpAdd(result[val.Item1], val.Item2);

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static SparseVector<TValueType> Add(SparseVector<TValueType> lhs, SparseVector<TValueType> rhs) { return lhs + rhs; }

        public static SparseVector<TValueType> operator -(SparseVector<TValueType> arg)
        {
            // Contract.Requires(arg != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Rows == arg.Rows);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Columns == arg.Columns);

            SparseVector<TValueType> result = (SparseVector<TValueType>)arg.DeepClone();

            for (int i = 0; i < result.Values.Count(); i++)
                result.Values[i] = OpNegate(result.Values[i]);

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static SparseVector<TValueType> Negate(SparseVector<TValueType> arg)
        {
            // Contract.Requires(arg != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Rows == arg.Rows);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Columns == arg.Columns);
            return -arg; 
        }

        public static TValueType operator *(SparseVector<TValueType> lhs, SparseVector<TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires(lhs.Columns == rhs.Rows);

            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TValueType Multiply(SparseVector<TValueType> lhs, SparseVector<TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires(lhs.Columns == rhs.Rows);
            return lhs * rhs;
        }

        public static SparseVector<TValueType> operator *(SparseVector<TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Columns == lhs.Columns);

            SparseVector<TValueType> result = (SparseVector<TValueType>)lhs.DeepClone();

            for (int i = 0; i < result.Values.Count(); i++)
                result.Values[i] = OpMultiply(result.Values[i], rhs);

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static SparseVector<TValueType> Multiply(SparseVector<TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Columns == lhs.Columns);

            return lhs * rhs;
        }

        public static SparseVector<TValueType> operator *(TValueType lhs, SparseVector<TValueType> rhs)
        {
            // Contract.Requires(rhs != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Rows == rhs.Rows);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Columns == rhs.Columns);

            return rhs * lhs;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static SparseVector<TValueType> Multiply(TValueType lhs, SparseVector<TValueType> rhs)
        {
            // Contract.Requires(rhs != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Rows == rhs.Rows);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Columns == rhs.Columns);

            return rhs * lhs;
        }

        public static SparseVector<TValueType> operator /(SparseVector<TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Columns == lhs.Columns);

            SparseVector<TValueType> result = (SparseVector<TValueType>)lhs.DeepClone();

            for (int i = 0; i < result.Values.Count(); i++)
                result.Values[i] = OpDivide(result.Values[i], rhs);

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static SparseVector<TValueType> Divide(SparseVector<TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<SparseVector<TValueType>>().Columns == lhs.Columns);

            return lhs / rhs;
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
