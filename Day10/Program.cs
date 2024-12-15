using SharedKernel;

namespace Day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 10: Hoof It"));
            Console.WriteLine("topographic map: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            int[,] map = PuzzleConverter.getInputAsMatrixInt(puzzleInput.Lines);

            Console.WriteLine("trailhead score: {0}", getTrailheadScores(map, false));
            Console.WriteLine("trailhead score unique paths: {0}", getTrailheadScores(map, true));
        }

        private static int getTrailheadScores(int[,] map, bool uniqueTrails)
        {
            int trailheadScores = 0;

            for(int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x,y] == 0)
                    {
                        List<Coordinate> reachedTops = new List<Coordinate>();
                        trailheadScores += getTrailheadScore(map, new Coordinate(x, y), reachedTops, uniqueTrails);
                    }
                }
            }

            return trailheadScores;
        }

        private static int getTrailheadScore(int[,] map, Coordinate coordinate, List<Coordinate> reachedTops, bool uniqueTrails)
        { 
            int trailheadScores = 0;

            if(coordinate.IsInMatrix(map) && map[coordinate.X, coordinate.Y] == 9)
            {
                if (uniqueTrails)
                {
                    trailheadScores = 1;
                }
                else
                {
                    if (!reachedTops.Contains(coordinate))
                    {
                        trailheadScores = 1;
                        reachedTops.Add(coordinate);
                    }
                }
            }
            else
            {
                foreach (Coordinate adjacent in coordinate.GetAdjacentCoordinatesCardinalPoints())
                {
                    if (adjacent.IsInMatrix(map) && map[adjacent.X, adjacent.Y] == map[coordinate.X, coordinate.Y] + 1)
                    {
                        trailheadScores += getTrailheadScore(map, adjacent, reachedTops, uniqueTrails);
                    }
                }
            }

            return trailheadScores;
        }
    }
}
