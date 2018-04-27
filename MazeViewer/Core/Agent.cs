using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeViewer.Core
{
    public class Agent
    {
        public Point Pos { get; private set; } = new Point(0.0, 0.0);
        
        public void Simulate()
        {

        }
        
        public void Clear()
        {

        }

        internal enum State
        {
            Initialize,
            Search,
            Goal,

        }
    }
}
