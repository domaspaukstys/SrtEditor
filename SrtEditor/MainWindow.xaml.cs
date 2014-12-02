using System.Windows;
using SrtEditor.Controls;
using SrtEditor.Models;

namespace SrtEditor
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Model = (MainModel) DataContext;
            
        }

        public MainModel Model { get; private set; }

        private void OnIntervalSelected(object sender, RoutedEventArgs e)
        {
            IntervalSelectedRoutedEventArgs args = e as IntervalSelectedRoutedEventArgs;
            if (args != null)
            {
                Model.SelectedInterval = args.Interval;
            }
        }
    }
}