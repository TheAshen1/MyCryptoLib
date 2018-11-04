using McElieceCryptosystem.Models;

namespace McElieceCryptosystem.Algorithms
{
    public static class PolynomialAlgorithms
    {
        public static PolynomialDouble GCD(PolynomialDouble polynomialLeft, PolynomialDouble polynomialRight)
        {
            PolynomialDouble A;
            PolynomialDouble B;

            if (polynomialLeft.Degree >= polynomialRight.Degree)
            {
                A = polynomialLeft.Clone();
                B = polynomialRight.Clone();
            }
            else
            {
                A = polynomialRight.Clone();
                B = polynomialLeft.Clone();
            }

            while (!B.IsZero())
            {
                var divisionResult = A / B;

                A = B;
                B = divisionResult.Mod;
            }
            return A;
        }

        public static PolynomialExtendedGcdResult ExtendedGCD(PolynomialDouble polynomialLeft, PolynomialDouble polynomialRight)
        {
            PolynomialDouble A;
            PolynomialDouble B;

            PolynomialDouble X0 = new PolynomialDouble(new double[] { 1 });
            PolynomialDouble X1 = new PolynomialDouble(new double[] { 0 });
            PolynomialDouble Y0 = new PolynomialDouble(new double[] { 0 });
            PolynomialDouble Y1 = new PolynomialDouble(new double[] { 1 });

            if (polynomialLeft.Degree >= polynomialRight.Degree)
            {
                A = polynomialLeft.Clone();
                B = polynomialRight.Clone();
            }
            else
            {
                A = polynomialRight.Clone();
                B = polynomialLeft.Clone();
            }

            while (!B.IsZero())
            {
                var divisionResult = A / B;

                A = B;
                B = divisionResult.Mod;

                var tempX = X0 - divisionResult.Result * X1;
                X0 = X1;
                X1 = tempX;

                var tempY = Y0 - divisionResult.Result * Y1;
                Y0 = Y1;
                Y1 = tempY;
            }
            var result = new PolynomialExtendedGcdResult
            {
                Gcd = A,
                S = X1,
                T = Y0
            };
            return result;
        }

        public static PolynomialDouble ModularComposition(int degree, PolynomialDouble polynomial)
        {
            if (degree == 0)
            {
                return new PolynomialDouble(1, 1.0);
            }


            if (degree % 2 == 0)
            {
                var temp = ModularComposition(degree / 2, polynomial);
                var anotherOne = new PolynomialDouble(temp.Degree + temp.Degree, 1.0);
                return (anotherOne / polynomial).Mod;
            }
            else
            {
                var temp = ModularComposition(degree - 1, polynomial);
                return (temp * temp / polynomial).Mod;
            }
        }

        public static bool IsIrreducible(PolynomialDouble polynomial)
        {
            var result = ModularComposition(polynomial.Degree, polynomial);

            if (result == new PolynomialDouble(1, 1.0))
            {
                return true;
            }

            return false;
        }
    }
}
