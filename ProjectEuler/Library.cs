using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;

namespace ConsoleApplication1
{
    public class Library
    {

        public int CalculateSmallestMultiple(int limit)
        {
            //use the limit to calculate some possible large numbers
            for (var i = limit; i < int.MaxValue - 1; i += limit)
            {
                for (var j = limit; j >= 1; j--)
                {
                    if (j == 1)
                    {
                        //then we have found the answer
                        return i;
                    }

                    if (i % j != 0)
                    {
                        break;
                    }
                }
            }

            return 0;
        }

        public Int64 GetSumOfNaturalNumbersSquares(int limit)
        {
            Int64 result = 0;

            for (var i = 1; i <= limit; i++)
            {
                result += (Int64)Math.Pow(i, 2);
            }

            return result;
        }

        public Int64 GetSquareOfSum(int limit)
        {
            Int64 sum = 0;

            for (var i = 1; i <= limit; i++)
            {
                sum += i;
            }

            return (Int64)Math.Pow(sum, 2);
        }

        public long GetMaxSumPathOfTriangle(List<int[]> grid)
        {
            //go from the end of the grid "up", starting at the penultimate line
            for (var i = grid.Count - 2; i >= 0; i--)
            {
                //go through each number in the line 
                for (var j = 0; j < grid[i].Length; j++)
                {
                    var left = grid[i][j] + grid[i + 1][j];
                    var right = grid[i][j] + grid[i + 1][j + 1];
                    grid[i][j] = left > right ? left : right;
                }
            }

            return grid[0][0];
        }

        public UInt64 GetSumOfPathThroughTriangleGuessMethod(List<int[]> grid)
        {
            var r = new Random();

            var guesses = (int)Math.Pow(2, grid.Count - 1);

            UInt64 maxSum = 0;

            var paths = new HashSet<string>();

            while (paths.Count < guesses)
            {
                var currentPath = "";
                var index = 0;
                UInt64 sum = 0;

                //enter the guess loop
                for (var i = 0; i < grid.Count; i++)
                {
                    //start at the beginning which has only one possible value
                    sum += (UInt64)grid[i][index];

                    //if this is the last one then don't guess
                    if (i + 1 != grid.Count)
                    {
                        //guess where to go next
                        var nextRandom = r.Next(0, 11) > 5 ? 1 : 0;

                        //add this to the string of guesses
                        if (nextRandom == 0)
                        {
                            currentPath += "L";
                        }
                        else
                        {
                            currentPath += "R";
                        }

                        //add to the index
                        index += nextRandom;
                    }
                }

                //is this the max sum
                if (sum > maxSum)
                {
                    maxSum = sum;
                }

                //add to guess representation
                paths.Add(currentPath);
            }

            return maxSum;
        }

        public ISet<string> RandomWalkMatrix(int[][] matrix)
        {
            var results = new HashSet<string>();

            var r = new Random();

            var guesses = Math.Pow(matrix.Length * matrix.Length, 3);

            var maxLength = matrix.Length * 2;

            //enter the guess loop
            for (var i = 0; i < guesses; i++)
            {
                var currentPath = "";
                int x = 0, y = 0, length = 0;

                while (length < maxLength)
                {
                    //check which moves are valid
                    var downMove = x >= matrix.Length - 1 ? -1 : matrix[x + 1][y];
                    var rightMove = y >= matrix[0].Length - 1 ? -1 : matrix[x][y + 1];

                    if (downMove == -1 && rightMove == -1) break;

                    //guess where to go next
                    var nextRandom = r.Next(0, 11) > 5 ? 1 : 0;

                    //based on that decide to go right or down
                    if (nextRandom == 0)
                    {
                        if (rightMove != -1)
                        {
                            currentPath += "R";
                            y++;
                            length++;
                        }
                    }
                    else
                    {
                        if (downMove != -1)
                        {
                            currentPath += "D";
                            x++;
                            length++;
                        }
                    }

                }

                results.Add(currentPath);
            }

            return results;
        }

        public int[][] ReadBigMatrixFile(string fileName)
        {
            var matrixFile = File.ReadAllLines(fileName).ToList();

            var bigMatrix = new int[matrixFile.Count][];

            for (var i = 0; i < matrixFile.Count; i++)
            {
                var parse = matrixFile[i].Split(',');

                var row = new int[parse.Length];

                for (var j = 0; j < parse.Length; j++)
                {
                    row[j] = int.Parse(parse[j]);
                }

                bigMatrix[i] = row;
            }

            return bigMatrix;
        }

        /**
         * by starting in any cell in the left column 
         * and finishing in any cell in the right column, and only moving up, down, and right
         * 
         */
        public int GetMinSumThroughMatrixUpDownAndRight(int[][] matrix)
        {

            for (var i = matrix.Length - 2; i >= 0; i--)
            {
                //calculate last column
                matrix[i][matrix.Length - 1] += matrix[i + 1][matrix.Length - 1];
                //calculate last row
                matrix[matrix.Length - 1][i] += matrix[matrix.Length - 1][i + 1];
                //calculate down
            }

            //go through each cell in the matrix
            for (var i = matrix.Length - 2; i >= 0; i--)
            {
                for (var j = matrix.Length - 2; j >= 0; j--)
                {
                    //decide which is smaller (down or left) and add that value
                    matrix[i][j] += matrix[i + 1][j] < matrix[i][j + 1] ? matrix[i + 1][j] : matrix[i][j + 1];
                }
            }

            return matrix[0][0];

        }

        public int GetMinSumThroughMatrixRightAndDownOnly(int[][] matrix)
        {

            for (var i = matrix.Length - 2; i >= 0; i--)
            {
                //calculate last column
                matrix[i][matrix.Length - 1] += matrix[i + 1][matrix.Length - 1];
                //calculate last row
                matrix[matrix.Length - 1][i] += matrix[matrix.Length - 1][i + 1];
            }

            //go through each cell in the matrix
            for (var i = matrix.Length - 2; i >= 0; i--)
            {
                for (var j = matrix.Length - 2; j >= 0; j--)
                {
                    //decide which is smaller (down or left) and add that value
                    matrix[i][j] += matrix[i + 1][j] < matrix[i][j + 1] ? matrix[i + 1][j] : matrix[i][j + 1];
                }
            }

            return matrix[0][0];
        }

        public static void PrintMatrix(int[][] matrix)
        {
            for (var i = 0; i < matrix.Length; i++)
            {
                for (var j = 0; j < matrix.Length; j++)
                {
                    Console.Out.Write(matrix[i][j] + " ");
                }
                Console.Out.WriteLine();
            }

            Console.Out.WriteLine();
        }

        //two ways only
        public BigInteger GetNumberOfPathsThroughMatrix(int matrixSize)
        {
            //(2R!)/(R! ^ 2)

            var r = new BigInteger(matrixSize);

            var rFact = Factorial(r);

            var twoRFact = Factorial(r * 2);

            var rFactSq = rFact * rFact;

            var answer = twoRFact / rFactSq;

            return answer;
        }

        public long GetNumberOfPathsRightAndDownOnlyThroughGrid(int sizeOfSquare)
        {
            var size = sizeOfSquare + 1;

            var grid = new long[size, size];

            for (var n = 0; n < size; n++)
            {
                grid[n, 0] = grid[0, n] = 1;
            }

            for (var i = 1; i < size; i++)
            {
                for (var j = 1; j < size; j++)
                {
                    grid[i, j] = grid[i - 1, j] + grid[i, j - 1];
                }
            }

            return grid[size - 1, size - 1];
        }

        public UInt64 GetGreatestProductFromNConsecutiveNumbersInStringOfNumbers(String largeNumberAsString,
            int consecutiveNumbers)
        {
            //initialise 
            UInt64 maxProduct = 0;

            //start at beginning of string
            for (var i = 0; i < largeNumberAsString.Length; i++)
            {
                //reset this
                UInt64 currentProduct = 1;

                //check that we can have the next n entries
                if (i + consecutiveNumbers - 1 >= largeNumberAsString.Length)
                {
                    //end the for loop here
                    break;
                }

                //start at sub index i of string and get the next n 
                for (var j = i; j < i + consecutiveNumbers; j++)
                {
                    currentProduct *= UInt64.Parse(largeNumberAsString.Substring(j, 1));
                }

                //set maxProduct
                if (currentProduct > maxProduct)
                {
                    maxProduct = currentProduct;
                }
            }

            return maxProduct;
        }

        public UInt64 GetNthPrime(UInt64 n)
        {
            UInt64 count = 0;
            for (UInt64 i = 2; i < UInt64.MaxValue; i++)
            {
                if (IsPrime(i))
                {
                    count++;
                    if (count.Equals(n))
                    {
                        return i;
                    }
                }
            }

            return 0;
        }

        public bool IsPrime(UInt64 number)
        {
            var isPrime = true;

            for (UInt64 i = 2; i < Math.Sqrt(number) + 1; i++)
            {
                if (number % i == 0)
                {
                    //i divides this number evenly
                    isPrime = false;
                    break;
                }
            }
            return isPrime;
        }

        public ISet<int> GetSetOfPrimesFromFile()
        {

            Console.Out.WriteLine("Reading Prime Numbers from file...");
            var s = Stopwatch.StartNew();

            var primes = File.ReadAllLines(@"c:\LocalFolder\dont\primes1.txt")
                .Select(line => line.Split(',').Select(int.Parse).ToArray()).ToList();

            var primesSet = new HashSet<int>();

            foreach (var i in primes.SelectMany(primeArray => primeArray))
            {
                primesSet.Add(i);
            }

            s.Stop();
            Console.Out.WriteLine("Reading {0} Prime numbers took {1}, max Prime is: {2}\r\n", primesSet.Count,
                s.Elapsed, primesSet.Max());


            return primesSet;
        }

        public ISet<long> GetSetOfLongPrimesFromFile()
        {

            Console.Out.WriteLine("Reading Prime Numbers from file...");
            var s = Stopwatch.StartNew();

            var primes = File.ReadAllLines(@"c:\LocalFolder\dont\primes1.txt")
                .Select(line => line.Split(',').Select(long.Parse).ToArray()).ToList();

            var primesSet = new HashSet<long>();

            foreach (var i in primes.SelectMany(primeArray => primeArray))
            {
                primesSet.Add(i);
            }

            s.Stop();
            Console.Out.WriteLine("Reading {0} Prime numbers took {1}, max Prime is: {2}\r\n", primesSet.Count,
                s.Elapsed, primesSet.Max());


            return primesSet;
        }

        public ISet<long> GetLongSetOfPrimesFromFile()
        {
            Console.Out.WriteLine("Reading Prime Numbers from file...");
            var s = Stopwatch.StartNew();

            var primes = File.ReadAllLines(@"c:\LocalFolder\dont\primes1.txt")
                .Select(line => line.Split(',').Select(long.Parse).ToArray()).ToList();

            primes.AddRange(File.ReadAllLines(@"c:\LocalFolder\dont\primes2.txt")
                .Select(line => line.Split(',').Select(long.Parse).ToArray()).ToList());

            primes.AddRange(File.ReadAllLines(@"c:\LocalFolder\dont\primes3.txt")
                .Select(line => line.Split(',').Select(long.Parse).ToArray()).ToList());

            primes.AddRange(File.ReadAllLines(@"c:\LocalFolder\dont\primes4.txt")
                .Select(line => line.Split(',').Select(long.Parse).ToArray()).ToList());

            primes.AddRange(File.ReadAllLines(@"c:\LocalFolder\dont\primes5.txt")
                .Select(line => line.Split(',').Select(long.Parse).ToArray()).ToList());

            primes.AddRange(File.ReadAllLines(@"c:\LocalFolder\dont\primes6.txt")
                .Select(line => line.Split(',').Select(long.Parse).ToArray()).ToList());

            var primesSet = new HashSet<long>();

            foreach (var i in primes.SelectMany(primeArray => primeArray))
            {
                primesSet.Add(i);
            }

            s.Stop();
            Console.Out.WriteLine("Reading {0} Prime numbers took {1}, max Prime is: {2}\r\n", primesSet.Count,
                s.Elapsed, primesSet.Max());


            return primesSet;
        }

        public ISet<UInt64> GetSetOfSquaresFromFile()
        {
            Console.Out.WriteLine("Reading Square Numbers from file...");
            var s = Stopwatch.StartNew();

            var squares = File.ReadAllLines(@"c:\LocalFolder\MISC\squares.txt")
                .Select(line => line.Split(',').Select(UInt64.Parse).ToArray()).ToList();

            var squaresSet = new HashSet<UInt64>();

            foreach (var i in squares.SelectMany(primeArray => primeArray))
            {
                squaresSet.Add(i);
            }

            s.Stop();
            Console.Out.WriteLine("Reading {0} square numbers took {1}, max square is: {2}\r\n", squaresSet.Count,
                s.Elapsed, squaresSet.Max());

            return squaresSet;
        }

        public ISet<string> GetSetOfSquaresAsStringsFromFile()
        {
            Console.Out.WriteLine("Reading Square Numbers as Strings from file...");
            var s = Stopwatch.StartNew();

            var squares = File.ReadAllLines(@"c:\LocalFolder\MISC\squares.txt").ToList();

            var squaresSet = new HashSet<string>();

            foreach (var i in squares)
            {
                squaresSet.Add(i);
            }

            s.Stop();
            Console.Out.WriteLine("Reading {0} square numbers took {1}\r\n", squaresSet.Count,
                s.Elapsed);

            return squaresSet;
        }

        public void WriteListOfSqaureNumbers(long limit)
        {

            var writetext = new StreamWriter(@"c:\LocalFolder\MISC\squares.txt");

            var b = new BigInteger(1);

            for (Int64 i = 1; i * i <= limit; i++)
            {
                writetext.WriteLine(b * b);

                b = b + 1;
            }

            writetext.Close();

        }

        //upper limit is 3037000499
        public ISet<Int64> GetSquareNumbers(Int64 limit)
        {
            var squares = new HashSet<Int64>();

            for (Int64 i = 1; i < limit; i++)
            {
                squares.Add(i * i);
            }

            return squares;
        }

        public List<int> GetListOfPrimes(int limit)
        {

            Console.Out.WriteLine("Generating Prime Numbers...");
            var s = Stopwatch.StartNew();

            //make a list of booleans
            var primesAsBooleansIndex = new bool[limit + 1];
            primesAsBooleansIndex[0] = false;
            primesAsBooleansIndex[1] = false;

            //default the entire list to true
            for (var i = 2; i < primesAsBooleansIndex.Count(); i++)
            {
                primesAsBooleansIndex[i] = true;
            }

            //multiples of 2 (even numbers)
            for (var i = 4; i < primesAsBooleansIndex.Count(); i += 2)
            {
                primesAsBooleansIndex[i] = false;
            }

            //go through the odd numbers and their multiples
            for (var i = 3; i < primesAsBooleansIndex.Count(); i += 2)
            {
                for (var j = i + i; j < primesAsBooleansIndex.Count(); j += i)
                {
                    primesAsBooleansIndex[j] = false;
                }
            }

            var primes = new List<int>();

            for (var i = 0; i < primesAsBooleansIndex.Count(); i++)
            {
                if (primesAsBooleansIndex[i])
                {
                    primes.Add(i);
                }
            }

            s.Stop();
            Console.Out.WriteLine("Generating {0} Prime numbers took {1}, max prime is: {2}\r\n", primes.Count,
                s.Elapsed, primes.Max());

            return primes;


        }

        /**
         * The multiplicative order of 10 mod an integer n relatively prime to 10 
         * gives the period of the decimal expansion of the reciprocal of n (Glaisher 1878, Lehmer 1941).
         */

        public int GetLengthOfRepeatingDigitCycleInMantissa(float number)
        {

            //start at 10 mod n
            var r = 10.0f;

            //get the list of remainders
            var remainders = new List<float>();

            //get unique remainders
            while (!remainders.Contains(r))
            {
                remainders.Add(r);
                r = 10.0f * (r % number);

            }

            //return the count
            return remainders.Count;
        }


        public int BinarySearch(int search, List<int> inputList)
        {
            var value = 0;

            switch (inputList.Count)
            {
                case 0:
                    Console.Out.WriteLine("Search term not found in list");
                    break;
                case 2:
                    if (inputList[0] == search || inputList[1] == search)
                    {
                        Console.Out.WriteLine("Search Term " + search + " was Found in list.");
                        value++;
                    }
                    else
                    {
                        Console.Out.WriteLine("Search Term " + search + " was Not Found in list.");
                    }
                    break;
                default:
                    {
                        //split the list into 2 
                        var middle = inputList.Count / 2;

                        if (search == inputList[middle])
                        {
                            Console.Out.WriteLine("Found input at middle: " + inputList[middle]);
                            value++;
                        }
                        else if (search < inputList[middle])
                        {
                            value += BinarySearch(search, inputList.GetRange(0, middle));
                        }
                        else
                        {
                            value += BinarySearch(search, inputList.GetRange(middle + 1, inputList.Count - middle - 1));
                        }
                    }
                    break;
            } //end switch

            return value;
        }

        public List<int> BinarySort(List<int> input)
        {
            var output = new List<int>();

            switch (input.Count)
            {

                case 0:
                    //nothing to do
                    break;
                case 1:
                    //return the input
                    output.AddRange(input);
                    break;
                default:
                    {
                        //choose pivot
                        var pivot = input[input.Count / 2];

                        //make lists
                        var left = new List<int>();
                        var right = new List<int>();
                        var pivots = new List<int>();

                        //order the lists by the chosen pivot
                        foreach (var i in input)
                        {
                            if (i < pivot)
                            {
                                left.Add(i);
                            }
                            else if (i == pivot)
                            {
                                pivots.Add(i);
                            }
                            else
                            {
                                right.Add(i);
                            }
                        }

                        //recursive call to sort sub lists
                        left = BinarySort(left);
                        right = BinarySort(right);

                        //add the sorted lists to the output
                        left.AddRange(pivots);
                        left.AddRange(right);

                        output = left;
                    }
                    break;
            } //end switch

            return output;

        }

        public List<int> Factorise(int number)
        {

            var factors = new HashSet<int>();

            if (number == 1)
            {
                return new List<int> { 1 };
            }

            //the only factors are 2 * 1
            if (number == 2)
            {
                return new List<int> { 1, 2 };
            }

            factors.Add(1);

            for (var i = 2; i < Math.Sqrt(number) + 1; i++)
            {
                if (number % i != 0) continue;
                factors.Add(i);
                factors.Add(number / i);
            }
            return factors.ToList();
        }

        public List<BigInteger> BigFactorise(BigInteger number)
        {

            var factors = new HashSet<BigInteger>();

            if (number == 1)
            {
                return new List<BigInteger> { 1 };
            }

            //the only factors are 2 * 1
            if (number == 2)
            {
                return new List<BigInteger> { 1, 2 };
            }

            for (BigInteger i = 2; i < (BigInteger)Math.Sqrt((double)number) + 1; i++)
            {
                if (number % i != 0) continue;
                factors.Add(i);
                factors.Add(number / i);
            }
            return factors.ToList();
        }

        public List<int> PrimeFactorise(int number, List<int> primes)
        {
            //make list of numbers 
            var primeFactorisation = new List<int>();

            //trying to factorise a prime number
            if (primes.Contains(number)) return new List<int> { number };

            //don't do anything if the number is one (1 is not prime)
            if (number == 1) return primeFactorisation;

            //factorise the number and get max 
            var maxFactor = Factorise(number).Max();

            //get the minimum factor
            var minFactor = number / maxFactor;

            //add the prime factorisation 
            primeFactorisation.AddRange(PrimeFactorise(maxFactor, primes));
            primeFactorisation.AddRange(PrimeFactorise(minFactor, primes));

            //return prime factorisation
            return primeFactorisation;
        }


        //gets n ^ n
        public BigInteger Power(UInt64 n)
        {

            BigInteger result = 1;

            for (UInt64 i = 0; i < n; i++)
            {
                result *= n;
            }

            return result;
        }

        //gets b^n
        public BigInteger ToThePower(BigInteger b, int n)
        {

            var result = new BigInteger(1);

            for (var i = 0; i < n; i++)
            {
                result *= b;
            }

            return result;
        }

        public BigInteger Factorial(BigInteger n)
        {
            if (n.Equals(BigInteger.Zero))
            {
                return BigInteger.One;
            }

            if (n.Equals(BigInteger.One))
            {
                return BigInteger.One;
            }
            return n * Factorial(n - 1);
        }

        /**
         * n! / r!(n-r)!
         */

        public BigInteger GetNumberOfCombinations(int n, int r)
        {

            if (r > n)
            {
                return BigInteger.Zero;
            }

            var nFact = Factorial(new BigInteger(n));
            var rFact = Factorial(new BigInteger(r));
            var nMinusrFact = Factorial(new BigInteger(n - r));
            return nFact / (rFact * nMinusrFact);
        }

        public List<int[]> GetCombinations(int[] n, int k)
        {

            var combinations = new List<int[]>();

            if (k == 0)
            {
                return combinations;
            }

            for (var i = 0; i < n.Length; i++)
            {
                // get new first element
                var first = new[] { n[i] };

                var restArray = n.Skip(i + 1).ToArray();

                var newCombinations = GetCombinations(restArray, k - 1);

                if (newCombinations.Count == 0 && k == 1)
                {
                    combinations.Add(first);
                }

                combinations.AddRange(
                    newCombinations.Select(newCombination => first.ToList().Concat(newCombination).ToArray())
                        .Where(c => c.Length == k));
            }

            return combinations;

        }


        public List<long[]> GetLongCombinations(long[] n, int k)
        {

            var combinations = new List<long[]>();

            if (k == 0)
            {
                return combinations;
            }

            for (var i = 0; i < n.Length; i++)
            {
                // get new first element
                var first = new[] { n[i] };

                var restArray = n.Skip(i + 1).ToArray();

                var newCombinations = GetLongCombinations(restArray, k - 1);

                if (newCombinations.Count == 0 && k == 1)
                {
                    combinations.Add(first);
                }

                combinations.AddRange(
                    newCombinations.Select(newCombination => first.ToList().Concat(newCombination).ToArray())
                        .Where(c => c.Length == k));
            }

            return combinations;

        }

        /**
         * WARNING: this doesn't seem to work above 4P4
         */

        public IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public ISet<string> GetStringPermutations(string input)
        {

            var permutations = new HashSet<string>();

            var nextPerm = input;

            do
            {
                nextPerm = GetNextPermutation(nextPerm.ToCharArray(), nextPerm.Length);
                permutations.Add(nextPerm);

            } while (!nextPerm.Equals(input));


            return permutations;
        }

        public string SortString(string toSort)
        {
            var sList = toSort.ToCharArray().ToList();
            sList.Sort();
            return new string(sList.ToArray());

        }

        public bool IsNumberPermutation(int a, int b)
        {
            return SortString(a.ToString(CultureInfo.InvariantCulture)).Equals(SortString(b.ToString(CultureInfo.InvariantCulture)));
        }

        public bool NumberContainsSequenceInOrder(string number, char[] sequence)
        {
            var lastIndex = -1;

            foreach (var cn in sequence)
            {
                if (number.Contains(cn) && number.IndexOf(cn) > lastIndex)
                {
                    //number is valid
                    lastIndex = number.IndexOf(cn);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        //this is dykstra's method for lexographical permutations
        public string GetNextPermutation(char[] input, int n)
        {
            var i = n - 1;

            while (i > 0 && input[i - 1] >= input[i])
            {
                i = i - 1;
            }

            var j = n;

            while (j > 0 && i > 0 && input[j - 1] <= input[i - 1])
            {
                j = j - 1;
            }

            if (i > 0 && j > 0)
            {
                SwapCharactersInArray(input, i - 1, j - 1); // swap values at positions (i-1) and (j-1)
            }

            i++;
            j = n;

            while (i < j)
            {
                SwapCharactersInArray(input, i - 1, j - 1);
                i++;
                j--;
            }

            return new string(input);
        }

        public void HeapPermute(char[] input, int n)
        {

            if (n == 1)
            {
                Console.Out.WriteLine(new string(input));
            }
            else
            {
                for (var i = 0; i < n; i++)
                {
                    HeapPermute(input, n - 1);

                    // if n is odd
                    if (n % 2 == 1)
                    {
                        SwapCharactersInArray(input, 0, n - 1);
                    }
                    else // if n is even
                    {
                        SwapCharactersInArray(input, i, n - 1);
                    }
                }
            }
        }

        private void SwapCharactersInArray(char[] input, int i, int j)
        {
            var tmp = input[i];
            input[i] = input[j];
            input[j] = tmp;
        }

        public List<int> SwapDigitsInNumber(int number, int[] swapIndexes, int[] swapDigits)
        {
            //swap indexes could crash this if it contains an invalid index

            //number as character array
            var cNumber = number.ToString(CultureInfo.InvariantCulture).ToCharArray();

            //hold the substitutions
            var subs = new List<int>();

            foreach (var d in swapDigits)
            {
                //swap digit as character
                var cSwapDigit = char.Parse(d.ToString(CultureInfo.InvariantCulture));

                //each swap index
                foreach (var i in swapIndexes)
                {
                    cNumber[i] = cSwapDigit;
                }
                //add to the list of subs
                subs.Add(int.Parse(new string(cNumber)));
            }

            //return the list of substitutions
            return subs;
        }

        //this works because the partial quotient of root 2 is [1:2] (2 repeating)
        public Fraction GenerateRootTwoAsContinuousFraction(int i, Fraction f)
        {
            return
                f.DivideFraction(i == 1
                    ? new Fraction(2, 1)
                    : new Fraction(2, 1).AddFraction(GenerateRootTwoAsContinuousFraction(i - 1, f)));
        }

        //this method is innacurate after about 16 terms
        public List<long> GetRecurringPartialFractions(double start, int limit, bool debug)
        {
            var partialQuotients = new List<long>();
            var previousQuotients = new List<decimal>();

            var startD = new decimal(start);
            var a0 = decimal.ToInt32(decimal.Floor(startD));

            partialQuotients.Add(a0);
            previousQuotients.Add(startD);

            if (debug)
            {
                Console.Out.WriteLine();
            }

            for (var i = 1; i <= limit; i++)
            {
                var subtraction = decimal.Subtract(previousQuotients[i - 1], partialQuotients[i - 1]);
                var reciprocal = decimal.Divide(decimal.One, subtraction);

                previousQuotients.Add(reciprocal);

                var aii = decimal.ToInt32(decimal.Floor(reciprocal));
                partialQuotients.Add(aii);

                if (debug)
                {
                    Console.Out.WriteLine("a{0} : {1} = 1.0 / {2} - {3}", i, aii, previousQuotients[i - 1],
                        partialQuotients[i - 1]);
                }
            }

            if (debug)
            {
                Console.Out.WriteLine();
            }

            return partialQuotients;
        }

        public List<long> GeneratePartialFractionListFromSquareRoot(int n, bool debug)
        {
            var partialQuotients = new List<long>();

            var r = (long)Math.Floor(Math.Sqrt(n));

            partialQuotients.Add(r);

            long a = r, p = 0, q = 1;

            do
            {
                p = a * q - p;
                q = (n - p * p) / q;
                a = (r + p) / q;

                partialQuotients.Add(a);

            } while (q != 1);


            if (!debug) return partialQuotients;

            Console.Out.Write("{0} = [{1}: ", n, partialQuotients[0]);

            for (var b = 1; b < partialQuotients.Count; b++)
            {
                Console.Out.Write("{0}", partialQuotients[b]);

                if (b < partialQuotients.Count - 1)
                {
                    Console.Out.Write(", ");
                }
            }

            Console.Out.WriteLine("]");

            return partialQuotients;
        }

        public Fraction GenerateContinuousFraction(int limit, int i, List<long> partials)
        {
            return
                new Fraction(1, 1).DivideFraction(i == limit
                    ? new Fraction(partials[i], 1)
                    : new Fraction(partials[i], 1).AddFraction(GenerateContinuousFraction(limit, i + 1, partials)));
        }

        public List<long> GetPartialQuotientsOfE(int limit)
        {
            var quotients = new List<long> { 2 };
            var n = 2;

            for (var i = 0; i < limit; i++)
            {
                quotients.Add(1);
                quotients.Add(n);
                quotients.Add(1);
                n += 2;
            }

            return quotients;
        }

        /**
         * the exponents of x on the right hand side are the generalized pentagonal numbers; i.e., 
         * numbers of the form ½m(3m − 1), where m is an integer and the signs in the 
         * summation alternate as (-1)^m.
         * 
         * 1 - x - x^2 + x^5 + x^7 - x^12 - x^15 + x^22 + x^26
         * 
         * This theorem can be used to derive a recurrence for the partition function:
         * 
         * p(k) = p(k − 1) + p(k − 2) − p(k − 5) − p(k − 7) + p(k − 12) + p(k − 15) − p(k − 22) − ...
         * 
         * where p(0) is taken to equal 1, and p(k) is taken to be zero for negative k.
         * 
         * The first few values of the partition function are (starting with p(0)=1):
         * 
         * 1, 1, 2, 3, 5, 7, 11, 15, 22, 30, 42, 56, 77, 101, 135, 176, 231, 297, 
         * 385, 490, 627, 792, 1002, 1255, 1575, 1958, 2436, 3010, 3718, 4565, 5604, … 
         * (sequence A000041 in OEIS).
         * 
         */
        public BigInteger PartitionFunction(int n, List<BigInteger> pValues)
        {
            //p(0) = 1
            if (n == 0) return 1;

            //p(n<0) = 0
            if (n < 0) return 0;

            //already worked out in input list
            if (pValues.Count - 1 >= n)
            {
                return pValues[n];
            }

            //make the sign and default to positive
            var sign = 1;

            //start values
            var k = 1;
            BigInteger result = 0;

            //pentagonal integers (+/- k) for the indexes to the 
            //generator series
            var k1 = k * (3 * k - 1) / 2;
            var k2 = k * (3 * k + 1) / 2;

            while (k1 <= n)
            {
                if (n - k1 >= 0)
                {
                    result = result + pValues[n - k1] * sign;
                }

                if (n - k2 >= 0)
                {
                    result = result + pValues[n - k2] * sign;
                }

                //increment k
                k = k + 1;

                //calculate indexes for next loop
                k1 = k * (3 * k - 1) / 2;
                k2 = k * (3 * k + 1) / 2;

                //alternate sign
                sign = -1 * sign;
            }

            return result;

        }

        /* Triangle P3,n = n(n+1)/2
         * 1, 3, 6, 10, 15, ...
         */
        public List<long> GetTriangleNumbers(long limit)
        {
            var returnList = new List<long>();

            for (var n = 1; n <= limit; n++)
            {
                returnList.Add((n * (n + 1)) / 2);
            }

            return returnList;
        }

        /* 
        * Pentagonal P5,n = n(3n−1)/2
        * 1, 5, 12, 22, 35, ...
        */
        public List<long> GetPentagonalNumbers(long limit)
        {
            var returnList = new List<long>();

            for (var n = 1; n <= limit; n++)
            {
                returnList.Add((n * ((3 * n) - 1)) / 2);
            }

            return returnList;
        }

        /* 
        * Hexagonal P6,n=n(2n−1)
        * 1, 6, 15, 28, 45, ...
        */
        public List<long> GetHexagonalNumbers(long limit)
        {
            var returnList = new List<long>();

            for (var n = 1; n <= limit; n++)
            {
                returnList.Add(n * ((2 * n) - 1));
            }

            return returnList;
        }

        /* Heptagonal P7,n=n(5n−3)/2
        * 1, 7, 18, 34, 55
        */

        public List<long> GetHeptagonalNumbers(long limit)
        {
            var returnList = new List<long>();

            for (var n = 1; n <= limit; n++)
            {
                returnList.Add((n * ((5 * n) - 3)) / 2);
            }

            return returnList;
        }

        /* Octagonal P8,n=n(3n−2)
        * 1, 8, 21, 40, 65...
        */

        public List<long> GetOctagonalNumbers(long limit)
        {
            var returnList = new List<long>();

            for (var n = 1; n <= limit; n++)
            {
                returnList.Add(n * ((3 * n) - 2));
            }

            return returnList;
        }


        public int PhiOfN(int n, ISet<int> primes, List<int> primesList)
        {
            int phi;

            //if n is prime then phi(n) is n-1
            if (primes.Contains(n))
            {
                phi = n - 1;
            }
            else
            {
                var primeDivisors = Factorise(n).Where(primes.Contains).ToList();

                phi = n;

                if (primeDivisors.Count == 0)
                {
                    Console.Out.WriteLine("Error occurred, no prime factors for n={0}", n);
                    phi = 0;
                }
                else
                {
                    foreach (var primeDivisor in primeDivisors)
                    {
                        phi = (int)(phi * (1.0 - (1.0 / primeDivisor)));
                    }
                }

            }

            return phi;

        }

        public int SquareDigitChain(int start, bool debug)
        {
            if (debug) { Console.Out.Write(start); }


            var result = 0;

            while (result != 1 && result != 89)
            {
                result = 0;

                foreach (var c in start.ToString(CultureInfo.InvariantCulture).ToCharArray())
                {
                    var d = int.Parse(c.ToString(CultureInfo.InvariantCulture));

                    result += d * d;
                }

                start = result;

                if (debug)
                {
                    Console.Out.Write(" -> {0}", result);
                }

            }

            if (debug)
            {
                Console.Out.WriteLine();
            }
            return result;
        }

    }
}

