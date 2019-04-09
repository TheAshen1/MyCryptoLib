using System.Collections.Generic;

namespace CryptoSystems.Models
{
    public class PrivateKey
    {
		/// <summary>
        /// K rows N columns
        /// </summary>
        public MatrixInt GeneratorMatrix { get; set; }
        /// <summary>
        /// K rows K columns
        /// </summary>
        public MatrixInt ScramblerMatrix { get; set; }
        public MatrixInt InverseScramblerMatrix { get; set; }
        /// <summary>
        /// permutation matrix
        /// </summary>
        public IList<int> Permutation { get; set; }
        public IList<int> InversePermutation { get; set; }
        /// <summary>
        /// list of N elements of finite field which will be added to corresponding columns
        /// </summary>
        public IList<int> Mask { get; set; }
    }
}
