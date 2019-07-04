using CryptoSystems.Models;
using System.Collections.Generic;

namespace CryptoSystems.Util
{
    public static class Constants
    {
        public static readonly Dictionary<int, MatrixInt> IrreduciblePolynoms = new Dictionary<int, MatrixInt>()
        {
            {
                2,
                new MatrixInt(new[,]
                {
                    { 1, 1, 1 }
                })
            },{
                3,
                new MatrixInt(new[,]
                {
                    { 1, 1, 0, 1 }
                })
            },{
                4,
                new MatrixInt(new[,]
                {
                   { 1, 1, 0, 0, 1 }
                })
            },{
                5,
                new MatrixInt(new[,]
                {
                   { 1, 1, 0, 0, 0, 1 }
                })
            }
        };
    }
}
