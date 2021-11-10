using System.Diagnostics.Contracts;

namespace Util
{
    [ContractClass(typeof(IDeepCloneableContract<>))]
    public interface IDeepCloneable<out T>
    {
        T DeepClone();
    }
}
