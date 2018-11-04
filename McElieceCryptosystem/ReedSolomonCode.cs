using McElieceCryptosystem.Exceptions;
using McElieceCryptosystem.Interfaces;
using McElieceCryptosystem.Models;
using System;

namespace McElieceCryptosystem
{
    public class ReedSolomonCode : ILinearCode
    {
        #region Properties
        public GaloisField GaloisField { get; }
        /// <summary>
        /// Total length of an encoded word of length K
        /// </summary>
        public int N => GaloisField.WordCount;
        /// <summary>
        /// Number of information digits in liner code
        /// </summary>
        public int K => GaloisField.WordLength;

        public int MinimumDistance { get; }

        public int CanDetectUpTo => MinimumDistance - 1;

        public int CanCorrectUpTo => (MinimumDistance - 1) / 2;

        public MatrixInt GeneratorMatrix { get; }

        public MatrixInt ParityCheckMatrix { get; }
        #endregion


        public ReedSolomonCode(GaloisField galoisField)
        {
            GaloisField = galoisField;

            MinimumDistance = N - K + 1;

            var rawParityCheckMatrix = GenerateParityCheckMatrix(galoisField.WordCount, CanCorrectUpTo);

            var rawGeneratorMatrix = CalculateGeneratorMatrix(N, K, rawParityCheckMatrix, galoisField);

            ParityCheckMatrix = rawParityCheckMatrix ;
            GeneratorMatrix = rawGeneratorMatrix;
        }



        public MatrixInt DecodeAndCorrect(MatrixInt message)
        {
            throw new NotImplementedException();
        }

        public MatrixInt Encode(MatrixInt message)
        {
            if (message.ColumnCount != K)
            {
                throw new DimensionMismatchException("Number of columns in first matrix does not equal number of rows in second matrix.");
            }

            var rowCount = message.RowCount;
            var columnCount = GeneratorMatrix.ColumnCount;
            var rawEncodedMessage = new int[rowCount, columnCount];

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    int sum = -1;
                    for (int k = 0; k < message.ColumnCount; k++)
                    {
                        var product = GaloisField.MultiplyWords(message.Data[row, k], GeneratorMatrix.Data[k, col]);
                        sum = GaloisField.SumWords(sum, product);
                    }
                    rawEncodedMessage[row, col] = sum;
                }
            }

            var encodedMessage = new MatrixInt(rawEncodedMessage);
            return encodedMessage;
        }

        public MatrixInt Encode(MatrixInt message, MatrixInt errorVector)
        {
            if (errorVector.ColumnCount != N)
            {
                throw new DimensionMismatchException("Number of values in the mesage matrix does not equal number of values in the error vector.");
            }

            var encodedMessage = Encode(message);
            var rawResult = encodedMessage.Data;

            for (int i = 0; i < encodedMessage.ColumnCount; i++)
            {
                rawResult[0, i] = GaloisField.SumWords(rawResult[0, i], errorVector.Data[0, i]); 
            }

            return encodedMessage;
        }

        #region Private Methods
        /// <summary>
        /// G = [I_k | P]
        /// H = [-P^-1 | I_n-k]^-1
        /// Negation is not nessesary on binary fields
        /// </summary>
        /// <returns></returns>
        private MatrixInt GenerateParityCheckMatrix(int galoisFieldWordCount, int errorCorrectionLimit)
        {
            var rows = 2 * errorCorrectionLimit;
            var rawResult = new int[rows, galoisFieldWordCount];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < galoisFieldWordCount; col++)
                {
                    rawResult[row, col] = (row * col) % galoisFieldWordCount;
                }
            }

            var H = new MatrixInt(rawResult);
            return H;
        }

        /// <summary>
        /// G * H^T = 0
        /// </summary>
        /// <param name="n">Length of an encoded message</param>
        /// <param name="k">Number of information bits in message</param>
        /// <param name="parityCheckMatrix">H</param>
        /// <returns></returns>
        private MatrixInt CalculateGeneratorMatrix(int n, int k, MatrixInt parityCheckMatrix, GaloisField galoisField)
        {
            var rawResult = new int[k, n];
            #region Init
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if (i == j)
                    {
                        rawResult[i, j] = 0;
                    }
                    else
                    {
                        rawResult[i, j] = -1;
                    }
                }
            }
            #endregion

            #region Solve k systems of linear equasions on field elements
            for (int i = 0; i < k; i++)
            {
                var system = parityCheckMatrix.GetRangeOfColumns(new RangeInt(k, n)) | parityCheckMatrix.GetColumn(i);
                Console.WriteLine(system);
                var result = Solve(system, galoisField);

                #region CopyResults
                for (int row = 0; row < result.RowCount; row++)
                {
                    rawResult[i, row + k] = result.Data[row, result.ColumnCount - 1];
                }
                #endregion
            }
            #endregion

            var generatorMatrix = new MatrixInt(rawResult);
            Console.WriteLine(generatorMatrix);
            return generatorMatrix;
        }

        private MatrixInt Solve(MatrixInt matrix, GaloisField galoisField)
        {
            var rawResult = matrix.Data;

            var leadColumn = 0; // lead column
            for (int leadRow = 0; (leadRow + 1) < matrix.RowCount; leadRow++)
            {
                #region Make leading diagonal word 0
                if (rawResult[leadRow, leadColumn] != 0)
                {
                    var targetWordNumber = rawResult[leadRow, leadColumn];

                    for (int col = leadColumn; col < matrix.ColumnCount; col++)
                    {
                        rawResult[leadRow, col] = galoisField.DivideWords(rawResult[leadRow, col], targetWordNumber);
                    }
                }
                #endregion


                #region subtract from all other rows 
                //subtraction is basicy the same as summation here

                for (int i = (leadRow + 1); i < matrix.RowCount; i++)
                {
                    var otherRowLeadingValue = rawResult[i, leadColumn];
                    for (int col = leadColumn; col < matrix.ColumnCount; col++)
                    {
                        var wordToSubtract = galoisField.MultiplyWords(otherRowLeadingValue, rawResult[leadRow, col]);
                        rawResult[i, col] = galoisField.SumWords(wordToSubtract, rawResult[i, col]);
                    }
                    //Console.WriteLine(new MatrixInt(rawResult));
                }
                #endregion

                leadColumn++;
            }

            #region Backwards        
            for (int leadRow = (matrix.RowCount - 1); leadRow > 0; leadRow--)
            {

                rawResult[leadRow, matrix.ColumnCount - 1] = galoisField.DivideWords(rawResult[leadRow, matrix.ColumnCount - 1], rawResult[leadRow, leadColumn]);
                rawResult[leadRow, leadColumn] = 0;

                //Console.WriteLine(new MatrixInt(rawResult));

                for (int row = (leadRow - 1); row >= 0; row--)
                {
                    var wordToSubtract = galoisField.MultiplyWords(rawResult[row, leadColumn], rawResult[leadRow, matrix.ColumnCount - 1]);

                    rawResult[row, matrix.ColumnCount - 1] = galoisField.SumWords(wordToSubtract, rawResult[row, matrix.ColumnCount - 1]);
                    rawResult[row, leadColumn] = -1;
                }

                //Console.WriteLine(new MatrixInt(rawResult));

                leadColumn--;
            }
            #endregion

            var result = new MatrixInt(rawResult).GetColumn(matrix.ColumnCount - 1);
            return result;
        }
        #endregion
    }
}
