using SharedKernel;
using System.ComponentModel.DataAnnotations;

namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 5: Print Queue"));
            Console.WriteLine("Rules: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            List<OrderingRules> orderingRules = getOrderingRules(puzzleInput.BlockLines[0]);
            List<List<int>> updates = getUpdates(puzzleInput.BlockLines[1]);

            List<List<int>> correctUpdates = updates.Where( u => isCorrectUpdate(orderingRules, u )).ToList();
            Console.WriteLine("Sum of middle page numbers: {0}", correctUpdates.Select(u => u[(int) Math.Floor((double) u.Count / 2)]).Sum());

            List<List<int>> incorrectUpdates = updates.Where(u => !isCorrectUpdate(orderingRules, u)).ToList();
            List<List<int>> correctedUpdates = incorrectUpdates.Select( u => fixUpdates(orderingRules, u)).ToList();
            Console.WriteLine("Sum of middle page numbers: {0}", correctedUpdates.Select(u => u[(int)Math.Floor((double)u.Count / 2)]).Sum());
        }

        private static List<int> fixUpdates(List<OrderingRules> orderingRules, List<int> update)
        {
            List<int> fixxedUpdates = new List<int>();
            List<int> tempUpdates = update.ToList();
            int currentIndex = 0;

            fixxedUpdates.Add(tempUpdates.First());
            tempUpdates.RemoveAt(0);

            while (tempUpdates.Count > 0) 
            {
                int insertAt = -1;
                bool found = false;
                List<OrderingRules> rulesForIndex = orderingRules.Where(r => r.Left == tempUpdates[currentIndex] || r.Right == tempUpdates[currentIndex]).ToList();

                foreach (OrderingRules rule in rulesForIndex) 
                {
                    if (rule.Left == tempUpdates[currentIndex] && fixxedUpdates.FindIndex( u => u == rule.Right) >= 0)
                    {
                        if (!found) { insertAt = fixxedUpdates.FindIndex(u => u == rule.Right); }
                        else { insertAt = Math.Min(insertAt, fixxedUpdates.FindIndex(u => u == rule.Right)); }
                        
                        found = true;
                    }
                    else if (rule.Right == tempUpdates[currentIndex] && fixxedUpdates.FindIndex(u => u == rule.Left) >= 0)
                    {
                        if (!found) { insertAt = fixxedUpdates.FindIndex(u => u == rule.Left) + 1; }
                        else { insertAt = Math.Max(insertAt, fixxedUpdates.FindIndex(u => u == rule.Left) + 1); }
                        found = true;
                    }
                }

                if (insertAt >= 0 && found)
                {
                    fixxedUpdates.Insert( insertAt, tempUpdates[currentIndex]);
                    tempUpdates.RemoveAt(currentIndex);
                    currentIndex = 0;
                }
                else
                {
                    currentIndex++;
                }
            }

            return fixxedUpdates;
        }

        private static bool isCorrectUpdate(List<OrderingRules> orderingRules, List<int> update)
        {
            bool isCorrectUpdate = true;

            foreach(OrderingRules rule in orderingRules)
            {
                int indexLeft = update.FindIndex(u => u == rule.Left);
                int indexRight = update.FindIndex(u => u == rule.Right);

                if (indexLeft != -1 && indexRight != -1)
                {
                    isCorrectUpdate = indexLeft < indexRight;
                }

                if (!isCorrectUpdate)
                {
                    break;
                }
            }

            return isCorrectUpdate;
        }

        private static List<List<int>> getUpdates(List<string> input)
        {
            List<List<int>> updates = new List<List<int>>();

            foreach (string element in input)
            {
                List<int> update = new List<int>();
                updates.Add(update);

                string[] strings = element.Split(',');

                foreach (string s in strings)
                {
                    update.Add(int.Parse(s));
                }
            }

            return updates;
        }

        private static List<OrderingRules> getOrderingRules(List<string> list)
        {
            List<OrderingRules> orderingRules = new List<OrderingRules>();

            foreach (string line in list) 
            { 
                orderingRules.Add(new OrderingRules(line));
            }

            return orderingRules;
        }
    }
}
