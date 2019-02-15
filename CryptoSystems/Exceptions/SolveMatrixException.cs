using System;

namespace CryptoSystems.Exceptions
{
    class SolveMatrixException : Exception
    {
        public SolveMatrixException() : base() { }
        public SolveMatrixException(string message) : base(message) { }
        public SolveMatrixException(string message, Exception innerException) : base(message, innerException) { }
    }
}
