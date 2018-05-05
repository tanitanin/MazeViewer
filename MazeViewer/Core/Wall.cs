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
}
