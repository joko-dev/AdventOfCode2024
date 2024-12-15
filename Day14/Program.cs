using SharedKernel;
using SkiaSharp;
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

            Console.WriteLine("Print bitmaps? (y,n)");
            Console.WriteLine("christmas tree: {0}", getSecondsForChristmasTree(robots, spaceWide, spaceTall, Console.Read() == 'y'));
        }

        private static int getSecondsForChristmasTree(List<Robot> robots, int spaceWide, int spaceTall, bool printBitmaps)
        {
            bool isChristmasTree = false;
            int seconds = 0;
            int middleIndexHorizontal = (int)Math.Floor((double)spaceWide / 2);

            do
            {
                seconds++;
                List<Coordinate> current = moveRobots(robots, spaceWide, spaceTall, seconds);

                if (printBitmaps)
                {
                    var bmap = new SKBitmap(spaceWide, spaceTall, false);
                    var canvas = new SKCanvas(bmap);
                    canvas.Clear(SKColors.White);

                    foreach (Coordinate c in current)
                    {
                        bmap.SetPixel((int)c.X, (int)c.Y, SKColors.Black);
                    }

                    using (var image = SKImage.FromBitmap(bmap))
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                    using (var stream = File.OpenWrite(DateTime.Now.ToString().Replace(":", "") + seconds.ToString().PadLeft(4, '0') + ".png"))
                    {
                        data.SaveTo(stream);
                    }
                }

                if (current.Distinct().Count() == robots.Count)
                {
                    isChristmasTree = true;
                }

            }
            while (!isChristmasTree);

            return seconds;
        }

        private static int getSafetyFactor(List<Robot> robots, int spaceWide, int spaceTall, int seconds)
        {
            List<int> quadrant = new List<int> { 0, 0, 0, 0 };
            int middleIndexHorizontal = (int)Math.Floor((double)spaceWide / 2);
            int middleIndexVertical = (int)Math.Floor((double)spaceTall / 2);

            moveRobots(robots, spaceWide, spaceTall, seconds);

            foreach (Coordinate position in moveRobots(robots, spaceWide, spaceTall, seconds))
            {
                if (position.X < middleIndexHorizontal && position.Y < middleIndexVertical) { quadrant[0]++; }
                else if (position.X > middleIndexHorizontal && position.Y < middleIndexVertical) { quadrant[1]++; }
                else if (position.X < middleIndexHorizontal && position.Y > middleIndexVertical) { quadrant[2]++; }
                else if (position.X > middleIndexHorizontal && position.Y > middleIndexVertical) { quadrant[3]++; }
            }

            return quadrant.Aggregate(1, (x, y) => x * y);
        }

        private static List<Coordinate> moveRobots(List<Robot> robots, int spaceWide, int spaceTall, int seconds)
        {
            List<Coordinate> movedRobots = new List<Coordinate>();

            foreach (Robot robot in robots)
            {
                Int64 newX = robot.Position.X + (seconds * robot.Vector.X);
                Int64 newY = robot.Position.Y + (seconds * robot.Vector.Y);

                newX = nfmod(newX, spaceWide);
                newY = nfmod(newY, spaceTall);

                movedRobots.Add(new Coordinate(newX, newY));
            }

            return movedRobots;
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
