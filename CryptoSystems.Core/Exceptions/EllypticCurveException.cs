using System;

namespace CryptoSystems.Exceptions
{
    public class EllypticCurveException : Exception
    {
        public EllypticCurveException() : base() { }
        public EllypticCurveException(string message) : base(message) { }
        public EllypticCurveException(string message, Exception innerException) : base(message, innerException) { }
    }
}

