using System;
using System.Diagnostics.Contracts;

namespace MathLib.Evolution
{
    [ContractClassFor(typeof(IEvolvableObject<>))]
// ReSharper disable InconsistentNaming
    abstract class IEvolvableObjectContract<T> : IEvolvableObject<T> where T : class, IEvolvableObject<T>
// ReSharper restore InconsistentNaming
    {
        #region IEvolvableObject<T> Members

        public T RecombineWith(T additionalParent)
        {
            // Contract.Requires(additionalParent != null);            
            // Contract.Ensures(// Contract.Result<T>() != null);

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
