//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;
//using SrtEditor.Annotations;
//
//namespace SrtEditor.Data
//{
//    public class TestSrtList : ICollection<SrtInterval>, INotifyCollectionChanged, INotifyPropertyChanged
//    {
//        private SrtInterval _first;
//
//        public SrtInterval First
//        {
//            get
//            {
//                if (_first != null)
////                    _first = _first.FindFirst();
//                return _first;
//            }
//        }
//
//        public SrtInterval Last
//        {
//            get
//            {
//                SrtInterval result = null;
//                if (_first != null)
////                    result = _first.FindLast();
//                return result;
//            }
//        }
//
//        public IEnumerator<SrtInterval> GetEnumerator()
//        {
//            return ToList().GetEnumerator();
//        }
//
//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }
//
//        public void Add(SrtInterval item)
//        {
//            if (_first != null)
//            {
//                _first.Insert(item);
//            }
//            else
//            {
//                _first = item;
//            }
//            _first = _first.FindFirst();
//            item.Collection = this;
//        }
//
//        public void Clear()
//        {
//            if (_first != null)
//            {
//                _first = _first.FindFirst();
//                while (_first.Next != null)
//                {
//                    _first.Next.Remove();
//                }
//                _first = null;
//            }
//        }
//
//        public bool Contains(SrtInterval item)
//        {
//            return _first.Contains(item);
//        }
//
//        public void CopyTo(SrtInterval[] array, int arrayIndex)
//        {
//        }
//
//        public bool Remove(SrtInterval item)
//        {
//            if (_first.Equals(item))
//            {
//                _first = _first.Next;
//            }
//            item.Remove();
//            _first = _first.FindFirst();
//            return true;
//        }
//
//        public int Count
//        {
//            get
//            {
//                int i = 0;
//                if (_first != null)
//                {
//                    SrtInterval item = _first.FindFirst();
//                    do
//                    {
//                        i++;
//                        item = item.Next;
//                    } while (item != null);
//                }
//                return i;
//            }
//        }
//
//        public bool IsReadOnly
//        {
//            get { return false; }
//        }
//
//        public event NotifyCollectionChangedEventHandler CollectionChanged;
//
//        public event PropertyChangedEventHandler PropertyChanged;
//
//        public List<SrtInterval> Find(long time)
//        {
//            return _first == null ? new List<SrtInterval>() : _first.Find(time);
//        }
//
//        public List<SrtInterval> ToList()
//        {
//            var result = new List<SrtInterval>();
//            if (_first != null)
//            {
//                SrtInterval item = _first.FindFirst();
//                while (item != null)
//                {
//                    result.Add(item);
//                    item = item.Next;
//                }
//            }
//            return result;
//        }
//
//        public virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
//        {
//            NotifyCollectionChangedEventHandler handler = CollectionChanged;
//            if (handler != null) handler(this, e);
//        }
//
//        [NotifyPropertyChangedInvocator]
//        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChangedEventHandler handler = PropertyChanged;
//            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}