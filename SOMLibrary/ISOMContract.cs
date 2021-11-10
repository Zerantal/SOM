using System;
using System.Diagnostics.Contracts;

namespace SomLibrary
{
    [ContractClassFor(typeof(ISOM))]
    internal abstract class ISOMContract : ISOM
    {

        #region ISOM Members

        public event EventHandler ProgressUpdate
        {
            add { }
            remove { }
        }

        public void Train()
        {
            ISOM @this = this;
            // Contract.Requires(@this.InputReader != null);            
            // Contract.Requires(@this.InputReader.InputDimension == @this.Map.InputDimension);            
        }

        public int Simulate(MathLib.Matrices.Vector x, out double error)
        {
            ISOM @this = this;
            // Contract.Requires(x != null);            
            // Contract.Requires(@this.Map.InputDimension == x.Length);           

            throw new NotImplementedException();            
        }

        public INeuronMap Map
        {
            get
            {
                // Contract.Ensures(// Contract.Result<INeuronMap>() != null);

                throw new NotImplementedException();
            }
            set
            {
                // Contract.Requires(value != null);
            }
        }

        public int ProgressInterval
        {
            get
            {
                return 0;
            }
            set
            {
                // Contract.Requires(value >= 0);
            }
        }

        public IInputLayer InputReader
        {
            get
            {
                return default(IInputLayer);
            }
            set
            {
                // Contract.Requires(value != null);
            }
        }

        public void CancelTraining()
        {
        }

        #endregion       
    }
}
