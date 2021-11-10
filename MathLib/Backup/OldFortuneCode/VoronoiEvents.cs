using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

using MathLib.Matrices;

namespace MathLib.Graph
{
    internal class VoronoiEvent : IComparable<VoronoiEvent>
    {
        protected Vector _eventLocation;

        internal VoronoiEvent()
        {
            _eventLocation = new Vector(2);
        }
        public VoronoiEvent(Vector eventLocation) 
        {
            Contract.Requires(eventLocation.Length == 2);

            this._eventLocation = eventLocation; 
        }

        #region IComparable<VoronoiEvent> Members

        public int CompareTo(VoronoiEvent other)
        {
            // make comparison based on first element of vector
            return (_eventLocation[2].CompareTo(other._eventLocation[2]));           
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_eventLocation.Length == 2);
        }

        public Vector Position { get { return _eventLocation; } }
    }

    internal class SiteEvent : VoronoiEvent
    {
        public SiteEvent(Vector eventLocation) : base(eventLocation) { }
    }

    internal class CircleEvent : VoronoiEvent
    {
        Vector _v1, _v2, _v3;
        Vector _centre;               // centre of circle
        double radius;
        Arc _middleArc;

        public CircleEvent(Tuple<Arc, Arc, Arc> arcTriplet)
        {
            // ensure that arc triplet is in order from minimum x to maximum x values
            Contract.Requires(arcTriplet.Item1.Site[1] <= arcTriplet.Item2.Site[1] &&
                arcTriplet.Item2.Site[1] <= arcTriplet.Item3.Site[1]);

            _v1 = arcTriplet.Item1.Site;
            _v2 = arcTriplet.Item2.Site;
            _v3 = arcTriplet.Item3.Site;

            // calculate centre and radius of circle
            double a = -(_v2[2] - _v1[2]) / (_v2[1] - _v1[1]);
            double b = (_v2[2] * _v2[2] + _v2[1] * _v2[1] - _v1[2] * _v1[2] - _v1[1] * _v1[1]) / (2 * (_v2[1] - _v1[1]));
            double c = -(_v3[2] - _v1[2]) / (_v3[1] - _v1[1]);
            double d = (_v3[2] * _v3[2] + _v3[1] * _v3[1] - _v1[2] * _v1[2] - _v1[1] * _v1[1]) / (2 * (_v3[1] - _v1[1]));

            _centre = new Vector(2);
            _centre[2] = (d - b) / (a - c);
            _centre[1] = c * _centre[2] + d;

            radius = (_v1 - _centre).Norm;
            _eventLocation[1] = _centre[1];
            _eventLocation[2] = _centre[2] + radius;    // set event location to top of circle

            // Find out which breakpoints are to be froz 
            _middleArc = arcTriplet.Item2;
             
        }

        public Vector Centre { get { return _centre; } }

        public Arc MiddleArc { get { return _middleArc; } }

        public bool IsArcDisappearing
        {
            get
            {
                if (_v2[2] < _v1[2] && _v2[2] > _v3[2])
                    return true;
                else
                    return false;
            }
        }

    }

}
