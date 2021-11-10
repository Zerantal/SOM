using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics.Contracts;

// Matrix already defined in SomLibrary.Util

namespace SomLibrary.Drawers
{
    public class WinDrawer : IDrawer
    {
        Bitmap _backBuffer;
        private Graphics _drawingArea;
        const float Margin = 0.05f;       // fraction of drawingArea to use as a margin

        public WinDrawer(int width, int height)
            : this(new Size(width, height))
        {
        }

        public WinDrawer(Size size)            
        {
            ResizeDrawer(size);                        
        }       

        private Pen GetPen(int colour, float width)
        {
            return new Pen(Color.FromArgb(colour), width);
        }

        private Brush GetBrush(int colour)
        {
            return new SolidBrush(Color.FromArgb(colour));
        }

        public void Clear()
        {
            _drawingArea.Clear(Color.White);
        }

        public void DrawLine(Point pt1, Point pt2, float width, int colour)
        {
            _drawingArea.DrawLine(GetPen(colour, width),
                pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        public void DrawCircle(Point pt, float radius, float penWidth, int penColour)
        {
            float tmp = radius / 1.414213562f;
            float wh = 2 * tmp; // width and height of bounding rectangle
            
            _drawingArea.DrawEllipse(GetPen(penColour, penWidth),
                pt.X - tmp, pt.Y - tmp, wh, wh);            
        }

        public void FillCircle(Point pt, float radius, int paintColour)
        {
            float tmp = radius / 1.414213562f;
            float wh = 2 * tmp; // width and height of bounding rectangle

            _drawingArea.FillEllipse(GetBrush(paintColour),
                pt.X - tmp, pt.Y - tmp, wh, wh);
        }

        public void DrawRectangle(Point bottomLeft, float width, float height, float penWidth, int penColour)
        {
            _drawingArea.DrawRectangle(GetPen(penColour, penWidth),
                    bottomLeft.X, bottomLeft.Y, width, height);                                
        }

        public void FillRectangle(Point bottomLeft, float width, float height, int paintColour)
        {
            _drawingArea.FillRectangle(GetBrush(paintColour),
                bottomLeft.X, bottomLeft.Y, width, height);
        }

        public void FillPolygon(Point[] pts, int paintColour)
        {
            PointF[] _pts = new PointF[pts.Length];
            for (int i = 0; i < pts.Length; i++)
                _pts[i] = new PointF(pts[i].X, pts[i].Y);

            _drawingArea.FillPolygon(GetBrush(paintColour), _pts);
        }

        public void DrawText(Point pt, string text, float size, int colour)
        {
            float s, m;
            Matrix t;

            Brush br = GetBrush(colour);
            Font f = new Font("Arial", size);
            s = (float) _drawingArea.Transform.Elements.GetValue(0);
            m = (float) _drawingArea.Transform.Elements.GetValue(4);
            t = new Matrix(s, 0, 0, s, m, m);           
            _drawingArea.Transform = t;
            _drawingArea.DrawString(text, f, br, new PointF(pt.X, 1 - pt.Y));
            _drawingArea.Transform = new Matrix(s, 0, 0, -s, m, s+m);

        }

        public Image GetImage()
        {
            // Contract.Ensures(// Contract.Result<Image>() != null);
            
            return _backBuffer;
        }

        // change width of drawer (in pixels)
        public int Width
        {
            get { return _backBuffer.Width; }
            
            set 
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Width", "Width must be greater than zero");

                ResizeDrawer(new Size(value, _backBuffer.Height)); 
            }
        }

        // change height of drawer (in pixels)
        public int Height
        {
            get { return _backBuffer.Height; }

            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Height", "Height must be greater than zero");

                ResizeDrawer(new Size(_backBuffer.Width, value));
            }
        }

        public Size Size
        {
            get { return _backBuffer.Size; }
            set 
            {
                if (value.Width <= 0 || value.Height <= 0)
                    throw new ArgumentOutOfRangeException("Size", "Width and Height must both be greater than zero");

                ResizeDrawer(value);
            }
        }

        private void ResizeDrawer(Size newSize)
        {
            float m, s; // margin and actual screen size respectively

            s = newSize.Width < newSize.Height ? newSize.Width : newSize.Height;
            m = s * Margin;
            s -= 2 * m;

            _backBuffer = new Bitmap(newSize.Width, newSize.Height);
            _drawingArea = Graphics.FromImage(_backBuffer);
            _drawingArea.Transform = new Matrix(s, 0, 0, -s, m, s + m);
            _drawingArea.SmoothingMode = SmoothingMode.HighQuality;            
        }
    }
}
