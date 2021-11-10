using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

using MathLib.Matrices;
using MathLib.Statistics;

namespace MathLib.Evolution
{
    public class ESObject : IEvolvableObject<ESObject>
    {
        /*
        double _sigma;
        private double _tau;
        private T _soln;

        NormalRandomGenerator randn;

        public ESObject(T soln)
        {                                   
            _tau = 1 / Math.Sqrt(2 * soln.NumberOfParameters);
            _soln = soln;
            _sigma = 1;
            randn = new NormalRandomGenerator(0, 1);
        }

        private ESObject(double strategyParameter, T soln)
        {
            _sigma = strategyParameter; 
            _tau = 1 / Math.Sqrt(2 * soln.NumberOfParameters);
            _soln = soln;

            randn = new NormalRandomGenerator(0, 1);
        }

        #region IEvolvableObject<ESObject> Members

        public ESObject<T> RecombineWith(ESObject<T> additionalParent)
        {
            int numParents = additionalParent.Count() + 1;
            double meanSigma = _sigma;
            Vector meanParameterSet = _soln.ParameterSet.DeepClone();

            foreach (ESObject<T> f in additionalParent)
            {
                meanSigma += f.StrategyParameter;
                meanParameterSet += f._soln.ParameterSet;                
            }

            return new ESObject<T>(meanSigma / numParents,
                _soln.CreateNewSolution(meanParameterSet / numParents));            
        }

        public void Mutate()
        {
            // mutate strategy parameter
            _sigma *= Math.Exp(_tau * randn.Number);

            // mutate parameter set
            _soln.ParameterSet += _sigma * new Vector(_soln.NumberOfParameters, VectorType.RowVector, randn);
        }

        public double Fitness()
        {
            return _soln.Fitness();            
        }

        #endregion

        public double StrategyParameter
        {
            get { return _sigma; }          
        }

        public T Solution
        {
            get { return _soln; }
        }

        public double Tau
        {
            get { return _tau; }
            set { _tau = value; }
        }
         * */
        #region IEvolvableObject<ESObject> Members

        public ESObject RecombineWith(ESObject additionalParent)
        {
            throw new NotImplementedException();
        }

        public void Mutate()
        {
            throw new NotImplementedException();
        }

        public double Fitness()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
