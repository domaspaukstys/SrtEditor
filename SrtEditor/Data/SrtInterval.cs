using System;

namespace SrtEditor.Data
{
    public class SrtInterval : IComparable, IComparable<SrtInterval>, IEquatable<SrtInterval>
    {
        private bool _wasOverlapping;
        private SrtList _collection;
        private string _text;

        public SrtInterval(long from, long to)
        {
            From = from;
            To = to;
        }

        public long From { get; set; }
        public long To { get; set; }

        public string Text
        {
            get { return _text ?? ""; }
            set { _text = value; }
        }

        public SrtList Collection
        {
            get { return _collection; }
            set
            {
                _collection = value;
                _wasOverlapping = Overlaps();
            }
        }

        public void AppendText(string t)
        {
            if (string.IsNullOrEmpty(Text))
            {
                Text = t;
            }
            else
            {
                Text += Environment.NewLine + t;
            }
        }
        public static string LongToTime(long time)
        {
            string h = AddTrailingZeroes(time / 3600000L, 2);
            time = time % 3600000L;
            string min = AddTrailingZeroes(time / 60000L, 2);
            time = time % 60000L;
            string s = AddTrailingZeroes(time / 1000, 2);
            string ms = AddTrailingZeroes(time % 1000, 3);
            return string.Format("{0}:{1}:{2},{3}", h, min, s, ms);
        }
        private static string AddTrailingZeroes(object val, int length)
        {
            string result = val.ToString();
            while (result.Length < length)
            {
                result = "0" + result;
            }
            return result;
        }
        #region  IComparable, IComparable<SrtInterval>, IEquatable<SrtInterval>

        public int CompareTo(object obj)
        {
            int result = 0;
            var i = obj as SrtInterval;
            if (i != null)
            {
                result = From.CompareTo(i.From);
            }
            return result;
        }

        public int CompareTo(SrtInterval other)
        {
            return From.CompareTo(other.From);
        }

        public bool Equals(SrtInterval other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return From == other.From && To == other.To && string.Equals(Text, other.Text);
        }

        public override int GetHashCode()
        {
            return From.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SrtInterval)obj);
        }

        #endregion

        public void Resort()
        {
            if (Collection != null)
            {
                Collection.Sort();
            }
        }

        public bool Overlaps()
        {
            bool result = false;
            if (Collection != null)
            {
                result = Collection.Find(From, this).Count > 0 || Collection.Find(To, this).Count > 0;
            }
            return result;
        }

        public bool OverlapChanged()
        {
            bool newO = Overlaps();
            bool result = newO != _wasOverlapping;
            _wasOverlapping = newO;
            return result;
        }
    }
}