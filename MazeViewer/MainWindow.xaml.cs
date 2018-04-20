using MazeViewer.Helpers;
using MazeViewer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MazeViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        public Maze Maze { get; private set; } = new Maze();
        public Graph Graph { get; private set; } = null;

        public List<string> MazeFileList { get; } = new List<string>();

        public bool EnableMark { get; set; } = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MazeFileList.AddRange(Directory.EnumerateFiles(@"MazeData").ToList());
            DataSelector.ItemsSource = MazeFileList.Select(x => System.IO.Path.GetFileName(x));
            DataSelector.SelectedIndex = 0;
        }

        private void DataSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void UpdateMaze()
        {
            if(MazeFileList.Count > 0)
            {
                var path = MazeFileList[DataSelector.SelectedIndex];
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    Maze = Maze.Load(data);
                    stream.Close();
                }

                var flg = Maze.Validate();
                var canvas = Maze.ToCanvas(EnableMark);
                Presenter.Content = canvas;
            }
        }

        private void UpdateGraph()
        {
            if (Graph != null)
            {
                var canvas = Graph.ToCanvas(Maze);
                GraphPresenter.Content = canvas;
                GraphPresenter.Content = new TextBlock() { Text = "sadfasfhaslkjfhask" };
            }
        }

        private void ShowMarkCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EnableMark = true;
            Update();
        }

        private void ShowMarkCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EnableMark = false;
            Update();
        }

        private void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            var graph = Maze.ToGraph();
            if(graph != null)
            {
                var start = graph.Nodes.Where(n => n.Cell == Maze.Start)?.First() ?? null;
                var goal = graph.Nodes.Where(n => n.Cell == Maze.Goals.First())?.First() ?? null;
                Graph = graph.MinimumPath(start, goal);
                UpdateGraph();
            }
        }

        private void Update()
        {
            UpdateMaze();
            UpdateGraph();
        }
    }
}
