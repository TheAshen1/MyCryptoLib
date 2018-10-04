using McElieceCryptosystem.Models;
using McElieceCryptosystem.Util;
using System.Collections.Generic;

namespace McElieceCryptosystem
{
    public class BinaryGaloisField
    {
        #region Properties
        public int FieldPower { get; }

        public MatrixInt Polynomial { get; set; }

        public MatrixInt Field { get; set; }

        public int WordCount => Field.ColumnCount;

        public int WordLength => Field.RowCount;
        #endregion

        #region Constructors
        public BinaryGaloisField(int fieldPower, MatrixInt polynomial)
        {
            FieldPower = fieldPower;
            Polynomial = polynomial;
            Field = Generate();
        }
        #endregion

        #region Public Methods
        public int FindWord(MatrixInt column)
        {
            var wordNumber
 = Field.FindColumn(column);
            return wordNumber;
        }

        public MatrixInt GetWord(int number)
        {
            var word = Field.GetColumn(number);
            return word;
        }
        #endregion

        #region Private Methods
        private MatrixInt Generate()
        {
            var codeWordNumber = (2 << FieldPower - 1) - 1;
            var decimalEquivalentToPolynomial = Utility.BinaryToDecimal(Polynomial);

            var galoisFieldElements = new List<int>() { 1 };

            int codewordDecimal = 1;
            for (int i = 0; i < codeWordNumber; i++)
            {
                codewordDecimal = (codewordDecimal << 1);
                if (codewordDecimal >= codeWordNumber)
                {
                    codewordDecimal = codewordDecimal ^ decimalEquivalentToPolynomial;
                }
                galoisFieldElements.Add(codewordDecimal);
            }

            var rawResult = new int[codeWordNumber, FieldPower];
            for (int row = 0; row < codeWordNumber; row++)
            {
                var codeword = Utility.DecimalToBinary(galoisFieldElements[row]);
                for (int col = 0; col < FieldPower && col < codeword.ColumnCount; col++)
                {
                    rawResult[row, col] = codeword.Data[0, col];
                }
            }
            var galoisField = new MatrixInt(rawResult).Transpose();
            return galoisField;
        }
        #endregion
    }
}
