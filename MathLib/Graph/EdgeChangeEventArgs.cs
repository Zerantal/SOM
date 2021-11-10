using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLib.Graph
{
    public class EdgeChangeEventArgs : EventArgs
    {
        public int EdgeId { get; private set; }

        public EdgeChangeEventArgs(int Id)
        {
            EdgeId = Id;
        }

    }
}
