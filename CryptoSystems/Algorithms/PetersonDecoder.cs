using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;

namespace CryptoSystems.Algorithms
{
    public static class PetersonDecoder
    {
        public static MatrixInt DecodeAndCorrect(ILinearCode linearCode, MatrixInt message)
        {
            if (message.ColumnCount != linearCode.ParityCheckMatrix.ColumnCount)
            {
                throw new DimensionMismatchException("Incorrect length of message. Meesage length should be equal number of columns in parity check matrix.");
            }

            #region Calculate syndrome
            var syndrome = MatrixAlgorithms.DotMultiplication(message, linearCode.ParityCheckMatrix.Transpose(), linearCode.GaloisField);
            #endregion

            #region МЛО
            var rowCount = linearCode.T;
            var columnCount = linearCode.T + 1;

            var system = new int[rowCount, columnCount];

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    system[row, col] = syndrome.Data[0, row + col];
                }
            }
            var coefficients = MatrixAlgorithms.Solve(new MatrixInt(system), linearCode.GaloisField).Transpose();
            #endregion

            #region Calculate Error Positions
            var errorCheck = new int[linearCode.N];
            for (int position = 0, word = 1; position < linearCode.N; position++, word++)
            {
                var checkResult = coefficients.Data[0, 0];
                for (int i = 1; i < coefficients.ColumnCount; i++)
                {
                    var wordToAdd = linearCode.GaloisField.MultiplyWords(coefficients.Data[0, i], word);
                    checkResult = linearCode.GaloisField.AddWords(checkResult, wordToAdd);
                }

                var lastAddent = linearCode.GaloisField.Power(word, linearCode.T);
                checkResult = linearCode.GaloisField.AddWords(checkResult, lastAddent);

                errorCheck[position] = checkResult;
            }
            #endregion

            #region Caclulate Error vector
            rowCount = linearCode.T;
            columnCount = linearCode.T + 1;

            system = new int[rowCount, columnCount];
            #region Find error positions
            var errorNumber = 0;
            for (int errorPostition = 0; errorPostition < linearCode.N; errorPostition++)
            {
                if (errorCheck[errorPostition] == 0)
                {
                    for (int row = 0; row < rowCount; row++)
                    {
                        system[row, errorNumber] = linearCode.ParityCheckMatrix.Data[row, errorPostition];
                    }
                    errorNumber++;
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                system[i, columnCount - 1] = syndrome.Data[0, i];
            }
            #endregion

            #region Find error values
            var weights = MatrixAlgorithms.Solve(new MatrixInt(system), linearCode.GaloisField);
            #endregion

            #region Recreate complete error vector
            var rawErrorVector = new int[linearCode.N];

            errorNumber = 0;
            for (int i = 0; i < linearCode.N; i++)
            {
                if (errorCheck[i] == 0)
                {
                    rawErrorVector[i] = weights.Data[errorNumber, 0];
                    errorNumber++;
                    continue;
                }
                rawErrorVector[i] = 0;
            }
            #endregion

            #endregion

            var rawOriginalMessage = new int[linearCode.K];

            for (int i = 0; i < linearCode.K; i++)
            {
                rawOriginalMessage[i] = linearCode.GaloisField.AddWords(message.Data[0, i], rawErrorVector[i]);
            }

            var originalMessage = new MatrixInt(rawOriginalMessage);
            return originalMessage;
        }
    }
}
