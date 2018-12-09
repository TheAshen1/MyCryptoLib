using McElieceCryptosystem.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace McElieceCryptosystem.Models
{
    public class MatrixBase<T> : ICloneable
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
                    rawResult[row, col] = operation(matrixLeft[row, col], matrixRight[row, col]);
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
                    rawResult[row, col] = operation(matrix[row, col], scalar);
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
                    rawResult[row, col] = operation(matrix[row, col]);
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
                    rawResult[row, col] = matrixLeft[row, col];
                }

                for (var col = 0; col < matrixRight.ColumnCount; col++)
                {
                    rawResult[row, matrixLeft.ColumnCount + col] = matrixRight[row, col];
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
                    rawDataTransposed[col, row] = matrix[row, col];
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
            if (!matrix.ColumnRange.Contains(columnRange))
            {
                throw new ArgumentOutOfRangeException("columnRange");
            }
            if (!matrix.RowRange.Contains(rowRange))
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

        public static MatrixBase<T> GetRangeOfRows(
            MatrixBase<T> matrix,
            RangeInt rowRange)
        {
            if (!matrix.RowRange.Contains(rowRange))
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

        public static MatrixBase<T> GetRangeOfColumns(
            MatrixBase<T> matrix,
            RangeInt columnRange)
        {
            if (!matrix.ColumnRange.Contains(columnRange))
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

        public static MatrixBase<T> GetRow(
            MatrixBase<T> matrix,
            int rowNumber)
        {
            if (!matrix.RowRange.Contains(rowNumber))
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

        public static MatrixBase<T> GetColumn(
            MatrixBase<T> matrix,
            int columnNumber)
        {
            if (!matrix.ColumnRange.Contains(columnNumber))
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

        public static MatrixBase<T> SwapColumns(
            MatrixBase<T> matrix,
            int i,
            int j)
        {
            if (!matrix.ColumnRange.Contains(i))
            {
                throw new ArgumentOutOfRangeException("columnRange");
            }

            if (!matrix.ColumnRange.Contains(j))
            {
                throw new ArgumentOutOfRangeException("columnRange");
            }

            if (i == j)
            {
                return matrix.Clone();
            }

            var result = matrix.Clone();

            for (var row = 0; row < matrix.RowCount; row++)
            {
                var temp = result.Data[row, i];
                result.Data[row, i] = result.Data[row, j];
                result.Data[row, j] = temp;
            }
            return result;
        }

        public static MatrixBase<T> SwapRows(
          MatrixBase<T> matrix,
          int i,
          int j)
        {
            if (!matrix.RowRange.Contains(i))
            {
                throw new ArgumentOutOfRangeException("rowRange");
            }

            if (!matrix.RowRange.Contains(j))
            {
                throw new ArgumentOutOfRangeException("rowRange");
            }

            if (i == j)
            {
                return matrix.Clone();
            }

            var result = matrix.Clone();

            for (var col = 0; col < matrix.ColumnCount; col++)
            {
                var temp = result.Data[i, col];
                result.Data[i, col] = result.Data[j, col];
                result.Data[j, col] = temp;
            }

            return result;
        }

        public static MatrixBase<T> AppendRows(
            MatrixBase<T> matrix,
            MatrixBase<T> rows)
        {

            if (matrix.ColumnCount != rows.ColumnCount)
            {
                throw new DimensionMismatchException("The number of columns in matrix does not equal the number of columns in rows");
            }
            var rawResult = new T[matrix.RowCount + rows.RowCount, matrix.ColumnCount];

            for (var row = 0; row < matrix.RowCount; row++)
            {
                for (var col = 0; col < matrix.ColumnCount; col++)
                {
                    rawResult[row, col] = matrix.Data[row, col];
                }
            }

            for (var row = matrix.RowCount; row < matrix.RowCount + rows.RowCount; row++)
            {
                for (var col = 0; col < matrix.ColumnCount; col++)
                {
                    rawResult[row, col] = rows.Data[row - matrix.RowCount, col];
                }
            }
            var result = new MatrixBase<T>(rawResult);
            return result;
        }



        public int FindRow(MatrixInt matrix, MatrixInt rowToFind)
        {
            if (rowToFind.RowCount > 1)
            {
                throw new ArgumentException("The row matrix contains more than 1 row");
            }

            if (matrix.ColumnCount != rowToFind.ColumnCount)
            {
                throw new DimensionMismatchException("The number of columns in matrix does not equal the number of columns in the row");
            }

            for (var row = 0; row < matrix.RowCount; row++)
            {
                for (var col = 0; col < matrix.ColumnCount; col++)
                {
                    if (matrix.Data[row, col] != rowToFind.Data[0, col])
                    {
                        break;
                    }

                    if (col == matrix.ColumnCount - 1)
                    {
                        return row;
                    }
                }
            }
            return -1;
        }

        public int FindColumn(MatrixInt matrix, MatrixInt columnToFind)
        {

            if (columnToFind.ColumnCount > 1)
            {
                throw new ArgumentException("The column matrix contains more than 1 column");
            }

            if (matrix.RowCount != columnToFind.RowCount)
            {
                throw new DimensionMismatchException("The number of rows in matrix does not equal the number of rows in the column");
            }

            for (var col = 0; col < matrix.ColumnCount; col++)
            {
                for (var row = 0; row < matrix.RowCount; row++)
                {
                    if (matrix.Data[row, col] != columnToFind.Data[row, 0])
                    {
                        break;
                    }

                    if (row == matrix.RowCount - 1)
                    {
                        return col;
                    }
                }
            }
            return -1;
        }
        #endregion

        #region Properties
        public int RowCount { get; }
        public int ColumnCount { get; }
        public T[,] Data { get; }

        public RangeInt ColumnRange => new RangeInt(ColumnCount);
        public RangeInt RowRange => new RangeInt(RowCount);

        public T this[int row, int col]
        {
            get { return Data[row, col]; }
            set { Data[row, col] = value; }
        }
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

        public MatrixBase(T[] initialRow)
        {
            RowCount = 1;
            ColumnCount = initialRow.Length;

            Data = new T[RowCount, ColumnCount];

            for (var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColumnCount; col++)
                {
                    Data[row, col] = initialRow[col];
                }
            }
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
                    Data[row, col] = matrix[row, col];
                }
            }
        }

        public MatrixBase(List<T> list)
        {
            RowCount = 1;
            ColumnCount = list.Count;

            Data = new T[RowCount, ColumnCount];

            for (var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColumnCount; col++)
                {
                    Data[row, col] = list[col];
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
                    sb.Append(this[row, col].ToString() + " ");
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public MatrixBase<T> Clone()
        {
            return new MatrixBase<T>(this);
        }

        object ICloneable.Clone()
        {
            return new MatrixBase<T>(this);
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
