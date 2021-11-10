using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Diagnostics.Contracts;

using MathLib.Matrices;

namespace MathLib
{
    public static class MathExtensions
    {
        public static bool IsEqualTo(this double d1, double d2, double errorTolerance = Constants.Epsilon)           
        {
            // Contract.Requires(errorTolerance >= 0);

            return Math.Abs(d1 - d2) < errorTolerance;        
        }       
        
    }
}
