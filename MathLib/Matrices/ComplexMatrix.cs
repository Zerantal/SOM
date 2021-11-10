using System;
using System.Numerics;
using System.Diagnostics.Contracts;

using MathLib.Statistics;

namespace MathLib.Matrices
{
    [Serializable]
    public class ComplexMatrix : DenseMatrixBase<ComplexMatrix, ComplexVector, Complex>
    {        
        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="ComplexMatrix"/> class
        /// with a specified number of rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows in the new <see cref="ComplexMatrix"/>.</param>
        /// <param name="columns">The number of columns in the new <see cref="ComplexMatrix"/>.</param>
        /// <param name="initialValue">The value to initial all elements of matrix to.</param>
        /// <remarks>All values of the new <see cref="ComplexMatrix"/> will be initialised to zero
        /// by default.</remarks>
        /// <exception cref="ArgumentException">Either the <paramref name="rows"/>
        ///  or <paramref name="columns"/> parameter passed to the constructor is a 
        ///  zero or negative number.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public ComplexMatrix(int rows, int columns, Complex initialValue = default(Complex))
            : base(rows, columns, initialValue)
        {
            // Contract.Requires(rows > 0 && columns > 0);
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="ComplexMatrix"/> class
        /// with a specified number of rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows in the new <see cref="ComplexMatrix"/>.</param>
        /// <param name="columns">The number of columns in the new <see cref="ComplexMatrix"/>.</param>
        /// <param name="realNumberGenerator">A number source for initialising all real elements of matrix to.</param>
        /// <param name="imaginaryNumberGenerator">A number source for initialising all imaginary elements of matrix to.</param>
        /// <remarks>All values of the new <see cref="ComplexMatrix"/> will be initialised to zero
        /// by default.</remarks>
        /// <exception cref="ArgumentException">Either the <paramref name="rows"/>
        ///  or <paramref name="columns"/> parameter passed to the constructor is a 
        ///  zero or negative number.</exception>
        public ComplexMatrix(int rows, int columns, INumberGenerator realNumberGenerator, INumberGenerator imaginaryNumberGenerator) : base(rows, columns)          
        {
            // Contract.Requires(realNumberGenerator != null && imaginaryNumberGenerator != null);
            // Contract.Requires(rows > 0 && columns > 0);            

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                    ValuesData[r, c] = new Complex(realNumberGenerator.Number, imaginaryNumberGenerator.Number);  // initialise matrix
            }
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="ComplexMatrix"/> class
        /// with a 2-dimensional array of <see cref="Complex"/> numbers.
        /// </summary>
        /// <param name="values">Values to initialise <see cref="ComplexMatrix"/> with.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null </exception>        
        public ComplexMatrix(Complex[,] values)
            : base(values)
        {
            // Contract.Requires(values != null);
            // Contract.Requires(values.GetLength(0) > 0 && values.GetLength(1) > 0);
        }

        internal ComplexMatrix(int rows, int columns, Complex[,] sourceData) : base(rows, columns, sourceData)
        {
            // Contract.Requires(sourceData != null);
            // Contract.Requires(rows > 0 && columns > 0);                        
        }

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public bool IsEqualTo(ComplexMatrix matrix, double errorTolerance = Constants.Epsilon)
        {            
            if ((object)matrix == null)
                return false;

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (Complex.Abs(this[r, c] - matrix[r, c]) > errorTolerance)
                        return false;

            return true;
        }

        #region ICloneable Members

        public override ComplexMatrix DeepClone()
        {
            ComplexMatrix clone = new ComplexMatrix(Rows, Columns);                                      

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    clone.ValuesData[r, c] = ValuesData[r, c];

            return clone;            
        }

        #endregion

        #region Overrides of MatrixBase<ComplexMatrix,ComplexVector,Complex>

        protected override ComplexVector CreateVector(int rows, int columns)
        {
            return new ComplexVector(rows, columns);            
        }

        protected override ComplexMatrix CreateMatrix(int rows, int columns)
        {
            return new ComplexMatrix(rows, columns);            
        }

        #endregion
    }
}