using McElieceCryptosystem.Exceptions;
using System;
using System.Text;

namespace McElieceCryptosystem.Models
{
    public class PolynomialBase<T>
    {
        #region Constructors
        public PolynomialBase(int length)
        {
            if(length < 0)
            {
                throw new DimensionMismatchException("Polynomial's length can't be less than 0");
            }

            Length = length;
            Coefficients = new T[Length];
        }

        public PolynomialBase(int degree, T value)
        {
            if (degree < 0)
            {
                throw new DimensionMismatchException("Polynomial's length can't be less than 0");
            }

            Length = degree + 1;
            Coefficients = new T[Length];
            Coefficients[degree] = value;
        }

        public PolynomialBase(T[] initialValues)
        {
            Length = initialValues.Length;
            Coefficients = new T[Length];

            for (var i = 0; i < Length; i++)
            {
                Coefficients[i] = initialValues[i];
            }
        }

        public PolynomialBase(PolynomialBase<T> polynomialBase)
        {
            Length = polynomialBase.Length;
            Coefficients = new T[Length];

            for (var i = 0; i < Length; i++)
            {
                Coefficients[i] = polynomialBase.Coefficients[i];
            }
        }

        public PolynomialBase(MatrixBase<T> baseMatrix)
        {
            if(baseMatrix.RowCount > 1)
            {
                throw new DimensionMismatchException("Base matrix for shoud consist of single row");
            }

            Length = baseMatrix.ColumnCount;
            Coefficients = new T[Length];

            for (var i = 0; i < Length; i++)
            {
                Coefficients[i] = baseMatrix.Data[0, i];
            }
        }
        #endregion

        #region Properties
        public int Length { get; }
        public T[] Coefficients { get; }
        #endregion


        #region Static methods
        protected static PolynomialBase<T> ElementWiseOperation(
           PolynomialBase<T> polynomialLeft,
           PolynomialBase<T> polynomialRight,
           Func<T, T, T> operation)
        {
            if (polynomialLeft.Length != polynomialRight.Length)
            {
                throw new DimensionMismatchException("The length of this polynomial does not equal the length of other polynomial");
            }

            var rawResult = new T[polynomialLeft.Length];
            for (int i = 0; i < polynomialLeft.Length; i++)
            {
                rawResult[i] = operation(polynomialLeft.Coefficients[i], polynomialRight.Coefficients[i]);
            }

            PolynomialBase<T> result = new PolynomialBase<T>(rawResult);
            return result;
        }

        protected static PolynomialBase<T> ElementWiseOperation(
            PolynomialBase<T> polynomial,
            T scalar,
            Func<T, T, T> operation)
        {
            var rawResult = new T[polynomial.Length];
            for (int i = 0; i < polynomial.Length; i++)
            {
                rawResult[i] = operation(polynomial.Coefficients[i], scalar);
            }

            PolynomialBase<T> result = new PolynomialBase<T>(rawResult);
            return result;
        }
        #endregion

        #region Public methods
        public PolynomialBase<T> Clone()
        {
            var rawCopy = new T[Length];

            for (int i = 0; i < Length; i++)
            {
                rawCopy[i] = Coefficients[i];
            }

            return new PolynomialBase<T>(rawCopy);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < Length; i++)
            {
                sb.Append(Coefficients[i].ToString() + " ");
            }
            return sb.ToString();
        }
        #endregion

    }
}
