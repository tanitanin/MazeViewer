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
    public static class MazeExtension
    {
        static Color BaseColor = Colors.DarkSlateGray;
        static Color WallTopColor = Colors.Red;
        static double WallWidth = 1.0;
        static double CellWidth = 10.0;

        public static Canvas ToCanvas(this Maze maze, bool showMark = false)
        {
            var canvas = new Canvas();
            canvas.Width = maze.Size * 10;
            canvas.Height = maze.Size * 10;
            canvas.Background = new SolidColorBrush(BaseColor);
            for (int x = 0; x < maze.Size; ++x)
            {
                for (int y = 0; y < maze.Size; ++y)
                {
                    // 四隅の座標を求める
                    var n = maze.Size;
                    var nw = GetNorthWest(x, y, n);
                    var ne = GetNorthEast(x, y, n);
                    var sw = GetSouthWest(x, y, n);
                    var se = GetSouthEast(x, y, n);

                    // スタートとゴールを塗る
                    if (showMark)
                    {
                        if (maze.At(x, y).IsStart)
                        {
                            var text = new TextBox { Text = "S", FontSize = 10 };
                            var p = new Polygon() { Fill = new SolidColorBrush(Colors.HotPink) };
                            p.Points.Add(ne);
                            p.Points.Add(nw);
                            p.Points.Add(sw);
                            p.Points.Add(se);
                            canvas.Children.Add(p);
                        }
                        if (maze.At(x, y).IsGoal)
                        {
                            var text = new TextBox { Text = "G", FontSize = 10 };
                            var p = new Polygon() { Fill = new SolidColorBrush(Colors.Blue) };
                            p.Points.Add(ne);
                            p.Points.Add(nw);
                            p.Points.Add(sw);
                            p.Points.Add(se);
                            canvas.Children.Add(p);
                        }
                    }

                    // 壁を塗る
                    var north = new Line() { X1 = nw.X, Y1 = nw.Y, X2 = ne.X, Y2 = ne.Y, StrokeThickness = WallWidth, Stroke = new SolidColorBrush(WallTopColor) };
                    var east = new Line() { X1 = ne.X, Y1 = ne.Y, X2 = se.X, Y2 = se.Y, StrokeThickness = WallWidth, Stroke = new SolidColorBrush(WallTopColor) };
                    var south = new Line() { X1 = sw.X, Y1 = sw.Y, X2 = se.X, Y2 = se.Y, StrokeThickness = WallWidth, Stroke = new SolidColorBrush(WallTopColor) };
                    var west = new Line() { X1 = nw.X, Y1 = nw.Y, X2 = sw.X, Y2 = sw.Y, StrokeThickness = WallWidth, Stroke = new SolidColorBrush(WallTopColor) };
                    if (maze.At(x, y).North) canvas.Children.Add(north);
                    if (maze.At(x, y).East) canvas.Children.Add(east);
                    if (maze.At(x, y).South) canvas.Children.Add(south);
                    if (maze.At(x, y).West) canvas.Children.Add(west);
                }
            }
            return canvas;
        }

        private static Point GetNorthWest(int x, int y, int n) => new Point { X = x * CellWidth, Y = (n - y - 1) * CellWidth };
        private static Point GetNorthEast(int x, int y, int n) => new Point { X= (x + 1) * CellWidth, Y = (n - y - 1) * CellWidth };
        private static Point GetSouthEast(int x, int y, int n) => new Point { X = (x + 1) * CellWidth, Y = (n - y) * CellWidth };
        private static Point GetSouthWest(int x, int y, int n) => new Point { X = x * CellWidth, Y = (n - y) * CellWidth };
    }
}
