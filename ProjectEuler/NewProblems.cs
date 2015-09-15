
using ProjectEuler;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ConsoleApplication1
{
    public class NewProblems
    {
        private readonly Library _lib;

        public NewProblems(Library lib)
        {
            _lib = lib;
        }


        /*
         * The number 145 is well known for the property that the sum of the factorial of its digits is equal to 145
         * 
         * 1! + 4! + 5! = 1 + 24 + 120 = 145
         * 
         * Perhaps less well known is 169, in that it produces the longest chain of numbers that link back to 169; 
         * it turns out that there are only three such loops that exist
         * 
         * 169 → 363601 → 1454 → 169
         * 871 → 45361 → 871
         * 872 → 45362 → 872
         * 
         * It is not difficult to prove that EVERY starting number will eventually get stuck in a loop. For example,
         * 
         * 69 → 363600 → 1454 → 169 → 363601 (→ 1454)
         * 78 → 45360 → 871 → 45361 (→ 871)
         * 540 → 145 (→ 145)
         * 
         * Starting with 69 produces a chain of five non-repeating terms, 
         * but the longest non-repeating chain with a starting number below one million is sixty terms.

         * How many chains, with a starting number below one million, contain exactly sixty non-repeating terms?
         * 
         */

        public void Problem74()
        {

            var result = 0;

            for (var i = 69; i < 1000000; i++)
            {
                var b = new BigInteger(i);

                var sumSet = new HashSet<BigInteger> { b };

                for (var j = 0; j < 70; j++)
                {
                    var innerSum = new BigInteger(0);

                    foreach (var number in b.ToString().ToArray())
                    {
                        innerSum +=
                            _lib.Factorial(new BigInteger(int.Parse(number.ToString(CultureInfo.InvariantCulture))));
                    }

                    if (sumSet.Contains(innerSum)) break;

                    sumSet.Add(innerSum);

                    b = BigInteger.Parse(innerSum.ToString());

                }

                if (sumSet.Count != 60) continue;

                Console.Out.WriteLine("Found {0} which has length 60", i);
                result++;
            }

            Console.Out.WriteLine("Answer is {0}", result);

        }

        public void Problem76()
        {

            //make a list of the first values
            var pValues = new List<BigInteger> { 1, 1, 2, 3 };

            for (var n = 4; n <= 100000; n++)
            {
                var pN = _lib.PartitionFunction(n, pValues);
                pValues.Add(pN);

                BigInteger remainder;
                BigInteger.DivRem(pN, new BigInteger(1000000), out remainder);

                if (!remainder.Equals(BigInteger.Zero)) continue;
                Console.Out.WriteLine("Found divisible by 1000000 for n = {0}, p({0}) = {1}", n, pN);
                break;
            }
        }

        /***
         * 
         * A common security method used for online banking is to ask the user for three random characters from a passcode. 
         * For example, if the passcode was 531278, they may ask for the 2nd, 3rd, and 5th characters; the expected reply would be: 317.
         * 
         * The text file, keylog.txt, contains fifty successful login attempts.
         * 
         * Given that the three characters are always asked for in order, analyse the file so 
         * as to determine the shortest possible secret passcode of unknown length
         * 
         */
        public void Problem79()
        {

            //get distinct entries from reading the file
            var sequences = File.ReadAllLines(Constants.RESOURCE_LOCATION + "p079_keylog.txt").Distinct().Select(c => c.ToCharArray()).ToList();

            Console.Out.WriteLine("Number of distinct entries in the file {0}", sequences.Count);

            // NOTE : From here i am assuming that the guesses contain all the numbers in the
            // actual passcode - this may not be the case and could preculde the use of permutations
            // UPDATE: Not the case and the question is easier than first thought

            //get the numbers used in the guesses
            var numbers = new SortedSet<int>();

            foreach (var numberAsChar in sequences.SelectMany(charArray => charArray))
            {
                numbers.Add(int.Parse(numberAsChar.ToString(CultureInfo.InvariantCulture)));
            }

            //start with the sorted number containing all of the digits above
            var baseNumber = "";
            numbers.ToList().ForEach(n => baseNumber += n);
            Console.Out.WriteLine("Base number is {0}", baseNumber);

            //which number contains all of these attempts in the corret order - 
            //no need to check if number contains all numbers first, as
            //we are going to use permutations, hopefully lexographically so that this will stop on
            //the smallest possible match

            var permutation = new string(baseNumber.ToCharArray());

            for (var b = BigInteger.Zero; b < _lib.Factorial(new BigInteger(baseNumber.Length)); b++)
            {
                if (!sequences.All(sequence => _lib.NumberContainsSequenceInOrder(permutation, sequence)))
                {
                    permutation = _lib.GetNextPermutation(permutation.ToCharArray(), baseNumber.Length);
                    continue;
                }

                Console.Out.WriteLine("Number found: {0}", permutation);
                break;
            }

            // answer is 73162890

        }

        /**
         * 
         * In the 5 by 5 matrix below, the minimal path sum from the top left to the bottom right, 
         * by only moving to the right and down, 
         * is indicated in bold red and is equal to 2427.
         * 
         * 131 -> 201 -> 96 -> 342 -> 746 -> 422 -> 121 -> 37 -> 331
         * 
         * Find the minimal path sum, in matrix.txt (right click and "Save Link/Target As..."), 
         * a 31K text file containing a 80 by 80 matrix, from the top left to the bottom right by 
         * only moving right and down.
         * 
         **/
        public void Problem81()
        {
            int[][] matrix = 
            {
                new [] {131, 673, 234, 103, 18 },
                new [] {201,  96, 342, 965, 150},
                new [] {630, 803, 746, 422, 111},
                new [] {537, 699, 497, 121, 956},
                new [] {805, 732, 524,  37, 331}
                
            };

            Console.Out.WriteLine("Matrix Sum: {0}, expected: {1}", _lib.GetMinSumThroughMatrixRightAndDownOnly(matrix), 2427);
            Console.Out.WriteLine("Big Matrix Sum: {0}, expected: {1}", _lib.GetMinSumThroughMatrixRightAndDownOnly(_lib.ReadBigMatrixFile(Constants.RESOURCE_LOCATION + "p081_matrix.txt")), 427337);
        }

        /**
         * NOTE: This problem is a more challenging version of Problem 81.
         * 
         * The minimal path sum in the 5 by 5 matrix below, by starting in any cell in the left column 
         * and finishing in any cell in the right column, and only moving up, down, and right, is indicated in red and bold; 
         * the sum is equal to 994
         * 
         * Find the minimal path sum, in matrix.txt (right click and "Save Link/Target As..."), 
         * a 31K text file containing a 80 by 80 matrix, from the left column to the right column.
         * 
         */
        public void Problem82()
        {
            int[][] matrix = 
            {
                new [] {131, 673, 234, 103, 18 },
                new [] {201,  96, 342, 965, 150},
                new [] {630, 803, 746, 422, 111},
                new [] {537, 699, 497, 121, 956},
                new [] {805, 732, 524,  37, 331}
                
            };

            Console.Out.WriteLine("Matrix Sum: {0}, expected: {1}", _lib.GetMinSumThroughMatrixUpDownAndRight(matrix), 994);


        }

        /**
         * A player starts on the GO square and adds the scores on two 6-sided dice to determine the 
         * number of squares they advance in a clockwise direction. 
         * 
         * Without any further rules we would expect to visit each square with equal probability: 2.5%. 
         * However, landing on G2J (Go To Jail), CC (community chest), and CH (chance) changes this distribution.
         * 
         * In addition to G2J, and one card from each of CC and CH, that orders the player to go directly to jail,
         * if a player rolls three consecutive doubles, they do not advance the result of their 3rd roll. 
         * Instead they proceed directly to jail.
         * 
         * At the beginning of the game, the CC and CH cards are shuffled. 
         * When a player lands on CC or CH they take a card from the top of the respective pile and, 
         * after following the instructions, it is returned to the bottom of the pile. 
         * 
         * There are sixteen cards in each pile, but for the purpose of this problem we are only 
         * concerned with cards that order a movement; any instruction not concerned with movement 
         * will be ignored and the player will remain on the CC/CH square.
         *
         * •Community Chest (2/16 cards): 
         * 1.Advance to GO
         * 2.Go to JAIL
         * 
         * •Chance (10/16 cards): 
         * 1.Advance to GO
         * 2.Go to JAIL
         * 3.Go to C1
         * 4.Go to E3
         * 5.Go to H2
         * 6.Go to R1
         * 7.Go to next R (railway company)
         * 8.Go to next R
         * 9.Go to next U (utility company)
         * 10.Go back 3 squares.
         * 
         * The heart of this problem concerns the likelihood of visiting a particular square. 
         * That is, the probability of finishing at that square after a roll. 
         * For this reason it should be clear that, with the exception of G2J for which the 
         * probability of finishing on it is zero, the CH squares will have the lowest probabilities, 
         * as 5/8 request a movement to another square, and it is the final square that the player 
         * finishes at on each roll that we are interested in. 
         * 
         * We shall make no distinction between "Just Visiting" and being sent to JAIL, 
         * and we shall also ignore the rule about requiring a double to "get out of jail", 
         * assuming that they pay to get out on their next turn.
         * 
         * By starting at GO and numbering the squares sequentially from 00 to 39 we can concatenate these 
         * two-digit numbers to produce strings that correspond with sets of squares.
         * 
         * Statistically it can be shown that the three most popular squares, 
         * in order, are JAIL (6.24%) = Square 10, E3 (3.18%) = Square 24, and GO (3.09%) = Square 00. 
         * So these three most popular squares can be listed with the six-digit modal string: 102400.
         * 
         * If, instead of using two 6-sided dice, two 4-sided dice are used, find the six-digit modal string.
         * 
         * */
        public void Problem84()
        {

            var board = new[]
            {
                "GO", "A1", "CC1", "A2", "T1", "R1", "B1", "CH1", "B2", "B3", "JAIL",
                "C1", "U1", "C2", "C3", "R2", "D1", "CC2", "D2", "D3", "FP",
                "E1", "CH2", "E2", "E3", "R3", "F1", "F2", "U2", "F3", "G2J",
                "G1", "G2", "CC3", "G3", "R4", "CH3", "H1", "T2", "H2"
            };

            var communityChest = new[] { "AG", "JAIL", "NOOP", "NOOP", "NOOP", "NOOP", "NOOP", "NOOP", "NOOP", "NOOP", "NOOP", "NOOP", "NOOP", "NOOP", "NOOP", "NOOP" };

            var chance = new[] { "AG", "JAIL", "C1", "E3", "H2", "R1", "NR", "NR", "NU", "BACK3", "NOOP", "NOOP", "NOOP", "NOOP", "NOOP", "NOOP" };

            var r = new Random();
            const int diceMaxVal = 7;

            //make a map of the squares and times visited
            var squaresVisited = new Dictionary<String, int>();
            board.ToList().ForEach(s => squaresVisited.Add(s, 0));

            //start at go
            squaresVisited["GO"]++;

            //shuffle the card decks randomly
            var ccDeck = CreateShuffledCardDeck(communityChest);
            var chDeck = CreateShuffledCardDeck(chance);

            //number of goes for simulation
            const int simSteps = 100000000;

            //initial steps
            var currentSquareIndex = 0;
            var doublesRolled = 0;

            //play the game
            for (var i = 0; i < simSteps; i++)
            {
                //roll the dice
                var dice1 = r.Next(1, diceMaxVal);
                var dice2 = r.Next(1, diceMaxVal);

                //check double rolled
                if (dice1 == dice2)
                {
                    doublesRolled++;
                }
                else
                {
                    //no further turn as no consecutive double rolled
                    doublesRolled = 0;
                }

                //rolled 3 doubles
                if (doublesRolled == 3)
                {
                    //reset doubles, end of turn
                    doublesRolled = 0;

                    //go to jail
                    currentSquareIndex = board.ToList().IndexOf("JAIL");

                    //increment jail square by one
                    squaresVisited["JAIL"]++;

                    //go to next roll
                    continue;
                }

                //move to next square
                currentSquareIndex = (currentSquareIndex + dice1 + dice2) % board.Length;

                //is this go to jail?
                if (board[currentSquareIndex].Equals("G2J"))
                {
                    //reset doubles, end of turn
                    //doublesRolled = 0;

                    //go to jail
                    currentSquareIndex = board.ToList().IndexOf("JAIL");

                    //increment jail square by one
                    squaresVisited["JAIL"]++;

                    //go to next roll
                    continue;
                }

                var moves = -1;

                //is this a chance square
                if (board[currentSquareIndex].StartsWith("CH"))
                {
                    moves = TakeAChance(chDeck, board, currentSquareIndex);
                }

                //is this a community chest square
                if (board[currentSquareIndex].StartsWith("CC"))
                {
                    moves = TakeAChance(ccDeck, board, currentSquareIndex);
                }

                //if we were moved by a card
                if (moves != -1)
                {
                    //move to the new square
                    currentSquareIndex = moves;
                }

                //have we been sent to jail here ffs?
                if (board[currentSquareIndex].Equals("JAIL"))
                {
                    //reset doubles, end of turn
                    // doublesRolled = 0;

                    //go to jail
                    currentSquareIndex = board.ToList().IndexOf("JAIL");

                    //no need to increment jail square by one
                    //as we will do so in the process of moving
                    squaresVisited["JAIL"]++;

                    //go to next roll
                    continue;
                }

                //increment current square count
                squaresVisited[board[currentSquareIndex]]++;


            }

            var results = new SortedDictionary<double, string>();

            //output the results
            for (var i = 0; i < board.Length; i++)
            {
                Console.Out.WriteLine("Square {0} : {1} , visited {2} times out of {3}. Probablility is {4}", i, board[i], squaresVisited[board[i]], simSteps, squaresVisited[board[i]] / (double)simSteps * 100.00);
                results.Add(squaresVisited[board[i]] / (double)simSteps * 100.00, board[i]);
            }

            Console.Out.WriteLine();

            foreach (var result in results)
            {
                Console.Out.WriteLine("{0} \t({1}%) = Square {2:D}", result.Value, result.Key, board.ToList().IndexOf(result.Value));
            }

        }

        public LinkedList<string> CreateShuffledCardDeck(string[] deck)
        {
            var cardDeck = new LinkedList<string>();
            var rnd = new Random();
            deck.OrderBy(e => rnd.Next()).ToList().ForEach(x => cardDeck.AddLast(x));
            return cardDeck;
        }

        public int TakeAChance(LinkedList<string> cards, string[] board, int currentBoardIndex)
        {

            //pick the next card in the deck
            var card = cards.First;

            //remove it from the deck
            cards.RemoveFirst();

            //add it back to the bottom of the deck
            cards.AddLast(card);

            //if it's move back 3 spaces
            switch (card.Value)
            {
                case "NOOP":
                    return -1;
                case "BACK3":
                    return currentBoardIndex - 3;
                case "AG":
                    return board.ToList().IndexOf("GO");
                case "JAIL":
                    return board.ToList().IndexOf("JAIL");
                case "C1":
                    return board.ToList().IndexOf("C1");
                case "E3":
                    return board.ToList().IndexOf("E3");
                case "H2":
                    return board.ToList().IndexOf("H2");
                case "R1":
                    return board.ToList().IndexOf("R1");
                case "NR":
                    while (true)
                    {
                        if (currentBoardIndex == board.Length - 1)
                        {
                            currentBoardIndex = 0;
                        }

                        if (board[currentBoardIndex++].StartsWith("R"))
                        {
                            return currentBoardIndex;
                        }

                    }
                case "NU":
                    while (true)
                    {
                        if (currentBoardIndex == board.Length - 1)
                        {
                            currentBoardIndex = 0;
                        }

                        if (board[currentBoardIndex++].StartsWith("U"))
                        {
                            return currentBoardIndex;
                        }

                    }
            }

            return -1;

        }

        /**
         * 
         * The smallest number expressible as the sum of a prime square, prime cube, and prime fourth power is 28. 
         * In fact, there are exactly four numbers below fifty that can be expressed in such a way:
         * 
         * 28 = 2^2 + 2^3 + 2^4
         * 33 = 3^2 + 2^3 + 2^4
         * 49 = 5^2 + 2^3 + 2^4
         * 47 = 2^2 + 3^3 + 2^4
         * 
         * How many numbers below fifty million can be expressed as the sum of a prime square, prime cube, and prime fourth power?
         */
        public void Problem87()
        {
            //set limit of 50 million
            const long limit = 50000000;

            //get primes
            var primes = _lib.GetListOfPrimes(10000);

            var solutions = new HashSet<long>();

            //square prime numbers
            for (var i = 0; i < primes.Count; i++)
            {

                long square = primes[i] * primes[i];

                if (square > limit) continue;

                //cube prime numbers
                for (int j = 0; j < primes.Count; j++)
                {
                    long cube = primes[j] * primes[j] * primes[j];

                    if (square > limit) break;

                    if (square + cube > limit) break;

                    //fourth power prime numbers
                    for (int k = 0; k < primes.Count; k++)
                    {
                        long fourth = primes[k] * primes[k] * primes[k] * primes[k];

                        if (fourth > limit) break;

                        if (square + cube + fourth > limit) break;

                        solutions.Add(square + cube + fourth);
                    }

                }//end cubes

            }//end squares

            Console.Out.WriteLine(solutions.Count); // solved 1097343

        }

        /**
         * A natural number, N, that can be written as the sum and product of a given set of at 
         * least two natural numbers, {a1, a2, ... , ak} is called 
         * a product-sum number: N = a1 + a2 + ... + ak = a1 × a2 × ... × ak.
         * 
         * For example, 6 = 1 + 2 + 3 = 1 × 2 × 3.
         * 
         * For a given set of size, k, we shall call the smallest N with this property a minimal product-sum number. 
         * The minimal product-sum numbers for sets of size, k = 2, 3, 4, 5, and 6 are as follows.
         * 
         * k=2: 4 = 2 × 2 = 2 + 2
         * k=3: 6 = 1 × 2 × 3 = 1 + 2 + 3
         * k=4: 8 = 1 × 1 × 2 × 4 = 1 + 1 + 2 + 4
         * k=5: 8 = 1 × 1 × 2 × 2 × 2 = 1 + 1 + 2 + 2 + 2
         * k=6: 12 = 1 × 1 × 1 × 1 × 2 × 6 = 1 + 1 + 1 + 1 + 2 + 6
         * 
         * Hence for 2≤k≤6, the sum of all the minimal product-sum numbers is 4+6+8+12 = 30; 
         * note that 8 is only counted once in the sum.
         * 
         * In fact, as the complete set of minimal product-sum numbers for 2≤k≤12 is {4, 6, 8, 12, 15, 16}, the sum is 61.
         * 
         * What is the sum of all the minimal product-sum numbers for 2≤k≤12000
         * 
         */
        public void Problem88()
        {

            const int limit = 12;

            // k numbers in the set, minimal sum and product
            // smells like a combinatorics problem, choosing numbers for the
            // 12000 size set could be interesting

            int[] numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (var k = 2; k <= limit; k++)
            {




            }

        }

        /**
         * For a number written in Roman numerals to be considered valid there are basic rules which must be followed. 
         * Even though the rules allow some numbers to be expressed in more than one way 
         * there is always a "best" way of writing a particular number.
         * 
         * For example, it would appear that there are at least six ways of writing the number sixteen:
         * 
         * IIIIIIIIIIIIIIII
         * VIIIIIIIIIII
         * VVIIIIII
         * XIIIIII
         * VVVI
         * XVI
         * 
         * However, according to the rules only XIIIIII and XVI are valid, and the last example is considered to be the most efficient, 
         * as it uses the least number of numerals.
         * 
         * The 11K text file, roman.txt (right click and 'Save Link/Target As...'), contains one thousand numbers written in valid, 
         * but not necessarily minimal, Roman numerals; see About... Roman Numerals for the definitive rules for this problem.
         * 
         * Find the number of characters saved by writing each of these in their minimal form.
         * 
         * Note: You can assume that all the Roman numerals in the file contain no more than four consecutive identical units.
         * 
         * 
         */
        public void Problem89()
        {
            Console.Out.WriteLine("Test Case 1");

            var testCase1 = new List<RomanNumeral>
            {
                new RomanNumeral("IIIIIIIIIIIIIIII"),  //invalid
                new RomanNumeral("VIIIIIIIIIII"), //invalid
                new RomanNumeral("VVIIIIII"), //invalid
                new RomanNumeral("XIIIIII"), // valid - 16
                new RomanNumeral("VVVI"), // invalid
                new RomanNumeral("XVI"), // valid - 16
                new RomanNumeral("XIV"), // valid - 14
                new RomanNumeral("XIX") // valid - 19
            };

            testCase1.ForEach(t => Console.Out.WriteLine("{0} : Valid: {1}, Value: {2}, Subtraction: {3}, Parsed: {4} (valid: {5})", t.Numeral, t.IsNumeralValid, t.IntValue, t.ContainsSubtractive, new RomanNumeral(t.IntValue).Numeral, new RomanNumeral(t.IntValue).IsNumeralValid));

            Console.Out.WriteLine();

            var romanNumeralsFile = File.ReadAllLines(Constants.RESOURCE_LOCATION + "p089_roman.txt").ToList();

            var numberOfCharactersInFile = romanNumeralsFile.Sum(romanNumeralSequence => romanNumeralSequence.Length);

            Console.Out.WriteLine("File contains {0} lines and has {1} characters in total.", romanNumeralsFile.Count, numberOfCharactersInFile);

            var numeralObjects = romanNumeralsFile.Select(romanNumeral => new RomanNumeral(romanNumeral)).ToList();

            var invalidRomanNumerals = numeralObjects.Where(r => r.IsNumeralValid == false);

            Console.Out.WriteLine("Found {0} invalid roman numerals in file, should be 0.", invalidRomanNumerals.Count());

            //make the new roman numerals in minimal form
            var newNumeralObjects = numeralObjects.Select(romanNumeral => new RomanNumeral(romanNumeral.IntValue)).ToList();

            var newLength = newNumeralObjects.Sum(romanNumeral => romanNumeral.Numeral.Length);

            //var writetext = new StreamWriter(Constants.RESOURCE_LOCATION + "p89.csv");

            //writetext.WriteLine("Numeral, Valid, Int Value, Subtraction, Parsed Numeral, value, valid, comment");

            //numeralObjects.ForEach(t => writetext.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", t.Numeral, t.IsNumeralValid, t.IntValue, t.ContainsSubtractive, new RomanNumeral(t.IntValue).Numeral, new RomanNumeral(new RomanNumeral(t.IntValue).Numeral).IntValue, new RomanNumeral(t.IntValue).IsNumeralValid, new RomanNumeral(t.IntValue).Comment));

            //writetext.Close();

            Console.Out.WriteLine("Character saving is {0} - {1} = {2}", numberOfCharactersInFile, newLength, numberOfCharactersInFile - newLength);

            //solved: 743

        }


        /**
         * 
         * A number chain is created by continuously adding the square of the digits in a number to 
         * form a new number until it has been seen before.
         * 
         * For example,
         * 44 → 32 → 13 → 10 → 1 → 1
         * 85 → 89 → 145 → 42 → 20 → 4 → 16 → 37 → 58 → 89
         * 
         * Therefore any chain that arrives at 1 or 89 will become stuck in an endless loop. 
         * What is most amazing is that EVERY starting number will eventually arrive at 1 or 89.
         * 
         * How many starting numbers below ten million will arrive at 89
         * 
         */
        public void Problem92()
        {

            var result = 0;
            for (int i = 1; i < 10000000; i++)
            {
                if (_lib.SquareDigitChain(i, false) == 89)
                {
                    result++;
                }
            }

            Console.Out.WriteLine(result); // solved 8581146
        }

        /**
         * 
         * By using each of the digits from the set, {1, 2, 3, 4}, exactly once, 
         * and making use of the four arithmetic operations (+, −, *, /) and brackets/parentheses, 
         * it is possible to form different positive integer targets.
         * 
         * For example,
         * 8 = (4 * (1 + 3)) / 2
         * 14 = 4 * (3 + 1 / 2)
         * 19 = 4 * (2 + 3) − 1
         * 36 = 3 * 4 * (2 + 1)
         * 
         * Note that concatenations of the digits, like 12 + 34, are not allowed.
         * 
         * Using the set, {1, 2, 3, 4}, it is possible to obtain thirty-one different target 
         * numbers of which 36 is the maximum, and each of the numbers 1 to 28 can be obtained 
         * before encountering the first non-expressible number.
         * 
         * Find the set of four distinct digits, a < b < c < d, for which the longest set of 
         * consecutive positive integers, 1 to n, can be obtained, giving your answer as a string: abcd
         * 
         */
        public void Problem93()
        {
            var perms = _lib.GetStringPermutations("1234");

            var hNums = new HashSet<int>();

            foreach (var perm in perms)
            {
                var array = perm.ToCharArray();

                var a = int.Parse(array[0].ToString(CultureInfo.InvariantCulture));
                var b = int.Parse(array[1].ToString(CultureInfo.InvariantCulture));
                var c = int.Parse(array[2].ToString(CultureInfo.InvariantCulture));
                var d = int.Parse(array[3].ToString(CultureInfo.InvariantCulture));

                var a1 = a * b * c * d;

                var w = (d * (a + c)) / b;
                var x = d * (c + a / b);
                var y = d * (b + c) - a;
                var z = c * d * (b + a);

                hNums.Add(w);
                hNums.Add(x);
                hNums.Add(y);
                hNums.Add(z);
                hNums.Add(a1);

            }

            var l = hNums.ToList();
            l.Sort();

            l.ForEach(e => Console.Out.Write(e + " -> "));

            Console.Out.WriteLine("Number of numbers: {0}", l.Count);

        }

        public bool MakeNumberFromFourNumbers(int[] numbers, int target)
        {



            return false;
        }

        /**
         * The proper divisors of a number are all the divisors excluding the number itself. 
         * For example, the proper divisors of 28 are 1, 2, 4, 7, and 14. 
         * As the sum of these divisors is equal to 28, we call it a perfect number.
         * 
         * Interestingly the sum of the proper divisors of 220 is 284 and the sum of the proper divisors of 284 is 220, 
         * forming a chain of two numbers. For this reason, 220 and 284 are called an amicable pair.
         * 
         * Perhaps less well known are longer chains. For example, starting with 12496, we form a chain of five numbers:
         * 12496 → 14288 → 15472 → 14536 → 14264 (→ 12496 → ...)
         * 
         * Since this chain returns to its starting point, it is called an amicable chain.
         * Find the smallest member of the longest amicable chain with no element exceeding one million.
         */
        public void Problem95()
        {
            //start at less than one million as itself would be an element
            const int limit = 1000000;

            var primes = _lib.GetListOfPrimes(limit);

            var chains = new Dictionary<int, int[]>();

            for (var i = 6; i < limit; i++)
            {
                // do not try to factorise a prime
                if (primes.Contains(i)) continue;

                var chain = new List<int>();
                var j = i;

                while (j < limit && j != 1)
                {
                    j = _lib.Factorise(j).Sum();

                    // caught in a loop 
                    if (chain.Contains(j)) break;

                    //caught in an amicable loop
                    if (chains.Keys.Contains(j)) break;

                    chain.Add(j);

                    if (j != i) continue;

                    chains.Add(i, chain.ToArray());
                    Console.Out.Write("Chain found for {0} : ", i);
                    chain.ForEach(e => Console.Out.Write(e + " -> "));
                    Console.Out.WriteLine(i + " : Size - " + chain.Count);
                    break;
                }
            }

            //solved: 14316
        }

        /**
         * 
         * The first known prime found to exceed one million digits was discovered in 1999, 
         * and is a Mersenne prime of the form 2^6972593−1; it contains exactly 2,098,960 digits. 
         * Subsequently other Mersenne primes, of the form (2^p)−1, have been found which contain more digits.
         * 
         * However, in 2004 there was found a massive non-Mersenne prime which contains 2,357,207 digits: 
         * 28433×2^7830457+1
         * 
         * Find the last ten digits of this prime number
         * 
         */
        public void Problem97()
        {
            long i = 28433;
            for (int j = 1; j <= 7830457; j++)
            {
                i *= 2;
                i = i % 10000000000L;
            }
            i += 1;

            Console.Out.WriteLine(i);//8739992577

        }

        /**
         * 
         * By replacing each of the letters in the word CARE with 1, 2, 9, and 6 respectively, 
         * we form a square number: 1296 = 36^2. 
         * 
         * What is remarkable is that, by using the same digital substitutions, 
         * the anagram, RACE, also forms a square number: 9216 = 96^2. 
         * 
         * We shall call CARE (and RACE) a square anagram word pair and specify further that 
         * leading zeroes are not permitted, neither may a different letter have the same digital value as another letter.
         * 
         * Using words.txt (right click and 'Save Link/Target As...'), a 16K text 
         * file containing nearly two-thousand common English words, 
         * find all the square anagram word pairs (a palindromic word is NOT considered to be an anagram of itself).
         * 
         * What is the largest square number formed by any member of such a pair
         * 
         * NOTE: All anagrams formed must be contained in the given text file
         */
        public void Problem98()
        {
            //hash words for quick contains operations
            var wordSet = new HashSet<string>();

            //read words into hash
            foreach (var word in File.ReadAllLines(Constants.RESOURCE_LOCATION + "p098_words.txt").Select(c => c.Split(',')).ToList().SelectMany(line => line))
            {
                wordSet.Add(word.Replace("\"", ""));
            }

            var wordToAnagramsMapping = new Dictionary<String, List<String>>();

            //we need to make a map of the anagrams
            foreach (var word in wordSet)
            {
                //get same length words from word set
                var word1 = word;
                var pAnagrams = wordSet.Where(s => s.Length == word1.Length).ToList();

                pAnagrams.Remove(word);

                var wordArray = word.ToCharArray();

                Array.Sort(wordArray);

                var sortedWord = new string(wordArray);

                foreach (var pAnagram in pAnagrams)
                {
                    var pArray = pAnagram.ToCharArray();

                    Array.Sort(pArray);

                    if (new String(pArray).Equals(sortedWord))
                    {
                        if (wordToAnagramsMapping.ContainsKey(word))
                        {
                            wordToAnagramsMapping[word].Add(pAnagram);
                        }
                        else
                        {
                            wordToAnagramsMapping.Add(word, new List<string> { pAnagram });
                        }
                    }
                }
            }

            //get the maximum length of a word (it's 14 no need to do this every time)
            //var len = wordSet.Select(word => word.Length).Concat(new[] { 0 }).Max();

            //what is the maximum square number possible from the words
            var squares = _lib.GetSetOfSquaresAsStringsFromFile();

            var results = new List<int>();

            //go through the word list
            foreach (var word in wordToAnagramsMapping.Keys)
            {
                //get anagrams of this word that exist in the wordset
                var anagrams = wordToAnagramsMapping[word];

                //get the square numbers that match this word on length
                var word1 = word;
                var localSquares = squares.Where(s => s.Length == word1.Length);

                //no need to make this in the loop
                var originalWordArray = word.ToCharArray();

                //go through the matching length squares
                var enumerable = localSquares as string[] ?? localSquares.ToArray();

                foreach (var localSquare in enumerable)
                {
                    //assign the square to the original word
                    var letterMapping = new Dictionary<char, char>();

                    //square number as character array
                    var localSquareArray = localSquare.ToCharArray();

                    var valid = true;

                    //make the mapping
                    for (var i = 0; i < originalWordArray.Length; i++)
                    {
                        if (letterMapping.ContainsKey(originalWordArray[i]))
                        {
                            //we are trying to map an existing letter to different number
                            if (!letterMapping[originalWordArray[i]].Equals(localSquareArray[i]))
                            {
                                valid = false;
                                break;
                            }
                        }
                        else
                        {
                            //are we trying to map more than one letter to the same number?
                            if (!letterMapping.Values.Contains(localSquareArray[i]))
                            {
                                letterMapping.Add(originalWordArray[i], localSquareArray[i]);
                            }
                            else
                            {
                                valid = false;
                                break;
                            }
                        }

                    }

                    if (!valid) continue;

                    //go through the anagrams
                    foreach (var anagram in anagrams)
                    {
                        //make the anagrams squares
                        var anagramArray = anagram.ToCharArray();

                        var anagramSquare = anagramArray.Aggregate("", (current, anagramLetter) => current + letterMapping[anagramLetter]);

                        //exclude if this begins with zero
                        if (anagramSquare.StartsWith("0")) continue;

                        //check if this is a square number
                        if (enumerable.Contains(anagramSquare))
                        {
                            Console.Out.WriteLine("Anagram pair found {0}:{1} and {2}:{3}", word, localSquare, anagram, anagramSquare);
                            results.Add(int.Parse(localSquare));
                            results.Add(int.Parse(anagramSquare));
                        }

                    }
                }

            }

            Console.Out.WriteLine("Result: {0}", results.Max());

        }

        public void Problem99()
        {
            var exps = File.ReadAllLines(Constants.RESOURCE_LOCATION + "p099_base_exp.txt").Select(c => c.Split(',')).ToArray();

            var greatest = 0.0;
            var result = 0;

            for (var i = 0; i < exps.Count(); i++)
            {

                var a = int.Parse(exps[i][0]);
                var b = int.Parse(exps[i][1]);

                var log = b * Math.Log(a);

                if (log > greatest)
                {
                    greatest = log;
                    result = i;
                }

                Console.Out.WriteLine("{0}, {1} = {2}", a, b, log);

            }

            Console.Out.WriteLine(result);//solved

        }

        /**
         * If a box contains twenty-one coloured discs, composed of fifteen blue discs and six red discs, 
         * and two discs were taken at random, it can be seen that the probability of taking two blue discs, 
         * P(BB) = (15/21)×(14/20) = 1/2.
         * 
         * The next such arrangement, for which there is exactly 50% chance of taking two blue discs at random, 
         * is a box containing eighty-five blue discs and thirty-five red discs.
         * 
         * By finding the first arrangement to contain over 10^12 = 1,000,000,000,000 
         * discs in total, determine the number of blue discs that the box would contain.
         * 
         * looks like x/n ~ root(1/2), which screams continued fraction
         *
         * x	    n	    x/n	        (x/n)^2	    SQRT(0.5)	    (x/n)^2 - SQRT(0.5)
         * 3	    4	    0.75	    0.5625	    0.7071067812	0.042893218813
         * 15	    21	    0.714285714	0.510204082	0.7071067812	0.007178933099
         * 85	    120	    0.708333333	0.501736111	0.7071067812	0.001226552147
         * 493	    697	    0.707317073	0.500297442	0.7071067812	0.000210291984
         * 2871	    4060	0.707142857	0.50005102	0.7071067812	0.000036075956
         * 16731	23661	0.707112971	0.500008753	0.7071067812	0.000006189525
         * 
         */
        public void Problem100()
        {

            var limit = _lib.ToThePower(new BigInteger(10), 6);

            Console.Out.WriteLine("Limit is {0}", limit);

            _lib.GetRecurringPartialFractions(0.5, 20, true);

            //n is number of discs in total
            for (BigInteger n = 2; n < limit; n++)
            {
                var targetX = (n * n - n) / 2;

                for (var x = n / 2; x < n; x++)
                {
                    var xN = x * x - x;

                    if (xN != targetX) continue;

                    var h = new Fraction(x, n).MultiplyFraction(new Fraction(x - 1, n - 1)).MinimiseFraction();

                    Console.Out.WriteLine("X={0}, N={1}, target = {2}, Fraction: {3}", x, n, targetX, h);

                    break;
                }

                //if (found) break;
            }

        }


        public void TestZipNumbers()
        {

            Console.Out.WriteLine("Zip 0 and 0, expected 0, got {0}", ZipNumbers(0, 0));
            Console.Out.WriteLine("Zip 1 and 0, expected 10, got {0}", ZipNumbers(1, 0));
            Console.Out.WriteLine("Zip 0 and 1, expected 1, got {0}", ZipNumbers(0, 1));
            Console.Out.WriteLine("Zip 123 and 1, expected 1123, got {0}", ZipNumbers(123, 1));
            Console.Out.WriteLine("Zip 11 and 123, expected 11123, got {0}", ZipNumbers(11, 123));

            Console.Out.WriteLine("Zip 1111111111 and 123111111111, expected 0, got {0}", ZipNumbers(1111111111, 1231111111));
        }

        public int ZipNumbers(int a, int b)
        {
            var aArray = a.ToString(CultureInfo.InvariantCulture).ToCharArray();

            var bArray = b.ToString(CultureInfo.InvariantCulture).ToCharArray();

            var length = aArray.Length > bArray.Length ? aArray.Length : bArray.Length;

            var cList = new List<char>();

            for (var i = 0; i < length; i++)
            {
                if (i < aArray.Length)
                {
                    cList.Add(aArray[i]);
                }

                if (i < bArray.Length)
                {
                    cList.Add(bArray[i]);
                }
            }

            var s = new StringBuilder();

            cList.ForEach(x => s.Append(x));

            int c;

            try
            {
                c = int.Parse(s.ToString());
            }
            catch (OverflowException)
            {

                c = 0;
            }


            if (c > 100000000)
            {
                c = 0;
            }

            return c;

        }


        public void TestCodility()
        {

            Console.Out.WriteLine("Exepected index: 2, Actual: {0}", Codility(new[] { -1, 1, 3, 1, 1, 1 }));

        }

        public int Codility(int[] a)
        {

            int acount = 0;

            int count = 0;

            int middle = a.Length / 2;

            for (int i = 0; i < a.Length; i++)
            {
                count += a[i];

                acount++;

                var innerCount = 0;

                for (int j = i + 1; j < a.Length; j++)
                {
                    innerCount += a[j];

                    acount++;
                }

                if (count == innerCount)
                {
                    Console.Out.WriteLine("Equlibrium found at index {0}", i);
                }

            }

            Console.Out.WriteLine("Size of a: {0}, num ops: {1}", a.Length, acount);


            return -1;
        }

    }
}
