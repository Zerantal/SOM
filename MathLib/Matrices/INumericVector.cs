namespace MathLib.Matrices
{
    public interface INumericVector
    {
        double Norm { get; }

        double NormSquared { get; }

        double InfinityNorm { get; }

        double OneNorm { get; }
        
    }
}
