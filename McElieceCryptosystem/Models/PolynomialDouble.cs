using McElieceCryptosystem.Exceptions;
using McElieceCryptosystem.Util;
using System;
using System.Text;

namespace McElieceCryptosystem.Models
{
    public class PolynomialDouble : PolynomialBase<double>, IEquatable<PolynomialDouble>
    {
        static readonly double Epsilon = 0.000_000_000_000_001;

        #region Properties
        private int _degree = -1;
        public int Degree
        {
            get
            {
                if (_degree == -1)
                {
                    _degree = 0;
                    for (int i = Length - 1; i >= 0; i--)
                    {
                        if (Math.Abs(Coefficients[i]) > Epsilon)
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
        public PolynomialDouble(int length) : base(length)
        {
        }

        public PolynomialDouble(int degree, double value) : base(degree, value)
        {
        }

        public PolynomialDouble(double[] initialValues) : base(initialValues)
        {
        }

        public PolynomialDouble(PolynomialBase<double> polynomialBase) : base(polynomialBase)
        {
        }

        public PolynomialDouble(MatrixBase<double> baseMatrix) : base(baseMatrix)
        {
        }
        #endregion

        #region Static methods
        protected static PolynomialDouble Addition(PolynomialDouble polynomialLeft, PolynomialDouble polynomialRight)
        {
            var resultLength = Math.Max(polynomialLeft.Length, polynomialRight.Length);
            var rawResult = new double[resultLength];

            for (int i = 0; i < resultLength; i++)
            {
                rawResult[i] =
                    (i < polynomialLeft.Length ? polynomialLeft.Coefficients[i] : 0.0) +
                    (i < polynomialRight.Length ? polynomialRight.Coefficients[i] : 0.0);
            }

            var result = new PolynomialDouble(rawResult);
            return result;
        }

        protected static PolynomialDouble Addition(PolynomialDouble polynomial, double scalar)
        {
            return new PolynomialDouble(
                ElementWiseOperation(
                    polynomial,
                    scalar,
                    (a, b) => a + b
                    ));
        }

        protected static PolynomialDouble Subtraction(PolynomialDouble polynomialLeft, PolynomialDouble polynomialRight)
        {
            var resultLength = Math.Max(polynomialLeft.Length, polynomialRight.Length);
            var rawResult = new double[resultLength];

            for (int i = 0; i < resultLength; i++)
            {
                rawResult[i] =
                    (i < polynomialLeft.Length ? polynomialLeft.Coefficients[i] : 0.0) -
                    (i < polynomialRight.Length ? polynomialRight.Coefficients[i] : 0.0);
            }

            var result = new PolynomialDouble(rawResult);
            return result;
        }

        protected static PolynomialDouble Subtraction(PolynomialDouble polynomial, double scalar)
        {
            return new PolynomialDouble(
               ElementWiseOperation(
                   polynomial,
                   scalar,
                   (a, b) => a - b
                   ));
        }

        protected static PolynomialDouble Multiplication(PolynomialDouble polynomialLeft, PolynomialDouble polynomialRight)
        {
            if (polynomialLeft.IsZero() || polynomialRight.IsZero())
            {
                return Utility.ZeroPolynomialDouble();
            }

            var resultLength = polynomialLeft.Length + polynomialRight.Length;
            var rawResult = new double[resultLength];

            for (int i = 0; i < polynomialLeft.Length; i++)
            {
                for (int j = 0; j < polynomialRight.Length; j++)
                {
                    rawResult[i + j] += polynomialLeft.Coefficients[i] * polynomialRight.Coefficients[j];
                }
            }

            var result = new PolynomialDouble(rawResult);
            return result;
        }

        protected static PolynomialDouble Multiplication(PolynomialDouble polynomial, double scalar)
        {
            return new PolynomialDouble(
                ElementWiseOperation(
                    polynomial,
                    scalar,
                    (a, b) => a * b
                    ));
        }

        protected static PolynomialLongDivisionResult Division(PolynomialDouble polynomialDivident, PolynomialDouble polynomialDivisor)
        {
            if (polynomialDivisor.Degree < 0)
            {
                throw new ArgumentException("Can't divide by empty polynomial");
            }

            if (polynomialDivident.Degree < polynomialDivisor.Degree)
            {
                return new PolynomialLongDivisionResult
                {
                    Result = new PolynomialDouble(0, 0.0),
                    Mod = polynomialDivident
                };
            }

            var divident = polynomialDivident.Clone();

            var rawResult = new double[polynomialDivident.Length];
            while (divident.Degree >= polynomialDivisor.Degree)
            {
                var degreeDifference = divident.Degree - polynomialDivisor.Degree;
                var divisorShifted = polynomialDivisor >> (degreeDifference);

                rawResult[degreeDifference] = divident.Coefficients[divident.Degree] / divisorShifted.Coefficients[divident.Degree];

                divisorShifted = divisorShifted * rawResult[degreeDifference];

                divident = divident - divisorShifted;
            }

            var result = new PolynomialLongDivisionResult
            {
                Result = new PolynomialDouble(rawResult),
                Mod = divident
            };
            return result;
        }

        protected static PolynomialDouble Mod(PolynomialDouble polynomial, int scalar)
        {
            return new PolynomialDouble(
               ElementWiseOperation(
                   polynomial,
                   scalar,
                   (a, b) => a % b
                   ));
        }

        protected static PolynomialDouble ShiftRight(PolynomialDouble polynomial, int shift)
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

            var resultLength = polynomial.Length + shift;
            var rawResult = new double[resultLength];

            for (int i = 0; i < polynomial.Length; i++)
            {
                rawResult[i + shift] = polynomial.Coefficients[i];
            }

            var result = new PolynomialDouble(rawResult);
            return result;
        }

        protected static PolynomialDouble ShiftLeft(PolynomialDouble polynomial, int shift)
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

            var rawResult = new double[polynomial.Length];


            for (int i = 0; i < polynomial.Length + shift; i++)
            {
                rawResult[i] = polynomial.Coefficients[i + shift];
            }

            var result = new PolynomialDouble(rawResult);
            return result;
        }
        #endregion

        #region Public methods
        public new PolynomialDouble Clone()
        {
            var baseCopy = base.Clone();
            return new PolynomialDouble(baseCopy);
        }

        public bool IsZero()
        {
            foreach (var value in Coefficients)
            {
                if (Math.Abs(value) > Epsilon)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Equals(PolynomialDouble other)
        {
            if (Degree != other.Degree)
            {
                return false;
            }

            var length = Math.Max(Length, other.Length);
            for (var i = 0; i < length; i++)
            {

                if (Math.Abs(
                    (i < Length ? Coefficients[i] : 0.0) - (i < other.Length ? other.Coefficients[i] : 0.0 )
                    ) > Epsilon)
                {
                    return false;
                }

            }
            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i <= Degree; i++)
            {
                sb.Append(Coefficients[i].ToString() + " ");
            }
            return sb.ToString();
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
        public static bool operator ==(PolynomialDouble polynomialLeft, PolynomialDouble polynomialRight)
        {
            return polynomialLeft.Equals(polynomialRight);
        }

        public static bool operator !=(PolynomialDouble polynomialLeft, PolynomialDouble polynomialRight)
        {
            return !polynomialLeft.Equals(polynomialRight);
        }

        public static PolynomialDouble operator +(PolynomialDouble polynomialLeft, PolynomialDouble polynomialRight)
        {
            return Addition(polynomialLeft, polynomialRight);
        }

        public static PolynomialDouble operator +(PolynomialDouble polynomial, double scalar)
        {
            return Addition(polynomial, scalar);
        }

        public static PolynomialDouble operator -(PolynomialDouble polynomialLeft, PolynomialDouble polynomialRight)
        {
            return Subtraction(polynomialLeft, polynomialRight);
        }

        public static PolynomialDouble operator -(PolynomialDouble polynomial, double scalar)
        {
            return Subtraction(polynomial, scalar);
        }

        public static PolynomialDouble operator *(PolynomialDouble polynomialLeft, PolynomialDouble polynomialRight)
        {
            return Multiplication(polynomialLeft, polynomialRight);
        }

        public static PolynomialDouble operator *(PolynomialDouble polynomial, double scalar)
        {
            return Multiplication(polynomial, scalar);
        }

        public static PolynomialLongDivisionResult operator /(PolynomialDouble polynomialLeft, PolynomialDouble polynomialRight)
        {
            return Division(polynomialLeft, polynomialRight);
        }


        public static PolynomialDouble operator %(PolynomialDouble polynomial, int scalar)
        {
            return Mod(polynomial, scalar);
        }

        public static PolynomialDouble operator >>(PolynomialDouble polynomial, int scalar)
        {
            return ShiftRight(polynomial, scalar);
        }

        public static PolynomialDouble operator <<(PolynomialDouble polynomial, int scalar)
        {
            return ShiftLeft(polynomial, scalar);
        }
        #endregion
    }
}
