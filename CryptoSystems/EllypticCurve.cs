using CryptoSystems.Exceptions;
using CryptoSystems.Models;
using System;
using System.Collections.Generic;

namespace CryptoSystems
{
    public class EllypticCurve
    {
        private readonly GaloisField _galoisField;
        private readonly int _power;
        private readonly int _fixedZValue;

        public EllypticCurve(GaloisField galoisField)
        {
            if (galoisField.FieldPower % 2 != 0)
            {
                throw new EllypticCurveException("Field power has to be even");
            }
            _fixedZValue = 1;
            _galoisField = galoisField;

            var q = Math.Pow(_galoisField.Base, _galoisField.FieldPower);

            _power = (int)Math.Round(Math.Sqrt(q));
        }

        public List<Point> CalculateAllZeroPoints()
        {
            var points = new List<Point>();

            for (int y = 0; y <= _galoisField.WordCount; y++)
            {
                for (int x = 0; x <= _galoisField.WordCount; x++)
                {
                    if (Calculate(x, y) == 0)
                    {
                        points.Add(new Point(x, y, _fixedZValue));
                    }
                }
            }
            return points;
        }

        private int Calculate(int x, int y)
        {
            var functionOutput = 0;

            functionOutput = _galoisField.AddWords(functionOutput, _galoisField.Power(x, _power + 1));
            functionOutput = _galoisField.AddWords(functionOutput, _galoisField.MultiplyWords(_fixedZValue, _galoisField.Power(y, _power)));
            functionOutput = _galoisField.AddWords(functionOutput, _galoisField.MultiplyWords(y, _galoisField.Power(_fixedZValue, _power)));

            return functionOutput;
        }
    }
}
