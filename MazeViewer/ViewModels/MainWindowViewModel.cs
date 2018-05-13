using MazeViewer.Core;
using MazeViewer.Core.Algorithm;
using MazeViewer.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeViewer.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindow View { get; }

        public MainWindowViewModel(MainWindow mainWindow)
        {
            View = mainWindow;
        }

        private MazeData mazeData = null;
        public MazeData MazeData { get => this.mazeData; private set => SetValueAndNotify(ref this.mazeData, value, nameof(MazeData), nameof(CanvasWidth), nameof(CanvasHeight)); }

        private Graph<Point> graph = null;
        public Graph<Point> Graph { get => this.graph; private set => SetValueAndNotify(ref this.graph, value, nameof(Graph), nameof(MinimumStep)); }
        public double MinimumStep { get => Graph?.Edges?.Aggregate(0.0, (s, e) => { return s + e.Weight; }) ?? 0; }

        public int SelectedMazeFileIndex { get; set; } = 0;
        public ObservableCollection<string> MazeFileList { get; private set; } = new ObservableCollection<string>(Directory.EnumerateFiles(@"MazeData"));

        public Agent Agent { get; } = new Agent();

        //public bool MarkEnabled { get; set; } = true;

        private double scale = 1.0;
        public double Scale { get => this.scale; set => SetValue(ref this.scale, value); }

        public double CanvasWidth { get => MazeData.Size * Consts.ActualMazeCellWidth; }
        public double CanvasHeight { get => MazeData.Size * Consts.ActualMazeCellWidth; }

        private int searchStep = 0;
        public int SearchStep { get => this.searchStep; set => SetValue(ref this.searchStep, value); }

        private bool initialized = false;
        public void InitializeData()
        {
            if (this.initialized) return;

            this.initialized = true;
        }

        public void UpdateMaze()
        {
            if (MazeFileList.Count > 0)
            {
                var path = MazeFileList[SelectedMazeFileIndex];
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    var mazeData = MazeData.Load(data);
                    if (mazeData.Validate())
                    {
                        MazeData = mazeData;
                        Graph = null;
                    }
                    stream.Close();
                }
            }
        }

        public void CalcMinimumPath()
        {
            if(MazeData != null)
            {
                var (graph, start, goal) = MakeGraph(MazeData);
                //var start = graph.Nodes.Where(n => n.Data == MazeData.Start)?.First() ?? null;
                //var goal = graph.Nodes.Where(n => n.Data == MazeData.Goals.First())?.First() ?? null;
                //var result = graph.GetMinimumPath(start, goal);
                var result = graph.Dijkstra(start, goal);
                if(result != null)
                {
                    result.Edges.RemoveAll(e => e.End == goal);
                    result.Nodes.Remove(goal);
                    Graph = result;
                }
            }
        }

        public void CalcMinimumPath2()
        {
            if (MazeData != null)
            {
                var (graph, start, goal) = MakeGraph2(MazeData);
                //var start = graph.Nodes.Where(n => n.Data == MazeData.Start)?.First() ?? null;
                //var goal = graph.Nodes.Where(n => n.Data == MazeData.Goals.First())?.First() ?? null;
                //var result = graph.GetMinimumPath(start, goal);
                var result = graph.Dijkstra(start, goal);
                if (result != null)
                {
                    result.Edges.RemoveAll(e => e.End == goal);
                    result.Nodes.Remove(goal);
                    Graph = result;
                }
            }
        }

        private (Graph<Point>, Node<Point>, Node<Point>) MakeGraph(MazeData mazeData)
        {
            var graph = new Graph<Point>()
            {
                Nodes = new List<Node<Point>>(),
                Edges = new List<Edge<Point>>(),
            };
            var nodes = new Dictionary<Cell, Node<Point>>();

            for (int x = 0; x < mazeData.Size; ++x)
            {
                for (int y = 0; y < mazeData.Size; ++y)
                {
                    var cell = mazeData.At(x, y);
                    var node = new Node<Point>() { Data = mazeData.GetCenterPoint(x, y), Incidents = new List<Edge<Point>>() };
                    nodes.Add(cell, node);
                }
            }
            graph.Nodes.AddRange(nodes.Values);

            var edges = graph.Edges;

            for (int x = 0; x < mazeData.Size; ++x)
            {
                for (int y = 0; y < mazeData.Size; ++y)
                {
                    var cell = mazeData.At(x, y);

                    var incidents = nodes[cell].Incidents;
                    if (!cell.East) incidents.Add(new Edge<Point>() { Start = nodes[cell], End = nodes[mazeData.At(x + 1, y)], Weight = 1.0 });
                    if (!cell.West) incidents.Add(new Edge<Point>() { Start = nodes[cell], End = nodes[mazeData.At(x - 1, y)], Weight = 1.0 });
                    if (!cell.North) incidents.Add(new Edge<Point>() { Start = nodes[cell], End = nodes[mazeData.At(x, y + 1)], Weight = 1.0 });
                    if (!cell.South) incidents.Add(new Edge<Point>() { Start = nodes[cell], End = nodes[mazeData.At(x, y - 1)], Weight = 1.0 });

                    foreach (var e in incidents) edges.Add(e);
                }
            }

            var point = mazeData.Goals.Aggregate(new Point(0.0, 0.0), (seed, cell) => {
                var p = mazeData.GetCenterPoint(cell.Pos.X, cell.Pos.Y);
                return seed + new Vector(p.X, p.Y);
            });
            graph.Nodes.Add(new Node<Point>()
            {
                Data = new Point(point.X / mazeData.Goals.Count(), point.Y / mazeData.Goals.Count()),
                Incidents = new List<Edge<Point>>(),
            });
            var goal = graph.Nodes.Last();
            foreach (var g in mazeData.Goals)
            {
                graph.Edges.Add(new Edge<Point>() { Start = nodes[g], End = goal, Weight = 1.0 });
                var e = graph.Edges.Last();
                //goal.Incidents.Add(e);
                nodes[g].Incidents.Add(e);
            }

            return (graph, nodes[mazeData.Start], goal);
        }

        private (Graph<Point>, Node<Point>, Node<Point>) MakeGraph2(MazeData mazeData)
        {
            var graph = new Graph<Point>()
            {
                Nodes = new List<Node<Point>>(),
                Edges = new List<Edge<Point>>(),
            };
            var nodes = new Dictionary<(Cell, DirectionType), Node<Point>>();

            for (int x = 0; x < mazeData.Size; ++x)
            {
                for (int y = 0; y < mazeData.Size; ++y)
                {
                    var cell = mazeData.At(x, y);
                    var v_center = new Node<Point>() { Data = mazeData.GetCenterPoint(x, y), Incidents = new List<Edge<Point>>() };
                    var v_north = new Node<Point>() { Data = mazeData.GetNorthPoint(x, y), Incidents = new List<Edge<Point>>() };
                    var v_south = new Node<Point>() { Data = mazeData.GetSouthPoint(x, y), Incidents = new List<Edge<Point>>() };
                    var v_east = new Node<Point>() { Data = mazeData.GetEastPoint(x, y), Incidents = new List<Edge<Point>>() };
                    var v_west = new Node<Point>() { Data = mazeData.GetWestPoint(x, y), Incidents = new List<Edge<Point>>() };
                    nodes.Add((cell, DirectionType.Center), v_center);
                    nodes.Add((cell, DirectionType.North), v_north);
                    nodes.Add((cell, DirectionType.South), v_south);
                    nodes.Add((cell, DirectionType.East), v_east);
                    nodes.Add((cell, DirectionType.West), v_west);
                }
            }
            graph.Nodes.AddRange(nodes.Values);

            var edges = graph.Edges;

            for (int x = 0; x < mazeData.Size; ++x)
            {
                for (int y = 0; y < mazeData.Size; ++y)
                {
                    var cell = mazeData.At(x, y);
                    if (!cell.East)
                    {
                        var incidents = nodes[(cell, DirectionType.East)].Incidents;
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.East)], End = nodes[(mazeData.At(x + 1, y), DirectionType.West)], Weight = 0.0 });
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.East)], End = nodes[(cell, DirectionType.Center)], Weight = 0.5 });
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.East)], End = nodes[(cell, DirectionType.North)], Weight = 0.5 * 1.414 });
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.East)], End = nodes[(cell, DirectionType.South)], Weight = 0.5 * 1.414 });
                    }
                    if (!cell.West)
                    {
                        var incidents = nodes[(cell, DirectionType.West)].Incidents;
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.West)], End = nodes[(mazeData.At(x - 1, y), DirectionType.East)], Weight = 0.0 });
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.West)], End = nodes[(cell, DirectionType.Center)], Weight = 0.5 });
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.West)], End = nodes[(cell, DirectionType.North)], Weight = 0.5 * 1.414 });
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.West)], End = nodes[(cell, DirectionType.South)], Weight = 0.5 * 1.414 });
                    }
                    if (!cell.North)
                    {
                        var incidents = nodes[(cell, DirectionType.North)].Incidents;
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.North)], End = nodes[(mazeData.At(x, y + 1), DirectionType.South)], Weight = 0.0 });
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.North)], End = nodes[(cell, DirectionType.Center)], Weight = 0.5 });
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.North)], End = nodes[(cell, DirectionType.West)], Weight = 0.5 * 1.414 });
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.North)], End = nodes[(cell, DirectionType.East)], Weight = 0.5 * 1.414 });
                    }
                    if (!cell.South)
                    {
                        var incidents = nodes[(cell, DirectionType.South)].Incidents;
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.South)], End = nodes[(mazeData.At(x, y - 1), DirectionType.North)], Weight = 0.0 });
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.South)], End = nodes[(cell, DirectionType.Center)], Weight = 0.5 });
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.South)], End = nodes[(cell, DirectionType.West)], Weight = 0.5 * 1.414 });
                        incidents.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.South)], End = nodes[(cell, DirectionType.East)], Weight = 0.5 * 1.414 });
                    }

                    if (cell.IsStart || cell.IsGoal)
                    {
                        var incidents_center = nodes[(cell, DirectionType.Center)].Incidents;
                        incidents_center.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.Center)], End = nodes[(cell, DirectionType.West)], Weight = 0.5 });
                        incidents_center.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.Center)], End = nodes[(cell, DirectionType.East)], Weight = 0.5 });
                        incidents_center.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.Center)], End = nodes[(cell, DirectionType.South)], Weight = 0.5 });
                        incidents_center.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.Center)], End = nodes[(cell, DirectionType.North)], Weight = 0.5 });
                    }
                    else
                    {
                        var incidents_center = nodes[(cell, DirectionType.Center)].Incidents;
                        incidents_center.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.Center)], End = nodes[(cell, DirectionType.West)], Weight = 0.5 });
                        incidents_center.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.Center)], End = nodes[(cell, DirectionType.East)], Weight = 0.5 });
                        incidents_center.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.Center)], End = nodes[(cell, DirectionType.South)], Weight = 0.5 });
                        incidents_center.Add(new Edge<Point>() { Start = nodes[(cell, DirectionType.Center)], End = nodes[(cell, DirectionType.North)], Weight = 0.5 });
                    }
                    foreach (var e in nodes[(cell, DirectionType.Center)].Incidents) edges.Add(e);
                    foreach (var e in nodes[(cell, DirectionType.East)].Incidents) edges.Add(e);
                    foreach (var e in nodes[(cell, DirectionType.West)].Incidents) edges.Add(e);
                    foreach (var e in nodes[(cell, DirectionType.North)].Incidents) edges.Add(e);
                    foreach (var e in nodes[(cell, DirectionType.South)].Incidents) edges.Add(e);
                }
            }

            var point = mazeData.Goals.Aggregate(new Point(0.0, 0.0), (seed, cell) => {
                var p = mazeData.GetCenterPoint(cell.Pos.X, cell.Pos.Y);
                return seed + new Vector(p.X, p.Y);
            });
            graph.Nodes.Add(new Node<Point>()
            {
                Data = new Point(point.X / mazeData.Goals.Count(), point.Y / mazeData.Goals.Count()),
                Incidents = new List<Edge<Point>>(),
            });
            var goal = graph.Nodes.Last();
            foreach (var g in mazeData.Goals)
            {
                graph.Edges.Add(new Edge<Point>() { Start = nodes[(g, DirectionType.Center)], End = goal, Weight = 1.0 });
                var e = graph.Edges.Last();
                //goal.Incidents.Add(e);
                nodes[(g, DirectionType.Center)].Incidents.Add(e);
            }

            return (graph, nodes[(mazeData.Start, DirectionType.Center)], goal);
        }

        //public void UpdateGraph()
        //{
        //    var graph = MazeData.ToGraph();
        //    if (graph != null)
        //    {
        //        var start = graph.Nodes?.Where(n => n.Data == MazeData.Start)?.First() ?? null;
        //        var goal = graph.Nodes?.Where(n => n.Data == MazeData.Goals.First())?.First() ?? null;

        //        if(start != null && goal != null)
        //        {
        //            Graph = graph.GetMinimumPath(start, goal);
        //        }
        //    }

        //    //if (Graph != null)
        //    //{
        //    //    var canvas = Graph.ToCanvas(Maze);
        //    //    GraphPresenter.Content = canvas;
        //    //    GraphPresenter.Content = new TextBlock() { Text = "sadfasfhaslkjfhask" };
        //    //}
        //}
    }
}
