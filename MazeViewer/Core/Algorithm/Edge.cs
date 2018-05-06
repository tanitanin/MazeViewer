using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core.Algorithm
{
    public class Edge
    {
        public Node Start { get; set; }
        public Node End { get; set; }
        public double Weight { get; set; }
    }

    public class Edge<T> : Edge
    {
        public Node<T> Start { get; set; }
        public Node<T> End { get; set; }
        public double Weight { get; set; }
    }
}
