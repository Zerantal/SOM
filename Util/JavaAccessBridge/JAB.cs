using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Diagnostics.Contracts;
using System.Diagnostics;

namespace Util.JavaAccessBridge
{
    public static class JAB
    {
        static bool _isJABInitialized = false;

        public static void InitializeJAB()
        {
            if (_isJABInitialized)
                return;

            if (Application.MessageLoop == false)
                throw new JABException("Unable to initialize java access bridge: no message loop " +
                    "running on thread.");
            UnsafeNativeMethods.Windows_run();
            Application.DoEvents(); // complete initialisation

            _isJABInitialized = true;
        }

        static public bool IsJABInitialized { get { return _isJABInitialized; } }

        #region Convenient methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static ReadOnlyCollection<JavaControl> GetJavaWindows()
        {
            // Contract.Requires<JABException>(JAB.IsJABInitialized, "Java Access Bridge is not initialized");           

            bool success;
            IntPtr ac = IntPtr.Zero, vmIDPtr;
            List<JavaControl> controls = new List<JavaControl>();

            ReadOnlyCollection<Tuple<string, IntPtr>> desktopWindows = WinApiWrapper.GetDesktopWindows();

            foreach (Tuple<string, IntPtr> win in desktopWindows)
            {

                // is window a java window?
                success = UnsafeNativeMethods.isJavaWindow(win.Item2);
                if (success)
                {
                    // get accessible context
                    success = UnsafeNativeMethods.getAccessibleContextFromHWND(win.Item2, out  vmIDPtr, out ac);
                    if (success)
                    {
                        controls.Add(new JavaControl(vmIDPtr.ToInt32(), ac));
                    }
                }
            }

            return new ReadOnlyCollection<JavaControl>(controls);
        }

        public static JavaControl GetJavaControlFromHWND(IntPtr windowHandle)
        {
            // Contract.Ensures(// Contract.Result<JavaControl>() != null);

            bool success;            
            IntPtr ac = IntPtr.Zero, vmIDPtr;

            if (!JAB.IsJABInitialized)
                throw new JABException("Java Access Bridge is not initialized.");

            // is window handle valid?
            if (WinApiWrapper.IsValidWindow(windowHandle) == false)
                throw new JABException("Window handle does not identify a window.");

            // is window a java window?
            success = UnsafeNativeMethods.isJavaWindow(windowHandle);
            if (!success)
            {
                throw new JABException("Window handle does not identify a java window.");
            }

            // get accessible context
            success = UnsafeNativeMethods.getAccessibleContextFromHWND(windowHandle, out vmIDPtr, out ac);
            if (!success)
            {
               throw new JABException("Unable to acquire the accessible context for window!");
            }            
            
            return new JavaControl(vmIDPtr.ToInt32(), ac);
        }    

        public static AccessBridgeVersionInfo GetVersionInfo(Int32 virtualMachineId)
        {
            AccessBridgeVersionInfo versionInfo;

            bool success = UnsafeNativeMethods.getVersionInfo(virtualMachineId, out versionInfo);

            if (!success)
            {
                Trace.TraceWarning("Call to getVersionInfo failed.");
                return new AccessBridgeVersionInfo();
            }

            return versionInfo;
        }

        #endregion
    }
}
