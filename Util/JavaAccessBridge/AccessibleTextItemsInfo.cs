using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Util.JavaAccessBridge
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct AccessibleTextItemsInfo
    {
        private char _letter;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.ShortStringSize)]
        private string _word;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.MaxStringSize)]
        private string _sentence;

        public char Letter { get { return _letter; } }
        public string Word { get { return _word; } }
        public string Sentence { get { return _sentence; } }

    }
};
