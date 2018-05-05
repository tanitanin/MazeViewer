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
        public MazeData MazeData { get => this.mazeData; private set => SetValue(ref this.mazeData, value); }

        private Graph graph = null;
        public Graph Graph { get => this.graph; private set => SetValueAndNotify(ref this.graph, value, nameof(Graph), nameof(MinimumStep)); }
        public int MinimumStep { get => Graph?.Edges?.Count() ?? 0; }

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
                var result = graph.GetMinimumPath(start, goal);
                result.Edges.RemoveAll(e => e.End == goal);
                result.Nodes.Remove(goal);
                Graph = result;
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
                    if (!cell.East) incidents.Add(new Edge<Point>() { Start = nodes[cell], End = nodes[mazeData.At(x + 1, y)] });
                    if (!cell.West) incidents.Add(new Edge<Point>() { Start = nodes[cell], End = nodes[mazeData.At(x - 1, y)] });
                    if (!cell.North) incidents.Add(new Edge<Point>() { Start = nodes[cell], End = nodes[mazeData.At(x, y + 1)] });
                    if (!cell.South) incidents.Add(new Edge<Point>() { Start = nodes[cell], End = nodes[mazeData.At(x, y - 1)] });

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
                graph.Edges.Add(new Edge<Point>() { Start = nodes[g], End = goal });
                var e = graph.Edges.Last();
                goal.Incidents.Add(e);
                nodes[g].Incidents.Add(e);
            }

            return (graph, nodes[mazeData.Start], goal);
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
