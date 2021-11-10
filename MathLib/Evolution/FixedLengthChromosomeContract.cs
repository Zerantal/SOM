using System;
using System.Diagnostics.Contracts;

using Util;

namespace MathLib.Evolution
{
    [ContractClassFor(typeof(FixedLengthChromosome<,>))]
    internal abstract class FixedLengthChromosomeContract<T,TChromosome> : FixedLengthChromosome<T,TChromosome>
        where TChromosome : FixedLengthChromosomeContract<T, TChromosome> where T : class, IDeepCloneable<T>
    {
        protected FixedLengthChromosomeContract(T[] initialValues) : base(initialValues)
        {
        }

        public override void Mutate()
        {
            throw new NotImplementedException();
        }

        protected override TChromosome CreateChromosome(T[] geneData)
        {
            // Contract.Requires(geneData != null);            
            // Contract.Requires(geneData.Length > 0);
            // Contract.Ensures(// Contract.Result<TChromosome>() != null);
            // Contract.Ensures(// Contract.Result<TChromosome>().Length == geneData.Length);
            throw new NotImplementedException();
        }

        public override TChromosome DeepClone()
        {
            throw new NotImplementedException();
        }
    }
}
