using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Djikstra
{
    public class DjikstraCoordinates
    {
        public Coordinate Coordinate { get; }
        public List<DjikstraNeighbour> Neighbours { get; }

        public DjikstraCoordinates(Coordinate coordinate) 
        { 
            Coordinate = coordinate;
            Neighbours = new List<DjikstraNeighbour>();
        }

        public void AddNeighbour(DjikstraNeighbour neighbour)
        {
            Neighbours.Add(neighbour);
        }
    }
}
