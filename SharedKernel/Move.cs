using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel
{
    public class Move
    {
        public enum DirectionType { Left = 1, Right = 2, Up = 3, Down = 4, UpLeft = 5, UpRight = 6, DownLeft = 7, DownRight = 8 }
        public Coordinate Coordinate { get; set; }
        public DirectionType Direction { get; set; }
        public Move(Coordinate coordinate, DirectionType direction)
        {
            Coordinate = coordinate;
            Direction = direction;
        }
        public override bool Equals(object obj)
        {
            var move = obj as Move;

            if (move is null)
            {
                return false;
            }
            return (this.Coordinate.Equals(move.Coordinate) && (this.Direction == move.Direction));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Coordinate, Direction);
        }

        public Move MoveToDirection()
        {
           return new Move(Coordinate.GetAdjacentCoordinate(Direction), Direction);
        }
        
        public Move RotateLeft()
        {
            DirectionType rotatedDirection;
            switch (Direction)
            {
                case DirectionType.Left: rotatedDirection = DirectionType.Down; break;
                case DirectionType.Right: rotatedDirection = DirectionType.Up; break;
                case DirectionType.Up: rotatedDirection = DirectionType.Left; break;
                case DirectionType.Down: rotatedDirection = DirectionType.Right; break;
                default: throw new NotImplementedException();
            }
            return new Move(Coordinate, rotatedDirection);
        }
        public Move RotateRight()
        {
            DirectionType rotatedDirection;
            switch (Direction)
            {
                case DirectionType.Left: rotatedDirection = DirectionType.Up; break;
                case DirectionType.Right: rotatedDirection = DirectionType.Down; break;
                case DirectionType.Up: rotatedDirection = DirectionType.Right; break;
                case DirectionType.Down: rotatedDirection = DirectionType.Left; break;
                default: throw new NotImplementedException();
            }
            return new Move(Coordinate, rotatedDirection);
        }
    }
}
