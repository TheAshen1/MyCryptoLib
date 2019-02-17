using CryptoSystems.Exceptions;
using CryptoSystems.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoSystems
{
    public class PolynomialOnGaloisField
    {
        public GaloisField GaloisField { get; set; }
        public MatrixInt PolynomialMembers { get; }
        public List<int> Coefficients { get; }


        public PolynomialOnGaloisField(int degree, GaloisField galoisField)
        {
            GaloisField = galoisField;
            PolynomialMembers = CalculatePolynomialMembers(degree);
            Coefficients = Enumerable.Repeat(1, PolynomialMembers.RowCount).ToList();
        }

        public PolynomialOnGaloisField(int degree, IList<int> coefficients, GaloisField galoisField)
        {
            GaloisField = galoisField;
            PolynomialMembers = CalculatePolynomialMembers(degree);

            Coefficients = new List<int>();
            for (int i = 0; i < PolynomialMembers.RowCount; i++)
            {
                if(i >= coefficients.Count)
                {
                    Coefficients.Add(0);
                    continue;
                }
                Coefficients.Add(coefficients[i]);
            }
        }

        public int Calculate(int x, int y, int? z = null)
        {
            var functionOutput = 0;

            for (int i = 0; i < PolynomialMembers.RowCount; i++)
            {
                if (Coefficients[i] == 0)
                {
                    continue;
                }

                var memberValue = Coefficients[i];
                var powerForX = PolynomialMembers[i, 0];
                var powerForY = PolynomialMembers[i, 1];
                var powerForZ = PolynomialMembers[i, 2];

                memberValue = GaloisField.MultiplyWords(memberValue, GaloisField.Power(x, powerForX));
                memberValue = GaloisField.MultiplyWords(memberValue, GaloisField.Power(y, powerForY));
                if(z.HasValue)
                    memberValue = GaloisField.MultiplyWords(memberValue, GaloisField.Power(z.Value, powerForZ));
                try
                {
                    functionOutput = GaloisField.AddWords(functionOutput, memberValue);

                }
                catch (Exception ex) when (functionOutput == -1)
                {

                }
            }

            return functionOutput;
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
