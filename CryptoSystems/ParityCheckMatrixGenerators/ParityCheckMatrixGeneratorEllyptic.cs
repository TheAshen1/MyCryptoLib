using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using System;
using System.Collections.Generic;

namespace CryptoSystems.ParityCheckMatrixGenerators
{
    public class ParityCheckMatrixGeneratorEllyptic : IParityCheckMatrixGenerator
    {
        public List<Point> Points { get; private set; }

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

            Console.WriteLine(_polynomial.Members);

            var functions = new List<int>()
            {
                5,2,4,0,1,3
            };
            #region Pick K random functions
            var selectedFunctions = new List<int>();
            var j = 0;
            while (j < (linearCode.D))
            {
                var rand = new Random();
                var r = rand.Next(_polynomial.Members.RowCount);
                if (!selectedFunctions.Contains(r))
                {
                    selectedFunctions.Add(r);
                    j++;
                }
            }
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
