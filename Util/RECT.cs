using System.Runtime.InteropServices;
using System.Drawing;

namespace Util
{
    // add a reference to system.drawing.dll
    // Note: backing fields were added because structs don't automatically supply them.
    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        int _left;
        int _top;
        int _right;
        int _bottom;

        public RECT(global::System.Drawing.Rectangle rectangle)
            : this(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)
        {
        }
        public RECT(int left, int top, int right, int bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        internal Rectangle ToRectangle()
        {
            return new global::System.Drawing.Rectangle(this._left, this._top, _right - _left, _bottom - _top);
        }
        /*         
                internal static RECT FromRectangle(Rectangle Rectangle)
                {
                    return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
                }
                */
        public override string ToString() => "{Left: " + _left + "; " + "Top: " + _top + "; Right: " + _right + "; Bottom: " + _bottom + "}";

        internal bool Equals(RECT Rectangle)
        {
            return Rectangle._left == _left && Rectangle._top == _top 
                && Rectangle._right == _right && Rectangle._bottom == _bottom;
        }

        public override bool Equals(object Object)
        {
            if (Object is RECT)
            {
                return Equals((RECT)Object);
            }
            else if (Object is Rectangle)
            {
                return Equals(new RECT((global::System.Drawing.Rectangle)Object));
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _left.GetHashCode() ^ _right.GetHashCode() ^ _top.GetHashCode() ^ _bottom.GetHashCode();
        }
    }
}