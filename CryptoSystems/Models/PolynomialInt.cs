using CryptoSystems.Exceptions;
using CryptoSystems.Utility;
using System;

namespace CryptoSystems.Models
{
    public class PolynomialInt : PolynomialBase<int>, IEquatable<PolynomialInt>
    {
        #region Properties
        private int _degree = -1;
        public int Degree
        {
            get
            {
                if (_degree == -1)
                {
                    for (int i = Length - 1; i >= 0; i--)
                    {
                        if (Coefficients[i] != 0)
                        {
                            _degree = i;
                            break;
                        }
                    }
                }
                return _degree;
            }
        }
        #endregion

        #region Constructors
        public PolynomialInt(int length) : base(length)
        {
        }

        public PolynomialInt(int[] initialValues) : base(initialValues)
        {
        }

        public PolynomialInt(PolynomialBase<int> polynomialBase) : base(polynomialBase)
        {
        }
        #endregion

        #region Static methods
        protected static PolynomialInt Addition(PolynomialInt polynomialLeft, PolynomialInt polynomialRight)
        {
            var resultLength = Math.Max(polynomialLeft.Length, polynomialRight.Length);
            var rawResult = new int[resultLength];

            for (int i = 0; i < resultLength; i++)
            {
                rawResult[i] =
                    i < polynomialLeft.Length ? polynomialLeft.Coefficients[i] : 0 +
                    i < polynomialRight.Length ? polynomialRight.Coefficients[i] : 0;
            }

            var result = new PolynomialInt(rawResult);
            return result;
        }

        protected static PolynomialInt Addition(PolynomialInt polynomial, int scalar)
        {
            return new PolynomialInt(
                ElementWiseOperation(
                    polynomial,
                    scalar,
                    (a, b) => a + b
                    ));
        }

        protected static PolynomialInt Subtraction(PolynomialInt polynomialLeft, PolynomialInt polynomialRight)
        {
            var resultLength = Math.Max(polynomialLeft.Length, polynomialRight.Length);
            var rawResult = new int[resultLength];

            for (int i = 0; i < resultLength; i++)
            {
                rawResult[i] =
                    i < polynomialLeft.Length ? polynomialLeft.Coefficients[i] : 0 -
                    i < polynomialRight.Length ? polynomialRight.Coefficients[i] : 0;
            }

            var result = new PolynomialInt(rawResult);
            return result;
        }

        protected static PolynomialInt Multiplication(PolynomialInt polynomialLeft, PolynomialInt polynomialRight)
        {
            if (polynomialLeft.IsZero() || polynomialRight.IsZero())
            {
                return Helper.ZeroPolynomialInt();
            }

            var resultLength = polynomialLeft.Length + polynomialRight.Length;
            var rawResult = new int[resultLength];

            for (int i = 0; i < polynomialLeft.Length; i++)
            {
                for (int j = 0; j < polynomialRight.Length; j++)
                {
                    rawResult[i + j] = polynomialLeft.Coefficients[i] * polynomialRight.Coefficients[j];
                }
            }

            var result = new PolynomialInt(rawResult);
            return result;
        }

        protected static PolynomialInt Multiplication(PolynomialInt polynomial, int scalar)
        {
            return new PolynomialInt(
                ElementWiseOperation(
                    polynomial,
                    scalar,
                    (a, b) => a * b
                    ));
        }

        //protected static PolynomialInt Division(PolynomialInt polynomialDivident, PolynomialInt polynomialDivisor)
        //{
        //    if (polynomialDivisor.Degree < 0)
        //    {
        //        throw new ArgumentException("Can't divide by empty polynomial");
        //    }

        //    if (polynomialDivident.Degree < polynomialDivisor.Degree)
        //    {
        //        throw new DimensionMismatchException("The degree of the divisor cannot exceed that of the divident");
        //    }

        //    while (polynomialDivident.Degree >= polynomialDivisor.Degree)
        //    {
        //        var divisorShifted = polynomialDivisor >> (polynomialDivident.Degree - polynomialDivisor.Degree);

        //        var coefficient = new int[] {
        //            polynomialDivident.Coefficients[polynomialDivident.Degree] / };

        //    }
        //}

        protected static PolynomialInt Mod(PolynomialInt polynomial, int scalar)
        {
            return new PolynomialInt(
               ElementWiseOperation(
                   polynomial,
                   scalar,
                   (a, b) => a % b
                   ));
        }

        protected static PolynomialInt ShiftRight(PolynomialInt polynomial, int shift)
        {
            if (shift == 0)
            {
                return polynomial;
            }

            shift = Math.Abs(shift);

            if (shift > polynomial.Length)
            {
                throw new DimensionMismatchException("Polynomial shift is greater than the length of a polynomial");
            }

            var rawResult = new int[polynomial.Length];

            for (int i = 0; i < polynomial.Length; i++)
            {
                rawResult[i + shift] = polynomial.Coefficients[i];
            }

            var result = new PolynomialInt(rawResult);
            return result;
        }

        protected static PolynomialInt ShiftLeft(PolynomialInt polynomial, int shift)
        {
            if (shift == 0)
            {
                return polynomial;
            }

            shift = Math.Abs(shift);

            if (shift > polynomial.Length)
            {
                throw new DimensionMismatchException("Polynomial shift is greater than the length of a polynomial");
            }

            var rawResult = new int[polynomial.Length];


            for (int i = 0; i < polynomial.Length + shift; i++)
            {
                rawResult[i] = polynomial.Coefficients[i + shift];
            }

            var result = new PolynomialInt(rawResult);
            return result;
        }
        #endregion

        #region Public methods


        public bool IsZero()
        {
            foreach (var value in Coefficients)
            {
                if (value != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Equals(PolynomialInt other)
        {
            if (Length != other.Length)
            {
                throw new DimensionMismatchException("The length of this polynomial does not equal the length of other polynomial");
            }

            for (var i = 0; i < Length; i++)
            {

                if (Coefficients[i] != other.Coefficients[i])
                {
                    return false;
                }

            }
            return true;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Operator overloads
        public static bool operator ==(PolynomialInt polynomialLeft, PolynomialInt polynomialRight)
        {
            return polynomialLeft.Equals(polynomialRight);
        }

        public static bool operator !=(PolynomialInt polynomialLeft, PolynomialInt polynomialRight)
        {
            return !polynomialLeft.Equals(polynomialRight);
        }

        public static PolynomialInt operator +(PolynomialInt polynomialLeft, PolynomialInt polynomialRight)
        {
            return Addition(polynomialLeft, polynomialRight);
        }

        public static PolynomialInt operator +(PolynomialInt polynomial, int scalar)
        {
            return Addition(polynomial, scalar);
        }

        public static PolynomialInt operator *(PolynomialInt polynomialLeft, PolynomialInt polynomialRight)
        {
            return Multiplication(polynomialLeft, polynomialRight);
        }

        public static PolynomialInt operator *(PolynomialInt polynomial, int scalar)
        {
            return Multiplication(polynomial, scalar);
        }

        public static PolynomialInt operator %(PolynomialInt polynomial, int scalar)
        {
            return Mod(polynomial, scalar);
        }

        public static PolynomialInt operator >>(PolynomialInt polynomial, int scalar)
        {
            return ShiftRight(polynomial, scalar);
        }

        public static PolynomialInt operator <<(PolynomialInt polynomial, int scalar)
        {
            return ShiftLeft(polynomial, scalar);
        }
        #endregion
    }
}
