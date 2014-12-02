using System.Windows;
using SrtEditor.Models;

namespace SrtEditor.Controls
{

    public class IntervalSelectedRoutedEventArgs : RoutedEventArgs
    {
        public IntervalModel Interval { get; private set; }
        public IntervalSelectedRoutedEventArgs(RoutedEvent re, object source, IntervalModel item)
            : base(re, source)
        {
            Interval = item;
        }
    }

}
