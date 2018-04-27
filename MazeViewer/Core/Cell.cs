using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeViewer.Core
{
    public class Cell
    {
        public bool North { get; set; } = false;
        public bool East { get; set; } = false;
        public bool West { get; set; } = false;
        public bool South { get; set; } = false;
        public bool IsStart { get; set; } = false;
        public bool IsGoal { get; set; } = false;
        public Point Pos { get; set; }
    }
}
