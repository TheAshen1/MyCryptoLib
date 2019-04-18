using CryptoSystems;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;
using CryptoSystems.Utility;
using System.Collections.Generic;
using Xunit;

namespace CryptoSystemsTests
{
    public class McElieseTests
    {
        public static IEnumerable<object[]> GetData => new List<object[]>
        {
            new object[] {
                new MatrixInt(new int[,] { { 1, 0, 2 } }),
                new MatrixInt(new int[,] { { 1, 0, 0, 3, 0, 0, 0 } }),
                new MatrixInt(new int[,] { { 2, 0, 1 }, { 0, 5, 3 }, { 4, 0, 1} }),
                new List<int> { 0,2,1,6,4,3,5 },
                new List<int> { 1,2,3,1,2,3,2 }
            },
            new object[] {
                new MatrixInt(new int[,] { { 7, 1, 5 } }),
                new MatrixInt(new int[,] { { 0, 0, 6, 0, 0, 7, 0 } }),
                new MatrixInt(new int[,] { { 2, 0, 1 }, { 0, 5, 3 }, { 4, 0, 1} }),
                new List<int> { 0,2,1,6,4,3,5 },
                new List<int> { 1,2,3,1,2,3,2 }
            },
            new object[] {
                new MatrixInt(new int[,] { { 5, 5, 5 } }),
                new MatrixInt(new int[,] { { 0, 0, 2, 0, 0, 3, 0 } }),
                new MatrixInt(new int[,] { { 2, 0, 1 }, { 0, 5, 3 }, { 4, 0, 1} }),
                new List<int> { 0,2,1,6,4,3,5 },
                new List<int> { 1,2,3,1,2,3,2 }
            },
            new object[] {
                new MatrixInt(new int[,] { { 1, 2, 5 } }),
                new MatrixInt(new int[,] { { 1, 0, 7, 0, 0, 0, 0 } }),
                new MatrixInt(new int[,] { { 2, 0, 1 }, { 0, 5, 3 }, { 4, 0, 1} }),
                new List<int> { 0,2,1,6,4,3,5 },
                new List<int> { 1,2,3,1,2,3,2 }
            },
            new object[] {
                new MatrixInt(new int[,] { { 6, 6, 6 } }),
                new MatrixInt(new int[,] { { 0, 0, 6, 6, 0, 0, 0 } }),
                new MatrixInt(new int[,] { { 2, 0, 1 }, { 0, 5, 3 }, { 4, 0, 1} }),
                new List<int> { 0,2,1,6,4,3,5 },
                new List<int> { 1,2,3,1,2,3,2 }
            },
            new object[] {
                new MatrixInt(new int[,] { { 7, 0, 7 } }),
                new MatrixInt(new int[,] { { 0, 2, 5, 0, 0, 0, 0 } }),
                new MatrixInt(new int[,] { { 6, 0, 1 }, { 1, 0, 4 }, { 7, 5, 3} }),
                new List<int> { 5,1,0,6,3,2,4 },
                new List<int> { 3,3,1,2,1,1,3 }
            },
            new object[] {
                new MatrixInt(new int[,] { { 2, 2, 2 } }),
                new MatrixInt(new int[,] { { 2, 2, 0, 0, 0, 0, 0 } }),
                new MatrixInt(new int[,] { { 6, 0, 1 }, { 1, 0, 4 }, { 7, 5, 3} }),
                new List<int> { 5,1,0,6,3,2,4 },
                new List<int> { 3,3,1,2,1,1,3 }
            },
            new object[] {
                new MatrixInt(new int[,] { { 4, 0, 4 } }),
                new MatrixInt(new int[,] { { 0, 0, 4, 0, 0, 4, 0 } }),
                new MatrixInt(new int[,] { { 6, 0, 1 }, { 1, 0, 4 }, { 7, 5, 3} }),
                new List<int> { 5,1,0,6,3,2,4 },
                new List<int> { 3,3,1,2,1,1,3 }
            },
            new object[] {
                new MatrixInt(new int[,] { { 1, 5, 0 } }),
                new MatrixInt(new int[,] { { 0, 0, 0, 0, 0, 1, 2 } }),
                new MatrixInt(new int[,] { { 6, 0, 1 }, { 1, 0, 4 }, { 7, 5, 3} }),
                new List<int> { 5,1,0,6,3,2,4 },
                new List<int> { 3,3,1,2,1,1,3 }
            },
            new object[] {
                new MatrixInt(new int[,] { { 2, 1, 0 } }),
                new MatrixInt(new int[,] { { 7, 7, 0, 0, 0, 0, 0 } }),
                new MatrixInt(new int[,] { { 6, 0, 1 }, { 1, 0, 4 }, { 7, 5, 3} }),
                new List<int> { 5,1,0,6,3,2,4 },
                new List<int> { 3,3,1,2,1,1,3 }
            }
        };


        [Theory, MemberData(nameof(GetData))]
        public void TestMcElieceCryptosystem(MatrixInt message, MatrixInt errorVector, MatrixInt scrambler, IList<int> permutation, IList<int> mask)
        {
            var galoisField = new GaloisField(2, 3, Constants.IrreduciblePolynoms[3]);

            var generator = new ParityCheckMatrixGeneratorGeneric();
            var reedSolomonCode = new ReedSolomonCode(galoisField, generator);

            var mcElieseCryptosystem = new McEliece(reedSolomonCode, scrambler, permutation, mask);

            var encryptedMessage = mcElieseCryptosystem.EncryptMessage(mcElieseCryptosystem.PublicKey, message, errorVector);
            var originalMessage = mcElieseCryptosystem.DecryptMessage(encryptedMessage);

            Assert.True(message == originalMessage);
        }
    }
}
