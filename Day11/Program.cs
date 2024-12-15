using Microsoft.VisualBasic.FileIO;
using SharedKernel;
using System.Collections.Concurrent;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Day11
{
    internal class Program
    {
        static Dictionary<Int64, List<Int64>> stoneCache = new Dictionary<Int64, List<Int64>>();

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 11: Plutonian Pebbles"));
            Console.WriteLine("stones: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<Int64> stones = puzzleInput.Lines[0].Split(' ').Select(s => Int64.Parse(s)).ToList();

            Console.WriteLine("trailhead score 25: {0}", blinkAtStones(stones, 25));
            Console.WriteLine("trailhead score 75: {0}", blinkAtStones(stones, 75));
        }

        private static Int64 blinkAtStones(List<Int64> stones, int blinkTimes)
        {
            Dictionary<Int64, Int64> blinked = new Dictionary<Int64, Int64>();

            foreach (Int64 stone in stones)
            {
                if (blinked.ContainsKey(stone))
                {
                    blinked[stone] += 1;
                }
                else
                {
                    blinked.Add(stone, 1);
                }
            }

            for(int b = 0; b < blinkTimes; b++)
            {
                Dictionary<Int64, Int64> temp = new Dictionary<Int64, Int64>();

                foreach(KeyValuePair<Int64, Int64> pair in blinked)
                {
                    List<Int64> newStones = getStones(pair.Key);
                    foreach (Int64 stone in newStones)
                    {
                        if (!temp.ContainsKey(stone))
                        {
                            temp.Add(stone, 0);
                        }
                        temp[stone] += pair.Value;
                    }
                }

                blinked = temp;
            }

            return blinked.Select( b => b.Value).Sum();
        }

        private static List<Int64> getStones(Int64 stone)
        {
            if (stoneCache.ContainsKey(stone))
            {
                return stoneCache[stone];
            }
            else
            {
                string carving = stone.ToString();
                List<Int64> stones = new List<Int64>();

                if (stone == 0)
                {
                    stones.Add(1);
                }
                else if (carving.Length % 2 == 0)
                {
                    stones.Add(Int64.Parse(carving.Substring(0, carving.Length / 2)));
                    stones.Add(Int64.Parse(carving.Substring(carving.Length / 2)));
                }
                else
                {
                    stones.Add(stone * 2024);
                }

                stoneCache.Add(stone, stones);

                return stones;
            }
   
        }

    }
}
