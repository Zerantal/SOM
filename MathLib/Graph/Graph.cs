using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Util;

namespace MathLib.Graph
{
    public class Graph : IDeepCloneable<Graph>
    {
        protected int VertexIdCounter;
        protected int EdgeIdCounter { get; set; }

        private readonly HashSet<int> _vertices;
        private readonly Dictionary<int, GraphEdge> _edges;

        public event EventHandler<VertexChangeEventArgs> VertexAddedEvent;
        public event EventHandler<VertexChangeEventArgs> VertexRemovedEvent;
        public event EventHandler<EdgeChangeEventArgs> EdgeAddedEvent;
        public event EventHandler<EdgeChangeEventArgs> EdgeRemovedEvent;

        protected Graph()
        {

            _vertices = new HashSet<int>();
            _edges = new Dictionary<int, GraphEdge>();
        }

        public int AddVertex()
        {
            while (_vertices.Contains(VertexIdCounter))
                VertexIdCounter++;

            int newVertexId = VertexIdCounter++;
            _vertices.Add(newVertexId);

            OnVertexAddedEvent(new VertexChangeEventArgs(newVertexId));

            return newVertexId;
        }

        public void AddVertex(int vertexId)
        {            
            if (_vertices.Contains(vertexId))
                throw new ArgumentException("A Vertex with the specified label already exists in graph.");

            _vertices.Add(vertexId);

            OnVertexAddedEvent(new VertexChangeEventArgs(vertexId));
        }

        public bool TryAddVertex(int vertexId)
        {
            if (_vertices.Contains(vertexId))
                return false;

            _vertices.Add(vertexId);

            OnVertexAddedEvent(new VertexChangeEventArgs(vertexId));

            return true;
        }

        public bool RemoveVertex(int vertexId)
        {
            if (!_vertices.Contains(vertexId))
                return false;

            _vertices.Remove(vertexId);

            // remove all edges to and from vertex
            List<int> edgesToRemove =
                _edges.Where(e => (e.Value.FirstVertex == vertexId || e.Value.SecondVertex == vertexId)).Select(
                    kvp => kvp.Key).ToList();
            foreach (int e in edgesToRemove)
            {
                _edges.Remove(e);
                OnEdgeRemovedEvent(new EdgeChangeEventArgs(e));
            }

            OnVertexRemoveEvent(new VertexChangeEventArgs(vertexId));

            return true;
        }

        public int AddEdge(GraphEdge edge)
        {
            // Contract.Requires(ContainsVertex(edge.FirstVertex));
            // Contract.Requires(ContainsVertex(edge.SecondVertex));                        

            while (_edges.ContainsKey(EdgeIdCounter))
                EdgeIdCounter++;

            int newEdgeId = EdgeIdCounter++;
            _edges.Add(newEdgeId, edge);
           
            OnEdgeAddedEvent(new EdgeChangeEventArgs(newEdgeId));

            return newEdgeId;
        }

        public void AddEdge(int edgeId, GraphEdge edge)
        {
            // Contract.Requires(!ContainsEdge(edgeId));
            // Contract.Requires(ContainsVertex(edge.FirstVertex));
            // Contract.Requires(ContainsVertex(edge.SecondVertex));            

            int newEdgeId = EdgeIdCounter++;
            _edges.Add(newEdgeId, edge);

            OnEdgeAddedEvent(new EdgeChangeEventArgs(newEdgeId));            
        }

        public bool RemoveEdge(int edgeId)
        {
            if (!_edges.ContainsKey(edgeId))
                return false;

            _edges.Remove(edgeId);

            OnEdgeRemovedEvent(new EdgeChangeEventArgs(edgeId));

            return true;
        }

        public bool TryAddEdge(int edgeId, GraphEdge edge)
        {
            if (ContainsEdge(edgeId))
                return false;
            if (!ContainsVertex(edge.FirstVertex))
                return false;
            if (!ContainsVertex(edge.SecondVertex))
                return false;

            AddEdge(edgeId, edge);            

            return true;
        }

        public bool TryAddEdge(GraphEdge edge)
        {
            int edgeId;
            return TryAddEdge(edge, out edgeId);
        }

        public bool TryAddEdge(GraphEdge edge, out int edgeId)
        {
            if (!ContainsVertex(edge.FirstVertex))
            {
                edgeId = 0;
                return false;                
            }
            if (!ContainsVertex(edge.SecondVertex))
            {
                edgeId = 0;
                return false;
            }

            edgeId = AddEdge(edge);

            return true;
        }

        public GraphEdge GetEdge(int edgeId)
        {
            // Contract.Requires(ContainsEdge(edgeId));
            
            return _edges[edgeId];
        }

        protected void OnVertexAddedEvent(VertexChangeEventArgs e)
        {
            EventHandler<VertexChangeEventArgs> handler = VertexAddedEvent;

            if (handler != null)
                handler(this, e);            
        }

        protected void OnVertexRemoveEvent(VertexChangeEventArgs e)
        {
            EventHandler<VertexChangeEventArgs> handler = VertexRemovedEvent;

            if (handler != null)
                handler(this, e);
        }

        protected void OnEdgeAddedEvent(EdgeChangeEventArgs e)
        {
            EventHandler<EdgeChangeEventArgs> handler = EdgeAddedEvent;

            if (handler != null)
                handler(this, e);
        }

        protected void OnEdgeRemovedEvent(EdgeChangeEventArgs e)
        {
            EventHandler<EdgeChangeEventArgs> handler = EdgeRemovedEvent;

            if (handler != null)
                handler(this, e);
        }
        
        public ICollection<int> GetVertexList()
        {
            // Contract.Ensures(// Contract.Result<ICollection<int>>() != null);
            return new List<int>(_vertices.ToList());
        }

        public ICollection<int> GetEdgeList()
        {
            // Contract.Ensures(// Contract.Result<ICollection<int>>() != null);

            return new List<int>(_edges.Keys);
        }

        public int EdgeCount { get { return _edges.Count(); } }
        public int VertexCount { get { return _vertices.Count(); } }
        
        [Pure]
        public bool ContainsEdge(int edgeId)
        {
            return _edges.ContainsKey(edgeId);
        }

        [Pure]
        public bool ContainsVertex(int vertexId)
        {
            return _vertices.Contains(vertexId);
        }

        #region Implementation of IDeepCloneable<out Graph>

        public Graph DeepClone()
        {
            throw new NotImplementedException();
        }
        
        #endregion

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            if (_edges.Count() == 0)
                return "{}";

            str.Append("{ ");

            // write edge set
            foreach (KeyValuePair<int, GraphEdge> kvp in _edges)            
                str.Append(kvp.Value.FirstVertex + "->" + kvp.Value.SecondVertex + ", ");

            // Contract.Assume(str.Length >= 2);
            str.Remove(str.Length - 2, 2); // remove last comma
            str.Append(" }");

            return str.ToString();            
        }             

        public string ToStringWithVertexSet()
        {
            StringBuilder str = new StringBuilder();

            if (_vertices.Count == 0)
                str.Append("{}, ");
            else
            {                
                str.Append("{ ");
                // write vertex set
                foreach (int v in _vertices)
                    str.Append(v + ", ");

                // Contract.Assume(str.Length >= 2);
                str.Remove(str.Length - 2, 2);
                str.Append(" }, ");
            }

            if (_edges.Count() == 0)
                str.Append("{}");
            else
            {
                str.Append("{ ");

                // write edge set
                foreach (KeyValuePair<int, GraphEdge> kvp in _edges)
                    str.Append(kvp.Value.FirstVertex + "->" + kvp.Value.SecondVertex + ", ");

                // Contract.Assume(str.Length >= 2);
                str.Remove(str.Length - 2, 2); // remove last comma
                str.Append(" }");                
            }               

            return str.ToString();    
        }

        [ContractInvariantMethod]
// ReSharper disable UnusedMember.Local
        private void ObjectInvariant()
// ReSharper restore UnusedMember.Local
        {
            // Contract.Invariant(_vertices != null);
            // Contract.Invariant(_edges != null);
        }
    }
}
