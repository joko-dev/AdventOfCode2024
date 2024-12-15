using SharedKernel;

namespace Day12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 12: Garden Groups"));
            Console.WriteLine("garden: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<List<Coordinate>> regions = getRegions(PuzzleConverter.getInputAsMatrixChar(puzzleInput.Lines, null));
            Console.WriteLine("total price: {0}", getTotalPriceForRegions(regions));
            Console.WriteLine("total price: {0}", getTotalPriceForRegionsSides(regions));
        }

        private static int getTotalPriceForRegions(List<List<Coordinate>> regions)
        {
            int totalPrice = 0;

            foreach (List<Coordinate> region in regions)
            {
                int priceRegion = 0;
                int perimeter = 0;

                foreach (Coordinate coord in region)
                {
                    foreach(Coordinate adj in coord.GetAdjacentCoordinatesCardinalPoints())
                    {
                        if (!region.Contains(adj))
                        {
                            perimeter++;
                        }
                    }
                }

                priceRegion = region.Count * perimeter;

                totalPrice += priceRegion;
            }

            return totalPrice;
        }

        private static int getTotalPriceForRegionsSides(List<List<Coordinate>> regions)
        {
            int totalPrice = 0;

            foreach (List<Coordinate> region in regions)
            {
                int priceRegion = 0;
                int sideUp = 0;
                int sideDown = 0;
                int sideLeft = 0;
                int sideRight = 0;


                for (Int64 y = region.Select(r => r.Y).Min(); y <= region.Select(r => r.Y).Max(); y++)
                {
                    bool sideBeginUpside = false;
                    bool sideBeginDownside = false;

                    for (Int64 x = region.Select(r => r.X).Min(); x <= region.Select(r => r.X).Max(); x++)
                    {
                        Coordinate toCheck = new Coordinate(x, y);
                        if (region.Contains(toCheck))
                        {
                            Coordinate adjUp = toCheck.GetAdjacentCoordinate(Move.DirectionType.Up);
                            Coordinate adjDown = toCheck.GetAdjacentCoordinate(Move.DirectionType.Down);

                            if (!region.Contains(adjUp))
                            {
                                if (!sideBeginUpside)
                                {
                                    sideBeginUpside = true;
                                    sideUp++;
                                }
                            }
                            else
                            {
                                sideBeginUpside = false;
                            }

                            if (!region.Contains(adjDown))
                            {
                                if (!sideBeginDownside)
                                {
                                    sideBeginDownside = true;
                                    sideDown++;
                                }
                            }
                            else
                            {
                                sideBeginDownside = false;
                            }
                        }
                        else
                        {
                            sideBeginUpside = false;
                            sideBeginDownside = false;
                        }
                    }
                }

                for (Int64 x = region.Select(r => r.X).Min(); x <= region.Select(r => r.X).Max(); x++)
                {
                    bool sideBeginLeft = false;
                    bool sideBeginRight = false;
                    

                    for (Int64 y = region.Select(r => r.Y).Min(); y <= region.Select(r => r.Y).Max(); y++)
                    {
                        Coordinate toCheck = new Coordinate(x, y);
                        if (region.Contains(toCheck))
                        {
                            Coordinate adjLeft = toCheck.GetAdjacentCoordinate(Move.DirectionType.Left);
                            Coordinate adjRight = toCheck.GetAdjacentCoordinate(Move.DirectionType.Right);

                            if (!region.Contains(adjLeft))
                            {
                                if (!sideBeginLeft)
                                {
                                    sideBeginLeft = true;
                                    sideLeft++;
                                }
                            }
                            else
                            {
                                sideBeginLeft = false;
                            }

                            if (!region.Contains(adjRight))
                            {
                                if (!sideBeginRight)
                                {
                                    sideBeginRight = true;
                                    sideRight++;
                                }
                            }
                            else
                            {
                                sideBeginRight = false;
                            }
                        }
                        else
                        {
                            sideBeginRight = false;
                            sideBeginLeft = false;
                        }
                    }
                }

                priceRegion = region.Count * (sideLeft + sideRight + sideUp + sideDown);

                totalPrice += priceRegion;
            }

            return totalPrice;
        }

        private static List<List<Coordinate>> getRegions(char[,] map)
        {
            List<List<Coordinate>> regions = new List<List<Coordinate>>();
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for(int x = 0; x < map.GetLength(0); x++)
                {
                    Coordinate start = new Coordinate(x, y);
                    if(!regions.Exists(r => r.Contains(start)))
                    {
                        regions.Add(getNewRegion(map, start));
                    }
                }
            }

            return regions;
        }

        private static List<Coordinate> getNewRegion(char[,] map, Coordinate start)
        {
            char fruit = map[start.X, start.Y];
            List<Coordinate> region = new List<Coordinate>();

            fillRegion(map, start, fruit, region);

            return region;
        }

        private static void fillRegion(char[,] map, Coordinate coordinate, char fruit, List<Coordinate> region)
        {
            if (coordinate.IsInMatrix(map) && map[coordinate.X, coordinate.Y] == fruit && !region.Contains(coordinate))
            {
                region.Add(coordinate);
                List<Coordinate> adjacent = coordinate.GetAdjacentCoordinatesCardinalPoints();
                foreach (Coordinate adj in adjacent) 
                {
                    fillRegion(map, adj, fruit, region);
                }
            }
        }
    }
}
