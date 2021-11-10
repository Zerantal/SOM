using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MathLib.Matrices
{
    /// <summary>
    /// A vector of complex numbers
    /// </summary>
    [Serializable]
    public class ComplexVector : ICloneable
    {
        internal ComplexMatrix _mat;
        private int _Dimension;

        ///<summary>
        /// Initialise a new instance of the <see cref="ComplexVector"/> class
        /// with a specified dimension.
        ///</summary>
        ///<param name="dim">Dimension of new <see cref="ComplexVector"/>.</param>
        /// <remarks>New vector is a row vector by default.</remarks>
        ///<exception cref="ArgumentOutOfRangeException">Dimension is less
        /// than or equal to zero.</exception>
        public ComplexVector(int dim) : this(dim, VectorType.RowVector) { }

        ///<summary>
        /// Initialise a new instance of the <see cref="ComplexVector"/> class
        /// with a specified dimension and orientation.
        ///</summary>
        ///<param name="dim">Dimension of new <see cref="ComplexVector"/>.</param>
        ///<param name="type">Orientation of new <see cref="ComplexVector"/>.</param>
        ///<exception cref="ArgumentOutOfRangeException">Dimension is less
        /// than or equal to zero.</exception>
        public ComplexVector(int dim, VectorType type)
        {
            if (dim < 1)
                throw new ArgumentOutOfRangeException("zero or negative size passed to constructor",
                        (Exception)null);

            _Dimension = dim;
            if (type == VectorType.ColumnVector)
                _mat = new ComplexMatrix(dim, 1);
            else
                _mat = new ComplexMatrix(1, dim);
        }

        ///<summary>
        /// Initialise a new instance of the <see cref="ComplexVector"/> class
        /// with a specific content.
        ///</summary>
        ///<param name="values">The values to initialise the 
        /// <see cref="ComplexVector"/> with.</param>
        ///<exception cref="ArgumentNullException">values is null.</exception>
        public ComplexVector(Complex[] values) : this(values, VectorType.RowVector) { }

        ///<summary>
        /// Initialise a new instance of the <see cref="ComplexVector"/> class
        /// with a specific content and orientation.
        ///</summary>
        ///<param name="values">The values to initialise the 
        /// <see cref="ComplexVector"/> with.</param>
        ///<param name="type">Orientation of new <see cref="ComplexVector"/>.</param>
        ///<exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public ComplexVector(Complex[] values, VectorType type)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            _Dimension = values.Length;
            if (type == VectorType.RowVector)
            {
                _mat = new ComplexMatrix(1, values.Length);
                _mat._Values[0] = (Complex[])values.Clone();
            }
            else
            {
                _mat = new ComplexMatrix(values.Length, 1);
                for (int r = 0; r < values.Length; r++)
                    _mat._Values[r][0] = values[r];
            }
        }

        ///<summary>
        /// Initialise a new instance of the <see cref="ComplexVector"/> class 
        /// with the content of another <see cref="ComplexVector"/>.
        ///</summary>
        ///<param name="v">Vector whos values to copy.</param>
        ///<exception cref="ArgumentNullException"><paramref name="v"/> is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "v")]
        public ComplexVector(ComplexVector v)
        {
            if (v == null)
                throw new ArgumentNullException("v");

            _Dimension = v._Dimension;
            _mat = (ComplexMatrix)v._mat.Clone();
        }

        ///<summary>
        /// Initialise a new instance of the <see cref="ComplexVector"/> class with
        /// the content of a <see cref="ComplexMatrix"/>.
        ///</summary>
        ///<param name="arg"><see cref="ComplexMatrix"/> whos values to copy.</param>
        ///<exception cref="ArgumentNullException"><paramref name="arg"/> is null.</exception>
        ///<exception cref="ArgumentException"><paramref name="arg"/> is not a vector.</exception>
        /// <remarks>Parameter <paramref name="arg"/> must be a 
        /// <see cref="ComplexMatrix"/> with either one row or one column.</remarks>
        public ComplexVector(ComplexMatrix arg)
        {
            if (arg == null)
                throw new ArgumentNullException("arg");
            if (arg._Rows != 1 && arg._Columns != 1)
                throw new ArgumentException("Matrix is not mat vector.");

            _mat = (ComplexMatrix)arg.Clone();
            if (_mat._Rows == 1)
                _Dimension = _mat._Columns;
            else
                _Dimension = _mat._Rows;
        }

        ///<summary>
        /// Return the dimensionality of <see cref="ComplexVector"/>.
        ///</summary>
        public int Length { get { return _Dimension; } }

        /// <summary>
        /// Gets or sets the value of the specified element of the 
        /// <see cref="ComplexVector"/>.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <value>The value at the specified index.</value>
        /// <remarks>The <paramref name="index"/> parameters uses zero 
        /// based indexing.</remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is 
        /// either less than zero or greater than or equal to the total length of
        /// this <see cref="ComplexVector"/>
        /// </exception>
        public Complex this[int index]
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
        /// Returns the orientation of the <see cref="ComplexVector"/>.
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
        /// Returns the euclidean norm of the <see cref="ComplexVector"/>
        ///</summary>
        public Double Norm
        {
            get
            {
                double result = 0;
                if (this.Orientation == VectorType.RowVector)
                    for (int i = 0; i < _Dimension; i++)
                    {
                        result += _mat._Values[0][i].Real*_mat._Values[0][i].Real;
                        result += _mat._Values[0][i].Imaginary*_mat._Values[0][i].Imaginary;
                    }                                  
                else
                    for (int i = 0; i < _Dimension; i++)
                    {
                        result += _mat._Values[i][0].Real * _mat._Values[i][0].Real;
                        result += _mat._Values[i][0].Imaginary * _mat._Values[i][0].Imaginary;                       
                    }

                return Math.Sqrt(result);
            }
        }

        ///<summary>
        /// Find the minimum value in the <see cref="ComplexVector"/>.
        ///</summary>
        ///<returns>The minimum value.</returns>
        public Complex Min()
        {
            Complex min = new Complex(double.MaxValue, double.MaxValue);
            Complex tmp;
            for (int i = 0; i < Length; i++)
            {
                tmp = this[i];
                if (Complex.Abs(tmp) < Complex.Abs(min))
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
            Complex min = new Complex(double.MaxValue, double.MaxValue);
            Complex tmp;
            int minIdx = 0;
            for (int i = 0; i < Length; i++)
            {
                tmp = this[i];
                if (Complex.Abs(tmp) < Complex.Abs(min))
                {
                    min = tmp;
                    minIdx = i;
                }
            }

            return minIdx;
        }




        //******************** Operator overloads

        ///<summary>
        /// Implicitly cast a <see cref="ComplexVector"/> to a 
        /// <see cref="ComplexMatrix"/>.
        ///</summary>
        ///<param name="arg"><see cref="ComplexVector"/> to cast.</param>
        ///<returns><see cref="ComplexMatrix"/> form of the 
        /// <see cref="ComplexVector"/>.</returns>
        public static implicit operator ComplexMatrix(ComplexVector arg)
        {
            return arg._mat;
        }

        ///<summary>
        /// Explicitly cast a <see cref="ComplexMatrix"/> to a 
        /// <see cref="ComplexVector"/>.
        ///</summary>
        ///<param name="arg"><see cref="ComplexMatrix"/> to cast.</param>
        ///<returns>A <see cref="ComplexVector"/> representation of the 
        /// <see cref="ComplexMatrix"/>.</returns>
        ///<exception cref="ArgumentException"><see cref="ComplexMatrix"/> is 
        /// not a <see cref="ComplexVector"/>.</exception>
        public static explicit operator ComplexVector(ComplexMatrix arg)
        {
            if (arg.Rows != 1 && arg.Columns != 1)
                throw new ArgumentException("Matrix is not mat vector.");

            return new ComplexVector(arg);
        }

        ///<summary>
        /// Tests two complex vectors for equality.
        ///</summary>
        ///<param name="lhs">Left hand side of equality.</param>
        ///<param name="rhs">Right hand side of equality.</param>
        ///<returns><see langword="true"/> if two complex vectors are 
        /// equal, <see langword="false"/> otherwise.</returns>
        public static bool operator ==(ComplexVector lhs, ComplexVector rhs)
        {
            return lhs.Equals(rhs);
        }

        ///<summary>
        /// Tests two complex vectors for inequality.
        ///</summary>
        ///<param name="lhs">Left hand side of inequality.</param>
        ///<param name="rhs">Right hand side of inequality.</param>
        ///<returns><see langword="true"/> if two complex vectors are 
        /// not equal, <see langword="false"/> otherwise.</returns>
        public static bool operator !=(ComplexVector lhs, ComplexVector rhs)
        {
            return !lhs.Equals(rhs);
        }

        ///<summary>
        /// Negates a <see cref="ComplexVector"/>.
        ///</summary>      
        ///<param name="arg"><see cref="ComplexVector"/> to negate</param>
        ///<returns>Result of the negation.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="arg"/> is 
        /// a <see langword="null"/> value.</exception> 
        public static ComplexVector Negate(ComplexVector arg)
        {
            return -arg;
        }

        ///<summary>
        /// Negates a <see cref="ComplexVector"/>.
        ///</summary>      
        ///<param name="arg"><see cref="ComplexVector"/> to negate</param>
        ///<returns>Result of the negation.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="arg"/> is 
        /// a <see langword="null"/> value.</exception> 
        public static ComplexVector operator -(ComplexVector arg)
        {
            if (arg == null)
                throw new ArgumentNullException("arg");

            return new ComplexVector(-(ComplexMatrix)(arg));
        }

        ///<summary>
        /// Subtract one <see cref="ComplexVector"/> from another.
        ///</summary>
        ///<param name="lhs">Left hand side of the subtraction.</param>
        ///<param name="rhs">Right hand side of the subtraction.</param>
        ///<returns>Result of the subtraction.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs"/> and
        ///<paramref name="rhs"/> do not have the same length.</exception>
        public static ComplexVector Subtract(ComplexVector lhs, ComplexVector rhs)
        {
            return lhs - rhs;
        }

        ///<summary>
        /// Subtract one <see cref="ComplexVector"/> from another.
        ///</summary>
        ///<param name="lhs">Left hand side of the subtraction.</param>
        ///<param name="rhs">Right hand side of the subtraction.</param>
        ///<returns>Result of the subtraction.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs"/> and
        ///<paramref name="rhs"/> do not have the same length.</exception>
        public static ComplexVector operator -(ComplexVector lhs, ComplexVector rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if (lhs.Length != rhs.Length)
                throw new SizeMismatchException("Vectors do not have the same size.");

            ComplexVector result = new ComplexVector(lhs.Length);

            for (int i = 0; i < result.Length; i++)
                result[i] = lhs[i] - rhs[i];

            return result;
        }

        ///<summary>
        /// Adds one <see cref="ComplexVector"/> to another.
        ///</summary>
        ///<param name="lhs">Left hand side of the addition.</param>
        ///<param name="rhs">Right hand side of the addition.</param>
        ///<returns>Result of the addition.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs"/> and
        ///<paramref name="rhs"/> do not have the same length.</exception>
        public static ComplexVector Add(ComplexVector lhs, ComplexVector rhs)
        {
            return lhs + rhs;
        }

        ///<summary>
        /// Adds one <see cref="ComplexVector"/> to another.
        ///</summary>
        ///<param name="lhs">Left hand side of the addition.</param>
        ///<param name="rhs">Right hand side of the addition.</param>
        ///<returns>Result of the addition.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs"/> and
        ///<paramref name="rhs"/> do not have the same length.</exception>
        public static ComplexVector operator +(ComplexVector lhs, ComplexVector rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if (lhs.Length != rhs.Length)
                throw new SizeMismatchException("Vectors do not have the same size.");

            ComplexVector result = new ComplexVector(lhs.Length);

            for (int i = 0; i < result.Length; i++)
                result[i] = lhs[i] + rhs[i];

            return result;
        }

        ///<summary>
        /// Multiplies a <see cref="ComplexVector"/> by a scalar value. 
        ///</summary>
        ///<param name="lhs">Scalar value.</param>
        ///<param name="rhs"><see cref="ComplexVector"/> value.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="rhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static ComplexVector Multiply(double lhs, ComplexVector rhs)
        {
            return lhs * rhs;
        }

        ///<summary>
        /// Multiplies a <see cref="ComplexVector"/> by a scalar value. 
        ///</summary>
        ///<param name="lhs">Scalar value.</param>
        ///<param name="rhs"><see cref="ComplexVector"/> value.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="rhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static ComplexVector operator *(double lhs, ComplexVector rhs)
        {
            if (rhs == null)
                throw new ArgumentNullException("rhs");

            ComplexVector result = new ComplexVector(rhs.Length);

            for (int i = 0; i < rhs.Length; i++)
                result[i] = rhs[i] * lhs;

            return result;
        }

        ///<summary>
        /// Multiplies a <see cref="ComplexVector"/> by a scalar value. 
        ///</summary>
        ///<param name="lhs"><see cref="ComplexVector"/> value.</param>
        ///<param name="rhs">Scalar value.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static ComplexVector operator *(ComplexVector lhs, double rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("rhs");

            ComplexVector result = new ComplexVector(lhs.Length);

            for (int i = 0; i < lhs.Length; i++)
                result[i] = lhs[i] * rhs;

            return result;
        }

        /// <summary>
        /// Convert the current <see cref="ComplexVector"/> to a string 
        /// representation.
        /// </summary>
        /// <returns>A string that represents the current <see cref="ComplexVector"/>.</returns>
        public override string ToString()
        {
            return this._mat.ToString();
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="ComplexVector"/>
        /// and is equal to this <see cref="ComplexVector"/>.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>This method returns <see langword="true"/> if <paramref name="obj"/> is
        /// the specified <see cref="ComplexVector"/> identical to this <see cref="ComplexVector"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (this.GetType() != obj.GetType()) return false;

            ComplexVector rhs = (ComplexVector)obj;

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
        /// <returns>The hash code for this <see cref="ComplexVector"/>.</returns>
        public override int GetHashCode()
        {
            int hash = 0;

            foreach (Complex[] ca in _mat._Values)
                foreach (Complex c in ca)
                    hash ^= c.GetHashCode();

            return hash;
        }

        // ICloneable Implementation
        /// <summary>
        /// Perform a deep copy of this <see cref="ComplexVector"/>.
        /// </summary>
        /// <returns>The cloned <see cref="ComplexVector"/>.</returns>
        public object Clone()
        {
            return new ComplexVector(this);
        }
    }
    
}
