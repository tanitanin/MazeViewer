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
        
        public Maze(int size = 0)
        {
            Size = size;
            VerticalWalls   = new List<Wall>(Enumerable.Repeat(new Wall() { Exist = false, Virtual = false }, Size * (Size + 1)));
            HorizontalWalls = new List<Wall>(Enumerable.Repeat(new Wall() { Exist = false, Virtual = false }, Size * (Size + 1)));
            Start = null;
            Goals = new List<Cell>();
        }

        public Wall At(int x, int y, DirectionType direction)
        {
            try
            {
                switch (direction)
                {
                    case DirectionType.North: return HorizontalWalls[x + (y + 1) * Size];
                    case DirectionType.South: return HorizontalWalls[x + y * Size];
                    case DirectionType.East: return VerticalWalls[(x + 1) + y * (Size + 1)];
                    case DirectionType.West: return VerticalWalls[x + y * (Size + 1)];
                    default: return default(Wall);
                }
            }
            catch (Exception)
            {
                return default(Wall);
            }
        }

        public static Maze LoadFromMazeData(MazeData mazeData)
        {
            var wallMaze = new Maze(mazeData.Size);

            for (var y = 0; y < mazeData.Size; ++y)
            {
                for (var x = 0; x < mazeData.Size; ++x)
                {
                    if (mazeData.At(x, y).East) wallMaze.At(x, y, DirectionType.East).Exist = true;
                    if (mazeData.At(x, y).West) wallMaze.At(x, y, DirectionType.West).Exist = true;
                    if (mazeData.At(x, y).North) wallMaze.At(x, y, DirectionType.North).Exist = true;
                    if (mazeData.At(x, y).South) wallMaze.At(x, y, DirectionType.South).Exist = true;
                    if (mazeData.At(x, y).IsStart) wallMaze.Start = mazeData.At(x, y);
                    if (mazeData.At(x, y).IsGoal) wallMaze.Goals.Add(mazeData.At(x, y));
                }
            }

            return wallMaze;
        }
    }
}
