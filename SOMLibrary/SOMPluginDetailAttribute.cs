using System;

namespace SomLibrary
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SOMPluginDetailAttribute : Attribute
    {
        private readonly string _name;
        private readonly string _desc;

        private readonly Type _mapType;

        public SOMPluginDetailAttribute() : this("no name", "Algorithm has no name!!!") { }

        public SOMPluginDetailAttribute(string name, string description)
        {
            _name = name;
            _desc = description;
            _mapType = typeof(INeuronMap);
        }

        public SOMPluginDetailAttribute(string name, string description, Type mapType) : this(name, description)
        {
            _mapType = mapType;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get {return _desc;}
        }

        public Type MapType
        {
            get { return _mapType; }
        }
    }
}
