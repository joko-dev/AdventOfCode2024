using SharedKernel;

namespace Day20
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 20: Race Condition"));
            Console.WriteLine("racetrack: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            Console.WriteLine("Cheats saving at least 100 picoseconds: {0}", getCheatCount(getRaceTrack(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null)), 2));
        }

        private static int getCheatCount(List<Coordinate> track, int cheatSeconds)
        {
            int cheatCount = 0;

            for (int i = 0; i < track.Count; i++)
            {
                Coordinate currentPosition = track[i];
                foreach (Coordinate adjacent in currentPosition.GetAdjacentCoordinatesCardinalPoints())
                {
                    foreach (Coordinate possibleShortcut in adjacent.GetAdjacentCoordinatesCardinalPoints())
                    {
                        int positionPossibleShortcut = track.FindIndex( t => t.Equals(possibleShortcut ));
                        if (!possibleShortcut.Equals(currentPosition) && positionPossibleShortcut > -1)
                        {
                            if (positionPossibleShortcut - i - cheatSeconds >= 100)
                            {
                                cheatCount++;
                            }
                        }
                    }
                }
            }

            return cheatCount;
        }
        private static List<Coordinate> getRaceTrack(char[,] map)
        {
            List<Coordinate> track = new List<Coordinate>();
            Coordinate nextCoordinate = PuzzleConverter.findValueInMatrix(map, 'S');
            Coordinate previousCoordinate = null;

            do
            {

                Coordinate current = nextCoordinate;
                track.Add(current);
                nextCoordinate = null;

                foreach(Coordinate adjacent in current.GetAdjacentCoordinatesCardinalPoints())
                {
                    if(adjacent.IsInMatrix(map) && map[adjacent.X, adjacent.Y] != '#' && !adjacent.Equals(previousCoordinate))
                    {
                        nextCoordinate = adjacent;
                        previousCoordinate = current;
                        break;
                    }
                }

            } while (nextCoordinate != null);

            return track;
        }
    }
}
