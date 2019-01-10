using System;
using CryptoSystems;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;
using CryptoSystems.Utility;
using NUnit.Framework;

namespace CryptoSystemsTests
{
    [TestFixture]
    public class ReedSolomonCodeTests
    {
        private MatrixInt Message;
        private MatrixInt ErrorVector;

        [OneTimeSetUp]
        public void Init()
        {
            Message = new MatrixInt(new int[,]{
                { 1, 0, 2 }
            });
            ErrorVector = new MatrixInt(new int[,]{
                { 0, 0, 0, 3, 0, 7, 0 }
            });
        }


        [Test]
        [Repeat(100)]
        public void TestReedSolomonCode()
        {
            var galoisField = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);
            var generator = new ParityCheckMatrixGeneratorGeneric();
            var reedSolomonCode = new ReedSolomonCode(galoisField, generator);

            var encodedMessage = reedSolomonCode.Encode(Message, ErrorVector);

            var originalMessage = reedSolomonCode.DecodeAndCorrect(encodedMessage);

            Assert.IsTrue(Message == originalMessage);
        }

        [Test]
        [Repeat(100)]
        public void TestReedSolomonCodeWithEllyptics()
        {
            Console.WriteLine("Test start:");

            var galoisField = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);
            var generator = new ParityCheckMatrixGeneratorEllyptic();
            var reedSolomonCode = new ReedSolomonCode(galoisField, generator);

            Console.WriteLine("Parity check matrix:");
            Console.WriteLine(reedSolomonCode.ParityCheckMatrix);
            Console.WriteLine("Generator matrix:");
            Console.WriteLine(reedSolomonCode.GeneratorMatrix);

            Console.WriteLine("Encoding message:");
            var encodedMessage = reedSolomonCode.Encode(Message, ErrorVector);

            Console.WriteLine("Decoding message:");
            var originalMessage = reedSolomonCode.DecodeAndCorrect(encodedMessage);

            Assert.IsTrue(Message == originalMessage);
        }
    }
}
