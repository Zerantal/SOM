using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SomLibrary.NeuronMaps;

namespace SomLibrary
{
    public class LabeledNeuron2DVisualiser //: IVisualiser
    {
        private IDrawer m_Drawer;

        public void VisualiseMap(ISOM algorithm)
        {            
            if (m_Drawer == null)
                throw new InvalidOperationException("drawer has not been set");
            if (!CanVisualiseMap(algorithm))
                throw new InvalidAlgorithmException("Algorithm must be using a RectNeuronMap.");
            /*
            RectNeuronMap map = algorithm.Map as RectNeuronMap;
            float xMin = float.MaxValue;
            float yMin = float.MaxValue;
            float xMax = float.MinValue;
            float yMax = float.MinValue;
            int x, y;//, height, width;                    
            //int idx = 0;
            float scale;          
                        
            m_Drawer.Clear();            
            List<Point> nodes = new List<Point>(map.MapSize);
            List<Point> labeledPts = new List<Point>();
            List<Neuron> labeledNeurons = new List<Neuron>();

            Neuron tmp;
            List<string> l;
            double se;
            BufferedInputLayer data = algorithm.InputReader as BufferedInputLayer;
            Dictionary<int, List<string>> labels =
                new Dictionary<int, List<string>>();            
            for (int i = 0; i < data.BufferSize; i++)
            {
                tmp = algorithm.Simulate(data.Peek(i), out se);
                if (labels.TryGetValue(tmp.Index, out l))
                {                    
                    l.Add(data.GetLabel(i));
                }
                else
                {
                    labels.Add(tmp.Index, new List<string>(new string [] {data.GetLabel(i)}));
                }
            }

            int[] neuronPosition;
            foreach (Neuron n in map)
            {
                neuronPosition = map.NeuronPosition(n);
                x = neuronPosition[0];
                y = neuronPosition[1];

                if (x < xMin)
                    xMin = x;
                if (y < yMin)
                    yMin = y;
                if (x > xMax)
                    xMax = x;
                if (y > yMax)
                    yMax = y;

                if (labels.ContainsKey(n.Index))
                {
                    labeledPts.Add(new Point(x, y));
                    labeledNeurons.Add(n);
                }
                else
                    nodes.Add(new Point(x, y));                               
            }

            if ((xMax - xMin) < (yMax - yMin))
            {
                scale = ((float)yMax - (float)yMin);                    
            }
            else
            {
                scale = ((float)xMax - (float)xMin);
            }

            for (int i = 0; i < nodes.Count; i++)                
            {                
                nodes[i] = new Point((nodes[i].X - xMin) / scale, (nodes[i].Y - yMin) / scale);
            }
            for (int i = 0; i < labeledPts.Count; i++)
            {
                labeledPts[i] = new Point((labeledPts[i].X - xMin) / scale, (labeledPts[i].Y - yMin) / scale);
            }
            
            m_Drawer.DrawCircles(nodes.ToArray(),0.01f, 0.005f, unchecked((int)0xFF000000), false);
            m_Drawer.DrawCircles(labeledPts.ToArray(), 0.01f, 0.005f, unchecked((int)0xFFFF0000), true);
            
            int numberLabel = 1;
            for (int i = 0; i < labeledNeurons.Count; i++)                
            {
                if (labels[labeledNeurons[i].Index].Count > 5)
                {
                    labels[labeledNeurons[i].Index][0] = "(" + numberLabel.ToString() + ")";
                    numberLabel++;
                }
                if (labels[labeledNeurons[i].Index].Count != 0)
                    m_Drawer.DrawText(labeledPts[i], labels[labeledNeurons[i].Index][0], 0.02f, unchecked((int)0xFF000000));
            }

            /*
            Rect2dNeuronMap m = (Rect2dNeuronMap) map;    // convenient reference
            drawer.Clear();
            float sx = (float)drawer.Width;
            float sy = (float)drawer.Height;
            foreach (PointF[] pts in m.WeightVectorEnum)
            {
                DrawingUtil.ScalePts(pts, sx, sy);
                if (pts.Length > 1)  // in case map is one-dimensional
                {
                    drawer.DrawLines(pts);
                    drawer.DrawPoints(pts);
                }
            }*/
        }
        
        public bool CanVisualiseMap(ISOM algorithm)
        {
            INeuronMap map = algorithm.Map;
            if (map == null || !(map is RectNeuronMap) )
                return false;
            /*
            IInputLayer inputSrc = algorithm.InputReader;
            if (!(inputSrc is BufferedInputLayer))
                return false;
            if (!((BufferedInputLayer)inputSrc).IsLabeled)
                return false;
            */
            return true;
        }

        public IDrawer Drawer
        {
            get { return m_Drawer; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                m_Drawer = value;
            }
        }
    }
}


