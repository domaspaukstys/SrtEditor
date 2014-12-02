using System.Windows;
using System.Windows.Controls.Primitives;
using SrtEditor.Models;

namespace SrtEditor.Controls
{
    /// <summary>
    ///     Interaction logic for IntervalControl.xaml
    /// </summary>
    public partial class IntervalControl
    {
        public IntervalControl()
        {
            InitializeComponent();
        }

        public IntervalControl(IntervalModel model) : this()
        {
            Model = model;
            DataContext = Model;
        }

        public IntervalModel Model { get; private set; }

        private void OnDragMin(object sender, DragDeltaEventArgs e)
        {
            if (Model.PositionX + e.HorizontalChange > 0)
            {
                Model.PositionX += e.HorizontalChange;
            }
            if (Model.Width - e.HorizontalChange > 0)
            {
                Model.Width -= e.HorizontalChange;
            }
        }

        private void OnDragMax(object sender, DragDeltaEventArgs e)
        {
            if (Model.Width + e.HorizontalChange > 0)
            {
                Model.Width += e.HorizontalChange;
            }
        }

        private void Sort(object sender, DragCompletedEventArgs e)
        {
            Model.Sort();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            OnIntervalSelected();
        }

        public static readonly RoutedEvent IntervalSelectedEvent = EventManager.RegisterRoutedEvent("IntervalSelected",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (IntervalControl));

        public event RoutedEventHandler IntervalSelected
        {
            add { AddHandler(IntervalSelectedEvent, value);}
            remove { RemoveHandler(IntervalSelectedEvent, value);}
        }

        protected void OnIntervalSelected()
        {
            IntervalSelectedRoutedEventArgs args = new IntervalSelectedRoutedEventArgs(IntervalSelectedEvent, this, Model);
            RaiseEvent(args);
        }
    }
}