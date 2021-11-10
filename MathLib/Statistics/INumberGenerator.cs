using System;

using Util;

namespace MathLib.Statistics
{
    public interface INumberGenerator : IDeepCloneable<INumberGenerator>
    {
        double Number {get;}
    }
}
