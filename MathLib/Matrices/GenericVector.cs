using System;
using System.Diagnostics.Contracts;

namespace MathLib.Matrices
{
    [Serializable]
    public class Vector<TValueType> : Matrix<TValueType>, 
        IVector<Vector<TValueType>, TValueType>
    {
      
        #region Constructors

        private Vector(int rows, int columns)
            : base(rows, columns)
        {
            // Contract.Requires(rows > 0);
            // Contract.Requires(columns > 0);            
            // Contract.Requires(rows == 1 || columns == 1);            
        } 

        public Vector(int dimension, VectorType orientation = VectorType.RowVector, TValueType initialValue = default(TValueType))
            : base(orientation == VectorType.RowVector ? 1 : dimension, orientation == VectorType.RowVector ? dimension : 1)

        {
            // Contract.Requires(dimension >= 1);

            if (orientation == VectorType.ColumnVector)
            {             
                if (!initialValue.Equals(default(TValueType)))
                    for (int r = 0; r < Rows; r++)
                        ValuesData[r, 0] = initialValue;
            }
            else
            {                                
                if (!initialValue.Equals(default(TValueType)))
                    for (int c = 0; c < Columns; c++)
                        ValuesData[0, c] = initialValue;
            }
        }

        public Vector(TValueType[] values, VectorType orientation = VectorType.RowVector)
            : base(orientation == VectorType.RowVector ? 1 : values.Length, orientation == VectorType.RowVector ? values.Length : 1)

        {
            // Contract.Requires(values != null);
            // Contract.Requires(values.Length > 0, "No values passed to vector constructor.");

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

        #endregion   

        public new int Rows
        {
            get
            {
                // Contract.Ensures(// Contract.Result<int>() == 1 || Columns == 1);
                return base.Rows;
            }
        }
        
        public new int Columns
        {
            get
            {
                // Contract.Ensures(// Contract.Result<int>() == 1 || Rows == 1);
                return base.Columns;
            }
        }


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
            get {
                return Rows == 1 ? Columns : Rows;
            }
        }

        public TValueType this[int index]
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

        public Vector<TValueType> ArrayMultiplication(Vector<TValueType> rhs)
        {
            Vector<TValueType> retVector = CreateVector(Rows, Columns);            

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    retVector.ValuesData[r, c] = OpMultiply(ValuesData[r, c], rhs.ValuesData[r, c]);

            return retVector;            
        }

        #endregion

        #region Operator overloads
        //////////////////////////////// operator overloads //////////////////////////////////      
        public static Vector<TValueType> operator -(Vector<TValueType> lhs, Vector<TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires(lhs.Rows == rhs.Rows);
            // Contract.Requires(lhs.Columns == rhs.Columns);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Columns == lhs.Columns);

            Vector<TValueType> retVector = new Vector<TValueType>(lhs.Rows, lhs.Columns);            

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                {
                    retVector.ValuesData[r, c] = OpSubtract(lhs.ValuesData[r, c],
                        rhs.ValuesData[r, c]);
                }
            return retVector;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Vector<TValueType> Subtract(Vector<TValueType> lhs, Vector<TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires(lhs.Rows == rhs.Rows);
            // Contract.Requires(lhs.Columns == rhs.Columns);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Columns == lhs.Columns);

            return lhs - rhs;
        }

        public static Vector<TValueType> operator +(Vector<TValueType> lhs, Vector<TValueType> rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Requires(rhs != null);
            // Contract.Requires(lhs.Rows == rhs.Rows);
            // Contract.Requires(lhs.Columns == rhs.Columns);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Columns == lhs.Columns);

            Vector<TValueType> retVector = new Vector<TValueType>(lhs.Rows, lhs.Columns);              

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    retVector.ValuesData[r,c] = OpAdd(lhs.ValuesData[r, c],
                        rhs.ValuesData[r, c]);

            return retVector;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Vector<TValueType> Add(Vector<TValueType> lhs, Vector<TValueType> rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires(lhs.Rows == rhs.Rows);
            // Contract.Requires(lhs.Columns == rhs.Columns);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Columns == lhs.Columns);

            return lhs + rhs;
        }

        public static Vector<TValueType> operator -(Vector<TValueType> arg)
        {
            // Contract.Requires(arg != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Rows == arg.Rows);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Columns == arg.Columns);

            Vector<TValueType> retVector = new Vector<TValueType>(arg.Rows, arg.Columns);              

            for (int r = 0; r < arg.Rows; r++)
                for (int c = 0; c < arg.Columns; c++)
                    retVector.ValuesData[r, c] = OpNegate(arg.ValuesData[r, c]);

            return retVector;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Vector<TValueType> Negate(Vector<TValueType> arg)
        {
            // Contract.Requires(arg != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Rows == arg.Rows);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Columns == arg.Columns);

            return -arg;
        }

        public static TValueType operator *(Vector<TValueType> lhs, Vector<TValueType> rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Requires(rhs != null);
            // Contract.Requires(lhs.Columns == rhs.Rows);
            // Contract.Requires(lhs.Rows == 1 && rhs.Columns == 1);

            TValueType result = OpMultiply(lhs.ValuesData[0, 0], rhs.ValuesData[0, 0]);

            int i;

            for (i = 1; i < lhs.Columns; i++)
                result = OpAddAssign(result, OpMultiply(lhs.ValuesData[0, i],
                    rhs.ValuesData[i, 0]));

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TValueType Multiply(Vector<TValueType> lhs, Vector<TValueType> rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Requires(rhs != null);
            // Contract.Requires(lhs.Columns == rhs.Rows);
            return lhs * rhs;
        }

        public static Vector<TValueType> operator *(Vector<TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Columns == lhs.Columns);

            Vector<TValueType> retVector = new Vector<TValueType>(lhs.Rows, lhs.Columns);              

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    retVector.ValuesData[r, c] = OpMultiply(lhs.ValuesData[r, c], rhs);

            return retVector;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Vector<TValueType> Multiply(Vector<TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Columns == lhs.Columns);

            return lhs * rhs;
        }

        public static Vector<TValueType> operator *(TValueType lhs, Vector<TValueType> rhs)
        {
            // Contract.Requires(rhs != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Rows == rhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Columns == rhs.Columns);

            return rhs * lhs;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Vector<TValueType> Multiply(TValueType lhs, Vector<TValueType> rhs)
        {
            // Contract.Requires(rhs != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Rows == rhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Columns == rhs.Columns);

            return rhs * lhs;
        }

        public static Vector<TValueType> operator /(Vector<TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Columns == lhs.Columns);

            Vector<TValueType> retVector = new Vector<TValueType>(lhs.Rows, lhs.Columns);              

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    retVector.ValuesData[r, c] = OpDivide(lhs.ValuesData[r, c], rhs);

            return retVector;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Vector<TValueType> Divide(Vector<TValueType> lhs, TValueType rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>() != null);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector<TValueType>>().Columns == lhs.Columns);
            return lhs / rhs;
        }

        ////////////////////////////////////////////////////////////////////////////////////////

        #endregion

        #region Object Invariants
        [ContractInvariantMethod]
// ReSharper disable UnusedMember.Local
        private void ObjectInvariant()
// ReSharper restore UnusedMember.Local
        {
            // Contract.Invariant(Rows > 0 && Columns > 0);
            // Contract.Invariant(Rows == 1 || Columns == 1);
            // Contract.Invariant(this.Rows == base.Rows && this.Columns == base.Columns);
        }
        #endregion
    }
}
