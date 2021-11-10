using System.Diagnostics.Contracts;

namespace Util
{
    [ContractClass(typeof(IShallowCloneableContract<>))]
    public interface IShallowCloneable<out T>
    {
        T ShallowClone();
    }
}
