using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

using SomLibrary;

namespace SOMSimulator
{
    class PluginDetails
    {
        private Type _pluginClass;
        private string _pluginName;      
        private SOMPluginDetailAttribute _pluginAttr;

        public PluginDetails(Type t)
        {
            // Contract.Requires<ArgumentNullException>(t != null);

            bool pluginAttrFound = false;            
            
            object[] attributes = t.GetCustomAttributes(typeof(SOMPluginDetailAttribute), false);
            foreach (object a in attributes)
            {
                if (a is SOMPluginDetailAttribute)
                {
                    pluginAttrFound = true;
                    _pluginAttr = (SOMPluginDetailAttribute)a;
                }
            }
            if (!pluginAttrFound)
                throw new ArgumentException("Type must be decorated with the SOMPluginDetailAttribute.");

            _pluginClass = t;
            _pluginName = _pluginAttr.Name;

            // validate plugin properties are of the right type            
            System.Reflection.PropertyInfo[] pi = _pluginClass.GetProperties();
            foreach (System.Reflection.PropertyInfo p in pi)
            {
                attributes = p.GetCustomAttributes(typeof(SOMLibPropertyAttribute), false);
                if (attributes.Length == 1 && !(p.PropertyType.Name == "Int32" || p.PropertyType.Name == "Double" || p.PropertyType.Name == "String"))
                    throw new ArgumentException("Plugin has properties with return types other than the supported types: Int32, Double, or String.");                    
            }         
        }

        public string Name { get { return _pluginName; } set { _pluginName = value; } }

        public string Description { get { return _pluginAttr.Description; } }

        public Type AcceptableMapType { get { return _pluginAttr.MapType; } }

        public Type PluginType { get { return _pluginClass; } }

        public List<SOMPluginControl> CompilePluginControls(object pluginInstance)
        {
            List<SOMPluginControl> retControls = new List<SOMPluginControl>();
            System.Reflection.PropertyInfo[] pi = _pluginClass.GetProperties();
            object[] attributes;

            foreach (System.Reflection.PropertyInfo p in pi)
            {
                attributes = p.GetCustomAttributes(typeof(SOMLibPropertyAttribute), false);
                if (attributes.Length == 1)
                {
                    // Contract.Assume(p.PropertyType.Name == "Int32" || p.PropertyType.Name == "Double");
                    retControls.Add(new SOMPluginControl((SOMLibPropertyAttribute)attributes[0], p));
                }
            }

            return retControls;
        }

    }
}
