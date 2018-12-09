using McElieceCryptosystem.Exceptions;
using McElieceCryptosystem.Models;
using McElieceCryptosystem.Util;
using System;

namespace McElieceCryptosystem.Algorithms
{
    public static class MatrixAlgorithms
    {
        public static MatrixInt Solve(MatrixInt matrix)
        {
            var result = matrix.Clone();
            var lead = 0; // lead column
            for (int row = 0; row < matrix.RowCount; row++)
            {
                if (lead >= matrix.ColumnCount)
                    break;

                #region Find first row from top with nonzero element at lead column   
                int rowWithNonZeroElement = row;
                while (result[rowWithNonZeroElement, lead] == 0)
                {
                    rowWithNonZeroElement++;
                    if (rowWithNonZeroElement == matrix.RowCount)
                    {
                        rowWithNonZeroElement = row;
                        lead++;
                        if (lead == matrix.ColumnCount)
                        {
                            lead--;
                            break;
                        }
                    }
                }
                #endregion

                #region Swap rows 
                result.SwapRows(row, rowWithNonZeroElement);
                #endregion


                //skipping division part cause we are operating on binary matrix
                //int div = rawResult[row, lead];
                //if (div != 0)
                //    for (int j = 0; j < matrix.ColumnCount; j++)
                //        rawResult[row, j] /= div;

                #region Subtraction rows 
                for (int j = 0; j < matrix.RowCount; j++)
                {
                    if (j != row)
                    {
                        int sub = result[j, lead];
                        for (int k = 0; k < matrix.ColumnCount; k++)
                        {
                            result.Data[j, k] = (result.Data[j, k] - (sub * result.Data[row, k])) % 2;
                        }
                    }
                }
                #endregion

                lead++;
            }

            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int col = 0; col < matrix.ColumnCount; col++)
                {
                    if (result[row, col] < 0)
                    {
                        result[row, col] = -result[row, col];
                    }
                }
            }

            return result;
        }

        public static MatrixInt Solve(MatrixInt matrix, GaloisField galoisField)
        {
            var result = matrix.Clone();

            var leadColumn = 0; // lead column
            for (int leadRow = 0; leadRow < matrix.RowCount; leadRow++)
            {
                #region Make leading diagonal word 0
                if (result[leadRow, leadColumn] != 0)
                {
                    var targetWordNumber = result[leadRow, leadColumn];

                    for (int col = leadColumn; col < matrix.ColumnCount; col++)
                    {
                        result[leadRow, col] = galoisField.DivideWords(result[leadRow, col], targetWordNumber);
                    }
                }
                #endregion

                #region subtract from all other rows 
                //subtraction is basicy the same as summation here

                for (int i = (leadRow + 1); i < matrix.RowCount; i++)
                {
                    var otherRowLeadingValue = result[i, leadColumn];
                    for (int col = leadColumn; col < matrix.ColumnCount; col++)
                    {
                        var wordToSubtract = galoisField.MultiplyWords(otherRowLeadingValue, result[leadRow, col]);
                        result[i, col] = galoisField.AddWords(wordToSubtract, result[i, col]);
                    }
                }
                #endregion
                leadColumn++;
            }

            #region Backwards    
            leadColumn--;

            for (int leadRow = (matrix.RowCount - 1); leadRow > 0; leadRow--)
            {
                for (int row = (leadRow - 1); row >= 0; row--)
                {
                    var otherRowLeadingValue = result[row, leadColumn];
                    for (int col = matrix.ColumnCount - 1; col >= leadColumn; col--)
                    {
                        var wordToSubtract = galoisField.MultiplyWords(otherRowLeadingValue, result[leadRow, col]);

                        result[row, col] = galoisField.AddWords(wordToSubtract, result[row, col]);
                    }
                }
                leadColumn--;
            }
            #endregion

            return result.GetRangeOfColumns(new RangeInt(result.RowCount, result.ColumnCount));
        }

        public static MatrixInt LUPDecomposition(MatrixInt matrix, GaloisField galoisField)
        {
            var n = matrix.RowCount;
            var lu = new int[n, n];

            for (int row = 0; row < n; row++)
            {
                for (int col = row; col < n; col++)
                {
                    var sum = -1;
                    for (int k = 0; k < row; k++)
                    {
                        var word = galoisField.MultiplyWords(lu[row, k], lu[k, col]);
                        sum = galoisField.AddWords(sum, word);
                    }
                    lu[row, col] = galoisField.AddWords(matrix.Data[row, col], sum);

                    Console.WriteLine(new MatrixInt(lu));
                }

                for (int j = row + 1; j < n; j++)
                {
                    var sum = -1;
                    for (int k = 0; k < row; k++)
                    {
                        var word = galoisField.MultiplyWords(lu[j, k], lu[k, row]);
                        sum = galoisField.AddWords(sum, word);
                    }
                    lu[j, row] = galoisField.DivideWords(galoisField.AddWords(matrix.Data[j, row], sum), lu[row, row]);

                    Console.WriteLine(new MatrixInt(lu));
                }
            }

            // find solution of Ly = b
            var y = new int[n];
            for (int i = 0; i < n; i++)
            {
                var sum = -1;
                for (int k = 0; k < i; k++)
                {
                    var word = galoisField.MultiplyWords(lu[i, k], y[k]);
                    sum = galoisField.AddWords(sum, word);
                }

                y[i] = galoisField.AddWords(matrix.Data[i, matrix.ColumnCount - 1], sum);
            }

            // find solution of Ux = y
            var x = new int[n];
            for (int i = n - 1; i >= 0; i--)
            {
                var sum = -1;
                for (int k = i + 1; k < n; k++)
                {
                    var word = galoisField.MultiplyWords(lu[i, k], x[k]);
                    sum = galoisField.AddWords(sum, word);
                }
                x[i] = galoisField.DivideWords(galoisField.AddWords(y[i], sum), lu[i, i]);
            }

            return new MatrixInt(x);
        }

        public static MatrixInt DotMultiplication(MatrixInt matrixLeft, MatrixInt matrixRight, GaloisField galoisField)
        {
            if (matrixLeft.ColumnCount != matrixRight.RowCount)
            {
                throw new DimensionMismatchException("Number of columns in first matrix does not equal number of rows in second matrix.");
            }

            var rowCount = matrixLeft.RowCount;
            var columnCount = matrixRight.ColumnCount;

            var rawResult = new int[rowCount, columnCount];

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    int sum = -1;
                    for (int k = 0; k < matrixLeft.ColumnCount; k++)
                    {
                        var product = galoisField.MultiplyWords(matrixLeft.Data[row, k], matrixRight.Data[k, col]);
                        sum = galoisField.AddWords(sum, product);
                    }
                    rawResult[row, col] = sum;
                }
            }

            var result = new MatrixInt(rawResult);
            return result;
        }

        public static MatrixInt MatrixInverse(MatrixInt matrix, GaloisField galoisField)
        {
            var identity = Utility.GenerateIdentityMatrix(matrix.RowCount);

            var toSolve = matrix | (identity - 1);

            var inverse = MatrixAlgorithms.Solve(toSolve, galoisField);

            return inverse;
        }
    }
}
