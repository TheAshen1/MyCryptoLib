using CryptoSystems.Exceptions;
using CryptoSystems.Interfaces;
using CryptoSystems.Models;
using CryptoSystems.ParityCheckMatrixGenerators;
using System;
using System.Diagnostics;
using System.Linq;

namespace CryptoSystems.Algorithms
{
    public static class DecoderEllyptic
    {
        public static MatrixInt DecodeAndCorrect(ILinearCode linearCode, MatrixInt message, ParityCheckMatrixGeneratorEllyptic generator)
        {
            if (message.ColumnCount != linearCode.ParityCheckMatrix.ColumnCount)
            {
                throw new DimensionMismatchException("Incorrect length of message. Meesage length should be equal number of columns in parity check matrix.");
            }

            #region Calculate syndrome
            var syndrome = MatrixAlgorithms.DotMultiplication(message, linearCode.ParityCheckMatrix.Transpose(), linearCode.GaloisField);
            #endregion

            #region Locate errors
            var errorLocators = ErrorLocatorEllyptic.LocateErrors(linearCode, syndrome, generator);

            var errorCount = errorLocators.Where(v => v == 0).Count();
            #endregion

            Debug.WriteLine(linearCode.ParityCheckMatrix);
            #region Caclulate Error vector
            var system = new MatrixInt(linearCode.D, errorCount);
            for (int row = 0; row < linearCode.D; row++)
            {
                for (int col = 0, i = 0; col < linearCode.N; col++)
                {
                    if (errorLocators[col] == 0)
                    {
                        system[row, i] = linearCode.ParityCheckMatrix[row, col];
                        i++;
                    }
                }
            }
            Debug.WriteLine(system);
            system |= syndrome.Transpose();
            Debug.WriteLine(system);
            var errorVectorValues = MatrixAlgorithms.Solve(system, linearCode.GaloisField);

            var errorVector = new int[errorLocators.Length];
            for (int i = 0, j = 0; i < errorVector.Length; i++)
            {
                if (errorLocators[i] == 0)
                {
                    errorVector[i] = errorVectorValues[j, 0];
                    j++;
                }
                else
                {
                    errorVector[i] = 0;
                }
            }
            #endregion


            var rawOriginalMessage = new int[linearCode.K];

            for (int i = 0; i < linearCode.K; i++)
            {
                rawOriginalMessage[i] = linearCode.GaloisField.AddWords(message.Data[0, i + linearCode.D], errorVector[i + linearCode.D]);
            }

            var originalMessage = new MatrixInt(rawOriginalMessage);
            return originalMessage;
        }
    }
}
