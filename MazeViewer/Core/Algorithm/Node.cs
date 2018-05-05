using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core.Algorithm
{
    public class Node
    {
        public List<Edge> Incidents { get; set; }
        public object Data { get; set; }
    }

    public class Node<T> : Node
    {
        public new List<Edge<T>> Incidents { get; set; }
        public new T Data { get; set; }
    }
}
