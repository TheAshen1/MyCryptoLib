using CryptoSystems;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;
using CryptoSystems.Utility;
using System.Collections.Generic;
using Xunit;

namespace CryptoSystemsTests
{
    public class ParityCheckMatrixGeneratorTests
    {

        public static IEnumerable<object[]> GetDataForEllypticParityCheckMatrixGeneratorTest => new List<object[]>
        {
            new object[] {
                3,
                Constants.IrreduciblePolynom_deg3,
                new MatrixInt(new int[,] { { 1, 0, 2 } }),
                new MatrixInt(new int[,] { { 1, 0, 0, 3, 0, 0, 0 } }),
                3,
                new List<int> { 1, 1, 1, 0, 1 }
            },
            new object[] {
                3,
                Constants.IrreduciblePolynom_deg3,
                new MatrixInt(new int[,] { { 7, 1, 5 } }),
                new MatrixInt(new int[,] { { 0, 0, 6, 0, 0, 7, 0 } }),
                3,
                new List<int> { 1, 1, 1, 0, 1 }
            },
            new object[] {
                3,
                Constants.IrreduciblePolynom_deg3,
                new MatrixInt(new int[,] { { 0, 6, 1 } }),
                new MatrixInt(new int[,] { { 1, 0, 0, 0, 5, 0, 0 } }),
                3,
                new List<int> { 1, 1, 1, 0, 1 }
            },
            new object[] {
                3,
                Constants.IrreduciblePolynom_deg3,
                new MatrixInt(new int[,] { { 4, 3, 5 } }),
                new MatrixInt(new int[,] { { 0, 2, 0, 0, 0, 0, 7 } }),
                3,
                new List<int> { 1, 1, 1, 0, 1 }
            },
            new object[] {
                3,
                Constants.IrreduciblePolynom_deg3,
                new MatrixInt(new int[,] { { 7, 6, 1 } }),
                new MatrixInt(new int[,] { { 0, 0, 0, 0, 6, 6, 0 } }),
                3,
                new List<int> { 1, 1, 1, 0, 1 }
            },
            new object[] {
                3,
                Constants.IrreduciblePolynom_deg3,
                new MatrixInt(new int[,] { { 3, 5, 7 } }),
                new MatrixInt(new int[,] { { 6, 0, 0, 5, 0, 0, 0 } }),
                3,
                new List<int> { 1, 1, 1, 0, 1 }
            },
            new object[] {
                3,
                Constants.IrreduciblePolynom_deg3,
                new MatrixInt(new int[,] { { 1, 1, 1 } }),
                new MatrixInt(new int[,] { { 0, 0, 0, 0, 0, 1, 7 } }),
                3,
                new List<int> { 1, 1, 1, 0, 1 }
            },
            new object[] {
                3,
                Constants.IrreduciblePolynom_deg3,
                new MatrixInt(new int[,] { { 5, 0, 0 } }),
                new MatrixInt(new int[,] { { 1, 0, 0, 5, 0, 0, 0 } }),
                3,
                new List<int> { 1, 1, 1, 0, 1 }
            }
        };

        [Theory, MemberData(nameof(GetDataForEllypticParityCheckMatrixGeneratorTest))]
        public void EllypticParityCheckMatrixGeneratorTest(int fieldPower, MatrixInt irreduciblePolynoimial, MatrixInt message, MatrixInt errorVector, int degree, IList<int> coefficients)
        {
            var galoisField = new GaloisField(2, fieldPower, irreduciblePolynoimial);
            var generator = new ParityCheckMatrixGeneratorEllyptic(degree, coefficients);
            var reedSolomonCode = new ReedSolomonCode(galoisField, generator);

            var encodedMessage = reedSolomonCode.Encode(message, errorVector);

            var originalMessage = reedSolomonCode.DecodeAndCorrect(encodedMessage);

            Assert.True(message == originalMessage);
        }
    }
}
