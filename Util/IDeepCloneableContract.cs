using System;
using System.Diagnostics.Contracts;

namespace Util
{
    [ContractClassFor(typeof(IDeepCloneable<>))]
    abstract class IDeepCloneableContract<T> : IDeepCloneable<T>
    {
        #region IDeepCloneable<T> Members

        public T DeepClone()
        {
            // Contract.Ensures(// Contract.Result<T>() != null);

            throw new NotImplementedException();
        }

        #endregion
    }
}
