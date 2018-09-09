using McElieceCryptosystem.Exceptions;

namespace McElieceCryptosystem.Models
{
    class MatrixInt: MatrixBase<int>
    {
        #region Constructors
        public MatrixInt(int size) : base(size)
        {
        }

        public MatrixInt(int rows, int columns) : base(rows, columns)
        {
        }

        public MatrixInt(int[,] initialValues): base(initialValues)
        {
        }

        public MatrixInt(MatrixBase<int> matrix) : base(matrix)
        {
        }
        #endregion

        #region Operations
        protected static MatrixInt Addition(
           MatrixInt matrix1,
           MatrixInt matrix2)
        {
            return new MatrixInt(
                ElementWiseOperation(
                    matrix1,
                    matrix2,
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
          MatrixInt matrix1,
          MatrixInt matrix2)
        {
            return new MatrixInt(
                ElementWiseOperation(
                    matrix1,
                    matrix2,
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
   
        protected static MatrixInt Multiplication(MatrixInt matrix1, MatrixInt matrix2)
        {
            if (matrix1.ColumnCount != matrix2.RowCount)
                throw new DimensionMismatchException("Number of columns in first matrix does not equal number of rows in second matrix.");

            var rawResult = new int[matrix1.RowCount, matrix2.ColumnCount];

            for (int row = 0; row < rawResult.GetLength(0); row++)
                for (int col = 0; col < rawResult.GetLength(1); col++)
                {
                    int sum = 0;
                    for (int k = 0; k < matrix1.ColumnCount; k++)
                        sum += matrix1.Data[k, col] * matrix2.Data[row, k];
                    rawResult[row, col] = sum;
                }
            MatrixInt result = new MatrixInt(matrix2.ColumnCount, matrix1.RowCount);
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
        #endregion

        #region Operator Overloads
        public static MatrixInt operator +(MatrixInt matrix1, MatrixInt matrix2)
        {
            return Addition(matrix1, matrix2);
        }

        public static MatrixInt operator +(MatrixInt matrix, int scalar)
        {
            return Addition(matrix, scalar);
        }

        public static MatrixInt operator +(int scalar, MatrixInt matrix)
        {
            return Addition(matrix, scalar);
        }

        public static MatrixInt operator -(MatrixInt matrix1, MatrixInt matrix2)
        {
            return Subtraction(matrix1, matrix2);
        }

        public static MatrixInt operator -(MatrixInt matrix, int scalar)
        {
            return Subtraction(matrix, scalar);
        }

        public static MatrixInt operator *(MatrixInt matrix1, MatrixInt matrix2)
        {
            return Multiplication(matrix1, matrix2);
        }

        public static MatrixInt operator *(MatrixInt matrix, int scalar)
        {
            return Multiplication(matrix, scalar);
        }
        #endregion
    }
}
