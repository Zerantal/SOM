using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;

using MathLib.Statistics;
using MathLib.Graph;
using Util;

namespace MathLib.Evolution
{
    public class GraphChromosome<TGraph> : IChromosome<GraphChromosome<TGraph>>
        where TGraph : Graph.Graph, IMutator, new()
    {
        static private readonly NormalRandomGenerator Random;
        private static List<int> _vertexPool;           // available vertices to add from
        private static List<int> _immortalVertices;      // vertices that aren't subject to deletion

        static GraphChromosome()
        {
            Random = new NormalRandomGenerator();
        }

        public GraphChromosome(TGraph genes)
        {
            // Contract.Requires(genes != null);

            ChromosomalGraph = genes;

            VertexAdditionRate = 1;
            VertexDeletionRate = 1;
            EdgeAdditionRate = 1;
            EdgeDeletionRate = 1;
        }

        public TGraph ChromosomalGraph { get; private set; }

        static public double VertexAdditionRate { get; set; }
        static public double VertexDeletionRate { get; set; }
        static public double EdgeAdditionRate { get; set; }
        static public double EdgeDeletionRate { get; set; }
        public static List<int> VertexPool
        {
            set
            {
                if (value == null)
                    _vertexPool = null;
                else
                    _vertexPool = new List<int>(value);
            }
        }
        public static List<int> ImmortalVertices
        {
            set
            {
                if (value == null)
                    _immortalVertices = null;
                else
                    _immortalVertices = new List<int>(value);
            }
        }

        #region Implementation of IDeepCloneable<out IChromosome>

        public GraphChromosome<TGraph> DeepClone()
        {
            return new GraphChromosome<TGraph>((TGraph)ChromosomalGraph.DeepClone());
        }

        #endregion

        #region Implementation of IChromosome

        public GraphChromosome<TGraph> Crossover(GraphChromosome<TGraph> extraChromosome)
        {            
            TGraph extraGraph = extraChromosome.ChromosomalGraph;                                    

            switch (GeneticAlgorithm.CrossoverType)
            {
                case CrossoverMethod.SinglePoint:
                    return SinglePointCrossover(extraGraph);                    
                case CrossoverMethod.TwoPoint:
                    return TwoPointCrossover(extraGraph);
                default:                
                    return UniformCrossover(extraGraph);                    
            }           
        }

        private GraphChromosome<TGraph> UniformCrossover(TGraph extraGraph)
        {
            TGraph newGraph = new TGraph();

            SortedSet<int> vertexList1 = new SortedSet<int>(ChromosomalGraph.GetVertexList());
            SortedSet<int> vertexList2 = new SortedSet<int>(extraGraph.GetVertexList());
            SortedSet<int> edgeList1 = new SortedSet<int>(ChromosomalGraph.GetEdgeList());
            SortedSet<int> edgeList2 = new SortedSet<int>(extraGraph.GetEdgeList());

            // add vertices            
            foreach (int v in vertexList1.Union(vertexList2))
            {
                if (StaticRandom.NextDouble() < 0.5) // add from vertexList1
                {
                    if (vertexList1.Contains(v))
                        newGraph.AddVertex(v);
                }
                else //add from vertexList2
                {
                    if (vertexList2.Contains(v))
                        newGraph.AddVertex(v);
                }
            }

            // Add edges
            foreach (int edgeId in edgeList1.Union(edgeList2))
            {
                if (StaticRandom.NextDouble() < 0.5) // add from edgeList1
                {
                    if (ChromosomalGraph.ContainsEdge(edgeId))
                        newGraph.TryAddEdge(edgeId, ChromosomalGraph.GetEdge(edgeId));
                }
                else //add from edgeList2
                {
                    if (extraGraph.ContainsEdge(edgeId))
                        newGraph.TryAddEdge(extraGraph.GetEdge(edgeId));
                }
            }

            return new GraphChromosome<TGraph>(newGraph);
        }

        private GraphChromosome<TGraph> TwoPointCrossover(TGraph extraGraph)
        {
            throw new NotImplementedException();
        }

        private GraphChromosome<TGraph> SinglePointCrossover(TGraph extraGraph)
        {
            TGraph newGraph = new TGraph();
            SortedSet<int> vertexList1 = new SortedSet<int> (ChromosomalGraph.GetVertexList());
            SortedSet<int> vertexList2 = new SortedSet<int> (extraGraph.GetVertexList());
            SortedSet<int> edgeList1 = new SortedSet<int>(ChromosomalGraph.GetEdgeList());
            SortedSet<int> edgeList2 = new SortedSet<int>(extraGraph.GetEdgeList());

            int vertexCrossoverPt = StaticRandom.Next(vertexList1.Count);
            int edgeCrossoverPt = StaticRandom.Next(edgeList1.Count);

                // add vertices
            if (vertexList1.Count > 0)
            {            
                foreach (int v in vertexList1.Take(vertexCrossoverPt))
                    newGraph.AddVertex(v);
                int lastVertexLabel = vertexList1.ElementAt(vertexCrossoverPt);
                foreach (int v in vertexList2.TakeWhile(v => v >= lastVertexLabel))
                    newGraph.AddVertex(v);
            }

            if (edgeList1.Count > 0)
            {                
            // add edges      
                foreach (int edgeId in edgeList1.Take(edgeCrossoverPt))
                    newGraph.TryAddEdge(edgeId, ChromosomalGraph.GetEdge(edgeId));
                int lastEdgeLabel = edgeList1.ElementAt(edgeCrossoverPt);
                foreach (int edgeId in edgeList2.TakeWhile(e => e >= lastEdgeLabel))
                    newGraph.TryAddEdge(edgeId, extraGraph.GetEdge(edgeId));
            }

            return new GraphChromosome<TGraph>(newGraph);
        }

        public bool IsCompatibleWith(GraphChromosome<TGraph> otherChromosome)
        {
            return true;
        }

        public void Mutate()
        {
            // remove random edges
            RemoveRandomEdges();

            // remove random vertices
            RemoveRandomVertices();

            // add random vertices
            AddRandomVertices();

            // add random edges
            AddRandomEdges();

            // mutate graph data
            ChromosomalGraph.Mutate();
        }

        private void AddRandomEdges()
        {
            int edgesToAdd = Convert.ToInt32(Random.Number * EdgeAdditionRate);
            List<int> vertices = new List<int>(ChromosomalGraph.GetVertexList());
            if (edgesToAdd <= 0 || vertices.Count() <= 0) return;

            for (int i = 0; i < edgesToAdd; i++)
            {
                int v1 = vertices[StaticRandom.Next(vertices.Count())];
                int v2 = vertices[StaticRandom.Next(vertices.Count())];
                ChromosomalGraph.TryAddEdge(new GraphEdge(v1, v2));
            }
        }

        private void RemoveRandomVertices()
        {
            int verticesToRemove = Convert.ToInt32(Random.Number * VertexDeletionRate);
            List<int> vertices = new List<int>(ChromosomalGraph.GetVertexList());
            if (verticesToRemove <= 0 || vertices.Count() <= 0) return;

            for (int i = 0; i < verticesToRemove; i++)
            {
                int v = StaticRandom.Next(vertices.Count());
                if (!_immortalVertices.Contains(v))
                    ChromosomalGraph.RemoveVertex(vertices[StaticRandom.Next(vertices.Count())]);
            }
        }

        private void AddRandomVertices()
        {
            int verticesToAdd = Convert.ToInt32(Random.Number * VertexAdditionRate);
            if (verticesToAdd <= 0) return;

            if (_vertexPool != null)
            {
                for (int i = 0; i < verticesToAdd; i++)
                {
                    int vertexId = _vertexPool[StaticRandom.Next(_vertexPool.Count)];
                    ChromosomalGraph.TryAddVertex(vertexId);
                }
            }
            else
            {
                for (int i = 0; i < verticesToAdd; i++)
                    ChromosomalGraph.AddVertex();
            }
        }

        private void RemoveRandomEdges()
        {
            int edgesToRemove = Convert.ToInt32(Random.Number * EdgeDeletionRate);
            List<int> edges = new List<int>(ChromosomalGraph.GetEdgeList());
            if (edgesToRemove <= 0 || edges.Count() <= 0) return;
            
            for (int i = 0; i < edgesToRemove; i++)
            {
                int edgeIdx = StaticRandom.Next(edges.Count);                
                ChromosomalGraph.RemoveEdge(edges[edgeIdx]);
            }
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(Random != null);
            // Contract.Invariant(ChromosomalGraph != null);
        }
    }
}
