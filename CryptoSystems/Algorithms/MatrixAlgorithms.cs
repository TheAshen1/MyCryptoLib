using CryptoSystems.Exceptions;
using CryptoSystems.Models;
using CryptoSystems.Utility;
using System;

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
                Console.WriteLine(result);
                #region Check if first row is not 0 and swap with non 0 row
                if (result[leadRow, leadColumn] == 0)
                {
                    for (int row = leadRow + 1; row < result.RowCount; row++)
                    {
                        if (result[row, leadColumn] != 0)
                        {
                            result = result.SwapRows(leadRow, row);
                            break;
                        }
                        if (row >= result.RowCount - 1)
                        {
                            throw new SolveMatrixException("Rank of the matrix is lower than expected.");
                        }
                    }
                }
                #endregion
                Console.WriteLine(result);

                #region Make leading diagonal word 1
                if (result[leadRow, leadColumn] != 1)
                {
                    var leadWordNumber = result[leadRow, leadColumn];

                    for (int col = leadColumn; col < matrix.ColumnCount; col++)
                    {
                        result[leadRow, col] = galoisField.DivideWords(result[leadRow, col], leadWordNumber);
                    }
                }
                #endregion
                Console.WriteLine(result);

                #region make all other values in the same column 0
                for (int i = 0; i < matrix.RowCount; i++)
                {
                    if (i == leadRow)
                    {
                        continue;
                    }
                    if (result[i, leadColumn] == 0)
                    {
                        continue;
                    }

                    var multiplier = galoisField.DivideWords(result[i, leadColumn], result[leadColumn, leadColumn]);
                    for (int col = leadColumn; col < matrix.ColumnCount; col++)
                    {
                        var wordToAdd = galoisField.MultiplyWords(multiplier, result[leadRow, col]);
                        result[i, col] = galoisField.AddWords(wordToAdd, result[i, col]);
                    }
                    Console.WriteLine(result);

                }
                #endregion

                leadColumn++;
            }
           
            Console.WriteLine(result);

            return result.GetRangeOfColumns(new RangeInt(result.RowCount, result.ColumnCount));
        }

        public static (MatrixInt, MatrixInt) LUDecomposition(MatrixInt matrix, GaloisField galoisField)
        {
            if (matrix.RowCount != matrix.ColumnCount)
            {
                throw new DimensionMismatchException("The number of rows in the matrix should be same as number columns.");
            }

            var U = matrix.Clone();
            var L = Helper.GenerateIdentityMatrix(matrix.RowCount);

            int leadColumn = 0;
            for (int leadRow = 0; leadRow < U.RowCount; leadRow++)
            {
                var diagonalWordNumber = U[leadRow, leadColumn];

                for (int row = (leadRow + 1); row < matrix.RowCount; row++)
                {
                    var otherRowLeadingValue = U[row, leadColumn];
                    if (otherRowLeadingValue == 0)
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

            var result = new MatrixInt(rowCount, columnCount);

            for (int row = 0; row < rowCount; row++)
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

        public static MatrixInt MatrixInverse(MatrixInt matrix, GaloisField galoisField)
        {
            var identity = Helper.GenerateIdentityMatrix(matrix.RowCount);

            var toSolve = matrix | (identity);

            var inverse = MatrixAlgorithms.Solve(toSolve, galoisField);

            return inverse;
        }

        //public static int RankOfMatrix(MatrixInt matrix)
        //{
        //    int rank = matrix.ColumnCount;

        //    for (int row = 0; row < rank; row++)
        //    {

        //        // Before we visit current row  
        //        // 'row', we make sure that  
        //        // mat[row][0],....mat[row][row-1] 
        //        // are 0. 

        //        // Diagonal element is not zero 
        //        if (matrix[row, row] != 0)
        //        {
        //            for (int col = 0; col < R; col++)
        //            {
        //                if (col != row)
        //                {
        //                    // This makes all entries  
        //                    // of current column  
        //                    // as 0 except entry  
        //                    // 'mat[row][row]' 
        //                    double mult =
        //                       (double)matrix[col, row] /
        //                                matrix[row, row];

        //                    for (int i = 0; i < rank; i++)

        //                        matrix[col, i] -= (int)mult
        //                                 * matrix[row, i];
        //                }
        //            }
        //        }

        //        // Diagonal element is already zero.  
        //        // Two cases arise: 
        //        // 1) If there is a row below it  
        //        // with non-zero entry, then swap  
        //        // this row with that row and process  
        //        // that row 
        //        // 2) If all elements in current  
        //        // column below mat[r][row] are 0,  
        //        // then remvoe this column by  
        //        // swapping it with last column and 
        //        // reducing number of columns by 1. 
        //        else
        //        {
        //            bool reduce = true;

        //            // Find the non-zero element  
        //            // in current column  
        //            for (int i = row + 1; i < R; i++)
        //            {
        //                // Swap the row with non-zero  
        //                // element with this row. 
        //                if (mat[i, row] != 0)
        //                {
        //                    swap(mat, row, i, rank);
        //                    reduce = false;
        //                    break;
        //                }
        //            }

        //            // If we did not find any row with  
        //            // non-zero element in current  
        //            // columnm, then all values in  
        //            // this column are 0. 
        //            if (reduce)
        //            {
        //                // Reduce number of columns 
        //                rank--;

        //                // Copy the last column here 
        //                for (int i = 0; i < R; i++)
        //                    mat[i, row] = mat[i, rank];
        //            }

        //            // Process this row again 
        //            row--;
        //        }

        //        // Uncomment these lines to see  
        //        // intermediate results display(mat, R, C); 
        //        // printf("\n"); 
        //    }
        //}


        public static int RankOfMatrix(MatrixInt matrix, GaloisField galoisField)
        {
            var result = matrix.Clone();
            int rank = result.ColumnCount;
            Console.WriteLine(result);

            for (int row = 0; row < rank; row++)
            {

                // Before we visit current row  
                // 'row', we make sure that  
                // mat[row][0],....mat[row][row-1] 
                // are 0. 

                // Diagonal element is not zero 
                if (result[row, row] != 0)
                {
                    for (int subRow = 0; subRow < result.RowCount; subRow++)
                    {
                        if (subRow != row)
                        {
                            // This makes all entries  
                            // of current column  
                            // as 0 except entry  
                            // 'mat[row][row]' 
                            var mult = galoisField.DivideWords(result[subRow, row], result[row, row]);

                            for (int i = 0; i < rank; i++)
                            {
                                var temp = galoisField.MultiplyWords(mult, result[row, i]);
                                result[subRow, i] = galoisField.AddWords(temp, result[subRow, i]);
                            }
                            Console.WriteLine(result);
                        }
                    }
                    Console.WriteLine(result);

                }

                // Diagonal element is already zero.  
                // Two cases arise: 
                // 1) If there is a row below it  
                // with non-zero entry, then swap  
                // this row with that row and process  
                // that row 
                // 2) If all elements in current  
                // column below mat[r][row] are 0,  
                // then remvoe this column by  
                // swapping it with last column and 
                // reducing number of columns by 1. 
                else
                {
                    bool reduce = true;

                    // Find the non-zero element  
                    // in current column  
                    for (int i = row + 1; i < result.RowCount; i++)
                    {
                        // Swap the row with non-zero  
                        // element with this row. 
                        if (result[i, row] != 0)
                        {
                            result = result.SwapRows(row, i);
                            reduce = false;
                            break;
                        }
                    }
                    Console.WriteLine(result);

                    // If we did not find any row with  
                    // non-zero element in current  
                    // columnm, then all values in  
                    // this column are 0. 
                    if (reduce)
                    {
                        // Reduce number of columns 
                        rank--;

                        // Copy the last column here 
                        for (int i = 0; i < result.RowCount; i++)
                            result[i, row] = result[i, rank];
                    }

                    // Process this row again 
                    row--;
                }

                // Uncomment these lines to see  
                // intermediate results display(mat, R, C); 
                // printf("\n"); 
            }

            return rank;
        }
    }
}
