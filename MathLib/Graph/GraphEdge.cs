using System.Diagnostics.Contracts;
using System;

namespace MathLib.Graph
{
    public struct GraphEdge : IEquatable<GraphEdge>
    {
        private int _firstVertex;
        public int FirstVertex
        {
            get { return _firstVertex; }
            private set { _firstVertex = value; }
        }

        private int _secondVertex;
        public int SecondVertex
        {
            get { return _secondVertex; }
            private set { _secondVertex = value; }
        }
        public GraphEdge(int firstVertex, int secondVertex)
        {
            _firstVertex = firstVertex;
            _secondVertex = secondVertex;
        }

        #region IEquatable<GraphEdge> Members

        public bool Equals(GraphEdge other)
        {
            return (_firstVertex == other.FirstVertex && _secondVertex == other.SecondVertex);            
        }

        #endregion
    }
}
