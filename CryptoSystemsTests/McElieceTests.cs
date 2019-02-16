using CryptoSystems;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;
using CryptoSystems.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CryptoSystemsTests
{
    [TestFixture]
    public class McElieseTests
    {
        private MatrixInt Message;
        private MatrixInt ErrorVector;
        private MatrixInt ScramblerMatrix;
        private List<int> Permutation;
        private List<int> Mask;

        public McElieseTests()
        {

        }
        [OneTimeSetUp]
        public void Init()
        {
            Message = new MatrixInt(new int[,]{
                { 7, 1, 5 }
            });
            ErrorVector = new MatrixInt(new int[,]{
                { 0, 0, 6, 0, 0, 7, 0 }
            });

            ScramblerMatrix = new MatrixInt(new int[,] {
               {2, 0, 1},
               {0, 5, 3},
               {4, 0, 1}
            });

            Permutation = new List<int>
            {
                0, 2, 1, 6, 4, 3, 5
            };

            Mask = new List<int>
            {
                1, 2, 3, 1, 2, 3, 2
            };
        }

        [Test]
        public void TestMcElieceCryptosystem()
        {
            var galoisField = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);

            var generator = new ParityCheckMatrixGeneratorGeneric();
            var reedSolomonCode = new ReedSolomonCode(galoisField, generator);

            var mcElieseCryptosystem = new McElieceCryptosystem(reedSolomonCode, ScramblerMatrix, Permutation, Mask);

            var encryptedMessage = mcElieseCryptosystem.EncryptMessage(mcElieseCryptosystem.PublicKey, Message, ErrorVector);
            var originalMessage = mcElieseCryptosystem.DecryptMessage(encryptedMessage);

            Assert.IsTrue(Message == originalMessage);
        }

        [Test]
        [Repeat(100)]
        public void TestMcElieceCryptosystemWithEllyptics()
        {
            var galoisField = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);

            var generator = new ParityCheckMatrixGeneratorEllyptic();
            var reedSolomonCode = new ReedSolomonCode(galoisField, generator);

            var mcElieseCryptosystem = new McElieceCryptosystem(reedSolomonCode, ScramblerMatrix, Permutation, Mask);

            var encryptedMessage = mcElieseCryptosystem.EncryptMessage(mcElieseCryptosystem.PublicKey, Message, ErrorVector);
            var originalMessage = mcElieseCryptosystem.DecryptMessage(encryptedMessage);

            Assert.IsTrue(Message == originalMessage);
        }

    }
}
