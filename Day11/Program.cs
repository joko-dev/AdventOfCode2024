using SharedKernel;

namespace Day11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 11: Plutonian Pebbles"));
            Console.WriteLine("stones: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<Int64> stones = puzzleInput.Lines[0].Split(' ').Select(s => Int64.Parse(s)).ToList();

            Console.WriteLine("trailhead score 25: {0}", blinkAtStones(stones, 25).Count);
            //Console.WriteLine("trailhead score 75: {0}", blinkAtStones(stones, 75).Count);
        }

        private static List<Int64> blinkAtStones(List<Int64> stones, int blinkTimes)
        {
            List<Int64> blinked = stones.ToList();

            for(int b = 0; b < blinkTimes; b++)
            {
                List<Int64> temp = new List<Int64>();
                for(int i = 0; i < blinked.Count; i++)
                {
                    string number = blinked[i].ToString();

                    if (blinked[i] == 0)
                    {
                        temp.Add(1);
                    }
                    else if (number.Length % 2 == 0)
                    {
                        temp.Add(Int64.Parse(number.Substring(0, number.Length / 2)));
                        temp.Add(Int64.Parse(number.Substring(number.Length / 2)));
                    }
                    else
                    {
                        temp.Add(blinked[i] * 2024);
                    }
                }

                blinked = temp;
            }

            return blinked;
        }
    }
}
