using SharedKernel;
using System.Linq;
using System.Security.Cryptography;
using static SharedKernel.Move;

namespace Day15
{
    internal class Program
    {
        private class DoubleCoordinate
        {
            public Coordinate LeftSide { get; private set; }
            public Coordinate RightSide { get; private set; }

            public DoubleCoordinate(DoubleCoordinate original)
            {
                this.LeftSide = new Coordinate(original.LeftSide);
                this.RightSide = new Coordinate(original.RightSide);
            }

            public DoubleCoordinate(Coordinate leftSide)
            {
                this.LeftSide = leftSide;
                this.RightSide = leftSide.GetAdjacentCoordinate(DirectionType.Right);
            }

            public bool ContainsCoordinate(Coordinate toCheck)
            {
                return LeftSide.Equals(toCheck) || RightSide.Equals(toCheck);
            }

            public void Move(DirectionType direction)
            {
                LeftSide.Move(direction);
                RightSide.Move(direction);
            }

            public bool OverlapDoubleCoordinate(DoubleCoordinate toCheck)
            {
                return LeftSide.Equals(toCheck.LeftSide) || LeftSide.Equals(toCheck.RightSide) || RightSide.Equals(toCheck.LeftSide) || RightSide.Equals(toCheck.RightSide);
            }

            public override bool Equals(object obj)
            {
                var coordinate = obj as DoubleCoordinate;
                if (coordinate == null)
                {
                    return false;
                }
                return (this.LeftSide.Equals(coordinate.LeftSide) && this.RightSide.Equals(coordinate.RightSide));
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 15: Warehouse Woes"));
            Console.WriteLine("Robot movements: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            List<Coordinate> boxes = moveRobot(PuzzleConverter.getInputAsMatrixChar(puzzleInput.BlockLines[0], null), puzzleInput.BlockLines[1]);
            Console.WriteLine("Sum of coordinats: {0}", boxes.Select( b => (100 * b.Y) + b.X).Sum());

            List<DoubleCoordinate> boxesDouble = moveRobotDoubleSize(PuzzleConverter.getInputAsMatrixChar(puzzleInput.BlockLines[0], null), puzzleInput.BlockLines[1]);
            Console.WriteLine("Sum of coordinats double: {0}", boxesDouble.Select(b => (100 * b.LeftSide.Y) + b.LeftSide.X).Sum());
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
        private static List<DoubleCoordinate> moveRobotDoubleSize(char[,] map, List<string> moves)
        {
            Coordinate robot = PuzzleConverter.findValueInMatrix(map, '@');
            List<Coordinate> boxes = PuzzleConverter.getCoordinatesForValueInMatrix(map, 'O');
            List<Coordinate> walls = PuzzleConverter.getCoordinatesForValueInMatrix(map, '#');

            List<DoubleCoordinate> doubleBoxes = doubleSizeCoordinates(boxes);
            List<DoubleCoordinate> doubleWalls = doubleSizeCoordinates(walls);
            robot = new Coordinate(robot.X * 2, robot.Y);

            foreach (char move in string.Join("", moves))
            {
                DirectionType direction = Move.CreateDirectionFromChar(move);

                Coordinate nextCoordinate = robot.GetAdjacentCoordinate(direction);

                if(doubleBoxes.Any(b => b.ContainsCoordinate(nextCoordinate)))
                {
                    bool checkNextCoordinate = true;
                    bool moveDoubleBoxes = true;
                    List<DoubleCoordinate> toMove = new List<DoubleCoordinate>();
                    toMove.Add(doubleBoxes.First(b => b.ContainsCoordinate(nextCoordinate)));

                    while (checkNextCoordinate)
                    {
                        List<DoubleCoordinate> toAddCoordinates = new List<DoubleCoordinate>();

                        foreach (DoubleCoordinate box in toMove)
                        {
                            DoubleCoordinate nextDouble = new DoubleCoordinate(box);
                            nextDouble.Move(direction);

                            if (doubleWalls.Any( w => w.OverlapDoubleCoordinate(nextDouble)))
                            {
                                checkNextCoordinate = false;
                                moveDoubleBoxes = false;
                            }
                            else
                            {
                                foreach (DoubleCoordinate nextBox in doubleBoxes.Where( b => b.OverlapDoubleCoordinate(nextDouble) ).ToList())
                                {
                                    if (!toMove.Contains(nextBox))
                                    {
                                        if (!toAddCoordinates.Contains(nextBox))
                                        {
                                            toAddCoordinates.Add(nextBox);
                                        }
                                    }                                
                                }
                            }
                        }

                        checkNextCoordinate = checkNextCoordinate && toAddCoordinates.Count > 0;
                        toMove.AddRange(toAddCoordinates);
                    }

                    if (moveDoubleBoxes)
                    {
                        robot.Move(direction);
                        // wList<DoubleCoordinate> boxesToMove = doubleBoxes.Where(b => toMove.Contains(b)).ToList();
                        foreach (DoubleCoordinate box in toMove)
                        {
                            box.Move(direction);
                        }
                    }

                }
                else if (!doubleWalls.Any(w => w.ContainsCoordinate(nextCoordinate)))
                {
                    robot.Move(direction);
                }


                

                //Console.Write("\n Move: {0}", move.ToString());

                //for (int y = 0; y < map.GetLength(1); y++)
                //{
                //    Console.Write("\n");
                //    for (int x = 0; x < map.GetLength(0) * 2; x++)
                //    {
                //        if (doubleBoxes.Any( b => b.ContainsCoordinate(new Coordinate(x,y)))) { Console.Write("O"); }
                //        else if (doubleWalls.Any(b => b.ContainsCoordinate(new Coordinate(x, y)))) { Console.Write("#"); }
                //        else if (robot.Equals(new Coordinate(x, y))) { Console.Write("@"); }
                //        else { Console.Write("."); }
                //    }
                //}

            }

            return doubleBoxes;
        }

        private static List<DoubleCoordinate> doubleSizeCoordinates(List<Coordinate> coordinates)
        {
            List<DoubleCoordinate> doubleSize = new List<DoubleCoordinate>();

            for (int i = 0; i < coordinates.Count; i++)
            {
                doubleSize.Add(new DoubleCoordinate(new Coordinate(2 * coordinates[i].X, coordinates[i].Y)));
            }

            return doubleSize;
        }
    }
}
