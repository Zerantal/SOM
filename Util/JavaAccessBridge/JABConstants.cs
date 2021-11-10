using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.JavaAccessBridge
{
    static public class JABConstants
    {
        public const int MaxStringSize = 1024;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "short")]
        public const int ShortStringSize = 256;

        public const int MaxActionInfo = 256;

        public const int MaxActionsToDo = 32;

        public const int MaxKeyBindings = 8;
    }
}
