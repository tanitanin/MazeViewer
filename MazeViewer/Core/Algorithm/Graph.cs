using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core.Algorithm
{
    public class Graph
    {
        public List<Node> Nodes { get; set; }
        public List<Edge> Edges { get; set; }
    }

    public class Graph<T> : Graph
    {
        public new List<Node<T>> Nodes { get; set; }
        public new List<Edge<T>> Edges { get; set; }
    }
}
