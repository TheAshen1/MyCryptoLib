using CryptoSystems.Algorithms;
using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;

namespace CryptoSystems
{
    public class ReedSolomonCode : ILinearCode
    {
        #region Properties
        public int N => GaloisField.WordCount;
        public int K => GaloisField.WordLength;
        public int T => (MinimumDistance - 1) / 2;

        public GaloisField GaloisField { get; }

        public MatrixInt ParityCheckMatrix { get; }
        public MatrixInt GeneratorMatrix { get; }

        public int MinimumDistance => N - K + 1;
        public int CanDetectUpTo => MinimumDistance - 1;
        #endregion

        public ReedSolomonCode(GaloisField galoisField)
        {
            GaloisField = galoisField;
            ParityCheckMatrix = ParityCheckMatrixGeneratorGeneric.GenerateParityCheckMatrix(galoisField, T);
            GeneratorMatrix = GeneratorMatrixCalculator.CalculateGeneratorMatrix(N, K, ParityCheckMatrix, galoisField);
        }

        public ReedSolomonCode(GaloisField galoisField, MatrixInt parityCheckMatrix)
        {
            GaloisField = galoisField;
            ParityCheckMatrix = parityCheckMatrix;
            GeneratorMatrix = GeneratorMatrixCalculator.CalculateGeneratorMatrix(N, K, ParityCheckMatrix, galoisField);
        }

        public MatrixInt DecodeAndCorrect(MatrixInt message)
        {
            return PetersonDecoder.DecodeAndCorrect(this, message);
        }

        public MatrixInt Encode(MatrixInt message)
        {
            var encodedMessage = MatrixAlgorithms.DotMultiplication(message, GeneratorMatrix, GaloisField);
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
                rawResult[0, i] = GaloisField.AddWords(rawResult[0, i], errorVector.Data[0, i]);
            }

            return encodedMessage;
        }
    }
}
