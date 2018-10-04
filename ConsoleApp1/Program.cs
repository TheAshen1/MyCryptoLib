using McElieceCryptosystem;
using McElieceCryptosystem.Models;
using McElieceCryptosystem.Util;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Testing out polynomials

            //var polynom1 = new PolynomialDouble(new double[] { -42.0, 0.0, -12.0, 1.0 });
            //var polynom2 = new PolynomialDouble(new double[] { -3.0, 1.0 });

            //Console.WriteLine(polynom1);
            //Console.WriteLine(polynom2);

            //var result = polynom1 / polynom2;

            //Console.WriteLine(result.Result);
            //Console.WriteLine(result.Mod);


            var polynom1 = new PolynomialDouble(new double[] { 1.0, 0.0, -1.0, 0.0, 2.0, 1.0 });
            var polynom2 = new PolynomialDouble(new double[] { -1.0, 0.0, 0.0, 0.0, 1.0 });

            var extendedGcd = Utility.PolynomialExtendedGCD(polynom1, polynom2);
            Console.WriteLine(extendedGcd.Gcd);
            Console.WriteLine(extendedGcd.S);
            Console.WriteLine(extendedGcd.T);


            #endregion

            #region Fiddle
            // 1 + X + X^4
            //var polynomial = new MatrixInt(new int[1, 5] { { 1, 1, 0, 0, 1 } });

            //var goppaPolynomial = new MatrixInt(new int[1, 3] { { 0, 7, 0 } });

            //var goppaCode = new GoppaCode(goppaPolynomial, polynomial, 4);
            //Console.WriteLine(goppaCode.ParityCheckMatrix);

            //var matrix = Utility.ToReducedRowEchelonForm(goppaCode.ParityCheckMatrix);
            //Console.WriteLine(matrix);

            //var weights = new List<Tuple<int, int>>();

            //for (int row = 0; row < matrix.RowCount; row++)
            //{
            //    var rowWeight = Utility.Weight(matrix.GetRow(row));
            //    weights.Add(new Tuple<int, int>(row, rowWeight));
            //    Console.Write(rowWeight + " ");
            //}
            //Console.Write("\n");
            //weights.Sort((dataPairLeft, dataPairRight) => dataPairLeft.Item2 - dataPairRight.Item2);

            //foreach (var item in weights)
            //{
            //    Console.Write(item + " ");
            //}
            //Console.Write("\n");

            //var X = new int[matrix.ColumnCount];
            //foreach (var item in weights)
            //{
            //    var currentRowNumber = item.Item1;
            //    var currentRowWeight = item.Item2;
            //    for (int col = 0; col < matrix.ColumnCount; col++)
            //    {
            //        if(matrix.Data[currentRowNumber, col] != 0)
            //        {

            //        }
            //    }
            //}

            //var test = new MatrixInt(new int[6, 8]{
            //   { 1, 0, 0, 0, 0, 0, 1, 0 },
            //   { 0, 1, 0, 0, 0, 0, 1, 1 },
            //   { 1, 0, 0, 1, 0, 1, 0, 1 },
            //   { 0, 0, 1, 1, 1, 1, 0, 1 },
            //   { 0, 0, 0, 1, 0, 1, 0, 1 },
            //   { 0, 1, 0, 1, 1, 1, 1, 0 }
            //});
            //var testRREF = Utility.ToReducedRowEchelonForm(test);
            //Console.WriteLine(testRREF);
            #region goppa example 


            //var q = 2;
            //var m = 4;
            //var n = 12;
            //var t = 2;

            //var col0 = GF.GetColumn(0);

            //var h = new MatrixInt(1, 1);
            //int ptr = 0;
            //for (var i = 2; i < GF.ColumnCount - 1; i++)
            //{
            //    var one = (GF.GetColumn((i + i) % 15));
            //    var two = (GF.GetColumn((i + 7) % 15));
            //    var three = col0;

            //    var res = (one + two + three) % 2;

            //    var index = GF.LookupColumn(res);
            //    if (index > -1)
            //    {
            //        h = h | new MatrixInt(new int[1, 1] { { 15 - index } });
            //    }

            //}
            //h = h.GetRangeOfColumns(new RangeInt(1, 13));
            //Console.WriteLine(h);

            //var H_top = new MatrixInt(4, 1);
            //var H_bot = new MatrixInt(4, 1);
            //var col7 = GF.GetColumn(7);
            //for (int i = 2, j = 0; i < GF.ColumnCount - 1 && j < h.ColumnCount; i++, j++)
            //{
            //    //top part
            //    if(i == 7)
            //    {
            //        H_top = H_top | new MatrixInt(new int[4,1]);
            //        continue;
            //    }
            //    int index1 = GF.LookupColumn((GF.GetColumn(i) + col7) % 2);
            //    var col_top = GF.GetColumn((index1 + h.Data[0, j]) % 15);
            //    H_top = H_top | col_top;
            //    //top part

            //    //bottom part
            //    if (i == 7)
            //    {
            //        H_bot = H_bot | col0;
            //        continue;
            //    }
            //    int index2 = GF.LookupColumn((GF.GetColumn(i) + col7) % 2);
            //    var col = GF.GetColumn((index2 + h.Data[0, j]) % 15);
            //    H_bot = H_bot | col;
            //    //bottom part


            //}

            // var H = H_top.AppendRows(H_bot);
            //Console.WriteLine(H);

            #endregion

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

            #region Step-by-Step Encryption Process with Hamming code
            //var linearCode = new HammingCode();

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

            //var mcElieceCryptosystem = new McElieceCryptosystem.McElieceCryptosystem(linearCode, scramblerMatrix, permutationMatrix);

            //var message = new MatrixInt(new int[,]
            //{
            //    { 1, 1, 0, 1 }
            //});

            //var errorVector = new MatrixInt(new int[,]
            //{
            //    { 0, 0, 0, 0, 1, 0, 0 }
            //});

            //var someOnesPublicKey = new PublicKey()
            //{
            //    EncryptionMatrix = new MatrixInt(new int[,] {
            //        { 1, 1, 1, 1, 0, 0, 0 },
            //        { 1, 1, 0, 0, 1, 0, 0 },
            //        { 1, 0, 0, 1, 1, 0, 1 },
            //        { 0, 1, 0, 1, 1, 1, 0 }
            //    }),
            //    ErrorVectorMaxWeight = linearCode.CanCorrectUpTo
            //};
            //var encryptedMessage = mcElieceCryptosystem.EncryptMesaage(someOnesPublicKey, message, errorVector);

            //Console.WriteLine(encryptedMessage.ToString());

            //var decryptedMessage = mcElieceCryptosystem.DecryptMessage(encryptedMessage);

            //Console.WriteLine(decryptedMessage.ToString());
            #endregion

            #region Step-by-Step Encryption Process with Goppa code
            //var message = new MatrixInt(new int[,] {
            //    { 1, 1, 1, 1 }
            //});
            //var generatorMatrix = Constants.GoppaCodeGeneratorMatrix_4_12;
            //var encodedMessage = message * generatorMatrix % 2;
            //Console.WriteLine(encodedMessage);

            //var errorVector = new MatrixInt(new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 } });

            //var messageWithErrors = (encodedMessage + errorVector) % 2 ;
            //Console.WriteLine(messageWithErrors);
            #endregion
        }
    }
}