using System;

namespace McElieceCryptosystem.Exceptions
{
    class DimensionMismatchException : ArithmeticException
    {
        public DimensionMismatchException() : base() { }
        public DimensionMismatchException(string message) : base(message) { }
        public DimensionMismatchException(string message, Exception innerException) : base(message, innerException) { }
    }
}
