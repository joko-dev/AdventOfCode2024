using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day05
{
    internal class OrderingRules
    {
        public int Left {  get; }
        public int Right { get; }

        public OrderingRules (string line)
        {
            string[] elements = line.Split ('|');
            Left = int.Parse (elements[0]);
            Right = int.Parse (elements[1]);
        }
    }
}
