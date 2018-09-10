using McElieceCryptosystem.Interfaces;
using McElieceCryptosystem.Models;
using System;

namespace McElieceCryptosystem.LinearCodes
{
    public class HammingCode : ILinearCode
    {
        #region Properties
        public int N { get; }

        public int K { get; }

        public int MaxErrors { get; }

        public MatrixInt GeneratorMatrix { get; }

        public MatrixInt ParityCheckMatrix { get; }
        #endregion

        #region Constructors
        public HammingCode(int fieldPower)
        {

        }
        #endregion

        #region Public Methods
        public MatrixInt CorrectErrors(MatrixInt message)
        {
            throw new NotImplementedException();
        }

        public bool ParityCheck()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// G = [I_k | P]
        /// H = [-P^-1 | I_n-k]
        /// Negation is not nessesary on binary fields
        /// </summary>
        /// <returns></returns>
        private MatrixInt GenerateParityCheckMatrix()
        {
            var I_k = GeneratorMatrix.SubMatrix(new RangeInt(K));

            var P = GeneratorMatrix.SubMatrix(new RangeInt(K), new RangeInt(K, N));

            var H = P.Transpose() | Utility.GenerateIdentityMatrix(N-K);

            return H;
        }
        #endregion
    }
}
