using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace MathLib.Evolution
{
    [ContractClass(typeof(IFitnessSelectorContract<>))]
    public interface IFitnessSelector<T> where T : class
    {
        IList<T> SelectParents(IList<Tuple<T, double>> populationFitness, int parentsToSelect);
    }
}
