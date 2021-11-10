using System.Diagnostics.Contracts;

namespace MathLib.Evolution
{
    [ContractClass(typeof(GaObjectContract<,>))]
    public abstract class GaObject<TObject, TChromosome> : IEvolvableObject<TObject>
        where TObject : GaObject<TObject, TChromosome>
        where TChromosome : class, IChromosome<TChromosome>
    {
        private TChromosome _chromosome;

        protected TChromosome Chromosome
        {
            get
            {
                // Contract.Ensures(// Contract.Result<TChromosome>() != null);
                return _chromosome; 
            }
            set
            {
                // Contract.Requires(value != null);
                _chromosome = value; 
            }
        }

        protected abstract TObject CreateObject();        

        #region IEvolvableObject<GAObject<Genome>> Members

        public TObject RecombineWith(TObject additionalParent)
        {
            TObject child = CreateObject();

            child._chromosome = _chromosome.Crossover(additionalParent.Chromosome);

            return child;            
        }        

        public void Mutate()
        {
            _chromosome.Mutate();            
        }

        public abstract double Fitness();
        #endregion

        [ContractInvariantMethod]
        // ReSharper disable UnusedMember.Local
        private void ObjectInvariant()
        // ReSharper restore UnusedMember.Local
        {
            // Contract.Invariant(_chromosome != null);
        }
    }
}
