//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Windows.Automation.Peers;
//
//namespace SrtEditor.Data
//{
//    public class TestSrtInterval : IComparable, IComparable<SrtInterval>, IEquatable<SrtInterval>
//    {
//        public TestSrtInterval(long from, long to)
//        {
//            From = from;
//            To = to;
//        }
//
//        public long From { get; set; }
//        public long To { get; set; }
//        public string Text { get; set; }
//
//        public SrtInterval Previous { get; private set; }
//
//        public SrtInterval Next { get; private set; }
//
//        public SrtList Collection { get; set; }
//
//        public void AppendText(string t)
//        {
//            if (string.IsNullOrEmpty(Text))
//            {
//                Text = t;
//            }
//            else
//            {
//                Text += Environment.NewLine + t;
//            }
//        }
//
//        public void Insert(SrtInterval i)
//        {
//            if (CompareTo(i) > 0)
//            {
//                if (Previous != null)
//                {
//                    if (Previous.CompareTo(i) < 0)
//                    {
//                        i.Previous = Previous;
//                        i.Next = this;
//                        Previous.Next = i;
//                        Previous = i;
//                    }
//                    else
//                    {
//                        Previous.Insert(i);
//                    }
//                }
//                else
//                {
//                    i.Next = this;
//                    Previous = i;
//                }
//            }
//            else
//            {
//                if (Next != null)
//                {
//                    if (Next.CompareTo(i) > 0)
//                    {
//                        i.Next = Next;
//                        i.Previous = this;
//                        Next.Previous = i;
//                        Next = i;
//                    }
//                    else
//                    {
//                        Next.Insert(i);
//                    }
//                }
//                else
//                {
//                    i.Previous = this;
//                    Next = i;
//                }
//            }
//        }
//
//        public List<SrtInterval> Find(long time)
//        {
//            var result = new List<SrtInterval>();
//            var item = this;
//            bool moveBackwards = time < From;
//            do
//            {
//                if (item.From < time && item.To >= time)
//                {
//                    result.Add(item);
//                }
//                item = moveBackwards ? item.Previous : item.Next;
//            } while (item != null && ((item.From < time && item.To >= time) || (moveBackwards && item.From > time)
//                                      || (!moveBackwards && item.To < time)
//
//                ));
//            return result;
//        }
//
//        public bool Overlaps()
//        {
//            bool result = Next != null && Next.Overlaps();
//            if (!result)
//            {
//                var item = this;
//                while (item.Previous != null)
//                {
//                    if (item.Previous.To > From)
//                    {
//                        result = true;
//                        break;
//                    }
//                    item = item.Previous;
//                }
//            }
//            return result;
//        }
//
//        public void Resort()
//        {
//            SrtInterval item = FindFirst();
//            do
//            {
//                if (item.Previous != null && item.CompareTo(item.Previous) < 0)
//                {
//                    item.Previous.Next = item.Next;
//                    item.Next = item.Previous;
//                    item.Previous = item.Next.Previous;
//                    item.Next.Previous = this;
//                    if (item.Equals(item.Previous))
//                    {
//
//                    }
//                    if (item.Equals(item.Next))
//                    {
//
//                    }
//                }
//                else
//                {
//                    item = item.Next;
//                }
//            } while (item != null);
//            Collection.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
//        }
//
//        public SrtInterval FindFirst()
//        {
//            SrtInterval item = this;
//            while (item.Previous != null)
//            {
//                if (item.Equals(item.Previous))
//                    break;
//                item = item.Previous;
//
//            }
//            return item;
//        }
//
//        public SrtInterval FindLast()
//        {
//            SrtInterval current = this;
//            while (current.Next != null)
//            {
//                current = current.Next;
//            }
//            return current;
//        }
//
//        public bool Contains(SrtInterval i)
//        {
//            bool result = false;
//            SrtInterval current = FindFirst();
//            do
//            {
//                if (current.Equals(i))
//                {
//                    result = true;
//                    break;
//                }
//                current = current.Next;
//            } while (current != null);
//            return result;
//        }
//
//        public void Remove()
//        {
//            if (Previous != null)
//            {
//                Previous.Next = Next;
//            }
//            if (Next != null)
//            {
//                Next.Previous = Previous;
//            }
//            Collection = null;
//            Next = null;
//            Previous = null;
//        }
//
//        public static string LongToTime(long time)
//        {
//            string h = AddTrailingZeroes(time / 3600000L, 2);
//            time = time % 3600000L;
//            string min = AddTrailingZeroes(time / 60000L, 2);
//            time = time % 60000L;
//            string s = AddTrailingZeroes(time / 1000, 2);
//            string ms = AddTrailingZeroes(time % 1000, 3);
//            return string.Format("{0}:{1}:{2},{3}", h, min, s, ms);
//        }
////
//        private static string AddTrailingZeroes(object val, int length)
//        {
//            string result = val.ToString();
//            while (result.Length < length)
//            {
//                result = "0" + result;
//            }
//            return result;
//        }
//
//        public override string ToString()
//        {
//            return string.Format(@"{0} --> {1}{2}{3}", LongToTime(From), LongToTime(To), Environment.NewLine, Text);
//        }
//
//        #region  IComparable, IComparable<SrtInterval>, IEquatable<SrtInterval>
//
//        public int CompareTo(object obj)
//        {
//            int result = 0;
//            var i = obj as SrtInterval;
//            if (i != null)
//            {
//                result = From.CompareTo(i.From);
//            }
//            return result;
//        }
//
//        public int CompareTo(SrtInterval other)
//        {
//            return From.CompareTo(other.From);
//        }
//
//        public bool Equals(SrtInterval other)
//        {
//            if (ReferenceEquals(null, other)) return false;
//            if (ReferenceEquals(this, other)) return true;
//            return From == other.From && To == other.To && string.Equals(Text, other.Text) &&
//                   Equals(Previous, other.Previous) && Equals(Next, other.Next);
//        }
//
//        public override int GetHashCode()
//        {
//            return From.GetHashCode();
//        }
//
//        public override bool Equals(object obj)
//        {
//            if (ReferenceEquals(null, obj)) return false;
//            if (ReferenceEquals(this, obj)) return true;
//            if (obj.GetType() != GetType()) return false;
//            return Equals((SrtInterval)obj);
//        }
//
//        #endregion
//    }
//}