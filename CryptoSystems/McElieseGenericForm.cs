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
    public static class McElieseGenericForm
    {
        public static MatrixInt Encrypt(ILinearCode linearCode, MatrixInt scramblerMatrix, IList<int> permutation, IList<int> mask, ParityCheckMatrixGeneratorEllyptic generator, MatrixInt message, MatrixInt errorVector)
        {

            var encryptionMatrix = MatrixAlgorithms.DotMultiplication(scramblerMatrix, linearCode.GeneratorMatrix, linearCode.GaloisField);
            Console.WriteLine(encryptionMatrix);
            encryptionMatrix = encryptionMatrix.PermuteColumns(permutation);
            Console.WriteLine(encryptionMatrix);
            for (int col = 0; col < encryptionMatrix.ColumnCount; col++)
            {
                for (int row = 0; row < encryptionMatrix.RowCount; row++)
                {
                    encryptionMatrix[row, col] = linearCode.GaloisField.MultiplyWords(encryptionMatrix[row, col], mask[col]);
                }
            }
            Console.WriteLine(encryptionMatrix);

            var encryptedMessage = MatrixAlgorithms.DotMultiplication(message, encryptionMatrix, linearCode.GaloisField);
            for (int i = 0; i < encryptedMessage.ColumnCount; i++)
            {
                encryptedMessage[0, i] = linearCode.GaloisField.AddWords(encryptedMessage[0, i], errorVector[0, i]);
            }
            return encryptedMessage;
        }

        public static MatrixInt Decrypt(ILinearCode linearCode, IList<int> permutation, IList<int> mask, MatrixInt scramblerMatrix, ParityCheckMatrixGeneratorEllyptic generator, MatrixInt encryptedMessage)
        {
            var message = encryptedMessage.Clone();
            #region Unmask
            for (int col = 0; col < message.ColumnCount; col++)
            {
                for (int row = 0; row < message.RowCount; row++)
                {
                    message[row, col] = linearCode.GaloisField.MultiplyWords(message[row, col], linearCode.GaloisField.GetMultiplicativeInverse(mask[col]));
                }
            }
            #endregion
            Console.WriteLine(message);

            #region Inverse permutation
            var inversePermutation = Helper.InversePermutation(permutation);
            message = message.PermuteColumns(inversePermutation);
            #endregion
            Console.WriteLine(message);

            #region Correct Errors
            var correctedMessage = DecoderEllyptic.DecodeAndCorrect(linearCode, message, generator);
            #endregion
            Console.WriteLine(correctedMessage);

            #region Apply the inverse scrambler matrix
            var inverseScramblerMatrix = MatrixAlgorithms.MatrixInverse(scramblerMatrix, linearCode.GaloisField);
            var decryptedMessage = MatrixAlgorithms.DotMultiplication(correctedMessage, inverseScramblerMatrix, linearCode.GaloisField);
            #endregion

            return decryptedMessage;
        }
    }
}
