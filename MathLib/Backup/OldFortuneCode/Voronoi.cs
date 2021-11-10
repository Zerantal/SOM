using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

using MathLib.Matrices;
using Util;

[assembly: InternalsVisibleTo("VoronoiVisualiser")]

namespace MathLib.Graph
{    
    public static class Voronoi
    {
#if DEBUG
        // A delegate type for hooking into the Fortune algorithm to aid debugging.
        internal delegate void FortuneEventHandler(object sender, FortuneEventArgs e);
        
        internal static event FortuneEventHandler FortuneEvent;

        internal class FortuneEventArgs : EventArgs
        {
            FortuneAlgParams _algParams;

            internal FortuneEventArgs(ref FortuneAlgParams algParameters)
            {
                _algParams = algParameters;
            }

            internal FortuneAlgParams AlgorithmParameters { get { return _algParams; } }
        }
#endif

        internal struct FortuneAlgParams
        {
            internal Graph<Vector, object> VoronoiGraph;
            internal SortedList<VoronoiEvent, object> EventQueue;
            internal BeachLine Beachline;
            // these events should all be maintained within the event queue at all times
            public Dictionary<Arc, List<CircleEvent>> ArcCircleEventRegistry;
            public Dictionary<Tuple<Arc, Arc, Arc>, CircleEvent> ArcTripletEventRegistry;
        }

        public static Graph<Vector, object> VoronoiPolyhedra2D(IEnumerable<Vector> points)
        {
            Contract.Requires<ArgumentNullException>(points != null);
            Contract.Requires<ArgumentException>(Contract.ForAll<Vector>(points, new Predicate<Vector>(v => v.Length == 2)),
                "All vectors passed to method must have dimension of 2.");

            FortuneAlgParams AlgParams;
            AlgParams.VoronoiGraph = new Graph<Vector, object>();
            AlgParams.EventQueue = new SortedList<VoronoiEvent,object>();
            AlgParams.Beachline = new BeachLine();
            AlgParams.ArcCircleEventRegistry = new Dictionary<Arc,List<CircleEvent>>();
            AlgParams.ArcTripletEventRegistry = new Dictionary<Tuple<Arc, Arc, Arc>, CircleEvent>();

            IList<VoronoiEvent> SortedEventList = AlgParams.EventQueue.Keys;
            VoronoiEvent currentEvent;
            

            // load all points as site events in the event queue
            foreach (Vector v in points)
                AlgParams.EventQueue.Add(new SiteEvent(v), null);

            while (SortedEventList.Count() > 0)
            {
                currentEvent = SortedEventList[0];
                if (currentEvent is SiteEvent)                
                     ProcessSiteEvent(currentEvent, ref AlgParams);                
                else  // Circle event
                {                  
                    ProcessCircleEvent(currentEvent, ref AlgParams);                    
                }
                
#if DEBUG               
                OnFortuneEvent(new FortuneEventArgs(ref AlgParams));
#endif
                AlgParams.EventQueue.RemoveAt(0);
            }

            return AlgParams.VoronoiGraph;
        }

        private static void ProcessCircleEvent(VoronoiEvent ev, ref FortuneAlgParams algParameters)
        {            
            Arc a = (ev as CircleEvent).MiddleArc;

            foreach (CircleEvent cev in algParameters.ArcCircleEventRegistry[a])
                algParameters.EventQueue.Remove(cev);

            GraphVertex<Vector> v = new GraphVertex<Vector>(((CircleEvent)ev).Centre);
            a.LeftBreakpoint.AddVertex(v);
            a.RightBreakpoint.AddVertex(v);
            
            if ((ev as CircleEvent).IsArcDisappearing)
                algParameters.Beachline.DeleteArc(a);

            AddNewCircleEvents(ev.Position[2], ref algParameters);                        
        }

        private static void ProcessSiteEvent(VoronoiEvent ev, ref FortuneAlgParams algParameters)
        {
            Arc a = algParameters.Beachline.FindClosestArc(ev.Position);
            
            if (a != null)    
                if (algParameters.ArcCircleEventRegistry.ContainsKey(a))
                    foreach (CircleEvent cev in algParameters.ArcCircleEventRegistry[a])
                        algParameters.EventQueue.Remove(cev);

            Arc newArc = algParameters.Beachline.AddArc(ev.Position);
            algParameters.ArcCircleEventRegistry.Add(newArc, new List<CircleEvent>());
            
            AddNewCircleEvents(ev.Position[2], ref algParameters);

        }

        private static void AddNewCircleEvents(double sweepPosition, ref FortuneAlgParams algParameters)
        {
            CircleEvent cev;
            foreach (Tuple<Arc, Arc, Arc> arcTriplet in algParameters.Beachline.ArcTripletEnumerator)
            {
                cev = new CircleEvent(arcTriplet);
                if (!algParameters.ArcTripletEventRegistry.ContainsKey(arcTriplet))
                {
                    if (cev.Position[2] > sweepPosition)   // if circle intersects sweep line
                    {
                        algParameters.ArcTripletEventRegistry.Add(arcTriplet, cev);
                        algParameters.ArcCircleEventRegistry[arcTriplet.Item1].Add(cev);
                        algParameters.ArcCircleEventRegistry[arcTriplet.Item2].Add(cev);
                        algParameters.ArcCircleEventRegistry[arcTriplet.Item3].Add(cev);
                        algParameters.EventQueue.Add(cev, null);
                    }
                }
            }
        }

        internal static void OnFortuneEvent(FortuneEventArgs e)
        {
            if (FortuneEvent != null)
                FortuneEvent(null, e);
        }
    }
}
