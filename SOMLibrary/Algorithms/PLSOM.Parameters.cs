using System;
using System.Diagnostics.Contracts;

namespace SomLibrary.Algorithms
{
    public partial class PLSOM
    {            
        protected int MMaxEpoch = 5;
        protected double _beta = 25;

        #region Algorithm Parameters
         [SOMLibProperty("Maximum Epoch", "The maximum training epoch is ignored for dynamic data sets",
             1, Int16.MaxValue, 5)]
        public int MaxEpoch
        {
            get { return MMaxEpoch; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 1,
                    //"MaxEpoch should be at least 1");
                MMaxEpoch = value;
            }
        }

        [SOMLibProperty("Beta", "neighbourhood size", 0, int.MaxValue, 25)]
        public double Beta
        {
            get { return _beta; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value > 0,
                    //"Beta should be greater than zero");
                _beta = value;
            }
        }

        #endregion         
    }
}
