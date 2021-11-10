using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using MathLib.Matrices;

namespace SomLibrary
{
    [ContractClassFor(typeof(IInputLayer))]
    internal abstract class IInputLayerContract : IInputLayer
    {
        #region IInputLayer Members

        public int InputDimension
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsOnlineSource
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable<Vector> Members

        public IEnumerator<Vector> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
