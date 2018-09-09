using McElieceCryptosystem.Models;

namespace McElieceCryptosystem
{
    public static class ErrorCorrection
    {
        public static MatrixInt Apply(MatrixInt messageWithErrors)
        {
            return new MatrixInt(messageWithErrors.RowCount, messageWithErrors.ColumnCount);
        }
    }
}
