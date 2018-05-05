using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core
{

    public class Maze
    {
        public int Size { get; set; } = 0;
        public List<Wall> VerticalWalls { get; set; }
        public List<Wall> HorizontalWalls { get; set; }

        public Cell Start { get; set; }
        public List<Cell> Goals { get; set; }
        
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

        public static Maze LoadFromMazeData(MazeData maze)
        {
            var wallMaze = new Maze
            {
                Size = maze.Size,
                VerticalWalls = new List<Wall>(maze.Size * (maze.Size + 1)),
                HorizontalWalls = new List<Wall>(maze.Size * (maze.Size + 1)),
                Goals = new List<Cell>(),
            };

            for (var y = 0; y < maze.Size; ++y)
            {
                for (var x = 0; x < maze.Size; ++x)
                {
                    if (maze.At(x, y).East) wallMaze.At(x, y, DirectionType.East).Exist = true;
                    if (maze.At(x, y).West) wallMaze.At(x, y, DirectionType.West).Exist = true;
                    if (maze.At(x, y).North) wallMaze.At(x, y, DirectionType.North).Exist = true;
                    if (maze.At(x, y).South) wallMaze.At(x, y, DirectionType.South).Exist = true;
                    if (maze.At(x, y).IsStart) wallMaze.Start = maze.At(x, y);
                    if (maze.At(x, y).IsGoal) wallMaze.Goals.Add(maze.At(x, y));
                }
            }

            return wallMaze;
        }
    }
}
