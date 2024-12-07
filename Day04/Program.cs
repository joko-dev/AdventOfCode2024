using SharedKernel;

namespace Day04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 04: Ceres Search"));
            Console.WriteLine("Word search: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            Console.WriteLine("Word count XMAS: {0}", getWordCountXMAS(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null)));
            Console.WriteLine("Word count X-MAS: {0}", getWordCountX_MAS(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null)));
        }

        private static int getWordCountXMAS(char[,] chars)
        {
            int wordCount = 0;

            for (int y = 0; y < chars.GetLength(1); y++)
            {
                for (int x = 0; x < chars.GetLength(0); x++)
                {
                    if (chars[x, y] == 'X')
                    {
                        wordCount += checkXMAS(chars, new Coordinate(x, y));
                    }
                }
            }

            return wordCount;
        }

        private static int checkXMAS(char[,] chars, Coordinate coordinate)
        {
            int XMASCount = 0;

            foreach (Move.DirectionType direction in Enum.GetValues(typeof(Move.DirectionType)))
            {
                if (checkWord(chars, new Move(coordinate, direction), "MAS"))
                {
                    XMASCount++;
                }
            }

            return XMASCount;
        }

        private static int getWordCountX_MAS(char[,] chars)
        {
            int wordCount = 0;

            for (int y = 0; y < chars.GetLength(1); y++)
            {
                for (int x = 0; x < chars.GetLength(0); x++)
                {
                    if (chars[x, y] == 'A')
                    {
                        Coordinate currentA = new Coordinate(x, y); 
                        Coordinate upLeft = currentA.GetAdjacentCoordinate(Move.DirectionType.UpLeft);
                        Coordinate upRight = currentA.GetAdjacentCoordinate(Move.DirectionType.UpRight);
                        Coordinate downLeft = currentA.GetAdjacentCoordinate(Move.DirectionType.DownLeft);
                        Coordinate downRight = currentA.GetAdjacentCoordinate(Move.DirectionType.DownRight);

                        if(upLeft.IsInMatrix<char>(chars) && downLeft.IsInMatrix<char>(chars) && upRight.IsInMatrix<char>(chars) && downLeft.IsInMatrix<char>(chars))
                        {
                            bool found = (chars[upLeft.X, upLeft.Y] == 'M' && chars[downRight.X, downRight.Y] == 'S')
                                            || (chars[upLeft.X, upLeft.Y] == 'S' && chars[downRight.X, downRight.Y] == 'M');

                            found = found && ((chars[upRight.X, upRight.Y] == 'M' && chars[downLeft.X, downLeft.Y] == 'S')
                                            || (chars[upRight.X, upRight.Y] == 'S' && chars[downLeft.X, downLeft.Y] == 'M'));

                            if (found)
                            {
                                wordCount++;
                            }
                        }
                    }
                }
            }

            return wordCount;
        }

        private static bool checkWord(char[,] chars, Move move, string leftLetters)
        {
            Move newCoordinate = move.MoveToDirection();
            bool wordFound = false;

            if (newCoordinate.Coordinate.IsInMatrix<char>(chars) && chars[newCoordinate.Coordinate.X, newCoordinate.Coordinate.Y] == leftLetters.First())
            {
                if (leftLetters.Length == 1)
                {
                    wordFound = true;
                }
                else 
                {
                    string tail = leftLetters.Substring(1);
                    wordFound = checkWord(chars, newCoordinate, tail);
                }
            }

            return wordFound;
        }
    }
}
