using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace MathLib.Evolution
{
    [ContractClassFor(typeof(IChromosome<>))]
    abstract class IChromosomeContract<T> : IChromosome<T> where T: class, IChromosome<T>
    {
        #region IChromosome Members

        public T Crossover(T extraChromosome)
        {
            // Contract.Requires(extraChromosome != null);
            // Contract.Ensures(// Contract.Result<T>() != null);                     
            throw new NotImplementedException();
        }        

        public void Mutate()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDeepCloneable<IChromosome> Members

        public T DeepClone()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
