using McElieceCryptosystem.Models;

namespace McElieceCryptosystem
{
    public static class Decoder
    {
        public static MatrixInt Decode(MatrixInt encodedMessage)
        {
            return new MatrixInt(encodedMessage.RowCount, encodedMessage.ColumnCount);
        }
    }
}
