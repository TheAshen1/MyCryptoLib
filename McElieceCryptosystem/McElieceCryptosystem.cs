using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using System.Collections.Generic;

namespace CryptoSystems
{
    public class McElieceCryptosystem
    {
        public ILinearCode LinearCode { get; }

        public PrivateKey PrivateKey { get; }
        public PublicKey PublicKey { get; }

        public McElieceCryptosystem(ILinearCode linearCode, MatrixInt scramblerMatrix, List<int> permutations, List<int> mask)
        {
            LinearCode = linearCode;
            PrivateKey = new PrivateKey
            {
                GeneratorMatrix = linearCode.GeneratorMatrix,
                ScramblerMatrix = scramblerMatrix,
                Permutations = permutations,
                Mask = mask
            };

            PublicKey = new PublicKey
            {
                EncryptionMatrix = scramblerMatrix * linearCode.GeneratorMatrix * permutationMatrix % 2,
                ErrorVectorMaxWeight = linearCode.MinimumDistance
            };
        }

        public MatrixInt EncryptMesaage(PublicKey publicKey, MatrixInt message, MatrixInt errorVector)
        {
            var result = (message * publicKey.EncryptionMatrix + errorVector) % 2;
            return result;
        }

        public MatrixInt DecryptMessage(MatrixInt encryptedMessage)
        {
            var messageWithErrors = encryptedMessage * PrivateKey.PermutationMatrix.Transpose();
            var correctedMessage = LinearCode.DecodeAndCorrect(messageWithErrors);
            var message = correctedMessage * PrivateKey.ScramblerMatrix.Transpose();
            return message;
        }
    }
}
