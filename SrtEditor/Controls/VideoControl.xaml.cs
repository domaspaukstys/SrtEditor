using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SrtEditor.Annotations;
using SrtEditor.Commands;
using SrtEditor.Data;
using SrtEditor.Models;

namespace SrtEditor.Controls
{
    /// <summary>
    ///     Interaction logic for VideoControl.xaml
    /// </summary>
    public partial class VideoControl : IModel
    {
        public static readonly DependencyProperty VideoSourceProperty =
            DependencyProperty.Register("VideoSource", typeof(string), typeof(VideoControl),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    VideoSourceChanged));

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(double), typeof(VideoControl),
                new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CurrentTimeChanged));

        private static void CurrentTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public static readonly DependencyProperty VideoLengthProperty =
            DependencyProperty.Register("VideoLength", typeof(double), typeof(VideoControl),
                new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty VideoEnabledProperty =
            DependencyProperty.Register("VideoEnabled", typeof(bool), typeof(VideoControl),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SrtCollectionProperty =
            DependencyProperty.Register("SrtCollection", typeof(SrtList), typeof(VideoControl),
                new FrameworkPropertyMetadata(default(SrtList), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsPaused
        {
            get; set;
        }

        public SrtList SrtCollection
        {
            get { return (SrtList)GetValue(SrtCollectionProperty); }
            set { SetValue(SrtCollectionProperty, value); }
        }

        private SrtInterval LastInterval { get; set; }

        private readonly List<ModelCommand<VideoControl>> _commands;
        private bool _bindingChange;

        public VideoControl()
        {
            Timer = new DispatcherTimer(TimeSpan.FromMilliseconds(200D), DispatcherPriority.Normal, OnTick, Dispatcher)
            {
                IsEnabled = false
            };
            Play = new PlayCommand(this);
            Stop = new StopCommand(this);
            Pause = new PauseCommand(this);
            PlayPause = new PlayPauseCommand(this);

            _commands = new List<ModelCommand<VideoControl>> { Play, Stop, Pause };
            InitializeComponent();
        }

        public DispatcherTimer Timer { get; private set; }

        public ModelCommand<VideoControl> Play { get; set; }
        public ModelCommand<VideoControl> Stop { get; set; }
        public ModelCommand<VideoControl> Pause { get; set; }
        public ModelCommand<VideoControl> PlayPause { get; set; }

        public string VideoSource
        {
            get { return (string)GetValue(VideoSourceProperty); }
            set
            {
                SetValue(VideoSourceProperty, value);
                OnPropertyChanged();
            }
        }

        public bool VideoEnabled
        {
            get { return (bool)GetValue(VideoEnabledProperty); }
            set
            {
                SetValue(VideoEnabledProperty, value);
                OnPropertyChanged();
            }
        }

        public double VideoLength
        {
            get { return (double)GetValue(VideoLengthProperty); }
            set
            {
                SetValue(VideoLengthProperty, value);
                OnPropertyChanged();
            }
        }

        public double CurrentTime
        {
            get { return (double)GetValue(CurrentTimeProperty); }
            set
            {
                _bindingChange = true;
                SetValue(CurrentTimeProperty, value);
                OnPropertyChanged();
                List<SrtInterval> intervals = null;
                if (SrtCollection != null)
                {
                    intervals = SrtCollection.Find((long)value);
                }
                if (intervals != null && intervals.Count > 0)
                {
                    LastInterval = intervals[0];
                    Subtitles = intervals.Select(x => x.Text).Aggregate(GetSubs).Replace(Environment.NewLine, "<br>");
                }
                else
                {
                    Subtitles = string.Empty;
                }
                _bindingChange = false;
            }
        }

        private string GetSubs(string arg1, string arg2)
        {
            return arg1 + Environment.NewLine + arg2;
        }

        private string _subtitles;

        public string Subtitles
        {
            get { return _subtitles; }
            set
            {
                if (_subtitles != value)
                {
                    _subtitles = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static void VideoSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VideoControl;
            var newValue = e.NewValue as string;
            if (control != null && !string.IsNullOrEmpty(newValue))
            {
                control.CloseVideo();
                control.Player.Source = new Uri(newValue);
                control.Player.Play();
            }
        }

        private void CloseVideo()
        {
            VideoEnabled = false;
            CurrentTime = 0D;
            VideoLength = 0D;
            Timer.Stop();

            Player.Source = null;
            _commands.ForEach(x => x.OnCanExecuteChanged());
        }

        private void OpenVideo()
        {
            if (Player.NaturalDuration.HasTimeSpan)
            {
                VideoLength = Player.NaturalDuration.TimeSpan.TotalMilliseconds;

                VideoEnabled = true;

                Timer.Start();
                _commands.ForEach(x => x.OnCanExecuteChanged());
            }
            else
            {
                CloseVideo();
                MessageBox.Show("Video broken");
            }
        }


        private void OnTick(object sender, EventArgs e)
        {
            CurrentTime = Player.Position.TotalMilliseconds;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnMediaOpened(object sender, RoutedEventArgs e)
        {
            OpenVideo();
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            if (slider != null && !_bindingChange)
            {
                Player.Position = TimeSpan.FromMilliseconds(e.NewValue);
                CurrentTime = Player.Position.TotalMilliseconds;
            }
        }
    }
}