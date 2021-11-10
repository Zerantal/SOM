using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;

namespace Util.JavaAccessBridge
{
    // an action assocated with a component
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct AccessibleActionInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.ShortStringSize)]
        private string _name;	        // action name

        public AccessibleActionInfo(string actionName)
        {
            // Contract.Requires(actionName != null);

            _name = actionName;
        }

        public string Name { get { return _name; } }
    }
}
