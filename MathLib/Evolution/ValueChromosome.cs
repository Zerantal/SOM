using System.Diagnostics.Contracts;

using Util;

namespace MathLib.Evolution
{
    public class ValueChromosome<T> : FixedLengthChromosome<T, ValueChromosome<T>> 
        where T : class,  IMutator, IDeepCloneable<T>
    {
        public ValueChromosome(T[] initialValues) : base(initialValues)
        {
            // Contract.Requires(initialValues != null);
            // Contract.Requires(initialValues.Length > 0);
            // Contract.Ensures(Length == initialValues.Length);
        }

        #region Overrides of FixedLengthChromosome<T>

        public override void Mutate()
        {
            for (var geneIdx = 0; geneIdx < Length; geneIdx++)
            {
                if (StaticRandom.NextDouble() < GeneticAlgorithm.MutationRate)
                    this[geneIdx].Mutate();                    
            }
        }

        protected override ValueChromosome<T> CreateChromosome(T[] geneData)
        {
            return new ValueChromosome<T>(geneData);            
        }

        public override ValueChromosome<T> Crossover(ValueChromosome<T> extraChromosome)
        {
            // Contract.Assume(extraChromosome.Length == Length);

            var childGenes = new T[extraChromosome.Length];

            int pt1, pt2;   // crossover points            

            switch (GeneticAlgorithm.CrossoverType)
            {
                case CrossoverMethod.SinglePoint:
                    pt1 = StaticRandom.Next(Length);
                    for (var i = 0; i < pt1; i++)
                        childGenes[i] = this[i].DeepClone();
                    for (var i = pt1; i < Length; i++)
                        childGenes[i] = extraChromosome[i].DeepClone();
                    break;
                case CrossoverMethod.TwoPoint:
                    pt1 = StaticRandom.Next(Length);
                    pt2 = StaticRandom.Next(Length);
                    if (pt2 < pt1)  // swap values
                    {
                        var tmp = pt1;
                        pt1 = pt2;
                        pt2 = tmp;
                    }
                    for (var i = 0; i < pt1; i++)
                        childGenes[i] = this[i].DeepClone();
                    for (var i = pt1; i < pt2; i++)
                        childGenes[i] = extraChromosome[i].DeepClone();
                    for (var i = pt2; i < Length; i++)
                        childGenes[i] = this[i].DeepClone();
                    break;
                case CrossoverMethod.Uniform:
                    for (var i = 0; i < Length; i++)
                        if (StaticRandom.NextDouble() < 0.5)
                            childGenes[i] = extraChromosome[i].DeepClone();
                        else
                            childGenes[i] = this[i].DeepClone();
                    break;
            }

            return CreateChromosome(childGenes);
        }

        public override ValueChromosome<T> DeepClone()
        {
            var clonedGenes = new T[Length];
            for (var i = 0; i < Length; i++)
                clonedGenes[i] = this[i].DeepClone();
           
            return new ValueChromosome<T>(clonedGenes);            
        }

        #endregion
    }
}
