using SharedKernel;
using System.Text.RegularExpressions;

namespace Day14
{
    internal class Program
    {
        public struct Robot
        {
            public Coordinate Position { set;  get; }

            public CoordinateVector Vector { get; }

            public Robot(Coordinate position, CoordinateVector vector)
            {
                Position = position;
                Vector = vector;
            }

        }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 14: Restroom Redoubt"));
            Console.WriteLine("Robot list: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<Robot> robots = getRobots(puzzleInput.FullText);
            int spaceWide, spaceTall = 0;
            if(robots.Max(r => r.Position.X) < 11)
            {
                spaceWide = 11;
                spaceTall = 7;
            }
            else
            {
                spaceWide = 101;
                spaceTall = 103;
            }

            Console.WriteLine("safety factor: {0}", getSafetyFactor(robots, spaceWide, spaceTall, 100));
        }

        private static int getSafetyFactor(List<Robot> robots, int spaceWide, int spaceTall, int seconds)
        {
            List<int> quadrant = new List<int>{0, 0, 0, 0 };
            int middleIndexHorizontal = (int) Math.Floor((double)spaceWide / 2);
            int middleIndexVertical = (int)Math.Floor((double)spaceTall / 2); 

            foreach (Robot robot in robots) 
            {
                Int64 newX = robot.Position.X + (seconds * robot.Vector.X);
                Int64 newY = robot.Position.Y + (seconds * robot.Vector.Y);

                newX = nfmod(newX, spaceWide);
                newY = nfmod(newY, spaceTall);

                Coordinate newPosition = new Coordinate(newX, newY);

                if (newPosition.X < middleIndexHorizontal  && newPosition.Y < middleIndexVertical) { quadrant[0]++; }
                else if (newPosition.X > middleIndexHorizontal && newPosition.Y < middleIndexVertical) { quadrant[1]++; }
                else if (newPosition.X < middleIndexHorizontal && newPosition.Y > middleIndexVertical) { quadrant[2]++; }
                else if (newPosition.X > middleIndexHorizontal && newPosition.Y > middleIndexVertical) { quadrant[3]++; }
                
            }

            return quadrant.Aggregate(1, (x, y) => x * y);
        }

        private static Int64 nfmod(float a, float b)
        {
            return (Int64) (a - b * Math.Floor(a / b));
        }

        private static List<Robot> getRobots(string lines)
        {
            List<Robot> robots = new List<Robot>();

            var matches = new Regex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)").Matches(lines);

            foreach (Match match in matches)
            {
                robots.Add(new Robot(new Coordinate(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)), new CoordinateVector(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))));
            }

            return robots;
        }
    }
}
