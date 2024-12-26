using SharedKernel;

namespace Day19
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 19: Linen Layout"));
            Console.WriteLine("Designs: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            List<string> towels = puzzleInput.BlockLines[0][0].Split(',').Select(x => x.Trim()).ToList();
            List<string> designs = puzzleInput.BlockLines[1];

            int possible = 0;
            foreach (string design in designs)
            {
                if (isDesignPossible(towels, design, false) > 0) { possible++; }
            }
            Console.WriteLine("Designs possible: {0}", possible);

            possible = 0;
            foreach (string design in designs)
            {
                possible += isDesignPossible(towels, design, true);
            }
            Console.WriteLine("Unique combinations: {0}", possible);
        }

        private static int isDesignPossible(List<string> towels, string design, bool uniqueDesign)
        {
            int possible = 0;

            foreach (string towel in towels)
            {
                if (design.StartsWith(towel))
                {
                    string leftOverDesign = design.Substring(towel.Length);
                    if (leftOverDesign == "") { possible += 1; }
                    else 
                    {   if (uniqueDesign || possible == 0) { possible += isDesignPossible(towels, leftOverDesign, uniqueDesign); }
                    }
                }
            }

            return possible;
        }
    }
}
