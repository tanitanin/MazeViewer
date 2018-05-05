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
using MazeViewer.ViewModels;

namespace MazeViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Current { get; private set; }
        public MainWindowViewModel ViewModel { get; }

        public MainWindow()
        {
            Current = this;
            ViewModel = new MainWindowViewModel(this);
            this.DataContext = ViewModel;
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.InitializeData();
        }

        private void DataSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.UpdateMaze();
        }

        //private void ShowMarkCheckBox_Checked(object sender, RoutedEventArgs e)
        //{
        //    ViewModel.UpdateMaze();
        //}

        //private void ShowMarkCheckBox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    ViewModel.UpdateMaze();
        //}

        private void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CalcMinimumPath();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
