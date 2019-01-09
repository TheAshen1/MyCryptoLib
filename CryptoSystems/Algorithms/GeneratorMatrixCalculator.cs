using CryptoSystems.Models;

namespace CryptoSystems.Algorithms
{
    public static class GeneratorMatrixCalculator
    {
        public static MatrixInt CalculateGeneratorMatrix(int n, int k, MatrixInt parityCheckMatrix, GaloisField galoisField)
        {
            var generatorMatrix = new MatrixInt(new int[k, n]);
            #region Init
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
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
            for (int i = 0; i < k; i++)
            {
                var system = parityCheckMatrix.GetRangeOfColumns(new RangeInt(k, n)) | parityCheckMatrix.GetColumn(i);
                var systemSolution = MatrixAlgorithms.Solve(system, galoisField);

                #region CopyResults
                for (int row = 0; row < systemSolution.RowCount; row++)
                {
                    generatorMatrix[i, row + k] = systemSolution.Data[row, systemSolution.ColumnCount - 1];
                }
                #endregion
            }
            #endregion

            return generatorMatrix;
        }
    }
}
