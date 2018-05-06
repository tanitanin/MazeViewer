using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core.Algorithm
{
    public static class GraphAlgorithm
    {
        public static Graph<T> GetMinimumPath<T>(this Graph<T> graph, Node<T> start, Node<T> goal)
        {
            // pre
            bool flg = true;
            while (flg)
            {
                flg = false;
                foreach (var node in graph.Nodes.Where(n => n != start && n.Incidents.Count() == 1))
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
            var prev = graph.Nodes.ToDictionary(x => x, x => null as Edge<T>);

            var queue = new Queue<Node<T>>();
            visited[start] = true;
            queue.Enqueue(start);

            while (queue.Count() > 0)
            {
                var n = queue.Dequeue();
                visited[n] = true;

                if (n == goal)
                {
                    break;
                }

                foreach (var e in n.Incidents)
                {
                    if (!visited[e.End])
                    {
                        queue.Enqueue(e.End);
                        prev[e.End] = e;
                    }
                }
            }

            if (prev[goal] == null) return null;

            var path = new Graph<T>() { Nodes = new List<Node<T>>(), Edges = new List<Edge<T>>() };
            for (var n = goal; prev[n] != null; n = prev[n].Start)
            {
                if (prev[n].Start == goal) continue;
                path.Nodes.Add(n);
                path.Edges.Add(prev[n]);
            }
            path.Nodes.Add(start);

            path.Nodes.Reverse();
            path.Edges.Reverse();

            path.Nodes.AddRange(graph.Nodes.Where(n => n.Incidents.Count() == 0));

            return path;
        }

        public static Graph<T> Dijkstra<T>(this Graph<T> graph, Node<T> start, Node<T> goal)
        {
            // 前処理
            bool flg = true;
            while (flg)
            {
                flg = false;
                foreach (var node in graph.Nodes.Where(n => n != start && n.Incidents.Count() == 1))
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

            // Calc
            var visited = graph.Nodes.ToDictionary(x => x, x => false);
            var prev = graph.Nodes.ToDictionary(x => x, x => null as Edge<T>);
            var cost = graph.Nodes.ToDictionary(x => x, x => double.MaxValue);

            var queue = new PriorityQueue<Node<T>>(graph.Nodes.Count(), Comparer<Node<T>>.Create((a, b) => { return (int)(cost[b] - cost[a]); }));
            visited[start] = true;
            cost[start] = 0.0;
            queue.Push(start);

            while (queue.Count > 0)
            {
                var n = queue.Top;
                queue.Pop();
                visited[n] = true;

                foreach (var e in n.Incidents)
                {
                    if(cost[e.End] > cost[n] + e.Weight)
                    {
                        queue.Push(e.End);
                        prev[e.End] = e;
                        cost[e.End] = cost[n] + e.Weight;
                    }
                }
            }
            if (prev[goal] == null) return null;

            // Construct path
            var path = new Graph<T>() { Nodes = new List<Node<T>>(), Edges = new List<Edge<T>>() };
            for (var n = goal; prev[n] != null; n = prev[n].Start)
            {
                if (prev[n].Start == goal) continue;
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
