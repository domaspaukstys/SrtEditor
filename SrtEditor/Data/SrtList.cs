using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Documents;

namespace SrtEditor.Data
{
    public class SrtList : ICollection<SrtInterval>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private List<SrtInterval> _intervals;

        public SrtList()
        {
            _intervals = new List<SrtInterval>();
        }

        public IEnumerator<SrtInterval> GetEnumerator()
        {
            _intervals.Sort(IntervalsComparer);
            return _intervals.GetEnumerator();
        }

        private int IntervalsComparer(SrtInterval x, SrtInterval y)
        {
            return x.CompareTo(y);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(SrtInterval item)
        {
            _intervals.Add(item);
            item.Collection = this;
            _intervals.Sort(IntervalsComparer);
        }

        public void Clear()
        {
            _intervals.Clear();
        }

        public bool Contains(SrtInterval item)
        {
            return _intervals.Contains(item);
        }

        public void CopyTo(SrtInterval[] array, int arrayIndex)
        {
            _intervals.Sort(IntervalsComparer);
            _intervals.CopyTo(array, arrayIndex);
        }

        public bool Remove(SrtInterval item)
        {
            bool result = _intervals.Remove(item);
            _intervals.Sort(IntervalsComparer);
            return result;
        }

        public int Count
        {
            get { return _intervals.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public List<SrtInterval> Find(long time, SrtInterval srtInterval = null)
        {
            List<SrtInterval> result = _intervals.Where(x => x.From <= time && x.To > time && (srtInterval == null || !srtInterval.Equals(x))).ToList();
            return result;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null) handler(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Sort()
        {
            _intervals.Sort();
        }
    }
}