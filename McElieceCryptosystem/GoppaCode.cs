using McElieceCryptosystem.Algorithms;
using McElieceCryptosystem.Interfaces;
using McElieceCryptosystem.Models;
using McElieceCryptosystem.Util;
using System;
using System.Collections.Generic;

namespace McElieceCryptosystem
{
    public class GoppaCode : ILinearCode
    {
        #region Properties

        public MatrixInt GoppaPolynomial { get; }
        /// <summary>
        /// Total length of an encoded word of length K
        /// </summary>
        public int N => GeneratorMatrix.ColumnCount;
        /// <summary>
        /// Number of information digits in liner code
        /// </summary>
        public int K => GeneratorMatrix.RowCount;
        /// <summary>
        /// Max amount of errors code can correct
        /// </summary>
        public int MinimumDistance { get; }

        public MatrixInt GeneratorMatrix { get; }

        public MatrixInt ParityCheckMatrix { get; }

        public GaloisField GaloisField { get; }

        public List<Tuple<int, int>> FieldSubset { get; }

        public int CanDetectUpTo => MinimumDistance - 1;

        public int CanCorrectUpTo => (MinimumDistance - 1) / 2;
        #endregion

        #region Constructors
        public GoppaCode(MatrixInt goppaPolynomial, MatrixInt basePolynomialForGalouisField, int galoisFieldBase, int galoisFieldPower)
        {
            GoppaPolynomial = goppaPolynomial;

            GaloisField = new GaloisField(galoisFieldBase, galoisFieldPower, basePolynomialForGalouisField);

            FieldSubset = GetValuesOfGoppaPolynomial();

            ParityCheckMatrix = CalculateParityCheckMatrix();

            //GeneratorMatrix = CalculateGeneratorMatrix();
        }
        #endregion

        #region Public
        public MatrixInt Encode(MatrixInt message)
        {
            var encodedMessage = message * GeneratorMatrix % 2;
            return encodedMessage;
        }
        /// <summary>
        /// First K digits of a message are the information digits
        /// When digits from K to N are redundancy digits
        /// </summary>
        /// <param name="message">Message, encoded with same linear code</param>
        /// <returns></returns>
        public MatrixInt DecodeAndCorrect(MatrixInt message)
        {
            var actualMessage = message.GetRangeOfColumns(new RangeInt(K)) % 2;
            var syndromeMatrix = message * ParityCheckMatrix % 2;
            //var correctedMessage = ;

            return syndromeMatrix;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// Negation is not nessesary on binary fields
        /// </summary>
        /// <returns></returns>
        private MatrixInt CalculateParityCheckMatrix()
        {
            #region Matrix C
            var C_rowCount = GoppaPolynomial.ColumnCount - 1;
            var C_columnCount = GoppaPolynomial.ColumnCount - 1;
            var C = new int[C_rowCount, C_columnCount];

            for (int row = 0; row < C_rowCount; row++)
            {
                for (int col = 0; col < C_columnCount; col++)
                {
                    C[row, col] = -1;
                }
            }
            for (int row = 0; row < C_rowCount; row++)
            {
                for (int col = row; col < C_columnCount; col++)
                {
                    C[row, col] = GoppaPolynomial.Data[0, GoppaPolynomial.ColumnCount - 1 - col - row];
                }
            }
            #endregion

            #region Matrix X
            var X_rowCount = GoppaPolynomial.ColumnCount - 1;
            var X_columnCount = FieldSubset.Count;
            var X = new int[X_rowCount, X_columnCount];

            for (int row = 0; row < X_rowCount - 1; row++)
            {
                for (int col = 0; col < X_columnCount; col++)
                {
                    X[row, col] = (FieldSubset[col].Item1 * (X_rowCount - 1)) % GaloisField.WordCount;
                }
            }
            #endregion

            #region Matrix Y
            var Y_rowCount = FieldSubset.Count;
            var Y_columnCount = FieldSubset.Count;
            var Y = new int[Y_rowCount, Y_columnCount];
            for (int row = 0; row < Y_rowCount; row++)
            {
                for (int col = 0; col < Y_columnCount; col++)
                {
                    if(row == col)
                    {
                        Y[row, col] = GaloisField.WordCount - FieldSubset[row].Item2;
                        continue;
                    }
                    Y[row, col] = -1;
                }
            }
            #endregion

            #region Polynomial Dot Multiplication C . X
            var CX_rowCount = C_rowCount;
            var CX_columnCount = X_columnCount;
            var CX = new int[CX_rowCount, CX_columnCount];

            for (int row = 0; row < CX_rowCount; row++)
            {
                for (int col = 0; col < CX_columnCount; col++)
                {
                    var sum = new MatrixInt(new int[GaloisField.WordLength, 1]);
                    for (int k = 0; k < C_columnCount; k++)
                    {
                        if (C[row, k] >= 0)
                        {
                            sum = sum + GaloisField.GetWord((C[row, k] + X[k, col]) % GaloisField.WordCount);
                        }
                    }
                    sum = sum % 2;
                    CX[row, col] = GaloisField.FindWord(sum);
                }
            }
            #endregion 

            #region Polynomial Dot Multiplication C . X . Y
            var H_rowCount = CX_rowCount;
            var H_columnCount = Y_columnCount;
            var H = new int[H_rowCount, H_columnCount];

            for (int row = 0; row < H_rowCount; row++)
            {
                for (int col = 0; col < H_columnCount; col++)
                {
                    var sum = new MatrixInt(new int[GaloisField.WordLength, 1]);
                    for (int k = 0; k < CX_columnCount; k++)
                    {
                        if (Y[k, col] >= 0 && CX[row, k] >= 0)
                        {
                            sum = sum + GaloisField.GetWord((CX[row, k] + Y[k, col]) % GaloisField.WordCount);
                        }
                    }
                    sum = sum % 2;
                    H[row, col] = GaloisField.FindWord(sum);
                }
            }
            #endregion

            #region Final assembly
            var parityCheckMatrix_rowCount = H_rowCount * GaloisField.WordLength;
            var parityCheckMatrix_columnCount = H_columnCount + 1;//first column will be empty
            var parityCheckMatrix = new MatrixInt(new int[1, parityCheckMatrix_columnCount]);

            for (int row = 0; row < H_rowCount; row++)
            {
                var rowMatrix = new MatrixInt(new int[GaloisField.WordLength, 1]);
                for (int col = 0; col < H_columnCount; col++)
                {
                    if (H[row, col] < 0)
                    {
                        rowMatrix = rowMatrix | new MatrixInt(new int[GaloisField.WordLength, 1]);
                        continue;
                    }
                    rowMatrix = rowMatrix | GaloisField.GetWord(H[row, col]);
                }
                parityCheckMatrix = parityCheckMatrix.AppendRows(rowMatrix);
            }
            #endregion

            parityCheckMatrix = parityCheckMatrix.SubMatrix(new RangeInt(1, parityCheckMatrix.RowCount), new RangeInt(1, parityCheckMatrix.ColumnCount));

            return parityCheckMatrix;
        }

        private MatrixInt CalculateGeneratorMatrix()
        {
            var parityCheckMatrixStandardForm = MatrixAlgorithms.Solve(ParityCheckMatrix);


            return parityCheckMatrixStandardForm;
        }

        private int CalculateMinimumDistance(MatrixInt generatorMatrix)
        {
            int minimumDistance = generatorMatrix.ColumnCount;
            for (var row = 0; row < generatorMatrix.RowCount - 1; row++)
            {
                for (var anotherRow = row + 1; anotherRow < generatorMatrix.RowCount; anotherRow++)
                {
                    minimumDistance = Math.Min(
                        minimumDistance,
                        Utility.Distance(generatorMatrix.GetRow(row), generatorMatrix.GetRow(anotherRow))
                    );
                }
            }
            return minimumDistance;
        }

        private List<Tuple<int, int>> GetValuesOfGoppaPolynomial()
        {
            var subset = new List<Tuple<int, int>>();

            for (int col = 1; col < GaloisField.Field.ColumnCount; col++)
            {
                var polynomValue = Utility.CalculateGoppaPolynomialValue(GoppaPolynomial, GaloisField, col) % 2;
                if (Utility.Weight(polynomValue) != 0)
                {
                    subset.Add(new Tuple<int, int>(col, GaloisField.Field.FindColumn(polynomValue)));
                }
            }
            return subset;
        }

        public MatrixInt Encode(MatrixInt message, MatrixInt errorVector)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
