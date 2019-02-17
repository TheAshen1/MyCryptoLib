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
        /// <summary>
        /// list of N integers which correspond to numbers of columns
        /// </summary>
        public IList<int> Permutation { get; set; }
        /// <summary>
        /// list of N elements of finite field which will be added to corresponding columns
        /// </summary>
        public IList<int> Mask { get; set; }
    }
}
