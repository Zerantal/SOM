using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Util.JavaAccessBridge
{
    public struct JavaControlRole
    {
        public const string Text = "text";
        public const string PasswordText = "password text";
        public const string Button = "push button";
        public const string Label = "label";
        public const string InternalFrame = "internal frame";
        public const string Table = "table";
        public const string Unspecified = "";
    }

    public class JavaControl
    {
        private readonly IntPtr _accessibleContext;
        private readonly Int32 _vmID;

        internal JavaControl(Int32 vmID, IntPtr ac)
        {
            // Contract.Requires(JAB.IsJABInitialized);

            _vmID = vmID;
            _accessibleContext = ac;
        }

        static private JavaControl CreateControl(Int32 vmID, IntPtr ac)
        {
            // Contract.Requires(JAB.IsJABInitialized);

            AccessibleContextInfo info;
            bool success;            

            success = UnsafeNativeMethods.getAccessibleContextInfo(vmID, ac, out info);
            // Contract.Assert(JAB.IsJABInitialized);
            if (success)
                return CreateControlUsingRole(vmID, ac, info.Role);
            else
            {
                Trace.TraceWarning("Call to getAccessibleContextInfo failed.");
                return new JavaControl(-1, IntPtr.Zero);
            }               
        }

        static private JavaControl CreateControlUsingRole(Int32 vmID, IntPtr ac, string role)
        {
            // Contract.Requires(JAB.IsJABInitialized);

            switch (role)
            {
                case JavaControlRole.Button:
                    return new JavaButtonControl(vmID, ac);

                case JavaControlRole.PasswordText:
                case JavaControlRole.Text:
                    return new JavaTextInputControl(vmID, ac);

                default:
                    return new JavaControl(vmID, ac);                  
            }
        }

        // return first node found matching search criteria
        public JavaControl FindChildControl(string name, string role)
        {
            // Contract.Requires(name != null);
          
            Queue<IntPtr> searchList = new Queue<IntPtr>();
                        
            IntPtr currentAC, childAC;
            AccessibleContextInfo info;
            bool success;
            StringBuilder controlName = new StringBuilder(JABConstants.ShortStringSize);

            // enqueue all child controls first
            info = this._accessibleContextInfo;  
                                   
                // add children controls to search queue
            for (int i = 0; i < info.ChildrenCount; i++)
            {
                childAC = UnsafeNativeMethods.getAccessibleChildFromContext(_vmID, _accessibleContext, i);
                if (childAC != IntPtr.Zero)
                {
                    searchList.Enqueue(childAC);
                }
            }
            
            while (searchList.Count != 0)
            {
                currentAC = searchList.Dequeue();
                // get context info
                success = UnsafeNativeMethods.getAccessibleContextInfo(_vmID, currentAC, out info);
                if (success)
                {
                    success = UnsafeNativeMethods.getVirtualAccessibleName(_vmID, currentAC, controlName, JABConstants.ShortStringSize);
                    if (success)
                    {
                        if (name.Equals(controlName.ToString()) && info.Role.Equals(role))
                            return CreateControlUsingRole(_vmID, currentAC, role);

                        // add children controls to search queue
                        for (int i = 0; i < info.ChildrenCount; i++)
                        {
                            childAC = UnsafeNativeMethods.getAccessibleChildFromContext(_vmID, currentAC, i);
                            if (childAC != IntPtr.Zero)
                            {
                                searchList.Enqueue(childAC);
                            }
                        }
                    }
                }
            }
            return null;
        }

        public JavaControl GetParent()
        {
            IntPtr parentAC = UnsafeNativeMethods.getAccessibleParentFromContext(_vmID, _accessibleContext);

            if (parentAC == IntPtr.Zero)
                return null;
            else
                return CreateControl(_vmID, parentAC);
        }

        public JavaControl GetChild(int childIndex)
        {
            IntPtr childAC = UnsafeNativeMethods.getAccessibleChildFromContext(_vmID, _accessibleContext, childIndex);

            return CreateControl(_vmID, childAC);
        }

        public ReadOnlyCollection<JavaControl> Children
        {          
            get
            {
                List<JavaControl> childControls = new List<JavaControl>();
                AccessibleContextInfo info = this._accessibleContextInfo;
                IntPtr childAC;

                for (int i = 0; i < info.ChildrenCount; i++)
                {
                    childAC = UnsafeNativeMethods.getAccessibleChildFromContext(_vmID, _accessibleContext, i);

                    if (!childAC.Equals(IntPtr.Zero))
                        childControls.Add(new JavaControl(_vmID, childAC));
                    else
                        Trace.TraceWarning("Call to getAccessibleChildFromContext failed.");
                }

                return new ReadOnlyCollection<JavaControl>(childControls);
            }
        }

        public int VirtualMachineId { get { return _vmID; } }

        public bool IsVisible
        {
            get
            {
                AccessibleContextInfo info = _accessibleContextInfo;

                return (info.States.Contains("visible"));
            }
        }

        public int ChildCount
        {
            get
            {
                AccessibleContextInfo info = _accessibleContextInfo;

                return info.ChildrenCount;
            }
        }

        public string Name
        {
            get
            {
                AccessibleContextInfo info = _accessibleContextInfo;
                return info.Name;
            }
        }

        private AccessibleContextInfo _accessibleContextInfo
        {
            get
            {
                AccessibleContextInfo info;
                bool success = UnsafeNativeMethods.getAccessibleContextInfo(_vmID, _accessibleContext, out info);

                if (success)
                    return info;
                else
                {
                    Trace.TraceError("Call to getAccessibleContextInfo failed.");
                    return new AccessibleContextInfo();
                }
                    
            }
        }
       

        protected IntPtr AccessibleContext { get { return _accessibleContext; } }

        ~JavaControl()
        {
            UnsafeNativeMethods.releaseJavaObject(_vmID, _accessibleContext);
        }
    }
}
