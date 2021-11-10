using System;
using System.Diagnostics.Contracts;

namespace MathLib.Evolution
{
    [ContractClassFor(typeof(GaObject<,>))]
    abstract class GaObjectContract<TObject, TChromosome> : GaObject<TObject, TChromosome>
        where TObject : GaObject<TObject, TChromosome>
        where TChromosome : class, IChromosome<TChromosome>
    {
        #region Overrides of GaObject<TObject,TChromosome>

        protected override TObject CreateObject()
        {
            // Contract.Ensures(// Contract.Result<TObject>() != null);
            throw new NotImplementedException();
        }

        public override double Fitness()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
