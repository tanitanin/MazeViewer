using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeViewer.Core
{
    public class Wall
    {
        //public Point P1 { get; set; }
        //public Point P2 { get; set; }
        public bool Exist { get; set; } = false;
        public bool Virtual { get; set; } = false;
    }

    public class WallMaze
    {
        public int Size { get; set; } = 0;
        public IEnumerable<Wall> VerticalWalls { get; set; }
        public IEnumerable<Wall> HorizontalWalls { get; set; }

        public Cell Start { get; set; }
        public IEnumerable<Cell> Goals { get; set; }

        public bool IsWall(int x, int y, DirectionType direction)
        {
            switch (direction)
            {
                case DirectionType.North:

                case DirectionType.South:
                case DirectionType.East:
                case DirectionType.West:
                default:
                    return false;
            }
        }
    }
}
