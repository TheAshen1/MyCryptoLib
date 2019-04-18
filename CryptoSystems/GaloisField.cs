using CryptoSystems.Exceptions;
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

        public int MaxValue { get; }


        public int WordCount => Field.Count - 1;

        public int WordLength => Polynomial.ColumnCount - 1;


        private List<int> Field { get; set; }

        public MatrixInt AdditionTable { get; }

        public MatrixInt MultiplicationTable { get; }

        public MatrixInt DivisionTable { get; }
        #endregion

        #region Constructors
        public GaloisField(int baseNumber, int fieldPower, MatrixInt polynomial)
        {
            Base = baseNumber;
            FieldPower = fieldPower;
            Polynomial = polynomial;
            MaxValue = (Base << FieldPower - 1);
            Field = Generate();
            AdditionTable = CalculateAdditionLookupTable(Field);
            MultiplicationTable = CalculateMultiplicationLookupTable(Field);
            DivisionTable = CalculateDivisionLookupTable(Field);
        }

        public GaloisField(int baseNumber, int fieldPower)
        {
            if (!Constants.IrreduciblePolynoms.ContainsKey(fieldPower))
            {
                throw new GaloisFieldException($"It seems there is no fitting irreducible polynomial for such parameters: power={fieldPower}");
            }
            Base = baseNumber;
            FieldPower = fieldPower;

            Polynomial = Constants.IrreduciblePolynoms[fieldPower];
            MaxValue = (Base << FieldPower - 1);
            Field = Generate();
            AdditionTable = CalculateAdditionLookupTable(Field);
            MultiplicationTable = CalculateMultiplicationLookupTable(Field);
            DivisionTable = CalculateDivisionLookupTable(Field);
        }
        #endregion

        #region Public Methods
        public int FindWord(int wordToFind)
        {
            var wordNumber = Field.LastIndexOf(wordToFind);
            return wordNumber;
        }

        public int GetMultiplicativeInverse(int wordNumber)
        {
            if(wordNumber < 0 || wordNumber > WordCount)
            {
                throw new GaloisFieldException($"Field does not containt word with number {wordNumber}.");
            }

            for (int i = 0; i < Field.Count; i++)
            {
                if (MultiplicationTable[wordNumber, i] == 1)
                {
                    return i;
                }
            }

            throw new GaloisFieldException($"Could not find multiplicative inverse of word number {wordNumber}.");
        }

        public int AddWords(int wordNumberLeft, int wordNumberRight)
        {
            if (wordNumberLeft < 0 || wordNumberLeft > WordCount)
            {
                throw new GaloisFieldException($"Field does not containt word with number {wordNumberLeft}.");
            }

            if (wordNumberRight < 0 || wordNumberRight > WordCount)
            {
                throw new GaloisFieldException($"Field does not containt word with number {wordNumberRight}.");
            }

            return AdditionTable[wordNumberLeft, wordNumberRight];
        }

        public int MultiplyWords(int wordNumberLeft, int wordNumberRight)
        {
            if (wordNumberLeft < 0 || wordNumberLeft > WordCount)
            {
                throw new GaloisFieldException($"Field does not containt word with number {wordNumberLeft}.");
            }

            if (wordNumberRight < 0 || wordNumberRight > WordCount)
            {
                throw new GaloisFieldException($"Field does not containt word with number {wordNumberRight}.");
            }

            return MultiplicationTable[wordNumberLeft, wordNumberRight];
        }

        public int DivideWords(int wordNumberLeft, int wordNumberRight)
        {
            if (wordNumberLeft < 0 || wordNumberLeft > WordCount)
            {
                throw new GaloisFieldException($"Field does not containt word with number {wordNumberLeft}.");
            }

            if (wordNumberRight < 0 || wordNumberRight > WordCount)
            {
                throw new GaloisFieldException($"Field does not containt word with number {wordNumberRight}.");
            }

            return DivisionTable[wordNumberLeft, wordNumberRight];
        }

        public int Power(int wordNumber, int power)
        {
            if (wordNumber < 0 || wordNumber > WordCount)
            {
                throw new GaloisFieldException($"Field does not containt word with number {wordNumber}.");
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
        private List<int> Generate()
        {
            var wordCount = MaxValue - 1;
            var decimalEquivalentToPolynomial = Helper.BinaryToDecimal(Polynomial);

            var galoisFieldElements = new List<int>() { 0, 1 };

            int codeword = 1;
            for (int i = 1; i < wordCount; i++)
            {
                codeword = (codeword << 1);
                if (codeword >= MaxValue)
                {
                    codeword ^= decimalEquivalentToPolynomial;
                }
                galoisFieldElements.Add(codeword);
            }
            return galoisFieldElements;
        }

        private MatrixInt CalculateAdditionLookupTable(List<int> field)
        {
            var multiplicationTable = new MatrixInt(new int[field.Count, field.Count]);

            for (int row = 0; row < field.Count; row++)
            {
                for (int col = 0; col < field.Count; col++)
                {
                    multiplicationTable[row, col] = field.LastIndexOf(field[row] ^ field[col]);
                }
            }
            return multiplicationTable;
        }

        private MatrixInt CalculateMultiplicationLookupTable(List<int> field)
        {
            var multiplicationTable = new MatrixInt(new int[field.Count, field.Count]);

            for (int row = 0; row < field.Count; row++)
            {
                for (int col = 0; col < field.Count; col++)
                {
                    if (row == 0 || col == 0)
                    {
                        multiplicationTable[row, col] = 0;
                        continue;
                    }
                    multiplicationTable[row, col] = (row + col - 2) % WordCount + 1;
                }
            }
            return multiplicationTable;
        }

        private MatrixInt CalculateDivisionLookupTable(List<int> field)
        {
            var divisionTable = new MatrixInt(new int[field.Count, field.Count]);

            for (int row = 0; row < field.Count; row++)
            {
                for (int col = 0; col < field.Count; col++)
                {
                    if (row == 0 || col == 0)
                    {
                        divisionTable[row, col] = 0;
                        continue;
                    }
                    var power = (row - col + WordCount);
                    divisionTable[row, col] = power % WordCount + 1;
                }
            }
            return divisionTable;
        }

        #endregion
    }
}
