using McElieceCryptosystem;
using McElieceCryptosystem.Models;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //var mcElice = new McElieceCryptosystem.McElieceCryptosystem();
            var m1 = new int[,] { { 1, 2 } };
            var m2 = new int[,] { { 1, 2 }, { 3, 4 } };

            var M1 = new MatrixInt(m1);
            var M2 = new MatrixInt(m2);

            Console.WriteLine(M1.ToString());
            Console.WriteLine(M2.ToString());
            Console.WriteLine((M1 * M2).ToString());
        }
    }
}
