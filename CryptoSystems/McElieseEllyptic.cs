using CryptoSystems.Algorithms;
using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;
using CryptoSystems.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CryptoSystems
{
    public class McElieseEllyptic
    {
        public ILinearCode LinearCode { get; }

        public PrivateKey PrivateKey { get; }
        public PublicKey PublicKey { get; }

        private readonly ParityCheckMatrixGeneratorEllyptic _generator;

        public McElieseEllyptic(int n, int k, int d, int t, GaloisField galoisField, MatrixInt scramblerMatrix = null, IList<int> permutation = null, IList<int> mask = null)
        {
            _generator = new ParityCheckMatrixGeneratorEllyptic(2);
            LinearCode = new LinearCode(n, k, d, t, galoisField);

            MatrixInt parityCheckMatrix = null;
            MatrixInt generatorMatrix = null;
            while (true)
            {
                parityCheckMatrix = _generator.Generate(LinearCode);
                LinearCode.ParityCheckMatrix = parityCheckMatrix;
                Debug.WriteLine(parityCheckMatrix);

                var minValueFillPercentage = 0.7;

                if (Helper.Weight(parityCheckMatrix) < Math.Ceiling(parityCheckMatrix.RowCount * parityCheckMatrix.ColumnCount * minValueFillPercentage))
                {
                    continue;
                }

                try
                {
                    generatorMatrix = GeneratorMatrixCalculator.CalculateGeneratorMatrixAlt(LinearCode);
                    Debug.WriteLine(generatorMatrix);
                }
                catch (LinearCodeException ex)
                {
                    Debug.WriteLine(ex.Message);
                    continue;
                }

                if (IsGeneratorMatrixValid(parityCheckMatrix, generatorMatrix, galoisField))
                {
                    LinearCode.GeneratorMatrix = generatorMatrix;
                    break;
                }
            }
            if (scramblerMatrix is null)
            {
                scramblerMatrix = Helper.GenerateScramblerMatrix(LinearCode.K);
                while (true)
                {
                    try
                    {
                        MatrixAlgorithms.MatrixInverse(scramblerMatrix, galoisField);
                        break;
                    }
                    catch (SolveMatrixException)
                    {
                        Debug.WriteLine("Reattempting to generate scrambler matrix");
                    }
                }
            }
            if (permutation is null)
            {
                permutation = Helper.GeneratePermutaionList(LinearCode.N);
            }
            if (mask is null)
            {
                mask = Helper.GenerateMask(LinearCode.N, LinearCode.GaloisField);
            }
            var inverseMask = new List<int>(LinearCode.N);
            for (int i = 0; i < LinearCode.N; i++)
            {
                inverseMask.Add(LinearCode.GaloisField.GetMultiplicativeInverse(mask[i]));
            }
            PrivateKey = new PrivateKey
            {
                GeneratorMatrix = LinearCode.GeneratorMatrix,
                ScramblerMatrix = scramblerMatrix,
                InverseScramblerMatrix = MatrixAlgorithms.MatrixInverse(scramblerMatrix, galoisField),
                Permutation = permutation,
                InversePermutation = Helper.InversePermutation(permutation),
                Mask = mask,
                InverseMask = inverseMask
            };

            var encryptionMatrix = MatrixAlgorithms.DotMultiplication(PrivateKey.ScramblerMatrix, generatorMatrix, LinearCode.GaloisField);
            Debug.WriteLine(encryptionMatrix);
            encryptionMatrix = encryptionMatrix.PermuteColumns(PrivateKey.Permutation);
            Debug.WriteLine(encryptionMatrix);
            for (int col = 0; col < encryptionMatrix.ColumnCount; col++)
            {
                for (int row = 0; row < encryptionMatrix.RowCount; row++)
                {
                    encryptionMatrix[row, col] = LinearCode.GaloisField.MultiplyWords(encryptionMatrix[row, col], PrivateKey.Mask[col]);
                }
            }
            Debug.WriteLine(encryptionMatrix);

            PublicKey = new PublicKey
            {
                EncryptionMatrix = encryptionMatrix,
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
                    message[row, col] = LinearCode.GaloisField.MultiplyWords(message[row, col], PrivateKey.InverseMask[col]);
                }
            }
            #endregion
            Debug.WriteLine(message);

            #region Inverse permutation
            message = message.PermuteColumns(PrivateKey.InversePermutation);

            #endregion
            Debug.WriteLine(message);

            #region Correct Errors
            var correctedMessage = DecoderEllyptic.DecodeAndCorrect(LinearCode, message, _generator);
            #endregion
            Debug.WriteLine(correctedMessage);

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
