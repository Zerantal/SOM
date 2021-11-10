using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;
using System.Diagnostics;

using SomLibrary;
using Util;

namespace SOMSimulator
{
    class PluginRegistry<PluginType>
    {
        private Dictionary<string, PluginDetails> _plugins; // list of plugins        
        
        public PluginRegistry()
        {
            _plugins = new Dictionary<string, PluginDetails>();            
        }

        public bool Add(Type t)
        {
            // Contract.Requires<ArgumentNullException>(t != null);

            if (!IsValidPlugin(t))
            {
                Trace.WriteLine("Invalid plugin. The following conditions must hold for plugin:\n" +
                    "* Type must be a concrete class of the right type.\n" +
                    "* Type must be decorated with the SOMPluginDetailAttribute.\n" + 
                    "* All plugin properties of plugin must have return types of either Int32, Double, or String.");
                return false;
            }

            PluginDetails details = new PluginDetails(t);

            // Don't add the plugin if a plugin with the same name already exist in registry
            if (_plugins.ContainsKey(details.Name))
            {
                Trace.WriteLine("Plugin " + details.Name + " already exists in registry... skipping.");
                return false; ;
            }

            // Don't add if type doesn't have a parameterless constructor
            if (t.GetConstructor(new Type[] { }) == null)
            {
                Trace.WriteLine("Plugin " + details.Name + " has no parameterless constructor... skipping.");
                return false;
            }
            
            _plugins.Add(details.Name, details);
            
            return true;
        }       

        [Pure()]
        internal bool IsValidPlugin(Type t)
        {
            // Contract.Requires(t != null);

            if (!typeof(PluginType).IsAssignableFrom(t) || t.IsAbstract)
                return false;

            object [] attributes = t.GetCustomAttributes(typeof(SOMPluginDetailAttribute), false);
            if (attributes.Length != 1)
                return false;

            // validate plugin properties are of the supported types: Int32, Double, String            
            System.Reflection.PropertyInfo[] pi = t.GetProperties();
            foreach (System.Reflection.PropertyInfo p in pi)
            {
                attributes = p.GetCustomAttributes(typeof(SOMLibPropertyAttribute), false);
                if (attributes.Length == 1 && !(p.PropertyType.Name == "Int32" || p.PropertyType.Name == "Double" || p.PropertyType.Name == "String"))
                    return false;
            } 

            return true;
        }
        
        public IEnumerable<string> NameEnum
        {
            get
            {
                foreach (string name in _plugins.Keys)
                    yield return name;                
            }
        }

        public PluginType CreatePluginInstance(string name)
        {
            if (!_plugins.ContainsKey(name))
                throw new ArgumentException("There is no plugin with the specified name in the plugin registry.");

            PluginType retInstance;
            try
            {
                retInstance = (PluginType)Activator.CreateInstance(_plugins[name].PluginType);
            }
            catch (Exception e)
            {
                Trace.WriteLine("Exception creating plugin instance: " + e.InnerException.Message);
                return default(PluginType);
            }
            
            return retInstance;
        }

        public List<SOMPluginControl> CompilePropertyControls(string name, object pluginInstance)
        {            
            List<SOMPluginControl> retProps;          

            retProps = new List<SOMPluginControl>();

            if (!_plugins.ContainsKey(name))
                throw new ArgumentException("There is no plugin with the specified name in the plugin registry.");

            return _plugins[name].CompilePluginControls(pluginInstance);

        }

        public int Count
        {
            get { return _plugins.Count; }
        }

        public PluginDetails Details(string name)
        {
            if (!_plugins.ContainsKey(name))
                throw new ArgumentException("There is no plugin with the specified nam in the plugin registry.");

            return _plugins[name];

        }
    }
}
