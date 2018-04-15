using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Models
{
    public class Cell
    {
        public bool North { get; set; } = false;
        public bool East { get; set; } = false;
        public bool West { get; set; } = false;
        public bool South { get; set; } = false;
    }

    public partial class Maze
    {
        public int Size { get; set; } = 0;
        public List<Cell> Cells { get; private set; } = new List<Cell>();

        public Cell At(int x, int y) => Cells[x*Size+ y];

        public static Maze Load(byte[] bytes)
        {
            return new Maze()
            {
                Size = (int)Math.Sqrt(bytes.Count()),
                Cells = bytes.Select(b => new Cell {
                    North = (b & 0x01) > 0,
                    East = (b & 0x02) > 0,
                    West = (b & 0x08) > 0,
                    South = (b & 0x04) > 0,
                }).ToList()
            };
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
