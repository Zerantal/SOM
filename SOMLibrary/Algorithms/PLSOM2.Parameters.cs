using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;

namespace SomLibrary.Algorithms
{
    public partial class PLSOM2
    {        
        protected int m_MaxEpoch = 5;
        protected double _beta = 40;

        #region Algorithm Parameters
        [SOMLibProperty("Maximum Epoch", "The maximum training epoch is ignored for dynamic data sets",
            1, int.MaxValue, 5)]
        public int MaxEpoch
        {
            get { return m_MaxEpoch; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value >= 1,
                    //"MaxEpoch must be at least 1");

                m_MaxEpoch = value;
            }
        }

        [SOMLibProperty("Beta", "Neighbourhood range", 0, int.MaxValue, 40)]
        public double Beta
        {
            get { return _beta; }
            set
            {
                // Contract.Requires<ArgumentOutOfRangeException>(value > 0,
                    //"Beta must be greater than zero");

                _beta = value;
            }
        }

        #endregion         
    }
}
