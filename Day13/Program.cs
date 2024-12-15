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

            List<List<(Int64, Int64)>> machines = getMachines(puzzleInput.Lines);
            Console.WriteLine("Token count: {0}", getTokenCount(machines, 100, 0));
            Console.WriteLine("Token count: {0}", getTokenCount(machines, 0, 10000000000000));
        }

        private static Int64 getTokenCount(List<List<(Int64, Int64)>> machines, int maxPresses, Int64 toAdd)
        {
            Int64 tokens = 0;

            foreach (var machine in machines)
            {
                (Int64, Int64) prize = machine[2];
                prize.Item1 += toAdd;
                prize.Item2 += toAdd;

                Int64 determinante = (machine[0].Item1 * machine[1].Item2) - (machine[0].Item2 * machine[1].Item1);
                Int64 determinanteA = (prize.Item1 * machine[1].Item2) - (prize.Item2 * machine[1].Item1);
                Int64 determinanteB = (machine[0].Item1 * prize.Item2) - (machine[0].Item2 * prize.Item1);

                if(!(determinante == 0 && determinanteA == 0 && determinanteB == 0) && !(determinante == 0 && determinanteA != 0 && determinanteB != 0))
                {
                    if (determinanteA % determinante == 0 && determinanteB % determinante == 0)
                    {
                        Int64 a = determinanteA / determinante;
                        Int64 b = determinanteB / determinante;

                        if (!(maxPresses > 0) || (a <= maxPresses && b <= maxPresses))
                        {
                            tokens += a * 3 + b;
                        }
                    }
                }
                               
            }

            return tokens;
        }

        private static List<List<(Int64, Int64)>> getMachines(List<string> lines)
        {
            List<List<(Int64, Int64)>> machines = new List<List<(Int64, Int64)>>();

            for (int i = 0; i < lines.Count; i= i + 3)
            { 
                List<(Int64, Int64)> machine = new List<(Int64, Int64)>();

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
