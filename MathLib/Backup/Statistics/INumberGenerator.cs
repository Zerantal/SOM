using System;
using System.Collections.Generic;
using System.Text;

namespace MathLib.Statistics
{
    public interface INumberGenerator
    {
        Int32 Int32Number { get; }
        double Number {get;}
    }
}
