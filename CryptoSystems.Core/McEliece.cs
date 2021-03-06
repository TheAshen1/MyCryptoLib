﻿using CryptoSystems.Algorithms;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using CryptoSystems.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CryptoSystems
{
    public class McEliece
    {
        public ILinearCode LinearCode { get; }

        public PrivateKey PrivateKey { get; }
        public PublicKey PublicKey { get; }

        public McEliece(ILinearCode linearCode, MatrixInt scramblerMatrix, IList<int> permutation, IList<int> mask)
        {
            LinearCode = linearCode;
            PrivateKey = new PrivateKey
            {
                GeneratorMatrix = linearCode.GeneratorMatrix,
                ScramblerMatrix = scramblerMatrix,
                Permutation = permutation,
                Mask = mask
            };

            var encryptionMatrix = MatrixAlgorithms.DotMultiplication(scramblerMatrix, linearCode.GeneratorMatrix, linearCode.GaloisField);
            Debug.WriteLine(encryptionMatrix);
            encryptionMatrix = encryptionMatrix.PermuteColumns(PrivateKey.Permutation);
            Debug.WriteLine(encryptionMatrix);
            for (int col = 0; col < encryptionMatrix.ColumnCount; col++)
            {
                for (int row = 0; row < encryptionMatrix.RowCount; row++)
                {
                    encryptionMatrix[row, col] = linearCode.GaloisField.MultiplyWords(encryptionMatrix[row, col], mask[col]);
                }
            }
            Debug.WriteLine(encryptionMatrix + 1);

            PublicKey = new PublicKey
            {
                EncryptionMatrix = encryptionMatrix,
                ErrorVectorMaxWeight = linearCode.MinimumDistance
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
            //Debug.WriteLine(message);

            #region Inverse permutation
            var inversePermutation = Helper.InversePermutation(PrivateKey.Permutation);
            message = message.PermuteColumns(inversePermutation);
            #endregion
            //Debug.WriteLine(message);

            #region Correct Errors
            var correctedMessage = LinearCode.DecodeAndCorrect(message);
            #endregion
            //Debug.WriteLine(correctedMessage);

            #region Apply the inverse scrambler matrix
            var inverseScramblerMatrix = MatrixAlgorithms.MatrixInverse(PrivateKey.ScramblerMatrix, LinearCode.GaloisField);
            var decryptedMessage = MatrixAlgorithms.DotMultiplication(correctedMessage, inverseScramblerMatrix, LinearCode.GaloisField);
            #endregion

            return decryptedMessage;
        }
    }
}
