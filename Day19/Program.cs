using SharedKernel;
using System.Net.Http.Headers;

namespace Day19
{
    internal class Program
    {
        static internal Dictionary<(string, string), Int64> cache = new Dictionary<(string, string), Int64>();
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 19: Linen Layout"));
            Console.WriteLine("Designs: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            List<string> towels = puzzleInput.BlockLines[0][0].Split(',').Select(x => x.Trim()).ToList();
            List<string> designs = puzzleInput.BlockLines[1];

            Int64 possible = 0;
            foreach (string design in designs)
            {
                if (isDesignPossible(towels, design, false) > 0) { possible++; }
            }
            Console.WriteLine("Designs possible: {0}", possible);

            cache.Clear();
            possible = 0;
            foreach (string design in designs)
            {
                possible += isDesignPossible(towels, design, true);
            }
            Console.WriteLine("Unique combinations: {0}", possible);
        }

        private static Int64 isDesignPossible(List<string> towels, string design, bool uniqueDesign)
        {
            Int64 possible = 0;

            foreach (string towel in towels)
            {
                if (design.StartsWith(towel))
                {
                    Int64 addPossible = 0;
                    if(cache.ContainsKey((design, towel)))
                    {
                        addPossible = cache[(design, towel)];
                    }
                    else
                    {
                        string leftOverDesign = design.Substring(towel.Length);
                        if (leftOverDesign == "") { addPossible = 1; }
                        else
                        {
                            if (uniqueDesign || possible == 0) { addPossible += isDesignPossible(towels, leftOverDesign, uniqueDesign); }
                        }

                        cache[(design, towel)] = addPossible;
                    }
                    
                    possible += addPossible;
                }
            }

            return possible;
        }
    }
}
