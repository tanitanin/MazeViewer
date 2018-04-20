using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Models
{
    public class Node
    {
        public List<Edge> Incidents { get; set; }
        public Cell Cell { get; set; }
    }
}
