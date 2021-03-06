﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Problems
    {

        private readonly Library _lib;

        public Problems(Library lib)
        {
            _lib = lib;
            Console.Out.WriteLine("Euler Problems 1 - 20.");
        }


        public void Problem3()
        {

            var primes = _lib.GetSetOfLongPrimesFromFile();

            //todo factorise into long
            var factors = _lib.BigFactorise(600851475143);

            var longFactors = new List<long>();

            factors.ForEach(e => longFactors.Add((long)e));

            foreach (var factror in longFactors)
            {
                if (primes.Contains(factror))
                {
                    Console.Out.WriteLine(factror);
                }
            }

        }

        /**
         * Problem 5
         * 
         * 2520 is the smallest number that can be divided by each of the numbers from 1 to 10 without any remainder.
         *
         * What is the smallest positive number that is evenly divisible by all of the numbers from 1 to 20?
         */
        public void Problem5()
        {
            Console.Out.WriteLine("Euler Problem 5 (10) : " + _lib.CalculateSmallestMultiple(10));
            Console.Out.WriteLine("Euler Problem 5 (20) : " + _lib.CalculateSmallestMultiple(20));
        }


        /**
         * Problem 6
         * The sum of the squares of the first ten natural numbers is,
         * 
         *  12 + 22 + ... + 102 = 385
         *
         * The square of the sum of the first ten natural numbers is,
         *
         *   (1 + 2 + ... + 10)2 = 552 = 3025
         *
         * Hence the difference between the sum of the squares of the first 
         * ten natural numbers and the square of the sum is 3025 − 385 = 2640.
         *
         * Find the difference between the sum of the squares of the first 
         * one hundred natural numbers and the square of the sum.
         *
         **/
        public void Problem6()
        {
            Console.Out.WriteLine("Euler Problem 6 (10)  : " + (_lib.GetSquareOfSum(10) - _lib.GetSumOfNaturalNumbersSquares(10)));
            Console.Out.WriteLine("Euler Problem 6 (100) : " + (_lib.GetSquareOfSum(100) - _lib.GetSumOfNaturalNumbersSquares(100)));
        }

        /**
         * Problem 7
         * 
         * By listing the first six prime numbers: 2, 3, 5, 7, 11, and 13, we can see that the 6th prime is 13.
         *
         * What is the 10 001st prime number
         * 
        */
        public void Problem7()
        {
            Console.Out.WriteLine("Euler Problem 7 (10)   : " + _lib.GetNthPrime(6));
            Console.Out.WriteLine("Euler Problem 7 (1001) : " + _lib.GetNthPrime(10001));
        }

        /**
        * Problem 8 
        * The four adjacent digits in the 1000-digit number that have the greatest product are 9 × 9 × 8 × 9 = 5832.
        *
        * Find the thirteen adjacent digits in the 1000-digit number that have the greatest product.
        * 
        * What is the value of this product?
        */
        public void Problem8()
        {
            const string largeNumber = "73167176531330624919225119674426574742355349194934"
                                       + "96983520312774506326239578318016984801869478851843"
                                       + "85861560789112949495459501737958331952853208805511"
                                       + "12540698747158523863050715693290963295227443043557"
                                       + "66896648950445244523161731856403098711121722383113"
                                       + "62229893423380308135336276614282806444486645238749"
                                       + "30358907296290491560440772390713810515859307960866"
                                       + "70172427121883998797908792274921901699720888093776"
                                       + "65727333001053367881220235421809751254540594752243"
                                       + "52584907711670556013604839586446706324415722155397"
                                       + "53697817977846174064955149290862569321978468622482"
                                       + "83972241375657056057490261407972968652414535100474"
                                       + "82166370484403199890008895243450658541227588666881"
                                       + "16427171479924442928230863465674813919123162824586"
                                       + "17866458359124566529476545682848912883142607690042"
                                       + "24219022671055626321111109370544217506941658960408"
                                       + "07198403850962455444362981230987879927244284909188"
                                       + "84580156166097919133875499200524063689912560717606"
                                       + "05886116467109405077541002256983155200055935729725"
                                       + "71636269561882670428252483600823257530420752963450";

            Console.Out.WriteLine("Euler Problem 8 (4)  : " + _lib.GetGreatestProductFromNConsecutiveNumbersInStringOfNumbers(largeNumber, 4));

            Console.Out.WriteLine("Euler Problem 8 (13) : " + _lib.GetGreatestProductFromNConsecutiveNumbersInStringOfNumbers(largeNumber, 13));
        }

        public void Problem10()
        {

            Console.Out.WriteLine(Enumerable.Range(2, 1999997).AsParallel().Where(delegate(int x)
            {
                for (var i = 2; i < (int)Math.Sqrt(x) + 1; i++)
                {
                    if (x % i == 0) return false;
                }
                return true;
            }).Sum(e => (long)e));

            Console.Out.WriteLine(142913828922);
        }

        /**
        * 
        * Problem 18 and problem 67
        * 
        *  By starting at the top of the triangle below and moving to adjacent numbers on the row below, 
        *  the maximum total from top to bottom is 23.
        * 
        *                  3
        *                 7 4
        *                2 4 6
        *               8 5 9 3
        *
        *  That is, 3 + 7 + 4 + 9 = 23.
        *
        *  Find the maximum total from top to bottom of the triangle below:
        *
        *                 75
        *                95 64
        *               17 47 82
        *              18 35 87 10
        *             20 04 82 47 65
        *            19 01 23 75 03 34
        *           88 02 77 73 07 63 67
        *          99 65 04 28 06 16 70 92
        *         41 41 26 56 83 40 80 70 33
        *        41 48 72 33 47 32 37 16 94 29
        *       53 71 44 65 25 43 91 52 97 51 14
        *      70 11 33 28 77 73 17 78 39 68 17 57
        *     91 71 52 38 17 14 91 43 58 50 27 29 48
        *    63 66 04 68 89 53 67 30 73 16 69 87 40 31
        *   04 62 98 27 23 09 70 98 73 93 38 53 60 04 23
        * 
        * NOTE: As there are only 16384 routes, it is possible to solve this problem by trying every route. 
        * However, Problem 67, is the same challenge with a triangle containing one-hundred rows; 
        * it cannot be solved by brute force, and requires a clever method! ;o)
        * 
        * this is 2^14 (which is 2^n-1) where n is number of rows 
        * 
        * for 100 rows this is 2^99? which is massive:
        * 633825300114114700748351602688
        * 
        * path will always be n elements where n is number of rows (n-1 moves)
        * 
       */
        public void Problem18()
        {
            var littleGrid = new List<int[]>
            {
                new [] {3},
                new [] {7, 4},
                new [] {2, 4, 6},
                new [] {8, 5, 9, 3}
            };

            var grid = new List<int[]>
            {
                new [] {75},
                new [] {95, 64},
                new [] {17, 47, 82},
                new [] {18, 35, 87, 10},
                new [] {20, 04, 82, 47, 65},
                new [] {19, 01, 23, 75, 03, 34},
                new [] {88, 02, 77, 73, 07, 63, 67},
                new [] {99, 65, 04, 28, 06, 16, 70, 92},
                new [] {41, 41, 26, 56, 83, 40, 80, 70, 33},
                new [] {41, 48, 72, 33, 47, 32, 37, 16, 94, 29},
                new [] {53, 71, 44, 65, 25, 43, 91, 52, 97, 51, 14},
                new [] {70, 11, 33, 28, 77, 73, 17, 78, 39, 68, 17, 57},
                new [] {91, 71, 52, 38, 17, 14, 91, 43, 58, 50, 27, 29, 48},
                new [] {63, 66, 04, 68, 89, 53, 67, 30, 73, 16, 69, 87, 40, 31},
                new [] {04, 62, 98, 27, 23, 09, 70, 98, 73, 93, 38, 53, 60, 04, 23}
            };

            var bigGrid = File.ReadAllLines(@"c:\LocalFolder\p067_triangle.txt")
                .Select(line => line.Split(' ').Select(int.Parse).ToArray()).ToList();

            Console.Out.WriteLine("Euler Problem 18      (Little Grid) : " + _lib.GetMaxSumPathOfTriangle(littleGrid));
            Console.Out.WriteLine("Euler Problem 18      (Medium Grid) : " + _lib.GetMaxSumPathOfTriangle(grid));
            Console.Out.WriteLine("Euler Problem 18 & 67 (Big Grid)    : " + _lib.GetMaxSumPathOfTriangle(bigGrid));
        }

    }
}
