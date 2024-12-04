using SharedKernel;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Day01
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 01: Historian Hysteria"));
            Console.WriteLine("Calibration document: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<int> left;
            List<int> right;

            getLocationIds(puzzleInput.Lines, out left, out right);
            
            left.Sort();
            right.Sort();

            int totalDistance = 0;
            for (int i = 0; i < left.Count; i++)
            {
                totalDistance += Math.Abs(left[i] - right[i]);
            }

            Console.WriteLine("total distance: {0}", totalDistance);

        }

        private static void getLocationIds(List<string> lines, out List<int> left, out List<int> right)
        {
            left = new List<int>();
            right = new List<int>();   

            foreach (string line in lines)
            {
                string[] values = Regex.Replace(line, @"\s+", " ").Split(' ');
                left.Add(Convert.ToInt32(values[0]));
                right.Add(Convert.ToInt32(values[1]));
            }

        }
    }
}