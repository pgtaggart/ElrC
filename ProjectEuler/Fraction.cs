using System;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleApplication1
{
    public class Fraction
    {
        private readonly BigInteger _denominator;
        private readonly BigInteger _numerator;

        public static Fraction One = new Fraction(1, 1);
        public static Fraction Two = new Fraction(2, 1);

        public Fraction(BigInteger numerator, BigInteger denominator)
        {
            _numerator = numerator;
            _denominator = denominator;
        }

        public override String ToString()
        {
            return _numerator + "/" + _denominator;
        }

        public BigInteger GetNumerator()
        {
            return _numerator;
        }

        public BigInteger GetDenominator()
        {
            return _denominator;
        }

        public Fraction AddFraction(Fraction f)
        {
            //get common denominator
            var commonDenominator = _denominator * f.GetDenominator();
            var addition = (_numerator * (commonDenominator / _denominator)) + (f.GetNumerator() * (commonDenominator / f.GetDenominator()));

            return new Fraction(addition, commonDenominator);
        }

        public Fraction SubtractFraction(Fraction f)
        {
            //get common denominator
            var commonDenominator = _denominator * f.GetDenominator();
            var addition = (_numerator * (_denominator / commonDenominator)) - (f.GetNumerator() * (f.GetDenominator() / commonDenominator));

            return new Fraction(addition, commonDenominator);

        }

        public Fraction MultiplyFraction(Fraction f)
        {
            return new Fraction(_numerator * f.GetNumerator(), _denominator * f.GetDenominator());
        }

        public Fraction DivideFraction(Fraction f)
        {
            //switch numerator/denominator on f and multiply
            return MultiplyFraction(new Fraction(f.GetDenominator(), f.GetNumerator()));
        }

        public Fraction MinimiseFraction()
        {
            var a = BigInteger.GreatestCommonDivisor(_numerator, _denominator);
            return new Fraction(_numerator / a, _denominator / a);
        }

        public List<BigInteger> GetContinuousFractionFromGcd()
        {
            var cfList = new List<BigInteger>();

            var numerator = _numerator < _denominator ? _denominator : _numerator;
            var denominator = _numerator < _denominator ? _numerator : _denominator;

            BigInteger modDivision;
            var divisor = BigInteger.DivRem(numerator, denominator, out modDivision);

            cfList.Add(divisor);
            Console.Out.WriteLine();
            Console.Out.WriteLine("{0} = {1} x {2} + {3}", numerator, divisor, denominator, modDivision);

            do
            {
                numerator = denominator;
                denominator = modDivision;
                divisor = BigInteger.DivRem(numerator, denominator, out modDivision);
                cfList.Add(divisor);
                Console.Out.WriteLine("{0} = {1} x {2} + {3}", numerator, divisor, denominator, modDivision);


            } while (!modDivision.Equals(BigInteger.Zero));

            Console.Out.WriteLine();

            return cfList;
        }

        public decimal GetDecimalValue()
        {

            if (_numerator > new BigInteger(double.MaxValue) || _denominator > new BigInteger(double.MaxValue))
            {
                Console.Out.WriteLine("Error: Numerator or Denominator of Fraction was outside of divisible range: {0}", this);
                return -1;
            }

            var value = (double)_numerator / (double)_denominator;

            if (!double.IsInfinity(value) && !double.IsNaN(value)) return (decimal)_numerator / (decimal)_denominator;

            Console.Out.WriteLine("Error: Decimal devision of Fraction was outside of range: {0}", this);

            return -1;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Fraction)) return false;

            var b = (Fraction)obj;

            if (_numerator.Equals(b.GetNumerator()) && _denominator.Equals(b.GetDenominator()))
            {
                return true;
            }

            var c = MinimiseFraction();
            var d = b.MinimiseFraction();

            if (c.GetNumerator().Equals(d.GetNumerator()) && c.GetDenominator().Equals(d.GetDenominator()))
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {

            var hashCode = (_numerator / _denominator) * (_numerator.ToString().Length / (_denominator.ToString().Length + 1));

            return int.Parse(hashCode.ToString());
        }

        public class FractionComparator : Comparer<Fraction>
        {
            public override int Compare(Fraction x, Fraction y)
            {
                var retVal = 0;

                if (x != null && y != null)
                {
                    var a = decimal.Parse(x.GetNumerator().ToString());
                    var b = decimal.Parse(x.GetDenominator().ToString());

                    var c = decimal.Parse(y.GetNumerator().ToString());
                    var d = decimal.Parse(y.GetDenominator().ToString());

                    var e = decimal.Divide(a, b);
                    var f = decimal.Divide(c, d);

                    retVal = e.CompareTo(f);
                }

                return retVal;
            }
        }

    }
}