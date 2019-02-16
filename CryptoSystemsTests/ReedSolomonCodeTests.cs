using System;
using CryptoSystems;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;
using CryptoSystems.Utility;
using Xunit;
using Xunit.Extensions;

namespace CryptoSystemsTests
{
    //[TestFixture]
    //public class ReedSolomonCodeTests
    //{
    //    private MatrixInt _message;
    //    private MatrixInt _errorVector;


    //    public ReedSolomonCodeTests()
    //    {
    //        _message = new MatrixInt(new int[,] { { 7, 3, 5 } });
    //        _errorVector = new MatrixInt(new int[,] { { 6, 0, 0, 5, 0, 0, 0 } });
    //    }

    //    [Test]
    //    public void TestReedSolomonCode()
    //    {
    //        var galoisField = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);
    //        var generator = new ParityCheckMatrixGeneratorGeneric();
    //        var reedSolomonCode = new ReedSolomonCode(galoisField, generator);

    //        var encodedMessage = reedSolomonCode.Encode(_message, _errorVector);

    //        var originalMessage = reedSolomonCode.DecodeAndCorrect(encodedMessage);

    //        Assert.IsTrue(_message == originalMessage);
    //    }

    //    [Test]
    //    public void TestReedSolomonCodeWithEllyptics()
    //    {
    //        Console.WriteLine("Test start:");

    //        var galoisField = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);
    //        var generator = new ParityCheckMatrixGeneratorEllyptic();
    //        var reedSolomonCode = new ReedSolomonCode(galoisField, generator);

    //        Console.WriteLine("Parity check matrix:");
    //        Console.WriteLine(reedSolomonCode.ParityCheckMatrix);
    //        Console.WriteLine("Generator matrix:");
    //        Console.WriteLine(reedSolomonCode.GeneratorMatrix);

    //        Console.WriteLine("Encoding message:");
    //        var encodedMessage = reedSolomonCode.Encode(_message, _errorVector);

    //        Console.WriteLine("Decoding message:");
    //        var originalMessage = reedSolomonCode.DecodeAndCorrect(encodedMessage);

    //        Assert.IsTrue(_message == originalMessage);
    //    }
    //}


    public class ReedSolomonCodeTests
    {
        [Theory, ClassData(typeof(TestDataProvider))]
        public void TestReedSolomonCode(MatrixInt message, MatrixInt errorVector)
        {
            var galoisField = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);
            var generator = new ParityCheckMatrixGeneratorGeneric();
            var reedSolomonCode = new ReedSolomonCode(galoisField, generator);

            var encodedMessage = reedSolomonCode.Encode(message, errorVector);

            var originalMessage = reedSolomonCode.DecodeAndCorrect(encodedMessage);

            Assert.True(message == originalMessage);
        }
    }
}
