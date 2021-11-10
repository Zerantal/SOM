using System;
using MathLib.Matrices;

namespace SomLibrary
{   
    static public class MapInitialiser
    {
        static public void Random(INeuronMap map)
        {            
            Random r = new Random();
            int vecSize = map.InputDimension;

            for (int neuronIdx = 0; neuronIdx < map.MapSize; neuronIdx++)
            {
                Vector randVec = new Vector(vecSize);
                
                for (int i = 0; i < vecSize; i++)
                    randVec[i] = r.NextDouble();

                map[neuronIdx] = randVec;
            }
        }      
    }
}
