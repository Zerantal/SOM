using System;
using System.Collections.Generic;
using System.Text;
using SomLibrary;
using System.Diagnostics;
using System.Diagnostics.Contracts;


namespace SOMSimulator
{
    [Serializable]
    internal class DefaultVisualiser : IVisualiser
    {
        private int m_CurrentIter = 0;         
        private int m_IterationInc = 0;       
        private bool m_IsExecuting = false;

        public void VisualiseMap(ISOM algorithm, IDrawer drawer)
        {
            StringBuilder text = new StringBuilder();
                        
            text.Append("Completed " +
                m_CurrentIter + " iterations.");
            
            if (m_IsExecuting)
                m_CurrentIter += m_IterationInc;
            
            drawer.Clear();
            drawer.DrawText(new Point(0f, 0.9f), text.ToString(), 0.04f, unchecked((int)0xff000000));
            
            /*
            // Test drawing primitives
            mapDrawer.DrawLine(new Point(0, 1), new Point(1, 1), 0.01f, unchecked((int)0xFFFF0000));
            mapDrawer.DrawLine(new Point(1, 1), new Point(1, 0), 0.01f, unchecked((int)0xFF00ff00));
            mapDrawer.DrawLine(new Point(1, 0), new Point(0, 0), 0.01f, unchecked((int)0xFFff00ff));
            mapDrawer.DrawLine(new Point(0, 0), new Point(0, 1), 0.01f, unchecked((int)0xFFffff00));
            mapDrawer.DrawLine(new Point(0, 1), new Point(1, 0), 0.01f, unchecked((int)0xFF0000ff));
            mapDrawer.DrawLine(new Point(0, 0), new Point(1, 1), 0.01f, unchecked((int)0xFF00ffff));
            Point[] pts = new Point[5];
            pts[0] = new Point(0.25f, 0.25f);
            pts[1] = new Point(0.25f, 0.75f);
            pts[2] = new Point(0.75f, 0.75f);
            pts[3] = new Point(0.75f, 0.25f);
            pts[4] = new Point(0.25f, 0.25f);
            mapDrawer.DrawLines(pts, 0.02f, unchecked((int)0xFFc07ae8));
            mapDrawer.DrawCircle(new Point(0.25f, 0.25f), 0.1f, 0.01f, unchecked((int)0xff7ae899), false);
            mapDrawer.DrawCircle(new Point(0.25f, 0.75f), 0.1f, 0.01f, unchecked((int)0xff7ae899), true);
            mapDrawer.DrawCircle(new Point(0.75f, 0.75f), 0.1f, 0.01f, unchecked((int)0xff7ae899), false);
            mapDrawer.DrawCircle(new Point(0.75f, 0.25f), 0.1f, 0.01f, unchecked((int)0xff7ae899), true);*/
        }

        public bool CanVisualiseMap(ISOM algorithm)
        {
            return true;
        }

        public void Start(int iterInc)
        {
            m_CurrentIter = 0;
            m_IterationInc = iterInc;                       
            m_IsExecuting = true;
        }    

        public void Stop()
        {
            m_IsExecuting = false;
            m_CurrentIter -= m_IterationInc;
        }
    }
}
