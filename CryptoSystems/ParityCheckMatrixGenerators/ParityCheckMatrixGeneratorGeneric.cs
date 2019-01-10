using CryptoSystems.Interfaces;
using CryptoSystems.Models;

namespace CryptoSystems.ParityCheckMatrixGenerators
{
    public class ParityCheckMatrixGeneratorGeneric : IParityCheckMatrixGenerator
    {
        public MatrixInt Generate(ILinearCode linearCode)
        {
            var rows = 2 * linearCode.T;
            var rawResult = new int[rows, linearCode.GaloisField.WordCount];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < linearCode.GaloisField.WordCount; col++)
                {
                    rawResult[row, col] = (row * col) % linearCode.GaloisField.WordCount;
                }
            }

            var H = new MatrixInt(rawResult) + 1;
            return H;
        }
    }
}
