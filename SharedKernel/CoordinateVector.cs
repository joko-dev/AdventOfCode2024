using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel
{
    public class CoordinateVector
    {
        public Int64 X { get; private set; }
        public Int64 Y { get; private set; }

        public CoordinateVector(Int64 X, Int64 Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public CoordinateVector(Coordinate from, Coordinate to)
        {
            this.X = to.X - from.X;
            this.Y = to.Y - from.Y;
        }

        static public Coordinate Add(CoordinateVector vector, Coordinate coordinate)
        {
            Coordinate newCoordinate = new Coordinate(coordinate.X + vector.X, coordinate.Y + vector.Y);

            return newCoordinate;
        }
    }
}
