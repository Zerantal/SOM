using System.Diagnostics.Contracts;

using Util;

namespace MathLib.Evolution
{
    public class RealChromosome : IChromosome<RealChromosome>, IDeepCloneable<RealChromosome>
    {
        private readonly double[] _genes;

        public RealChromosome(double[] initialValues)
        {
            // Contract.Requires(initialValues != null);
            // Contract.Requires(initialValues.Length > 0);
            // Contract.Ensures(Length == initialValues.Length);
            MutationStrength = 1;
            _genes = (double[]) initialValues.Clone();
        }

        public RealChromosome(int length)
        {
            // Contract.Requires(length > 0);
            // Contract.Ensures(Length == length);

            MutationStrength = 1;
            _genes = new double[length];
        }

        public int Length
        {
            get
            {
                // Contract.Ensures(// Contract.Result<int>() > 0);
                return _genes.Length;
            }
        }

        public double this[int index]
        {
            get
            {
                // Contract.Requires(index >= 0 && index <Length);
                return _genes[index];
            }

            set
            {
                // Contract.Requires(index >= 0 && index < Length);                
                _genes[index] = value;
            }
        }

        public RealChromosome Crossover(RealChromosome extraChromosome)
        {
            // Contract.Assume(extraChromosome.Length == Length);      // can't be proven

            var childChromosome = new RealChromosome(Length);            

            int pt1, pt2;   // crossover points            

            switch (GeneticAlgorithm.CrossoverType)
            {
                case CrossoverMethod.SinglePoint:
                    pt1 = StaticRandom.Next(Length);
                    for (int i = 0; i < pt1; i++)
                        childChromosome[i] = this[i];
                    for (int i = pt1; i < _genes.Length; i++)
                        childChromosome[i] = extraChromosome[i];
                    break;
                case CrossoverMethod.TwoPoint:
                    pt1 = StaticRandom.Next(_genes.Length);
                    pt2 = StaticRandom.Next(_genes.Length);
                    if (pt2 < pt1)  // swap values
                    {
                        int tmp = pt1;
                        pt1 = pt2;
                        pt2 = tmp;
                    }
                    for (int i = 0; i < pt1; i++)
                        childChromosome._genes[i] = _genes[i];
                    for (int i = pt1; i < pt2; i++)
                        childChromosome[i] = extraChromosome[i];
                    for (int i = pt2; i < _genes.Length; i++)
                        childChromosome._genes[i] = _genes[i];
                    break;
                case CrossoverMethod.Uniform:                    
                    for (int i = 0; i < _genes.Length; i++)
                        if (StaticRandom.NextDouble() < 0.5)
                            childChromosome[i] = extraChromosome[i];
                        else
                            childChromosome._genes[i] = _genes[i];
                    break;
            }

            return childChromosome;            
        }
        
        public void Mutate()
        {
            for (int geneIdx = 0; geneIdx < _genes.Length; geneIdx++)
            {
                if (StaticRandom.NextDouble() < GeneticAlgorithm.MutationRate)
                    _genes[geneIdx] += (StaticRandom.NextDouble() - 0.5)*MutationStrength;
            }             
        }

        public RealChromosome DeepClone()
        {
            var clonedGenes = (double[]) _genes.Clone();

            return new RealChromosome(clonedGenes);
        }

        public double MutationStrength { get; set; }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(_genes != null);
            // Contract.Invariant(_genes.Length > 0);
        }
    }
}
