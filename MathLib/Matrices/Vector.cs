using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;

using MathLib.Statistics;
using Util;

namespace MathLib.Matrices
{
    [Serializable]
    public class Vector : Matrix, IVector<Vector, double>, INumericVector, 
        IEnumerable<double>, IDeepCloneable<Vector>
    {
        #region Constructors

        internal Vector(int rows, int columns)
            : base(rows, columns) 
        {
            // Contract.Requires(rows > 0);
            // Contract.Requires(columns > 0);
            // Contract.Requires(rows == 1 || columns == 1);
            // Contract.Ensures(Rows == rows);
            // Contract.Ensures(Columns == columns);
        }

        public Vector(int dimension, VectorType orientation = VectorType.RowVector)
            : base(orientation == VectorType.RowVector ? 1 : dimension, orientation == VectorType.RowVector ? dimension : 1)

        {
            // Contract.Requires(dimension >= 1);
        }

        public Vector(int dimension, double initialValue, VectorType orientation = VectorType.RowVector)
            : base(orientation == VectorType.RowVector ? 1 : dimension, orientation == VectorType.RowVector ? dimension : 1)

        {
            // Contract.Requires(dimension >= 1);

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++ )
                    ValuesData[r, c] = initialValue;
            
        }

        public Vector(int dimension, VectorType orientation, INumberGenerator numberSource)
            : base(orientation == VectorType.RowVector ? 1 : dimension, orientation == VectorType.RowVector ? dimension : 1)
        {
            // Contract.Requires(dimension > 0);
            // Contract.Requires(numberSource != null);

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++ )
                    ValuesData[r, c] = numberSource.Number;            
        }

        public Vector(double[] values, VectorType orientation = VectorType.RowVector)
            : base(orientation == VectorType.RowVector ? 1 : values.Length, orientation == VectorType.RowVector ? values.Length : 1)

        {
            // Contract.Requires(values != null);
            // Contract.Requires(values.Length > 0);

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
                // Contract.Ensures(// Contract.OldValue(Rows) == Rows);
                // Contract.Ensures(// Contract.OldValue(base.Rows) == base.Rows);
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
       
        public new double[,] ValuesData
        {
            get
            {
                // Contract.Ensures(// Contract.Result<double[,]>() != null);
                // Contract.Ensures(// Contract.OldValue(Rows) == Rows);
                // Contract.Ensures(// Contract.OldValue(Columns) == Columns);
                return base.ValuesData;
            }
        }

        #region IVector Members

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

        public double this[int index]
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

                // Contract.Assert(Rows > 0);
                // Contract.Assert(Columns > 0);
            }
        }

        public Vector ArrayMultiplication(Vector rhs)
        {
            Vector retVector = CreateVector(Rows, Columns);

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    retVector.ValuesData[r, c] = OpMultiply(ValuesData[r, c], rhs.ValuesData[r, c]);

            return retVector;
        }

        #endregion

        #region INumericVector Members

        public double Norm
        {
            get 
            {
                double result = 0;
                if (Orientation == VectorType.RowVector)
                    for (int i = 0; i < Columns; i++)
                        result += ValuesData[0, i] * ValuesData[0, i];
                else
                    for (int i = 0; i < Rows; i++)
                        result += ValuesData[i, 0] * ValuesData[i, 0];

                return Math.Sqrt(result);                 
            }
        }

        public double NormSquared
        {
            get
            {
                double result = 0;
                if (Orientation == VectorType.RowVector)
                    for (int i = 0; i < 0 + Columns; i++)
                        result += ValuesData[0, i] * ValuesData[0, i];
                else
                    for (int i = 0; i < Rows; i++)
                        result += ValuesData[i, 0] * ValuesData[i, 0];

                return result;
            }
        }

        public double InfinityNorm
        {
            get
            {
                double result = Math.Abs(this[0]);
                for (int i = 1; i < Length; i++)
                {
                    double tmp = Math.Abs(this[i]);
                    if (tmp > result)
                        result = tmp;
                }

                return result;
            }
        }

        public double OneNorm
        {
            get
            {
                double result = 0;
                if (Orientation == VectorType.RowVector)
                    for (int i = 0; i < 0 + Columns; i++)
                        result += Math.Abs(ValuesData[0, i]);
                else
                    for (int i = 0; i < 0 + Rows; i++)
                        result += Math.Abs(ValuesData[i, 0]);

                return result;
            }
        }

        #endregion

        #region Operator overloads
        //////////////////////////////// operator overloads //////////////////////////////////      
        public static Vector operator -(Vector lhs, Vector rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Requires(rhs != null);            
            // Contract.Requires(lhs.Rows == rhs.Rows);
            // Contract.Requires(lhs.Columns == rhs.Columns);
            // Contract.Ensures(// Contract.Result<Vector>() != null);
            // Contract.Ensures(// Contract.Result<Vector>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector>().Columns == lhs.Columns);

            Vector retVector = new Vector(lhs.Rows, lhs.Columns);

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                {
                    retVector.ValuesData[r, c] = lhs.ValuesData[r, c] -
                        rhs.ValuesData[r, c];
                }

            return retVector;
        }

        public static Vector Subtract(Vector lhs, Vector rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Requires(rhs != null);   
            // Contract.Requires((lhs.Rows == rhs.Rows) && (lhs.Columns == rhs.Columns));
            // Contract.Ensures(// Contract.Result<Vector>() != null);
            // Contract.Ensures(// Contract.Result<Vector>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector>().Columns == lhs.Columns);

            return lhs - rhs;
        }

        public static Vector operator +(Vector lhs, Vector rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Requires(rhs != null);
            // Contract.Requires(lhs.Rows == rhs.Rows);
            // Contract.Requires(lhs.Columns == rhs.Columns);
            // Contract.Ensures(// Contract.Result<Vector>() != null);
            // Contract.Ensures(// Contract.Result<Vector>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector>().Columns == lhs.Columns);

            Vector retVector = new Vector(lhs.Rows, lhs.Columns);            

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    retVector.ValuesData[r, c] = lhs.ValuesData[r, c] +
                        rhs.ValuesData[r, c];

            return retVector;
        }

        public static Vector Add(Vector lhs, Vector rhs)
        {
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires((lhs.Rows == rhs.Rows) && (lhs.Columns == rhs.Columns));
            // Contract.Ensures(// Contract.Result<Vector>() != null);
            // Contract.Ensures(// Contract.Result<Vector>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector>().Columns == lhs.Columns);

            return lhs + rhs;
        }

        public static Vector operator -(Vector arg)
        {
            // Contract.Requires(arg != null);            
            // Contract.Ensures(// Contract.Result<Vector>() != null);
            // Contract.Ensures(// Contract.Result<Vector>().Rows == arg.Rows);
            // Contract.Ensures(// Contract.Result<Vector>().Columns == arg.Columns);

            Vector retVector = new Vector(arg.Rows, arg.Columns);            

            for (int r = 0; r < arg.Rows; r++)
                for (int c = 0; c < arg.Columns; c++)
                    retVector.ValuesData[r, c] = -arg.ValuesData[r, c];

            return retVector;
        }

        public static Vector Negate(Vector arg)
        {
            // Contract.Requires(arg != null);
            // Contract.Ensures(// Contract.Result<Vector>() != null);
            // Contract.Ensures(// Contract.Result<Vector>().Rows == arg.Rows);
            // Contract.Ensures(// Contract.Result<Vector>().Columns == arg.Columns);

            return -arg;
        }

        public static double operator *(Vector lhs, Vector rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Requires(rhs != null);
            // Contract.Requires(lhs.Columns == rhs.Rows);
            // Contract.Requires(lhs.Rows == 1 && rhs.Columns == 1);

            double result = default(double);

            int i;

            int r2 = 0;
            int c1 = 0;
            for (i = 0; i < lhs.Columns; i++, r2++, c1++)
                result += lhs.ValuesData[0, c1] * rhs.ValuesData[r2, 0];

            return result;
        }

        public static double Multiply(Vector lhs, Vector rhs)
        {
            // Contract.Requires(lhs != null);
            // Contract.Requires(rhs != null);
            // Contract.Requires(lhs.Columns == rhs.Rows);
            // Contract.Requires(lhs.Rows == 1 && rhs.Columns == 1);

            return lhs * rhs;
        }

        public static Vector operator *(Vector lhs, double rhs)
        {
            // Contract.Requires(lhs != null);            
            // Contract.Ensures(// Contract.Result<Vector>() != null);
            // Contract.Ensures(// Contract.Result<Vector>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector>().Columns == lhs.Columns);

            Vector retVector = new Vector(lhs.Rows, lhs.Columns);            

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    retVector.ValuesData[r, c] = lhs.ValuesData[r, c] * rhs;

            return retVector;
        }

        public static Vector Multiply(Vector lhs, double rhs)
        {
            // Contract.Requires(lhs != null);            
            // Contract.Ensures(// Contract.Result<Vector>() != null);
            // Contract.Ensures(// Contract.Result<Vector>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector>().Columns == lhs.Columns);

            return lhs * rhs;
        }

        public static Vector operator *(double lhs, Vector rhs)
        {
            // Contract.Requires(rhs != null);            
            // Contract.Ensures(// Contract.Result<Vector>() != null);
            // Contract.Ensures(// Contract.Result<Vector>().Rows == rhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector>().Columns == rhs.Columns);

            return rhs * lhs;
        }

        public static Vector Multiply(double lhs, Vector rhs)
        {
            // Contract.Requires(rhs != null);            
            // Contract.Ensures(// Contract.Result<Vector>() != null);
            // Contract.Ensures(// Contract.Result<Vector>().Rows == rhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector>().Columns == rhs.Columns);

            return rhs * lhs;
        }

        public static Vector operator /(Vector lhs, double rhs)
        {
            // Contract.Requires(lhs != null);            
            // Contract.Ensures(// Contract.Result<Vector>() != null);
            // Contract.Ensures(// Contract.Result<Vector>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector>().Columns == lhs.Columns);

            Vector retVector = new Vector(lhs.Rows, lhs.Columns);            

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    retVector.ValuesData[r, c] = lhs.ValuesData[r, c] / rhs;

            return retVector;
        }

        public static Vector Divide(Vector lhs, double rhs)
        {
            // Contract.Requires(lhs != null);            
            // Contract.Ensures(// Contract.Result<Vector>() != null);
            // Contract.Ensures(// Contract.Result<Vector>().Rows == lhs.Rows);
            // Contract.Ensures(// Contract.Result<Vector>().Columns == lhs.Columns);

            return lhs / rhs;
        }

        ////////////////////////////////////////////////////////////////////////////////////////

        #endregion

        #region IEnumerable<double> Members

        public IEnumerator<double> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
                yield return this[i];            
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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

        #region IDeepCloneable<Vector> Members

        public new Vector DeepClone()
        {
            Vector clone = new Vector(Rows, Columns);            

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    clone.ValuesData[r, c] = ValuesData[0 + r, 0 + c];

            return clone;
        }

        #endregion

        #region static methods
        public static double DotProduct(Vector v1, Vector v2)
        {
            // Contract.Requires(v1 != null && v2 != null);
            // Contract.Requires(v1.Rows == v2.Rows && v1.Columns == v2.Columns);

            return v1.Select((t, i) => t*v2[i]).Sum();
        }
        #endregion        
    }
}
