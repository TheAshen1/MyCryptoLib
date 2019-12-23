using CryptoSystems.Interfaces;
using CryptoSystems.Models;

namespace CryptoSystems.Algorithms
{
    public static class ErrorLocatorDefault
    {
        public static int[] LocateErrors(ILinearCode linearCode, MatrixInt syndrome)
        {
            #region Error locator polynomial
            var rowCount = linearCode.T;
            var columnCount = linearCode.T + 1;

            var system = new int[rowCount, columnCount];

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    system[row, col] = syndrome[0, row + col];
                }
            }
            var coefficients = MatrixAlgorithms.Solve(new MatrixInt(system), linearCode.GaloisField).Transpose();
            #endregion

            #region Calculate Error Positions
            var errorLocators = new int[linearCode.N];
            for (int position = 0, word = 1; position < linearCode.N; position++, word++)
            {
                var sum = coefficients[0, 0];
                for (int i = 1; i < coefficients.ColumnCount; i++)
                {
                    var wordPower = linearCode.GaloisField.Power(word, i);
                    var wordToAdd = linearCode.GaloisField.MultiplyWords(coefficients[0, i], wordPower);
                    sum = linearCode.GaloisField.AddWords(sum, wordToAdd);
                }

                var lastWord = linearCode.GaloisField.Power(word, linearCode.T);
                sum = linearCode.GaloisField.AddWords(sum, lastWord);

                errorLocators[position] = sum;
            }
            #endregion
            return errorLocators;
        }

    }
}
