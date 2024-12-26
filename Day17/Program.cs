using SharedKernel;

namespace Day17
{
    internal class Program
    {
        internal enum OpCode { adv = 0, bxl = 1, bst = 2, jnz = 3, bxc = 4, output = 5, bdv = 6, cdv = 7 }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 17: Chronospatial Computer"));
            Console.WriteLine("Instructions: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            int registerA = getRegister(puzzleInput.Lines[0]);
            int registerB = getRegister(puzzleInput.Lines[1]);
            int registerC = getRegister(puzzleInput.Lines[2]);
            List<int> instructions = getInstructions(puzzleInput.Lines[4]);

            Console.WriteLine("Output: {0}", runInstruction(registerA, registerB, registerC, instructions));
        }

        private static string runInstruction(int registerA, int registerB, int registerC, List<int> instructions)
        {
            int instructionPointer = 0;
            string output = "";

            while (instructionPointer < instructions.Count)
            {
                bool jnzUsed = false;
                OpCode instruction = (OpCode) instructions[instructionPointer];
                int operand = instructions[instructionPointer + 1];
                
                switch (instruction)
                {
                    case OpCode.adv:
                        registerA = (int) Math.Floor((registerA / Math.Pow(2, getComboOperand(registerA, registerB, registerC, operand))));
                        break;
                    case OpCode.bxl:
                        registerB = registerB ^ operand;
                        break;
                    case OpCode.bst:
                        registerB = getComboOperand(registerA, registerB, registerC, operand) % 8;
                        break;
                    case OpCode.jnz:
                        if(registerA != 0)
                        {
                            instructionPointer = operand;
                            jnzUsed = true;
                        }
                        break;
                    case OpCode.bxc:
                        registerB = registerB ^ registerC;
                        break;
                    case OpCode.output:
                        output += "," + (getComboOperand(registerA, registerB, registerC, operand) % 8).ToString();
                        break;
                    case OpCode.bdv:
                        registerB = (int)Math.Floor((registerA / Math.Pow(2, getComboOperand(registerA, registerB, registerC, operand))));
                        break;
                    case OpCode.cdv:
                        registerC = (int)Math.Floor((registerA / Math.Pow(2, getComboOperand(registerA, registerB, registerC, operand))));
                        break;
                }

                if(!jnzUsed) { instructionPointer += 2; }
            }
            
            return output;
        }

        private static int getComboOperand(int registerA, int registerB, int registerC, int operand)
        {
            int result = 0;

            if(operand <= 3) { result = operand; }
            else if(operand == 4) { result = registerA; }
            else if(operand == 5) { result = registerB; }
            else if(operand == 6) { result = registerC; }
            else { throw new ArgumentException(); }

            return result;

        }

        private static List<int> getInstructions(string line)
        {
            return line.Split(':')[1].Split(",").Select( n => int.Parse(n)).ToList();
        }

        private static int getRegister(string line)
        {
            return int.Parse(line.Split(':')[1]);
        }
    }
}
