using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using System;
using System.Diagnostics;

namespace CryptoSystems.Algorithms
{
    public static class GeneratorMatrixCalculator
    {
        public static MatrixInt CalculateGeneratorMatrix(ILinearCode linearCode)
        {
            var generatorMatrix = new MatrixInt(linearCode.K, linearCode.N);

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
                Debug.WriteLine(systemB);

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

        public static MatrixInt CalculateGeneratorMatrixAlt(ILinearCode linearCode)
        {
            var generatorMatrix = new MatrixInt(linearCode.K, linearCode.N);
            var system = linearCode.ParityCheckMatrix.Clone();
            MatrixInt solution = null;
            try
            {
                solution = MatrixAlgorithms.Solve(system, linearCode.GaloisField);
            }
            catch (SolveMatrixException)
            {
                throw new LinearCodeException("Could not produce correct Generator matrix from provided ParityCheck matrix.");
            }

            #region Assemble generator matrix
            for (int i = 0; i < linearCode.K; i++)
            {
                for (int j = 0; j < solution.RowCount; j++)
                {
                    generatorMatrix[i, j] = solution[j, i];
                }

                if(solution.RowCount < linearCode.N)
                {
                    generatorMatrix[i, linearCode.N - linearCode.K + i] = 1;
                }
            }
            #endregion

            return generatorMatrix;
        }
    }
}
