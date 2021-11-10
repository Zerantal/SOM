using System;
using System.Diagnostics.Contracts;

namespace MathLib.Matrices
{
    [ContractClassFor(typeof(IVector<,>))]
    abstract class IVectorContract<TVectorType, TValueType> :         
        IVector<TVectorType, TValueType>       
        where TVectorType : IVector<TVectorType, TValueType>
    {
        #region IVector<TValueType> Members

        public VectorType Orientation
        {
            get 
            {
                // Contract.Ensures(// Contract.Result<VectorType>() == VectorType.ColumnVector ||
                    // Contract.Result<VectorType>() == VectorType.RowVector);

                throw new NotImplementedException(); 
            }
        }

        public int Length
        {
            get 
            {
                // Contract.Ensures(// Contract.Result<int>() >= 1);

                throw new NotImplementedException(); 
            }
        }

        public TValueType this[int index]
        {
            get
            {              
                // Contract.Requires(index >= 0 && index < Length);

                throw new NotImplementedException();
            }
            set
            {                
                // Contract.Requires(index >= 0 && index < Length);
                
                throw new NotImplementedException();
            }
        }

        public TVectorType ArrayMultiplication(TVectorType rhs)
        {
            throw new NotImplementedException();
        }

        #endregion
       
    }
}
