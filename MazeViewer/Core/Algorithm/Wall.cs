using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeViewer.Core.Algorithm
{
    public class Wall
    {
        //public Point P1 { get; private set; }
        //public Point P2 { get; private set; }
        public bool Exist { get; private set; } = false;
    }

    public class WallMazeData
    {
        public IEnumerable<Wall> VerticalWalls { get; set; }
        public IEnumerable<Wall> HorizontalWalls { get; set; }
        public bool IsWall(int x, int y, DirectionType direction)
        {
            return false;
        }
    }
}
