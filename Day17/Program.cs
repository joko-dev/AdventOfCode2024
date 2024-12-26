using SharedKernel;

namespace Day17
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 17: Chronospatial Computer"));
            Console.WriteLine("Map: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            // Console.WriteLine("Lowest score: {0}, TileCount: {1}", result.Score, result.TileCount);
        }
    }
}
