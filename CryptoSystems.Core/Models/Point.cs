namespace CryptoSystems.Models
{
    public struct Point
    {
        public readonly int x;
        public readonly int y;
        public readonly int z;

        public Point(int x,int y,int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
