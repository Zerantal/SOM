using System;
using System.Diagnostics.Contracts;

using MathLib.Matrices;

namespace SomLibrary
{
    [ContractClassFor(typeof(INeuronMap))]
    internal abstract class INeuronMapContract : INeuronMap
    {
        public Vector NeuronPosition(int nodeIdx)
        {
            // Contract.Requires<ArgumentOutOfRangeException>(nodeIdx >= 0);
            // Contract.Requires<ArgumentOutOfRangeException>(nodeIdx < MapSize);
            throw new NotImplementedException();
        }
        public int[] Neighbours(int nodeIdx, int kernel)
        {
            // Contract.Requires(nodeIdx >= 0);
            // Contract.Requires(nodeIdx < MapSize);
            // Contract.Requires(kernel >= 0, "Size of neighbourhood must be greater than or equal to zero");
            // Contract.Ensures(// Contract.ForAll<int>(// Contract.Result<int[]>(), i => i >= 0));
            // Contract.Ensures(// Contract.ForAll<int>(// Contract.Result<int[]>(), i => i < MapSize));

            throw new NotImplementedException();            
        }
        
        public int MapSize
        {
            get 
            {
                // Contract.Ensures(// Contract.Result<int>() > 0);
                throw new NotImplementedException(); 
            }
        }

        public Vector this[int nodeIdx]
        {
            get
           {       
                // Contract.Requires(nodeIdx >= 0);
                // Contract.Requires(nodeIdx < MapSize);
                // Contract.Ensures(// Contract.Result<Vector>() != null);
                // Contract.Ensures(// Contract.Result<Vector>().Rows == 1);
                // Contract.Ensures(// Contract.Result<Vector>().Columns == InputDimension);                                                
                throw new NotImplementedException();
            }

            set
            {
                // Contract.Requires(nodeIdx >= 0);
                // Contract.Requires(nodeIdx < MapSize);
                // Contract.Requires(value != null);
                // Contract.Requires(value.Rows == 1);
                // Contract.Requires(value.Columns == InputDimension);
                throw new NotImplementedException();
            }
        }

        public int InputDimension
        {
            get 
            {
                // Contract.Ensures(// Contract.Result<int>() >= 1);
                throw new NotImplementedException(); 
            }
            set
            {
                // Contract.Requires(value >= 1);
                throw new NotImplementedException();
            }
        }   
    }
}
