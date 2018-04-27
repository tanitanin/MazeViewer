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
            Notify(propertyName);
        }
        public void SetValueAndNotify<T>(ref T target, T value, [CallerMemberName] string propertyName = "", params string[] properties)
        {
            target = value;
            Notify(propertyName);
            Notify(properties);
        }
        public void Notify(params string[] properties) => properties.ForEach(s => Notify(s));
        public void Notify([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
