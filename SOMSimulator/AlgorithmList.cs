using System;
using System.Collections.Generic;
using System.Text;
using SomLibrary;
using Util;

namespace SOMSimulator
{
    class AlgorithmList : List<Type>
    {

        [TODO("This should return the name as given in the AlgorithmDetailAttribute")]
        public IEnumerable<string> NameEnum
        {
            get
            {
                foreach (Type t in this)
                {
                    yield return t.Name;
                }
            }
        }

        public ISOM CreateAlgorithmInstance(string name)
        {
            foreach (Type t in this)
            {
                if (t.Name.Equals(name))
                    return (ISOM)Activator.CreateInstance(t);
            }

            throw new ArgumentException("Type name parameter does not corrospond to any type in list");
        }

        public List<ParameterDetails> CompilePropertyList(string name)
        {
            object[] attributes;
            System.Reflection.PropertyInfo[] pi;
            List<ParameterDetails> retProps;
            Type t = null;

            retProps = new List<ParameterDetails>();

            foreach (Type tmp in this)
                if (tmp.Name.Equals(name))
                    t = tmp;

            if (t == null)
                return retProps;
            pi = t.GetProperties();
            foreach (System.Reflection.PropertyInfo p in pi)
            {
                attributes = p.GetCustomAttributes(typeof(SOMLibPropertyAttribute), false);
                if (attributes.Length == 1)
                    retProps.Add(new ParameterDetails(
                        (SOMLibPropertyAttribute)attributes[0], p));
            }

            return retProps;
        }

        public void add(Type t)
        {
            if (null != t.GetInterface(typeof(ISOM).FullName) && !t.IsAbstract)
                base.Add(t);
            else
                throw new ArgumentException("Type must implement the ISOM interface and not be and abstract class.");
        }
    }
}
