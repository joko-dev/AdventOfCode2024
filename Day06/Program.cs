using SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Day06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 6: Guard Gallivant"));
            Console.WriteLine("Map: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);
            
            List<Coordinate> coordinates = getGuardPositions(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null), out bool isLoop ); 
            Console.WriteLine("Distinct positions: {0}", coordinates.Count);

            Console.WriteLine("Diffent positions for obstruction: {0}", getCountObstructions(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null), coordinates));
        }

        private static int getCountObstructions(char[,] map, List<Coordinate> possibleConstructions)
        {
            int countObstructions = 0;

            foreach (Coordinate possible in possibleConstructions)
            {
                if (map[possible.X, possible.Y] != '^')
                {
                    char[,] copyMap = map.Clone() as char[,];
                    copyMap[possible.X, possible.Y] = '#';

                    getGuardPositions(copyMap, out bool isLoop);
                    if (isLoop) { countObstructions++; }
                }
            }

            return countObstructions;
        }

        private static List<Coordinate> getGuardPositions(char[,] map, out bool isLoop)
        {
            List<Coordinate> positions = new List<Coordinate>();
            List<Move> moves = new List<Move>();

            isLoop = false;
            Move currentPoint = getStartingPoint(map);
            
            do
            {
                if (!positions.Contains(currentPoint.Coordinate))
                {
                    positions.Add(currentPoint.Coordinate);
                }
                if (!moves.Contains(currentPoint))
                {
                    moves.Add(currentPoint);
                }
                else
                {
                    isLoop = true;
                    break;
                }

                bool checkNextPoint = true;
                while(checkNextPoint)
                {
                    Move nextPoint = currentPoint.MoveToDirection();

                    if (nextPoint.Coordinate.IsInMatrix(map))
                    {
                        if (map[nextPoint.Coordinate.X, nextPoint.Coordinate.Y] == '#')
                        {
                            currentPoint = currentPoint.RotateRight();
                        }
                        else { checkNextPoint = false; }
                    }
                    else { checkNextPoint = false; }
                }

                currentPoint = currentPoint.MoveToDirection();
            }
            while (currentPoint.Coordinate.IsInMatrix(map));

            return positions;
        }

        private static Move getStartingPoint(char[,] map)
        {         
            for(int y = 0; y < map.GetLength(1); y++)
            {
                for(int x = 0; x < map.GetLength(0); x++)
                {
                    if(map[x, y] == '^')
                    {
                        return new Move(new Coordinate(x, y), Move.DirectionType.Up);
                    }
                }
            }

            throw new InvalidDataException();
        }
    }
}
