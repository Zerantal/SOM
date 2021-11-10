namespace MathLib.Matrices
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public interface INumericMatrix<in TMatrixType, out TVectorType, in TValueType>
        where TMatrixType : INumericMatrix<TMatrixType, TVectorType, TValueType>
        where TVectorType : INumericVector
    {
        bool IsEqualTo(TMatrixType arg, TValueType errorTolerance);

        TVectorType RowNorms();
    }
}
