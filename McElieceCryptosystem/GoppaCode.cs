using McElieceCryptosystem.Interfaces;
using McElieceCryptosystem.Models;
using McElieceCryptosystem.Util;
using System;
using System.Collections.Generic;

namespace McElieceCryptosystem
{
    public class GoppaCode : ILinearCode
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
        /// <summary>
        /// Max amount of errors code can correct
        /// </summary>
        public int MinimumDistance { get; }

        public MatrixInt GeneratorMatrix { get; }

        public MatrixInt ParityCheckMatrix { get; }

        public int CanDetectUpTo => MinimumDistance - 1;

        public int CanCorrectUpTo => (MinimumDistance - 1) / 2;
        #endregion

        #region Constructors
        public GoppaCode(MatrixInt generatorMatrix)
        {
            GeneratorMatrix = generatorMatrix;
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
            var actualMessage = message.GetRangeOfColumns(new RangeInt(K)) % 2;
            var syndromeMatrix = message * ParityCheckMatrix % 2;
            //var correctedMessage = ;

            return syndromeMatrix;
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

            var parityCheckMatrix = H.Transpose();

            return parityCheckMatrix;
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

        private MatrixInt CMLD(MatrixInt encodedMessage)
        {
            for (var row = 0; row < encodedMessage.RowCount; row++)
            {
                var encodedMessageRow = encodedMessage.GetRow(row);

                int minimumDistance = GeneratorMatrix.ColumnCount;
                int mostLikelyCodewordNumber = 0;
                for (var codewordNumber = 0; codewordNumber < GeneratorMatrix.RowCount; codewordNumber++)
                {
                    int distance = Utility.Distance(encodedMessageRow, GeneratorMatrix.GetRow(codewordNumber));
                    if (distance < minimumDistance)
                    {
                        minimumDistance = distance;
                        mostLikelyCodewordNumber = codewordNumber;
                    }
                }
            }

            throw new NotImplementedException();
        }

        private MatrixInt PattersonAlgorithm()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
