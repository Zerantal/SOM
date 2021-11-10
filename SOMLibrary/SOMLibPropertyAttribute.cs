using System;
using System.Diagnostics.Contracts;


namespace SomLibrary
{
    [AttributeUsage(AttributeTargets.Property, 
        AllowMultiple = false, Inherited = true)]
    public sealed class SOMLibPropertyAttribute : Attribute
    {
        // set 'isValid' to false to disable properties in a base class

        private readonly string _desc;    // description of the item
        private readonly string _name;   // name of the item (shouldn't be more than a few words)
        
        private readonly double _lowerBound;  // not used for string properties
        private readonly double _upperBound;
        private readonly double _defaultValue;
        
        public SOMLibPropertyAttribute() : this("name", "description", 0, 0, 0)
        {            
        }

        public SOMLibPropertyAttribute(string name, string description, 
            double lowerBound, double upperBound, double defaultValue)
        {
            PropertyEnabled = false;
            _name = name;
            _desc = description;
            _upperBound = upperBound;
            _lowerBound = lowerBound;
            _defaultValue = defaultValue;
        }

        public bool PropertyEnabled { get; set; }

        public string Description
        {
            get { return _desc;}
        }

        public string Name
        {
            get { return _name; }
        }
          
        public double UpperBound
        {
            get { return _upperBound; }
        }

        public double LowerBound
        {
            get { return _lowerBound; }          
        }
        
        public double Default
        {
            get { return _defaultValue; }           
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(_upperBound >= _lowerBound);
            // Contract.Invariant(_defaultValue >= _lowerBound);
            // Contract.Invariant(_defaultValue <= _upperBound);
        }
    }
}
