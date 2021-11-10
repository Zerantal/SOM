using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLib.Graph
{
    public class VertexChangeEventArgs : EventArgs
    {
        public int VertexId { get; private set; }

        public VertexChangeEventArgs(int id)
        {
            VertexId = id;
        }
    }
}
