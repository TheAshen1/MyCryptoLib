﻿using CryptoSystems.Models;
using System.Collections.Generic;
using System.Linq;

namespace CryptoSystems
{
    public class PolynomialOnGaloisField
    {
        public MatrixInt Terms { get; }

        private readonly GaloisField _galoisField;
        private readonly List<int> _coefficients;

        public PolynomialOnGaloisField(int degree, GaloisField galoisField)
        {
            _galoisField = galoisField;
            Terms = CalculatePolynomialMembers(degree);
            _coefficients = Enumerable.Repeat(1, Terms.RowCount).ToList();
        }

        public PolynomialOnGaloisField(int degree, IList<int> coefficients, GaloisField galoisField)
        {
            _galoisField = galoisField;
            Terms = CalculatePolynomialMembers(degree);

            _coefficients = new List<int>();
            for (int i = 0; i < Terms.RowCount; i++)
            {
                if (i >= coefficients.Count)
                {
                    _coefficients.Add(0);
                    continue;
                }
                _coefficients.Add(coefficients[i]);
            }
        }

        public int Calculate(int x, int y, int? z = null)
        {
            var functionOutput = 0;

            for (int i = 0; i < Terms.RowCount; i++)
            {
                if (_coefficients[i] == 0)
                {
                    continue;
                }

                var memberValue = _coefficients[i];
                var powerForX = Terms[i, 0];
                var powerForY = Terms[i, 1];
                var powerForZ = Terms[i, 2];

                memberValue = _galoisField.MultiplyWords(memberValue, _galoisField.Power(x, powerForX));
                memberValue = _galoisField.MultiplyWords(memberValue, _galoisField.Power(y, powerForY));
                if (z.HasValue)
                    memberValue = _galoisField.MultiplyWords(memberValue, _galoisField.Power(z.Value, powerForZ));

                functionOutput = _galoisField.AddWords(functionOutput, memberValue);
            }

            return functionOutput;
        }

        public int CalculateMember(int position, int x, int y, int? z = null)
        {
            var memberValue = 1;

            var powerForX = Terms[position, 0];
            var powerForY = Terms[position, 1];
            var powerForZ = Terms[position, 2];

            memberValue = _galoisField.MultiplyWords(memberValue, _galoisField.Power(x, powerForX));
            memberValue = _galoisField.MultiplyWords(memberValue, _galoisField.Power(y, powerForY));
            if (z.HasValue)
                memberValue = _galoisField.MultiplyWords(memberValue, _galoisField.Power(z.Value, powerForZ));

            return memberValue;
        }



        private MatrixInt CalculatePolynomialMembers(int degree)
        {
            var polynomialMembers = new MatrixInt(new int[0, 3]);

            for (int x = degree; x >= 0; x--)
            {
                for (int y = degree - x; y >= 0; y--)
                {
                    var z = degree - x - y;

                    polynomialMembers = polynomialMembers.AppendRows(new MatrixInt(new int[,]
                    {
                        { x, y, z }
                    }));
                }
            }

            return polynomialMembers;
        }

    }
}
