using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SrtEditor.Annotations;

namespace SrtEditor.Controls
{
    /// <summary>
    ///     Interaction logic for TimeDisplayControl.xaml
    /// </summary>
    public partial class TimeDisplayControl : INotifyPropertyChanged
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (TimeDisplayControl),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty PositionXProperty =
            DependencyProperty.Register("PositionX", typeof (double), typeof (TimeDisplayControl),
                new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public TimeDisplayControl()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                OnPropertyChanged();
            }
        }

        public double PositionX
        {
            get { return (double) GetValue(PositionXProperty); }
            set
            {
                SetValue(PositionXProperty, value);
                OnPropertyChanged();
            }
        }

        public double Time { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}