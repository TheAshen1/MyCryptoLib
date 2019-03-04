using CryptoSystems.Algorithms;
using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;
using CryptoSystems.Utility;
using System;
using System.Collections.Generic;

namespace CryptoSystems
{
    public class McElieseEllyptic
    {
        public ILinearCode LinearCode { get; }

        public PrivateKey PrivateKey { get; }
        public PublicKey PublicKey { get; }

        private readonly GaloisField _galoisField;
        private readonly List<Point> _points;
        private MatrixInt _generatorMatrix;
        private readonly MatrixInt _parityCheckMatrix;


        public McElieseEllyptic(int n, int k, int d, int t, GaloisField galoisField, MatrixInt scramblerMatrix, IList<int> permutation, IList<int> mask)
        {
            _galoisField = galoisField;
            var generator = new ParityCheckMatrixGeneratorEllyptic(2);
            LinearCode = new LinearCode(n, k, d, t, galoisField, generator);


            while (true)
            {
                _parityCheckMatrix = generator.Generate(LinearCode);
                LinearCode.ParityCheckMatrix = _parityCheckMatrix;
                Console.WriteLine(_parityCheckMatrix);

                if (Helper.Weight(_parityCheckMatrix) < Math.Ceiling(_parityCheckMatrix.RowCount * _parityCheckMatrix.ColumnCount * 0.7))
                {
                    continue;
                }

                try
                {
                    _generatorMatrix = GeneratorMatrixCalculator.CalculateGeneratorMatrixAlt(LinearCode);
                    Console.WriteLine(_generatorMatrix);
                }
                catch (LinearCodeException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

                if (IsGeneratorMatrixValid(_parityCheckMatrix, _generatorMatrix, galoisField))
                {
                    LinearCode.GeneratorMatrix = _generatorMatrix;
                    _points = generator.Points;
                    break;
                }
            }


            PrivateKey = new PrivateKey
            {
                GeneratorMatrix = LinearCode.GeneratorMatrix,
                ScramblerMatrix = scramblerMatrix,
                Permutation = permutation,
                Mask = mask
            };

            var encryptionMatrix = MatrixAlgorithms.DotMultiplication(scramblerMatrix, _generatorMatrix, LinearCode.GaloisField);
            Console.WriteLine(encryptionMatrix);
            //encryptionMatrix = MatrixAlgorithms.DotMultiplication(encryptionMatrix, PrivateKey.Permutation, LinearCode.GaloisField);
            encryptionMatrix = encryptionMatrix.PermuteColumns(PrivateKey.Permutation);
            Console.WriteLine(encryptionMatrix);
            for (int col = 0; col < encryptionMatrix.ColumnCount; col++)
            {
                for (int row = 0; row < encryptionMatrix.RowCount; row++)
                {
                    encryptionMatrix[row, col] = LinearCode.GaloisField.MultiplyWords(encryptionMatrix[row, col], mask[col]);
                }
            }
            Console.WriteLine(encryptionMatrix);

            PublicKey = new PublicKey
            {
                EncryptionMatrix = encryptionMatrix,
                //ErrorVectorMaxWeight = LinearCode.MinimumDistance
            };
        }

        public MatrixInt EncryptMessage(PublicKey publicKey, MatrixInt message, MatrixInt errorVector)
        {
            var encryptedMessage = MatrixAlgorithms.DotMultiplication(message, publicKey.EncryptionMatrix, LinearCode.GaloisField);
            for (int i = 0; i < encryptedMessage.ColumnCount; i++)
            {
                encryptedMessage[0, i] = LinearCode.GaloisField.AddWords(encryptedMessage[0, i], errorVector[0, i]);
            }
            return encryptedMessage;
        }

        public MatrixInt DecryptMessage(MatrixInt encryptedMessage)
        {
            var message = encryptedMessage.Clone();
            #region Unmask
            for (int col = 0; col < message.ColumnCount; col++)
            {
                for (int row = 0; row < message.RowCount; row++)
                {
                    message[row, col] = LinearCode.GaloisField.MultiplyWords(message[row, col], LinearCode.GaloisField.GetMultiplicativeInverse(PrivateKey.Mask[col]));
                }
            }
            #endregion
            Console.WriteLine(message);

            #region Inverse permutation
            //message = MatrixAlgorithms.DotMultiplication(message, PrivateKey.Permutation.Transpose(), LinearCode.GaloisField);
            var inverse = Helper.InversePermutation(PrivateKey.Permutation);
            message = message.PermuteColumns(inverse);

            #endregion
            Console.WriteLine(message);

            #region Correct Errors
            var correctedMessage = DecoderEllyptic.DecodeAndCorrect(LinearCode, message, _points);
            #endregion
            Console.WriteLine(correctedMessage);

            #region Apply the inverse scrambler matrix
            var inverseScramblerMatrix = MatrixAlgorithms.MatrixInverse(PrivateKey.ScramblerMatrix, LinearCode.GaloisField);
            var decryptedMessage = MatrixAlgorithms.DotMultiplication(correctedMessage, inverseScramblerMatrix, LinearCode.GaloisField);
            #endregion

            return decryptedMessage;
        }

        private bool IsGeneratorMatrixValid(MatrixInt generatorMatrix, MatrixInt parityCheckMatrix, GaloisField galoisField)
        {
            var result = MatrixAlgorithms.DotMultiplication(generatorMatrix, parityCheckMatrix.Transpose(), galoisField);
            return Helper.Weight(result) == 0;
        }
    }
}
