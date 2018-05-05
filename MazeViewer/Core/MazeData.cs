using MazeViewer.Core.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeViewer.Core
{
    public partial class MazeData
    {
        public int Size { get; set; } = 0;
        public List<Cell> Cells { get; private set; } = new List<Cell>();

        public Cell At(int x, int y) => Cells[x*Size+ y];

        public Cell Start { get => Cells.Where(c => c.IsStart).First(); }
        public IEnumerable<Cell> Goals { get => Cells.Where(c => c.IsGoal); }

        public double CellWidth { get => Consts.ActualMazeCellWidth; }
        public double WallWidth { get => Consts.ActualMazeWallWidth; }

        public static MazeData Load(byte[] bytes)
        {
            var maze = new MazeData()
            {
                Size = (int)Math.Sqrt(bytes.Count()),
                Cells = bytes.Select(b => new Cell {
                    North = (b & 0x01) > 0,
                    East = (b & 0x02) > 0,
                    West = (b & 0x08) > 0,
                    South = (b & 0x04) > 0,
                    IsStart = (b & 0x10) > 0,
                    IsGoal = (b & 0x20) > 0,
                }).ToList(),
            };
            maze.At(0, 0).IsStart = true;
            if(maze.Cells.Where(x => x.IsGoal).Count() < 1)
            {
                var n = maze.Size / 2;
                maze.At(n-1, n-1).IsGoal = true;
                maze.At(n-1, n  ).IsGoal = true;
                maze.At(n  , n-1).IsGoal = true;
                maze.At(n  , n  ).IsGoal = true;
            }
            for(int x = 0; x < maze.Size; ++x)
            {
                for (int y = 0; y < maze.Size; ++y)
                {
                    maze.At(x, y).Pos = new Index2D() { X = x, Y = y };
                }
            }
            return maze;
        }

        public bool Validate()
        {
            for(int i=0; i < Size; ++i)
            {
                for(int j = 0; j < Size; ++j)
                {
                    if (j > 0)
                    {
                        var u = At(i, j - 1);
                        var v = At(i, j);
                        if (u.North != v.South) return false;
                    }
                    if (i > 0)
                    {
                        var s = At(i - 1, j);
                        var t = At(i, j);
                        if (s.East != t.West) return false;
                    }
                }
            }
            return true;
        }
    }
}
