using System;
using System.Text;

namespace CryptoSystems.Models
{
    public class Terms
    {
        private readonly Term[] _terms;

        public Terms(int length)
        {
            _terms = new Term[length];
        }

        public int this[int x, int y]
        {
            get
            {
                for (var i = 0; i < _terms.Length; i++)
                {
                    if (_terms[i].X == x && _terms[i].Y == y)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        public void SetTerm(int i, int x, int y)
        {
            _terms[i] = new Term(x, y);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var term in _terms)
            {
                stringBuilder.AppendLine($"{term.X} {term.Y}");
            }
            return stringBuilder.ToString();
        }
    }
}
