using CryptoSystems.Exceptions;
using CryptoSystems.Models;
using System;
using System.Collections.Generic;

namespace CryptoSystems.Util
{
    public static class Utility
    {
        public static int Weight(MatrixInt vector)
        {
            var count = 0;

            if (vector.RowCount == 1 || vector.ColumnCount == 1)
            {
                foreach (var value in vector.Data)
                {
                    if (value != 0)
                    {
                        count++;
                    }
                }
                return count;
            }
            throw new DimensionMismatchException("Vector should consist of single row or column");
        }

        public static int Distance(MatrixInt vector1, MatrixInt vector2)
        {
            if (vector1.RowCount != vector2.RowCount)
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

        internal static MatrixInt CalculateGoppaPolynomialValue(MatrixInt goppaPolynomial, GaloisField galoisField, int wordNumber)
        {
            var x = galoisField.Field.GetColumn(wordNumber);
            var result = galoisField.Field.GetColumn(goppaPolynomial.Data[0, 0]);

            for (int col = 1; col < goppaPolynomial.ColumnCount; col++)
            {
                if (goppaPolynomial.Data[0, col] < 0)
                {
                    continue;
                }
                var word = galoisField.Field.GetColumn((goppaPolynomial.Data[0, col] + wordNumber * col) % (galoisField.Field.ColumnCount));
                result = result + word;
            }
            return result;
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

        public static MatrixInt GenerateScramblerMatrix(int size)
        {
            var result = GenerateScramblerMatrix(size, size);
            return result;
        }

        public static MatrixInt GenerateScramblerMatrix(int numberOfRows, int numberOfCols)
        {
            var rawResult = new int[numberOfRows, numberOfCols];
            var rand = new Random();
            for (var row = 0; row < numberOfRows; row++)
            {
                for (var col = 0; col < numberOfCols; col++)
                {
                    rawResult[row, col] = rand.Next(2);
                }
            }
            var result = new MatrixInt(rawResult);
            return result;
        }

        public static MatrixInt GenerateScramblerMatrix(int size, GaloisField galoisField, Random rand = null)
        {
            var result = GenerateScramblerMatrix(size, size, galoisField, rand);
            return result;
        }

        public static MatrixInt GenerateScramblerMatrix(int numberOfRows, int numberOfCols, GaloisField galoisField, Random rand = null)
        {
            if (rand is null)
            {
                rand = new Random();
            }

            var rawResult = new int[numberOfRows, numberOfCols];
            for (var row = 0; row < numberOfRows; row++)
            {
                for (var col = 0; col < numberOfCols; col++)
                {
                    rawResult[row, col] = rand.Next(galoisField.WordCount) - 1;
                }
            }
            var result = new MatrixInt(rawResult);
            return result;
        }


        public static MatrixInt GeneratePermutationMatrix(int size)
        {
            var permutationMatrix = GenerateIdentityMatrix(size);
            var rand = new Random();
            for (var col = 0; col < permutationMatrix.ColumnCount; col++)
            {
                permutationMatrix.SwapColumns(col, rand.Next(col, permutationMatrix.ColumnCount));
            }
            return permutationMatrix;
        }

        public static int BinaryToDecimal(MatrixInt binaryVector)
        {
            if (binaryVector.RowCount > 1)
            {
                throw new DimensionMismatchException("Method BinaryToDecimal accepts only binaty vectors");
            }

            int result = 0;

            for (var col = 0; col < binaryVector.ColumnCount; col++)
            {
                if (binaryVector.Data[0, col] != 0)
                {
                    result += 1 << col;
                }
            }

            return result;
        }

        public static MatrixInt DecimalToBinary(int number)
        {
            var rawResult = new List<int>();

            while (number >= 1)
            {
                rawResult.Add(number % 2);
                number = number >> 1;
            }
            var result = new MatrixInt(rawResult);
            return result;
        }

        public static List<int> InversePermutation(List<int> permutation)
        {
            var inverse = new List<int>(permutation);
            for (int i = 0; i < permutation.Count; i++)
            {
                inverse[permutation[i]] = i;
            }
            return inverse;
        }

        //public static MatrixInt ShiftRight(MatrixInt matrix, int shiftLength = 1)
        //{
        //    var rawResult = new int[matrix.RowCount, matrix.ColumnCount + shiftLength];

        //    for (int row = 0; row < matrix.RowCount; row++)
        //    {
        //        for (int col = 0; col < matrix.ColumnCount; col++)
        //        {
        //            rawResult[row, col + shiftLength] = matrix.Data[row, col];
        //        }
        //    }

        //    var result = new MatrixInt(rawResult);
        //    return result;
        //}

        //public static MatrixInt BinaryPolinomialModuloOperation(MatrixInt matrix, MatrixInt modulus)
        //{

        //    if (matrix.RowCount != modulus.RowCount)
        //    {
        //        throw new DimensionMismatchException("The number of rows in matrix does not equal the number of rows in modulus");
        //    }

        //    if (matrix.ColumnCount < modulus.ColumnCount)
        //    {
        //        throw new DimensionMismatchException("The number of columns in matrix has to be less than the number of columns in modulus");
        //    }

        //    var rawResult = matrix.Data;
        //    for (var row = 0; row < matrix.RowCount; row++)
        //    {
        //        for (int col = 0; col < matrix.ColumnCount; col++)
        //        {
        //            rawResult[row, col] = rawResult[row, col] + modulus.Data[row, col] % 2;
        //        }
        //    }

        //    var result = new MatrixInt(new int[1, 1]);
        //    return result;
        //}

        public static PolynomialInt ZeroPolynomialInt()
        {
            return new PolynomialInt(0);
        }

        public static PolynomialDouble ZeroPolynomialDouble()
        {
            return new PolynomialDouble(0);
        }
    }
}
