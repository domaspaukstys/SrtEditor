using System;
using System.Collections.Specialized;
using SrtEditor.Commands;
using SrtEditor.Data;

namespace SrtEditor.Models
{
    public class MainModel : Model
    {
        private double _currentTime;
        private string _text;
        private double _videoLength;
        private bool _videoLoaded;
        private string _videoSource;
        private IntervalModel _selectedInterval;

        public MainModel()
        {
            OpenSrt = new OpenSrtCommand(this);
            OpenVideo = new OpenVideoCommand(this);
            SaveSrt = new SaveSrtCommand(this);
            SrtDocument = new SrtList();
            SrtDocument.CollectionChanged += SrtDocumentOnCollectionChanged;
        }

        private void SrtDocumentOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            SaveSrt.OnCanExecuteChanged();
        }

        public ModelCommand<MainModel> OpenSrt { get; private set; }
        public ModelCommand<MainModel> SaveSrt { get; private set; }
        public ModelCommand<MainModel> OpenVideo { get; private set; }
        public SrtList SrtDocument { get; private set; }

        public string VideoSource
        {
            get { return _videoSource; }
            set { SetField(ref _videoSource, value); }
        }

        public string Text
        {
            get { return _text; }
            set { SetField(ref _text, value); }
        }

        public double CurrentTime
        {
            get { return _currentTime; }
            set { SetField(ref _currentTime, value); }
        }

        public double VideoLength
        {
            get { return _videoLength; }
            set { SetField(ref _videoLength, value); }
        }

        public bool VideoLoaded
        {
            get { return _videoLoaded; }
            set
            {
                SetField(ref _videoLoaded, value);
                OpenSrt.OnCanExecuteChanged();
            }
        }

        public IntervalModel SelectedInterval
        {
            get { return _selectedInterval; }
            set
            {
                SetField(ref _selectedInterval, value);
                OnPropertyChanged("IntervalSelected");
            }
        }

        public bool IntervalSelected
        {
            get { return _selectedInterval != null; }
        }
    }
}