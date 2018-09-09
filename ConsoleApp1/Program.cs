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
            var M = new MatrixBase<int>(3, 6);
            Console.Write(M.ToString());
            Console.Write(M.Transpose().ToString());
        }
    }
}
