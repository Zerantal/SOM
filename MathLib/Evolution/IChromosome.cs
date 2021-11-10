using System.Diagnostics.Contracts;

namespace MathLib.Evolution
{
    [ContractClass(typeof(IChromosomeContract<>))]
    public interface IChromosome<TChrom> where TChrom : class, IChromosome<TChrom>
    {
        TChrom Crossover(TChrom extraChromosome);

        void Mutate();
    }
}
