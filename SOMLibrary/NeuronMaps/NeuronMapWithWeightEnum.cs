using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;

namespace SomLibrary.NeuronMaps
{
    /// <summary>
    /// Ordered weight enumerator. returns an array of points in a span
    /// of neurons, and continues for all spans in map.
    /// </summary>
    [Serializable]
    public abstract class NeuronMapWithWeightEnum : INeuronMap
    {
        public NeuronMapWithWeightEnum(int inputDim, int mapSize)
            : base(inputDim, mapSize)
        {
            Contract.Requires(mapSize > 0);
            Contract.Requires(inputDim > 0);
        }
        
        public abstract IEnumerable<Point[]> WeightEnum {get;}
    }
}
