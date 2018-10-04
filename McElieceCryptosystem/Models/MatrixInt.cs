using McElieceCryptosystem.Exceptions;
using System;
using System.Collections.Generic;

namespace McElieceCryptosystem.Models
{
    public class MatrixInt : MatrixBase<int>, IEquatable<MatrixInt>
    {
        #region Constructors
        public MatrixInt(int size) : base(size)
        {
        }

        public MatrixInt(int rows, int columns) : base(rows, columns)
        {
        }

        public MatrixInt(int[,] initialValues) : base(initialValues)
        {
        }

        public MatrixInt(MatrixBase<int> matrix) : base(matrix)
        {
        }

        public MatrixInt(List<int> list) : base(list)
        {
        }
        #endregion

        #region Methods

        #region Public Methods
        public override bool Equals(object obj)
        {
            var item = obj as MatrixInt;

            if (item == null)
            {
                return false;
            }

            return Equals(item);
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public bool Equals(MatrixInt other)
        {
            if (RowCount != other.RowCount)
            {
                throw new DimensionMismatchException("The number of rows in this matrix does not equal the number of rows in other matrix");
            }
            if (ColumnCount != other.ColumnCount)
            {
                throw new DimensionMismatchException("The number of columns in this matrix does not equal the number of columns in other matrix");
            }

            for (var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColumnCount; col++)
                {
                    if (Data[row, col] != other.Data[row, col])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public MatrixInt Transpose()
        {
            var rawResult = Transpose(this);

            var result = new MatrixInt(rawResult);
            return result;
        }

        public MatrixInt SubMatrix(
            RangeInt range)
        {
            var result = SubMatrix(range, range);
            return result;
        }

        public MatrixInt SubMatrix(
            RangeInt rowRange,
            RangeInt columnRange)
        {
            var rawResult = SubMatrix(this, rowRange, columnRange);
            var result = new MatrixInt(rawResult);
            return result;
        }

        public MatrixInt GetRangeOfRows(
            RangeInt rowRange)
        {
            var rawResult = GetRangeOfRows(this, rowRange);
            var result = new MatrixInt(rawResult);
            return result;
        }

        public MatrixInt GetRangeOfColumns(
            RangeInt columnRange)
        {
            var rawResult = GetRangeOfColumns(this, columnRange);
            var result = new MatrixInt(rawResult);
            return result;
        }

        public MatrixInt GetRow(
            int rowNumber)
        {
            var rawResult = GetRow(this, rowNumber);
            var result = new MatrixInt(rawResult);
            return result;
        }

        public MatrixInt GetColumn(
            int columnNumber)
        {
            var rawResult = GetColumn(this, columnNumber);
            var result = new MatrixInt(rawResult);
            return result;
        }

        public MatrixInt SwapColumns(int i, int j)
        {
            var rawResult = SwapColumns(this, i, j);
            var result = new MatrixInt(rawResult);
            return result;
        }

        public MatrixInt AppendRows(MatrixInt rows)
        {
            var rawResult = AppendRows(this, rows);
            var result = new MatrixInt(rawResult);
            return result;
        }

        public int FindColumn(MatrixInt column)
        {
            var result = FindColumn(this, column);
            return result;
        }

        #endregion Public Methods

        #region Static Methods
        public static MatrixInt Concatenate(
            MatrixInt matrixLeft,
            MatrixInt matrixRight)
        {
            var rawResult = MatrixBase<int>.Concatenate(matrixLeft, matrixRight);
            var result = new MatrixInt(rawResult);
            return result;
        }

        protected static MatrixInt Negative(
            MatrixInt matrix)
        {
            return new MatrixInt(
                UnaryElementWiseOperation(
                    matrix,
                    (a) => -a
                    ));

        }

        protected static MatrixInt Addition(
            MatrixInt matrixLeft,
            MatrixInt matrixRight)
        {
            return new MatrixInt(
                ElementWiseOperation(
                    matrixLeft,
                    matrixRight,
                    (a, b) => a + b
                    ));

        }

        protected static MatrixInt Addition(
            MatrixInt matrix,
            int scalar)
        {
            return new MatrixInt(
                ElementWiseOperation(
                    matrix,
                    scalar,
                    (a, b) => a + b
                    ));
        }

        protected static MatrixInt Subtraction(
            MatrixInt matrixLeft,
            MatrixInt matrixRight)
        {
            return new MatrixInt(
                ElementWiseOperation(
                    matrixLeft,
                    matrixRight,
                    (a, b) => a - b
                    ));

        }

        protected static MatrixInt Subtraction(
            MatrixInt matrix,
            int scalar)
        {
            return new MatrixInt(
                ElementWiseOperation(
                    matrix,
                    scalar,
                    (a, b) => a - b
                    ));

        }

        protected static MatrixInt Multiplication(
            MatrixInt matrixLeft,
            MatrixInt matrixRight)
        {
            return new MatrixInt(
                ElementWiseOperation(
                    matrixLeft,
                    matrixRight,
                    (a, b) => a * b
                    ));

        }

        protected static MatrixInt DotMultiplication(
            MatrixInt matrixLeft,
            MatrixInt matrixRight)
        {
            if (matrixLeft.ColumnCount != matrixRight.RowCount)
            {
                throw new DimensionMismatchException("Number of columns in first matrix does not equal number of rows in second matrix.");
            }

            var rawResult = new int[matrixLeft.RowCount, matrixRight.ColumnCount];

            for (int row = 0; row < rawResult.GetLength(0); row++)
            {
                for (int col = 0; col < rawResult.GetLength(1); col++)
                {
                    int sum = 0;
                    for (int k = 0; k < matrixLeft.ColumnCount; k++)
                    {
                        sum += matrixLeft.Data[row, k] * matrixRight.Data[k, col];
                    }
                    rawResult[row, col] = sum;
                }
            }
            MatrixInt result = new MatrixInt(rawResult);
            return result;
        }

        protected static MatrixInt Multiplication(
            MatrixInt matrix,
            int scalar)
        {
            return new MatrixInt(
                ElementWiseOperation(
                    matrix,
                    scalar,
                    (a, b) => a * b
                    ));

        }

        protected static MatrixInt Mod(
            MatrixInt matrix,
            int scalar)
        {
            return new MatrixInt(
               ElementWiseOperation(
                   matrix,
                   scalar,
                   (a, b) => a % b
                   ));
        }

        #endregion Static Methods

        #endregion Methods

        #region Operator Overloads
        public static MatrixInt operator -(MatrixInt matrix)
        {
            return Negative(matrix);
        }

        public static MatrixInt operator +(MatrixInt matrixLeft, MatrixInt matrixRight)
        {
            return Addition(matrixLeft, matrixRight);
        }

        public static MatrixInt operator +(MatrixInt matrix, int scalar)
        {
            return Addition(matrix, scalar);
        }

        public static MatrixInt operator +(int scalar, MatrixInt matrix)
        {
            return Addition(matrix, scalar);
        }

        public static MatrixInt operator -(MatrixInt matrixLeft, MatrixInt matrixRight)
        {
            return Subtraction(matrixLeft, matrixRight);
        }

        public static MatrixInt operator -(MatrixInt matrix, int scalar)
        {
            return Subtraction(matrix, scalar);
        }

        public static MatrixInt operator *(MatrixInt matrixLeft, MatrixInt matrixRight)
        {
            return DotMultiplication(matrixLeft, matrixRight);
        }

        public static MatrixInt operator *(MatrixInt matrix, int scalar)
        {
            return Multiplication(matrix, scalar);
        }

        public static MatrixInt operator %(MatrixInt matrix, int scalar)
        {
            return Mod(matrix, scalar);
        }

        public static MatrixInt operator ^(MatrixInt matrixLeft, MatrixInt matrixRight)
        {
            return Multiplication(matrixLeft, matrixRight);
        }

        public static MatrixInt operator |(MatrixInt matrixLeft, MatrixInt matrixRight)
        {
            return Concatenate(matrixLeft, matrixRight);
        }

        public static bool operator ==(MatrixInt matrixLeft, MatrixInt matrixRight)
        {
            return matrixLeft.Equals(matrixRight);
        }

        public static bool operator !=(MatrixInt matrixLeft, MatrixInt matrixRight)
        {
            return !matrixLeft.Equals(matrixRight);
        }
        #endregion
    }
}
