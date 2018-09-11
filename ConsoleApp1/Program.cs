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
            var linearCode = new BinaryLinearCode(Constants.HammingCodeGeneratorMatrix);

            //System.Console.WriteLine((linearCode.GeneratorMatrix * linearCode.ParityCheckMatrix.Transpose()).ToString());

            var message = new MatrixInt(new int[,]
            {
                { 1, 0, 1, 1 }
            });
            var biggerMessage = new MatrixInt(new int[,]
            {
                { 1, 0, 1, 1 },
                { 0, 1, 1, 0 },
                { 1, 0, 1, 1 }
            });

            Console.WriteLine(linearCode.GeneratorMatrix.ToString());
            Console.WriteLine(linearCode.ParityCheckMatrix.ToString());
            Console.WriteLine(linearCode.MinimumDistance);

            var encodedMessage = linearCode.Encode(biggerMessage);
            Console.WriteLine(encodedMessage.ToString());

            var errorVector = new MatrixInt(new int[,]
            {
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0 }
            });
            encodedMessage = (encodedMessage + errorVector) % 2;
            Console.WriteLine(errorVector.ToString());
            Console.WriteLine(encodedMessage.ToString());

            var decodedMessage = linearCode.DecodeAndCorrect(encodedMessage);
            Console.WriteLine(decodedMessage.ToString());
            #endregion

            #region Step-by-Step Encryption Process
            //var generatorMatrix = Constants.HammingCodeGeneratorMatrix;

            //var scramblerMatrix = new MatrixInt(new[,]
            //{
            //    { 1, 1, 0, 1 },
            //    { 1, 0, 0, 1 },
            //    { 0, 1, 1, 1 },
            //    { 1, 1, 0, 0 }
            //});

            //var permutationMatrix = new MatrixInt(new[,]
            //{
            //    { 0, 1, 0, 0, 0, 0, 0 },
            //    { 0, 0, 0, 1, 0, 0, 0 },
            //    { 0, 0, 0, 0, 0, 0, 1 },
            //    { 1, 0, 0, 0, 0, 0, 0 },
            //    { 0, 0, 1, 0, 0, 0, 0 },
            //    { 0, 0, 0, 0, 0, 1, 0 },
            //    { 0, 0, 0, 0, 1, 0, 0 }
            //});

            //var encryptionMatrix = scramblerMatrix * generatorMatrix * permutationMatrix % 2;

            //Console.WriteLine(encryptionMatrix.ToString());

            //var message = new MatrixInt(new int[,]
            //{
            //    { 1, 1, 0, 1 }
            //});

            //var errorVector = new MatrixInt(new int[,]
            //{
            //    { 0, 0, 0, 0, 1, 0, 0 }
            //});

            //var encryptedMessage = (((message * encryptionMatrix) % 2) + errorVector) % 2;

            //Console.WriteLine(encryptedMessage.ToString());
            #endregion
        }
    }
}