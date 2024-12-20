using SharedKernel;
using System.Security.Cryptography;
using static SharedKernel.Move;

namespace Day15
{
    internal class Program
    {
        private struct DoubleCoordinate
        {
            public Coordinate LeftSide { get; private set; }
            public Coordinate RightSide { get; private set; }

            public DoubleCoordinate(Coordinate leftSide)
            {
                this.LeftSide = leftSide;
                this.RightSide = leftSide.GetAdjacentCoordinate(DirectionType.Right);
            }

             
        }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 15: Warehouse Woes"));
            Console.WriteLine("Robot movements: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            List<Coordinate> boxes = moveRobot(PuzzleConverter.getInputAsMatrixChar(puzzleInput.BlockLines[0], null), puzzleInput.BlockLines[1]);
            Console.WriteLine("Sum of coordinats: {0}", boxes.Select( b => (100 * b.Y) + b.X).Sum());


        }

        private static List<Coordinate> moveRobot(char[,] map, List<string> moves)
        {
            Coordinate robot = PuzzleConverter.findValueInMatrix(map, '@');
            List<Coordinate> boxes = PuzzleConverter.getCoordinatesForValueInMatrix(map, 'O');
            List<Coordinate> walls = PuzzleConverter.getCoordinatesForValueInMatrix(map, '#');

            foreach (char move in string.Join("", moves))
            { 
                DirectionType direction = Move.CreateDirectionFromChar(move);
                bool checkNextCoordinate = true;
                List<Coordinate> toMove = new List<Coordinate>();

                Coordinate currentCoordinate = robot;

                while (checkNextCoordinate)
                {
                    Coordinate nextCoordinate = currentCoordinate.GetAdjacentCoordinate(direction);

                    if (walls.Contains(nextCoordinate))
                    {
                        checkNextCoordinate = false;
                        toMove.Clear();
                    }
                    else if(boxes.Contains(nextCoordinate))
                    {
                        toMove.Add(currentCoordinate);
                    }
                    else
                    {
                        toMove.Add(currentCoordinate);
                        checkNextCoordinate = false;
                    }
                    currentCoordinate = nextCoordinate;
                }

                if (toMove.Count > 0)
                {
                    //first Coordinate is always the robot
                    robot.Move(direction);
                    List<Coordinate> boxesToMove = boxes.Where(b => toMove.Contains(b)).ToList();
                    foreach (Coordinate box in boxesToMove)
                    {
                        box.Move(direction);
                    }
                }

                //Console.Write("\n Move: {0}", move.ToString());

                //for (int y = 0; y < map.GetLength(1); y++)
                //{
                //    Console.Write("\n");
                //    for (int x = 0; x < map.GetLength(0); x++)
                //    {
                //        if (boxes.Contains(new Coordinate(x, y))) { Console.Write("O"); }
                //        else if (walls.Contains(new Coordinate(x, y))) { Console.Write("#"); }
                //        else if (robot.Equals(new Coordinate(x, y))) { Console.Write("@"); }
                //        else { Console.Write("."); }
                //    }
                //}

            }

            return boxes;
        }
    }
}
