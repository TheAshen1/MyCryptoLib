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
            //var linearCode = new HammingCode();

            //var message = new MatrixInt(new int[,]
            //{
            //    { 1, 1, 0, 1 }
            //});
            //var scrambler = new MatrixInt(new int[,]
            //{
            //    { 1, 0, 1, 1 },
            //    { 0, 1, 1, 0 },
            //    { 1, 0, 1, 1 }
            //});

            //Console.WriteLine(linearCode.GeneratorMatrix.ToString());
            //Console.WriteLine(linearCode.ParityCheckMatrix.ToString());
            //Console.WriteLine(linearCode.MinimumDistance);

            //var encodedMessage = linearCode.Encode(message);
            //Console.WriteLine(encodedMessage.ToString());

            //var errorVector = new MatrixInt(new int[,]
            //{
            //    { 0, 0, 0, 0, 1, 0, 0 }
            //});
            //encodedMessage = (encodedMessage + errorVector) % 2;
            //Console.WriteLine(errorVector.ToString());
            //Console.WriteLine(encodedMessage.ToString());

            //var decodedMessage = linearCode.DecodeAndCorrect(encodedMessage);
            //Console.WriteLine(decodedMessage.ToString());
            #endregion

            #region Step-by-Step Encryption Process
            var linearCode = new HammingCode();

            var scramblerMatrix = new MatrixInt(new[,]
            {
                { 1, 1, 0, 1 },
                { 1, 0, 0, 1 },
                { 0, 1, 1, 1 },
                { 1, 1, 0, 0 }
            });

            var permutationMatrix = new MatrixInt(new[,]
            {
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 1, 0, 0 }
            });

            var mcElieceCryptosystem = new McElieceCryptosystem.McElieceCryptosystem(linearCode, scramblerMatrix, permutationMatrix);

            var message = new MatrixInt(new int[,]
            {
                { 1, 1, 0, 1 }
            });

            var errorVector = new MatrixInt(new int[,]
            {
                { 0, 0, 0, 0, 1, 0, 0 }
            });

            var someOnesPublicKey = new PublicKey()
            {
                EncryptionMatrix = new MatrixInt(new int[,] {
                    { 1, 1, 1, 1, 0, 0, 0 },
                    { 1, 1, 0, 0, 1, 0, 0 },
                    { 1, 0, 0, 1, 1, 0, 1 },
                    { 0, 1, 0, 1, 1, 1, 0 }
                }),
                ErrorVectorMaxWeight = linearCode.CanCorrectUpTo
            };
            var encryptedMessage = mcElieceCryptosystem.EncryptMesaage(someOnesPublicKey, message, errorVector);

            Console.WriteLine(encryptedMessage.ToString());

            var decryptedMessage = mcElieceCryptosystem.DecryptMessage(encryptedMessage);

            Console.WriteLine(decryptedMessage.ToString());
            #endregion
        }
    }
}