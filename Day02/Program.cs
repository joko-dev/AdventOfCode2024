using SharedKernel;
using System.Runtime.CompilerServices;

namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 02: Red-Nosed Reports"));
            Console.WriteLine("Unusual data: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<List<int>> reports = getReports(puzzleInput.Lines);
            Console.WriteLine("safe reports: {0}", getSaveReports(reports, false));
            Console.WriteLine("safe reports with tolerate: {0}", getSaveReports(reports, true));
        }

        private static int getSaveReports(List<List<int>> reports, bool checkToleration)
        {
            int saveReports = 0;

            foreach (List<int> report in reports)
            {
                if (isSaveReport(report, checkToleration))
                {
                    saveReports++;
                }
            }

            return saveReports;
        }

        private static bool isSaveReport(List<int> report, bool checkToleration)
        {
            bool isAscending = report[0] < report[1];
            bool isDescending = report[0] > report[1];

            bool isSafe = isAscending || isDescending;

            for (int i = 0; i < report.Count - 1; i++)
            {
                if (isAscending)
                {
                    isSafe = isSafe && report[i] < report[i + 1];
                }
                else if (isDescending)
                {
                    isSafe = isSafe && report[i] > report[i + 1];
                }

                isSafe = isSafe && (Math.Abs(report[i] - report[i + 1]) >= 1);
                isSafe = isSafe && (Math.Abs(report[i] - report[i + 1]) <= 3);

                if (!isSafe && checkToleration)
                {
                    isSafe = isSaveReport(report.Where((x, j) => i != j).ToList(), false);
                    isSafe = isSafe || isSaveReport(report.Where((x, j) => i + 1 != j).ToList(), false);
                    if(i == 1)
                    {
                        isSafe = isSafe || isSaveReport(report.Where((x, j) => i - 1 != j).ToList(), false);
                    }
                    break;
                }

                if (!isSafe)
                {
                    break;
                }
            }

            return isSafe;
        }

        private static List<List<int>> getReports(List<string> lines)
        {
            List<List<int>> reports = new List<List<int>>();

            foreach (string line in lines)
            {
                string[] levels = line.Split(' ');

                reports.Add(levels.Select( l => Int32.Parse(l)).ToList());
            }

            return reports;
        }
    }
}
