using System;
using System.Collections.Generic;
using System.Text;

namespace SomLibrary
{
    static public class Draw
    {
        static public void Lines(IDrawer drawer, Point[] pts, float penWidth, int penColour)
        {
            if (drawer == null || pts == null)
                throw new ArgumentNullException();
            if (pts.Length <= 1)
                throw new ArgumentException("Must be at least 2 points passed in array.");

            for (int i = 0; i < pts.Length - 1; i++ )
                drawer.DrawLine(pts[i], pts[i + 1], penWidth, penColour);            
        }

        static public void FillCircles(IDrawer drawer, Point[] pts, float radius, int paintColour)
        {
            if (drawer == null || pts == null)
                throw new ArgumentNullException();
            if (pts.Length == 0)
                throw new ArgumentException("Must be at least 1 point passed in array.");

            for (int i = 0; i < pts.Length; i++)
                drawer.FillCircle(pts[i], radius, paintColour);
        }
    }
}
