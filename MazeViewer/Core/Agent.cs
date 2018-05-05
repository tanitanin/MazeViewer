using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace MazeViewer.Core
{
    public class Agent
    {
        public Point Pos { get; private set; } = new Point(0.0, 0.0);
        private Stopwatch stopwatch = null;
        public Stopwatch Stopwatch { get => this.stopwatch; }

        public Maze InternalMazeData { get; set; } = new Maze();

        public void Simulate()
        {
            this.stopwatch = new Stopwatch();
            this.stopwatch.Start();
        }
        
        public void Clear()
        {
            this.stopwatch = null;
        }

        internal enum State
        {
            Initialize,
            Search,
            Goal,

        }
    }
}
