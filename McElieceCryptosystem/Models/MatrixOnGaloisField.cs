using System;
using System.Collections.Generic;
using CryptoSystems;
using CryptoSystems.Exceptions;
using CryptoSystems.Models;

namespace McElieceCryptosystem.Models
{
    public class MatrixOnGaloisField : MatrixBase<int>, IEquatable<MatrixOnGaloisField>
    {
        #region Properties
        public GaloisField GaloisField { get; }
        #endregion

        #region Constructors
        public MatrixOnGaloisField(int size, GaloisField galoisField) : base(size)
        {
            GaloisField = galoisField;
        }

        public MatrixOnGaloisField(int[] initialRow, GaloisField galoisField) : base(initialRow)
        {
            GaloisField = galoisField;
        }

        public MatrixOnGaloisField(int[,] initialValues, GaloisField galoisField) : base(initialValues)
        {
            GaloisField = galoisField;
            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColumnCount; col++)
                {
                    this[row, col] = this[row, col] % galoisField.WordCount;
                }
            }

        }

        public MatrixOnGaloisField(MatrixBase<int> matrix, GaloisField galoisField) : base(matrix)
        {
            GaloisField = galoisField;
        }

        public MatrixOnGaloisField(MatrixOnGaloisField matrix) : base(matrix)
        {
            GaloisField = matrix.GaloisField;
        }

        public MatrixOnGaloisField(List<int> list, GaloisField galoisField) : base(list)
        {
            GaloisField = galoisField;
        }

        public MatrixOnGaloisField(int rows, int columns, GaloisField galoisField) : base(rows, columns)
        {
            GaloisField = galoisField;
        }
        #endregion

        #region Methods

        #region Public Methods
        public override bool Equals(object obj)
        {
            var item = obj as MatrixOnGaloisField;

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

        public bool Equals(MatrixOnGaloisField other)
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
                    if (this[row, col] != other[row, col])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public MatrixOnGaloisField Transpose()
        {
            var rawResult = Transpose(this);

            var result = new MatrixOnGaloisField(rawResult, GaloisField);
            return result;
        }

        public MatrixOnGaloisField SubMatrix(
            RangeInt range)
        {
            var result = SubMatrix(range, range);
            return result;
        }

        public MatrixOnGaloisField SubMatrix(
            RangeInt rowRange,
            RangeInt columnRange)
        {
            var baseResult = SubMatrix(this, rowRange, columnRange);
            var result = new MatrixOnGaloisField(baseResult, GaloisField);
            return result;
        }

        public MatrixOnGaloisField GetRangeOfRows(
            RangeInt rowRange)
        {
            var baseResult = GetRangeOfRows(this, rowRange);
            var result = new MatrixOnGaloisField(baseResult, GaloisField);
            return result;
        }

        public MatrixOnGaloisField GetRangeOfColumns(
            RangeInt columnRange)
        {
            var baseResult = GetRangeOfColumns(this, columnRange);
            var result = new MatrixOnGaloisField(baseResult, GaloisField);
            return result;
        }

        public MatrixOnGaloisField GetRow(
            int rowNumber)
        {
            var baseResult = GetRow(this, rowNumber);
            return new MatrixOnGaloisField(baseResult, GaloisField);
        }

        public MatrixOnGaloisField GetColumn(
            int columnNumber)
        {
            var baseResult = GetColumn(this, columnNumber);
            return new MatrixOnGaloisField(baseResult, GaloisField);
        }

        public MatrixOnGaloisField SwapColumns(int i, int j)
        {
            var baseResult = SwapColumns(this, i, j);
            return new MatrixOnGaloisField(baseResult, GaloisField);
        }

        public MatrixOnGaloisField SwapRows(int i, int j)
        {
            var baseResult = SwapRows(this, i, j);
            return new MatrixOnGaloisField(baseResult, GaloisField);
        }

        public MatrixOnGaloisField AppendRows(MatrixOnGaloisField rows)
        {
            var baseResult = AppendRows(this, rows);
            return new MatrixOnGaloisField(baseResult, GaloisField);
        }

        internal int FindRow(MatrixOnGaloisField row)
        {
            var result = FindRow(this, row);
            return result;
        }


        public int FindColumn(MatrixOnGaloisField column)
        {
            var result = FindColumn(this, column);
            return result;
        }

        public new MatrixOnGaloisField Clone()
        {
            return new MatrixOnGaloisField(this);
        }
        #endregion


        #region Static Methods
        public static MatrixOnGaloisField Concatenate(
         MatrixOnGaloisField matrixLeft,
         MatrixOnGaloisField matrixRight)
        {
            var baseResult = MatrixBase<int>.Concatenate(matrixLeft, matrixRight);
            var result = new MatrixOnGaloisField(baseResult, matrixLeft.GaloisField);
            return result;
        }

        protected static MatrixOnGaloisField Addition(
            MatrixOnGaloisField matrixLeft,
            MatrixOnGaloisField matrixRight)
        {
            return new MatrixOnGaloisField(
                ElementWiseOperation(
                    matrixLeft,
                    matrixRight,
                    (a, b) => matrixLeft.GaloisField.AddWords(a, b)),
                matrixLeft.GaloisField);
        }

        protected static MatrixOnGaloisField Addition(
            MatrixOnGaloisField matrix,
            int scalar)
        {
            return new MatrixOnGaloisField(
                ElementWiseOperation(
                    matrix,
                    scalar,
                    (a, b) => matrix.GaloisField.AddWords(a, b)),
                matrix.GaloisField);
        }

        protected static MatrixOnGaloisField Multiplication(
            MatrixOnGaloisField matrixLeft,
            MatrixOnGaloisField matrixRight)
        {
            return new MatrixOnGaloisField(
                ElementWiseOperation(
                    matrixLeft,
                    matrixRight,
                    (a, b) => matrixLeft.GaloisField.MultiplyWords(a, b)
                    ),
                matrixLeft.GaloisField);

        }

        public static MatrixOnGaloisField DotMultiplication(MatrixOnGaloisField matrixLeft, MatrixOnGaloisField matrixRight)
        {
            if (matrixLeft.ColumnCount != matrixRight.RowCount)
            {
                throw new DimensionMismatchException("Number of columns in first matrix does not equal number of rows in second matrix.");
            }

            var rowCount = matrixLeft.RowCount;
            var columnCount = matrixRight.ColumnCount;

            var rawResult = new int[rowCount, columnCount];

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    int sum = -1;
                    for (int k = 0; k < matrixLeft.ColumnCount; k++)
                    {
                        var product = matrixLeft.GaloisField.MultiplyWords(matrixLeft.Data[row, k], matrixRight.Data[k, col]);
                        sum = matrixLeft.GaloisField.AddWords(sum, product);
                    }
                    rawResult[row, col] = sum;
                }
            }

            var result = new MatrixInt(rawResult);
            return result;
        }

        protected static MatrixOnGaloisField Multiplication(
            MatrixOnGaloisField matrix,
            int scalar)
        {
            return new MatrixOnGaloisField(
                ElementWiseOperation(
                    matrix,
                    scalar,
                    (a, b) => matrix.GaloisField.MultiplyWords(a, b)
                    ),
                matrix.GaloisField);

        }
        #endregion

        #endregion

        #region Operator Overloads
        public static MatrixOnGaloisField operator +(MatrixOnGaloisField matrixLeft, MatrixOnGaloisField matrixRight)
        {
            return Addition(matrixLeft, matrixRight);
        }

        public static MatrixOnGaloisField operator +(MatrixOnGaloisField matrix, int scalar)
        {
            return Addition(matrix, scalar);
        }

        public static MatrixOnGaloisField operator +(int scalar, MatrixOnGaloisField matrix)
        {
            return Addition(matrix, scalar);
        }

        public static MatrixOnGaloisField operator *(MatrixOnGaloisField matrixLeft, MatrixOnGaloisField matrixRight)
        {
            return DotMultiplication(matrixLeft, matrixRight);
        }

        public static MatrixOnGaloisField operator *(MatrixOnGaloisField matrix, int scalar)
        {
            return Multiplication(matrix, scalar);
        }

        public static MatrixOnGaloisField operator ^(MatrixOnGaloisField matrixLeft, MatrixOnGaloisField matrixRight)
        {
            return Multiplication(matrixLeft, matrixRight);
        }

        public static MatrixOnGaloisField operator |(MatrixOnGaloisField matrixLeft, MatrixOnGaloisField matrixRight)
        {
            return Concatenate(matrixLeft, matrixRight);
        }

        public static bool operator ==(MatrixOnGaloisField matrixLeft, MatrixOnGaloisField matrixRight)
        {
            return matrixLeft.Equals(matrixRight);
        }

        public static bool operator !=(MatrixOnGaloisField matrixLeft, MatrixOnGaloisField matrixRight)
        {
            return !matrixLeft.Equals(matrixRight);
        }
        #endregion
    }
}
