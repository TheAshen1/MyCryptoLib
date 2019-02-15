using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using System;

namespace CryptoSystems.Algorithms
{
    public static class GeneratorMatrixCalculator
    {
        public static MatrixInt CalculateGeneratorMatrix(ILinearCode linearCode)
        {
            var generatorMatrix = new MatrixInt(new int[linearCode.K, linearCode.N]);

            #region Init
            for (int i = 0; i < linearCode.K; i++)
            {
                for (int j = 0; j < linearCode.K; j++)
                {
                    if (i == j)
                    {
                        generatorMatrix[i, j] = 1;
                    }
                    else
                    {
                        generatorMatrix[i, j] = 0;
                    }
                }
            }
            #endregion

            #region Solve k systems of linear equasions on field elements
            for (int i = 0; i < linearCode.K; i++)
            {
                var systemA = linearCode.ParityCheckMatrix.GetRangeOfColumns(new RangeInt(linearCode.K, linearCode.N));             
                var systemB = systemA | linearCode.ParityCheckMatrix.GetColumn(i);
                Console.WriteLine(systemB);

                MatrixInt systemSolution;
                try
                {
                    systemSolution = MatrixAlgorithms.Solve(systemB, linearCode.GaloisField);
                }
                catch (SolveMatrixException)
                {
                    throw new LinearCodeException("Could not produce correct Generator matrix from provided ParityCheck matrix.");
                }

                #region CopyResults
                for (int row = 0; row < systemSolution.RowCount; row++)
                {
                    generatorMatrix[i, row + linearCode.K] = systemSolution[row, systemSolution.ColumnCount - 1];
                }
                #endregion

            }
            #endregion

            return generatorMatrix;
        }
    }
}
