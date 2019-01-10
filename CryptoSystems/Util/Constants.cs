using CryptoSystems.Models;

namespace CryptoSystems.Utility
{
    public static class Constants
    {       
        public static readonly MatrixInt IrreduciblePolynom_deg2 = new MatrixInt(new[,]
            {
                { 1, 1, 1 }
            });

        public static readonly MatrixInt IrreduciblePolynom_deg3 = new MatrixInt(new[,]
            {
                { 1, 1, 0, 1 }
            });

        public static readonly MatrixInt IrreduciblePolynom_deg4 = new MatrixInt(new[,]
            {
                { 1, 1, 0, 0, 1 }
            });

        public static readonly MatrixInt IrreduciblePolynom_deg5 = new MatrixInt(new[,]
            {
                { 1, 1, 0, 0, 0, 1 }
            });
    }
}
