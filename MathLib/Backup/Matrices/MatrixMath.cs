using System;
using System.Collections.Generic;
using System.Text;

namespace MathLib.Matrices
{ 
    /// <summary>
    /// Matrix and Vector based math routines
    /// </summary>
    static public class MatrixMath
    {         /*    
        static public Matrix Exp(Matrix m)
        {
            if (m == null)
                throw new ArgumentNullException();
            Matrix result = new Matrix(m.Rows, m.Columns);
            
            for (int r = 0; r <  m.Rows; r++)
                for (int c = 0; c < m.Columns; c++)
                    result._Values[r][c] = Math.Exp(m._Values[r][c]);

            return result;
        }
        */
        static public Matrix RepeatMatrix(Matrix mat, int horizontalRepetitions, int verticalRepetitions)
        {
            if (mat == null)
                throw new ArgumentNullException("mat");
            if (horizontalRepetitions < 1 || verticalRepetitions < 1)
                throw new ArgumentException("'horizontalRepetitions' and 'verticalRepetitions' must be greater than or equal to 1");

            Matrix result = new Matrix(mat._Rows * horizontalRepetitions, mat._Columns * verticalRepetitions);

            int row = 0, col = 0, r, c;

            for (r = 0; r < mat._Rows; r++)
            {
                for (c = 0; c < mat._Columns; c++)
                {                    
                    for (row = r; row < mat._Rows*horizontalRepetitions; row += mat.Rows)
                    {                                                
                        for (col = c; col < mat._Columns*verticalRepetitions; col += mat._Columns)
                        {
                            result._Values[row][col] = mat._Values[r][c];                           
                        }                                    
                    }
                }
            }            

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sqrt")]
        public static Matrix ElementWiseSqrt(Matrix mat)
        {
            if (mat == null)
                throw new ArgumentNullException("mat");

            Matrix result = new Matrix(mat._Rows, mat._Columns);

            for (int r = 0; r < mat._Rows; r++)
                for (int c = 0; c < mat._Columns; c++)
                    result._Values[r][c] = Math.Sqrt(mat._Values[r][c]);

            return result;
        }
        
        public static Matrix RowNorms(Matrix mat)
        {
            if (mat == null)
                throw new ArgumentNullException("mat");
            
            Matrix tmp = new Matrix(mat._Rows, 1);
            Matrix result;
            int r,c;

            for (r = 0; r < mat._Rows; r++)            
                for (c = 0; c < mat._Columns; c++)                
                    tmp._Values[r][0] += (mat._Values[r][c] * mat._Values[r][c]);

            result = MatrixMath.ElementWiseSqrt(tmp);

            return result;
                            
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "v"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "u")]
        public static double DotProduct(Vector u, Vector v)
        {
            if (u == null)
                throw new ArgumentNullException("u");
            if (v == null)
                throw new ArgumentNullException("v");
            if (u.Length != v.Length)
                throw new SizeMismatchException("Vectors do not have the same size.");

            double result = 0;

            for (int i = 0; i < u.Length; i++)            
                result += u[i] * v[i];            

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "v"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "u")]
        public static double Angle(Vector u, Vector v)
        {
            double vNorm = v.Norm;
            double uNorm = u.Norm;
            
            if (vNorm == 0 || uNorm == 0)
                return 0d;

            return Math.Acos(DotProduct(u, v) / vNorm / uNorm);
        }
        
    }
}
