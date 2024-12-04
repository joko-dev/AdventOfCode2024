using SharedKernel;
using System.Text.RegularExpressions;

namespace Day03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 03: Mull It Over"));
            Console.WriteLine("memory: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            Console.WriteLine("Multiplication results: {0}", parseMuliplications( puzzleInput.FullText, false));
            Console.WriteLine("Multiplication results with do: {0}", parseMuliplications(puzzleInput.FullText, true));
        }

        private static int parseMuliplications(string puzzleInput, bool enableDoStatements)
        {
            // Source because too unfamiliar with regex: https://github.com/ldorval/AdventOfCode2024/blob/main/Day03/Day03.cs, updated to more correct expression
            var matches = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)").Matches(puzzleInput);
            var sum = 0;
            var mulEnabled = true;

            foreach (Match match in matches)
            {
                if (match.Groups[0].Value.StartsWith("mul("))
                    sum += mulEnabled ? int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value) : 0;
                else if (match.Groups[0].Value == "do()" && enableDoStatements)
                    mulEnabled = true;
                else if (match.Groups[0].Value == "don't()" && enableDoStatements)
                    mulEnabled = false;
            }

            return sum;
        }
    }
}
