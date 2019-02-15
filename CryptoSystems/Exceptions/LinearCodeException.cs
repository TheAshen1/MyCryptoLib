using System;

namespace CryptoSystems.Exceptions
{
    class LinearCodeException : Exception
    {
        public LinearCodeException() : base() { }
        public LinearCodeException(string message) : base(message) { }
        public LinearCodeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
