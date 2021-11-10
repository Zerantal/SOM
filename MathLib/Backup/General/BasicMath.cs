using System;
using System.Collections.Generic;
using System.Text;

namespace MathLib
{
    /// <summary>
    /// Basic math routines
    /// </summary>
    static public class BasicMath
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "n"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sqr")]
        static public double Sqr(double n)
        {
            return n * n;
        }        
    }
}
