using System;

namespace CryptoSystems.Exceptions
{
    class ParityCheckMatrixGeneratorException : Exception
    {
        public ParityCheckMatrixGeneratorException() : base() { }
        public ParityCheckMatrixGeneratorException(string message) : base(message) { }
        public ParityCheckMatrixGeneratorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
