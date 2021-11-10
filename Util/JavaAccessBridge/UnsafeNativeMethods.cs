using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Util.JavaAccessBridge
{
    internal static class UnsafeNativeMethods
    {        
        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void Windows_run();
        
        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal extern static bool isJavaWindow(IntPtr window);
        
        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal extern static bool getAccessibleContextFromHWND(IntPtr window, out IntPtr vmID, out IntPtr accessibleContext);      

        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal extern static bool getAccessibleContextInfo(Int32 vmID, IntPtr accessibleContext, out AccessibleContextInfo info);
        
        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal extern static bool getVersionInfo(Int32 vmID, out AccessBridgeVersionInfo info);        

        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr getAccessibleChildFromContext(Int32 vmID, IntPtr accessibleContext, Int32 childIdx);

        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal extern static bool getVirtualAccessibleName(Int32 vmID, IntPtr accessibleContext,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder name, Int32 nameLength);

        /*
        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr getTopLevelObject(Int32 vmID, IntPtr accessibleContext);
        */
        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void releaseJavaObject(Int32 vmID, IntPtr javaObject);

        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl)]
        internal extern static IntPtr getAccessibleParentFromContext(Int32 vmID, IntPtr accessibleContext);

        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal extern static bool setTextContents(Int32 vmID, IntPtr accessibleContext, string text);

        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal extern static bool doAccessibleActions(Int32 vmID, IntPtr accessibleCont, ref AccessibleActionsToDo actionsToDo, out IntPtr failure);
        /*
        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal extern static bool getAccessibleKeyBindings(Int32 vmID, IntPtr ac, out AccessibleKeyBindings keyBindings);

        /*
        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal extern static bool getAccessibleActions(Int32 vmID, IntPtr ac, IntPtr actionsPtr);
        */
        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal extern static bool getAccessibleTextInfo(Int32 vmID, IntPtr at, out AccessibleTextInfo textInfo, Int32 x, Int32 y);

        [DllImport("WindowsAccessBridge.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal extern static bool getAccessibleTextItems(Int32 vmID, IntPtr at, out AccessibleTextItemsInfo textItems, Int32 index);
    }
}
