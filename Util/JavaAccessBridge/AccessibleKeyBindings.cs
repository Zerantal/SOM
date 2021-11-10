using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Util.JavaAccessBridge
{
    // all of the key bindings associated with a component
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal class AccessibleKeyBindings
    {
        private Int32 _keyBindingsCount;	// number of key bindings

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = JABConstants.MaxKeyBindings)]
	    private AccessibleKeyBindingInfo[] _keyBindingInfo;

        public AccessibleKeyBindings()
        {
            _keyBindingsCount = 0;
            _keyBindingInfo = new AccessibleKeyBindingInfo[JABConstants.MaxKeyBindings];
        }

        public Int32 KeyBindingsCount { get { return _keyBindingsCount; } }
        public ReadOnlyCollection<AccessibleKeyBindingInfo> KeyBindingInfo
        {
            get
            {
                List<AccessibleKeyBindingInfo> bindings = new List<AccessibleKeyBindingInfo>(_keyBindingsCount);
                for (int i = 0; i < _keyBindingsCount; i++)
                    bindings.Add(_keyBindingInfo[i]);

                return new ReadOnlyCollection<AccessibleKeyBindingInfo>(bindings);
            }
        }

    }

    // a key binding associates with a component
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct AccessibleKeyBindingInfo 
    {
        private char _character;		// the key character
	    private Int32 _modifiers;	    // the key modifiers

        public char Character { get { return _character; } }
        public Int32 Modifiers { get { return _modifiers; } }
    }
}
