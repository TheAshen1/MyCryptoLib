using System;

namespace CryptoSystems.Exceptions
{
    class GaloisFieldException : Exception
    {
        public GaloisFieldException() : base() { }
        public GaloisFieldException(string message) : base(message) { }
        public GaloisFieldException(string message, Exception innerException) : base(message, innerException) { }
    }
}
