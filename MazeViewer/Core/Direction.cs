using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeViewer.Core
{
    public struct Direction
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Direction(int x, int y) { X = x; Y = y; } 
        public static Direction North = new Direction(0, +1);
        public static Direction South = new Direction(0, -1);
        public static Direction East  = new Direction(+1, 0);
        public static Direction West  = new Direction(-1, 0);

        public static implicit operator Point(Direction direction) => new Point(direction.X, direction.Y);
        public static implicit operator Vector(Direction direction) => new Vector(direction.X, direction.Y);
        public static implicit operator Index2D(Direction direction) => new Index2D(direction.X, direction.Y);
    }

    public enum DirectionType
    {
        North,
        South,
        East,
        West,
        Center
    }

    public static partial class VectorExtensions
    {
        public static DirectionType GetDirection(this Vector vector)
        {
            if(vector.Y > vector.X)
            {
                if (vector.X + vector.Y > 0) return DirectionType.North;
                if (vector.X + vector.Y < 0) return DirectionType.South;
            }
            else if(vector.Y < vector.X)
            {
                if (vector.X + vector.Y > 0) return DirectionType.East;
                if (vector.X + vector.Y < 0) return DirectionType.West;
            }
            return DirectionType.Center;
        }
    }
}
