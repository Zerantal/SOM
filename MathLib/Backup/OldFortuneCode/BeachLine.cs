using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

using MathLib.Matrices;
using MathLib;

namespace MathLib.Graph
{
    #region Beachline declaration
    internal class BeachLine
    {
        LinkedList<Breakpoint> _breakpoints;  // list of all breakpoints that occur during sweeping
        LinkedList<Arc> _arcs;                // list of current arcs in beachline (ordered from smallest x value to largest x value)               

        // sweep position has to be at least ahead of all arc vectors
        internal double minSweepPosition;

        internal BeachLine()
        {
            _breakpoints = new LinkedList<Breakpoint>();
            _arcs = new LinkedList<Arc>();
            minSweepPosition = double.MinValue;
        }

        internal Arc FindClosestArc(Vector sweepEventVector)
        {
            Contract.Requires(sweepEventVector.Length == 2);
            Contract.Requires(sweepEventVector[2] >= minSweepPosition);

            LinkedListNode<Arc> node = FindClosestArcNode(sweepEventVector);

            if (node == null)
                return null;
            else
                return node.Value;
        }

        private LinkedListNode<Arc> FindClosestArcNode(Vector sweepEventVector)
        {
            Contract.Requires(sweepEventVector.Length == 2);

            if (_arcs.Count == 0)
                return null;
            if (_arcs.Count == 1)
                return _arcs.First;


            double rightBPValue;    // x coordinate of breakpoint to immediate right of current arc            
            bool closestArcFound = false;
            LinkedListNode<Arc> a = _arcs.First;

            while (!closestArcFound)
            {
                rightBPValue = a.Value.RightBreakpoint.CalcBreakpoint(sweepEventVector[2]);
                if (sweepEventVector[1] > rightBPValue && a.Next != null)
                    a = a.Next;
                else
                    closestArcFound = true;
            }

            return a;    
        }

        internal Arc AddArc(Vector siteVector)
        {
            Contract.Requires(siteVector.Length == 2);

            if (siteVector[2] >= minSweepPosition)
                minSweepPosition = siteVector[2];

            LinkedListNode<Arc> arcToBisect = FindClosestArcNode(siteVector);
            Arc newArc;

            if (arcToBisect == null)            
            {
                newArc = new Arc(siteVector);                
                _arcs.AddFirst(newArc);           
            }
            else
            {
                newArc = new Arc(siteVector, arcToBisect.Value);
                _breakpoints.AddLast(newArc.LeftBreakpoint);
                _breakpoints.AddLast(newArc.RightBreakpoint);
                if (arcToBisect.Value.Site[1] < siteVector[1])     // new arc is to the right of bisecting arc
                {
                    _arcs.AddAfter(arcToBisect, newArc);
                    arcToBisect.Value.RightBreakpoint = newArc.LeftBreakpoint;
                }
                else
                {
                    _arcs.AddBefore(arcToBisect, newArc);
                    arcToBisect.Value.LeftBreakpoint = newArc.RightBreakpoint;
                }
            }
            
            return newArc;
        }

        internal void DeleteArc(Arc a)
        {
            Contract.Requires(a != null);

            Contract.Assert(_arcs.Contains(a));

            LinkedListNode<Arc> arcNode = _arcs.Find(a);
            LinkedListNode<Arc> preArcNode = arcNode.Previous;
            LinkedListNode<Arc> postArcNode = arcNode.Next;            
            Breakpoint newBP;

            Contract.Assert(preArcNode != null);
            Contract.Assert(postArcNode != null);

            newBP = new Breakpoint(preArcNode.Value, postArcNode.Value);
            _breakpoints.AddLast(newBP);
            preArcNode.Value.RightBreakpoint = newBP;
            postArcNode.Value.LeftBreakpoint = newBP;
            
            _arcs.Remove(arcNode);            
        }

        public IEnumerable<Tuple<Arc, Arc, Arc>> ArcTripletEnumerator 
        {
            get
            {
                LinkedListNode<Arc> a1, a2, a3;
                a1 = _arcs.First;
                if (a1 != null)
                {
                    a2 = a1.Next;
                    if (a2 != null)
                    {
                        a3 = a2.Next;
                        while (a3 != null)
                        {
                            yield return new Tuple<Arc, Arc, Arc>(a1.Value, a2.Value, a3.Value);
                            a1 = a2;
                            a2 = a3;
                            a3 = a2.Next;
                        }
                    }
                }
                
            }
        }

        [Pure]
        private bool ArcsAreOrdered()
        {
            LinkedListNode<Arc> node = _arcs.First;
            LinkedListNode<Arc> nextNode;            

            if (node != null)
                nextNode = node.Next;
            else return true;

            while (nextNode != null)
            {
                if (node.Value.Site[1] > nextNode.Value.Site[1])
                    return false;
            }

            return true;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(ArcsAreOrdered() == true);
            Contract.Invariant(_arcs != null);
            Contract.Invariant(_breakpoints != null);
        }


    }
    #endregion

    #region Breakpoint declaration
    internal class Breakpoint
    {
        private static int _breakpointCounter = 0;

        internal Arc _left;
        internal Arc _right;
        GraphEdge<object, Vector> _edge;

        private int breakpointID;

        internal Breakpoint(Arc leftArc, Arc rightArc)
        {
            _left = leftArc;
            _right = rightArc;
            _edge = new GraphEdge<object, Vector>();
            breakpointID = ++_breakpointCounter;
        }

        internal void AddVertex(GraphVertex<Vector> v)
        {
            if (_edge.Vertex1 != null)
                _edge.Vertex1 = v;
            else
            {
                Contract.Assert(_edge.Vertex2 != null);
                _edge.Vertex2 = v;
            }
        }

        // calculate x value of breakpoint
        internal double CalcBreakpoint(double sweepPosition)
        {           
            Contract.Requires(sweepPosition >= _left.Site[2] && sweepPosition >= _right.Site[2]); 

            double root1, root2;
            double a, b, c, x1, x2, y1, y2;

            x1 = _left.Site[1]; y1 = _left.Site[2];
            x2 = _right.Site[1]; y2 = _right.Site[2];           

            /* deal with degenerate cases first */
            if (y1.ApproximatelyEquals(sweepPosition) && y2.ApproximatelyEquals(sweepPosition))
            {
                return ((x1 + x2) / 2.0);
            }
            else if (y2.ApproximatelyEquals(sweepPosition))
            {
                return x2;
            }
            else if (y1.ApproximatelyEquals(sweepPosition))
            {
                return x1;
            }

            a = 2 * y2 - 2 * y1;
            b = -4 * x1 * (y2 - sweepPosition) + 4 * x2 * (y1 - sweepPosition);
            c = 2 * (x1 * x1 + y1 * y1 - sweepPosition * sweepPosition) * (y2 - sweepPosition) - 
                2 * (x2 * x2 + y2 * y2 - sweepPosition * sweepPosition) * (y1 - sweepPosition);

            if (a < 0)
            {
                a = -a; b = -b; c = -c;
            }

            if (a.ApproximatelyEquals(0.0))
                return (-c / b);

            root1 = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
            root2 = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);

            if (_left.CalcY(root1, sweepPosition) < _left.CalcY(root2, sweepPosition))
                return root2;
            else
                return root1;          
        }

        internal int BreakpointID { get { return breakpointID; } }

        public override string ToString()
        {
            return "Breakpoint: " + breakpointID.ToString() + 
                   ".  Left Arc: " + _left.ArcID.ToString() + 
                   ".  Right Arc: " + _right.ArcID.ToString();
        }

        [ContractInvariantMethod()]
        private void ObjectInvariant()
        {
            Contract.Invariant(_left != null);
            Contract.Invariant(_right != null);
            Contract.Invariant(_edge != null);
        }
    }   

    #endregion

    #region Arc Declaration
    internal class Arc
    {
        static int _arcCounter = 0;
        Breakpoint _left;
        Breakpoint _right;
        Vector _site;

        private int arcID;

        internal Arc(Vector v, Arc bisectingArc)
        {
            _site = v;
            _left = new Breakpoint(bisectingArc, this);
            _right = new Breakpoint(this, bisectingArc);

            arcID = ++_arcCounter;
        }

        // used for the initial arc in Beachline
        internal Arc(Vector v)
        {
            _site = v;
            _left = null;
            _right = null;

            arcID = ++_arcCounter;
        }

        internal Vector Site { get { return _site; } }

        internal Breakpoint LeftBreakpoint
        {
            get
            {
                return _left;
            }

            set
            {
                _left = value;
            }
        }
        internal Breakpoint RightBreakpoint
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;
            }
        }

        internal double CalcY(double x, double sweepPosition)
        {
            double c2, c1, c0;           

            c2 = 1 / (2 * (_site[2] - sweepPosition));
            c1 = -2 * _site[1] / (2 * (_site[2] - sweepPosition));
            c0 = ((Math.Pow(_site[1], 2.0) + Math.Pow(_site[2], 2.0) - Math.Pow(sweepPosition, 2.0)) / (2 * (_site[2] - sweepPosition)));

            return (c2 * Math.Pow(x, 2.0) + c1 * x + c0);
        }

        internal int ArcID { get { return arcID; } }

        public override string ToString()
        {
            string leftBP;
            string rightBP;

            if (_left == null)
                leftBP = "null";
            else
                leftBP = _left.BreakpointID.ToString();
            if (_right == null)
                rightBP = "null";
            else
                rightBP = _right.BreakpointID.ToString();

            return "Arc: " + arcID.ToString() +
                   ".  Site Vector: " + _site.ToString() +
                   ".  Left BP: " + leftBP +
                   ".  Right BP: " + rightBP;

        }


        [ContractInvariantMethod()]
        private void ObjectInvariant()
        {
            Contract.Invariant(_site != null);
        }

    #endregion
    }
}
