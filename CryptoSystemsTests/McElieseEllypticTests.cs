using CryptoSystems;
using CryptoSystems.Algorithms;
using CryptoSystems.Exceptions;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;
using CryptoSystems.Utility;
using System;
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
                2,
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
                2,
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
                2,
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
        public void McElieseEllypticTest(int n, int k, int d, int t, int fieldPower, MatrixInt message, MatrixInt errorVector, MatrixInt scrambler, int[] permutation, int[] mask)
        {
            var galoisField = new GaloisField(2, fieldPower);
            var mceliese = new McElieseEllyptic(n, k, d, t, galoisField, scrambler, permutation, mask);
            var crytptogram = mceliese.EncryptMessage(mceliese.PublicKey, message, errorVector);
            var decryptedMessage = mceliese.DecryptMessage(crytptogram);

            Assert.True(message == decryptedMessage);
        }

        [Theory, MemberData(nameof(GetDataForEllypticParityCheckMatrixGeneratorTest))]
        public void McElieseGenericFormTest(int n, int k, int d, int t, int fieldPower, MatrixInt message, MatrixInt errorVector, MatrixInt scrambler, int[] permutation, int[] mask)
        {
            var galoisField = new GaloisField(2, fieldPower);
            var generator = new ParityCheckMatrixGeneratorEllyptic(2);
            var linearCode = new LinearCode(n, k, d, t, galoisField, generator);

            while (true)
            {
                linearCode.ParityCheckMatrix = generator.Generate(linearCode);

                if (Helper.Weight(linearCode.ParityCheckMatrix) < Math.Ceiling(linearCode.ParityCheckMatrix.RowCount * linearCode.ParityCheckMatrix.ColumnCount * 0.7))
                {
                    continue;
                }

                try
                {
                    linearCode.GeneratorMatrix = GeneratorMatrixCalculator.CalculateGeneratorMatrixAlt(linearCode);
                }
                catch (LinearCodeException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

                if (Helper.Weight(MatrixAlgorithms.DotMultiplication(linearCode.GeneratorMatrix, linearCode.ParityCheckMatrix.Transpose(), galoisField)) == 0)
                {
                    linearCode.GeneratorMatrix = linearCode.GeneratorMatrix;
                    break;
                }
            }


            var crytptogram = McElieseGenericForm.Encrypt(linearCode, scrambler, permutation, mask, generator, message, errorVector);
            var decryptedMessage = McElieseGenericForm.Decrypt(linearCode, permutation, mask, scrambler, generator, crytptogram);

            Assert.True(message == decryptedMessage);
        }


    }
}
