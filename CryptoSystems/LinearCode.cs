using CryptoSystems.Algorithms;
using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using CryptoSystems.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSystems
{
    public class LinearCode : ILinearCode
    {
        public int N { get; }

        public int K { get; }

        public int D { get; }

        public int T { get; }

        public int MinimumDistance => throw new NotImplementedException();

        public int CanDetectUpTo => throw new NotImplementedException();

        public MatrixInt GeneratorMatrix { get; set; }

        public MatrixInt ParityCheckMatrix { get; set; }

        public GaloisField GaloisField { get; }


        public LinearCode(int n, int k, int d, int t, GaloisField galoisField, IParityCheckMatrixGenerator parityCheckMatrixGenerator)
        {
            N = n;
            K = k;
            D = d;
            T = t;
            GaloisField = galoisField;
        }


        public MatrixInt DecodeAndCorrect(MatrixInt message)
        {
            throw new NotImplementedException();
        }

        public MatrixInt Encode(MatrixInt message)
        {
            throw new NotImplementedException();
        }

        public MatrixInt Encode(MatrixInt message, MatrixInt errorVector)
        {
            throw new NotImplementedException();
        }

    }
}
