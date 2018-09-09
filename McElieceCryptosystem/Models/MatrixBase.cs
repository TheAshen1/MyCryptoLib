using McElieceCryptosystem.Exceptions;
using System;
using System.Text;

namespace McElieceCryptosystem.Models
{
    public class MatrixBase<T>
    {
        #region static
        protected static MatrixBase<T> ElementWiseOperation(
          MatrixBase<T> matrix1,
          MatrixBase<T> matrix2,
          Func<T,T,T> operation)
        {
            if (matrix1.RowCount != matrix2.RowCount)
                throw new DimensionMismatchException("The number of rows in matrix1 does not equal the number of rows in matrix2");
            if (matrix1.ColumnCount != matrix2.ColumnCount)
                throw new DimensionMismatchException("The number of columns in matrix1 does not equal the number of columns in matrix2");

            var rawResult = new T[matrix1.RowCount, matrix1.ColumnCount];
            for (int row = 0; row < rawResult.GetLength(0); row++)
                for (int col = 0; col < rawResult.GetLength(1); col++)
                    rawResult[row, col] = operation(matrix1.Data[row, col], matrix1.Data[row, col]);

            MatrixBase<T> result = new MatrixBase<T>(rawResult);
            return result;
        }

        protected static MatrixBase<T> ElementWiseOperation(
         MatrixBase<T> matrix1,
         T scalar,
         Func<T, T, T> operation)
        {
            var rawResult = new T[matrix1.RowCount, matrix1.ColumnCount];
            for (int row = 0; row < rawResult.GetLength(0); row++)
                for (int col = 0; col < rawResult.GetLength(1); col++)
                    rawResult[row, col] = operation(matrix1.Data[row, col], scalar);

            MatrixBase<T> result = new MatrixBase<T>(rawResult);
            return result;
        }
        #endregion

        public int RowCount { get; }
        public int ColumnCount { get; }
        public T[,] Data { get; }

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

        #region Generic Operations
        public MatrixBase<T> Transpose()
        {
            var rawDataTransposed = new T[ColumnCount, RowCount];
            

            for (int row = 0; row < RowCount; row++)
                for (int col = 0; col < ColumnCount; col++)
                    rawDataTransposed[col, row] = Data[row, col];

            var result = new MatrixBase<T>(rawDataTransposed);
            return result;
        }
        #endregion

        override public string ToString()
        {
            var sb = new StringBuilder();

            for(var row = 0; row < RowCount; row++)
            {
                for (var col = 0; col < ColumnCount; col++)
                {
                    sb.Append(Data[row,col].ToString() + " ");
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
