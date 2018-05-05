using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeViewer.Core
{
    public static class MazeDataExtensions
    {
        public static Point GetCenterPoint(this MazeData data, int x, int y)
            => new Point { X = x * data.CellWidth + (data.CellWidth / 2), Y = (data.Size - y - 1) * data.CellWidth + (data.CellWidth / 2) };
        public static Point GetNorthPoint(this MazeData data, int x, int y)
            => new Point { X = x * data.CellWidth + (data.CellWidth / 2), Y = (data.Size - y - 1) * data.CellWidth };
        public static Point GetSouthPoint(this MazeData data, int x, int y)
            => new Point { X = x * data.CellWidth + (data.CellWidth / 2), Y = (data.Size - y) * data.CellWidth };
        public static Point GetWestPoint(this MazeData data, int x, int y)
            => new Point { X = x * data.CellWidth, Y = (data.Size - y - 1) * data.CellWidth + (data.CellWidth / 2) };
        public static Point GetEastPoint(this MazeData data, int x, int y)
            => new Point { X = (x + 1) * data.CellWidth, Y = (data.Size - y - 1) * data.CellWidth + (data.CellWidth / 2) };
        public static Point GetNorthWestPoint(this MazeData data, int x, int y)
            => new Point { X = x * data.CellWidth, Y = (data.Size - y - 1) * data.CellWidth };
        public static Point GetNorthEastPoint(this MazeData data, int x, int y)
            => new Point { X = (x + 1) * data.CellWidth, Y = (data.Size - y - 1) * data.CellWidth };
        public static Point GetSouthEastPoint(this MazeData data, int x, int y)
            => new Point { X = (x + 1) * data.CellWidth, Y = (data.Size - y) * data.CellWidth };
        public static Point GetSouthWestPoint(this MazeData data, int x, int y)
            => new Point { X = x * data.CellWidth, Y = (data.Size - y) * data.CellWidth };
    }
}
