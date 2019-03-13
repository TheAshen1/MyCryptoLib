using CryptoSystems;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;
using CryptoSystems.Utility;
using System.Collections.Generic;
using Xunit;

namespace CryptoSystemsTests
{
    public class McElieseEllypticTests
    {

        public static IEnumerable<object[]> GetDataForEllypticParityCheckMatrixGeneratorTest => new List<object[]>
        {
            new object[] {
                8, 2, 6, 2,
                2, Constants.IrreduciblePolynom_deg2,
                new MatrixInt(new int[] { 1, 1 }),
                new MatrixInt(new int[] { 0, 0, 1, 0, 0, 2, 0, 0 }),
                new MatrixInt(new int[,] {
                    { 1, 2 },
                    { 3, 0 }
                }),
                new int[] { 1, 4, 3, 0, 2, 7, 5, 6 },
                new int[] { 1, 2, 3, 1, 2, 3, 1, 1 }
            },
            new object[] {
                8, 2, 6, 2,
                2, Constants.IrreduciblePolynom_deg2,
                new MatrixInt(new int[] { 1, 0 }),
                new MatrixInt(new int[] { 2, 2, 0, 0, 0, 0, 0, 0 }),
                new MatrixInt(new int[,] {
                    { 1, 2 },
                    { 3, 0 }
                }),
                new int[] { 1, 4, 3, 0, 2, 7, 5, 6 },
                new int[] { 1, 2, 3, 1, 2, 3, 1, 1 }
            },
            new object[] {
                8, 2, 6, 2,
                2, Constants.IrreduciblePolynom_deg2,
                new MatrixInt(new int[] { 2, 2 }),
                new MatrixInt(new int[] { 0, 0, 0, 0, 0, 0, 3, 3 }),
                new MatrixInt(new int[,] {
                    { 1, 2 },
                    { 3, 0 }
                }),
                new int[] { 1, 4, 3, 0, 2, 7, 5, 6 },
                new int[] { 1, 2, 3, 1, 2, 3, 1, 1 }
            },
        };

        [Theory, MemberData(nameof(GetDataForEllypticParityCheckMatrixGeneratorTest))]
        public void McElieseEllypticTest(int n, int k, int d, int t, int fieldPower, MatrixInt irreduciblePolynoimial, MatrixInt message, MatrixInt errorVector, MatrixInt scrambler, int[] permutation, int[] mask)
        {
            var galoisField = new GaloisField(2, fieldPower, irreduciblePolynoimial);
            var mceliese = new McElieseEllyptic(n, k, d, t, galoisField, scrambler, permutation, mask);
            var crytptogram = mceliese.EncryptMessage(mceliese.PublicKey, message, errorVector);
            var decryptedMessage = mceliese.DecryptMessage(crytptogram);

            Assert.True(message == decryptedMessage);
        }
    }
}
