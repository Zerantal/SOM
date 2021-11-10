using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace MathLib.Statistics
{    
    [ExcludeFromCodeCoverage]
    public class ConstantGenerator : INumberGenerator
    {
        readonly double _num;

        public ConstantGenerator(double constant)
        {
            _num = constant;
        }

        public double Number
        {
            get { return _num; }        
        }

        #region IDeepCloneable<INumberGenerator> Members

        public INumberGenerator DeepClone()
        {
            return new ConstantGenerator(_num);            
        }

        #endregion
    }
}
