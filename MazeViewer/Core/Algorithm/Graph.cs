using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core.Algorithm
{
    public class Graph<T>
    {
        public List<Node<T>> Nodes { get; set; }
        public List<Edge<T>> Edges { get; set; }
    }
}
