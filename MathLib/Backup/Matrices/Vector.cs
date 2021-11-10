using System;
using System.Collections.Generic;
using System.Text;

namespace MathLib.Matrices
{
    /// <summary>
    /// Indicates whether a Vector is a row vector or a column vector.
    /// </summary>
    public enum VectorType 
    {
        /// <summary>
        /// Row vector.
        /// </summary>
        RowVector, 
        /// <summary>
        /// Column vector.
        /// </summary>
        ColumnVector 
    };

    /// <summary>
    /// Vector of double precision values
    /// </summary>
    [Serializable]
    public class Vector : ICloneable
    {
        internal Matrix _mat;
        private int _Dimension;

        ///<summary>
        /// Initialise a new instance of the <see cref="Vector"/> class with a specified
        /// dimension.
        ///</summary>
        ///<param name="dim">Dimension of new <see cref="Vector"/>.</param>
        /// <remarks>New vector is a row vector by default.</remarks>
        ///<exception cref="ArgumentOutOfRangeException">Dimension is less
        /// than or equal to zero.</exception>
        public Vector(int dim) : this(dim, VectorType.RowVector) { }

        ///<summary>
        /// Initialise a new instance of the <see cref="Vector"/> class with a specified
        /// dimension and orientation.
        ///</summary>
        ///<param name="dim">Dimension of new <see cref="Vector"/>.</param>
        ///<param name="type">Orientation of new <see cref="Vector"/>: 
        /// row vector or column vector.</param>
        ///<exception cref="ArgumentOutOfRangeException">Dimension is less
        /// than or equal to zero.</exception>
        public Vector(int dim, VectorType type)
        {
            if (dim < 1)
                throw new ArgumentOutOfRangeException("zero or negative size passed to constructor",
                        (Exception)null);
            
            _Dimension = dim;
            if (type == VectorType.ColumnVector)
                _mat = new Matrix(dim, 1);
            else
                _mat = new Matrix(1, dim);            
        }

        ///<summary>
        /// Initialise a new instance of the <see cref="Vector"/> class with a specific
        /// content.
        ///</summary>
        ///<param name="values">The values to initialise the <see cref="Vector"/> with.</param>
        ///<exception cref="ArgumentNullException">values is null.</exception>
        public Vector(double[] values) : this(values, VectorType.RowVector) { }

        ///<summary>
        /// Initialise a new instance of the <see cref="Vector"/> class with a specific
        /// content and orientation.
        ///</summary>
        ///<param name="values">The values to initialise the <see cref="Vector"/> with.</param>
        ///<param name="type">Orientation of new <see cref="Vector"/>: 
        /// row vector or column vector.</param>
        ///<exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public Vector(double[] values, VectorType type)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            _Dimension = values.Length;
            if (type == VectorType.RowVector)
            {
                _mat = new Matrix(1, values.Length);
                _mat._Values[0] = (double[])values.Clone();
            }
            else
            {
                _mat = new Matrix(values.Length, 1);
                for (int r = 0; r < values.Length; r++)
                    _mat._Values[r][0] = values[r];
            }
        }

        ///<summary>
        /// Initialise a new instance of the <see cref="Vector"/> class with the 
        /// content of another <see cref="Vector"/>.
        ///</summary>
        ///<param name="v">Vector whos values to copy.</param>
        ///<exception cref="ArgumentNullException"><paramref name="v"/> is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "v")]
        public Vector(Vector v)
        {
            if (v == null)
                throw new ArgumentNullException("v");

            _Dimension = v._Dimension;
            _mat = (Matrix)v._mat.Clone();            
        }

        ///<summary>
        /// Initialise a new instance of the <see cref="Vector"/> class with the
        /// content of a <see cref="Matrix"/>.
        ///</summary>
        ///<param name="arg"><see cref="Matrix"/> whos values to copy.</param>
        ///<exception cref="ArgumentNullException"><paramref name="arg"/> is null.</exception>
        ///<exception cref="ArgumentException"><paramref name="arg"/> is not a 
        /// <see cref="Vector"/>.</exception>
        /// <remarks>Parameter <paramref name="arg"/> must be a <see cref="Matrix"/>
        /// with either one row or one column.</remarks>
        public Vector(Matrix arg)
        {
            if (arg == null)
                throw new ArgumentNullException("arg");
            if (arg._Rows != 1 && arg._Columns != 1 )            
                throw new ArgumentException("Matrix is not mat vector.");                            
            
            _mat = (Matrix)arg.Clone();            
            if  (_mat._Rows == 1)
                _Dimension = _mat._Columns;
            else
                _Dimension = _mat._Rows;
        }
      
        ///<summary>
        /// Return the dimensionality of <see cref="Vector"/>.
        ///</summary>
        public int Length { get { return _Dimension; } }

        /// <summary>
        /// Gets or sets the value of the specified element of the <see cref="Vector"/>.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <value>The value at the specified index.</value>
        /// <remarks>The <paramref name="index"/> parameters uses zero 
        /// based indexing.</remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is either less
        /// than zero or greater than or equal to the total length of this <see cref="Vector"/>
        /// </exception>
        public double this[int index]
        {
            get
            {
                if (index < 0 || index >= _Dimension)
                    throw new ArgumentOutOfRangeException("index");

                if (_mat._Rows == 1)
                    return _mat._Values[0][index];
                else
                    return _mat._Values[index][0];
            }

            set
            {
                if (index < 0 || index >= _Dimension)
                    throw new ArgumentOutOfRangeException("index");

                if (_mat._Rows == 1)
                    _mat._Values[0][index] = value;
                else
                    _mat._Values[index][0] = value;
            }
        }

        ///<summary>
        /// Returns the orientation of the <see cref="Vector"/>.
        ///</summary>
        public VectorType Orientation
        {
            get
            {
                if (_mat._Rows == 1)
                    return VectorType.RowVector;
                else
                    return VectorType.ColumnVector;
            }
        }

        ///<summary>
        /// Returns the euclidean norm of the <see cref="Vector"/>
        ///</summary>
        public double Norm
        {
            get
            {
                double result = 0;
                if (this.Orientation == VectorType.RowVector)
                    for (int i = 0; i < _Dimension; i++)
                        result += _mat._Values[0][i] * _mat._Values[0][i];
                else
                    for (int i = 0; i < _Dimension; i++)
                        result += _mat._Values[i][0] * _mat._Values[i][0];

                return Math.Sqrt(result);
            }
        }
                
        ///<summary>
        /// Find the minimum value in the <see cref="Vector"/>.
        ///</summary>
        ///<returns>The minimum value.</returns>
        public double Min()
        {
            double min = double.MaxValue;
            double tmp;            
            for (int i = 0; i < Length; i++)
            {
                tmp = this[i];
                if (tmp < min)                
                    min = tmp;                                   
            }
           
            return min;
        }

        ///<summary>
        /// Find index of minimum value.
        ///</summary>
        ///<returns>The index of the minimum value.</returns>
        public int MinIndex()
        {
            double min = double.MaxValue;
            double tmp;
            int minIdx = 0;
            for (int i = 0; i < Length; i++)
            {
                tmp = this[i];
                if (tmp < min)
                {
                    min = tmp;
                    minIdx = i;
                }
            }

            return minIdx;
        }


        public static Vector Concat(Vector vector0, Vector vector1)
        {
            if (vector0.Orientation != vector0.Orientation)
                throw new ArgumentException("Vectors must have the same orientation in order to be concatenated");

            Vector result = new Vector(vector0.Length + vector1.Length, vector0.Orientation);
            int i;
            int newVectorIdx = 0;
            for (i = 0; i < vector0.Length; i++)
                result[i] = vector0[i];
            for (i = 0, newVectorIdx = vector0.Length; i < vector0.Length; i++, newVectorIdx++)
                result[newVectorIdx] = vector1[i];

            return result;
            
        }


        //******************** Operator overloads

        ///<summary>
        /// Implicitly cast a <see cref="Vector"/> to a <see cref="Matrix"/>.
        ///</summary>
        ///<param name="arg"><see cref="Vector"/> to cast.</param>
        ///<returns><see cref="Matrix"/> form of the <see cref="Vector"/>.</returns>
        public static implicit operator Matrix(Vector arg)
        {
            return arg._mat;            
        }
        
        ///<summary>
        /// Explicitly cast a <see cref="Matrix"/> to a <see cref="Vector"/>.
        ///</summary>
        ///<param name="arg"><see cref="Matrix"/> to cast.</param>
        ///<returns>A <see cref="Vector"/> representation of the <see cref="Matrix"/>.</returns>
        ///<exception cref="ArgumentException"><see cref="Matrix"/> is not a 
        /// <see cref="Vector"/>.</exception>
        public static explicit operator Vector(Matrix arg)
        {
            if (arg.Rows != 1 && arg.Columns != 1)
                throw new ArgumentException("Matrix is not mat vector.");

            return new Vector(arg);
        }

        ///<summary>
        /// Tests two vectors for equality.
        ///</summary>
        ///<param name="lhs">Left hand side of equality.</param>
        ///<param name="rhs">Right hand side of equality.</param>
        ///<returns><see langword="true"/> if two vectors are equal, <see langword="false"/> 
        /// otherwise.</returns>
        public static bool operator ==(Vector lhs, Vector rhs)
        {
            return lhs.Equals(rhs);
        }

        ///<summary>
        /// Tests two vectors for inequality.
        ///</summary>
        ///<param name="lhs">Left hand side of inequality.</param>
        ///<param name="rhs">Right hand side of inequality.</param>
        ///<returns><see langword="true"/> if two vectors are not equal, <see langword="false"/> 
        /// otherwise.</returns>
        public static bool operator !=(Vector lhs, Vector rhs)
        {
            return !lhs.Equals(rhs);
        }

        ///<summary>
        /// Negates a <see cref="Vector"/>.
        ///</summary>      
        ///<param name="arg"><see cref="Vector"/> to negate</param>
        ///<returns>Result of the negation.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="arg"/> is 
        /// a <see langword="null"/> value.</exception> 
        public static Vector Negate(Vector arg)
        {
            return -arg;
        }

        ///<summary>
        /// Negates a <see cref="Vector"/>.
        ///</summary>      
        ///<param name="arg"><see cref="Vector"/> to negate</param>
        ///<returns>Result of the negation.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="arg"/> is 
        /// a <see langword="null"/> value.</exception> 
        public static Vector operator -(Vector arg)
        {
            if (arg == null)
                throw new ArgumentNullException("arg");

            return new Vector(-(Matrix)(arg));            
        }

        ///<summary>
        /// Subtract one <see cref="Vector"/> from another.
        ///</summary>
        ///<param name="lhs">Left hand side of the subtraction.</param>
        ///<param name="rhs">Right hand side of the subtraction.</param>
        ///<returns>Result of the subtraction.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs"/> and
        ///<paramref name="rhs"/> do not have the same length.</exception>
        public static Vector Subtract(Vector lhs, Vector rhs)
        {
            return lhs - rhs;
        }

        ///<summary>
        /// Subtract one <see cref="Vector"/> from another.
        ///</summary>
        ///<param name="lhs">Left hand side of the subtraction.</param>
        ///<param name="rhs">Right hand side of the subtraction.</param>
        ///<returns>Result of the subtraction.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs"/> and
        ///<paramref name="rhs"/> do not have the same length.</exception>
        public static Vector operator -(Vector lhs, Vector rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if (lhs.Length != rhs.Length)
                throw new SizeMismatchException("Vectors do not have the same size.");
            if (rhs.Orientation != VectorType.RowVector && lhs.Orientation != VectorType.RowVector)
                throw new NotImplementedException("only row vectors supported at the moment");

            Vector result = new Vector(lhs.Length);
            
            for (int i = 0; i < result.Length; i++)
                result._mat._Values[0][i] = lhs._mat._Values[0][i] - rhs._mat._Values[0][i];

            return result;
        }

        ///<summary>
        /// Adds one <see cref="Vector"/> to another.
        ///</summary>
        ///<param name="lhs">Left hand side of the addition.</param>
        ///<param name="rhs">Right hand side of the addition.</param>
        ///<returns>Result of the addition.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs"/> and
        ///<paramref name="rhs"/> do not have the same length.</exception>
        public static Vector Add(Vector lhs, Vector rhs)
        {
            return lhs + rhs;
        }

        ///<summary>
        /// Adds one <see cref="Vector"/> to another.
        ///</summary>
        ///<param name="lhs">Left hand side of the addition.</param>
        ///<param name="rhs">Right hand side of the addition.</param>
        ///<returns>Result of the addition.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs"/> and
        ///<paramref name="rhs"/> do not have the same length.</exception>
        public static Vector operator +(Vector lhs, Vector rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if (lhs.Length != rhs.Length)
                throw new SizeMismatchException("Vectors do not have the same size.");

            Vector result = new Vector(lhs.Length);

            for (int i = 0; i < result.Length; i++)
                result[i] = lhs[i] + rhs[i];

            return result;
        }

        ///<summary>
        /// Multiplies a <see cref="Vector"/> by a scalar value. 
        ///</summary>
        ///<param name="lhs">Scalar value.</param>
        ///<param name="rhs"><see cref="Vector"/> value.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="rhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static Vector Multiply(double lhs, Vector rhs)
        {
            return lhs*rhs;
        }

        ///<summary>
        /// Multiplies a <see cref="Vector"/> by a scalar value. 
        ///</summary>
        ///<param name="lhs">Scalar value.</param>
        ///<param name="rhs"><see cref="Vector"/> value.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="rhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static Vector operator *(double lhs, Vector rhs)
        {
            if (rhs == null)
                throw new ArgumentNullException("rhs");

            Vector result = new Vector(rhs.Length);

            for (int i = 0; i < rhs.Length; i++)
                result[i] = rhs[i] * lhs;

            return result;
        }

        /// <summary>
        /// Convert the current <see cref="Vector"/> to a string representation.
        /// </summary>
        /// <returns>A string that represents the current <see cref="Vector"/>.</returns>
        public override string ToString()
        {            
            return this._mat.ToString();            
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="Vector"/> and is equal
        /// to this <see cref="Vector"/>.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>This method returns <see langword="true"/> if <paramref name="obj"/> is
        /// the specified <see cref="Vector"/> identical to this <see cref="Vector"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (this.GetType() != obj.GetType()) return false;

            Vector rhs = (Vector)obj;

            if (rhs.Orientation != Orientation) return false;
            if (rhs.Length != this.Length) return false;

            for (int i = 0; i < _mat._Rows; i++)
                for (int j = 0; j < _mat._Columns; j++)
                    if (_mat._Values[i][j] != rhs._mat._Values[i][j]) return false;

            return true;
        }

        /// <summary>
        /// Returns a hash code
        /// </summary>
        /// <returns>The hash code for this <see cref="Vector"/>.</returns>
        public override int GetHashCode()
        {
            int hash = 0;

            foreach (double[] da in _mat._Values)
                foreach (double d in da)
                    hash ^= d.GetHashCode();

            return hash;
        }

        // ICloneable Implementation
        /// <summary>
        /// Perform a deep copy of this <see cref="Vector"/>.
        /// </summary>
        /// <returns>The cloned <see cref="Vector"/>.</returns>
        public object Clone()
        {
            return new Vector(this);
        }
    }
}
