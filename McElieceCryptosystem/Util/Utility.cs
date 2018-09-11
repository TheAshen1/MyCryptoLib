using McElieceCryptosystem.Exceptions;
using McElieceCryptosystem.Models;

namespace McElieceCryptosystem.Util
{
    public static class Utility
    {
        public static int Weight(MatrixInt vector)
        {
            if (vector.RowCount > 1)
            {
                throw new DimensionMismatchException("Vector should consist of single row");
            }

            var count = 0;
            foreach (var value in vector.Data)
            {
                if (value != 0)
                {
                    count++;
                }
            }
            return count;
        }

        public static int Distance(MatrixInt vector1, MatrixInt vector2)
        {
            if(vector1.RowCount != vector2.RowCount )
            {
                throw new DimensionMismatchException("Number of rows in first vector does not equal number of rows in second vector.");
            }
            if (vector1.ColumnCount != vector2.ColumnCount)
            {
                throw new DimensionMismatchException("Number of columns in first vector does not equal number of columns in second vector.");
            }

            int distance = 0;
            for (var row = 0; row < vector1.RowCount; row++)
            {
                for (var col = 0; col < vector1.ColumnCount; col++)
                {
                    if (vector1.Data[row, col] != vector2.Data[row, col])
                    {
                        distance++;
                    }
                }
            }
            return distance;
        }

        public static MatrixInt GenerateIdentityMatrix(int size)
        {
            var rawResult = new int[size, size];
            for (var i = 0; i < size; i++)
            {
                rawResult[i, i] = 1;
            }
            var result = new MatrixInt(rawResult);
            return result;
        }
    }
}
