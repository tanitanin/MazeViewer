﻿using MazeViewer.Core.Algorithm;
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
        public int NumOfHorizontalCell { get; set; } = 0;
        public int NumOfVerticalCell { get; set; } = 0;
        public List<Cell> Cells { get; private set; } = new List<Cell>();

        public Cell At(int x, int y) => Cells.ElementAtOrDefault(x + y * NumOfHorizontalCell);

        public Cell Start { get => Cells.Where(c => c.IsStart).First(); }
        public IEnumerable<Cell> Goals { get => Cells.Where(c => c.IsGoal); }

        public double CellWidth { get => Consts.ActualMazeCellWidth; }
        public double WallWidth { get => Consts.ActualMazeWallWidth; }

        public MazeData(int x = 0, int y = 0)
        {
            NumOfHorizontalCell = x;
            NumOfVerticalCell = y;
            if(x * y > 0)
            {
                foreach (var index in new Index2D.Range(NumOfHorizontalCell, NumOfVerticalCell))
                {
                    Cells.Add(new Cell());
                }
                foreach (var index in new Index2D.Range(NumOfHorizontalCell, NumOfVerticalCell))
                {
                    At(index.X, index.Y).Pos = index;
                }
            }
        }

        public static MazeData Load(byte[] bytes)
        {
            var size = (int)Math.Sqrt(bytes.Count());
            var maze = new MazeData()
            {
                NumOfHorizontalCell = size,
                NumOfVerticalCell = size,
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
                var n = size / 2;
                maze.At(n-1, n-1).IsGoal = true;
                maze.At(n-1, n  ).IsGoal = true;
                maze.At(n  , n-1).IsGoal = true;
                maze.At(n  , n  ).IsGoal = true;
            }
            //for(int x = 0; x < maze.Size; ++x)
            //{
            //    for (int y = 0; y < maze.Size; ++y)
            //    {
            //        maze.At(x, y).Pos = new Index2D() { X = x, Y = y };
            //    }
            //}
            foreach(var index in new Index2D.Range(maze.NumOfHorizontalCell, maze.NumOfVerticalCell))
            {
                maze.At(index.X, index.Y).Pos = index;
            }
            return maze;
        }

        public bool Validate()
        {
            for(int i=0; i < NumOfHorizontalCell; ++i)
            {
                for(int j = 0; j < NumOfVerticalCell; ++j)
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
