namespace CryptoSystems.Models
{
    public class PublicKey
    {
        public MatrixInt EncryptionMatrix { get; set; }
        public int ErrorVectorMaxWeight { get; set; }
    }
}
