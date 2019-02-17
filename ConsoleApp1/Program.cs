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
            var field = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);
            Console.WriteLine(field);
            Console.WriteLine(field.AdditionTable);
            Console.WriteLine(field.MultiplicationTable);
            Console.WriteLine(field.DivisionTable);

            var degree = 3;
            var coefficients = new List<int>
            {
                1, 1, 1, 0, 1,
            };

            var generator = new ParityCheckMatrixGeneratorEllyptic(degree, coefficients);
            var reedSolomonCode = new ReedSolomonCode(field, generator);

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