using System.Diagnostics.Contracts;

namespace SomLibrary
{
    public class RectRegion
    {
        double _xmin, _xmax, _ymin, _ymax;

        public RectRegion() : this(double.MinValue, double.MinValue, double.MaxValue, double.MaxValue) { }          

        public RectRegion(double xMin, double yMin, double xMax, double yMax)
        {
            // Contract.Requires(xMin < xMax);
            // Contract.Requires(yMin < yMax);

            _xmin = xMin;
            _xmax = xMax;
            _ymin = yMin;
            _ymax = yMax;
        }

        public double XMax 
        {
            set
            {
                // Contract.Requires(value > XMin);
            }

            get { return _xmax; }
        }

        public double XMin
        {
            set
            {
                // Contract.Requires(value < XMax);
            }

            get { return _xmin; }
        }

        public double YMax
        {
            set
            {
                // Contract.Requires(value > YMin);
            }

            get { return _ymax; }
        }

        public double YMin
        {
            set
            {
                // Contract.Requires(value < YMax);
            }

            get { return _ymin; }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(_xmin < _xmax);
            // Contract.Invariant(_ymin < _ymax);
        }
    }
}
