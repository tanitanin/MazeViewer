using MazeViewer.Core.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MazeViewer.Core
{
    public static class Renderer
    {
        public static Path RenderCell(this MazeData mazeData, int x, int y, Color color)
        {
            return new Path()
            {
                Fill = new SolidColorBrush(color),
                Data = new RectangleGeometry(new System.Windows.Rect()
                {
                    X = x * mazeData.CellWidth,
                    Y = (mazeData.Size - y - 1) * mazeData.CellWidth,
                    Width = mazeData.CellWidth,
                    Height = mazeData.CellWidth,
                }),
            };
        }

        public static Path RenderWall(this MazeData mazeData, int x, int y, DirectionType directionType, Color color)
        {
            var horizontal = (directionType == DirectionType.North || directionType == DirectionType.South) ? true : false;
            switch (directionType)
            {
                case DirectionType.North:
                    return new Path()
                    {
                        Stroke = new SolidColorBrush(color),
                        StrokeThickness = mazeData.WallWidth,
                        Data = new LineGeometry() {
                            StartPoint = mazeData.GetNorthEastPoint(x, y),
                            EndPoint = mazeData.GetNorthWestPoint(x, y),
                        },
                    };
                case DirectionType.South:
                    return new Path()
                    {
                        Stroke = new SolidColorBrush(color),
                        StrokeThickness = mazeData.WallWidth,
                        Data = new LineGeometry()
                        {
                            StartPoint = mazeData.GetSouthEastPoint(x, y),
                            EndPoint = mazeData.GetSouthWestPoint(x, y),
                        },
                    };
                case DirectionType.West:
                    return new Path()
                    {
                        Stroke = new SolidColorBrush(color),
                        StrokeThickness = mazeData.WallWidth,
                        Data = new LineGeometry()
                        {
                            StartPoint = mazeData.GetNorthWestPoint(x, y),
                            EndPoint = mazeData.GetSouthWestPoint(x, y),
                        },
                    };
                case DirectionType.East:
                    return new Path()
                    {
                        Stroke = new SolidColorBrush(color),
                        StrokeThickness = mazeData.WallWidth,
                        Data = new LineGeometry()
                        {
                            StartPoint = mazeData.GetNorthEastPoint(x, y),
                            EndPoint = mazeData.GetSouthEastPoint(x, y),
                        },
                    };
            }
            return null;
        }

        public static Canvas Render(this MazeData mazeData)
        {
            var canvas = new Canvas
            {
                Width = mazeData.Size * mazeData.CellWidth,
                Height = mazeData.Size * mazeData.CellWidth,
            };

            // 土台
            var basePath = new Path()
            {
                Fill = new SolidColorBrush(Colors.DarkSlateGray),
                Data = new RectangleGeometry(new Rect() {
                    X = 0.0,
                    Y = 0.0,
                    Width = mazeData.Size * mazeData.CellWidth,
                    Height = mazeData.Size * mazeData.CellWidth,
                }),
                Width = mazeData.Size * mazeData.CellWidth,
                Height = mazeData.Size * mazeData.CellWidth,
            };
            canvas.Children.Add(basePath);
            
            // スタート
            var start = mazeData.RenderCell(mazeData.Start.Pos.X, mazeData.Start.Pos.Y, Colors.DeepPink);
            canvas.Children.Add(start);

            // ゴール
            foreach(var goal in mazeData.Goals)
            {
                var g = mazeData.RenderCell(goal.Pos.X, goal.Pos.Y, Colors.Blue);
                canvas.Children.Add(g);
            }

            // 壁
            for (int y = 0; y < mazeData.Size; ++y)
            {
                for (int x = 0; x < mazeData.Size; ++x)
                {
                    if (mazeData.At(x, y).North)
                    {
                        var north = mazeData.RenderWall(x, y, DirectionType.North, Colors.Red);
                        canvas.Children.Add(north);
                    }
                    if (mazeData.At(x, y).East)
                    {
                        var east = mazeData.RenderWall(x, y, DirectionType.East, Colors.Red);
                        canvas.Children.Add(east);
                    }
                    if (mazeData.At(x, y).South)
                    {
                        var south = mazeData.RenderWall(x, y, DirectionType.South, Colors.Red);
                        canvas.Children.Add(south);
                    }
                    if (mazeData.At(x, y).West)
                    {
                        var west = mazeData.RenderWall(x, y, DirectionType.West, Colors.Red);
                        canvas.Children.Add(west);
                    }
                }
            }
            
            return canvas;
        }

        public static Canvas Render(this Graph<Point> graph)
        {
            var canvas = new Canvas();

            // ノード
            foreach(var node in graph.Nodes)
            {
                var n = new Path()
                {
                    Fill = new SolidColorBrush(Colors.White),
                    Data = new EllipseGeometry()
                    {
                        Center = node.Data,
                        RadiusX = 1.2,
                        RadiusY = 1.2,
                    },
                };
                canvas.Children.Add(n);
            }

            // エッジ
            foreach (var edge in graph.Edges)
            {
                var n = new Path()
                {
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 1.0,
                    Data = new LineGeometry()
                    {
                        StartPoint = edge.Start.Data,
                        EndPoint = edge.End.Data,
                    },
                };
                canvas.Children.Add(n);
            }

            return canvas;
        }
    }
}
