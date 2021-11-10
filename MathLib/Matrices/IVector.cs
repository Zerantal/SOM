using System.Diagnostics.Contracts;

namespace MathLib.Matrices
{
    [ContractClass(typeof(IVectorContract<,>))]
    public interface IVector<TVector, TValue> where TVector : IVector<TVector, TValue>
    {
        VectorType Orientation { get; }

        int Length { get; }

        TValue this[int index] { get; set; }

        TVector ArrayMultiplication(TVector rhs);
    }  
}
