using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace MathLib
{
    /// <summary>
    /// Basic math routines
    /// </summary>
    static public class BasicMath
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "n"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sqr")]
        [ExcludeFromCodeCoverage]
        static public double Sqr(double n)
        {
            return n * n;
        }

        [Pure()]
        [ExcludeFromCodeCoverage]
        static public bool IsPowerOf2(int arg)
        {
            // Contract.Requires(arg >= 0);
            return (arg & (arg - 1)) == 0;
        }
    }
}
