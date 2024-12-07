using SharedKernel;

namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 7: Bridge Repair"));
            Console.WriteLine("calibration equations: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<Int64> calibrationResult = getPossibleCalibrationResults(puzzleInput.Lines, false);
            Console.WriteLine("calibration result: {0}", calibrationResult.Sum());

            calibrationResult = getPossibleCalibrationResults(puzzleInput.Lines, true);
            Console.WriteLine("calibration result: {0}", calibrationResult.Sum());
        }

        private static List<Int64> getPossibleCalibrationResults(List<string> lines, bool withConcat)
        {
            List<Int64> correctResults = new List<Int64>();
            foreach (string line in lines)
            {
                string[] temp = line.Split(':');
                Int64 testValue = Int64.Parse(temp[0]);
                List<int> equatationNumbers = temp[1].Trim().Split(' ').Select(int.Parse).ToList();

                if (isPossibleEquatation(testValue, equatationNumbers, withConcat))
                {
                    correctResults.Add(testValue);
                }
            }

            return correctResults;
        }

        private static bool isPossibleEquatation(Int64 testValue, List<int> equatationNumbers, bool withConcat)
        {
            bool isPossible;
            int currentValue = equatationNumbers.First();
            equatationNumbers.RemoveAt(0);

            isPossible = calculateEquatation(testValue, currentValue, "+", equatationNumbers, withConcat)
                            || calculateEquatation(testValue, currentValue, "*", equatationNumbers, withConcat)
                            || (withConcat && calculateEquatation(testValue, currentValue, "||", equatationNumbers, withConcat));

            return isPossible;
        }

        private static bool calculateEquatation(Int64 testValue, Int64 currentValue, string op, List<int> equatationNumbers, bool withConcat)
        {
            bool isPossible;
            Int64 calc;
            int nextNumber = equatationNumbers.First();

            if (op == "+") { calc = currentValue + nextNumber; }
            else if (op == "*") { calc = currentValue * nextNumber; }
            else if (op == "||") { calc = Int64.Parse(currentValue.ToString() + nextNumber.ToString()); }
            else { throw new InvalidDataException(); }

            if (equatationNumbers.Count > 1)
            {
                List<int> copyNumbers = equatationNumbers.ToList();
                copyNumbers.RemoveAt(0);

                isPossible = calculateEquatation(testValue, calc, "+", copyNumbers, withConcat)
                            || calculateEquatation(testValue, calc, "*", copyNumbers, withConcat)
                            || (withConcat && calculateEquatation(testValue, calc, "||", copyNumbers, withConcat));
            }
            else
            {
                isPossible = (testValue == calc);
            }
                
            return isPossible;
        }
    }
}
