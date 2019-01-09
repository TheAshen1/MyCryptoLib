using CryptoSystems.Models;

namespace CryptoSystems.Interfaces
{
    public interface ILinearCode
    {
        #region Properties
        /// <summary>
        /// Length of a codeword 
        /// </summary>
        int N { get; }

        /// <summary>
        /// Number of information bits
        /// </summary>
        int K { get;}

        /// <summary>
        /// Error correction capability
        /// </summary>
        int T { get; }

        int MinimumDistance { get; }

        /// <summary>
        /// Error detection capability
        /// </summary>
        int CanDetectUpTo { get; }


        /// <summary>
        /// Generator matrix of a linear code with K rows and N columns
        /// </summary>
        MatrixInt GeneratorMatrix { get; }

        /// <summary>
        /// Parity check matrix of a linear code N - K rows and N columns
        /// </summary>
        MatrixInt ParityCheckMatrix { get; }

        GaloisField GaloisField { get; }
        #endregion

        #region Methods
        MatrixInt Encode(MatrixInt message);
        MatrixInt Encode(MatrixInt message, MatrixInt errorVector);
        MatrixInt DecodeAndCorrect(MatrixInt message);
        #endregion

    }
}
