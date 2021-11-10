using System;
using System.Collections.Generic;
using System.Text;

namespace MathLib.Statistics
{
    public class ConstantGenerator : INumberGenerator
    {
        readonly double Num;
        readonly Int32 iNum;
        public ConstantGenerator(double constant)
        {
            Num = constant;
            iNum = (Int32)Math.Round(constant);
        }

        public Int32 Int32Number
        {
            get { return iNum; }
        }

        public double Number
        {
            get { return Num; }        
        }
    }
}
