using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core
{

    public class Maze
    {
        public int NumOfHorizontalCell { get; set; } = 0;
        public int NumOfVerticalCell { get; set; } = 0;
        public List<Wall> VerticalWalls { get; set; }
        public List<Wall> HorizontalWalls { get; set; }

        public Cell Start { get; set; }
        public List<Cell> Goals { get; set; }
        
        public Maze(int size = 0) : this(size, size)
        {
        }

        public Maze(int x, int y)
        {
            NumOfHorizontalCell = x;
            NumOfVerticalCell = y;
            VerticalWalls = Enumerable.Repeat(new Wall() { Exist = false, Virtual = false }, NumOfHorizontalCell * (NumOfVerticalCell + 1)).ToList();
            HorizontalWalls = Enumerable.Repeat(new Wall() { Exist = false, Virtual = false }, NumOfVerticalCell * (NumOfHorizontalCell + 1)).ToList();
            Start = null;
            Goals = new List<Cell>();
        }

        public Wall At(int x, int y, DirectionType direction)
        {
            try
            {
                switch (direction)
                {
                    case DirectionType.North: return HorizontalWalls[x + (y + 1) * NumOfHorizontalCell];
                    case DirectionType.South: return HorizontalWalls[x + y * NumOfHorizontalCell];
                    case DirectionType.East: return VerticalWalls[(x + 1) + y * (NumOfHorizontalCell + 1)];
                    case DirectionType.West: return VerticalWalls[x + y * (NumOfHorizontalCell + 1)];
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
            var wallMaze = new Maze(mazeData.NumOfHorizontalCell, mazeData.NumOfVerticalCell);

            for (var y = 0; y < mazeData.NumOfVerticalCell; ++y)
            {
                for (var x = 0; x < mazeData.NumOfHorizontalCell; ++x)
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
