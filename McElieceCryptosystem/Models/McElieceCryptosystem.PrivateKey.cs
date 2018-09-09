namespace McElieceCryptosystem.Models
{
    public class PrivateKey
    {
        public int[] Polynomial { get; set; }
		/// <summary>
        /// K rows N columns
        /// </summary>
        public MatrixInt GeneratorMatrix { get; set; }
        /// <summary>
        /// K rows K columns
        /// </summary>
        public MatrixInt ScramblerMatrix { get; set; }
        /// <summary>
        /// N rows N columns
        /// </summary>
        public MatrixInt PermutationMatrix { get; set; }
    }
}
