using SharedKernel;

namespace Day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 13: Claw Contraption"));
            Console.WriteLine("Machine behavior: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<List<(int, int)>> machines = getMachines(puzzleInput.Lines);
            Console.WriteLine("Token count: {0}", getTokenCount(machines));
        }

        private static int getTokenCount(List<List<(int, int)>> machines)
        {
            int tokens = 0;

            foreach (var machine in machines)
            {
                for (int a = 0; a < 100; a++)
                {
                    for(int b = 0; b < 100; b++)
                    {
                        if( (a * machine[0].Item1 + b * machine[1].Item1 == machine[2].Item1)
                            && (a * machine[0].Item2 + b * machine[1].Item2 == machine[2].Item2))
                        {
                            tokens += 3 * a + b;
                        }
                    }
                }
            }

            return tokens;
        }

        private static List<List<(int, int)>> getMachines(List<string> lines)
        {
            List<List<(int, int)>> machines = new List<List<(int, int)>>();

            for (int i = 0; i < lines.Count; i= i + 3)
            { 
                List<(int,int)> machine = new List<(int, int)>();

                string[] buttonA = lines[i].Replace("Button A:", "").Replace("X+", "").Replace("Y+", "").Split(",");
                string[] buttonB = lines[i + 1].Replace("Button B:", "").Replace("X+", "").Replace("Y+", "").Split(",");
                string[] prize = lines[i + 2].Replace("Prize:", "").Replace("X=", "").Replace("Y=", "").Split(",");

                machine.Add((int.Parse(buttonA[0]), int.Parse(buttonA[1])));
                machine.Add((int.Parse(buttonB[0]), int.Parse(buttonB[1])));
                machine.Add((int.Parse(prize[0]), int.Parse(prize[1])));

                machines.Add(machine);
            }

            return machines;
        }
    }
}
