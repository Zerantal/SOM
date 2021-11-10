using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace MathLib.Evolution
{
    [ContractClassFor(typeof(IFitnessSelector<>))]
    public abstract class IFitnessSelectorContract<T> : IFitnessSelector<T> where T : class
    {
        #region IFitnessSelector<T> Members

        public IList<T> SelectParents(IList<Tuple<T, double>> populationFitness, int parentsToSelect)
        {
            // Contract.Requires(populationFitness != null);
            // Contract.Requires(populationFitness.Count > 0);
            // Contract.Requires(parentsToSelect > 0);
            // Contract.Requires(// Contract.ForAll(populationFitness, obj => obj != null));
            // Contract.Requires(// Contract.ForAll(populationFitness, obj => obj.Item1 != null));
            // Contract.Ensures(// Contract.Result<IList<T>>() != null);            
            // Contract.Ensures(// Contract.ForAll(// Contract.Result<IList<T>>(),
                                             //obj => obj != null));
            // Contract.Ensures(// Contract.OldValue(populationFitness.Count) == populationFitness.Count);            

            throw new NotImplementedException();
        }

        #endregion
    }
}
