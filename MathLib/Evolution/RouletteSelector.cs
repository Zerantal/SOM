using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace MathLib.Evolution
{
    public class RouletteSelector<T> : IFitnessSelector<T> where T : class
    {
        #region IFitnessSelector<T> Members

        public IList<T> SelectParents(IList<Tuple<T, double>> populationFitness, int parentsToSelect)
        {
            List<T> parents = new List<T>();

            T parent = populationFitness.First().Item1;
            for (int i = 0; i < parentsToSelect; i++)
            {                
                double runningFitnessTotal = 0;                

                double r = StaticRandom.NextDouble();
                for (int j = 0; j < populationFitness.Count(); j++)
                {
                    runningFitnessTotal += populationFitness[j].Item2;
                    if (runningFitnessTotal < r) continue;
                    parent = populationFitness[j].Item1;                        
                    break;
                }                
                
                parents.Add(parent);
            }
            return parents;
        }

        #endregion
    }
}
