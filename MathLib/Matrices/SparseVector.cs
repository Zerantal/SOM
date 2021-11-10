using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MathLib.Matrices
{
    [ContractVerification(true)]
    [Serializable]
    public class SparseVector : SparseMatrix, IVector<SparseVector, double>, INumericVector
    {
        #region Constructors

        public SparseVector(int dimension, VectorType orientation = VectorType.RowVector)
            : base(orientation == VectorType.RowVector ? 1 : dimension, orientation == VectorType.RowVector ? dimension : 1)
        {
            // Contract.Requires(dimension > 0);
            // Contract.Requires(dimension < int.MaxValue);
        }

        public SparseVector(int dimension, IEnumerable<Tuple<int, double>> initialValues, VectorType orientation = VectorType.RowVector)
            : base(orientation == VectorType.RowVector ? 1 : dimension, orientation == VectorType.RowVector ? dimension : 1)

        {
            // Contract.Requires(dimension > 0);
            // Contract.Requires(dimension < int.MaxValue);
            // Contract.Requires(initialValues != null);
            // Contract.Requires(// Contract.ForAll<Tuple<int, double>>
                //(initialValues, new Predicate<Tuple<int, double>>(
                //                    t => (t != null && t.Item1 < dimension && t.Item1 >= 0))));

            throw new NotImplementedException();
        }

        #endregion

        #region IVector<double> Members

        public VectorType Orientation
        {
            get { throw new NotImplementedException(); }
        }

        public int Length
        {
            get { throw new NotImplementedException(); }
        }

        public double this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public SparseVector ArrayMultiplication(SparseVector rhs)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(SparseVector destVector, int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region INumericVector Members

        public double Norm
        {
            get { throw new NotImplementedException(); }
        }

        public double NormSquared
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsEqualTo(SparseVector arg, double errorTolerance)
        {
            throw new NotImplementedException();
        }

        public double InfinityNorm
        {
            get { throw new NotImplementedException(); }
        }

        public double OneNorm
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

    }
}
