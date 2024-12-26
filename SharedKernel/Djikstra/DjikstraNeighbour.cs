using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Djikstra
{
    public class DjikstraNeighbour
    {
        public Coordinate Coordinate { get; }
        public int Distance { get; }
        public DjikstraNeighbour(Coordinate coordinate, int distance)
        { 
            Coordinate = coordinate;
            Distance = distance;
        }
    }
}
