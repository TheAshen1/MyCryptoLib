using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CryptoSystems.ParityCheckMatrixGenerators
{
    public class ParityCheckMatrixGeneratorEllyptic : IParityCheckMatrixGenerator
    {
        public List<Point> Points { get; private set; }
        public Terms Terms { get; private set; }

        private readonly int _degree;

        private EllypticCurve _ellypticCurve;
        private PolynomialOnGaloisField _polynomial;

        public ParityCheckMatrixGeneratorEllyptic(int degree)
        {
            _degree = degree;
        }

        public MatrixInt Generate(ILinearCode linearCode)
        {
            if (_ellypticCurve is null)
            {
                _ellypticCurve = new EllypticCurve(linearCode.GaloisField);
                Points = _ellypticCurve.CalculateAllZeroPoints();
            }

            if (Points.Count < linearCode.N)
            {
                throw new ParityCheckMatrixGeneratorException("The amout of acceptable point is too low for given linear code to generate ParityCheck Matrix.");
            }

            if (_polynomial is null)
            {
                _polynomial = new PolynomialOnGaloisField(_degree, linearCode.GaloisField);
            }

            Debug.WriteLine(_polynomial.Terms);

            #region Pick K random functions
            var functions = new List<int>();
            var j = 0;
            while (j < (linearCode.D))
            {
                var rand = new Random();
                var r = rand.Next(_polynomial.Terms.RowCount);
                if (!functions.Contains(r))
                {
                    functions.Add(r);
                    j++;
                }
            }
            Terms = new Terms(linearCode.D);
            for (var i = 0; i < functions.Count; i++)
            {
                Terms.SetTerm(i, _polynomial.Terms[functions[i], 0], _polynomial.Terms[functions[i], 1]);
            }
            Debug.WriteLine(Terms);
            #endregion

            #region Calculate parity check matrix
            var parityCheckMatrix = new MatrixInt(new int[linearCode.D, linearCode.N]);
            for (int row = 0; row < parityCheckMatrix.RowCount; row++)
            {
                for (int col = 0; col < parityCheckMatrix.ColumnCount; col++)
                {
                    parityCheckMatrix[row, col] = _polynomial.CalculateMember(functions[row], Points[col].x, Points[col].y);
                }
            }
            #endregion

            return parityCheckMatrix;
        }
    }
}
