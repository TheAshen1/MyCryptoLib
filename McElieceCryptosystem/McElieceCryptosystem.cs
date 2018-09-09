using McElieceCryptosystem.Models;

namespace McElieceCryptosystem
{
    public class McElieceCryptosystem
    {
        private PrivateKey PrivateKey { get; }
        public PublicKey PublicKey { get; }

        public McElieceCryptosystem()
        {

        }

        public MatrixInt EncryptMesaage(PublicKey publicKey, MatrixInt message, MatrixInt errorVector)
        {
            var result = publicKey.EncryptionMatrix * message + errorVector;
            return result;
        }

        public MatrixInt DecryptMessage(MatrixInt ciphertext)
        {
            var messageWithErrors = ciphertext * PrivateKey.PermutationMatrix.Transpose();
            var correctedMessage = ErrorCorrection.Apply(messageWithErrors);
            var decodedMessage = Decoder.Decode(correctedMessage);
            var message = decodedMessage * PrivateKey.ScramblerMatrix.Transpose();
            return message;
        }
    }
}
