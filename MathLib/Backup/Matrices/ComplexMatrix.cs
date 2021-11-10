using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;
using System.Numerics;

namespace MathLib.Matrices
{
    /// <summary>
    /// Matrix consisting of complex values
    /// </summary>
    [Serializable]
    public class ComplexMatrix : ICloneable
    {        
        internal Complex[][] _Values;
        internal int _Rows;
        internal int _Columns;

        /// <summary>
        /// Initialise a new instance of the <see cref="ComplexMatrix"/> class
        /// with a specified number of rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows in the new <see cref="ComplexMatrix"/>.</param>
        /// <param name="cols">The number of columns in the new <see cref="ComplexMatrix"/>.</param>
        /// <remarks>All values of the new <see cref="ComplexMatrix"/> will be initialised to zero
        /// by default.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">Either the <paramref name="rows"/>
        ///  or <paramref name="cols"/> parameter passed to the constructor is a 
        ///  zero or negative number.</exception>
        public ComplexMatrix(int rows, int cols)
        {
            Contract.Ensures(rows >= 1, "Matrix must have at least 1 row");
            Contract.Ensures(cols >= 1, "Matrix must have at least 1 column");
           
            _Rows = rows;
            _Columns = cols;
            _Values = new Complex[_Rows][];
            for (int r = 0; r < _Rows; r++)
                _Values[r] = new Complex[_Columns];
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="ComplexMatrix"/> class
        /// by cloning an existing <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <param name="matrix"><see cref="ComplexMatrix"/> to clone.</param>
        /// <exception cref="ArgumentNullException"><paramref name="matrix"/> is null.</exception>
        public ComplexMatrix(ComplexMatrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");
            _Rows = matrix._Rows;
            _Columns = matrix._Columns;
            _Values = new Complex[_Rows][];

            for (int r = 0; r < _Rows; r++)
            {
                _Values[r] = new Complex[_Columns];
                for (int c = 0; c < _Columns; c++)
                    _Values[r][c] = matrix._Values[r][c];
            }
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="ComplexMatrix"/> class
        /// with a 2-dimensional array of <see cref="Complex"/> numbers.
        /// </summary>
        /// <param name="values">Values to initialise <see cref="ComplexMatrix"/> with.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null </exception>
        /// <exception cref="ArgumentException"><paramref name="values"/> is not a rectangular array</exception>
        public ComplexMatrix(Complex[][] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            int cols = values[0].Length;
            int rows = 0;
            // check that array is rectangular
            foreach (Complex[] row in values)
            {
                rows++;
                if (row.Length != cols)
                    throw new ArgumentException("Array is not rectangular");
            }

            _Rows = rows;
            _Columns = cols;
            _Values = new Complex[Rows][];
            for (int r = 0; r < rows; r++)
                _Values[r] = (Complex[])(values[r]).Clone();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexMatrix"/> class 
        /// with a specified number of rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows in the new <see cref="ComplexMatrix"/>.</param>
        /// <param name="columns">The number of columns in the new <see cref="ComplexMatrix"/>.</param>
        /// <param name="opt">The initialisation option for the new <see cref="ComplexMatrix"/>.</param>
        public ComplexMatrix(int rows, int columns, MatrixCreationOption opt)
            : this(rows, columns)
        {
            int r,c;
            double val = 0;

            switch (opt)
            {
                case MatrixCreationOption.Random:
                    {
                        Random rand = new Random();
                        for (r = 0; r < Rows; r++)
                            for (c = 0; c < Columns; c++)
                            {
                                _Values[r][c] = new Complex(rand.NextDouble(), rand.NextDouble());                      
                            }
                    }
                    break;
                case MatrixCreationOption.Ones:
                    val = 1;
                    for (r = 0; r < Rows; r++)
                        for (c = 0; c < Columns; c++)
                            _Values[r][c] = new Complex(val, val);
                    break;
            }
        }

        /// <summary>
        /// Gets the row count of the <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <value>The rows.</value>
        public int Rows { get { return _Rows; } }
        /// <summary>
        /// Gets the column count of the <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <value>The columns.</value>
        public int Columns { get { return _Columns; } }
        /// <summary>
        /// Gets a value indicating whether this <see cref="ComplexMatrix"/> is square.
        /// </summary>
        /// <value><c>true</c> if this <see cref="ComplexMatrix"/> is square; otherwise, <c>false</c>.</value>
        public bool IsSquare { get { return (_Rows == _Columns); } }


        /// <summary>
        /// Gets or sets the value of the specified element of the 
        /// <see cref="ComplexMatrix"/>.
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
        public Complex this[int row, int column]
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
        /// Gets a specified row of this <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <param name="row">The row number.</param>
        /// <returns>A row vector.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="row"/> is either less than
        /// zero or greater than or equal to the total number of rows.</exception>
        public ComplexVector GetRow(int row)
        {
            if (row < 0 || row >= _Rows)
                throw new ArgumentOutOfRangeException("row");
            ComplexMatrix result = new ComplexMatrix(1, _Columns);
            result._Values[0] = (Complex[])_Values[row].Clone();
            return (ComplexVector)result;
        }


        /// <summary>
        /// Gets a specified column of this <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <param name="column">The column number.</param>
        /// <returns>A column vector.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="column"/> is either less than
        /// zero or greater than or equal to the total number of columns.</exception>        
        public ComplexVector GetColumn(int column)
        {
            if (column < 0 || column >= _Columns)
                throw new ArgumentOutOfRangeException("column");

            ComplexMatrix result = new ComplexMatrix(_Rows, 1);

            for (int i = 0; i < _Columns; i++)
                result._Values[i][0] = _Values[i][column];

            return (ComplexVector)result;
        }

        /// <summary>
        /// Sets specified row to the given row vector.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="rowVector">The row vector.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="row"/> is either less than
        /// zero or greater than or equal to the total number of 
        /// rows in this <see cref="ComplexMatrix"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="rowVector"/> must be a row vector and 
        /// have a length equal to the number of columns in this <see cref="ComplexMatrix"/></exception>
        public void SetRow(int row, ComplexVector rowVector)
        {
            if (row < 0 || row >= _Rows)
                throw new ArgumentOutOfRangeException("row");
            if (rowVector.Orientation != VectorType.RowVector)
                throw new ArgumentException("Vector must be mat row vector.");
            if (rowVector.Length != _Columns)
                throw new ArgumentException("vector must have the following length: " + _Columns);

            _Values[row] = (Complex[])rowVector._mat._Values[0].Clone();           
        }

        /// <summary>
        /// Sets the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="colVector">The column vector.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="column"/> is either less than
        /// zero or greater than or equal to the total number of columns
        /// in this <see cref="ComplexMatrix"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="colVector"/> must be a column vector and 
        /// have a length equal to the number of rows in this <see cref="ComplexMatrix"/></exception>        
       [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
       public void SetColumn(int column, ComplexVector colVector)
       {
           Contract.Requires<ArgumentNullException>(colVector != null);
           Contract.Requires<ArgumentOutOfRangeException>((column >= 0) && (column < _Columns));
           Contract.Requires<ArgumentException>(colVector.Orientation == VectorType.ColumnVector);
           Contract.Requires<ArgumentException>(colVector.Length == _Rows);
           
           for (int i = 0; i < _Rows; i++)
               _Values[i][column] = colVector._mat._Values[i][0];            
        }
        
        /// <summary>
        /// An enumerator for iterating over all of the rows of this <see cref="ComplexMatrix"/>.
        /// </summary>        
        public IEnumerable<ComplexVector> RowEnumerator
        {
            get
            {
                for (int i = 0; i < _Rows; i++)
                {
                    yield return new ComplexVector(_Values[i]);
                }
            }
        }


        ///<summary>
        /// An enumerator for iterating over all of the columns of this <see cref="ComplexMatrix"/>.
        ///</summary>
        public IEnumerable<ComplexVector> ColumnEnumerator
        {
            get
            {
                ComplexVector col = new ComplexVector(_Rows, VectorType.ColumnVector);
                for (int i = 0; i < _Columns; i++)
                {
                    for (int j = 0; j < _Rows; j++)
                        col[j] = _Values[j][i];
                    yield return new ComplexVector(col);
                }
            }
        }

        /// <summary>
        /// Transposes this <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <returns>The transposed <see cref="ComplexMatrix"/>.</returns>
        public ComplexMatrix Transpose()
        {
            ComplexMatrix result = new ComplexMatrix(_Columns, _Rows);

            for (int r = 0; r < _Rows; r++)
                for (int c = 0; c < _Columns; c++)
                    result._Values[c][r] = _Values[r][c];

            return result;
        }

        /// <summary>
        /// Convert the current <see cref="ComplexMatrix"/> to a string representation.
        /// </summary>
        /// <returns>A string that represents the current <see cref="ComplexMatrix"/>.</returns>
        public override string ToString()
        {
            StringBuilder retString = new StringBuilder();
            foreach (Complex[] row in _Values)
            {
                retString.Append("|");
                foreach (Complex c in row)
                {
                    retString.AppendFormat("{0,-10:0.0000}", c);
                    retString.Append("|");
                }
                //retString.Append("\n");
            }            

            return retString.ToString();
        }

        /// <summary>
        /// Tests whether the specified object is a <see cref="ComplexMatrix"/> and is equal
        /// to this <see cref="ComplexMatrix"/>.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>This method returns <see langword="true"/> if <paramref name="obj"/> is
        /// the specified <see cref="ComplexMatrix"/> identical to this <see cref="ComplexMatrix"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (GetType() != obj.GetType()) return false;

            ComplexMatrix rhs = (ComplexMatrix) obj;

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
        /// <returns>The hash code for this <see cref="ComplexMatrix"/>.</returns>
        public override int GetHashCode()
        {
            int hash = 0;

            foreach (Complex[] ca in _Values)
                foreach (Complex c in ca)
                    hash ^= c.GetHashCode();
            
            return hash;            
        }

        //////////////////////////////// operator overloads //////////////////////////////////

        ///<summary>
        /// Subtract one <see cref="ComplexMatrix"/> from another.
        ///</summary>
        ///<param name="lhs">Left hand side of the subtraction.</param>
        ///<param name="rhs">Right hand side of the subtraction.</param>
        ///<returns>Result of the subtraction.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs"/> and
        ///<paramref name="rhs"/> do not have the same dimensions.</exception>
        public static ComplexMatrix Subtract(ComplexMatrix lhs, ComplexMatrix rhs)
        {
            return lhs - rhs;
        }

        ///<summary>
        /// Subtract one <see cref="ComplexMatrix"/> from another.
        ///</summary>
        ///<param name="lhs">Left hand side of the subtraction.</param>
        ///<param name="rhs">Right hand side of the subtraction.</param>
        ///<returns>Result of the subtraction.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs"/> and
        ///<paramref name="rhs"/> do not have the same dimensions.</exception>
        public static ComplexMatrix operator -(ComplexMatrix lhs, ComplexMatrix rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if ((lhs.Rows != rhs.Rows) || (lhs.Columns != rhs.Columns))
                throw new SizeMismatchException();

            ComplexMatrix result = new ComplexMatrix(lhs.Rows, lhs.Columns);

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    result._Values[r][c] = lhs._Values[r][c] - rhs._Values[r][c];

            return result;
        }

        ///<summary>
        /// Adds one <see cref="ComplexMatrix"/> to another.
        ///</summary>
        ///<param name="lhs">Left hand side of the addition.</param>
        ///<param name="rhs">Right hand side of the addition.</param>
        ///<returns>Result of the addition.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs "/> and
        ///<paramref name="rhs"/> do not have the same dimensions.</exception>
        public static ComplexMatrix Add(ComplexMatrix lhs, ComplexMatrix rhs)
        {
            return lhs + rhs;
        }

        ///<summary>
        /// Adds one <see cref="ComplexMatrix"/> to another.
        ///</summary>
        ///<param name="lhs">Left hand side of the addition.</param>
        ///<param name="rhs">Right hand side of the addition.</param>
        ///<returns>Result of the addition.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException"><paramref name="lhs "/> and
        ///<paramref name="rhs"/> do not have the same dimensions.</exception>
        public static ComplexMatrix operator +(ComplexMatrix lhs, ComplexMatrix rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if ((lhs.Rows != rhs.Rows) || (lhs.Columns != rhs.Columns))
                throw new SizeMismatchException();

            ComplexMatrix result = new ComplexMatrix(lhs.Rows, lhs.Columns);

            for (int r = 0; r < lhs.Rows; r++)
                for (int c = 0; c < lhs.Columns; c++)
                    result._Values[r][c] = lhs._Values[r][c] + rhs._Values[r][c];

            return result;
        }

        ///<summary>
        /// Negates a <see cref="ComplexMatrix"/>.
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
        /// Negates a <see cref="ComplexMatrix"/>.
        ///</summary>      
        ///<param name="arg"><see cref="ComplexMatrix"/> to negate</param>
        ///<returns>Result of the negation.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="arg"/> is 
        /// a <see langword="null"/> value.</exception>                
        public static ComplexMatrix operator -(ComplexMatrix arg)
        {
            if (arg == null)
                throw new ArgumentNullException("arg");

            ComplexMatrix result = new ComplexMatrix(arg._Rows, arg._Columns);

            for (int r = 0; r < arg._Rows; r++)
                for (int c = 0; c < arg._Columns; c++)
                    result._Values[r][c] = -arg._Values[r][c];

            return result;          
        }

        ///<summary>
        /// Multiplies a <see cref="ComplexMatrix"/> by a complex scalar value. 
        ///</summary>
        ///<param name="lhs">Scalar value.</param>
        ///<param name="rhs"><see cref="ComplexMatrix"/> value.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="rhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static ComplexMatrix Multiply(Complex lhs, ComplexMatrix rhs)
        {
            return lhs*rhs;
        }

        ///<summary>
        /// Multiplies a <see cref="ComplexMatrix"/> by a complex scalar value. 
        ///</summary>
        ///<param name="lhs">Scalar value.</param>
        ///<param name="rhs"><see cref="ComplexMatrix"/> value.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="rhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static ComplexMatrix operator *(Complex lhs, ComplexMatrix rhs)
        {
            if (rhs == null)
                throw new ArgumentNullException("rhs", "ComplexMatrix is null");

            ComplexMatrix result = new ComplexMatrix(rhs._Rows, rhs._Columns);

            for (int r = 0; r < rhs._Rows; r++)
                for (int c = 0; c < rhs._Columns; c++)
                    result[r, c] = rhs._Values[r][c] * lhs;

            return result;            
        }

        ///<summary>
        /// Multiplies a <see cref="ComplexMatrix"/> by a complex scalar value. 
        ///</summary>
        ///<param name="lhs"><see cref="ComplexMatrix"/> value.</param>
        ///<param name="rhs">Scalar value.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static ComplexMatrix Multiply(ComplexMatrix lhs, Complex rhs)
        {
            return lhs*rhs;
        }

        ///<summary>
        /// Multiplies a <see cref="ComplexMatrix"/> by a complex scalar value. 
        ///</summary>
        ///<param name="lhs"><see cref="ComplexMatrix"/> value.</param>
        ///<param name="rhs">Scalar value.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> is
        /// a <see langword="null"/> value.</exception>
        public static ComplexMatrix operator *(ComplexMatrix lhs, Complex rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs", "Matrix is null");

            ComplexMatrix result = new ComplexMatrix(lhs._Rows, lhs._Columns);

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
        public static ComplexMatrix Multiply(ComplexMatrix lhs, ComplexMatrix rhs)
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
        public static ComplexMatrix operator *(ComplexMatrix lhs, ComplexMatrix rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if (lhs.Columns != rhs.Rows)
                throw new SizeMismatchException();

            ComplexMatrix result = new ComplexMatrix(rhs._Columns, lhs._Rows);
            int i;
            Complex tmp;
            for (int r = 0; r < rhs._Columns; r++)
                for (int c = 0; c < lhs._Rows; c++)
                {
                    tmp = new Complex();
                    for (i = 0; i < lhs._Columns; i++)
                        tmp += (lhs._Values[r][i] * rhs._Values[i][c]);
                    result._Values[r][c] = tmp;
                }

            return result;
        }

        ///<summary>
        /// Multiplies a <see cref="ComplexMatrix"/> by a <see cref="ComplexVector"/>.
        ///</summary>
        ///<param name="lhs">Left hand side of the multiplication.</param>
        ///<param name="rhs">Right hand side of the multiplication.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException">The number of columns in <paramref name="lhs"/>
        /// does not equal the number of rows in <paramref name="rhs"/>.</exception>
        public static ComplexVector Multiply(ComplexMatrix lhs, ComplexVector rhs)
        {
            return lhs*rhs;
        }

        ///<summary>
        /// Multiplies a <see cref="ComplexMatrix"/> by a <see cref="ComplexVector"/>.
        ///</summary>
        ///<param name="lhs">Left hand side of the multiplication.</param>
        ///<param name="rhs">Right hand side of the multiplication.</param>
        ///<returns>Result of the multiplication.</returns>
        ///<exception cref="ArgumentNullException"><paramref name="lhs"/> or 
        ///<paramref name="rhs"/> is <see langword="null"/>.</exception>
        ///<exception cref="SizeMismatchException">The number of columns in <paramref name="lhs"/>
        /// does not equal the number of rows in <paramref name="rhs"/>.</exception>
        public static ComplexVector operator *(ComplexMatrix lhs, ComplexVector rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if (lhs.Columns != rhs._mat._Rows)
                throw new SizeMismatchException();

            ComplexVector result = new ComplexVector(lhs._Columns);
            for (int r = 0; r < lhs._Rows; r++)
                for (int c = 0; c < lhs._Columns; c++)
                    result._mat._Values[0][r] += (rhs._mat._Values[0][c] * lhs._Values[r][c]);

            return result;
        }
        
        ///<summary>
        /// Multiplies a <see cref="ComplexVector"/> by a <see cref="ComplexMatrix"/>.
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
        public static ComplexVector Multiply(ComplexVector lhs, ComplexMatrix rhs)
        {
            return lhs*rhs;
        }

        ///<summary>
        /// Multiplies a <see cref="ComplexVector"/> by a <see cref="ComplexMatrix"/>.
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
        public static ComplexVector operator *(ComplexVector lhs, ComplexMatrix rhs)
        {
            if (lhs == null)
                throw new ArgumentNullException("lhs");
            if (rhs == null)
                throw new ArgumentNullException("rhs");
            if (lhs.Orientation != VectorType.RowVector)
                throw new ArgumentException("lhs is not mat row vector.");
            if (lhs.Length != rhs._Rows)
                throw new SizeMismatchException();

            return (new ComplexVector(lhs._mat * rhs));            
        }

        ///<summary>
        /// Tests two complex matrices for equality.
        ///</summary>
        ///<param name="lhs">Left hand side of equality.</param>
        ///<param name="rhs">Right hand side of equality.</param>
        ///<returns><see langword="true"/> if two complex matrices are equal, <see langword="false"/> 
        /// otherwise.</returns>
        public static bool operator ==(ComplexMatrix lhs, ComplexMatrix rhs)
        {
            if (System.Object.ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)lhs == null) || ((object)rhs == null))
            {
                return false;
            }

            if (rhs._Rows != lhs._Rows) return false;
            if (rhs._Columns != lhs._Columns) return false;

            for (int i = 0; i < lhs._Rows; i++)
                for (int j = 0; j < lhs._Columns; j++)
                    if (lhs._Values[i][j] != rhs._Values[i][j]) return false;
                        
            return true;            
        }


        ///<summary>
        /// Tests two complex matrices for inequality.
        ///</summary>
        ///<param name="lhs">Left hand side of inequality.</param>
        ///<param name="rhs">Right hand side of inequality.</param>
        ///<returns><see langword="true"/> if two complex matrices are not equal, <see langword="false"/> 
        /// otherwise.</returns>
        public static bool operator !=(ComplexMatrix lhs, ComplexMatrix rhs)
        {
            return !(lhs == rhs);    
        }


        // ICloneable Implementation
        /// <summary>
        /// Perform a deep copy of this <see cref="Matrix"/>.
        /// </summary>
        /// <returns>The cloned <see cref="Matrix"/>.</returns>
        public object Clone()
        {
            return new ComplexMatrix(this);
        }
    }
}
