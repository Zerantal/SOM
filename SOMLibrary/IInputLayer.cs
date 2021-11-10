using System.Collections.Generic;
using MathLib.Matrices;
using System.Diagnostics.Contracts;

namespace SomLibrary
{
    [ContractClass(typeof(IInputLayerContract))]
    public interface IInputLayer : IEnumerable<Vector>
    {
        int InputDimension { get;}

        bool IsOnlineSource { get;}
    }
}
