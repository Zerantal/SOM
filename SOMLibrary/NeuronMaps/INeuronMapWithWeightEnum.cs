using System.Collections.Generic;

namespace SomLibrary.NeuronMaps
{
    /// <summary>
    /// Ordered weight enumerator. returns an array of points in a span
    /// of neurons, and continues for all spans in map.
    /// </summary>
    public interface INeuronMapWithWeightEnum : INeuronMap
    {       
        IEnumerable<Point[]> WeightEnum {get;}
    }
}
