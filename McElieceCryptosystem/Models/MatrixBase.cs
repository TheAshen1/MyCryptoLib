using McElieceCryptosystem.Exceptions;
using System;
using System.Text;

namespace McElieceCryptosystem.Models
{
    public class MatrixBase<T>
    {
        #region Static methods
        protected static MatrixBase<T> ElementWiseOperation(
            MatrixBase<T> matrixLeft,
            MatrixBase<T> matrixRight,
            Func<T, T, T> operation)
        {
            if (matrixLeft.RowCount != matrixRight.RowCount)
            {
                throw new DimensionMismatchException("The number of rows in matrix1 does not equal the number of rows in matrix2");
            }
            if (matrixLeft.ColumnCount != matrixRight.ColumnCount)
            {
                throw new DimensionMismatchException("The number of columns in matrix1 does not equal the number of columns in matrix2");
            }

            var rawResult = new T[matrixLeft.RowCount, matrixLeft.ColumnCount];
            for (int row = 0; row < rawResult.GetLength(0); row++)
            {
                for (int col = 0; col < rawResult.GetLength(1); col++)
                {
                    rawResult[row, col] = operation(matrixLeft.Data[row, col], matrixRight.Data[row, col]);
                }
            }

            MatrixBase<T> result = new MatrixBase<T>(rawResult);
            return result;
        }

        protected static MatrixBase<T> ElementWiseOperation(
            MatrixBase<T> matrix,
            T scalar,
            Func<T, T, T> operation)
        {
            var rawResult = new T[matrix.RowCount, matrix.ColumnCount];
            for (int row = 0; row < rawResult.GetLength(0); row++)
            {
                for (int col = 0; col < rawResult.GetLength(1); col++)
                {
                    rawResult[row, col] = operation(matrix.Data[row, col], scalar);
                }
            }
            MatrixBase<T> result = new MatrixBase<T>(rawResult);
            return result;
        }

        protected static MatrixBase<T> UnaryElementWiseOperation(
            MatrixBase<T> matrix,
            Func<T, T> operation)
        {
            var rawResult = new T[matrix.RowCount, matrix.ColumnCount];
            for (int row = 0; row < rawResult.GetLength(0); row++)
            {
                for (int col = 0; col < rawResult.GetLength(1); col++)
                {
                    rawResult[row, col] = operation(matrix.Data[row, col]);
                }
            }
            MatrixBase<T> result = new MatrixBase<T>(rawResult);
            return result;
        }

        public static MatrixBase<T> Concatenate(
           MatrixBase<T> matrixLeft,
           MatrixBase<T> matrixRight)
        {
            if (matrixLeft.RowCount != matrixRight.RowCount)
            {
                throw new DimensionMismatchException("The number of rows in left matrix does not equal the number of rows in right matrix");
            }

            var rawResult = new T[matrixLeft.RowCount, matrixLeft.ColumnCount + matrixRight.ColumnCount];
            for (var row = 0; row < matrixLeft.RowCount; row++)
            {
                for (var col = 0; col < matrixLeft.ColumnCount; col++)
                {
                    rawResult[row, col] = matrixLeft.Data[row, col];
                }

                for (var col = 0; col < matrixRight.ColumnCount; col++)
                {
                    rawResult[row, matrixLeft.ColumnCount + col] = matrixRight.Data[row, col];
                }
            }
            var result = new MatrixBase<T>(rawResult);
            return result;
        }

        public static MatrixBase<T> Transpose(MatrixBase<T> matrix)
        {
            var rawDataTransposed = new T[matrix.ColumnCount, matrix.RowCount];

            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int col = 0; col < matrix.ColumnCount; col++)
                {
                    rawDataTransposed[col, row] = matrix.Data[row, col];
                }
            }
            var result = new MatrixBase<T>(rawDataTransposed);
            return result;
        }

        public static MatrixBase<T> SubMatrix(
            MatrixBase<T> matrix,
            RangeInt rowRange,
            RangeInt columnRange)
        {
            if (!matrix.DataColumnRange.Contains(columnRange))
            {
                throw new ArgumentOutOfRangeException("columnRange");
            }
            if (!matrix.DataRowRange.Contains(rowRange))
            {
                throw new ArgumentOutOfRangeException("rowRange");
            }

            var rawResult = new T[rowRange.Length, columnRange.Length];

            for (int row = rowRange.Start; row < rowRange.End; row++)
            {
                for (int col = columnRange.Start; col < columnRange.End; col++)
                {
                    rawResult[row - rowRange.Start, col - columnRange.Start] = matrix.Data[row, col];
                }
            }
            MatrixBase<T> result = new MatrixBase<T>(rawResult);
            return result;
        }

        public MatrixBase<T> GetRangeOfRows(
            MatrixBase<T> matrix,
            RangeInt rowRange)
        {
            if (!matrix.DataRowRange.Contains(rowRange))
            {
                throw new ArgumentOutOfRangeException("rowRange");
            }

            var rawResult = new T[rowRange.Length, matrix.ColumnCount];

            for (int row = rowRange.Start; row < rowRange.End; row++)
            {
                for (int col = 0; col < matrix.ColumnCount; col++)
                {
                    rawResult[row - rowRange.Start, col] = matrix.Data[row, col];
                }
            }
            MatrixBase<T> result = new MatrixBase<T>(rawResult);
            return result;
        }

        public MatrixBase<T> GetRangeOfColumns(
            MatrixBase<T> matrix,
            RangeInt columnRange)
        {
            if (!matrix.DataColumnRange.Contains(columnRange))
            {
                throw new ArgumentOutOfRangeException("columnRange");
            }

            var rawResult = new T[matrix.RowCount, columnRange.Length];

            for (int row = 0; row < matrix.RowCount; row++)
            {
                for (int col = columnRange.Start; col < columnRange.End; col++)
                {
                    rawResult[row, col - columnRange.Start] = matrix.Data[row, col];
                }
            }
            MatrixBase<T> result = new MatrixBase<T>(rawResult);
            return result;
        }

        public MatrixBase<T> GetRow(
            MatrixBase<T> matrix,
            int rowNumber)
        {
            if (rowNumber > matrix.RowCount - 1 || rowNumber < 0)
            {
                throw new ArgumentOutOfRangeException("columnRange");
            }
            var rawResult = new T[1, matrix.ColumnCount];

            for (int col = 0; col < matrix.ColumnCount; col++)
            {
                rawResult[0, col] = matrix.Data[rowNumber, col];
            }
            MatrixBase<T> result = new MatrixBase<T>(rawResult);
            return result;
        }

        public MatrixBase<T> GetColumn(
            MatrixBase<T> matrix,
            int columnNumber)
        {
            if (columnNumber > matrix.ColumnCount - 1 || columnNumber < 0)
            {
                throw new ArgumentOutOfRangeException("columnRange");
            }

            var rawResult = new T[matrix.RowCount, 1];

            for (int row = 0; row < matrix.RowCount; row++)
            {
                rawResult[row, 0] = matrix.Data[row, columnNumber];
            }
            MatrixBase<T> result = new MatrixBase<T>(rawResult);
            return result;
        }
        #endregion

        #region Properties
        public int RowCount { get; }
        public int ColumnCount { get; }
        public T[,] Data { get; }

        public RangeInt DataColumnRange => new RangeInt(ColumnCount);
        public RangeInt DataRowRange => new RangeInt(RowCount);
        #endregion

        #region Constructors
        public MatrixBase(int size) : this(size, size)
        {
        }

        public MatrixBase(int rows, int columns)
        {
            Data = new T[rows, columns];
            RowCount = rows;
            ColumnCount = columns;
        }

        public MatrixBase(T[,] initialValues)
        {
            RowCount = initialValues.GetLength(0);
            ColumnCount = initialValues.GetLength(1);

            Data = new T[RowCount, ColumnCount];

            for (var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColumnCount; col++)
                {
                    Data[row, col] = initialValues[row, col];
                }
            }
        }

        public MatrixBase(MatrixBase<T> matrix)
        {
            RowCount = matrix.RowCount;
            ColumnCount = matrix.ColumnCount;

            Data = new T[RowCount, ColumnCount];

            for (var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColumnCount; col++)
                {
                    Data[row, col] = matrix.Data[row, col];
                }
            }
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColumnCount; col++)
                {
                    sb.Append(Data[row, col].ToString() + " ");
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
        #endregion

        #region Operator Overloads
        public static MatrixBase<T> operator |(MatrixBase<T> matrix1, MatrixBase<T> matrix2)
        {
            return Concatenate(matrix1, matrix2);
        }
        #endregion
    }
}
