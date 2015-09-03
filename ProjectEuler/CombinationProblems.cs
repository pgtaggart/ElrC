using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace ConsoleApplication1
{
    class CombinationProblems
    {
        private readonly Library _lib;

        public CombinationProblems(Library lib)
        {
            _lib = lib;
        }

        /**
         * By replacing the 1st digit of the 2-digit number *3, 
         * it turns out that six of the nine possible values: 13, 23, 43, 53, 73, and 83, are all prime.
         * 
         * By replacing the 3rd and 4th digits of 56**3 with the same digit, this 5-digit number is 
         * the first example having seven primes among the ten generated numbers, yielding the family: 
         * 56003, 56113, 56333, 56443, 56663, 56773, and 56993. 
         * 
         * Consequently 56003, being the first member of this family, is the smallest prime with this property.
         * 
         * Find the smallest prime which, by replacing part of the number (not necessarily adjacent digits) with the same digit, 
         * is part of an eight prime value family.
         * 
         * answer is 121313 using 0_2_4 index replacements
         * 
         */

        public void Problem51()
        {
            //lets only look at the primes
            var primesSet = _lib.GetSetOfPrimesFromFile();

            var primesList = primesSet.ToList();
            primesList.Sort();

            var primesAsCharList = new List<List<char>>();

            for (var i = primesList.IndexOf(10007); i <= primesList.IndexOf(999961); i++)
            {
                primesAsCharList.Add(primesList[i].ToString(CultureInfo.InvariantCulture).ToCharArray().ToList());
            }

            // 10007 first five digit prime
            // 99991 last five digit prime
            for (var i = primesList.IndexOf(121313); i <= primesList.IndexOf(999961); i++)
            {
                var prime = primesList[i].ToString(CultureInfo.InvariantCulture).ToCharArray().ToList();

                var goodList = new Dictionary<string, List<int>>(primesList[i]);

                foreach (var candidatePrime in primesAsCharList)
                {
                    if (candidatePrime.Count == prime.Count)
                    {
                        //we have a candidate here
                        var differingCharacters = new List<char>();

                        var indexes = new List<int>();

                        for (var j = 0; j < candidatePrime.Count; j++)
                        {
                            if (candidatePrime[j] != prime[j])
                            {
                                //add char
                                differingCharacters.Add(candidatePrime[j]);

                                //add index
                                indexes.Add(j);
                            }
                        }

                        if (differingCharacters.Count != 3) continue;

                        var candidate = true;

                        //these should all be the same
                        var differingCharacterCandidate = candidatePrime[indexes[0]];
                        var differingCharacterPrime = prime[indexes[0]];

                        foreach (var index in indexes)
                        {
                            if (candidatePrime[index] != differingCharacterCandidate)
                            {
                                candidate = false;
                            }

                            if (prime[index] != differingCharacterPrime)
                            {
                                candidate = false;
                            }

                        }

                        if (!candidate) continue;

                        var fString = indexes[0] + "_" + indexes[1] + "_" + indexes[2];

                        if (goodList.ContainsKey(fString))
                        {
                            goodList[fString].Add(int.Parse(new string(candidatePrime.ToArray())));
                        }
                        else
                        {

                            var daNeuList = new List<int>();
                            daNeuList.Add(int.Parse(new string(candidatePrime.ToArray())));
                            goodList.Add(fString, daNeuList);
                        }



                    }
                }

                foreach (var key in goodList.Keys)
                {
                    if (goodList[key].Count >= 6)
                    {
                        Console.Out.Write("Candidate found for {0}: ", primesList[i]);
                        Console.Out.Write(" Using replacements: {0} : ", key);
                        goodList[key].ForEach(de => Console.Out.Write(de + " "));
                        Console.Out.WriteLine();
                    }
                }



            }

            // 100003 first six digit prime
            // 999961 last six digit prime


        }


        //answer is 121313 using 0_2_4 index replacements
        public void Problem51_1()
        {

            //lets only look at the primes
            var primesSet = _lib.GetSetOfPrimesFromFile();

            var primes = primesSet.ToList();
            primes.Sort();

            var maxPrime = primes.Max();

            Console.Out.WriteLine("Max prime from Set is: {0}", maxPrime);

            var input5 = new[] { 1, 2, 3, 4 };
            var input6 = new[] { 0, 1, 2, 3, 4, 5 };
            var input7 = new[] { 1, 2, 3, 4, 5, 6 };
            var input8 = new[] { 1, 2, 3, 4, 5, 6, 7 };

            var fiveDigitswapCombinations = new List<List<int[]>>();
            var sixDigitswapCombinations = new List<List<int[]>>();
            var sevenDigitswapCombinations = new List<List<int[]>>();
            var eightDigitswapCombinations = new List<List<int[]>>();

            for (var i = 1; i <= 5; i++)
            {
                fiveDigitswapCombinations.Add(_lib.GetCombinations(input5, i));
            }

            for (var i = 3; i <= 3; i++)
            {
                sixDigitswapCombinations.Add(_lib.GetCombinations(input6, i));
            }

            for (var i = 1; i <= 7; i++)
            {
                sevenDigitswapCombinations.Add(_lib.GetCombinations(input7, i));
            }

            for (var i = 1; i <= 8; i++)
            {
                eightDigitswapCombinations.Add(_lib.GetCombinations(input8, i));
            }

            var digits = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            //loop over the primes (limit prompted by question)primes.Count
            for (var i = primes.IndexOf(121313); primes[i] < 1000000; i++)
            {

                if (primes[i] == 121313)
                {
                    Console.Out.WriteLine("Bob");
                }

                var swapCombinations = primes[i] < 100000
                    ? fiveDigitswapCombinations
                    : (primes[i] < 1000000
                        ? sixDigitswapCombinations
                        : (primes[i] < 10000000 ? sevenDigitswapCombinations : eightDigitswapCombinations));

                foreach (var swapCombination in swapCombinations)
                {
                    foreach (var combination in swapCombination)
                    {

                        if (combination.Contains(0) && combination.Contains(2) && combination.Contains(4))
                        {
                            Console.Out.WriteLine("yes");
                        }

                        var subs = _lib.SwapDigitsInNumber(primes[i], combination, digits);

                        var count = subs.Count(primesSet.Contains);

                        if (count < 8) continue;

                        var s = combination.Aggregate("[ ", (current, c) => current + (c + " ")) + "]";

                        var min = subs.Min();

                        //if (min != primes[i]) continue;

                        //tell the world 
                        Console.Out.WriteLine(
                            "Prime: {0}, Min: {1}, No. subs: {2}, Primes From Subs: {3}, Replacements: {4}", primes[i],
                            min, subs.Count, count, s);

                        foreach (var sub in subs)
                        {
                            Console.Out.WriteLine(sub + " Prime: " + primesSet.Contains(sub));
                        }
                    }
                }
            }
        }


        public void Problem51_2()
        {
            //lets only look at the primes
            var primesSet = _lib.GetSetOfPrimesFromFile();

            var primes = primesSet.ToList();
            primes.Sort();

            var maxPrime = primes.Max();

            Console.Out.WriteLine("Max prime from Set is: {0}", maxPrime);
            Console.Out.WriteLine();
            var input5 = new[] { 1, 2, 3, 4 };

            var swapCombinations = new List<List<int[]>> { _lib.GetCombinations(input5, 2) };

            var digits = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            //loop over the primes (limit prompted by question)
            for (var i = primes.IndexOf(10007); primes[i] < 100000; i++)
            {
                foreach (var swapCombination in swapCombinations)
                {
                    var subs = new List<int>();

                    foreach (var combination in swapCombination)
                    {
                        subs.AddRange(_lib.SwapDigitsInNumber(primes[i], combination, digits));
                    }

                    var values = subs.Select(e => e).Intersect(primes).ToList().Select(p => p).ToList();
                    values.Sort();

                    var count = values.Count();
                    if (count != 8) continue;

                    var min = values.Min();

                    //if (min != primes[i]) continue;

                    //tell the world 
                    Console.Out.WriteLine(
                        "Number: {0}, Min: {1}, No. subs: {2}, Number of Primes From Subs: {3}", primes[i],
                        min, subs.Count, count);

                    foreach (var value in values)
                    {
                        Console.Out.Write("{0}", value);

                        if (values.IndexOf(value) != values.Count - 1)
                        {
                            Console.Out.Write(", ");
                        }
                    }

                    Console.Out.WriteLine();
                    Console.Out.WriteLine();
                }
            }
        }

        /**
         * The cube, 41063625 (345^3), can be permuted to produce two other cubes: 56623104 (384^3) and 66430125 (405^3). 
         * In fact, 41063625 is the smallest cube which has exactly three permutations of its digits which are also cube.
         * 
         * Find the smallest cube for which exactly five permutations of its digits are cube.
         */
        public void Problem62()
        {

            var cubes = new HashSet<String>();

            var maxCubeRoot = new BigInteger(10000);

            for (var c = new BigInteger(1); c <= maxCubeRoot; c++)
            {
                cubes.Add(_lib.ToThePower(c, 3).ToString());
            }

            var maxCube = _lib.ToThePower(maxCubeRoot, 3);

            Console.Out.WriteLine("Number of Cubes: {0}, Max Cube: {1}", cubes.Count, maxCube);
            Console.Out.WriteLine();

            var sortedCubes = new Dictionary<String, List<String>>();

            foreach (var cube in cubes)
            {
                var c = cube.ToCharArray().ToList().Select(e => int.Parse(e.ToString(CultureInfo.InvariantCulture))).ToList();
                c.Sort();

                var s = "";

                foreach (var i in c)
                {
                    s += i.ToString(CultureInfo.InvariantCulture);
                }

                if (sortedCubes.ContainsKey(s))
                {
                    sortedCubes[s].Add(cube);
                }
                else
                {
                    sortedCubes.Add(s, new List<string> { cube });
                }

            }

            foreach (var sortedCube in sortedCubes)
            {
                if (sortedCube.Value.Count == 5)
                {
                    Console.Out.WriteLine("base Key: {0}", sortedCube.Key);
                    Console.Out.WriteLine();
                    Console.Out.WriteLine("Sorted Big Integers : ");
                    Console.Out.WriteLine();

                    var cus = new List<BigInteger>();

                    sortedCube.Value.ForEach(x => cus.Add(BigInteger.Parse(x)));

                    cus.Sort();

                    cus.ForEach(e => Console.Out.WriteLine(e));

                    Console.Out.WriteLine();

                    for (var c = new BigInteger(1); c <= maxCubeRoot; c++)
                    {
                        if (_lib.ToThePower(c, 3).Equals(cus[0]))
                        {
                            Console.Out.WriteLine("Cube root is : {0}", c);
                        }
                    }
                    //answer is 127035954683

                    Console.Out.WriteLine();

                }
            }
        }

    }
}
