using CryptoSystems.Models;
using System.Collections.Generic;

namespace CryptoSystems.Interfaces
{
    public interface IParityCheckMatrixGenerator
    {
        MatrixInt Generate(ILinearCode linearCode);
    }
}
