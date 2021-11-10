using System;
using System.Diagnostics.Contracts;

namespace SomLibrary
{
    [ContractClassFor(typeof(IVisualiser))]
    abstract class IVisualiserContract : IVisualiser
    {
        #region IVisualiser Members

        void IVisualiser.VisualiseMap(ISOM algorithm, IDrawer drawer)
        {
            // Contract.Requires(algorithm != null);
            // Contract.Requires(drawer != null);
            throw new NotImplementedException();
        }

        bool IVisualiser.CanVisualiseMap(ISOM algorithm)
        {
            // Contract.Requires(algorithm != null);
            throw new NotImplementedException();
        }

        #endregion
    }
}
