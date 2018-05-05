﻿using MazeViewer.Core;
using MazeViewer.Core.Algorithm;
using MazeViewer.Helpers;
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

        private MazeData mazeData = null;
        public MazeData MazeData { get => this.mazeData; private set => SetValue(ref this.mazeData, value); }

        private Graph graph = null;
        public Graph Graph { get => this.graph; private set => SetValueAndNotify(ref this.graph, value, nameof(Graph), nameof(MinimumStep)); }
        public int MinimumStep { get => Graph?.Edges?.Count() ?? 0; }

        public int SelectedMazeFileIndex { get; set; } = 0;
        public ObservableCollection<string> MazeFileList { get; private set; } = new ObservableCollection<string>(Directory.EnumerateFiles(@"MazeData"));

        public Agent Agent { get; } = new Agent();

        public bool MarkEnabled { get; set; } = true;

        private double scale = 1.0;
        public double Scale { get => this.scale; set => SetValue(ref this.scale, value); }

        
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
                var graph = MazeData.ToGraph();
                var start = graph.Nodes.Where(n => n.Cell == MazeData.Start)?.First() ?? null;
                var goal = graph.Nodes.Where(n => n.Cell == MazeData.Goals.First())?.First() ?? null;
                Graph = graph.GetMinimumPath(start, goal);
            }
        }

        public void UpdateGraph()
        {
            var graph = MazeData.ToGraph();
            if (graph != null)
            {
                var start = graph.Nodes?.Where(n => n.Cell == MazeData.Start)?.First() ?? null;
                var goal = graph.Nodes?.Where(n => n.Cell == MazeData.Goals.First())?.First() ?? null;

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
