using SharedKernel;
using System.Runtime.CompilerServices;

namespace Day08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 8: Resonant Collinearity"));
            Console.WriteLine("Map: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<Coordinate> uniqueAntinodes = getUniqueAntinodes(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null));
            Console.WriteLine("unique antinodes: {0}", uniqueAntinodes.Count);

            uniqueAntinodes = getUniqueAntinodes(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null), true);
            Console.WriteLine("unique antinodes with resonance harmonics: {0}", uniqueAntinodes.Count);
        }

        private static List<Coordinate> getUniqueAntinodes(char[,] map, bool useResonantHarmonics = false)
        {
            List<Coordinate> result = new List<Coordinate>();

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] != '.')
                    {
                        Coordinate toCheck = new Coordinate(x, y);
                        List<Coordinate> antinodes = getAntinodes(map, toCheck, useResonantHarmonics);

                        foreach (Coordinate antinode in antinodes)
                        {
                            if (!result.Contains(antinode))
                            {
                                result.Add(antinode);
                            }
                        }
                    }
                }

            }

            foreach (Coordinate antinode in result)
            {
                map[antinode.X, antinode.Y] = '#';
            }

            foreach(string line in PuzzleOutputFormatter.outputMap(map))
            {
                Console.WriteLine(line);
            }
            

            return result;
        }

        private static List<Coordinate> getAntinodes(char[,] map, Coordinate toCheck, bool useResonantHarmonics = false)
        {
            List<Coordinate> result = new List<Coordinate>();
            char value = map[toCheck.X, toCheck.Y];

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y] == value && x != toCheck.X && y != toCheck.Y)
                    {
                        Coordinate newStartingPoint = new Coordinate(x, y);
                        CoordinateVector vector = new CoordinateVector(toCheck, newStartingPoint);

                        if(useResonantHarmonics)
                        {
                            result.Add(newStartingPoint);
                        }

                        bool getNextAntinode = true;

                        while (getNextAntinode)
                        {
                            Coordinate antinode = CoordinateVector.Add(vector, newStartingPoint);

                            if (antinode.IsInMatrix(map))
                            {
                                result.Add(antinode);
                                newStartingPoint = antinode;
                            }
                            else
                            {
                                getNextAntinode = false;
                            }

                            if (!useResonantHarmonics)
                            {
                                getNextAntinode = false;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}