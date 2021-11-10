using System;
using System.Diagnostics.Contracts;

namespace Util
{
    [ContractClassFor(typeof(IShallowCloneable<>))]
    abstract class IShallowCloneableContract<T> : IShallowCloneable<T>
    {
        #region IShallowCloneable<T> Members

        public T ShallowClone()
        {
            // Contract.Ensures(// Contract.Result<T>() != null);
            throw new NotImplementedException();
        }

        #endregion
    }
}
