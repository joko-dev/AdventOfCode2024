using SharedKernel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Day16
{
    internal class Program
    {
        
        private class Edge
        {
            public Move From { get; private set; }
            public Move To { get; private set; }

            public int Cost { get { return getCost(); } }

            private int getCost()
            {
                if (this.From.Direction == this.To.Direction) { return 1; }
                else { return 1000; }
            }

            public Edge(Move from, Move to)
            {
                this.From = from;
                this.To = to;
            }

            public override bool Equals(object obj)
            {
                var edge = obj as Edge;
                if (edge == null)
                {
                    return false;
                }
                return (this.From.Equals(edge.From) && this.To.Equals(edge.To));
            }

            public override string ToString()
            {
                return From.ToString() + ": " + To.ToString() + " " + Cost;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(From, To, Cost);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 16: Reindeer Maze"));
            Console.WriteLine("Map: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            (int Score, int TileCount) result = getLowestScore(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null));
            Console.WriteLine("Lowest score: {0}, TileCount: {1}", result.Score, result.TileCount);
        }

        private static(int Score, int TileCount) getLowestScore(char[,] map)
        {
            // Init
            Coordinate startCoordinate = PuzzleConverter.getCoordinatesForValueInMatrix(map, 'S').First();
            Coordinate endCoordinate = PuzzleConverter.getCoordinatesForValueInMatrix(map, 'E').First();
            getKnotsEdges(map, startCoordinate, endCoordinate, out List<Move> knots, out List<Edge> edges);
            List<Move> endPoints = knots.Where(k => k.Coordinate == endCoordinate).ToList();

            Move start = knots.Find( k => k.Coordinate == startCoordinate && k.Direction == Move.DirectionType.Right);

            Dictionary<Move, int> distance = new Dictionary<Move, int>();
            Dictionary<Move, Move> previous = new Dictionary<Move, Move>();
            Dictionary<Move, List<Move>> shortestPathsPredecessors = new Dictionary<Move, List<Move>>();
            foreach (Move knot in knots)
            {
                shortestPathsPredecessors[knot] = new List<Move>();
            }

            // Djikstra
            foreach (Move knot in knots)
            {
                distance[knot] = int.MaxValue;
                previous[knot] = null;
            }
            distance[start] = 0;

            while (knots.Count > 0)
            {
                Move current = null;
                int minValue =int.MaxValue;
                foreach (Move knot in knots) 
                {
                    if (distance[knot] < minValue)
                    {
                        minValue = distance[knot];
                        current = knot;
                    } 
                }

                knots.Remove(current);

                foreach (Edge edge in edges.Where( e => e.From == current))
                {
                    if (knots.Contains(edge.To))
                    {
                        int alternative = distance[current] + edge.Cost;

                        if (alternative <= distance[edge.To])
                        {
                            if (alternative < distance[edge.To])
                            {
                                shortestPathsPredecessors[edge.To].Clear();
                                distance[edge.To] = alternative;
                                previous[edge.To] = current;
                            }
                            shortestPathsPredecessors[edge.To].Add(current);
                        }
                    }
                }
            }

            int minDistance = distance.Where(d => endPoints.Contains(d.Key)).Min(d => d.Value);
            List<Coordinate> minPathCoordinates = new List<Coordinate>();
            foreach(Move point in endPoints)
            {
                if (distance[point] == minDistance)
                {
                    fillMinPathCoordinates(point, minPathCoordinates, shortestPathsPredecessors);
                }
                
            }

            return (minDistance, minPathCoordinates.Count);
        }

        private static void fillMinPathCoordinates(Move move, List<Coordinate> minPathCoordinates, Dictionary<Move, List<Move>> shortestPathsPredecessors)
        {
            if (!minPathCoordinates.Contains(move.Coordinate))
            {
                minPathCoordinates.Add(move.Coordinate);
            }

            foreach(Move predecessor in shortestPathsPredecessors[move])
            {
                fillMinPathCoordinates(predecessor, minPathCoordinates, shortestPathsPredecessors);
            }
        }

        private static void getKnotsEdges(char[,] map, Coordinate start, Coordinate end, out List<Move> knots, out List<Edge> edges)
        {
            knots = new List<Move>();
            edges = new List<Edge>();

            List<Coordinate> coordinates = PuzzleConverter.getCoordinatesForValueInMatrix(map, '.');
            coordinates.Add(start);
            coordinates.Add(end);

            foreach (Coordinate coord in coordinates)
            {
                knots.Add(new Move(coord, Move.DirectionType.Up));
                knots.Add(new Move(coord, Move.DirectionType.Down));
                knots.Add(new Move(coord, Move.DirectionType.Left));
                knots.Add(new Move(coord, Move.DirectionType.Right));
            }
            
            foreach(Move knot in knots)
            {
                Move forwardMove = knot.MoveToDirection();
                if (knots.Any( k=> k.Equals(forwardMove)))
                {
                    edges.Add(new Edge(knot, knots.Find( k => k.Equals(forwardMove))));
                }
                edges.Add(new Edge(knot, knots.Find(k => k.Equals(knot.RotateLeft()))));
                edges.Add(new Edge(knot, knots.Find(k => k.Equals(knot.RotateRight()))));
            }

        }
    }
}
