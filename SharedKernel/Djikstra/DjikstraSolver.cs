using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Djikstra
{
    public class DjikstraSolver
    {
        private List<DjikstraCoordinates> Coordinates;

        private Dictionary<DjikstraCoordinates, int> Distances;
        private Dictionary<DjikstraCoordinates, DjikstraCoordinates> Previous;

        public DjikstraSolver(List<DjikstraCoordinates> coordinates)
        {
            Coordinates = coordinates;
            
        }

        public void Solve(Coordinate startCoordinate)
        {
            Distances = new Dictionary<DjikstraCoordinates, int>();
            Previous = new Dictionary<DjikstraCoordinates, DjikstraCoordinates>();

            List<DjikstraCoordinates> coordinates = Coordinates.ToList();

            DjikstraCoordinates start = coordinates.Find(x => x.Coordinate.Equals(startCoordinate));

            foreach (DjikstraCoordinates coord in coordinates)
            {
                Distances[coord] = int.MaxValue;
                Previous[coord] = null;
            }
            Distances[start] = 0;

            while (coordinates.Count > 0)
            {
                DjikstraCoordinates current = null;
                int minValue = int.MaxValue;
                foreach (DjikstraCoordinates coord in coordinates)
                {
                    if (Distances[coord] < minValue)
                    {
                        minValue = Distances[coord];
                        current = coord;
                    }
                }

                //only unreachable points remaining --> break
                if (current == null)
                {
                    break;
                }

                coordinates.Remove(current);

                foreach (DjikstraNeighbour neighbour in current.Neighbours)
                {
                    DjikstraCoordinates neigh = Coordinates.Find(c => c.Coordinate.Equals(neighbour.Coordinate));

                    if (coordinates.Contains(neigh))
                    {
                        int alternative = Distances[current] + neighbour.Distance;

                        if (alternative < Distances[neigh])
                        {
                            Distances[neigh] = alternative;
                            Previous[neigh] = current;
                        }
                    }
                }
            }
        }

        public int? GetDistance(Coordinate endCoordinate)
        {
            int? distance = Distances[Coordinates.Find(c => c.Coordinate.Equals(endCoordinate))];
            if(distance == int.MaxValue) { distance = null; }
            return distance;
        }
    }
}
