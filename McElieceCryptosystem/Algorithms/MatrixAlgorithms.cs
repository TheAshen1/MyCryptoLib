using CryptoSystems.Exceptions;
using CryptoSystems.Models;
using CryptoSystems.Util;

namespace CryptoSystems.Algorithms
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

            var leadColumn = 0;
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

        //public static MatrixInt LUSolve(MatrixInt matrix, GaloisField galoisField)
        //{
        //    var (L, U) = LUDecomposition(matrix, galoisField);

        //    var rightPart = Utility.GenerateIdentityMatrix(L.RowCount) - 1;
        //    var y = rightPart.Clone();

        //    Console.WriteLine(rightPart);

        //    // find solution of Ly = b
        //    for (int row = 0; row < L.RowCount; row++)
        //    {
        //        for (int col = 0; col < row; col++)
        //        {
        //            var sum = -1;
        //            for (int k = 0; k < L.RowCount; k++)
        //            {
        //                var word = galoisField.MultiplyWords(L[row, k], y[k, col]);
        //                sum = galoisField.AddWords(sum, word);
        //            }
        //            y[row, col] = galoisField.AddWords(y[row, col], sum);
        //        }
        //        Console.WriteLine(y);
        //    }

        //    //// find solution of Ux = y
        //    //var x = new int[n];
        //    //for (int i = n - 1; i >= 0; i--)
        //    //{
        //    //    var sum = -1;
        //    //    for (int k = i + 1; k < n; k++)
        //    //    {
        //    //        var word = galoisField.MultiplyWords(lu[i, k], x[k]);
        //    //        sum = galoisField.AddWords(sum, word);
        //    //    }
        //    //    x[i] = galoisField.DivideWords(galoisField.AddWords(y[i], sum), lu[i, i]);
        //    //}

        //    return y;
        //}

        public static (MatrixInt, MatrixInt) LUDecomposition(MatrixInt matrix, GaloisField galoisField)
        {
            if (matrix.RowCount != matrix.ColumnCount)
            {
                throw new DimensionMismatchException("The number of rows in the matrix should be same as number columns.");
            }

            var U = matrix.Clone();
            var L = Utility.GenerateIdentityMatrix(matrix.RowCount) - 1;

            int leadColumn = 0;
            for (int leadRow = 0; leadRow < U.RowCount; leadRow++)
            {
                var diagonalWordNumber = U[leadRow, leadColumn];

                for (int row = (leadRow + 1); row < matrix.RowCount; row++)
                {
                    var otherRowLeadingValue = U[row, leadColumn];
                    if(otherRowLeadingValue == -1)
                    {
                        continue;
                    }

                    for (int col = leadColumn; (col + 1) < matrix.ColumnCount; col++)
                    {
                        var multiplier = galoisField.DivideWords(otherRowLeadingValue, U[leadRow, col]);
                        var wordToSubtract = galoisField.MultiplyWords(U[leadRow, col], multiplier);
                        L[row, col] = multiplier;
                        U[row, col] = galoisField.AddWords(U[row, col], wordToSubtract);
                    }
                }
                leadColumn++;
            }
            return (L, U);
        }

        public static int GetDeterminant(MatrixInt matrix, GaloisField galoisField)
        {
            var (L, U) = LUDecomposition(matrix, galoisField);
            
            var Ldeterminant = 0;
            var Udeterminant = 0;

            for (int i = 0; i < L.RowCount; i++)
            {
                Ldeterminant = galoisField.MultiplyWords(Ldeterminant, L[i, i]);
            }
            for (int i = 0; i < U.RowCount; i++)
            {
                Udeterminant = galoisField.MultiplyWords(Udeterminant, U[i, i]);
            }

            var determinant = galoisField.MultiplyWords(Ldeterminant, Udeterminant);
            return determinant;
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
