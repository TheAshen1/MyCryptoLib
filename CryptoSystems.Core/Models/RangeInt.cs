using System;

namespace CryptoSystems.Models
{
    public class RangeInt
    {
        #region Properties
        public int Start { get; }
        public int End { get; }

        public int Length => End - Start;
        #endregion

        #region Constructors
        public RangeInt(int length) : this(0, length)
        {
        }

        public RangeInt(int start, int end)
        {
            Start = Math.Min(start, end);
            End = Math.Max(start, end);
        }
        #endregion

        #region Methods
        public bool Contains(int number)
        {
            return  Start <= number && number < End;
        }

        public bool Contains(RangeInt range)
        {
            return Start <= range.Start && range.End <= End;
        }

        public override string ToString()
        {
            return String.Format("{0} [{1} - {2}]\n", GetType().Name, Start, End);
        }
        #endregion
    }
}
