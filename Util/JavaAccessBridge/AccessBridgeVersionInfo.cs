using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Util.JavaAccessBridge
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct AccessBridgeVersionInfo 
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.ShortStringSize)]
        private string _VMversion;               // version of the Java VM
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.ShortStringSize)]
        private string _bridgeJavaClassVersion;  // version of the AccessBridge.class
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.ShortStringSize)]
        private string _bridgeJavaDLLVersion;    // version of JavaAccessBridge.dll
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.ShortStringSize)]
        private string _bridgeWinDLLVersion;     // version of WindowsAccessBridge.dll


        public string VirtualMachineVersion { get { return _VMversion; } }

        public string BridgeJavaClassVersion { get { return _bridgeJavaClassVersion; } }

        public string BridgeJavaDllVersion { get { return _bridgeJavaDLLVersion; } }

        public string BridgeWinDllVersion { get { return _bridgeWinDLLVersion; } }
    }
}
