using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SrtEditor.Annotations;
using SrtEditor.Commands;
using SrtEditor.Data;
using SrtEditor.Models;

namespace SrtEditor.Controls
{
    /// <summary>
    ///     Interaction logic for TimelineControl.xaml
    /// </summary>
    public partial class TimelineControl : IModel
    {
        private const double PixelsPerSecond = 5D;

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(double), typeof(TimelineControl),
                new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    CurrentTimeChanged));

        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register("ZoomLevel", typeof(double), typeof(TimelineControl),
                new FrameworkPropertyMetadata(1D, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    ZoomLevelChanged));

        public static readonly DependencyProperty CurrentPositionProperty =
            DependencyProperty.Register("CurrentPosition", typeof(double), typeof(TimelineControl),
                new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                    ));


        public static readonly DependencyProperty TimelineWidthProperty =
            DependencyProperty.Register("TimelineWidth", typeof(double), typeof(TimelineControl),
                new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty LengthProperty =
            DependencyProperty.Register("Length", typeof(double), typeof(TimelineControl),
                new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, LengthChanged));

        public static readonly DependencyProperty SrtCollectionProperty =
            DependencyProperty.Register("SrtCollection", typeof(SrtList), typeof(TimelineControl),
                new FrameworkPropertyMetadata(default(SrtList), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SrtCollectionChanged));

        private readonly List<RowDefinition> _definitions;
        private readonly List<TimeDisplayControl> _displayTime;
        private readonly Dictionary<SrtInterval, IntervalControl> _intervals;
        private IntervalModel _selectedInterval;

        public IntervalModel SelectedInterval
        {
            get { return _selectedInterval; }
            set
            {

                if (Equals(value, _selectedInterval)) return;
                _selectedInterval = value;
                RemoveInterval.OnCanExecuteChanged();
                OnIntervalSelected(this, value);
                OnPropertyChanged();
            }
        }

        public TimelineControl()
        {
            _displayTime = new List<TimeDisplayControl>();
            _intervals = new Dictionary<SrtInterval, IntervalControl>();
            _definitions = new List<RowDefinition>();
            ZoomIn = new ZoomInCommand(this);
            ZoomOut = new ZoomOutCommand(this);
            AddInterval = new AddIntervalCommand(this);
            RemoveInterval = new RemoveIntervalCommand(this);
            InitializeComponent();
        }

        public ModelCommand<TimelineControl> ZoomIn { get; private set; }
        public ModelCommand<TimelineControl> ZoomOut { get; private set; }
        public ModelCommand<TimelineControl> AddInterval { get; private set; }
        public ModelCommand<TimelineControl> RemoveInterval { get; private set; }

        public double ZoomLevel
        {
            get { return (double)GetValue(ZoomLevelProperty); }
            set
            {
                SetValue(ZoomLevelProperty, value);
                OnPropertyChanged();
                ZoomOut.OnCanExecuteChanged();
            }
        }

        public SrtList SrtCollection
        {
            get { return (SrtList)GetValue(SrtCollectionProperty); }
            set { SetValue(SrtCollectionProperty, value); }
        }

        public double CurrentPosition
        {
            get { return (double)GetValue(CurrentPositionProperty); }
            set
            {
                SetValue(CurrentPositionProperty, value);
                OnPropertyChanged();
            }
        }

        public double CurrentTime
        {
            get { return (double)GetValue(CurrentTimeProperty); }
            set
            {
                SetValue(CurrentTimeProperty, value);
                OnPropertyChanged();
            }
        }

        public double Length
        {
            get { return (double)GetValue(LengthProperty); }
            set
            {
                SetValue(LengthProperty, value);
                OnPropertyChanged();
            }
        }

        public double TimelineWidth
        {
            get { return (double)GetValue(TimelineWidthProperty); }
            set
            {
                SetValue(TimelineWidthProperty, value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static void CurrentTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TimelineControl;
            if (control != null)
            {
                control.CurrentPosition = control.MsToPixels((double)e.NewValue);
                control.UpdateScroll();
            }
        }

        private void UpdateScroll()
        {
            if (AutoScroll.IsChecked == true)
            {
                Scroller.ScrollToHorizontalOffset(CurrentPosition - (Scroller.ActualWidth / 2));
            }
        }

        private static void ZoomLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TimelineControl;
            if (control != null)
            {
                control.CurrentPosition = control.MsToPixels((double)e.NewValue);
                control.UpdateScroll();
            }
        }

        private static void LengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var length = e.NewValue as double?;
            var control = d as TimelineControl;
            if (length.HasValue && control != null)
            {
                control.TimelineWidth = control.MsToPixels(length.Value);
                control.RefreshTimeline();
            }
        }

        private static void SrtCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newVal = e.NewValue as SrtList;

            if (newVal != null && d is TimelineControl)
            {
                ((TimelineControl)d).AttachToCollection(newVal);
            }
        }

        public void ChangeZoomLevel(double delta)
        {
            if (ZoomLevel + delta >= 1)
            {
                ZoomLevel += delta;

                foreach (TimeDisplayControl control in _displayTime)
                {
                    control.PositionX = MsToPixels(control.Time);
                    control.Width = MsToPixels(10000);
                }

                foreach (IntervalControl value in _intervals.Values)
                {
                    value.Model.PixelsToSecond = ZoomLevel * PixelsPerSecond;
                }

                TimelineWidth = MsToPixels(Length);
                CurrentPosition = MsToPixels(CurrentTime);
            }
        }

        public void AttachToCollection(SrtList newCollection)
        {
            newCollection.CollectionChanged += OnSrtCollectionChanged;
        }

        private void OnSrtCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            ResetIntervals();
        }

        public void ResetIntervals()
        {
            foreach (IntervalControl control in _intervals.Values)
            {
                control.IntervalSelected -= ControlOnIntervalSelected;
                Timeline.Children.Remove(control);
            }
            foreach (RowDefinition definition in _definitions)
            {
                Timeline.RowDefinitions.Remove(definition);
            }
            _definitions.Clear();
            _intervals.Clear();
            int currentRow = 1;
            bool firstOverLap = true;
            foreach (SrtInterval interval in SrtCollection)
            {
                var control = new IntervalControl(new IntervalModel(interval, MsToPixels(1000)));
                if (interval.Overlaps())
                {
                    if (!firstOverLap)
                    {
                        currentRow++;
                    }
                    firstOverLap = false;
                }
                else
                {
                    firstOverLap = true;
                    currentRow = 1;
                }
                if (_definitions.Count < currentRow)
                {
                    var definition = new RowDefinition { Height = GridLength.Auto };
                    Timeline.RowDefinitions.Add(definition);
                    _definitions.Add(definition);
                }
                Grid.SetRow(control, currentRow);
                Grid.SetColumn(control, 0);
                _intervals.Add(interval, control);
                control.IntervalSelected += ControlOnIntervalSelected;
                Timeline.Children.Add(control);
            }
        }

        private void ControlOnIntervalSelected(object sender, RoutedEventArgs routedEventArgs)
        {
            IntervalSelectedRoutedEventArgs args = routedEventArgs as IntervalSelectedRoutedEventArgs;
            if (args != null)
            {
                SelectedInterval = args.Interval;
            }
        }

        public static readonly RoutedEvent IntervalSelectedEvent = EventManager.RegisterRoutedEvent("IntervalSelected",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TimelineControl));

        public event RoutedEventHandler IntervalSelected
        {
            add { AddHandler(IntervalSelectedEvent, value); }
            remove { RemoveHandler(IntervalSelectedEvent, value); }
        }

        protected void OnIntervalSelected(object sender, IntervalModel item)
        {
            IntervalSelectedRoutedEventArgs args = new IntervalSelectedRoutedEventArgs(IntervalSelectedEvent, sender, item);
            RaiseEvent(args);
        }

        public void RefreshTimeline()
        {
            foreach (TimeDisplayControl control in _displayTime)
            {
                Timeline.Children.Remove(control);
            }
            _displayTime.Clear();
            var count = (long)Math.Ceiling(Length / 10000D);
            for (long i = 0; i < count; i++)
            {
                var control = new TimeDisplayControl
                {
                    Text = SrtInterval.LongToTime(i * 10000L),
                    Time = i * 10000,
                    PositionX = MsToPixels(i * 10000),
                    Width = MsToPixels(10000)
                };
                Grid.SetRow(control, 0);
                Grid.SetColumn(control, 0);
                Timeline.Children.Add(control);
                _displayTime.Add(control);
            }
        }

        public double MsToPixels(double ms)
        {
            return ms / 1000 * PixelsPerSecond * ZoomLevel;
        }

        public double PixelsToMs(double px)
        {
            return px * 1000 / PixelsPerSecond / ZoomLevel;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Timeline_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(Timeline);

            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Released)
            {
                CurrentTime = PixelsToMs(pos.X);
            }
            
        }
    }
}