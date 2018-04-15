using MazeViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static Canvas ToCanvas(this Maze maze)
        {
            var canvas = new Canvas();
            canvas.Width = maze.Size * 10;
            canvas.Height = maze.Size * 10;
            canvas.Background = new SolidColorBrush(BaseColor);
            for (int x = 0; x < maze.Size; ++x)
            {
                for (int y = 0; y < maze.Size; ++y)
                {
                    var n = maze.Size;
                    var north = new Line() { X1 = GetNorthWest(x, y, n).X, Y1 = GetNorthWest(x, y, n).Y, X2 = GetNorthEast(x, y, n).X, Y2 = GetNorthEast(x, y, n).Y, StrokeThickness = WallWidth, Stroke = new SolidColorBrush(WallTopColor) };
                    var east =  new Line() { X1 = GetNorthEast(x, y, n).X, Y1 = GetNorthEast(x, y, n).Y, X2 = GetSouthEast(x, y, n).X, Y2 = GetSouthEast(x, y, n).Y, StrokeThickness = WallWidth, Stroke = new SolidColorBrush(WallTopColor) };
                    var south = new Line() { X1 = GetSouthWest(x, y, n).X, Y1 = GetSouthWest(x, y, n).Y, X2 = GetSouthEast(x, y, n).X, Y2 = GetSouthEast(x, y, n).Y, StrokeThickness = WallWidth, Stroke = new SolidColorBrush(WallTopColor) };
                    var west =  new Line() { X1 = GetNorthWest(x, y, n).X, Y1 = GetNorthWest(x, y, n).Y, X2 = GetSouthWest(x, y, n).X, Y2 = GetSouthWest(x, y, n).Y, StrokeThickness = WallWidth, Stroke = new SolidColorBrush(WallTopColor) };
                    if (maze.At(x, y).North) canvas.Children.Add(north);
                    if (maze.At(x, y).East) canvas.Children.Add(east);
                    if (maze.At(x, y).South) canvas.Children.Add(south);
                    if (maze.At(x, y).West) canvas.Children.Add(west);
                }
            }
            return canvas;
        }

        private static AbsPos2d GetNorthWest(int x, int y, int n) => new AbsPos2d { X = x * CellWidth, Y = (n - y - 1) * CellWidth };
        private static AbsPos2d GetNorthEast(int x, int y, int n) => new AbsPos2d { X= (x + 1) * CellWidth, Y = (n - y - 1) * CellWidth };
        private static AbsPos2d GetSouthEast(int x, int y, int n) => new AbsPos2d { X = (x + 1) * CellWidth, Y = (n - y) * CellWidth };
        private static AbsPos2d GetSouthWest(int x, int y, int n) => new AbsPos2d { X = x * CellWidth, Y = (n - y) * CellWidth };
    }
}
