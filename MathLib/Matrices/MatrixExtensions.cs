using System.Diagnostics.Contracts;
using System.Numerics;

namespace MathLib.Matrices
{
    public static class MatrixExtensions
    {
        public static ComplexMatrix AsComplexMatrix(this Matrix<Complex> m)
        {
            // Contract.Requires(m != null);
            // Contract.Ensures(// Contract.Result<ComplexMatrix>() != null);
            // Contract.Ensures(// Contract.Result<ComplexMatrix>().Rows == m.Rows);
            // Contract.Ensures(// Contract.Result<ComplexMatrix>().Columns == m.Columns);
                  
            return new ComplexMatrix(m.Rows, m.Columns, m.ValuesData);           
        }

        public static bool IsEqualTo(this Matrix<Complex> m1, Matrix<Complex> m2, double errorTolerance = Constants.Epsilon)
        {
            // Contract.Requires(errorTolerance >= 0);
            // Contract.Requires(m1 != null);

            if ((object)m2 == null)
                return false;

            for (int r = 0; r < m1.Rows; r++)
                for (int c = 0; c < m1.Columns; c++)
                    if (Complex.Abs(m1[r, c] - m2[r, c]) > errorTolerance)
                        return false;

            return true;
        }
    }
}
