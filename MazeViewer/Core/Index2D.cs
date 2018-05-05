using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core
{
    public class Index2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Index2D(int x = 0, int y = 0) { X = x; Y = y; }

        public static Index2D operator +(Index2D a, Index2D other) => new Index2D() { X = a.X + other.X, Y = a.Y + other.Y };
        public static Index2D operator -(Index2D a, Index2D other) => new Index2D() { X = a.X - other.X, Y = a.Y - other.Y };
        public static Index2D operator *(Index2D a, int other) => new Index2D() { X = a.X * other, Y = a.Y * other };
        public static Index2D operator /(Index2D a, int other) => new Index2D() { X = a.X / other, Y = a.Y / other };
    }
}
