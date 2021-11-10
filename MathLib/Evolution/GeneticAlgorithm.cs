using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLib.Evolution
{
    public enum CrossoverMethod
    {
        SinglePoint,
        TwoPoint,
        Uniform
    } ;

    public static class GeneticAlgorithm
    {        
        public static double MutationRate { get; set; }
        public static CrossoverMethod CrossoverType { get; set; }

        static GeneticAlgorithm()
        {
            MutationRate = 0.05;
            CrossoverType = CrossoverMethod.SinglePoint;
        }
    }
}
