using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using SomLibrary.NeuronMaps;

namespace SomLibrary.Visualisers
{
    //[SOMPluginDetail("Voronoi Region Visualiser", "Plots the voronoi regions of the map vectors")]
    public class VoronoiRegionsVisualiser : IVisualiser
    {
        private string _errMsg;

        #region IVisualiser Members

        public void VisualiseMap(ISOM algorithm, IDrawer drawer)
        {            
            if (!CanVisualiseMap(algorithm))
                throw new InvalidAlgorithmException(_errMsg);         
        }

        public bool CanVisualiseMap(ISOM algorithm)
        {
            INeuronMap map = algorithm.Map;
            if (map == null)
            {
                _errMsg = "Algorithms map is not set.";
                return false;
            }

            if (!(map is INeuronMapWithWeightEnum))
            {
                _errMsg = "Algorithms map must be derived from NeuronMapWeightEnum.";
                return false;
            }

            if (map.InputDimension != 2)
            {
                _errMsg = "Map must have an input dimension of 2.";
                return false;
            }

            return true;
        }        

        #endregion
    }
}
