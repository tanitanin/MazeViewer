using MazeViewer.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

namespace MazeViewer.Helpers
{
    public class MazeToCanvasConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch(value)
            {
                case MazeData maze:
                    var markEnabled = MainWindow.Current?.ViewModel.MarkEnabled;
                    return markEnabled.HasValue ? maze.ToCanvas(markEnabled.Value) : maze.ToCanvas();
                default: return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
