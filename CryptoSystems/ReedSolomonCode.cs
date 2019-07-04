using CryptoSystems.Algorithms;
using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using CryptoSystems.Util;
using System;
using System.Diagnostics;

namespace CryptoSystems
{
    public class ReedSolomonCode : ILinearCode
    {
        #region Properties
        public int N => GaloisField.WordCount;
        public int K => GaloisField.WordLength;
        public int T => (MinimumDistance - 1) / 2;

        public int MinimumDistance => N - K + 1;
        public int CanDetectUpTo => MinimumDistance - 1;


        public GaloisField GaloisField { get; }
        public MatrixInt ParityCheckMatrix { get; set; }
        public MatrixInt GeneratorMatrix { get; set; }

        public int D => throw new NotImplementedException();
        #endregion

        public ReedSolomonCode(GaloisField galoisField, IParityCheckMatrixGenerator parityCheckMatrixGenerator)
        {
            GaloisField = galoisField;

            while (true)
            {
                ParityCheckMatrix = parityCheckMatrixGenerator.Generate(this);
                Debug.WriteLine(ParityCheckMatrix);

                if (Helper.Weight(ParityCheckMatrix) < Math.Ceiling(ParityCheckMatrix.RowCount * ParityCheckMatrix.ColumnCount * 0.7))
                {
                    continue;
                }

                try
                {
                    GeneratorMatrix = GeneratorMatrixCalculator.CalculateGeneratorMatrix(this);
                    Debug.WriteLine(GeneratorMatrix);
                }
                catch (LinearCodeException ex)
                {
                    Debug.WriteLine(ex.Message);
                    continue;
                }

                if (IsGeneratorMatrixValid(ParityCheckMatrix, GeneratorMatrix, GaloisField))
                {
                    break;
                }
            }
        }

        public ReedSolomonCode(GaloisField galoisField, MatrixInt parityCheckMatrix)
        {
            GaloisField = galoisField;
            ParityCheckMatrix = parityCheckMatrix;
            GeneratorMatrix = GeneratorMatrixCalculator.CalculateGeneratorMatrix(this);
            if (!IsGeneratorMatrixValid(ParityCheckMatrix, GeneratorMatrix, GaloisField))
            {
                throw new LinearCodeException("Could not produce correct Generator matrix from provided ParityCheck matrix.");
            }
        }

        public MatrixInt DecodeAndCorrect(MatrixInt message)
        {
            return Decoder.DecodeAndCorrect(this, message);
        }

        public MatrixInt Encode(MatrixInt message)
        {
            return MatrixAlgorithms.DotMultiplication(message, GeneratorMatrix, GaloisField);
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

        private bool IsGeneratorMatrixValid(MatrixInt generatorMatrix, MatrixInt parityCheckMatrix, GaloisField galoisField)
        {
            var result = MatrixAlgorithms.DotMultiplication(generatorMatrix, parityCheckMatrix.Transpose(), galoisField);
            return Helper.Weight(result) == 0;
        }
    }
}
