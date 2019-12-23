using CryptoSystems.Exceptions;
using CryptoSystems.Models;
using System;

namespace CryptoSystems.Mappers
{
    public static class Mapper
    {
        public static PolynomialDouble Map(MatrixInt matrix)
        {
            if (matrix.RowCount > 1)
            {
                throw new DimensionMismatchException("Base matrix for shoud consist of single row");
            }

            double[] values = new double[matrix.ColumnCount];

            for (int i = 0; i < matrix.ColumnCount; i++)
            {
                values[i] = (double)matrix.Data[0, i];
            }

            var result = new PolynomialDouble(values);
            return result;
        }

        public static MatrixInt Map(PolynomialDouble polynomial)
        {
            int[,] values = new int[1,polynomial.Degree];

            for (int i = 0; i < polynomial.Degree; i++)
            {
                values[0, i] = (int) Math.Round(polynomial.Coefficients[i]);
            }

            var result = new MatrixInt(values);
            return result;
        }
    }
}
