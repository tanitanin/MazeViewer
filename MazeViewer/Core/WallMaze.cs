using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core
{

    public class WallMaze
    {
        public int Size { get; set; } = 0;
        public List<Wall> VerticalWalls { get; set; }
        public List<Wall> HorizontalWalls { get; set; }

        public Cell Start { get; set; }
        public IEnumerable<Cell> Goals { get; set; }
        
        public Wall At(int x, int y, DirectionType direction)
        {
            switch (direction)
            {
                case DirectionType.North: return HorizontalWalls[x + y * (Size + 1)];
                case DirectionType.South: return HorizontalWalls[x + (y + 1) * (Size + 1)];
                case DirectionType.East: return VerticalWalls[(x + 1) + y * (Size + 1)];
                case DirectionType.West: return VerticalWalls[x + y * (Size + 1)];
                default: return null;
            }
        }

        public static WallMaze LoadFromMaze(Maze maze)
        {
            var wallMaze = new WallMaze();
            wallMaze.Size = maze.Size;
            wallMaze.VerticalWalls = new List<Wall>(wallMaze.Size * (wallMaze.Size + 1));
            wallMaze.HorizontalWalls = new List<Wall>(wallMaze.Size * (wallMaze.Size + 1));

            for (var y = 0; y < maze.Size; ++y)
            {
                for (var x = 0; x < maze.Size; ++x)
                {
                    if (maze.At(x, y).East) wallMaze.At(x, y, DirectionType.East).Exist = true;
                    if (maze.At(x, y).West) wallMaze.At(x, y, DirectionType.West).Exist = true;
                    if (maze.At(x, y).North) wallMaze.At(x, y, DirectionType.North).Exist = true;
                    if (maze.At(x, y).South) wallMaze.At(x, y, DirectionType.South).Exist = true;
                }
            }
            return wallMaze;
        }
    }
}
