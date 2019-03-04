using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using System.Collections.Generic;

namespace CryptoSystems.Algorithms
{
    public static class DecoderEllyptic
    {
        public static MatrixInt DecodeAndCorrect(ILinearCode linearCode, MatrixInt message, List<Point> points)
        {
            if (message.ColumnCount != linearCode.ParityCheckMatrix.ColumnCount)
            {
                throw new DimensionMismatchException("Incorrect length of message. Meesage length should be equal number of columns in parity check matrix.");
            }

            #region Calculate syndrome
            var syndrome = MatrixAlgorithms.DotMultiplication(message, linearCode.ParityCheckMatrix.Transpose(), linearCode.GaloisField);
            #endregion

            var errorLocations = ErrorLocatorEllyptic.LocateErrors(linearCode, syndrome, points);

            #region Caclulate Error vector
            var rowCount = linearCode.T;
            var columnCount = linearCode.T + 1;

            var system = new int[rowCount, columnCount];
            #region Find error positions
            var errorNumber = 0;
            for (int errorPosition = 0; errorPosition < linearCode.N; errorPosition++)
            {
                if (errorLocations[errorPosition] == 0)
                {
                    for (int row = 0; row < rowCount; row++)
                    {
                        system[row, errorNumber] = linearCode.ParityCheckMatrix[row, errorPosition];
                    }
                    errorNumber++;
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                system[i, columnCount - 1] = syndrome[0, i];
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
                if (errorLocations[i] == 0)
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
