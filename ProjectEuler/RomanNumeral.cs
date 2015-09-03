using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication1
{
    /**
    * In order for a number written in Roman numerals to be considered valid there are three basic rules which must be followed:
    * 
    * i.   Numerals must be arranged in descending order of size.
    * ii.  M, C, and X cannot be equalled or exceeded by smaller denominations.
    * iii. D, L, and V can each only appear once.
    * 
    * Subtractive rules:
    * 
    * i.   Only one I, X, and C can be used as the leading numeral in part of a subtractive pair.
    * ii.  I can only be placed before V and X.
    * iii. X can only be placed before L and C.
    * iv.  C can only be placed before D and M.
    * 
    */
    public class RomanNumeral
    {
        public readonly String Numeral;
        public readonly int IntValue;
        public readonly bool ContainsSubtractive;
        public readonly bool IsNumeralValid;
        public string Comment;

        private static readonly Dictionary<String, String> ValidSubtractions = new Dictionary<string, string>
            {
                {"IV","IIII"},
                {"IX","VIIII"},
                {"XL","XXXX"},
                {"XC","LXXXX"},
                {"CD","CCCC"},
                {"CM","DCCCC"}
            };

        private static readonly Dictionary<char, int> RomanNumeralsValues = new Dictionary<char, int> {
                {'I', 1},
                {'V', 5},
                {'X', 10},
                {'L', 50},
                {'C', 100},
                {'D', 500},
                {'M', 1000}
            };

        #region constructors
        public RomanNumeral(String romanNumeral)
        {
            Numeral = romanNumeral;
            IsNumeralValid = ValidateRomanNumeralString(Numeral, out Comment);
            ContainsSubtractive = NumeralContainsSubtractive(Numeral);
            IntValue = ParseIntegerValueFromNumeralString();
        }

        public RomanNumeral(int input)
        {
            IntValue = input;
            Numeral = CreateRomanNumeralStringFromIntegerValue();
            ContainsSubtractive = NumeralContainsSubtractive(Numeral);
            IsNumeralValid = ValidateRomanNumeralString(Numeral, out Comment);

        }

        #endregion constructors

        #region parsers
        private int ParseIntegerValueFromNumeralString()
        {

            if (!IsNumeralValid) return -1;

            //check if this contains a subtractive construct
            if (!ContainsSubtractive) return Numeral.ToCharArray().Sum(character => RomanNumeralsValues[character]);
            
            var numeralWithoutSubtractions = Numeral;

            //change the string
            foreach (var validSubstitution in ValidSubtractions.Keys)
            {
                if (numeralWithoutSubtractions.Contains(validSubstitution))
                {
                    numeralWithoutSubtractions = numeralWithoutSubtractions.Replace(validSubstitution, ValidSubtractions[validSubstitution]);
                }
            }

            return numeralWithoutSubtractions.ToCharArray().Sum(character => RomanNumeralsValues[character]);
        }

        private string CreateRomanNumeralStringFromIntegerValue()
        {
            var start = IntValue;
            var retval = "";

            foreach (var romanNumeral in RomanNumeralsValues.Keys.Reverse())
            {
                int rem;

                var divisor = Math.DivRem(start, RomanNumeralsValues[romanNumeral], out rem);

                if (divisor == 0) continue;

                for (int i = 0; i < divisor; i++)
                {
                    retval += romanNumeral;
                }

                start = rem;
            }

            foreach (var validSubtraction in ValidSubtractions.Keys.Reverse())
            {
                if (retval.Contains(ValidSubtractions[validSubtraction]))
                {
                    var pRetval = retval.Replace(ValidSubtractions[validSubtraction], validSubtraction);

                    string comment;
                    if (ValidateRomanNumeralString(pRetval, out comment))
                    {
                        retval = pRetval;
                    }
                }
            }

            return retval;
        }

        #endregion parsers

        #region class methods
        private static bool ValidateRomanNumeralString(String input, out string outComment)
        {
            outComment = "";

            // must be populated
            if (string.IsNullOrEmpty(input)) return false;


            // can only contain the characters we know about
            if (!input.ToCharArray().All(character => RomanNumeralsValues.ContainsKey(character)))
            {
                outComment = "Numeral String contains Invalid character.";
                return false;
            }

            // iii. D, L, and V can only appear once
            if (
                new[] { 'D', 'L', 'V' }.Where(character => input.IndexOf(character) != -1)
                    .Any(character => input.LastIndexOf(character) != input.IndexOf(character)))
            {
                outComment = "Numeral String contains more than one {D|L|V}";
                return false;
            }

            // ii.  M, C, and X cannot be equalled or exceeded by smaller denominations.
            var characterCount = new Dictionary<char, int>();

            foreach (var character in input.ToCharArray())
            {
                if (characterCount.ContainsKey(character))
                {
                    characterCount[character]++;
                }
                else
                {
                    characterCount.Add(character, 1);
                }
            }

            foreach (var character in characterCount.Keys)
            {
                var characterSumValue = characterCount[character] * RomanNumeralsValues[character];

                foreach (var targetCharacter in new[] { 'M', 'C', 'X' })
                {
                    if (character.Equals(targetCharacter) &&
                        characterSumValue == RomanNumeralsValues[targetCharacter]) continue;

                    if (!character.Equals(targetCharacter) && RomanNumeralsValues[character] < RomanNumeralsValues[targetCharacter] &&
                        characterSumValue >= RomanNumeralsValues[targetCharacter])
                    {
                        outComment = "String contains M, C, or X equalled or exceeded by smaller denominations: " + character + " ," + targetCharacter;
                        return false;
                    }
                }
            }

            // i.   Numerals must be arranged in descending order of size. (unless there is a subtraction)
            //todo: validation rules for a subtraction

            return true;
        }

        private static bool NumeralContainsSubtractive(string input)
        {
            return ValidSubtractions.Keys.Any(input.Contains);
        }

        #endregion class methods
    }
}
