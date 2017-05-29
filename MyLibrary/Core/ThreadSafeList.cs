using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.Serialization;

namespace JSegarra.Core
{

    public class ThreadList<T> : IEnumerable<T>
    {

        private readonly List<T> items = new List<T>();
        public long LongCount
        {
            get
            {
                lock (items) return items.LongCount();
            }
        }

        public IEnumerator<T> GetEnumerator() { return this.Clone().GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        public T Add(T item)
        {
            if (Equals(default(T), item)) return default(T);
            else lock (items)  items.Add(item);
            return item;
        }

        public void Clear()
        {
            lock (items)  items.Clear();
        }

        public bool Contains(T item)
        {
            lock (items)  return items.Contains(item);
        }

        public bool Remove(T item)
        {
            lock (items)  return items.Remove(item);
        }

        public int Count
        {
            get
            {
                lock (items)  return items.Count;
            }
        }

        public int IndexOf(T item)
        {
            lock (items)  return items.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            lock (items)  items.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                lock (items)  return items[index];
            }
            set
            {
                lock (items)  items[index] = value;
            }
        }

        public List<T> Clone(Boolean asParallel = true)
        {
            lock (items) return asParallel ? new List<T>(items.AsParallel()): new List<T>(items);
            
        }
    }
}
