using CryptoSystems.Interfaces;
using CryptoSystems.Models;

namespace CryptoSystems.Algorithms
{
    public static class ParityCheckMatrixGeneratorGeneric
    {
        public static MatrixInt GenerateParityCheckMatrix(GaloisField galoisField, int errorCorrectionCapability)
        {
            var rows = 2 * errorCorrectionCapability;
            var rawResult = new int[rows, galoisField.WordCount];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < galoisField.WordCount; col++)
                {
                    rawResult[row, col] = (row * col) % galoisField.WordCount;
                }
            }

            var H = new MatrixInt(rawResult) + 1;
            return H;
        }
    }
}
