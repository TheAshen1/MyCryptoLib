using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using System;
using System.Collections.Generic;

namespace CryptoSystems.ParityCheckMatrixGenerators
{
    public class ParityCheckMatrixGeneratorEllyptic : IParityCheckMatrixGenerator
    {
        private readonly int _degree;
        private readonly IList<int> _coefficient;

        private PolynomialOnGaloisField _ellipticCurve;

        public ParityCheckMatrixGeneratorEllyptic(int degree, IList<int> coefficient)
        {
            _degree = degree;
            _coefficient = coefficient;
        }

        public MatrixInt Generate(ILinearCode linearCode)
        {
            #region Generate points and functions
            if(_ellipticCurve is null)
            {
                _ellipticCurve = new PolynomialOnGaloisField(_degree, _coefficient, linearCode.GaloisField);

            }
            Console.WriteLine(_ellipticCurve.PolynomialMembers);

            var points = new List<(int, int, int)>();

            var fixedZValue = 1;
            for (int x = 0; x <= linearCode.GaloisField.WordCount; x++)
            {
                for (int y = 0; y <= linearCode.GaloisField.WordCount; y++)
                {
                    if (_ellipticCurve.Calculate(x, y) == 0)
                    {
                        points.Add((x, y, fixedZValue));
                        Console.WriteLine($"x: {x}, y: {y}, z: {fixedZValue}");
                    }
                }
            }

            var functions = _ellipticCurve.PolynomialMembers.Clone();
            #endregion

            if (points.Count < linearCode.N)
            {
                throw new ParityCheckMatrixGeneratorException("The amout of acceptable point is too low for given linear code to generate ParityCheck Matrix.");
            }


            #region Pick N random points and K random functions
            var Hdots = new MatrixInt(new int[0, _degree]);
            for (int i = 0; i < linearCode.N; i++)
            {
                var rand = new Random();
                var r = rand.Next(points.Count);
                Hdots = Hdots.AppendRows(new MatrixInt(new int[,] {
                    { points[r].Item1, points[r].Item2, points[r].Item3, }
                }));
                points.RemoveAt(r);
            }
            Hdots = Hdots.Transpose();
            Console.WriteLine("HDots");
            Console.WriteLine(Hdots);

            var Hpoly = new MatrixInt(new int[0, linearCode.K]);
            var selected = new List<int>();
            var j = 0;
            while (j < (linearCode.T * 2))
            {
                var rand = new Random();
                var r = rand.Next(functions.RowCount);
                if (!selected.Contains(r))
                {
                    Hpoly = Hpoly.AppendRows(functions.GetRow(r));
                    selected.Add(r);
                    j++;
                }
            }
            Console.WriteLine("Hpoly");
            Console.WriteLine(Hpoly);
            #endregion

            #region Calculate parity check matrix
            var parityCheckMatrix = new MatrixInt(new int[Hpoly.RowCount, Hdots.ColumnCount]);
            for (int row = 0; row < parityCheckMatrix.RowCount; row++)
            {
                for (int col = 0; col < parityCheckMatrix.ColumnCount; col++)
                {
                    parityCheckMatrix[row, col] = 1;
                    for (int k = 0; k < Hpoly.ColumnCount; k++)
                    {
                        var temp = linearCode.GaloisField.Power(Hdots[k, col], Hpoly[row, k]);
                        parityCheckMatrix[row, col] = linearCode.GaloisField.MultiplyWords(parityCheckMatrix[row, col], temp);
                    }
                }
            }
            #endregion

            return parityCheckMatrix;
        }
    }
}
