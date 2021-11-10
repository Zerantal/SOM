using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Diagnostics.Contracts;

using MathLib.Matrices;
using MathLib;

namespace MathLib
{
    /// <summary>
    /// Routines for performing a fast fourier transform on data
    /// </summary>
    static public class Fourier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FFT")]
        public static ComplexVector FFT(ComplexVector data)
        {
            Contract.Requires<ArgumentNullException>(data != null);
            Contract.Requires<ArgumentException>(IsPowerOf2(data.Length), "Length of data vector must be a power of 2.");
            
            Contract.Ensures(Contract.Result<ComplexVector>().Length == data.Length);
            
            return _fft(data, 1);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IFFT")]
        public static ComplexVector IFFT(ComplexVector data)
        {
            Contract.Requires<ArgumentNullException>(data != null);
            Contract.Requires<ArgumentException>(IsPowerOf2(data.Length), "Length of data vector must be a power of 2.");
            
            Contract.Ensures(Contract.Result<ComplexVector>().Length == data.Length);

            return _fft(data, -1);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FFT")]
        static public ComplexMatrix FFT2D(ComplexMatrix data)
        {
            Contract.Requires<ArgumentNullException>(data != null);
            Contract.Requires<ArgumentException>(IsPowerOf2(data.Rows) && (IsPowerOf2(data.Columns)), "Each dimension of data matrix must be a power of 2.");

            Contract.Ensures((Contract.Result<ComplexMatrix>().Rows == data.Rows) && (Contract.Result<ComplexMatrix>().Columns == data.Columns));

            return _fft2d(data, 1);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IFFT")]
        static public ComplexMatrix IFFT2D(ComplexMatrix data)
        {
            Contract.Requires<ArgumentNullException>(data != null);
            Contract.Requires<ArgumentException>(IsPowerOf2(data.Rows) && (IsPowerOf2(data.Columns)), "Each dimension of data matrix must be a power of 2.");

            Contract.Ensures((Contract.Result<ComplexMatrix>().Rows == data.Rows) && (Contract.Result<ComplexMatrix>().Columns == data.Columns));

            return _fft2d(data, -1);
        }

        static private bool IsPowerOf2(int x)
        {
            return (x & (x - 1)) == 0;
        }

        private static ComplexVector _fft(ComplexVector data, int isign)
        {
            int mmax, m, j, istep, i;
            double theta;
            int N = data.Length;
            ComplexVector result = new ComplexVector(N, data.Orientation);
            Complex temp, wp, w, ws;

            // bit reversing part
            j = 0;
            for (i = 0; i < N; i++)
            {
                if (j > i)
                {
                    result[j] = data[i];
                    result[i] = data[j];
                }
                else if (j == i)
                    result[i] = data[i];

                m = N / 2;
                while (m >= 1 && j >= m)
                {
                    j -= m;
                    m >>= 1;
                }
                j += m;
            }

            mmax = 1;
            while (N > mmax)
            {
                istep = mmax << 1;
                theta = isign * (Math.PI / mmax);
                wp = new Complex(-2.0 * BasicMath.Sqr(Math.Sin(0.5 * theta)), Math.Sin(theta));
                w = new Complex(1.0, 0.0);
                for (m = 1; m <= mmax; m++)
                {
                    ws = w;
                    for (i = m - 1; i < N - 1; i += istep)
                    {
                        j = i + mmax;
                        temp = ws * result[j];
                        result[j] = result[i] - temp;
                        result[i] = result[i] + temp;
                    }
                    w = w * wp + w;
                }
                mmax = istep;
            }

            // normalise result if inverse is required
            if (isign == -1)
            {
                result *= (1 / (double)N);
            }

            return result;
        }

        static private ComplexMatrix _fft2d(ComplexMatrix data, int isign)
        {
            int i, j;
            ComplexMatrix result = new ComplexMatrix(data);
            ComplexVector span;
            int nx = data.Columns;
            int ny = data.Rows;

            // transform rows            
            for (j = 0; j < ny; j++)
            {
                span = result.GetRow(j);
                span = _fft(span, isign);
                result.SetRow(j, span);
            }

            // transform columns            
            for (i = 0; i < nx; i++)
            {
                span = result.GetColumn(i);
                span = _fft(span, isign);
                result.SetColumn(i, span);
            }

            return result;
        }
    }
}
