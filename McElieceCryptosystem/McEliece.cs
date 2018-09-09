namespace McElieceCryptosystem
{
    public class McElieceCryptosystem
    {
        /// <summary>
        /// Dimension of linear code
        /// K = N - MT
        /// </summary>
        public int K { get; private set; }
        /// <summary>
        /// N = 2^M
        /// </summary>
        public int N { get; private set; }
        /// <summary>
        /// Number of error code will correct
        /// </summary>
        public int T { get; private set; }
        /// <summary>
        /// Field power
        /// GF(2^M)
        /// </summary>
        public int M { get; private set; }


        public int PublicKey { get; set; }


        public void EncryptMesaage()
        {

        }

        public void DecryptMessage()
        {

        }
    }
}
