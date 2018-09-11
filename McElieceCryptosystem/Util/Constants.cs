using McElieceCryptosystem.Models;

namespace McElieceCryptosystem.Util
{
    public static class Constants
    {
        public static readonly MatrixInt HammingCodeGeneratorMatrix = new MatrixInt(new[,]
            {
                { 1, 0, 0, 0, 1, 1, 0 },
                { 0, 1, 0, 0, 1, 0, 1 },
                { 0, 0, 1, 0, 0, 1, 1 },
                { 0, 0, 0, 1, 1, 1, 1 }
            });
    }
}
