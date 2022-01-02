using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    public class EventList<T> : IList<T>
    {
        public event Action<EventList<T>, T> OnAdd;
        public event Action<EventList<T>, T> OnRemove;
        public event Action<EventList<T>, int> OnRemoveAt;
        public event Action<EventList<T>, int, T> OnInsert;

        public event Action<EventList<T>> OnClear;


        public event Action<EventList<T>, T> OnIn;
        public event Action<EventList<T>, T> OnOut;

        public event Action<EventList<T>, T> OnInOut;

        public EventList()
        {
            this.OnAdd += (t, item) => OnIn?.Invoke(t, item);
            this.OnRemove += (t, item) => OnOut?.Invoke(t, item);
            this.OnRemoveAt += (t, index) => OnOut?.Invoke(t, t[index]);
            this.OnClear += t =>
            {
                if (OnRemove != default)
                {
                    foreach (var item in t)
                    {
                        OnRemove.Invoke(t, item);
                    }
                }
            };
            this.OnInsert += (t, index, item) => OnIn?.Invoke(t, item);

            this.OnIn += (t, item) => OnInOut?.Invoke(t, item);
            this.OnOut += (t, item) => OnInOut?.Invoke(t, item);
        }

        private List<T> items = new List<T>();

        public T this[int index] { get => ((IList<T>)items)[index]; set => ((IList<T>)items)[index] = value; }

        public int Count => ((ICollection<T>)items).Count;

        public bool IsReadOnly => ((ICollection<T>)items).IsReadOnly;

        public void Add(T item)
        {
            OnAdd?.Invoke(this, item);
            ((ICollection<T>)items).Add(item);
        }

        public void Clear()
        {
            //if (OnOut != default)
            //{
            //    foreach (var item in this)
            //    {
            //        OnOut.Invoke(this, item);
            //    }
            //}
            OnClear?.Invoke(this);
            ((ICollection<T>)items).Clear();
        }

        public bool Contains(T item)
        {
            return ((ICollection<T>)items).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)items).CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return ((IList<T>)items).IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            //  OnIn?.Invoke(this, item);
            OnInsert?.Invoke(this, index, item);
            ((IList<T>)items).Insert(index, item);
        }

        public bool Remove(T item)
        {
            //   OnOut?.Invoke(this, item);
            OnRemove?.Invoke(this, item);
            return ((ICollection<T>)items).Remove(item);
        }

        public void RemoveAt(int index)
        {
            //if (OnOut != default)
            //{
            //    OnOut.Invoke(this, this[index]);
            //}
            OnRemoveAt?.Invoke(this, index);
            ((IList<T>)items).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }
    }
}
