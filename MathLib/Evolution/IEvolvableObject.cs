using System.Diagnostics.Contracts;

namespace MathLib.Evolution
{
    [ContractClass(typeof(IEvolvableObjectContract<>))]
    public interface IEvolvableObject<T> where T : class, IEvolvableObject<T>
    {
        T RecombineWith(T additionalParent);

        void Mutate();

        double Fitness();       // highest score is the fittest
    }
}
