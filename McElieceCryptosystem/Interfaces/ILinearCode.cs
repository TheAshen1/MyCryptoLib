using McElieceCryptosystem.Models;

namespace McElieceCryptosystem.Interfaces
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
        int K { get; }

        /// <summary>
        /// Max amount of errors code can correct
        /// </summary>
        int MinimumDistance { get; }

        int CanDetectUpTo { get; }

        int CanCorrectUpTo { get; }

        /// <summary>
        /// Generator matrix of a linear code with K rows and N columns
        /// </summary>
        MatrixInt GeneratorMatrix { get; }

        /// <summary>
        /// Parity check matrix of a linear code 
        /// </summary>
        MatrixInt ParityCheckMatrix { get; }
        #endregion

        #region Methods
        MatrixInt Encode(MatrixInt message);
        MatrixInt Encode(MatrixInt message, MatrixInt errorVector);
        MatrixInt DecodeAndCorrect(MatrixInt message);
        #endregion

    }
}
