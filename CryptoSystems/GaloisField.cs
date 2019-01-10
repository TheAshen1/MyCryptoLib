using CryptoSystems.Algorithms;
using CryptoSystems.Exceptions;
using CryptoSystems.Mappers;
using CryptoSystems.Models;
using CryptoSystems.Utility;
using System;
using System.Collections.Generic;

namespace CryptoSystems
{
    public class GaloisField
    {
        #region Properties
        public int Base { get; }

        public int FieldPower { get; }

        public MatrixInt Polynomial { get; }

        public MatrixInt Field { get; }

        public int WordCount => Field.RowCount - 1;

        public int WordLength => Field.ColumnCount;

        public MatrixInt this[int word]
        {
            get { return GetWord(word); }
        }
        #endregion

        #region Constructors
        public GaloisField(int baseNumber, int fieldPower, MatrixInt polynomial)
        {
            Base = baseNumber;
            FieldPower = fieldPower;
            Polynomial = polynomial;
            Field = Generate();
        }
        #endregion

        #region Public Methods
        public int FindWord(MatrixInt wordToFind)
        {
            var wordNumber = Field.FindRow(wordToFind);
            return wordNumber;
        }

        public MatrixInt GetWord(int number)
        {
            if (number < 0)
            {
                return new MatrixInt(new int[1, WordLength]);
            }

            var word = Field.GetRow(number);
            return word;
        }

        public int GetMultiplicativeInverse(int wordNumber)
        {
            var wordPower = wordNumber - 1;
            if (wordPower >= WordCount)
            {
                throw new DimensionMismatchException("Word number cannot exceed number of all words in Galois field minus one");
            }

            var inverseWordNumber = (WordCount - wordPower) % WordCount + 1;
            return inverseWordNumber;
        }

        public int AddWords(int wordNumberLeft, int wordNumberRight)
        {
            if(wordNumberLeft == 0 && wordNumberRight > 0)
            {
                return wordNumberRight;
            }

            if (wordNumberRight == 0 && wordNumberLeft > 0)
            {
                return wordNumberLeft;
            }

            if (wordNumberLeft == 0 && wordNumberLeft == 0)
            {
                return 0;
            }

            var wordLeft = GetWord(wordNumberLeft);
            var wordRight = GetWord(wordNumberRight);
            var resultWord = (wordLeft + wordRight) % 2;
            var resultWordNumber = FindWord(resultWord);
            return resultWordNumber;
        }

        public int MultiplyWords(int wordNumberLeft, int wordNumberRight)
        {
            if (wordNumberLeft < 0 || wordNumberRight < 0)
            {
                throw new ArgumentException("Word number cannot be less than 0.");
            }

            if (wordNumberLeft == 0 || wordNumberRight == 0)
            {
                return 0;
            }

            var wordPowerLeft = wordNumberLeft - 1;
            var wordPowerRight = wordNumberRight -1;

            var resultWordPower = (wordPowerLeft + wordPowerRight) % WordCount;
            return resultWordPower + 1;
        }

        public int DivideWords(int wordNumberleft, int wordNumberRight)
        {
            if (wordNumberleft < 0 || wordNumberRight < 0)
            {
                throw new ArgumentException("Word number cannot be less than 0.");
            }

            if (wordNumberleft == 0 || wordNumberRight == 0)
            {
                return 0;
            }

            var wordPowerLeft = wordNumberleft - 1;
            var wordPowerRight = wordNumberRight - 1;

            var inverseWordPowerRight = WordCount - wordPowerRight;
            var resultWordPower = (wordPowerLeft + inverseWordPowerRight) % WordCount;
            return resultWordPower + 1;
        }

        public int Power(int wordNumber, int power)
        {
            if(power < 0)
            {
                throw new ArgumentException("Power cannot be negative here.");
            }

            if (power == 0)
            {
                return 1;
            }

            var result = wordNumber;
            
            for (int i = 1; i < power; i++)
            {
                result = MultiplyWords(result, wordNumber);
            }
            return result;
        }

        public override string ToString()
        {
            return Field.ToString();
        }
        #endregion

        #region Private Methods
        private MatrixInt Generate()
        {
            var codeWordCount = (Base << FieldPower - 1);
            var decimalEquivalentToPolynomial = Helper.BinaryToDecimal(Polynomial);

            var galoisFieldElements = new List<int>() { 0, 1 };

            int codewordDecimal = 1;
            for (int i = 0; i < codeWordCount; i++)
            {
                codewordDecimal = (codewordDecimal << 1);
                if (codewordDecimal >= codeWordCount)
                {
                    codewordDecimal = codewordDecimal ^ decimalEquivalentToPolynomial;
                }
                galoisFieldElements.Add(codewordDecimal);
            }

            var rawResult = new int[codeWordCount, FieldPower];
            for (int row = 0; row < codeWordCount; row++)
            {
                var codeword = Helper.DecimalToBinary(galoisFieldElements[row]);
                for (int col = 0; col < FieldPower && col < codeword.ColumnCount; col++)
                {
                    rawResult[row, col] = codeword.Data[0, col];
                }
            }
            var galoisField = new MatrixInt(rawResult);
            return galoisField;
        }
        #endregion
    }
}
