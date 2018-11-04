using McElieceCryptosystem.Interfaces;
using McElieceCryptosystem.Models;
using McElieceCryptosystem.Util;
using System;

namespace McElieceCryptosystem
{
    public class HammingCode : ILinearCode
    {
        #region Properties
        /// <summary>
        /// Total length of an encoded word of length K
        /// </summary>
        public int N { get; }
        /// <summary>
        /// Number of information digits in liner code
        /// </summary>
        public int K { get; }

        public int MinimumDistance { get; }

        public MatrixInt GeneratorMatrix { get; }

        public MatrixInt ParityCheckMatrix { get; }

        public int CanDetectUpTo => MinimumDistance - 1;

        public int CanCorrectUpTo => (MinimumDistance - 1) / 2;
        #endregion

        #region Constructors
        public HammingCode()
        {
            GeneratorMatrix = Constants.HammingCodeGeneratorMatrix;
            K = GeneratorMatrix.RowCount;
            N = GeneratorMatrix.ColumnCount;
            ParityCheckMatrix = GenerateParityCheckMatrix(GeneratorMatrix);
            MinimumDistance = CalculateMinimumDistance(GeneratorMatrix);
        }
        #endregion

        #region Public
        public MatrixInt Encode(MatrixInt message)
        {
            var encodedMessage = message * GeneratorMatrix % 2;
            return encodedMessage;
        }
        /// <summary>
        /// First K digits of a message are the information digits
        /// When digits from K to N are redundancy digits
        /// </summary>
        /// <param name="message">Message, encoded with same linear code</param>
        /// <returns></returns>
        public MatrixInt DecodeAndCorrect(MatrixInt message)
        {
            var syndromeMatrix = message * ParityCheckMatrix.Transpose() % 2;

            var rawMatrixData = message.Data;

            for (var row = 0; row < message.RowCount; row++)
            {
                var syndrome = syndromeMatrix.GetRow(row);
                for(var col = 0; col < ParityCheckMatrix.ColumnCount; col++)
                {
                    if(ParityCheckMatrix.GetColumn(col) == syndrome.Transpose())
                    {
                        rawMatrixData[row, col]++;
                    }
                }
            }
            var correctedMessage = new MatrixInt(rawMatrixData) % 2;
            var decodedMessage = correctedMessage.GetRangeOfColumns(new RangeInt(K));
            return decodedMessage;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// G = [I_k | P]
        /// H = [-P^-1 | I_n-k]^-1
        /// Negation is not nessesary on binary fields
        /// </summary>
        /// <returns></returns>
        private MatrixInt GenerateParityCheckMatrix(MatrixInt generatorMatrix)
        {
            var I_k = generatorMatrix.SubMatrix(new RangeInt(K));

            var P = generatorMatrix.SubMatrix(new RangeInt(K), new RangeInt(K, N));

            var H = P.Transpose() | Utility.GenerateIdentityMatrix(N - K);

            return H;
        }

        private int CalculateMinimumDistance(MatrixInt generatorMatrix)
        {
            int minimumDistance = generatorMatrix.ColumnCount;
            for (var row = 0; row < generatorMatrix.RowCount - 1; row++)
            {
                for (var anotherRow = row + 1; anotherRow < generatorMatrix.RowCount; anotherRow++)
                {
                    minimumDistance = Math.Min(
                        minimumDistance,
                        Utility.Distance(generatorMatrix.GetRow(row), generatorMatrix.GetRow(anotherRow))
                    );
                }
            }
            return minimumDistance;
        }

        public MatrixInt Encode(MatrixInt message, MatrixInt errorVector)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
