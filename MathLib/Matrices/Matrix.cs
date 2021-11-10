using System;
using System.Diagnostics.Contracts;

using MathLib.Statistics;

namespace MathLib.Matrices
{
    [Serializable]
    public class Matrix : DenseMatrixBase<Matrix, Vector, double>, INumericMatrix<Matrix, Vector, double>
    {        
        #region constructors

        internal Matrix(int rows, int columns)
            : base(rows, columns) 
        {
            // Contract.Requires(rows > 0);
            // Contract.Requires(columns > 0);
            // Contract.Ensures(Rows == rows);
            // Contract.Ensures(Columns == columns);
            // Contract.Ensures(ValuesData != null);
            // Contract.Ensures(ValuesData.GetLength(0) == Rows);
            // Contract.Ensures(ValuesData.GetLength(1) == Columns);

        }

        /// <summary>
        /// Initialise a new instance of the <see cref="Matrix"/> class with a specified
        /// number of rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows in the new matrix.</param>
        /// <param name="columns">The number of columns in the new matrix.</param>
        /// <param name="initialValue">The value to initialiase all elements of the matrix to.</param>
        /// <remarks>All values of the new matrix will be initialised to zero
        /// by default.</remarks>
        /// <exception cref="ArgumentException">Either the <paramref name="rows"/>
        ///  or <paramref name="columns"/> parameter passed to the constructor is a 
        ///  zero or negative number.</exception>
        public Matrix(int rows, int columns, double initialValue = 0.0F) : base(rows, columns, initialValue)
        {
            // Contract.Requires(rows > 0 && columns > 0);
            // Contract.Ensures(this.Columns == columns);
            // Contract.Ensures(this.Rows == rows);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> class with a specified number
        /// of rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows in the new <see cref="Matrix"/>.</param>
        /// <param name="columns">The number of columns in the new <see cref="Matrix"/>.</param>
        /// <param name="numberSource">The number source to use in initialising elements of matrix</param>
        public Matrix(int rows, int columns, INumberGenerator numberSource) : base(rows, columns)
        {
            // Contract.Requires(rows > 0 && columns > 0);
            // Contract.Requires(numberSource != null);                        

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    ValuesData[r, c] = numberSource.Number;
        }

        /// <summary>
        /// Initialise a new instance of the  <see cref="Matrix"/> class with a 2-dimensional
        /// array of double precision values.
        /// </summary>
        /// <param name="values">Values to initialise matrix with.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null </exception>        
        public Matrix(double[,] values) : base(values) 
        {
            // Contract.Requires(values != null);            
        }

        #endregion


        #region INumericMatrix methods

        public Vector RowNorms()
        {
            Vector result = new Vector(Rows, VectorType.ColumnVector);

            for (int r = 0; r < Rows; r++)
                result.ValuesData[r, 0] = GetRow(r).Norm;

            return result;
        }

        public bool IsEqualTo(Matrix arg, double errorTolerance = Constants.Epsilon)
        {
            if ((object)arg == null)
                return false;

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (Math.Abs(ValuesData[r, c] -
                        arg.ValuesData[r, c]) > errorTolerance)
                        return false;

            return true;            
        }
        
        #endregion

        #region Overrides of MatrixBase<Matrix,Vector,double>

        protected override Vector CreateVector(int rows, int columns)
        {
            return rows == 1 ? new Vector(columns) : new Vector(rows, VectorType.ColumnVector);
        }

        protected override Matrix CreateMatrix(int rows, int columns)
        {
            return new Matrix(rows, columns);            
        }

        #endregion
    }
}
