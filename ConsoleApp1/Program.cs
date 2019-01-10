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
            #region Fiddle

            #region Inverse matrix demo
            //var galoisField = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);
            //Console.WriteLine(galoisField);

            //var rawMatrix = new int[,] {
            //   {2, 0, 1},
            //   {0, 5, 3},
            //   {4, 0, 1}
            //};

            //var rawInverse = new int[,]
            //{
            //    {1, 0, 1},
            //    {2, 4, 7},
            //    {4, 0, 2}
            //};

            //var matrix = new MatrixInt(rawMatrix) - 1;

            //var inverse = MatrixAlgorithms.MatrixInverse(matrix, field);
            //Console.WriteLine(inverse);

            //var (L, U) = MatrixAlgorithms.LUDecomposition(matrix, field);
            //Console.WriteLine(L);
            //Console.WriteLine(U);

            //var det = MatrixAlgorithms.GetDeterminant(matrix, field);
            //Console.WriteLine(det);
            //Console.WriteLine(det);

            //var check = MatrixAlgorithms.DotMultiplication(matrix, inverse, field);
            //Console.WriteLine(check + 1);
            //Console.WriteLine(check + 1);

            //var rand = new Random();
            //for (int i = 0; i < 10; i++)
            //{
            //    var scramblerMatrix = Utility.GenerateScramblerMatrix(matrix.ColumnCount, galoisField, rand);
            //    var det = MatrixAlgorithms.GetDeterminant(scramblerMatrix, galoisField);
            //    Console.WriteLine(det);
            //    var inverse = MatrixAlgorithms.MatrixInverse(scramblerMatrix, galoisField);
            //    Console.WriteLine(inverse);
            //}

            #endregion


            #endregion

            #region McElieceExample
            //var galoisField = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);
            //Console.WriteLine(galoisField);

            //var reedSolomonCode = new ReedSolomonCode(galoisField);
            //Console.WriteLine(reedSolomonCode.CanCorrectUpTo);
            //Console.WriteLine(reedSolomonCode.ParityCheckMatrix);
            //Console.WriteLine(reedSolomonCode.GeneratorMatrix);


            //var demoMessage = new MatrixInt(new int[,] {
            //    {7, 1, 5}
            //});

            //var demoErrorVector = new MatrixInt(new int[,] {
            //    {0, 0, 6, 0, 0, 7, 0}
            //});
            //var demoScramblerMatrix = new MatrixInt(new int[,] {
            //   {2, 0, 1},
            //   {0, 5, 3},
            //   {4, 0, 1}
            //});

            //var demoPermutation = new List<int>
            //{
            //    0, 2, 1, 6, 4, 3, 5
            //};

            //var demoMask = new List<int>
            //{
            //    1, 2, 3, 1, 2, 3, 2
            //};

            //var mcElieseCryptosystem = new McElieceCryptosystem(reedSolomonCode, demoScramblerMatrix, demoPermutation, demoMask);

            //var encryptedMessage = mcElieseCryptosystem.EncryptMessage(mcElieseCryptosystem.PublicKey, demoMessage, demoErrorVector);
            //Console.WriteLine(encryptedMessage);

            //var originalMessage = mcElieseCryptosystem.DecryptMessage(encryptedMessage);
            //Console.WriteLine(originalMessage);

            #endregion

            #region Reed-Solomon code example
            var field = new GaloisField(2, 3, Constants.IrreduciblePolynom_deg3);
            Console.WriteLine(field);
            var generator = new ParityCheckMatrixGeneratorEllyptic();

            var parityCheckmatrix = new MatrixInt(new int[,]{
                   { 1, 1, 1, 1, 1, 1, 1 },
                   { 5, 2, 6, 3, 7, 4, 6 },
                   { 5, 2, 6, 3, 7, 4, 7 },
                   { 7, 6, 5, 4, 3, 2, 1 }
            });

            var reedSolomonCode = new ReedSolomonCode(field, parityCheckmatrix);
            Console.WriteLine(reedSolomonCode.T);
            Console.WriteLine(reedSolomonCode.ParityCheckMatrix);
            Console.WriteLine(reedSolomonCode.GeneratorMatrix);

            var message = new MatrixInt(new int[,]{
                { 1, 0, 2 }
            });
            var errorVector = new MatrixInt(new int[,]{
                { 0, 0, 0, 3, 0, 7, 0 }
            });

            var encodedMessage = reedSolomonCode.Encode(message, errorVector);
            Console.WriteLine(encodedMessage);

            var originalMessage = reedSolomonCode.DecodeAndCorrect(encodedMessage);
            Console.WriteLine(originalMessage);
            #endregion
        }


    }
}