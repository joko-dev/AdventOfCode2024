using SharedKernel;
using SharedKernel.Djikstra;
using System.Diagnostics.Metrics;

namespace Day18
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 18: RAM Run"));
            Console.WriteLine("Incoming bytes: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            DjikstraSolver solver = getDikstraSolver(getCorruptedBytes(puzzleInput.Lines, 1024), 70);
            solver.Solve(new Coordinate(0,0));
            Console.WriteLine("Minimum steps: {0}", solver.GetDistance(new Coordinate(70,70)));

            int steps = 2800;
            Coordinate coordinateBlocked = null;
            while (steps <= puzzleInput.Lines.Count && coordinateBlocked == null)
            {
                solver = getDikstraSolver(getCorruptedBytes(puzzleInput.Lines, steps), 70);
                solver.Solve(new Coordinate(0, 0));
                if(solver.GetDistance(new Coordinate(70, 70)) == null)
                {
                    coordinateBlocked = new Coordinate(puzzleInput.Lines[steps-1]);
                }
                steps++;
            }

            Console.WriteLine("blocked coordinate: {0},{1}", coordinateBlocked.X, coordinateBlocked.Y);
        }

        private static DjikstraSolver getDikstraSolver(List<Coordinate> corruptedBytes, int size)
        {
            List<DjikstraCoordinates> djikstraCoordinates = new List<DjikstraCoordinates>();
            for (int y = 0; y <= size; y++) 
            {
                for (int x = 0; x <= size; x++)
                {
                    Coordinate coordinate = new Coordinate(x, y);
                    if (!corruptedBytes.Contains(coordinate))
                    {
                        DjikstraCoordinates djikstra = new DjikstraCoordinates(coordinate);
                        foreach(Coordinate adjacent in coordinate.GetAdjacentCoordinatesCardinalPoints())
                        {
                            if(adjacent.X >= 0 && adjacent.X <= size && adjacent.Y >= 0 && adjacent.Y <= size && !corruptedBytes.Contains(adjacent))
                            {
                                djikstra.AddNeighbour(new DjikstraNeighbour(adjacent, 1));
                            }
                        }
                        
                        djikstraCoordinates.Add(djikstra);
                    }
                }
            }

            //for (int y = 0; y <= size; y++)
            //{
            //    Console.Write("\n");
            //    for (int x = 0; x <= size; x++)
            //    {
            //        if(corruptedBytes.Contains(new Coordinate(x, y))) { Console.Write("#"); }
            //        else {  Console.Write("."); }
            //    }
            //}

            return new DjikstraSolver(djikstraCoordinates);
        }

        private static List<Coordinate> getCorruptedBytes(List<string> lines, int? steps)
        {
            List<Coordinate> result = new List<Coordinate>();
            int counter = 1;

            foreach (string line in lines)
            {
                result.Add(new Coordinate(line));
                counter++;
                if(steps != null && counter > steps) {  break; }
            }

            return result;
        }
    }
}
