using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;

namespace Util
{
    static public class WinApiWrapper
    {
        public const int PostMouseDelay = 250; //quarter of a second
        const int MAXTITLE = 255;

        private const int MOUSEEVENTF_MOVE = 0x00000001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x00000002;
        private const int MOUSEEVENTF_LEFTUP = 0x00000004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x00000008;
        private const int MOUSEEVENTF_RIGHTUP = 0x00000010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x00000020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x00000040;
        private const int MOUSEEVENTF_WHEEL = 0x00000800;
        private const int MOUSEEVENTF_ABSOLUTE = 0x00008000;



        private static bool AddWindowTitleToList(IntPtr hWnd, ref object windowList)
        {
            // Contract.Requires(windowList != null);
            // Contract.Requires(windowList is List<Tuple<string, IntPtr>>);

            List<Tuple<string, IntPtr>> openWindows = windowList as List<Tuple<string, IntPtr>>;
  //          // Contract.Assume(openWindows != null);

            string title = GetWindowText(hWnd);
            if (title != null)
                openWindows.Add(new Tuple<string, IntPtr>(title, hWnd));
            return true;
        }
       
        /// <summary>
        /// Returns the caption of a windows by given HWND identifier.
        /// </summary>
        public static string GetWindowText(IntPtr hWnd)
        {
            StringBuilder title = new StringBuilder(MAXTITLE);
            int titleLength = NativeMethods.GetWindowText(hWnd, title, title.Capacity + 1);
            int errorCode = Marshal.GetLastWin32Error();

            if (titleLength <= 0)
            {
                if (errorCode != 0x0)
                    return null;
            }
            else
                title.Length = titleLength;

            return title.ToString();
        }

        public static ReadOnlyCollection<Tuple<string, IntPtr>> GetDesktopWindows()
        {
            object openWindows = new List<Tuple<string, IntPtr>>();
           
            NativeMethods.EnumDelegate enumfunc = new NativeMethods.EnumDelegate(AddWindowTitleToList);
            IntPtr hDesktop = IntPtr.Zero; // current desktop
            bool success = NativeMethods.EnumDesktopWindows(hDesktop, enumfunc, ref openWindows);

            if (!success)
            {
                // Get the last Win32 error code
                int errorCode = Marshal.GetLastWin32Error();

                throw new WinApiException("Failed to enumerate desktop windows.", sourceDll: "user32.dll",
                    invokedFunction: "EnumDesktopWindows", errorCode: errorCode); 

            }
            else
            {
                List<Tuple<String, IntPtr>> retList = openWindows as List<Tuple<string, IntPtr>>;

                if (retList != null)
                    return new ReadOnlyCollection<Tuple<string, IntPtr>>(retList);
                else // wrap empty list (shouldn't get here)
                    return new ReadOnlyCollection<Tuple<string, IntPtr>>(new List<Tuple<string, IntPtr>>());
            }
        }

        public static void MouseLeftClick(int xValue, int yValue)
        {            
            Cursor.Position = new Point(xValue, yValue);
            NativeMethods.mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero);
            NativeMethods.mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(PostMouseDelay);
        }

        public static bool IsValidWindow(IntPtr windowHandle)
        {
            return NativeMethods.IsWindow(windowHandle);            
        }

        public static bool SetForegroundWindow(IntPtr winHandle)
        {
            return NativeMethods.SetForegroundWindow(winHandle);                        
        }

        public static Rectangle GetWindowRectangle(IntPtr winHandle)
        {
            RECT winRect;

            if (!NativeMethods.GetWindowRect(winHandle, out winRect))
            {
                // Get the last Win32 error code
                int errorCode = Marshal.GetLastWin32Error();

                throw new WinApiException("Failed to get window dimensions.", sourceDll: "user32.dll",
                    invokedFunction: "GetWindowRect", errorCode: errorCode);  
            }

            return winRect.ToRectangle();
        }

        public static IntPtr FindWindow(string caption)
        {
            IntPtr winHandle = NativeMethods.FindWindow(null,caption);

            return winHandle;
        }
    }
}
