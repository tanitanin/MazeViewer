using MazeViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MazeViewer.Helpers
{
    public static class GraphExtension
    {
        public static Color NodeColor { get; } = Colors.White;
        public static Color EdgeColor { get; } = Colors.White;

        public static Canvas ToCanvas(this Graph graph, Maze maze)
        {
            if (maze == null) return null;

            var canvas = new Canvas();

            MainWindow.Current.Dispatcher.Invoke(() =>
            {
                canvas.Width = maze.Size * 10;
                canvas.Height = maze.Size * 10;

                foreach (var edge in graph.Edges)
                {
                    var start = GetCenter(maze, edge.Start.Cell);
                    var end = GetCenter(maze, edge.End.Cell);
                    canvas.Children.Add(new Line()
                    {
                        X1 = start.X,
                        Y1 = start.Y,
                        X2 = end.X,
                        Y2 = end.Y,
                        Stroke = new SolidColorBrush(EdgeColor),
                        StrokeThickness = 1.0,
                    });
                }

                foreach(var node in graph.Nodes.Where(n => n.Incidents.Count() == 0))
                {
                    var center = GetCenter(maze, node.Cell);
                    canvas.Children.Add(new Line()
                    {
                        X1 = center.X + 5,
                        Y1 = center.Y + 5,
                        X2 = center.X - 5,
                        Y2 = center.Y - 5,
                        Stroke = new SolidColorBrush(Colors.Red),
                        StrokeThickness = 1.0,
                    });
                    canvas.Children.Add(new Line()
                    {
                        X1 = center.X + 5,
                        Y1 = center.Y - 5,
                        X2 = center.X - 5,
                        Y2 = center.Y + 5,
                        Stroke = new SolidColorBrush(Colors.Red),
                        StrokeThickness = 1.0,
                    });
                }
            });

            return canvas;
        }

        private static Point GetCenter(Maze maze, Cell cell) => new Point {
            X = (double)cell.Pos.X * MazeExtension.CellWidth + (MazeExtension.CellWidth / 2),
            Y = (double)(maze.Size - cell.Pos.Y) * MazeExtension.CellWidth - (MazeExtension.CellWidth / 2),
        };

    }
}
