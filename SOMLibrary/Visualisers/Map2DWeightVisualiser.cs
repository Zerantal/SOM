using System;
using System.Collections.Generic;
using SomLibrary.NeuronMaps;
using Util;

namespace SomLibrary.Visualisers
{
    [SOMPluginDetail("2D Weight Visualiser", "Plots the weight vector of a 2D map")]
    public class Map2DWeightVisualiser : IVisualiser
    {
        private String _errMsg;

        [ToDo("plot is limited to weight vectors between 0 and 1.")]
        public void VisualiseMap(ISOM algorithm, IDrawer drawer)
        {
            if (!CanVisualiseMap(algorithm))
                throw new InvalidAlgorithmException(_errMsg);

            INeuronMapWithWeightEnum map = algorithm.Map as INeuronMapWithWeightEnum;

            drawer.Clear();
                  
            foreach (Point [] pts in map.WeightEnum)
            {
                if (pts.Length > 1) 
                     Draw.Lines(drawer, pts, 0.005f, unchecked((int)0xFF0000FF));
                Draw.FillCircles(drawer, pts, 0.0075f, unchecked((int)0xFFFF0000));
            } 
        }
        
        public bool CanVisualiseMap(ISOM algorithm)
        {
            INeuronMap map = algorithm.Map;

            if (!(map is INeuronMapWithWeightEnum))
            {
                _errMsg = "Algorithms map doesn't implement INeuronMapWeightEnum.";
                return false;
            }
            
            if (map.InputDimension < 2)
            {
                _errMsg = "Map must have an input dimension of at least 2.";
                return false;
            }
            
            return true;
        }
    }
}
