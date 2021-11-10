using System.Diagnostics.Contracts;

namespace SomLibrary
{    
    [ContractClass(typeof(IVisualiserContract))]
    public interface IVisualiser
    {
        // These method will generally require that the algorithms contain
        // an initialised map
        void VisualiseMap(ISOM algorithm, IDrawer drawer);
        bool CanVisualiseMap(ISOM algorithm);                
    }
}
