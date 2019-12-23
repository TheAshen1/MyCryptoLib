using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;

namespace CryptoSystems.Algorithms
{
    public static class ErrorLocatorEllyptic
    {
        public static int[] LocateErrors(ILinearCode linearCode, MatrixInt syndrome, ParityCheckMatrixGeneratorEllyptic generator)
        {
            #region Error locator polynomial
            var rowCount = linearCode.T;
            var columnCount = linearCode.T + 1;

            var system = new int[rowCount, columnCount];

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount - 1; col++)
                {
                    system[row, col] = syndrome[0, generator.Terms[(row + col), linearCode.T - 2]];
                }
                system[row, columnCount - 1] = syndrome[0, generator.Terms[row, linearCode.T - 1]];
            }
            var coefficients = MatrixAlgorithms.Solve(new MatrixInt(system), linearCode.GaloisField).Transpose();
            #endregion

            #region Calculate Error Positions
            var errorLocators = new int[linearCode.N];
            for (int position = 0; position < linearCode.N; position++)
            {
                var sum = 0;
                for (int i = 0; i < linearCode.T; i++)
                {
                    var wordToAdd = linearCode.GaloisField.MultiplyWords(coefficients[0, i], linearCode.GaloisField.Power(generator.Points[position].x, i));
                    sum = linearCode.GaloisField.AddWords(sum, wordToAdd);
                }

                sum = linearCode.GaloisField.AddWords(sum, generator.Points[position].y);

                errorLocators[position] = sum;
            }
            #endregion
            return errorLocators;
        }
    }
}
