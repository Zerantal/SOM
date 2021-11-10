using System;
using System.Collections.Generic;
using System.Text;

namespace MathLib.Matrices
{
    /// <summary>
    /// Matrix initialisation option
    /// </summary>
    public enum MatrixCreationOption 
    { 
        /// <summary>
        /// Initialise all matrix entries to zero.
        /// </summary>
        Zeros, // default behaviour
        /// <summary>
        /// Initialise all matrix entries to one.
        /// </summary>
        Ones,
        /// <summary>
        /// Initialise all matrix entries to random values between zero and one.
        /// </summary>
        Random 
    }; // add more as required

    /// <summary>
    /// Matrix of double precision values
    /// </summary>
    [Serializable]
    public class Matrix : ICloneable
    {        
        internal double[][] _Values;
        internal int _Rows;
        internal int _Columns;

        /// <summary>
        /// Initialise a new instance of the <see cref="Matrix"/> class with a specified
        /// number of rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows in the new matrix.</param>
        /// <param name="cols">The number of columns in the new matrix.</param>
        /// <remarks>All values of the new matrix will be initialised to zero
        /// by default.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">Either the <paramref name="rows"/>
        ///  or <paramref name="cols"/> parameter passed to the constructor is a 
        ///  zero or negative number.</exception>
        public Matrix(int rows, int cols)
        {
            if (rows < 1 || cols < 1)
                throw new ArgumentOutOfRangeException("zero or negative size passed to constructor", 
                    (Exception)null);
            _Rows = rows;
            _Columns = cols;
            _Values = new double[_Rows][];
            for (int r = 0; r < _Rows; r++)
                _Values[r] = new double[_Columns];
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="Matrix"/> class by cloning
        /// an existing Matrix.
        /// </summary>
        /// <param name="matrix">Matrix to clone.</param>
        /// <exception cref="ArgumentNullException"><paramref name="matrix"/> is null.</exception>
        public Matrix(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");
            _Rows = matrix._Rows;
            _Columns = matrix._Columns;
            _Values = new double[_Rows][];

            for (int r = 0; r < _Rows; r++)
            {
                _Values[r] = new double[_Columns];
                for (int c = 0; c < _Columns; c++)
                    _Values[r][c] = matrix._Values[r][c];
            }
        }

        /// <summary>
        /// Initialise a new instance of the  <see cref="Matrix"/> class with a 2-dimensional
        /// array of double precision values.
        /// </summary>
        /// <param name="values">Values to initialise matrix with.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null </exception>
        /// <exception cref="ArgumentException"><paramref name="values"/> is not a rectangular array</exception>
        public Matrix(double[][] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            int cols = values[0].Length;
            int rows = 0;
            // check that array is rectangular
            foreach (double[] row in values)
            {
                rows++;
                if (row.Length != cols)
                    throw new ArgumentException("Array is not rectangular");
            }

            _Rows = rows;
            _Columns = cols;
            _Values = new double[Rows][];
            for (int r = 0; r < rows; r++)
                _Values[r] = (double[])(values[r]).Clone();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class with a specified number
        /// of rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows in the new <see cref="Matrix"/>.</param>
        /// <param name="columns">The number of columns in the new <see cref="Matrix"/>.</param>
        /// <param name="opt">The initialisation option for the new <see cref="Matrix"/>.</param>
        public Matrix(int rows, int columns, MatrixCreationOption opt)
            : this(rows, columns)
        {
            int r,c;
            double val = 0;

            if (opt == MatrixCreationOption.Random)
            {
                Random rand = new Random();
                for (r = 0; r < Rows; r++)
                    for (c = 0; c < Columns; c++)
                        _Values[r][c] = rand.NextDouble();  
            }
            else if (opt == MatrixCreationOption.Ones)
            {
                val = 1;

                for (r = 0; r < Rows; r++)
                    for (c = 0; c < Columns; c++)
                        _Values[r][c] = val;
            }
        }

        /// <summary>
        /// Gets the row count of the <see cref="Matrix"/>.
        /// </summary>
        /// <value>The rows.</value>
        public int Rows { get { return _Rows; } }
        /// <summary>
        /// Gets the column count of the <see cref="Matrix"/>.
        /// </summary>
        /// <value>The columns.</value>
        public int Columns { get { return _Columns; } }
        /// <summary>
        /// Gets a value indicating whether this <see cref="Matrix"/> is square.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Matrix"/> is square; otherwise, <c>false</c>.</value>
        public bool IsSquare { get { return (_Rows == _Columns); } }


        /// <summary>
        /// Gets or sets the value of the specified element of the <see cref="Matrix"/>.
        /// </summary>
        /// <param name="row">The row number of the element.</param>
        /// <param name="column">The column number of the element.</param>
        /// <value>The value at the specified row and column.</value>
        /// <remarks>The <paramref name="row"/> and <paramref name="column"/> parameters use zero 
        /// based indexing.</remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="row"/> or <paramref name="column"/>
        /// are either less than zero or greater than or equal to the total number 
        /// of rows and columns respectively.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1023:IndexersShouldNotBeMultidimensional")]
        public double this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= _Rows)
                    throw new ArgumentOutOfRangeException("row");
                if (column < 0 || column >= _Columns)
                    throw new ArgumentOutOfRangeException("column");
                return _Values[row][column];
            }
            set
            {
                if (row < 0 || row >= _Rows)
                    throw new ArgumentOutOfRangeException("row");
                if (column < 0 || column >= _Columns)
                    throw new ArgumentOutOfRangeException("column");
                _Values[row][column] = value;
            }
        }

        /// <summary>
        /// Gets a specified row of this <see cref="Matrix"/>.
        /// </summary>
        /// <param name="row">The row number.</param>
        /// <returns>A row <see cref="Vector"/>.</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="row"/> is either less than
        /// zero or greater than or equal to the total number of rows.</exception>
        public Vector GetRow(int row)
        {
            if (row < 0 || row >= _Rows)
                throw new ArgumentOutOfRangeException("row");
            Matrix result = new Matrix(1, _Columns);
            result._Values[0] = (double[])_Values[row].Clone();
            return (Vector)result;
        }


        /// <summary>
        /// Gets a specified column of this <see cref="Matrix"/>.
        /// </summary>
        /// <param name="column">The column number.</param>
        /// <returns>A column <see cref="Vector"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="column"/> is either less than
        /// zero or greater than or equal to the total number of columns.</exception>        
        public Vector GetColumn(int column)
        {
            if (column < 0 || column >= _Columns)
                throw new ArgumentOutOfRangeException("column");

            Matrix result = new Matrix(_Rows, 1);

            for (int i = 0; i < _Columns; i++)
                result._Values[i][0] = _Values[i][column];

            return (Vector)result;
        }

        /// <summary>
        /// Sets specified row to the given row <see cref="Vector"/>.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="rowVector">The row vector.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="row"/> is either less than
        /// zero or greater than or equal to the total number of 
        /// rows in this <see cref="Matrix"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="rowVector"/> must be a row vector and 
        /// have a length equal to the number of columns in this <see cref="Matrix"/></exception>
        public void SetRow(int row, Vector rowVector)
        {
            if (row < 0 || row >= _Rows)
                throw new ArgumentOutOfRangeException("row");
            if (rowVector.Orientation != VectorType.RowVector)
                throw new ArgumentException("Vector must be mat row vector.");
            if (rowVector.Length != _Columns)
                throw new ArgumentException("vector must have the following length: " + _Columns);

            _Values[row] = (double[])rowVector._mat._Values[0].Clone();           
        }

        /// <summary>
        /// Sets the column.
        /// </summary>
        /// <param name="column">The col.</param>
        /// <param name="colVector">The col vector.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="column"/> is either less than
        /// zero or greater than or equal to the total number of columns
        /// in this <see cref="Matrix"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="colVector"/> must be a column vector and 
        /// have a length equal to the number of rows in this <see cref="Matrix"/></exception>        
       public void SetColumn(int column, Vector colVector)
        {
            if (column < 0 || column >= _Columns)
                throw new ArgumentOutOfRangeException("column");
            if (colVector.Orientation != VectorType.ColumnVector)
                throw new ArgumentException("Vector must be mat column vector");
            if (colVector.Length != _Rows)
                throw new ArgumentException("Vector must have the following length: " + _Rows);

            for (int i = 0; i < _Rows; i++)
                _Values[i][column] = colVector._mat._Values[i][0];            
        }
        
        /// <summary>
        /// An enumerator for iterating over all of the rows of this <see cref="Matrix"/>.
        /// </summary>        
        public IEnumerable<Vector> RowEnumerator
        {
            get
            {
                for (int i = 0; i < _Rows; i++)
                {
                    yield return new Vector(_Values[i]);
                }
            }
        }


        ///<summary>
        /// An enumerator for iterating over all of the columns of this <see cref="Matrix"/>.
        ///</summary>
        public IEnumerable<Vector> ColumnEnumerator
        {
            get
            {
                Vector col = new Vector(this._Rows, VectorType.ColumnVector);
                for (int i = 0; i < _Columns; i++)
                {
                    for (int j = 0; j < _Rows; j++)
                        col[j] = _Values[j][i];
                    yield return new Vector(col);
                }
            }
        }

        /// <summary>
        /// Transposes this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>The transposed <see cref="Matrix"/>.</returns>
        public Matrix Transpose()
        {
            Matrix result = new Matrix(_Columns, _Rows);

            for (int r = 0; r < _Rows; r++)
                for (int c = 0; c < _Columns; c++)
                    result._Values[c][r] = this._Values[r][c];

            return result;
        }

        /// <summary>
        /// Convert the current <see cref="Matrix"/> to a string representation.
        /// </summary>
        /// <returns>A string that represents the current <see cref="Matrix"/>.</returns>
        public override string ToString()
        {
            StringBuilder retString = new StringBuilder();
            foreach (double[] row in _Values)
            {
                retString.Append("|");
                foreach (double d in row)
                {
                    retString.AppendFormat("{0,-10:0.0000}", d);
                    retString.Append("|");
                }
                //retString.Append("\n");
            }            

            return retString.ToString();
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="Matrix"/> and is equal
        /// to this <see cref="Matrix"/>.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>This method returns <see langword="true"/> if <paramref name="obj"/> is
        /// the specified <see cref="Matrix"/> identical to this <see cref="Matrix"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (GetType() != obj.GetType()) return false;

            Matrix rhs = (Matrix) obj;

            if (rhs.Rows != Rows) return false;
            if (rhs.Columns != Columns) return false;

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    if (_Values[i][j] != rhs._Values[i][j]) return false;
                        
            return true;
        }
              
        /// <summary>
        /// Returns a hash code
        /// </summary>
        /// <returns>The hash code for this <see cref="Matrix"/>.</returns>
        public override int GetHashCode()
        {
            int hash = 0;

            foreach (double[] da in _Values)
                foreach (double d in da)
                    hash ^= d.GetHashCode();
            
            return hash;            
        }

        //////////////////////////////// operator overloads //////////////////////////////////

        ///<summary>
        /// Subtract one <see cref="Matrix"/> from another.
        ///</summary>
        ///<param name="lhs">Left hand side of the subtraction.</param>
        ///<param name="rhs">Right hand side of the subtraction.</param>
        ///<returns>Result of the subtraction.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs"/> and
        ///<paramref name="rhs"/> do not have the same dimensions.</exception>
        public static Matrix Subtract(Matrix lhs, Matrix rhs)
        {
            return lhs - rhs;
        }

        ///<summary>
        /// Subtract one <see cref="Matrix"/> from another.
        ///</summary>
        ///<param name="lhs">Left hand side of the subtraction.</param>
        ///<param name="rhs">Right hand side of the subtraction.</param>
        ///<returns>Result of the subtraction.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs"/> and
        ///<paramref name="rhs"/> do not have the same dimensions.</exception>
        public static Matrix operator -(Matrix lhs, Matrix rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if ((lhs.Rows != rhs.Rows) || (lhs.Columns != rhs.Columns))
                throw new SizeMismatchException();

            Matrix result = new Matrix(lhs.Rows, lhs.Columns);

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    result._Values[r][c] = lhs._Values[r][c] - rhs._Values[r][c];

            return result;
        }

        ///<summary>
        /// Adds one <see cref="Matrix"/> to another.
        ///</summary>
        ///<param name="lhs">Left hand side of the addition.</param>
        ///<param name="rhs">Right hand side of the addition.</param>
        ///<returns>Result of the addition.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs "/> and
        ///<paramref name="rhs"/> do not have the same dimensions.</exception>
        public static Matrix Add(Matrix lhs, Matrix rhs)
        {
            return lhs + rhs;
        }

        ///<summary>
        /// Adds one <see cref="Matrix"/> to another.
        ///</summary>
        ///<param name="lhs">Left hand side of the addition.</param>
        ///<param name="rhs">Right hand side of the addition.</param>
        ///<returns>Result of the addition.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs "/> and
        ///<paramref name="rhs"/> do not have the same dimensions.</exception>
        public static Matrix operator +(Matrix lhs, Matrix rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (lhs == null)
                throw new ArgumentNullException("rhs");
            if ((lhs.Rows != rhs.Rows) || (lhs.Columns != rhs.Columns))
                throw new SizeMismatchException();

            Matrix result = new Matrix(lhs.Rows, lhs.Columns);

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    result._Values[r][c] = lhs._Values[r][c] + rhs._Values[r][c];

            return result;
        }

        ///<summary>
        /// Negates a <see cref="Matrix"/>.
        ///</summary>      
        ///<param name="arg"><see cref="Matrix"/> to negate</param>
        ///<returns>Result of the negation.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="arg"/> is 
        /// a <see langword="null"/> value.</exception>                
        public static Matrix Negate(Matrix arg)
        {
            return -arg;
        }

        ///<summary>
        /// Negates a <see cref="Matrix"/>.
        ///</summary>      
        ///<param name="arg"><see cref="Matrix"/> to negate</param>
        ///<returns>Result of the negation.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="arg"/> is 
        /// a <see langword="null"/> value.</exception>                
        public static Matrix operator -(Matrix arg)
        {
            if (arg == null)
                throw new ArgumentNullException("arg");

            Matrix result = new Matrix(arg._Rows, arg._Columns);

            for (int r = 0; r < arg._Rows; r++)
                for (int c = 0; c < arg._Columns; c++)
                    result._Values[r][c] = -arg._Values[r][c];

            return result;          
        }

        ///<summary>
        /// Multiplies a <see cref="Matrix"/> by a scalar value. 
        ///</summary>
        ///<param name="lhs">Scalar value.</param>
        ///<param name="rhs"><see cref="Matrix"/> value.</param>
        ///<returns>result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="rhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static Matrix Multiply(double lhs, Matrix rhs)
        {
            return lhs*rhs;
        }

        ///<summary>
        /// Multiplies a <see cref="Matrix"/> by a scalar value. 
        ///</summary>
        ///<param name="lhs">Scalar value.</param>
        ///<param name="rhs"><see cref="Matrix"/> value.</param>
        ///<returns>result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="rhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static Matrix operator *(double lhs, Matrix rhs)
        {
            if (rhs == null)
                throw new ArgumentNullException("rhs", "Matrix is null");

            Matrix result = new Matrix(rhs._Rows, rhs._Columns);

            for (int r = 0; r < rhs._Rows; r++)
                for (int c = 0; c < rhs._Columns; c++)
                    result[r, c] = rhs._Values[r][c] * lhs;

            return result;            
        }

        ///<summary>
        /// Multiplies a <see cref="Matrix"/> by a scalar value. 
        ///</summary>
        ///<param name="lhs"><see cref="Matrix"/> value.</param>
        ///<param name="rhs">Scalar value.</param>
        ///<returns>result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static Matrix Multiply(Matrix lhs, double rhs)
        {
            return lhs*rhs;
        }

        ///<summary>
        /// Multiplies a <see cref="Matrix"/> by a scalar value. 
        ///</summary>
        ///<param name="lhs"><see cref="Matrix"/> value.</param>
        ///<param name="rhs">Scalar value.</param>
        ///<returns>result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static Matrix operator *(Matrix lhs, double rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs", "Matrix is null");

            Matrix result = new Matrix(lhs._Rows, lhs._Columns);

            for (int r = 0; r < lhs._Rows; r++)
                for (int c = 0; c < lhs._Columns; c++)
                    result[r, c] = lhs._Values[r][c] * rhs;

            return result;
        }

        ///<summary>
        /// Multiplies two matrices together.
        ///</summary>
        ///<param name="lhs">Left hand side of the multiplication.</param>
        ///<param name="rhs">Right hand side of the multiplication.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException">The number of columns in <paramref name="lhs"/>
        /// does not equal the number of rows in <paramref name="rhs"/>.</exception>
        public static Matrix Multiply(Matrix lhs, Matrix rhs)
        {
            return lhs*rhs;
        }

        ///<summary>
        /// Multiplies two matrices together.
        ///</summary>
        ///<param name="lhs">Left hand side of the multiplication.</param>
        ///<param name="rhs">Right hand side of the multiplication.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException">The number of columns in <paramref name="lhs"/>
        /// does not equal the number of rows in <paramref name="rhs"/>.</exception>
        public static Matrix operator *(Matrix lhs, Matrix rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if (lhs.Columns != rhs.Rows)
                throw new SizeMismatchException();

            Matrix result = new Matrix(rhs._Columns, lhs._Rows);
            int i;
            double tmp;
            for (int r = 0; r < rhs._Columns; r++)
                for (int c = 0; c < lhs._Rows; c++)
                {
                    tmp = 0;
                    for (i = 0; i < lhs._Columns; i++)
                        tmp += (lhs._Values[r][i] * rhs._Values[i][c]);
                    result._Values[r][c] = tmp;
                }

            return result;
        }

        ///<summary>
        /// Multiplies a <see cref="Matrix"/> by a <see cref="Vector"/>.
        ///</summary>
        ///<param name="lhs">Left hand side of the multiplication.</param>
        ///<param name="rhs">Right hand side of the multiplication.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException">The number of columns in <paramref name="lhs"/>
        /// does not equal the number of rows in <paramref name="rhs"/>.</exception>
        public static Vector Multiply(Matrix lhs, Vector rhs)
        {
            return lhs*rhs;
        }

        ///<summary>
        /// Multiplies a <see cref="Matrix"/> by a <see cref="Vector"/>.
        ///</summary>
        ///<param name="lhs">Left hand side of the multiplication.</param>
        ///<param name="rhs">Right hand side of the multiplication.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException">The number of columns in <paramref name="lhs"/>
        /// does not equal the number of rows in <paramref name="rhs"/>.</exception>
        public static Vector operator *(Matrix lhs, Vector rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if (lhs.Columns != rhs._mat._Rows)
                throw new SizeMismatchException();

            Vector result = new Vector(lhs._Columns);
            for (int r = 0; r < lhs._Rows; r++)
                for (int c = 0; c < lhs._Columns; c++)
                    result._mat._Values[0][r] += (rhs._mat._Values[0][c] * lhs._Values[r][c]);

            return result;
        }
        
        ///<summary>
        /// Multiplies a <see cref="Vector"/> by a <see cref="Matrix"/>.
        ///</summary>
        ///<param name="lhs">Left hand side of the multiplication.</param>
        ///<param name="rhs">Right hand side of the multiplication.</param>
        ///<returns>Result of multiplication</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        /// <paramref name="rhs"/> is null.</exception>
        ///<exception cref="ArgumentException"><paramref name="lhs"/> is not a 
        /// row <see cref="Vector"/>.</exception>
        ///<exception cref="SizeMismatchException">The length of <paramref name="lhs"/>
        /// is not equal to the number of rows in <paramref name="rhs"/></exception>
        public static Vector Multiply(Vector lhs, Matrix rhs)
        {
            return lhs*rhs;
        }

        ///<summary>
        /// Multiplies a <see cref="Vector"/> by a <see cref="Matrix"/>.
        ///</summary>
        ///<param name="lhs">Left hand side of the multiplication.</param>
        ///<param name="rhs">Right hand side of the multiplication.</param>
        ///<returns>Result of multiplication</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        /// <paramref name="rhs"/> is null.</exception>
        ///<exception cref="ArgumentException"><paramref name="lhs"/> is not a 
        /// row <see cref="Vector"/>.</exception>
        ///<exception cref="SizeMismatchException">The length of <paramref name="lhs"/>
        /// is not equal to the number of rows in <paramref name="rhs"/></exception>
        public static Vector operator *(Vector lhs, Matrix rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if (lhs.Orientation != VectorType.RowVector)
                throw new ArgumentException("lhs is not mat row vector.");
            if (lhs.Length != rhs._Rows)
                throw new SizeMismatchException();

            return (new Vector(lhs._mat * rhs));            
        }

        ///<summary>
        /// Tests two matrices for equality.
        ///</summary>
        ///<param name="lhs">Left hand side of equality.</param>
        ///<param name="rhs">Right hand side of equality.</param>
        ///<returns><see langword="true"/> if two matrices are equal, <see langword="false"/> 
        /// otherwise.</returns>
        public static bool operator ==(Matrix lhs, Matrix rhs)
        {
            return lhs.Equals(rhs);
        }

        ///<summary>
        /// Tests two matrices for inequality.
        ///</summary>
        ///<param name="lhs">Left hand side of inequality.</param>
        ///<param name="rhs">Right hand side of inequality.</param>
        ///<returns><see langword="true"/> if two matrices are not equal, <see langword="false"/> 
        /// otherwise.</returns>
        public static bool operator !=(Matrix lhs, Matrix rhs)
        {
            return !lhs.Equals(rhs);
        }


        // ICloneable Implementation
        /// <summary>
        /// Perform a deep copy of this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>The cloned <see cref="Matrix"/>.</returns>
        public object Clone()
        {
            return new Matrix(this);
        }
    }
}
