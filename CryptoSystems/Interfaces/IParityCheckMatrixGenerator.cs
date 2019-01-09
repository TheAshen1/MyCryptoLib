using CryptoSystems.Models;

namespace CryptoSystems.Interfaces
{
    public interface IParityCheckMatrixGenerator
    {
        MatrixInt Generate(GaloisField galoisField, int errorCorrectionCapability);
    }
}
