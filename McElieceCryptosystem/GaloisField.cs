using CryptoSystems.Algorithms;
using CryptoSystems.Exceptions;
using CryptoSystems.Mappers;
using CryptoSystems.Models;
using CryptoSystems.Util;
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

        public int WordCount => Field.RowCount;

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
            if(wordNumber >= WordCount)
            {
                throw new DimensionMismatchException("Word number cannot exceed number of all words in Galois field minus one");
            }

            var inverse = (WordCount - wordNumber) % WordCount;
            return inverse;
        }

        public int AddWords(int wordNumberleft, int wordNumberRight)
        {
            if(wordNumberleft < 0 && wordNumberRight >= 0)
            {
                return wordNumberRight;
            }

            if (wordNumberRight < 0 && wordNumberleft >= 0)
            {
                return wordNumberleft;
            }

            if (wordNumberleft < 0 && wordNumberleft < 0)
            {
                return -1;
            }

            var wordLeft = GetWord(wordNumberleft);
            var wordRight = GetWord(wordNumberRight);
            var resultWord = (wordLeft + wordRight) % 2;
            var resultWordNumber = FindWord(resultWord);
            return resultWordNumber;
        }

        public int MultiplyWords(int wordNumberleft, int wordNumberRight)
        {
            if (wordNumberleft < 0 || wordNumberRight < 0)
            {
                return -1;
            }

            var resultWordNumber = (wordNumberleft + wordNumberRight) % WordCount;
            return resultWordNumber;
        }

        public int DivideWords(int wordNumberleft, int wordNumberRight)
        {
            if (wordNumberleft < 0 || wordNumberRight < 0)
            {
                return -1;
            }

            var resultWordNumber = (wordNumberleft + GetMultiplicativeInverse(wordNumberRight)) % WordCount;
            return resultWordNumber;
        }

        public int Power(int wordNumber, int power)
        {
            if(power < 0)
            {
                throw new ArgumentException("Power cannot be negative here.");
            }

            if (power == 0)
            {
                return 0;
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
            var codeWordCount = (Base << FieldPower - 1) - 1;
            var decimalEquivalentToPolynomial = Utility.BinaryToDecimal(Polynomial);

            var galoisFieldElements = new List<int>() { 1 };

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
                var codeword = Utility.DecimalToBinary(galoisFieldElements[row]);
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
