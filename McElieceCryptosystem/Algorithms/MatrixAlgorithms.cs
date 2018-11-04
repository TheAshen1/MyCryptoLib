using McElieceCryptosystem.Models;
using System;

namespace McElieceCryptosystem.Algorithms
{
    public static class MatrixAlgorithms
    {
        public static MatrixInt ToReducedRowEchelonFormBinary(MatrixInt matrix)
        {
            var rawResult = matrix.Data;
            var lead = 0; // lead column
            for (int row = 0; row < matrix.RowCount; row++)
            {
                if (lead >= matrix.ColumnCount)
                    break;

                #region Find first row from top with nonzero element at lead column   
                int i = row;
                while (rawResult[i, lead] == 0)
                {
                    i++;
                    if (i == matrix.RowCount)
                    {
                        i = row;
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
                for (int col = 0; col < matrix.ColumnCount; col++)
                {
                    int temp = rawResult[row, col];
                    rawResult[row, col] = rawResult[i, col];
                    rawResult[i, col] = temp;
                }
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
                        int sub = rawResult[j, lead];
                        for (int k = 0; k < matrix.ColumnCount; k++)
                        {
                            rawResult[j, k] = (rawResult[j, k] - (sub * rawResult[row, k])) % 2;
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
                    if (rawResult[row, col] < 0)
                    {
                        rawResult[row, col] = -rawResult[row, col];
                    }
                }
            }

            var result = new MatrixInt(rawResult);
            return result;
        }   
    }
}
