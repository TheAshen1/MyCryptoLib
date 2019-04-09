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
        private readonly ParityCheckMatrixGeneratorEllyptic _generator;

        public McElieseEllyptic(int n, int k, int d, int t, GaloisField galoisField, MatrixInt scramblerMatrix, IList<int> permutation, IList<int> mask)
        {
            _galoisField = galoisField;
            _generator = new ParityCheckMatrixGeneratorEllyptic(2);
            LinearCode = new LinearCode(n, k, d, t, galoisField, _generator);

            MatrixInt parityCheckMatrix = null;
            MatrixInt generatorMatrix = null;
            while (true)
            {
                parityCheckMatrix = _generator.Generate(LinearCode);
                LinearCode.ParityCheckMatrix = parityCheckMatrix;
                Console.WriteLine(parityCheckMatrix);

                if (Helper.Weight(parityCheckMatrix) < Math.Ceiling(parityCheckMatrix.RowCount * parityCheckMatrix.ColumnCount * 0.7))
                {
                    continue;
                }

                try
                {
                    generatorMatrix = GeneratorMatrixCalculator.CalculateGeneratorMatrixAlt(LinearCode);
                    Console.WriteLine(generatorMatrix);
                }
                catch (LinearCodeException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

                if (IsGeneratorMatrixValid(parityCheckMatrix, generatorMatrix, galoisField))
                {
                    LinearCode.GeneratorMatrix = generatorMatrix;
                    break;
                }
            }


            PrivateKey = new PrivateKey
            {
                GeneratorMatrix = LinearCode.GeneratorMatrix,
                ScramblerMatrix = scramblerMatrix,
                InverseScramblerMatrix = MatrixAlgorithms.MatrixInverse(scramblerMatrix, galoisField),
                Permutation = permutation,
                InversePermutation = Helper.InversePermutation(permutation),
                Mask = mask,
            };

            var encryptionMatrix = MatrixAlgorithms.DotMultiplication(scramblerMatrix, generatorMatrix, LinearCode.GaloisField);
            Console.WriteLine(encryptionMatrix);
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
            message = message.PermuteColumns(PrivateKey.InversePermutation);

            #endregion
            Console.WriteLine(message);

            #region Correct Errors
            var correctedMessage = DecoderEllyptic.DecodeAndCorrect(LinearCode, message, _generator);
            #endregion
            Console.WriteLine(correctedMessage);

            #region Apply the inverse scrambler matrix
            var decryptedMessage = MatrixAlgorithms.DotMultiplication(correctedMessage, PrivateKey.InverseScramblerMatrix, LinearCode.GaloisField);
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
