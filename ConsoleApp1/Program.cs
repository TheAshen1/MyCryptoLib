using McElieceCryptosystem;
using McElieceCryptosystem.Models;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Fiddle
            //var mcElice = new McElieceCryptosystem.McElieceCryptosystem();
            //var m1 = new int[,] { { 1, 2 } };
            //var m2 = new int[,] { { 1, 2 }, { 3, 4 } };

            //var M1 = new MatrixInt(m1);
            //var M2 = new MatrixInt(m2);

            //Console.WriteLine(M1.ToString());
            //Console.WriteLine(M2.ToString());
            //Console.WriteLine((M1 * M2).ToString());

            var subMatrix1 = Constants.HammingCodeGeneratorMatrix.SubMatrix(new RangeInt(4), new RangeInt(4));
            Console.WriteLine(subMatrix1.ToString());

            var subMatrix2 = Constants.HammingCodeGeneratorMatrix.SubMatrix(new RangeInt(4,7), new RangeInt(4));
            Console.WriteLine(subMatrix2.ToString());

            Console.WriteLine((subMatrix1 | subMatrix2).ToString());
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