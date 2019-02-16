using CryptoSystems.Models;
using System.Collections;
using System.Collections.Generic;

namespace CryptoSystemsTests
{
    public class TestDataProvider : IEnumerable<object[]>
    {
        //public static IEnumerable<object[]> GetMessages()
        //{
        //    yield return new object[]{
        //    MatrixInt(new int[,] { { 1, 0, 2 } });

        //}

        private readonly List<object[]> _data = new List<object[]>()
        {
            new object[] {
                new MatrixInt(new int[,] { { 1, 0, 2 } }),
                new MatrixInt(new int[,] { { 1, 0, 0, 3, 0, 0, 0 } })
            },
            new object[] {
                new MatrixInt(new int[,] { { 7, 1, 5 } }),
                new MatrixInt(new int[,] { { 0, 0, 6, 0, 0, 7, 0 } })
            },
            new object[] {
                new MatrixInt(new int[,] { { 0, 6, 1 } }),
                new MatrixInt(new int[,] { { 1, 0, 0, 0, 5, 0, 0 } })
            },
            new object[] {
                new MatrixInt(new int[,] { { 5, 0, 5 } }),
                new MatrixInt(new int[,] { { 0, 0, 3, 4, 0, 0, 0 } })
            },
            new object[] {
                new MatrixInt(new int[,] { { 7, 6, 1 } }),
                new MatrixInt(new int[,] { { 0, 0, 0, 0, 6, 6, 0 } })
            },
            new object[] {
                new MatrixInt(new int[,] { { 7, 3, 5 } }),
                new MatrixInt(new int[,] { { 6, 0, 0, 5, 0, 0, 0 } })
            },
            new object[] {
                new MatrixInt(new int[,] { { 1, 1, 1 } }),
                new MatrixInt(new int[,] { { 0, 0, 0, 0, 0, 1, 7 } })
            },
            new object[] {
                new MatrixInt(new int[,] { { 5, 0, 0 } }),
                new MatrixInt(new int[,] { { 1, 0, 0, 5, 0, 0, 0 } })
            }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
