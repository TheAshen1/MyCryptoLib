using CryptoSystems;
using CryptoSystems.Algorithms;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;
using CryptoSystems.Utility;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var field = new GaloisField(2, 2, Constants.IrreduciblePolynoms[2]);
            Console.WriteLine(field);
            Console.WriteLine(field.AdditionTable);
            Console.WriteLine(field.MultiplicationTable);
            Console.WriteLine(field.DivisionTable);

            var message = new MatrixInt(new int[] { 1, 0 });
            var errorVector = new MatrixInt(new int[] { 2, 2, 0, 0, 0, 0, 0, 0 });
            var scrambler = new MatrixInt(new int[,] {
                { 1, 2 },
                { 3, 0 }
            });
            var permutation = new int[] { 1, 4, 3, 0, 2, 7, 5, 6 };
            //var permutationMatrix = new MatrixInt(new int[,] {
            //    { 0, 1, 0, 0, 0, 0, 0, 0 },
            //    { 0, 0, 0, 0, 1, 0, 0, 0 },
            //    { 0, 0, 0, 1, 0, 0, 0, 0 },
            //    { 1, 0, 0, 0, 0, 0, 0, 0 },
            //    { 0, 0, 1, 0, 0, 0, 0, 0 },
            //    { 0, 0, 0, 0, 0, 0, 0, 1 },
            //    { 0, 0, 0, 0, 0, 1, 0, 0 },
            //    { 0, 0, 0, 0, 0, 0, 1, 0 }
            //});

            //Console.WriteLine(permutationMatrix.Transpose());
            var mask = new int[] { 1, 2, 3, 1, 2, 3, 1, 1 };
            var mceliese = new McElieseEllyptic(8, 2, 6, 2, field, scrambler, permutation, mask);
            var crytptogram = mceliese.EncryptMessage(mceliese.PublicKey, message, errorVector);
            Console.WriteLine(crytptogram);
            var decryptedMessage = mceliese.DecryptMessage(crytptogram);
            Console.WriteLine(decryptedMessage);
            #region Reed-Solomon code example
            //var field = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);
            //Console.WriteLine(field);
            //Console.WriteLine(field.AdditionTable);
            //Console.WriteLine(field.MultiplicationTable);
            //Console.WriteLine(field.DivisionTable);
            ////var generator = new ParityCheckMatrixGeneratorGeneric();
            //var generator = new ParityCheckMatrixGeneratorEllyptic();

            //var reedSolomonCode = new ReedSolomonCode(field, generator);
            //Console.WriteLine(reedSolomonCode.T);
            //Console.WriteLine(reedSolomonCode.ParityCheckMatrix);
            //Console.WriteLine(reedSolomonCode.GeneratorMatrix);

            //var message = new MatrixInt(new int[,]{
            //    { 1, 0, 2 }
            //});
            //var errorVector = new MatrixInt(new int[,]{
            //    { 1, 0, 0, 3, 0, 0, 0 }
            //});

            //var encodedMessage = reedSolomonCode.Encode(message, errorVector);
            //Console.WriteLine(encodedMessage);

            //var originalMessage = reedSolomonCode.DecodeAndCorrect(encodedMessage);
            //Console.WriteLine(originalMessage);
            #endregion
        }


    }
}