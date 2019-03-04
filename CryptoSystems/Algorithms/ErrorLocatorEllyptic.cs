using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using System.Collections.Generic;

namespace CryptoSystems.Algorithms
{
    public static class ErrorLocatorEllyptic
    {
        public static int[] LocateErrors(ILinearCode linearCode, MatrixInt syndrome, List<Point> points)
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
            for (int position = 0; position < linearCode.N; position++)
            {
                var sum = coefficients[0, 0];

                var wordToAdd = linearCode.GaloisField.MultiplyWords(coefficients[0, 1], points[position].x);
                sum = linearCode.GaloisField.AddWords(sum, wordToAdd);

                sum = linearCode.GaloisField.AddWords(sum, points[position].y);

                errorLocators[position] = sum;
            }
            #endregion
            return errorLocators;
        }
    }
}
