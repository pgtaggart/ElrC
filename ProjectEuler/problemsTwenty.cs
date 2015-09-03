using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;

namespace ConsoleApplication1
{
    public class ProblemsTwenty
    {
        private readonly Library _lib;

        public ProblemsTwenty(Library lib)
        {
            _lib = lib;
        }

        /*
         * Euler discovered the remarkable quadratic formula:
         *
         * n² + n + 41
         *
         * It turns out that the formula will produce 40 primes for the consecutive values n = 0 to 39. 
         * However, when n = 40, 40^2 + 40 + 41 = 40(40 + 1) + 41 is divisible by 41, 
         * and certainly when n = 41, 41² + 41 + 41 is clearly divisible by 41.
         *
         * The incredible formula  n² − 79n + 1601 was discovered, which produces 80 primes for the consecutive values n = 0 to 79. 
         * The product of the coefficients, −79 and 1601, is −126479.
         *
         * Considering quadratics of the form:
         *
         * n² + an + b, where |a| < 1000 and |b| < 1000
         *
         * where |n| is the modulus/absolute value of n
         * e.g. |11| = 11 and |−4| = 4
         *
         * Find the product of the coefficients, a and b, for the quadratic expression that produces the maximum number 
         * of primes for consecutive values of n, starting with n = 0.
         * 
         * So the numbers are a=-61 and b=971, a*b = -59231
         * 
         */

        public void Problem27()
        {
            //get large list of primes
            List<int> primes = _lib.GetListOfPrimes(2000000);

            Console.Out.WriteLine("Number of consecutive primes to start: " + primes.Count);

            // n^2 + an + bn
            // consecutive values of n
            // a < 1000 > b
            int maxConsecutive = 0;
            int aFinal = 0;
            int bFinal = 0;

            //a goes odd numbers from -999 to 999
            for (int a = -999; a < 1000; a += 2)
            {
                //b must be positive and prime
                foreach (int t in primes)
                {
                    if (t > 1000)
                    {
                        break;
                    }

                    //variables for consecutive prime count
                    int count = 0;

                    //now go for a range of n
                    for (int n = 0; n < 100; n++)
                    {
                        int quad = (int)Math.Pow(n, 2) + (a * n) + t;

                        //is this prime?
                        if (primes.Contains(quad))
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                    } //end n loop

                    //check the count
                    if (count > maxConsecutive)
                    {
                        maxConsecutive = count;
                        aFinal = a;
                        bFinal = t;
                    }
                }
            } // end a loop

            Console.Out.WriteLine("a = " + aFinal + ", b = " + bFinal);
            Console.Out.WriteLine("a * b = " + aFinal * bFinal);
        } //end problem 27


        /**
        * 
        * Surprisingly there are only three numbers that can be written as the sum of fourth powers of their digits:
        *
        * 1634 = 1^4 + 6^4 + 3^4 + 4^4
        * 8208 = 8^4 + 2^4 + 0^4 + 8^4
        * 9474 = 9^4 + 4^4 + 7^4 + 4^4
        *
        * As 1 = 1^4 is not a sum it is not included.
        *
        * The sum of these numbers is 1634 + 8208 + 9474 = 19316.
        * 
        * Find the sum of all the numbers that can be written as the sum of fifth powers of their digits.
        *
        */

        public void Problem30()
        {
            var matchingNumbers = new List<int>();

            //go through the numbers
            for (int number = 10; number < 9999999; number++)
            {
                //split into individual digits
                char[] numberArray = number.ToString(CultureInfo.InvariantCulture).ToCharArray();

                //sum the 5th powers of the individual digits
                var sum =
                    numberArray.Sum(digit => (int)Math.Pow(int.Parse(digit.ToString(CultureInfo.InvariantCulture)), 5));

                if (sum == number)
                {
                    matchingNumbers.Add(number);
                }
            }

            matchingNumbers.ForEach(e => Console.Out.WriteLine(e));

            Console.Out.WriteLine("Number Sum = " + matchingNumbers.Sum());
        }

        /**
         * In England the currency is made up of pound, £, and pence, p, and there are eight coins in general circulation:
         *
         * 1p, 2p, 5p, 10p, 20p, 50p, £1 (100p) and £2 (200p).
         *
         * It is possible to make £2 in the following way:
         *
         * 1×£1 + 1×50p + 2×20p + 1×5p + 1×2p + 3×1p
         *
         * How many different ways can £2 be made using any number of coins?
         * 
         * 
         *  Input:
         *  Values (stored in array v)
         *  Weights (stored in array w)
         *  Number of distinct items (n)
         *  Knapsack capacity (W)
         *  
         *  for j from 0 to W do
         *       m[0, j] := 0
         *  end for 
         *       
         *  for i from 1 to n do 
         *      for j from 0 to W do
         *          if w[i] <= j then
         *               m[i, j] := max(m[i-1, j], m[i-1, j-w[i]] + v[i])
         *          else
         *               m[i, j] := m[i-1, j]
         *          end if
         *      end for
         * end for
         * 
         */

        public void Problem31()
        {
            //define the available coins
            //var coins = new[] { 1, 2, 5, 10, 20, 50, 100, 200 };

            var coins = new[] { 1, 2, 5, 10 };

            int m = coins.Length / coins[0];

            int count = Count(coins, m, 5, "initial");

            Console.Out.WriteLine(count);
        }

        // Returns the count of ways we can sum  S[0...m-1] coins to get sum n
        public int Count(int[] coins, int m, int n, string label)
        {
            Console.Out.Write(label + " - m:" + m + " n: " + n + " [ ");

            coins.ToList().ForEach(e => Console.Out.Write(e + " "));

            Console.Out.WriteLine("]");

            // If n is 0 then there is 1 solution (do not include any coin)    
            if (n == 0)
                return 1;

            // If n is less than 0 then no solution exists    
            if (n < 0)
                return 0;

            // If there are no coins and n is greater than 0, then no solution exist    
            if (m <= 0 && n >= 1)
                return 0;

            // count is sum of solutions (i) including S[m-1] (ii) excluding S[m-1]    
            return Count(coins, m - 1, n, "left   ") + Count(coins, m, n - coins[m - 1], "right  ");
        }


        /**
         * The fraction 49/98 is a curious fraction, as an inexperienced mathematician in 
         * attempting to simplify it may incorrectly believe that 49/98 = 4/8, which is correct, is obtained by cancelling the 9s.
         *
         * We shall consider fractions like, 30/50 = 3/5, to be trivial examples.
         *
         * There are exactly four non-trivial examples of this type of fraction, less than one in value, 
         * and containing two digits in the numerator and denominator.
         *
         * If the product of these four fractions is given in its lowest common terms, find the value of the denominator.
         * 
         */

        public void Problem33()
        {
            for (int x = 10; x < 100; x++)
            {
                for (int y = 10; y < 100; y++)
                {
                    if (y <= x)
                    {
                        continue;
                    }

                    float fraction1 = (float)x / y;

                    //get the numbers as strings
                    string sX = x.ToString(CultureInfo.InvariantCulture);
                    string sY = y.ToString(CultureInfo.InvariantCulture);

                    //length should be 2 for each
                    if (sX.Length != 2 && sY.Length != 2)
                    {
                        Console.Out.WriteLine("ERROR: unexpected string lengths: " + sX + ", " + sY);
                        continue;
                    }

                    //start removing some numbers
                    float x1 = float.Parse(sX.Substring(0, 1));
                    float x2 = float.Parse(sX.Substring(1, 1));
                    float y1 = float.Parse(sY.Substring(0, 1));
                    float y2 = float.Parse(sY.Substring(1, 1));

                    if (x2.ToString(CultureInfo.InvariantCulture).Equals("0") &&
                        y2.ToString(CultureInfo.InvariantCulture).Equals("0"))
                    {
                        //trivial comparison
                        continue;
                    }

                    if (x1.ToString(CultureInfo.InvariantCulture).Equals(x2.ToString(CultureInfo.InvariantCulture))
                        && y1.ToString(CultureInfo.InvariantCulture).Equals(y2.ToString(CultureInfo.InvariantCulture)))
                    {
                        //trivial comparison
                        continue;
                    }

                    string sFraction = x.ToString(CultureInfo.InvariantCulture)
                                       + " / " + y.ToString(CultureInfo.InvariantCulture)
                                       + " = " + fraction1.ToString(CultureInfo.InvariantCulture);

                    //test the replacements
                    if ((x1 / y1).Equals(fraction1) && x2.Equals(y2))
                    {
                        Console.Out.WriteLine(sFraction + " : " + x1 + " / " + y1 + " = " + (x1 / y1));
                    }
                    else if ((x1 / y2).Equals(fraction1) && x2.Equals(y1))
                    {
                        Console.Out.WriteLine(sFraction + " : " + x1 + " / " + y2 + " = " + (x1 / y2));
                    }
                    else if ((x2 / y1).Equals(fraction1) && x1.Equals(y2))
                    {
                        Console.Out.WriteLine(sFraction + " : " + x2 + " / " + y1 + " = " + (x2 / y1));
                    }
                    else if ((x2 / y2).Equals(fraction1) && x1.Equals(y1))
                    {
                        Console.Out.WriteLine(sFraction + " : " + x2 + " / " + y2 + " = " + (x2 / y2));
                    }
                }
            }
        }

        /**
         * 
         * 145 is a curious number, as 1! + 4! + 5! = 1 + 24 + 120 = 145.
         *
         * Find the sum of all numbers which are equal to the sum of the factorial of their digits.
         *
         * Note: as 1! = 1 and 2! = 2 are not sums they are not included.
         * 
         */

        public void Problem34()
        {
            for (var i = 3; i < 90000; i++)
            {
                var b = new BigInteger(i);
                var sum = new BigInteger(0);

                sum = b.ToString().ToArray().Aggregate(sum, (current, c)
                    =>
                    BigInteger.Add(current,
                        Factorial(new BigInteger(int.Parse(c.ToString(CultureInfo.InvariantCulture))))));

                //check if we find one
                if (!sum.Equals(b)) continue;

                Console.Out.Write("i: " + b + "[ ");

                foreach (var c in b.ToString().ToArray())
                {
                    Console.Out.Write(new BigInteger(int.Parse(c.ToString(CultureInfo.InvariantCulture))) + " ");
                }

                Console.Out.WriteLine("] " + ", sum: " + sum);
            }
        }

        public BigInteger Factorial(BigInteger number)
        {
            if (number.IsZero) return BigInteger.One;

            return number.IsOne
                ? new BigInteger(1)
                : BigInteger.Multiply(number, Factorial(BigInteger.Subtract(number, new BigInteger(1))));
        }


        /*
         * The number, 197, is called a circular prime because all rotations of the digits: 197, 971, and 719, are themselves prime.
         *
         * There are thirteen such primes below 100: 2, 3, 5, 7, 11, 13, 17, 31, 37, 71, 73, 79, and 97.
         *
         * How many circular primes are there below one million
         * 
         */

        public void Problem35(int target)
        {
            //get list of primes up to target
            List<int> primes = _lib.GetListOfPrimes(target);

            var circularPrimes = new List<int>();

            //go through numbers up to 1 million
            foreach (int prime in primes)
            {
                IEnumerable<int> rotations = NumberRotations(prime.ToString(CultureInfo.InvariantCulture));

                var enumerable = rotations as int[] ?? rotations.ToArray();
                if (!enumerable.All(primes.Contains)) continue;

                circularPrimes.Add(prime);
                Console.Out.Write("Circular Prime: ");

                Console.Out.Write(prime + " [ ");
                enumerable.ToList().ForEach(e => Console.Out.Write(e + " "));
                Console.Out.WriteLine("]");
            }

            Console.Out.WriteLine("Number of Circular primes below {0} : {1} ", target, circularPrimes.Count);
        }

        private static IEnumerable<int> NumberRotations(string a)
        {
            //make the list to hold the rotations
            var rotations = new List<int> { int.Parse(a) };

            string current = a;

            for (int i = 0; i < a.Length - 1; i++)
            {
                current = current.Substring(1, current.Length - 1) + current.Substring(0, 1);

                rotations.Add(int.Parse(current));
            }

            return rotations;
        }

        /**
         * 
         * The decimal number, 585 = 1001001001 (base 2) (binary), is palindromic in both bases.
         *
         * Find the sum of all numbers, less than one million, which are palindromic in base 10 and base 2.
         *
         * (Please note that the palindromic number, in either base, may not include leading zeros.)
         * 
         */

        public void Problem36(int target)
        {
            var pNumbers = new List<int>();

            for (int i = 1; i < target; i++)
            {
                string sI = i.ToString(CultureInfo.InvariantCulture);
                var sIr = new String(sI.ToCharArray().Reverse().ToArray());

                //is palindrome in base 10 ?
                if (!sI.Equals(sIr)) continue;

                string binaryFormat = Convert(i);
                var binaryReverse = new string(binaryFormat.Reverse().ToArray());

                //is palindrome in base 2 ?
                if (!binaryFormat.Equals(binaryReverse)) continue;

                //if it gets here then success
                Console.Out.WriteLine("Number: {0}, Reverse Number: {1}, Binary: {2}, Reverse Binary: {3}",
                    sI, sIr, binaryFormat, binaryReverse);

                //add to results
                pNumbers.Add(i);
            }

            Console.Out.WriteLine("Sum of Palindrome Numbers: {0}", pNumbers.Sum());
        }


        public string Convert(int x)
        {
            var bits = new char[32];
            int i = 0;

            while (x != 0)
            {
                bits[i++] = (x & 1) == 1 ? '1' : '0';
                x >>= 1;
            }

            Array.Reverse(bits, 0, i);
            return new string(bits.ToList().Where(c => !c.Equals('\0')).ToArray());
        }


        /**
         * The number 3797 has an interesting property. 
         * Being prime itself, it is possible to continuously remove digits from left to right, 
         * and remain prime at each stage: 3797, 797, 97, and 7. 
         * Similarly we can work from right to left: 3797, 379, 37, and 3.
         *
         * Find the sum of the only eleven primes that are both truncatable from left to right and right to left
         * 
         * NOTE: 2, 3, 5, and 7 are not considered to be truncatable primes
         * 
         */

        public void Problem37()
        {
            var primes = _lib.GetListOfPrimes(1000000);

            var truncatablePrimesSum = (from prime in primes
                                        where !new[] { 2, 3, 5, 7 }.Contains(prime)
                                        let ltr = GetDecreasingSubStringsAsInts(prime, 0)
                                        where ltr.All(primes.Contains)
                                        let rtl = GetDecreasingSubStringsAsInts(prime, 1)
                                        where rtl.All(primes.Contains)
                                        select prime).ToList().Sum();

            Console.Out.WriteLine("Sum: {0}", truncatablePrimesSum);
        }

        public List<int> GetDecreasingSubStringsAsInts(int number, int direction)
        {
            var subInts = new List<int>();

            string sNumber = number.ToString(CultureInfo.InvariantCulture);

            switch (direction)
            {
                case 0:
                    subInts.AddRange(sNumber.Select((t, i) => int.Parse(sNumber.Substring(i, sNumber.Length - i))));
                    break;
                case 1:
                    for (int i = sNumber.Length; i > 0; i--)
                    {
                        subInts.Add(int.Parse(sNumber.Substring(0, i)));
                    }
                    break;
            }

            return subInts;
        }

        /**
         * 
         * Take the number 192 and multiply it by each of 1, 2, and 3:
         * 192 × 1 = 192
         * 192 × 2 = 384
         * 192 × 3 = 576
         * 
         * By concatenating each product we get the 1 to 9 pandigital, 192384576. 
         * We will call 192384576 the concatenated product of 192 and (1,2,3)
         * 
         * The same can be achieved by starting with 9 and multiplying by 1, 2, 3, 4, and 5, 
         * giving the pandigital, 918273645, which is the concatenated product of 9 and (1,2,3,4,5)
         * 
         * What is the largest 1 to 9 pandigital 9-digit number that can be formed as 
         * the concatenated product of an integer with (1,2, ... , n) where n > 1?
         * 
         */

        public void Problem38()
        {

            var digits = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var panDigitalProducts = new List<int>();

            for (var i = 9; i < (int)Math.Sqrt(987654321); i++)
            {

                var sum = "";

                for (var j = 1; j < 100; j++)
                {

                    sum += (i * j).ToString(CultureInfo.InvariantCulture);

                    if (sum.Length != 9) continue;

                    if (sum.Length > 9) break;

                    if (digits.All(sum.ToList()
                        .Select(e => int.Parse(e.ToString(CultureInfo.InvariantCulture)))
                        .Contains))
                    {
                        panDigitalProducts.Add(int.Parse(sum));
                    }

                } //end j
            } //end i

            Console.Out.WriteLine("Max Pandigital Product: {0}", panDigitalProducts.Max());

        }


        /**
         * 
         * If p is the perimeter of a right angle triangle with integral length sides, {a,b,c}, 
         * there are exactly three solutions for p = 120
         * 
         * {20,48,52}, {24,45,51}, {30,40,50}
         * 
         * For which value of p ≤ 1000, is the number of solutions maximised
         * 
         */

        public void Problem39()
        {

            var solutions = new Dictionary<int, List<String>>();

            for (var p = 1; p < 1000; p++)
            {
                var pSolutions = new List<String>();

                for (var i = 1; i < 1000; i++)
                {
                    for (var j = i; j < 1000; j++)
                    {

                        if (i + j > p) break;

                        for (var k = j; k < 1000; k++)
                        {
                            if (i + j + k > p) break;

                            if (i + j + k != p) continue;

                            if ((int)Math.Pow(i, 2) + (int)Math.Pow(j, 2) == (int)Math.Pow(k, 2))
                            {
                                //solution found for p
                                pSolutions.Add("{" + i + "," + j + "," + k + "}");
                            }
                        }
                    }
                }

                if (pSolutions.Count == 0) continue;

                solutions.Add(p, pSolutions);

                Console.Out.Write("p: {0} , No. Solutions: {1} - ", p, pSolutions.Count);
                foreach (var ijk in pSolutions)
                {
                    Console.Out.Write(ijk);

                    if (pSolutions.IndexOf(ijk) < pSolutions.Count - 1)
                    {
                        Console.Out.Write(", ");
                    }

                }
                Console.Out.WriteLine();
            } //end p loop

            var max = 0;
            var finalSolution = "";

            foreach (var solution in solutions)
            {

                if (solution.Value.Count > max)
                {
                    max = solution.Value.Count;
                    finalSolution = solution.Key + " : ";

                    foreach (var ijk in solution.Value)
                    {
                        finalSolution += ijk;

                        if (solution.Value.IndexOf(ijk) < solution.Value.Count - 1)
                        {
                            finalSolution += ", ";
                        }

                    }

                }

            }

            Console.Out.WriteLine("Solution: {0}", finalSolution);


        }


        /**
         * 
         * An irrational decimal fraction is created by concatenating the positive integers:
         * 
         * 0.12345678910(1)112131415161718192021...
         * 
         * It can be seen that the 12th digit of the fractional part is 1.
         * 
         * If dn represents the nth digit of the fractional part, find the value of the following expression.
         * 
         * d(1) × d(10) × d(100) × d(1000) × d(10000) × d(100000) × d(1000000)
         * 
         */

        public void Problem40()
        {
            var product = 1;
            var len = 0;
            var i = 0;
            var index = 1;

            while (len < 1000000)
            {
                i++;
                var s = i.ToString(CultureInfo.InvariantCulture);

                if (len + s.Length < index)
                {
                    len += s.Length;
                    continue;
                }

                var b = index - len - 1;

                Console.Out.WriteLine("index: {0}, length: {1}, i: {2}, digit: {3}, i-index: {4}",
                    index, len, i, s.Substring(b, 1), b);

                product *= int.Parse(s.Substring(b, 1));
                len += s.Length;
                index *= 10;

            }

            Console.Out.WriteLine(product);

        }


        /**
         * 
         * We shall say that an n-digit number is pandigital if it makes use of all the digits 1 to n exactly once. 
         * For example, 2143 is a 4-digit pandigital and is also prime.
         * 
         * What is the largest n-digit pandigital prime that exists?
         * 
         */

        public void Problem41()
        {
            var primes = _lib.GetListOfPrimes(1000000000);

            var digits = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (var i = 1; i < digits.Length; i++)
            {

                var dList = digits.ToList().GetRange(0, i);

                foreach (var prime in primes)
                {
                    var sPrime = prime.ToString(CultureInfo.InvariantCulture);

                    if (sPrime.Length > dList.Count) break;

                    if (sPrime.Length == dList.Count && dList.All(sPrime.ToList()
                        .Select(e => int.Parse(e.ToString(CultureInfo.InvariantCulture)))
                        .Contains))
                    {
                        Console.Out.WriteLine("Pandigital Prime Found for n = {0}, Prime: {1}", dList.Count, prime);
                    }

                }
            }
        }

        /**
         * 
         * The first two consecutive numbers to have two distinct prime factors are:
         * 
         * 14 = 2 × 7
         * 15 = 3 × 5
         * 
         * The first three consecutive numbers to have three distinct prime factors are:
         * 
         * 644 = 2² × 7 × 23
         * 645 = 3 × 5 × 43
         * 646 = 2 × 17 × 19.
         * 
         * Find the first four consecutive integers to have four distinct prime factors. 
         * What is the first of these numbers?
         * 
         */

        public void Problem47()
        {

            var primes = _lib.GetListOfPrimes(1000000);

            const int count = 4;

            for (var i = 1000; i < 1000000; i++)
            {
                var consecutive = true;

                for (var j = i; j < i + count; j++)
                {
                    if (_lib.PrimeFactorise(j, primes).Distinct().Count() == count) continue;

                    consecutive = false;
                    break;
                }

                if (!consecutive) continue;

                for (var x = i; x < i + count; x++)
                {
                    Console.Out.Write("Number: {0}, Prime Factorisation: [ ", x);
                    var pF = _lib.PrimeFactorise(x, primes);
                    pF.ForEach(e => Console.Out.Write(e + " "));
                    Console.Out.WriteLine("], Distinct: {0}", pF.Distinct().Count());
                }

                break;
            }

        }


        /**
         * The series, 1^1 + 2^2 + 3^3 + ... + 10^10 = 10405071317.
         *
         * Find the last ten digits of the series, 1^1 + 2^2 + 3^3 + ... + 1000^1000.
         *
         * 9110846700
         * 
         */

        public void Problem48()
        {

            BigInteger total = 0;

            for (UInt64 i = 1; i <= 1000; i++)
            {
                total += _lib.Power(i);

            }

            Console.Out.WriteLine(total.ToString().Substring(total.ToString().Length - 10));
        }


        /**
         * 
         * The arithmetic sequence, 1487, 4817, 8147, in which each of the terms increases by 3330, is unusual in two ways: 
         * 
         * (i) each of the three terms are prime, and, 
         * (ii) each of the 4-digit numbers are permutations of one another.
         * 
         * There are no arithmetic sequences made up of three 1-, 2-, or 3-digit primes, exhibiting this property, 
         * but there is one other 4-digit increasing sequence.
         * 
         * What 12-digit number do you form by concatenating the three terms in this sequence?
         * 
         */

        public void Problem49()
        {

            var primes = _lib.GetListOfPrimes(100000);

            const int increase = 3330;

            for (var i = 1000; i < 1000 + (increase * 2); i++)
            {
                if (!primes.Contains(i) || !primes.Contains(i + increase) || !primes.Contains(i + increase + increase))
                    continue;


                var number = (i).ToString(CultureInfo.InvariantCulture).ToList();
                var numberPlusIncrease = (i + increase).ToString(CultureInfo.InvariantCulture).ToList();
                var numberPlusIncreaseTwice = (i + increase + increase).ToString(CultureInfo.InvariantCulture).ToList();

                number.Sort();
                numberPlusIncrease.Sort();
                numberPlusIncreaseTwice.Sort();

                var numberString = new string(number.ToArray());
                var numberPlusIncreaseString = new string(numberPlusIncrease.ToArray());
                var numberPlusIncreaseTwiceString = new string(numberPlusIncreaseTwice.ToArray());

                if (numberString.Equals(numberPlusIncreaseString) &&
                    numberPlusIncreaseString.Equals(numberPlusIncreaseTwiceString))
                {
                    Console.Out.WriteLine("Sequence found: {0},{1},{2}", i, i + increase, i + increase + increase);
                }
            }

        }

        /**
         * 
         * The prime 41, can be written as the sum of six consecutive primes
         * 
         * 41 = 2 + 3 + 5 + 7 + 11 + 13
         * 
         * This is the longest sum of consecutive primes that adds to a prime below one-hundred.
         * The longest sum of consecutive primes below one-thousand that adds to a prime, contains 21 terms, and is equal to 953
         * 
         * Which prime, below one-million, can be written as the sum of the most consecutive primes
         * 
         */

        public void Problem50()
        {
            var primes = _lib.GetListOfPrimes(1000000);

            const int limit = 1000000;

            var prime = 0;
            var maxCount = 0;
            var minprime = 0;

            for (var i = 0; i < primes.Count; i++)
            {
                if (primes[i] > limit) break;

                var result = 0;
                var count = 0;

                while (i + count < primes.Count && result + primes[i + count] < limit)
                {
                    result += primes[i + count];
                    count++;
                }

                if (count < maxCount) continue;

                while (!primes.Contains(result) && result > 0)
                {
                    result -= primes[i + count - 1];
                    count--;
                }

                if (count <= maxCount) continue;

                maxCount = count;
                prime = result;
                minprime = primes[i];
            }

            Console.Out.WriteLine("Prime: {0} is {1} consecutive primes starting from {2}", prime, maxCount, minprime);

        }

        public void TestCombinatorics()
        {

            var input5 = new[] { 0, 1, 2, 3, 4 };
            var input6 = new[] { 0, 1, 2, 3, 4, 5 };
            var input7 = new[] { 0, 1, 2, 3, 4, 5, 6 };

            var inputs = new List<int[]> { input5, input6, input7 };


            for (var i = 0; i <= 10; i++)
            {
                Console.Out.WriteLine("{0} Factorial ({1}!) = {2} ", i, i, _lib.Factorial(i));
            }


            //184756
            Console.Out.WriteLine("20C10 = {0}", _lib.GetNumberOfCombinations(20, 10));

            //1
            Console.Out.WriteLine("10C10 = {0}", _lib.GetNumberOfCombinations(10, 10));

            foreach (var input in inputs)
            {
                for (var r = 1; r <= input.Length; r++)
                {
                    var combinations = _lib.GetCombinations(input, r);
                    var expectedSize = _lib.GetNumberOfCombinations(input.Length, r);

                    for (var i = 0; i < combinations.Count; i++)
                    {
                        for (var j = 0; j < combinations.Count; j++)
                        {

                            var isEqual = combinations[i].SequenceEqual(combinations[j]);

                            if (i == j)
                            {
                                if (!isEqual)
                                {
                                    Console.Out.WriteLine("This compare thing does not work");
                                }
                            }
                            else
                            {
                                if (isEqual)
                                {
                                    Console.Out.WriteLine(
                                        "Duplicate found in List with size {0} at index i={1} and j={2}", input.Length,
                                        i, j);
                                }
                            }

                        }
                    }

                    Console.Out.WriteLine(
                        combinations.Count() == expectedSize
                            ? "SUCCESS: Expected {0} combinations and got: {1}.  n is {2} and r is {3}"
                            : "ERROR: Expected {0} combinations but got: {1}.  n is {2} and r is {3}",
                        expectedSize, combinations.Count, input.Length, r);
                }
            }
        }

        /**
         * 
         * It is possible to show that the square root of two can be expressed as an infinite continued fraction.
         * 
         * √2 = 1 + 1/(2 + 1/(2 + 1/(2 + ... ))) = 1.414213...
         * 
         * By expanding this for the first four iterations, we get:
         * 
         * 1 + 1/2 = 3/2 = 1.5
         * 1 + 1/(2 + 1/2) = 7/5 = 1.4
         * 1 + 1/(2 + 1/(2 + 1/2)) = 17/12 = 1.41666...
         * 1 + 1/(2 + 1/(2 + 1/(2 + 1/2))) = 41/29 = 1.41379
         * 
         * The next three expansions are 99/70, 239/169, and 577/408, but the eighth expansion, 
         * 1393/985, is the first example where the number of digits in the numerator exceeds the number of digits in the denominator.
         * 
         * In the first one-thousand expansions, how many fractions contain a numerator with more digits than denominator?
         * 
         * Answer is 153
         * 
         */
        public void Problem57()
        {
            var result = 0;

            var two = new Fraction(1, 1);

            for (var i = 1; i <= 1000; i++)
            {
                var f = new Fraction(1, 1).AddFraction(_lib.GenerateRootTwoAsContinuousFraction(i, two));

                if (f.GetNumerator().ToString().Length > f.GetDenominator().ToString().Length)
                {
                    result++;
                }

                //Console.Out.WriteLine("Term {0}: {1}/{2}", i, f.GetNumerator(), f.GetDenominator());
            }

            Console.Out.WriteLine("57 -> Result: {0}, Expected Result: {1}", result, 153);

        }

        /*
         * Each character on a computer is assigned a unique code and the preferred standard 
         * is ASCII (American Standard Code for Information Interchange). 
         * For example, uppercase A = 65, asterisk (*) = 42, and lowercase k = 107
         * 
         * A modern encryption method is to take a text file, convert the bytes to ASCII, 
         * then XOR each byte with a given value, taken from a secret key. 
         * 
         * The advantage with the XOR function is that using the same encryption key on the cipher text, 
         * restores the plain text; for example, 65 XOR 42 = 107, then 107 XOR 42 = 65
         * 
         * For unbreakable encryption, the key is the same length as the plain text message, 
         * and the key is made up of random bytes. 
         * 
         * The user would keep the encrypted message and the encryption key in different locations, 
         * and without both "halves", it is impossible to decrypt the message
         * 
         * Unfortunately, this method is impractical for most users, so the modified method is to use a password as a key. 
         * If the password is shorter than the message, which is likely, the key is repeated cyclically throughout the message. 
         * The balance for this method is using a sufficiently long password key for security, but short enough to be memorable
         * 
         * Your task has been made easy, as the encryption key consists of three lower case characters. 
         * Using cipher.txt (right click and 'Save Link/Target As...'), a file containing the encrypted ASCII codes, 
         * and the knowledge that the plain text must contain common English words, 
         * decrypt the message and find the sum of the ASCII values in the original text
         * 
         */
        public void Problem59()
        {
            //set number of letters in english language
            const int dictionarySize = 26;

            //read the file
            var cypher = File.ReadAllLines(@"c:\LocalFolder\dont\cipher.txt")
                .Select(line => line.Split(',').Select(int.Parse).ToArray()).ToList()[0];

            //size of cypher text
            Console.Out.WriteLine("Size of message: {0}", cypher.Length);

            //get all lower and upper case charachers
            var lowerCaseCharacters = new char[dictionarySize];
            var currentChar = (int)'a';

            // The values of actual characters are 65 (A) to 122 (z)
            // add these to the printables
            for (var i = 0; i < dictionarySize; i++)
            {
                lowerCaseCharacters[i] = (char)currentChar;
                currentChar++;
            }

            // get all size 3 permutations of 3 lower case characters
            var permutations = _lib.GetPermutations(lowerCaseCharacters.ToList(), 3);

            // how many permutations was this (how many keys are we going to try)
            // expected to be 15,600 (26P3)
            var enumerable = permutations as IList<IEnumerable<char>> ?? permutations.ToList();
            Console.Out.WriteLine("Number of permutations: {0}", enumerable.Count());

            //for each permutation, repeat it until we reach the message size
            foreach (var permutation in enumerable)
            {
                //permutation is in characters
                var c = permutation as char[] ?? permutation.ToArray();
                var p = c.ToArray();
                var key = new int[cypher.Length];

                for (var i = 0; i < key.Length; i++)
                {
                    //implicit conversion to ASCII int value
                    key[i] = p[i % p.Length];
                }

                var plainText = new int[cypher.Length];

                //apply each newly generated key to the cypher text
                for (var y = 0; y < cypher.Length; y++)
                {
                    plainText[y] = cypher[y] ^ key[y];
                }

                var bob = plainText.Aggregate("", (current, i) => current + (char)i);

                if (!bob.ToUpper().Contains("The Gospel of John".ToUpper())) continue;

                Console.Out.WriteLine(bob);
                c.ToList().ForEach(e => Console.Out.WriteLine(e + " = " + (int)e));

                Console.Out.WriteLine("Plain text sum: " + plainText.ToList().Sum());
                break;
            }
        }

        /**
         * The primes 3, 7, 109, and 673, are quite remarkable. 
         * By taking any two primes and concatenating them in any order the result will always be prime. 
         * For example, taking 7 and 109, both 7109 and 1097 are prime. The sum of these four primes, 
         * 792, represents the lowest sum for a set of four primes with this property.
         * 
         * Find the lowest sum for a set of five primes for which any two primes concatenate to produce another prime
         * 
         * 15,485,863
         * 
         */
        public void Problem60()
        {
            // set of primes for quic searching
            var primes = _lib.GetLongSetOfPrimesFromFile();

            // convert to new list for looping
            var primeList = primes.ToList();

            // loop in order
            primeList.Sort();

            // start with 5 digit primes
            var startIndex = primeList.IndexOf(99991);

            // count down from primes
            for (var index = startIndex; index >= 0; index--)
            {
                // get the prime to start with
                var startPrime = primeList[index];

                // get candidates for this prime
                var nextPrimes = FindConcatenatingPrimes(startPrime, index - 1, primeList, primes);

                // not found so move along
                if (nextPrimes.Count <= 0) continue;

                //loop through
                foreach (var nextPrime in nextPrimes)
                {
                    //get the list of primes that concat with this one
                    var concatPrimesWithNext = FindConcatenatingPrimes(nextPrime, primeList.IndexOf(nextPrime), primeList, primes);

                    foreach (var third in concatPrimesWithNext)
                    {
                        var candidates = new[] { startPrime, nextPrime, third };

                        if (IsCombinationPrime(candidates, primes))
                        {
                            var getFouthList = FindConcatenatingPrimes(third, primeList.IndexOf(third), primeList, primes);

                            foreach (var fourth in getFouthList)
                            {
                                var fourCandidates = new[] { startPrime, nextPrime, third, fourth };

                                if (IsCombinationPrime(fourCandidates, primes))
                                {
                                    //one more time
                                    var fithList = FindConcatenatingPrimes(fourth, primeList.IndexOf(fourth), primeList, primes);

                                    foreach (var fifth in fithList)
                                    {
                                        var fiveCandidates = new[] { startPrime, nextPrime, third, fourth, fifth };

                                        if (IsCombinationPrime(fiveCandidates, primes))
                                        {
                                            Console.Out.WriteLine("Five found:");
                                            fiveCandidates.ToList().ForEach(e => Console.Out.Write(e + " "));
                                            Console.Out.WriteLine("Sum is {0}", fiveCandidates.ToList().Sum());
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        public List<long> FindConcatenatingPrimes(long prime, int index, List<long> primeList, ISet<long> primeSet)
        {
            var matchers = new List<long>();

            for (var next = index; next >= 0; next--)
            {
                var nextPrime = primeList[next];

                if (IsCombinationPrime(new[] { prime, nextPrime }, primeSet))
                {
                    matchers.Add(nextPrime);
                }
            }

            return matchers;
        }

        public bool IsCombinationPrime(long[] input, ISet<long> primes)
        {
            var result = false;

            var combinations = _lib.GetLongCombinations(input, 2);

            var combiPrimes = new List<long>();

            foreach (var combination in combinations)
            {
                combiPrimes.Add(long.Parse(combination[0].ToString(CultureInfo.InvariantCulture) + combination[1].ToString(CultureInfo.InvariantCulture)));
                combiPrimes.Add(long.Parse(combination[1].ToString(CultureInfo.InvariantCulture) + combination[0].ToString(CultureInfo.InvariantCulture)));
            }

            if (combiPrimes.Count(primes.Contains) == combiPrimes.Count)
            {
                result = true;
            }

            return result;
        }

        /**
         * 
         * Triangle, square, pentagonal, hexagonal, heptagonal, and octagonal numbers are all figurate (polygonal) 
         * numbers and are generated by the following formulae (see library)
         * 
         * The ordered set of three 4-digit numbers: 8128, 2882, 8281, has three interesting properties
         * 
         * 1.The set is cyclic, in that the last two digits of each number is the first two digits 
         * of the next number (including the last number with the first)
         * 
         * 2.Each polygonal type: triangle (P3,127=8128), square (P4,91=8281), and pentagonal (P5,44=2882), 
         * is represented by a different number in the set
         * 
         * 3.This is the only set of 4-digit numbers with this property
         * 
         * Find the sum of the only ordered set of six cyclic 4-digit numbers for which each polygonal type: 
         * triangle, square, pentagonal, hexagonal, heptagonal, and octagonal, is represented by a different number in the set
         * 
         */
        public void Problem61()
        {
            const long limit = 10000;

            var triangleNumbers = _lib.GetTriangleNumbers(limit);
            var squareNumbers = _lib.GetSquareNumbers(limit).ToList(); squareNumbers.Sort();
            var pentagonalNumbers = _lib.GetPentagonalNumbers(limit);
            var hexagonalNumbers = _lib.GetHexagonalNumbers(limit);
            var heptagonalNumbers = _lib.GetHeptagonalNumbers(limit);
            var octagonalNumbers = _lib.GetOctagonalNumbers(limit);

            var fourDigitNumbers = new List<long>();
            var fourDigitNumbersAsStrings = new List<string>();

            for (var l = 1000; l < 10000; l++)
            {
                fourDigitNumbers.Add(l);
                fourDigitNumbersAsStrings.Add(l.ToString(CultureInfo.InvariantCulture));
            }

            for (var one = 1000; one < limit; one++)
            {
                var candidateList2 = GetNumbersStartingWith(one.ToString(CultureInfo.InvariantCulture).Substring(0, 2), fourDigitNumbersAsStrings);

                var oneEndsWith = one.ToString(CultureInfo.InvariantCulture).Substring(2, 2);

                foreach (var two in candidateList2)
                {
                    var candidateList3 = GetNumbersStartingWith(two.ToString(CultureInfo.InvariantCulture).Substring(0, 2), fourDigitNumbersAsStrings);

                    foreach (var three in candidateList3)
                    {
                        var candidateList4 = GetNumbersStartingWith(three.ToString(CultureInfo.InvariantCulture).Substring(0, 2), fourDigitNumbersAsStrings);

                        foreach (var four in candidateList4)
                        {
                            var candidateList5 = GetNumbersStartingWith(four.ToString(CultureInfo.InvariantCulture).Substring(0, 2), fourDigitNumbersAsStrings);

                            foreach (var five in candidateList5)
                            {
                                var candidateList6 = GetNumbersStartingWith(five.ToString(CultureInfo.InvariantCulture).Substring(0, 2), fourDigitNumbersAsStrings);

                                foreach (var six in candidateList6)
                                {
                                    if (six.ToString(CultureInfo.InvariantCulture).EndsWith(oneEndsWith))
                                    {
                                        //now we have found an ordered set of 6 cyclic 4-digit numbers
                                        var list = new[] { one, two, three, four, five, six };

                                        //in order of the lists in the question
                                        var criteria = new[] { false, false, false, false, false, false };

                                        foreach (var criterion in list)
                                        {
                                            var numberAlreadySatisfiesOneCriteria = false;

                                            if (triangleNumbers.Contains(criterion))
                                            {

                                                numberAlreadySatisfiesOneCriteria = true;
                                                criteria[0] = true;

                                            }
                                            if (squareNumbers.Contains(criterion))
                                            {
                                                if (numberAlreadySatisfiesOneCriteria)
                                                {
                                                    //possible issue here
                                                    //Console.Out.WriteLine("Number: {0} matches another type of number", criterion);
                                                }
                                                else
                                                {
                                                    numberAlreadySatisfiesOneCriteria = true;
                                                    criteria[1] = true;
                                                }
                                            }
                                            if (pentagonalNumbers.Contains(criterion))
                                            {
                                                if (numberAlreadySatisfiesOneCriteria)
                                                {
                                                    //possible issue here
                                                    //Console.Out.WriteLine("Number: {0} matches another type of number", criterion);
                                                }
                                                else
                                                {
                                                    numberAlreadySatisfiesOneCriteria = true;
                                                    criteria[2] = true;
                                                }
                                            }
                                            if (hexagonalNumbers.Contains(criterion))
                                            {
                                                if (numberAlreadySatisfiesOneCriteria)
                                                {
                                                    //possible issue here
                                                    //Console.Out.WriteLine("Number: {0} matches another type of number", criterion);
                                                }
                                                else
                                                {
                                                    numberAlreadySatisfiesOneCriteria = true;
                                                    criteria[3] = true;
                                                }
                                            }
                                            if (heptagonalNumbers.Contains(criterion))
                                            {
                                                if (numberAlreadySatisfiesOneCriteria)
                                                {
                                                    //possible issue here
                                                    //Console.Out.WriteLine("Number: {0} matches another type of number", criterion);
                                                }
                                                else
                                                {
                                                    numberAlreadySatisfiesOneCriteria = true;
                                                    criteria[4] = true;
                                                }
                                            }
                                            if (octagonalNumbers.Contains(criterion))
                                            {
                                                if (numberAlreadySatisfiesOneCriteria)
                                                {
                                                    //possible issue here
                                                    //Console.Out.WriteLine("Number: {0} matches another type of number", criterion);
                                                }
                                                else
                                                {
                                                    numberAlreadySatisfiesOneCriteria = true;
                                                    criteria[5] = true;
                                                }
                                            }

                                            if (!numberAlreadySatisfiesOneCriteria)
                                            {
                                                //no point continuing
                                                break;
                                            }
                                        }

                                        var solution = true;

                                        foreach (var criterion in criteria)
                                        {
                                            if (!criterion)
                                            {
                                                solution = false;
                                                break;
                                            }
                                        }

                                        if (solution)
                                        {
                                            Console.Out.WriteLine("Solution found!");
                                            list.ToList().ForEach(e => Console.Out.Write(e + " "));
                                            Console.Out.WriteLine(", Sum is {0}", list.ToList().Sum());
                                        }


                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public List<long> GetNumbersStartingWith(string startsWith, List<string> numbers)
        {
            return (from number in numbers where number.StartsWith(startsWith) select int.Parse(number)).Select(dummy => (long)dummy).ToList();
        }

        /**
         * 
         * Triangle, square, pentagonal, hexagonal, heptagonal, and octagonal numbers are all figurate (polygonal) 
         * numbers and are generated by the following formulae (see library)
         * 
         * The ordered set of three 4-digit numbers: 8128, 2882, 8281, has three interesting properties
         * 
         * 1.The set is cyclic, in that the last two digits of each number is the first two digits 
         * of the next number (including the last number with the first)
         * 
         * 2.Each polygonal type: triangle (P3,127=8128), square (P4,91=8281), and pentagonal (P5,44=2882), 
         * is represented by a different number in the set
         * 
         * 3.This is the only set of 4-digit numbers with this property
         * 
         * Find the sum of the only ordered set of six cyclic 4-digit numbers for which each polygonal type: 
         * triangle, square, pentagonal, hexagonal, heptagonal, and octagonal, is represented by a different number in the set
         * 
         */
        public void Problem61_1()
        {
            const long limit = 10000;

            //get different number types
            var triangleNumbers =
                (from t in _lib.GetTriangleNumbers(limit)
                 where t.ToString(CultureInfo.InvariantCulture).Length == 4
                 select t).ToList();
            var squareNumbers =
                (from t in _lib.GetSquareNumbers(limit)
                 where t.ToString(CultureInfo.InvariantCulture).Length == 4
                 select t).ToList();
            var pentagonalNumbers =
                (from t in _lib.GetPentagonalNumbers(limit)
                 where t.ToString(CultureInfo.InvariantCulture).Length == 4
                 select t).ToList();
            var hexagonalNumbers =
                (from t in _lib.GetHexagonalNumbers(limit)
                 where t.ToString(CultureInfo.InvariantCulture).Length == 4
                 select t).ToList();
            var heptagonalNumbers =
                (from t in _lib.GetHeptagonalNumbers(limit)
                 where t.ToString(CultureInfo.InvariantCulture).Length == 4
                 select t).ToList();
            var octagonalNumbers =
                (from t in _lib.GetOctagonalNumbers(limit)
                 where t.ToString(CultureInfo.InvariantCulture).Length == 4
                 select t).ToList();

            //get all numbers
            var allNumbers = new List<long>();
            allNumbers.AddRange(triangleNumbers);
            allNumbers.AddRange(squareNumbers);
            allNumbers.AddRange(pentagonalNumbers);
            allNumbers.AddRange(hexagonalNumbers);
            allNumbers.AddRange(heptagonalNumbers);
            allNumbers.AddRange(octagonalNumbers);
            allNumbers.Sort();

            Console.Out.Write("AllNumbers Size: {0}, ", allNumbers.Count);
            allNumbers = allNumbers.Distinct().ToList();
            allNumbers.Sort();
            Console.Out.WriteLine("AllNumbers Distinct Size: {0}, ", allNumbers.Count);

            //all numbers as string
            var allNumbersStrings = allNumbers.Select(number => number.ToString(CultureInfo.InvariantCulture)).ToList();

            //find 6 number cycles within list
            var cycles = new List<long[]>();

            foreach (var one in allNumbers)
            {
                var candidateList2 = GetNumbersStartingWith(one.ToString(CultureInfo.InvariantCulture).Substring(2, 2),
                    allNumbersStrings);

                var oneStartsWith = one.ToString(CultureInfo.InvariantCulture).Substring(0, 2);

                foreach (var two in candidateList2)
                {
                    var candidateList3 =
                        GetNumbersStartingWith(two.ToString(CultureInfo.InvariantCulture).Substring(2, 2),
                            allNumbersStrings);

                    foreach (var three in candidateList3)
                    {
                        var candidateList4 =
                            GetNumbersStartingWith(three.ToString(CultureInfo.InvariantCulture).Substring(2, 2),
                                allNumbersStrings);

                        foreach (var four in candidateList4)
                        {
                            var candidateList5 =
                                GetNumbersStartingWith(four.ToString(CultureInfo.InvariantCulture).Substring(2, 2),
                                    allNumbersStrings);

                            foreach (var five in candidateList5)
                            {
                                var candidateList6 =
                                    GetNumbersStartingWith(five.ToString(CultureInfo.InvariantCulture).Substring(2, 2),
                                        allNumbersStrings);

                                foreach (var six in candidateList6)
                                {
                                    if (six.ToString(CultureInfo.InvariantCulture).EndsWith(oneStartsWith))
                                    {
                                        cycles.Add(new[] { one, two, three, four, five, six });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (var cycle in cycles)
            {

                //Console.Out.Write("Cycle Found: ");
                //cycle.ToList().ForEach(x => Console.Out.Write(x + " "));
                //Console.Out.WriteLine();

                //in order of the lists in the question
                var criteria = new[] { false, false, false, false, false, false };
                var headings = new[] { "Trainge", "Square", "Pentagonal", "Hexagonal", "heptagonal", "octagonal" };
                var solution = new long[] { 0, 0, 0, 0, 0, 0 };

                foreach (var criterion in cycle)
                {
                    var numberOfCriteriaNumberSatisfies = 0;

                    if (triangleNumbers.Contains(criterion))
                    {
                        numberOfCriteriaNumberSatisfies++;

                        if (criteria[0])
                        {
                            //Console.Out.WriteLine("Number: {0} -:- Criteria {1} was already met by {2}", criterion, headings[0], solution[0]);
                        }
                        else
                        {

                            criteria[0] = true;
                            solution[0] = criterion;
                        }


                    }
                    if (squareNumbers.Contains(criterion))
                    {


                        if (criteria[1])
                        {
                            //Console.Out.WriteLine("Number: {0} -:- Criteria {1} was already met by {2}", criterion, headings[1], solution[1]);
                        }
                        else
                        {

                            criteria[1] = true;
                            solution[1] = criterion;
                        }


                    }
                    if (pentagonalNumbers.Contains(criterion))
                    {
                        numberOfCriteriaNumberSatisfies++;
                        if (criteria[2])
                        {
                            //Console.Out.WriteLine("Number: {0} -:- Criteria {1} was already met by {2}", criterion, headings[2], solution[2]);
                        }
                        else
                        {

                            criteria[2] = true;
                            solution[2] = criterion;
                        }


                    }
                    if (hexagonalNumbers.Contains(criterion))
                    {
                        numberOfCriteriaNumberSatisfies++;
                        if (criteria[3])
                        {
                            //Console.Out.WriteLine("Number: {0} -:- Criteria {1} was already met by {2}", criterion, headings[3], solution[3]);
                        }
                        else
                        {

                            criteria[3] = true;
                            solution[3] = criterion;
                        }


                    }
                    if (heptagonalNumbers.Contains(criterion))
                    {
                        numberOfCriteriaNumberSatisfies++;
                        if (criteria[4])
                        {
                            //Console.Out.WriteLine("Number: {0} -:- Criteria {1} was already met by {2}", criterion, headings[4], solution[4]);
                        }
                        else
                        {

                            criteria[4] = true;
                            solution[4] = criterion;
                        }

                    }
                    if (octagonalNumbers.Contains(criterion))
                    {
                        numberOfCriteriaNumberSatisfies++;
                        if (criteria[5])
                        {
                            //Console.Out.WriteLine("Number: {0} -:- Criteria {1} was already met by {2}", criterion, headings[5], solution[5]);
                        }
                        else
                        {

                            criteria[5] = true;
                            solution[5] = criterion;
                        }

                    }

                    if (numberOfCriteriaNumberSatisfies > 1)
                    {
                        // Console.Out.WriteLine("Warning: {0} was in more than one number type", criterion);
                    }

                }

                var solutionFound = true;

                foreach (var criterion in criteria)
                {
                    if (!criterion)
                    {
                        solutionFound = false;
                        break;
                    }
                }

                if (solutionFound)
                {
                    if (solution.ToList().Distinct().Count() != 6)
                    {
                        continue;
                    }

                    Console.Out.WriteLine();
                    Console.Out.WriteLine("Solution found!");
                    cycle.ToList().ForEach(e => Console.Out.Write(e + " "));
                    Console.Out.WriteLine();
                    headings.ToList().ForEach(e => Console.Out.Write(e + " "));
                    Console.Out.WriteLine();
                    solution.ToList().ForEach(e => Console.Out.Write(e + " "));
                    Console.Out.WriteLine(", Sum is {0}", cycle.ToList().Sum());
                    Console.Out.WriteLine();
                }

            }


        }

        /**
         * The 5-digit number, 16807=7^5, is also a fifth power. 
         * 
         * Similarly, the 9-digit number, 134217728=8^9, is a ninth power.
         * 
         * How many n-digit positive integers exist which are also an nth power?
         * 
         */
        public void Problem63()
        {
            var result = 0;

            for (var p = 1; p < 100; p++)
            {
                for (var b = new BigInteger(1); b < 100; b++)
                {
                    var x = _lib.ToThePower(b, p);

                    if (x.ToString().Length != p) continue;
                    Console.Out.WriteLine("Power found: {0} ^ {1} = {2}", b, p, x);
                    result++;
                }
            }

            Console.Out.WriteLine("Problem 63 result: {0}", result);
        }

        // count square roots with odd number of recurring partial fractions
        public void Problem64()
        {
            var squares = _lib.GetSquareNumbers(10000);

            var count = 0;
            var skipped = 0;
            var total = 0;

            for (var i = 1; i <= 10000; i++)
            {

                total++;
                if (squares.Contains(i))
                {
                    skipped++;
                    continue;

                }

                if ((_lib.GeneratePartialFractionListFromSquareRoot(i, false).Count - 1) % 2 != 0)
                {
                    count++;
                }

            }

            //answer is 1322

            Console.Out.WriteLine("Problem 64 Answer: {0}. Total Numbers: {1}, Skipped {2}", count, total, skipped);
        }

        /**
         * The square root of 2 can be written as an infinite continued fraction
         * 
         * The infinite continued fraction can be written, √2 = [1;(2)], (2) indicates that 2 repeats ad infinitum. 
         * In a similar way, √23 = [4;(1,3,1,8)].
         * It turns out that the sequence of partial values of continued fractions for square roots provide the best rational approximations. 
         * Let us consider the convergents for √2.
         * Hence the sequence of the first ten convergents for √2 are:
         * 1, 3/2, 7/5, 17/12, 41/29, 99/70, 239/169, 577/408, 1393/985, 3363/2378, ...
         * 
         * What is most surprising is that the important mathematical constant,
         * e = [2; 1,2,1, 1,4,1, 1,6,1 , ... , 1,2k,1, ...].
         * 
         * The first ten terms in the sequence of convergents for e are:
         * 2, 3, 8/3, 11/4, 19/7, 87/32, 106/39, 193/71, 1264/465, 1457/536, ...
         * 
         * The sum of digits in the numerator of the 10th convergent is 1+4+5+7=17.
         * 
         * Find the sum of digits in the numerator of the 100th convergent of the continued fraction for e.
         * 
         */
        public void Problem65()
        {

            const int limit = 100;

            var partials = _lib.GetPartialQuotientsOfE(limit);

            var fractions = new List<Fraction>();

            var f1 = new Fraction(partials[0], 1);

            fractions.Add(f1);

            //remove non-repeating start
            partials.RemoveAt(0);

            for (var i = 0; i <= limit; i++)
            {
                fractions.Add(f1.AddFraction(_lib.GenerateContinuousFraction(i, 0, partials)));
            }

            Console.Out.WriteLine("Euler 65 answer (272): " + fractions[limit - 1].GetNumerator().ToString().ToCharArray().ToList().Sum(e => int.Parse(e.ToString(CultureInfo.InvariantCulture))));

        }

        /**
         * Consider quadratic Diophantine equations of the form:
         * 
         * x^2 – Dy^2 = 1
         * 
         * For example, when D=13, the minimal solution in x is 649^2 – 13×180^2 = 1.
         * 
         * It can be assumed that there are no solutions in positive integers when D is square.
         * 
         * By finding minimal solutions in x for D = {2, 3, 5, 6, 7}, we obtain the following:
         * 
         * 3^2 – 2×2^2 = 1
         * 2^2 – 3×1^2 = 1
         * 9^2 – 5×4^2 = 1
         * 5^2 – 6×2^2 = 1 
         * 8^2 – 7×3^2 = 1
         * 
         * Hence, by considering minimal solutions in x for D ≤ 7, the largest x is obtained when D=5
         * 
         * Find the value of D ≤ 1000 in minimal solutions of x for which the largest value of x is obtained
         * 
         */
        public void Problem66()
        {
            const int limit = 100;

            var squares = _lib.GetSetOfSquaresFromFile();

            var max = BigInteger.One;

            var valueD = 0;

            for (var i = 2; i <= 1000; i++)
            {
                if (squares.Contains((ulong)i)) continue;

                var partials = _lib.GeneratePartialFractionListFromSquareRoot(i, false);

                var f1 = new Fraction(partials[0], 1);

                var fractions = new List<Fraction> { f1 };

                partials.RemoveAt(0);

                var fullPartials = new List<long>();

                for (var x = 0; x < limit; x++)
                {
                    fullPartials.AddRange(partials);
                }

                for (var j = 0; j < limit; j++)
                {
                    fractions.Add(f1.AddFraction(_lib.GenerateContinuousFraction(j, 0, fullPartials)));
                }

                var dee = new BigInteger(i);

                var found = false;

                foreach (var f in fractions)
                {
                    var d = (f.GetNumerator() * f.GetNumerator())
                    - (dee *
                            (f.GetDenominator() * f.GetDenominator())
                      );

                    if (!d.IsOne) continue;

                    found = true;
                    if (f.GetNumerator() > max)
                    {
                        max = f.GetNumerator();
                        valueD = i;
                    }
                    break;
                }

                if (!found)
                {
                    Console.Out.WriteLine("Warning :: No Solution found for : {0}", i);
                }
            }

            Console.Out.WriteLine("Max value of x is: {0} when D = {1}", max, valueD);
        }

        /**
         * 
         * Let x be a real number.
         *
         * A best approximation to x for the denominator bound d is a rational number r/s in reduced form, 
         * with s ≤ d, such that any rational number which is closer to x than r/s has a denominator larger than d:
         * 
         * |p/q-x| < |r/s-x| ⇒ q > d
         * 
         * For example, the best approximation to √13 for the denominator bound 20 is 18/5 
         * and the best approximation to √13 for the denominator bound 30 is 101/28.
         *
         * Find the sum of all denominators of the best approximations to √n for the denominator bound 10^12, 
         * where n is not a perfect square and 1 < n ≤ 100000.
         */
        public void Problem192()
        {
            const int limit = 1000000;
            const int nLimit = 20;

            //var denominatorLimit = _lib.ToThePower(new BigInteger(10), 12);
            var denominatorLimit = new BigInteger(30);

            var squares = _lib.GetSquareNumbers(100000);

            var answer = BigInteger.Zero;

            for (var n = 13; n <= nLimit; n++)
            {
                //skip perfect squares
                if (squares.Contains(n)) continue;

                //get the sqaure root of n
                var squareRootN = (decimal)Math.Sqrt(n);

                //create partials
                var partials = _lib.GeneratePartialFractionListFromSquareRoot(n, false);

                //collect fractions
                var fractions = new List<Fraction>();

                //create starting fraction
                var f1 = new Fraction(partials[0], 1);

                //add the first fraction to the list of fractions
                fractions.Add(f1);

                //start with the difference of f1 - sqrt(n)
                var epsilon = Math.Abs(f1.GetDecimalValue() - squareRootN);

                //default the nearest approximation to f1
                var finalFraction = f1;

                //remove the first value from the partials
                partials.RemoveAt(0);

                //make a new list for the partials up to a limit
                var fullPartials = new List<long>();

                //fill the list up to the limit
                for (var x = 0; x < limit; x++)
                {
                    fullPartials.AddRange(partials);
                }

                //find the convergent fractions from the continuous fractions
                for (var j = 0; j < limit; j++)
                {
                    if (j == limit - 1)
                    {
                        Console.Out.WriteLine("Warning : Convergent Fractions have reached the limit for {0}", n);
                    }

                    //generate continuous fraction
                    var f = f1.AddFraction(_lib.GenerateContinuousFraction(j, 0, fullPartials));

                    //make sure the denominator is valid
                    if (f.GetDenominator() > denominatorLimit)
                    {
                        break;
                    }

                    var startFraction = fractions.Last();

                    //add the CF to the list
                    fractions.Add(f);

                    if (Math.Abs(f.GetDecimalValue() - squareRootN) < epsilon)
                    {
                        finalFraction = f;
                        epsilon = Math.Abs(f.GetDecimalValue() - squareRootN);
                    }

                    if (Math.Abs(f.GetDecimalValue() - squareRootN).Equals(epsilon) &&
                        f.GetDenominator() > finalFraction.GetDenominator())
                    {
                        finalFraction = f;
                    }

                    var semiConvergents = GetSemiConvergents(startFraction, f, limit, denominatorLimit);

                    Console.Out.WriteLine("Semiconvergents length: {0}", semiConvergents.Count);
                    semiConvergents.ForEach(s => Console.Out.Write(s + " "));

                    foreach (var semiConvergent in semiConvergents)
                    {
                        if (Math.Abs(semiConvergent.GetDecimalValue() - squareRootN) < epsilon)
                        {
                            finalFraction = semiConvergent;
                            epsilon = Math.Abs(semiConvergent.GetDecimalValue() - squareRootN);
                        }
                        else if (Math.Abs(semiConvergent.GetDecimalValue() - squareRootN).Equals(epsilon) &&
                          semiConvergent.GetDenominator() > finalFraction.GetDenominator())
                        {
                            finalFraction = f;
                        }
                    }


                } //end the continuous fractions search here

                Console.Out.WriteLine("Fraction for {0} is {1}", n, finalFraction);

                answer = answer + finalFraction.GetDenominator();
            }

            Console.Out.WriteLine("Answer is {0}", answer);

        }

        /** find semiconvergents of the continuous fractions
         * any fraction of the form
         *
         * h(n−1) + a * h(n)
         * -------------------
         * k(n−1) + a * k(n)
         *
         * where a is a nonnegative integer and the numerators and denominators are between 
         * the n and n + 1 terms inclusive are called semiconvergents
         */
        public List<Fraction> GetSemiConvergents(Fraction start, Fraction finish, int multiplierLimit, BigInteger denominatorLimit)
        {
            var semiConvergents = new List<Fraction>();

            for (var a = 1; a < multiplierLimit; a++)
            {
                if (a == multiplierLimit - 1)
                {
                    Console.Out.WriteLine("Warning : semiconvergent Fractions have reached the limit.");
                }

                var semi = new Fraction(start.GetNumerator() + finish.GetNumerator() * a,
                    start.GetDenominator() + finish.GetDenominator() * a);

                if (semi.GetDenominator() > denominatorLimit)
                {
                    break;
                }

                semiConvergents.Add(semi);



            }

            return semiConvergents;
        }

        /**
         * Euler's Totient function, φ(n) [sometimes called the phi function], 
         * is used to determine the number of numbers less than n which are relatively prime to n. 
         * 
         * n	Relatively Prime	φ(n)	n/φ(n)
         * 2	1					1		2
         * 3	1,2					2		1.5
         * 4	1,3					2		2
         * 5	1,2,3,4				4		1.25
         * 6	1,5					2		3
         * 7	1,2,3,4,5,6			6		1.1666
         * 8	1,3,5,7				4		2
         * 9	1,2,4,5,7,8			6		1.5
         * 10	1,3,7,9				4		2.5
         * 
         * For example, as 1, 2, 4, 5, 7, and 8, are all less than nine and relatively prime to nine, φ(9)=6.
         * 
         * It can be seen that n=6 produces a maximum n/φ(n) for n ≤ 10.
         * 
         * Find the value of n ≤ 1,000,000 for which n/φ(n) is a maximum.
         * 
         * 510510
         * 
         */
        public void Problem69()
        {
            var primes = _lib.GetSetOfPrimesFromFile();
            var primesList = primes.ToList();
            primesList.Sort();

            var maxRatio = 0.5;
            var maxN = 0;

            for (var n = 2; n <= 1000000; n++)
            {
                var phi = _lib.PhiOfN(n, primes, primesList);

                if (!((double)n / phi > maxRatio)) continue;

                maxRatio = n / (double)phi;
                maxN = n;
            }

            Console.Out.WriteLine("Max N: {0}", maxN);
        }

        //8319823
        public void Problem70()
        {
            var primes = _lib.GetSetOfPrimesFromFile();
            var primesList = primes.ToList();
            primesList.Sort();

            var minRatio = 100.0;
            var minN = 0;

            var count = 0;

            for (var n = 2; n <= 10000000; n++)
            {
                if (primes.Contains(n)) continue;

                var phi = _lib.PhiOfN(n, primes, primesList);

                if (!_lib.IsNumberPermutation(n, phi)) continue;

                Console.Out.WriteLine("n={0}, phi(n)={1}, n/phi(n)={2}", n, phi, n / (double)phi);
                count++;

                if (!((double)n / phi < minRatio)) continue;

                minRatio = n / (double)phi;
                minN = n;
            }

            Console.Out.WriteLine("Min N: {0}, number found: {1}", minN, count);
        }

        /**
         * Consider the fraction, n/d, where n and d are positive integers. 
         * If n<d and HCF(n,d)=1, it is called a reduced proper fraction
         * 
         * If we list the set of reduced proper fractions for d ≤ 8 in ascending order of size, we get
         * 
         * 1/8, 1/7, 1/6, 1/5, 1/4, 2/7, 1/3, 3/8, 2/5, 3/7, 1/2, 4/7, 3/5, 5/8, 2/3, 5/7, 3/4, 4/5, 5/6, 6/7, 7/8
         * 
         * It can be seen that 2/5 is the fraction immediately to the left of 3/7
         * By listing the set of reduced proper fractions for d ≤ 1,000,000 in ascending order of size, 
         * find the numerator of the fraction immediately to the left of 3/7
         * 
         * 428570
         * 
         */
        public void Problem71()
        {

            var target = new Fraction(3, 7);
            Fraction t = null;

            var fc = new Fraction.FractionComparator();

            for (var denominator = new BigInteger(1000000); denominator > BigInteger.Zero; denominator--)
            {
                for (var numerator = new BigInteger(1); numerator < denominator; numerator++)
                {
                    if (!BigInteger.GreatestCommonDivisor(numerator, denominator).Equals(BigInteger.One)) continue;

                    var f = new Fraction(numerator, denominator);

                    //smaller than 3/5 ?
                    if (fc.Compare(f, target) == -1)
                    {
                        //"bigger" or closer to 3/5 than previous?
                        if (null == t)
                        {
                            t = f;
                        }
                        else if (fc.Compare(t, f) == -1)
                        {
                            t = f;
                        }
                    }
                    else
                    {
                        //bigger than 3/5 and will only continue to be so
                        break;
                    }
                }
            }

            Console.Out.WriteLine(t);

        }


        /**
         * Consider the fraction, n/d, where n and d are positive integers. 
         * If n<d and HCF(n,d)=1, it is called a reduced proper fraction
         * 
         * If we list the set of reduced proper fractions for d ≤ 8 in ascending order of size, we get
         * 1/8, 1/7, 1/6, 1/5, 1/4, 2/7, 1/3, 3/8, 2/5, 3/7, 1/2, 4/7, 3/5, 5/8, 2/3, 5/7, 3/4, 4/5, 5/6, 6/7, 7/8
         * 
         * It can be seen that there are 21 elements in this set
         * 
         * How many elements would be contained in the set of reduced proper fractions for d ≤ 1,000,000
         * 
         * Wrong : 3039650753
         * 
         * 303963552391
         * 
         */
        public void Problem72()
        {
            UInt64 phiCount = 0;

            var primeSet = _lib.GetSetOfPrimesFromFile();
            var primeList = primeSet.ToList();
            primeList.Sort();

            for (var denominator = 2; denominator <= 1000000; denominator++)
            {
                var phi = _lib.PhiOfN(denominator, primeSet, primeList);

                phiCount += (UInt64)phi;
            }

            Console.Out.WriteLine("Problem 72 Answer: {0}", phiCount);
        }


        /**
         * Consider the fraction, n/d, where n and d are positive integers. 
         * If n<d and HCF(n,d)=1, it is called a reduced proper fraction.
         * 
         * If we list the set of reduced proper fractions for d ≤ 8 in ascending order of size, we get:
         * 
         * 1/8, 1/7, 1/6, 1/5, 1/4, 2/7, 1/3, 3/8, 2/5, 3/7, 1/2, 4/7, 3/5, 5/8, 2/3, 5/7, 3/4, 4/5, 5/6, 6/7, 7/8
         * 
         * It can be seen that there are 3 fractions between 1/3 and 1/2.
         * 
         * How many fractions lie between 1/3 and 1/2 in the sorted set of reduced proper fractions for d ≤ 12,000
         * 
         */
        public void Problem73()
        {

            var greaterThan = new Fraction(1, 3);
            var lessThan = new Fraction(1, 2);

            var fractionsCount = 0;

            var fc = new Fraction.FractionComparator();

            for (var denominator = new BigInteger(12000); denominator > BigInteger.Zero; denominator--)
            {
                for (var numerator = new BigInteger(1); numerator < denominator; numerator++)
                {
                    if (!BigInteger.GreatestCommonDivisor(numerator, denominator).Equals(BigInteger.One)) continue;

                    var f = new Fraction(numerator, denominator);

                    //greater than 1/3 and less than 1/2 ?
                    if ((fc.Compare(f, lessThan) == -1) && (fc.Compare(f, greaterThan) == 1))
                    {
                        fractionsCount++;
                    }
                }
            }

            Console.Out.WriteLine(fractionsCount);

        }

        /**
         * 
         * It turns out that 12 cm is the smallest length of wire that can be bent 
         * to form an integer sided right angle triangle in exactly one way, but there are many more examples.
         * 
         * 12 cm: (3,4,5)
         * 24 cm: (6,8,10)
         * 30 cm: (5,12,13)
         * 36 cm: (9,12,15) 
         * 40 cm: (8,15,17)
         * 48 cm: (12,16,20)
         * 
         * In contrast, some lengths of wire, like 20 cm, cannot be bent to form an integer 
         * sided right angle triangle, and other lengths allow more than one solution to be found; 
         * for example, using 120 cm it is possible to form exactly three different integer sided right angle triangles.
         * 
         * 120 cm: (30,40,50), (20,48,52), (24,45,51)
         * 
         * Given that L is the length of the wire, for how many values of 
         * L ≤ 1,500,000 can exactly one integer sided right angle triangle be formed?
         * 
         */
        public void Problem75()
        {

            const UInt64 limit = 1500000;

            var resultsMap = new Dictionary<UInt64, ISet<PythagoreanTriple>>();

            for (UInt64 i = 12; i <= limit; i++)
            {
                if (i % 2 == 0)
                {
                    resultsMap.Add(i, new HashSet<PythagoreanTriple>());
                }
            }

            for (UInt64 n = 1; n < Math.Sqrt(limit); n++)
            {
                for (UInt64 m = n + 1; m < Math.Sqrt(limit); m++)
                {
                    UInt64 a = (m * m) - (n * n);
                    UInt64 b = 2 * m * n;
                    UInt64 c = (m * m) + (n * n);

                    var p = new PythagoreanTriple(a, b, c);

                    var x = p.Sum();

                    if (x > limit)
                    {
                        break;
                    }

                    if (p.CheckTripple())
                    {
                        resultsMap[x].Add(p);
                    }

                    for (UInt64 k = 2; k < limit; k++)
                    {
                        var kp = p.GenerateFromK(k);

                        var kx = kp.Sum();

                        if (kx > limit)
                        {
                            break;
                        }
                        if (kp.CheckTripple())
                        {
                            resultsMap[kx].Add(kp);
                        }
                    }
                }
            }

            var result = (from t in resultsMap where t.Value.Count == 1 select t).ToList();

            //161667
            Console.Out.WriteLine("Size of target list: {0}, Result = {1}", resultsMap.Count, result.Count);

        }

        class PythagoreanTriple : IEquatable<PythagoreanTriple>
        {
            public bool Equals(PythagoreanTriple other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return _a == other._a && _b == other._b && _c == other._c;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((PythagoreanTriple)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = _a.GetHashCode();
                    hashCode = (hashCode * 397) ^ _b.GetHashCode();
                    hashCode = (hashCode * 397) ^ _c.GetHashCode();
                    return hashCode;
                }
            }

            private readonly UInt64 _a;
            private readonly UInt64 _b;
            private readonly UInt64 _c;

            public PythagoreanTriple(UInt64 a, UInt64 b, UInt64 c)
            {
                _a = a > b ? b : a;
                _b = b < a ? a : b;
                _c = c;
            }

            public UInt64 Sum()
            {
                return (_a + _b + _c);
            }

            public bool CheckTripple()
            {
                return (_a * _a) + (_b * _b) == (_c * _c);
            }

            public PythagoreanTriple GenerateFromK(UInt64 k)
            {
                return new PythagoreanTriple(k * _a, k * _b, k * _c);
            }

            public override string ToString()
            {
                return "(" + _a + "," + _b + "," + _c + ")";
            }

        }
    }
}