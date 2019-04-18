using System.Collections.Generic;
using CryptoSystems;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;
using CryptoSystems.Utility;
using Xunit;

namespace CryptoSystemsTests
{
    public class ReedSolomonCodeTests
    {
        public static IEnumerable<object[]> GetDataForReedSolomonCodeTest => new List<object[]>
        {
            new object[] {
                3,
                Constants.IrreduciblePolynoms[3],
                new MatrixInt(new int[,] { { 1, 0, 2 } }),
                new MatrixInt(new int[,] { { 1, 0, 0, 3, 0, 0, 0 } })
            },
            new object[] {
                3,
                Constants.IrreduciblePolynoms[3],
                new MatrixInt(new int[,] { { 7, 1, 5 } }),
                new MatrixInt(new int[,] { { 0, 0, 6, 0, 0, 7, 0 } })
            },
            new object[] {
                3,
                Constants.IrreduciblePolynoms[3],
                new MatrixInt(new int[,] { { 0, 6, 1 } }),
                new MatrixInt(new int[,] { { 1, 0, 0, 0, 5, 0, 0 } })
            },
            new object[] {
                3,
                Constants.IrreduciblePolynoms[3],
                new MatrixInt(new int[,] { { 4, 3, 5 } }),
                new MatrixInt(new int[,] { { 0, 2, 0, 0, 0, 0, 7 } })
            },
            new object[] {
                3,
                Constants.IrreduciblePolynoms[3],
                new MatrixInt(new int[,] { { 7, 6, 1 } }),
                new MatrixInt(new int[,] { { 0, 0, 0, 0, 6, 6, 0 } })
            },
            new object[] {
                3,
                Constants.IrreduciblePolynoms[3],
                new MatrixInt(new int[,] { { 3, 5, 7 } }),
                new MatrixInt(new int[,] { { 6, 0, 0, 5, 0, 0, 0 } })
            },
            new object[] {
                3,
                Constants.IrreduciblePolynoms[3],
                new MatrixInt(new int[,] { { 1, 1, 1 } }),
                new MatrixInt(new int[,] { { 0, 0, 0, 0, 0, 1, 7 } })
            },
            new object[] {
                3,
                Constants.IrreduciblePolynoms[3],
                new MatrixInt(new int[,] { { 5, 0, 0 } }),
                new MatrixInt(new int[,] { { 1, 0, 0, 5, 0, 0, 0 } })
            }
        };

        [Theory, MemberData(nameof(GetDataForReedSolomonCodeTest))]
        public void TestReedSolomonCode(int fieldPower, MatrixInt irreduciblePolynoimial, MatrixInt message, MatrixInt errorVector)
        {
            var galoisField = new GaloisField(2, fieldPower, irreduciblePolynoimial);
            var generator = new ParityCheckMatrixGeneratorGeneric();
            var reedSolomonCode = new ReedSolomonCode(galoisField, generator);

            var encodedMessage = reedSolomonCode.Encode(message, errorVector);

            var originalMessage = reedSolomonCode.DecodeAndCorrect(encodedMessage);

            Assert.True(message == originalMessage);
        }
    }
}
