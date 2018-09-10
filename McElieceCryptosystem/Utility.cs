using McElieceCryptosystem.Exceptions;
using McElieceCryptosystem.Models;

namespace McElieceCryptosystem
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
