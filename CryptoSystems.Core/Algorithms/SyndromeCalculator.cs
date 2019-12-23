using CryptoSystems.Exceptions;
using CryptoSystems.Models;

namespace CryptoSystems.Algorithms
{
    public class SyndromeCalculator
    {
        public static MatrixInt Calculate(MatrixInt matrixLeft, MatrixInt matrixRight, GaloisField galoisField)
        {
            if (matrixLeft.ColumnCount != matrixRight.RowCount)
            {
                throw new DimensionMismatchException("Number of columns in first matrix does not equal number of rows in second matrix.");
            }

            var rowCount = matrixLeft.RowCount;
            var columnCount = matrixRight.ColumnCount;

            var result = new MatrixInt(rowCount, columnCount);

            for (int row = rowCount - 1; row >= 0; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    int sum = 0;
                    for (int k = 0; k < matrixLeft.ColumnCount; k++)
                    {
                        var product = galoisField.MultiplyWords(matrixLeft.Data[row, k], matrixRight.Data[k, col]);
                        sum = galoisField.AddWords(sum, product);
                    }
                    result[row, col] = sum;
                }
            }
            return result;
        }

    }
}
