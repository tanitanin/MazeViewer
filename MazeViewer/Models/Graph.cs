using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Models
{
    public class Graph
    {
        public List<Node> Nodes { get; set; }
        public List<Edge> Edges { get; set; }
    }

    public static class GraphAlgorithm
    {
        public static Graph GetMinimumPath(this Graph graph, Node start, Node goal)
        {
            // pre
            bool flg = true;
            while (flg)
            {
                flg = false;
                foreach (var node in graph.Nodes.Where(n => !n.Cell.IsStart && n.Incidents.Count()==1))
                {
                    flg = true;
                    foreach (var e in graph.Edges.Where(e => e.End == node))
                    {
                        e.Start.Incidents.Remove(e);
                    }
                    foreach (var e in node.Incidents)
                    {
                        graph.Edges.Remove(e);
                    }
                    node.Incidents.Clear();
                }
            }

            // BFS
            var visited = graph.Nodes.ToDictionary(x => x, x => false);
            var prev = graph.Nodes.ToDictionary(x => x, x => null as Edge);
            
            var queue = new Queue<Node>();
            visited[start] = true;
            queue.Enqueue(start);

            while(queue.Count() > 0)
            {
                var n = queue.Dequeue();
                visited[n] = true;

                if(n == goal)
                {
                    break;
                }

                foreach(var e in n.Incidents)
                {
                    if(!visited[e.End])
                    {
                        queue.Enqueue(e.End);
                        prev[e.End] = e;
                    }
                }
            }

            if (prev[goal] == null) return null;

            var path = new Graph() { Nodes = new List<Node>(), Edges = new List<Edge>() };
            for(var n = goal; prev[n] != null; n = prev[n].Start)
            {
                if (prev[n].Start.Cell.IsGoal) continue;
                path.Nodes.Add(n);
                path.Edges.Add(prev[n]);
            }
            path.Nodes.Add(start);

            path.Nodes.Reverse();
            path.Edges.Reverse();

            path.Nodes.AddRange(graph.Nodes.Where(n => n.Incidents.Count() == 0));

            return path;
        }
    }

}
