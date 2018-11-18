using McElieceCryptosystem;
using McElieceCryptosystem.Models;
using McElieceCryptosystem.Util;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Fiddle
            var field = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);
            Console.WriteLine(field);
            var reedSolomonCode = new ReedSolomonCode(field);
            Console.WriteLine(reedSolomonCode.CanCorrectUpTo);
            Console.WriteLine(reedSolomonCode.ParityCheckMatrix);
            Console.WriteLine(reedSolomonCode.GeneratorMatrix);

            var message = new MatrixInt(new int[,]{
                { 3, 2, 4 }
            });
            var errorVector = new MatrixInt(new int[,]{
                { -1, 1, -1, -1, -1, -1, 6 }
            });
            var encodedMessage = reedSolomonCode.Encode(message, errorVector);
            Console.WriteLine(encodedMessage);

            var originalMessage = reedSolomonCode.DecodeAndCorrect(encodedMessage);
            Console.WriteLine(originalMessage);
            Console.WriteLine(originalMessage);
            #endregion
        }
    }
}