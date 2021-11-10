using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Util.JavaAccessBridge
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct AccessibleContextInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.MaxStringSize)]
        private string _name;             // the AccessibleName of the object
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.MaxStringSize)]
        private string _description;      // the AccessibleDescription of the object
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.ShortStringSize)]
        private string _role;             // localized AccesibleRole string
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.ShortStringSize)]
        private string _role_en_US;	    // AccesibleRole string in the en_US locale
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.ShortStringSize)]
        private string _states;           // localized AccesibleStateSet string
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = JABConstants.ShortStringSize)]
        private string _states_en_US;     // AccesibleStateSet string in the en_US locale (comma separated)

        [MarshalAs(UnmanagedType.I4)]
        private Int32 _indexInParent;     // index of object in parent
        [MarshalAs(UnmanagedType.I4)]
        private Int32 _childrenCount;     // # of children, if any
        [MarshalAs(UnmanagedType.I4)]
        private Int32 _x;                 // screen coords in pixels
        [MarshalAs(UnmanagedType.I4)]
        private Int32 _y;                 // "
        [MarshalAs(UnmanagedType.I4)]
        private Int32 _width;             // pixel width of object
        [MarshalAs(UnmanagedType.I4)]
        private Int32 _height;            // pixel height of object

        [MarshalAs(UnmanagedType.Bool)]
        private bool _accessibleComponent;    // flags for various additional
        [MarshalAs(UnmanagedType.Bool)]
        private bool _accessibleAction;      //  Java Accessibility interfaces
        [MarshalAs(UnmanagedType.Bool)]
        private bool _accessibleSelection;   //  FALSE if this object doesn't
        [MarshalAs(UnmanagedType.Bool)]
        private bool _accessibleText;        //  implement the additional interface
        [MarshalAs(UnmanagedType.Bool)]
        private bool _accessibleInterfaces;  //  in question


        public string Name { get { return _name; } }
        public string Description { get { return _description;}}
        public string Role { get { return _role;}}
        public string RoleENUS { get { return _role_en_US; } }
        public string States { get { return _states; } }
        public string StatesENUS { get { return _states_en_US; } }
        public Int32 IndexInParent { get { return _indexInParent; } }
        public Int32 ChildrenCount { get { return _childrenCount; } }
        public Int32 X { get { return _x; } }
        public Int32 Y { get { return _y; } }
        public Int32 Width { get { return _width; } }
        public Int32 Height { get { return _height; } }
        public bool AccessibleComponent { get { return _accessibleComponent; } }
        public bool AccessibleAction { get { return _accessibleAction; } }
        public bool AccessibleSelection { get { return _accessibleSelection; } }
        public bool AccessibleText { get { return _accessibleText; } }
        public bool AccessibleInterfaces { get { return _accessibleInterfaces; } }
    }
}
