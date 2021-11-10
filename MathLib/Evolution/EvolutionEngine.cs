#region

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using Util;

#endregion

namespace MathLib.Evolution
{   
    public sealed class EvolutionEngine<T> where T : class, IEvolvableObject<T>
    {
        private readonly int _offspringPerGeneration;
        private List<T> _currentPopulation;
        private bool _evolutionStopped = true;
        private int _elitismSelection;
        private IFitnessSelector<T> _fitnessSelector;

        public EvolutionEngine(IList<T> initialPopulation, int offspringPerGeneration)
        {
            // Contract.Requires(initialPopulation != null);
            // Contract.Requires(initialPopulation.Count > 0);
            // Contract.Requires(// Contract.ForAll(initialPopulation, obj => obj != null));
            // Contract.Requires(offspringPerGeneration >= 1);                        
            
            _currentPopulation = new List<T> (initialPopulation);
            _offspringPerGeneration = offspringPerGeneration;
            _fitnessSelector = new RouletteSelector<T>();
            // Contract.Assume(initialPopulation[0] != null);
            BestSolution = initialPopulation[0];
            // Contract.Assume(// Contract.ForAll(_currentPopulation, obj => obj != null));
        }

        private T _bestSolution;
        public T BestSolution
        {
            get
            {
                // Contract.Ensures(// Contract.Result<T>() != null);
                return _bestSolution;
            }
            private set
            {
                // Contract.Requires(value != null); 
                _bestSolution = value;
            }
        }

        private int _currentGeneration;
        public int CurrentGeneration
        {
            get
            {
                // Contract.Ensures(// Contract.Result<int>() >= 0); 
                return _currentGeneration; 
            }
        }

        public int ElitismSelection
        {
            get
            {
                // Contract.Ensures(// Contract.Result<int>() >= 0 && // Contract.Result<int>() <= OffspringPerGeneration);
                return _elitismSelection; 
            }
            set
            {
                // Contract.Requires(value >= 0);
                if (value > OffspringPerGeneration)
                    _elitismSelection = OffspringPerGeneration;
                else
                    _elitismSelection = value;                
            }
        }

        public IFitnessSelector<T> FitnessSelector
        {
            get
            {
                // Contract.Ensures(// Contract.Result<IFitnessSelector<T>>() != null);
                return _fitnessSelector;
            }
            set
            {
                // Contract.Requires(value != null);

                _fitnessSelector = value;
            }
        }

        public int OffspringPerGeneration
        {
            get
            {
                // Contract.Ensures(// Contract.Result<int>() >= 1);
                return _offspringPerGeneration; 
            }
        }

        public event EventHandler NewGenerationCreated;

        public void Evolve(int numGeneration)
        {
            // Contract.Requires(numGeneration > 0);

            _evolutionStopped = false;
            int generationCount = 0;

            while (generationCount < numGeneration && !_evolutionStopped)
            {
                List<Tuple<T, double>> populationFitness = EvaluatePopulationFitness();

                CreateNextGeneration(populationFitness);

                generationCount++;
                _currentGeneration++;                
                OnNewGenerationCreated(new EventArgs());
            }
        }      


        private void CreateNextGeneration(List<Tuple<T, double>> populationFitness)
        {
            // Contract.Requires(populationFitness != null);
            // Contract.Requires(populationFitness.Count > 0);

            T child;
            List<T> nextGeneration = new List<T>();           

            // generate next generation
            List<T> parents1 = new List<T>(_fitnessSelector.SelectParents(populationFitness, OffspringPerGeneration));
            List<T> parents2 = new List<T>(_fitnessSelector.SelectParents(populationFitness, OffspringPerGeneration));
                        
            // Contract.Assert(parents1.Count() == OffspringPerGeneration);
            // Contract.Assert(parents2.Count() == OffspringPerGeneration);
            // Contract.Assert(parents1.Count() == parents2.Count());
            // Contract.Assert(// Contract.ForAll(parents2, obj => obj != null));

            for (int i = 0; i < OffspringPerGeneration; i ++)
            {
                // Contract.Assert(i < parents2.Count());    
                child = parents1[i].RecombineWith(parents2[i]);
                child.Mutate();
                nextGeneration.Add(child);
            }
                       
            if (ElitismSelection != 0)
            {
                int startIdx = populationFitness.Count() - ElitismSelection;
                int selectionCount = ElitismSelection;
                if (startIdx < 0)
                {
                    startIdx = 0;
                    selectionCount = populationFitness.Count();
                }
                nextGeneration.AddRange(populationFitness.GetRange(startIdx, selectionCount).Select(t => t.Item1));
            }
            _currentPopulation = nextGeneration;
            // Contract.Assume(// Contract.ForAll(_currentPopulation, obj => obj != null));
        }

        private List<Tuple<T, double>> EvaluatePopulationFitness()
        {
            // Contract.Ensures(// Contract.Result<List<Tuple<T, double>>>().Count == _currentPopulation.Count);

            // return all fitness evaluations normalised such that all fitness are greater than zero
            // the aggregate fitness is equal to 1. Sort in increasing fitness as well
            Dictionary<T, Task<double>> taskDictionary = new Dictionary<T, Task<double>>();

            // start all fitness evaluation concurrently and store them in task dictionary
            foreach(T solution in _currentPopulation)
            {
                T solution1 = solution;
                taskDictionary.Add(solution1, Task.Factory.StartNew(() => solution1.Fitness()));
            }

            // acquire results on all fitness evaluations
            List<Tuple<T, double>> fitnessScores = new List<Tuple<T, double>>();
            T bestSoln = _currentPopulation[0];
            double maxFitness = double.MinValue;
            double minFitness = double.MaxValue;
            foreach (KeyValuePair<T, Task<double>> kvp in taskDictionary)
            {
                // Contract.Assume(kvp.Value != null);
                double fitness = kvp.Value.Result;
                fitnessScores.Add(new Tuple<T, double>(kvp.Key, fitness));
                if (fitness > maxFitness)
                {
                    maxFitness = fitness;
                    bestSoln = kvp.Key;
                }
                if (fitness < minFitness) minFitness = fitness;
            }
            // Contract.Assume(bestSoln != null);  // should be able to be proven?
            BestSolution = bestSoln;
            minFitness += Constants.Epsilon; // ensure that lowest score is non zero

            // sort in increasing order
            fitnessScores.Sort((obj1, obj2) => obj1.Item2.CompareTo(obj2.Item2)); 

            // subtract minimum fitness from all
            fitnessScores = fitnessScores.Select(obj => new Tuple<T, double>(obj.Item1, obj.Item2 - minFitness)).ToList();

            double aggregateFitness = fitnessScores.Sum(t => t.Item2);
            if (aggregateFitness.IsEqualTo(0)) aggregateFitness += 1; // ensure it isn't zero

            // normalise list
            fitnessScores =
                fitnessScores.Select(obj => new Tuple<T, double>(obj.Item1, obj.Item2/aggregateFitness)).ToList();

            // return normalised result
            return fitnessScores; 
        }

        private void OnNewGenerationCreated(EventArgs e)
        {
            // Contract.Requires(e != null);

            EventHandler handler = NewGenerationCreated;

            if (handler != null)
            {
                handler(this, e);
            }            
        }

        [ContractInvariantMethod]
// ReSharper disable UnusedMember.Local
        private void ObjectInvariant()
// ReSharper restore UnusedMember.Local
        {
            // Contract.Invariant(_currentPopulation != null);
            // Contract.Invariant(_currentPopulation.Count > 0);
            // Contract.Invariant(// Contract.ForAll(_currentPopulation, obj => obj != null));
            // Contract.Invariant(_offspringPerGeneration >= 1);
            // Contract.Invariant(_elitismSelection >= 0 && _elitismSelection <= _offspringPerGeneration);
            // Contract.Invariant(_fitnessSelector != null);
            // Contract.Invariant(_bestSolution != null);
            // Contract.Invariant(_currentGeneration >= 0);
        }
    }
}