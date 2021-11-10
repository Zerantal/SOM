using System.Collections.Generic;
using System.Linq;
using MathLib.Matrices;
using SomLibrary.NeuronMaps;

namespace SomLibrary.Visualisers
{
    //[SOMPluginDetail("GSOM Geometry Visualiser", "Plots the neuron positions of all neurons in map")]
    public class GSOMGeometryVisualiser : IVisualiser
    {
        private bool CanVisualiseMap(ISOM algorithm, out string errMsg)
        {
            INeuronMap map = algorithm.Map;

            if (!(map is GrowingRectNeuronMap))
            {
                errMsg = "Algorithm map is not a GrowingRectNeuronMap.";
                return false;
            }
            errMsg = "";

            return true;
        }


        #region IVisualiser Members

        public void VisualiseMap(ISOM algorithm, IDrawer drawer)
        {
            string errMsg;
            if (!CanVisualiseMap(algorithm, out errMsg))
                throw new InvalidAlgorithmException(errMsg);

            GrowingRectNeuronMap map = algorithm.Map as GrowingRectNeuronMap;

            List<Vector> mapVectors = new List<Vector>();

            for (int i = 0; i < map.MapSize; i++ )
                mapVectors.Add(map.NeuronPosition(i));

            Point [] pts = new Point[mapVectors.Count];

            double minX = mapVectors.Min(t => t[0]);
            double minY = mapVectors.Min(t => t[1]);
            double maxX = mapVectors.Max(t => t[0]);
            double maxY = mapVectors.Max(t => t[1]);

            double xRange = maxX - minX;
            double yRange = maxY - minY;

            double range = xRange > yRange ? xRange : yRange;

            drawer.Clear();

            for (int i = 0; i < mapVectors.Count; i++)
            {
                pts[i] = new Point((float)((mapVectors[i][0]-minX)/range), (float)((mapVectors[i][1]-minY)/range));
            }
            
            Draw.FillCircles(drawer, pts, 0.01f, unchecked((int)0xFFFF0000));           
        }

        public bool CanVisualiseMap(ISOM algorithm)
        {
            string errMsg;

            return CanVisualiseMap(algorithm, out errMsg);
        }

        #endregion
    }
}
