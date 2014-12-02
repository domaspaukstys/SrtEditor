using System;
using System.Collections.Specialized;
using SrtEditor.Data;

namespace SrtEditor.Models
{
    public class IntervalModel : Model
    {
        public SrtInterval Interval { get; private set; }
        private double _pixelsToSecond;
        private double _positionX;
        private bool _updating;
        private double _width;

        public IntervalModel(SrtInterval interval, double pts)
        {
            Interval = interval;
            _pixelsToSecond = pts;
            ModelToView();
        }

        public string Text
        {
            get { return Interval.Text; }
            set
            {
                Interval.Text = value;
                OnPropertyChanged();
            }
        }

        public string From
        {
            get { return SrtInterval.LongToTime(Interval.From); }
        }

        public string To
        {
            get { return SrtInterval.LongToTime(Interval.To); }
        }

        public double PositionX
        {
            get { return _positionX; }
            set
            {
                SetField(ref _positionX, value);
                ViewToModel();
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                SetField(ref _width, value);
                ViewToModel();
            }
        }

        public double PixelsToSecond
        {
            get { return _pixelsToSecond; }
            set
            {
                if (Math.Abs(_pixelsToSecond - value) > 0.01)
                {
                    _pixelsToSecond = value;
                    ModelToView();
                }
            }
        }

        private void ModelToView()
        {
            _updating = true;

            PositionX = Interval.From / 1000D * _pixelsToSecond;
            Width = (Interval.To - Interval.From) / 1000D * _pixelsToSecond;

            _updating = false;
        }

        private void ViewToModel()
        {
            if (!_updating)
            {
                Interval.From = (long)(_positionX * 1000 / _pixelsToSecond);
                Interval.To = (long)((_positionX + _width) * 1000 / _pixelsToSecond);
                OnPropertyChanged("From");
                OnPropertyChanged("To");
            }
        }

        public void Sort()
        {
            if (Interval.OverlapChanged())
            {
                Interval.Resort();
                Interval.Collection.OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
    }
}