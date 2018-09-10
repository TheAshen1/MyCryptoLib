using McElieceCryptosystem.Interfaces;
using McElieceCryptosystem.Models;

namespace McElieceCryptosystem
{
    public class McElieceCryptosystem
    {
        public ILinearCode LinearCode { get; }

        public PrivateKey PrivateKey { get; }
        public PublicKey PublicKey { get; }

        public McElieceCryptosystem(ILinearCode linearCode, MatrixInt scramblerMatrix, MatrixInt permutationMatrix)
        {
            LinearCode = linearCode;
            PrivateKey = new PrivateKey
            {
                GeneratorMatrix = linearCode.GeneratorMatrix,
                ScramblerMatrix = scramblerMatrix,
                PermutationMatrix = permutationMatrix
            };

            PublicKey = new PublicKey
            {
                EncryptionMatrix = scramblerMatrix * linearCode.GeneratorMatrix * permutationMatrix % 2,
                ErrorVectorMaxWeight = linearCode.MaxErrors
            };
        }

        public MatrixInt EncryptMesaage(PublicKey publicKey, MatrixInt message, MatrixInt errorVector)
        {
            var result = publicKey.EncryptionMatrix * message + errorVector;
            return result;
        }

        public MatrixInt DecryptMessage(MatrixInt ciphertext)
        {
            var messageWithErrors = ciphertext * PrivateKey.PermutationMatrix.Transpose();
            var correctedMessage = LinearCode.CorrectErrors(messageWithErrors);
            var message = correctedMessage * PrivateKey.ScramblerMatrix.Transpose();
            return message;
        }
    }
}
