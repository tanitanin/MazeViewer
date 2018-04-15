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

        public List<string> MazeFileList { get; } = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MazeFileList.AddRange(Directory.EnumerateFiles(@"MazeData").ToList());
            DataSelector.ItemsSource = MazeFileList.Select(x => System.IO.Path.GetFileName(x));
            DataSelector.SelectedIndex = 0;
        }

        private void DataSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            var path = MazeFileList[combo.SelectedIndex];
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                Maze = Maze.Load(data);
                stream.Close();
            }

            var flg = Maze.Validate();
            var canvas = Maze.ToCanvas();
            Presenter.Content = Maze.ToCanvas();
            Presenter.Width = canvas.Width;
            Presenter.Height = canvas.Height;
        }
    }
}
