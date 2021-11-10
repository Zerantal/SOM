using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using SomLibrary;
using System.Windows.Forms;
using System.Diagnostics.Contracts;
using System.Diagnostics;

using Util;

namespace SOMSimulator
{
    [ToDo("The contents of some of these methods could be better consolidated")]
    internal class SOMPluginControl
    {

		#region Fields (8) 

        private string description;

        private NumericUpDown inputCtrl = null;
        private string name;
        private PropertyInfo pi;
        private double upperBound;
        private double lowerBound;
        private double _defaultValue;

		#endregion Fields 

		#region Constructors (1) 

        // property should have one of the following types: double, int, or string
        public SOMPluginControl(SOMLibPropertyAttribute prop, PropertyInfo propInfo)           
        {
            // Contract.Requires(propInfo != null);
            // Contract.Requires(prop != null);
            // Contract.Requires<ArgumentException>(propInfo.PropertyType.Name == "Int32" || propInfo.PropertyType.Name == "Double",
                //"SOM property type has to be either Int32 or Double");

            name = prop.Name;
            description = prop.Description;
            pi = propInfo;

            lowerBound = prop.LowerBound;
            upperBound = prop.UpperBound;
            _defaultValue = prop.Default;            

            CreateInputControl();
        }

		#endregion Constructors 

		#region Properties (5) 

        public string Description
        {
            get { return description; }
        }

        public double LowerBound
        {
            get { return lowerBound; }
        }

        public string Name
        {
            get { return name; }           
        }

        public PropertyInfo PropertyInfo
        {
            get { return pi; }
        }

        public double UpperBound
        {
            get { return upperBound; }
        }

        public Control ParameterControl
        {
            get { return inputCtrl; }
        }

		#endregion Properties 

		#region Methods (6) 
        
        private void CreateInputControl()
        {
            inputCtrl = new NumericUpDown();
            inputCtrl.Minimum = Convert.ToDecimal(lowerBound);
            inputCtrl.Maximum = Convert.ToDecimal(upperBound);
            inputCtrl.Name = Name + "Prop";
            inputCtrl.Value = Convert.ToDecimal(_defaultValue);

            switch (pi.PropertyType.Name)
            {
                case "Double":
                    inputCtrl.DecimalPlaces = 8;
                    inputCtrl.Increment = 0.01m;
                    break;

                case "Int32":
                    inputCtrl.DecimalPlaces = 0;
                    inputCtrl.Increment = 1;
                    break;

                default:
                    break;
            }
        }

        // Synchronize algorithms parameters with its input control value.
        public void UpdateParameter(object SOMInstance)
        {
            // Contract.Requires(SOMInstance != null);

            try
            {
                if (pi.PropertyType.Name == "Int32")
                    pi.SetValue(SOMInstance, (Int32)inputCtrl.Value, null);
                else
                    pi.SetValue(SOMInstance, (Double)inputCtrl.Value, null);
            }
            catch (System.Reflection.TargetException e)
            {
                Trace.WriteLine("Unable to set parameter of SOM algorithm: " + e.Message);
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                Trace.WriteLine("Unable to set parameter of SOM algorithm: " + e.Message);
            }
        }



		// Private Methods (3) 

		#endregion Methods 


        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // Contract.Invariant(pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double",
                //"SOM property type has to be either Int32, Double, or String");
            // Contract.Invariant(inputCtrl != null, "Parameter control hasn't been successfully created.");
        }
    }
}
