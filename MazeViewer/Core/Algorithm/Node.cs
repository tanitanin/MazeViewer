using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core.Algorithm
{
    public class Node<T>
    {
        public List<Edge<T>> Incidents { get; set; }
        public T Data { get; set; }
    }
}
