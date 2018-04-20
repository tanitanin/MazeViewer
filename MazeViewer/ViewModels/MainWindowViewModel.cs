using MazeViewer.Helpers;
using MazeViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindow View { get; }

        public MainWindowViewModel(MainWindow mainWindow)
        {
            View = mainWindow;
        }

        private Maze maze = null;
        public Maze Maze { get => this.maze; private set => SetValue(ref this.maze, value); }

        private Graph graph = null;
        public Graph Graph { get => this.graph; private set => SetValue(ref this.graph, value); }

        public int SelectedMazeFileIndex { get; set; } = 0;
        public ObservableCollection<string> MazeFileList { get; private set; } = new ObservableCollection<string>(Directory.EnumerateFiles(@"MazeData"));

        public bool MarkEnabled { get; set; } = true;

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
                    var maze = Maze.Load(data);
                    if (maze.Validate())
                    {
                        Maze = maze;
                        Graph = null;
                    }
                    stream.Close();
                }
            }
        }

        public void CalcMinimumPath()
        {
            if(Maze != null)
            {
                var graph = Maze.ToGraph();
                var start = graph.Nodes.Where(n => n.Cell == Maze.Start)?.First() ?? null;
                var goal = graph.Nodes.Where(n => n.Cell == Maze.Goals.First())?.First() ?? null;
                Graph = graph.GetMinimumPath(start, goal);
            }
        }

        public void UpdateGraph()
        {
            var graph = Maze.ToGraph();
            if (graph != null)
            {
                var start = graph.Nodes?.Where(n => n.Cell == Maze.Start)?.First() ?? null;
                var goal = graph.Nodes?.Where(n => n.Cell == Maze.Goals.First())?.First() ?? null;

                if(start != null && goal != null)
                {
                    Graph = graph.GetMinimumPath(start, goal);
                }
            }

            //if (Graph != null)
            //{
            //    var canvas = Graph.ToCanvas(Maze);
            //    GraphPresenter.Content = canvas;
            //    GraphPresenter.Content = new TextBlock() { Text = "sadfasfhaslkjfhask" };
            //}
        }
    }
}
