using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Util.JavaAccessBridge
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AccessibleTextInfo
    {
        private Int32 charCount;       // # of characters in this text object
        private Int32 caretIndex;      // index of caret
        private Int32 indexAtPoint;    // index at the passsed in point

        public int CharCount { get { return charCount; } }
        public int CaretIndex { get { return caretIndex; } }
        public int IndexAtPoint { get { return indexAtPoint; } }
    }
}
