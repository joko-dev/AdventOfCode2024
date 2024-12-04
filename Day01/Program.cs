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
            Console.WriteLine("Lists: ");
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

            Dictionary<int, int> similarityScores = getSimilarityScores( left,  right);
            Console.WriteLine("similarity score: {0}", similarityScores.Sum( s => s.Value));
        }

        private static Dictionary<int, int> getSimilarityScores(List<int> left, List<int> right)
        {
            Dictionary<int, int> similarityScores = new Dictionary<int, int>();

            foreach (int i in left)
            {
                // overkill to calculate the score each time
                if (!similarityScores.ContainsKey(i))
                {
                    similarityScores[i] = 0;
                }
                similarityScores[i] += i * right.Where( s => s == i).Count();
            }
            return similarityScores;
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