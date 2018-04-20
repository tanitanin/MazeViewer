using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Helpers
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public void SetValue<T>(ref T target, T value, [CallerMemberName] string propertyName = "")
        {
            target = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
