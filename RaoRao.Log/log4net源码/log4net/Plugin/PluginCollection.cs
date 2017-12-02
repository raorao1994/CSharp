namespace log4net.Plugin
{
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Reflection;

    public class PluginCollection : IList, ICollection, IEnumerable, ICloneable
    {
        private const int DEFAULT_CAPACITY = 0x10;
        private IPlugin[] m_array;
        private int m_count;
        private int m_version;

        public PluginCollection()
        {
            this.m_count = 0;
            this.m_version = 0;
            this.m_array = new IPlugin[0x10];
        }

        public PluginCollection(PluginCollection c)
        {
            this.m_count = 0;
            this.m_version = 0;
            this.m_array = new IPlugin[c.Count];
            this.AddRange(c);
        }

        protected internal PluginCollection(Tag tag)
        {
            this.m_count = 0;
            this.m_version = 0;
            this.m_array = null;
        }

        public PluginCollection(ICollection col)
        {
            this.m_count = 0;
            this.m_version = 0;
            this.m_array = new IPlugin[col.Count];
            this.AddRange(col);
        }

        public PluginCollection(int capacity)
        {
            this.m_count = 0;
            this.m_version = 0;
            this.m_array = new IPlugin[capacity];
        }

        public PluginCollection(IPlugin[] a)
        {
            this.m_count = 0;
            this.m_version = 0;
            this.m_array = new IPlugin[a.Length];
            this.AddRange(a);
        }

        public virtual int Add(IPlugin item)
        {
            if (this.m_count == this.m_array.Length)
            {
                this.EnsureCapacity(this.m_count + 1);
            }
            this.m_array[this.m_count] = item;
            this.m_version++;
            return this.m_count++;
        }

        public virtual int AddRange(PluginCollection x)
        {
            if ((this.m_count + x.Count) >= this.m_array.Length)
            {
                this.EnsureCapacity(this.m_count + x.Count);
            }
            Array.Copy(x.m_array, 0, this.m_array, this.m_count, x.Count);
            this.m_count += x.Count;
            this.m_version++;
            return this.m_count;
        }

        public virtual int AddRange(IPlugin[] x)
        {
            if ((this.m_count + x.Length) >= this.m_array.Length)
            {
                this.EnsureCapacity(this.m_count + x.Length);
            }
            Array.Copy(x, 0, this.m_array, this.m_count, x.Length);
            this.m_count += x.Length;
            this.m_version++;
            return this.m_count;
        }

        public virtual int AddRange(ICollection col)
        {
            if ((this.m_count + col.Count) >= this.m_array.Length)
            {
                this.EnsureCapacity(this.m_count + col.Count);
            }
            foreach (object obj2 in col)
            {
                this.Add((IPlugin) obj2);
            }
            return this.m_count;
        }

        public virtual void Clear()
        {
            this.m_version++;
            this.m_array = new IPlugin[0x10];
            this.m_count = 0;
        }

        public virtual object Clone()
        {
            PluginCollection plugins = new PluginCollection(this.m_count);
            Array.Copy(this.m_array, 0, plugins.m_array, 0, this.m_count);
            plugins.m_count = this.m_count;
            plugins.m_version = this.m_version;
            return plugins;
        }

        public virtual bool Contains(IPlugin item)
        {
            for (int i = 0; i != this.m_count; i++)
            {
                if (this.m_array[i].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void CopyTo(IPlugin[] array)
        {
            this.CopyTo(array, 0);
        }

        public virtual void CopyTo(IPlugin[] array, int start)
        {
            if (this.m_count > ((array.GetUpperBound(0) + 1) - start))
            {
                throw new ArgumentException("Destination array was not long enough.");
            }
            Array.Copy(this.m_array, 0, array, start, this.m_count);
        }

        private void EnsureCapacity(int min)
        {
            int num = (this.m_array.Length == 0) ? 0x10 : (this.m_array.Length * 2);
            if (num < min)
            {
                num = min;
            }
            this.Capacity = num;
        }

        public virtual IPluginCollectionEnumerator GetEnumerator() => 
            new Enumerator(this);

        public virtual int IndexOf(IPlugin item)
        {
            for (int i = 0; i != this.m_count; i++)
            {
                if (this.m_array[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public virtual void Insert(int index, IPlugin item)
        {
            this.ValidateIndex(index, true);
            if (this.m_count == this.m_array.Length)
            {
                this.EnsureCapacity(this.m_count + 1);
            }
            if (index < this.m_count)
            {
                Array.Copy(this.m_array, index, this.m_array, index + 1, this.m_count - index);
            }
            this.m_array[index] = item;
            this.m_count++;
            this.m_version++;
        }

        public static PluginCollection ReadOnly(PluginCollection list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            return new ReadOnlyPluginCollection(list);
        }

        public virtual void Remove(IPlugin item)
        {
            int index = this.IndexOf(item);
            if (index < 0)
            {
                throw new ArgumentException("Cannot remove the specified item because it was not found in the specified Collection.");
            }
            this.m_version++;
            this.RemoveAt(index);
        }

        public virtual void RemoveAt(int index)
        {
            this.ValidateIndex(index);
            this.m_count--;
            if (index < this.m_count)
            {
                Array.Copy(this.m_array, index + 1, this.m_array, index, this.m_count - index);
            }
            IPlugin[] sourceArray = new IPlugin[1];
            Array.Copy(sourceArray, 0, this.m_array, this.m_count, 1);
            this.m_version++;
        }

        void ICollection.CopyTo(Array array, int start)
        {
            Array.Copy(this.m_array, 0, array, start, this.m_count);
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            ((IEnumerator) this.GetEnumerator());

        int IList.Add(object x) => 
            this.Add((IPlugin) x);

        bool IList.Contains(object x) => 
            this.Contains((IPlugin) x);

        int IList.IndexOf(object x) => 
            this.IndexOf((IPlugin) x);

        void IList.Insert(int pos, object x)
        {
            this.Insert(pos, (IPlugin) x);
        }

        void IList.Remove(object x)
        {
            this.Remove((IPlugin) x);
        }

        void IList.RemoveAt(int pos)
        {
            this.RemoveAt(pos);
        }

        public virtual void TrimToSize()
        {
            this.Capacity = this.m_count;
        }

        private void ValidateIndex(int i)
        {
            this.ValidateIndex(i, false);
        }

        private void ValidateIndex(int i, bool allowEqualEnd)
        {
            int num = allowEqualEnd ? this.m_count : (this.m_count - 1);
            if ((i < 0) || (i > num))
            {
                throw SystemInfo.CreateArgumentOutOfRangeException("i", i, "Index was out of range. Must be non-negative and less than the size of the collection. [" + i + "] Specified argument was out of the range of valid values.");
            }
        }

        public virtual int Capacity
        {
            get => 
                this.m_array.Length;
            set
            {
                if (value < this.m_count)
                {
                    value = this.m_count;
                }
                if (value != this.m_array.Length)
                {
                    if (value > 0)
                    {
                        IPlugin[] destinationArray = new IPlugin[value];
                        Array.Copy(this.m_array, 0, destinationArray, 0, this.m_count);
                        this.m_array = destinationArray;
                    }
                    else
                    {
                        this.m_array = new IPlugin[0x10];
                    }
                }
            }
        }

        public virtual int Count =>
            this.m_count;

        public virtual bool IsFixedSize =>
            false;

        public virtual bool IsReadOnly =>
            false;

        public virtual bool IsSynchronized =>
            this.m_array.IsSynchronized;

        public virtual IPlugin this[int index]
        {
            get
            {
                this.ValidateIndex(index);
                return this.m_array[index];
            }
            set
            {
                this.ValidateIndex(index);
                this.m_version++;
                this.m_array[index] = value;
            }
        }

        public virtual object SyncRoot =>
            this.m_array.SyncRoot;

        object IList.this[int i]
        {
            get => 
                this[i];
            set
            {
                this[i] = (IPlugin) value;
            }
        }

        private sealed class Enumerator : IEnumerator, PluginCollection.IPluginCollectionEnumerator
        {
            private readonly PluginCollection m_collection;
            private int m_index;
            private int m_version;

            internal Enumerator(PluginCollection tc)
            {
                this.m_collection = tc;
                this.m_index = -1;
                this.m_version = tc.m_version;
            }

            public bool MoveNext()
            {
                if (this.m_version != this.m_collection.m_version)
                {
                    throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
                }
                this.m_index++;
                return (this.m_index < this.m_collection.Count);
            }

            public void Reset()
            {
                this.m_index = -1;
            }

            public IPlugin Current =>
                this.m_collection[this.m_index];

            object IEnumerator.Current =>
                this.Current;
        }

        public interface IPluginCollectionEnumerator
        {
            bool MoveNext();
            void Reset();

            IPlugin Current { get; }
        }

        private sealed class ReadOnlyPluginCollection : PluginCollection
        {
            private readonly PluginCollection m_collection;

            internal ReadOnlyPluginCollection(PluginCollection list) : base(PluginCollection.Tag.Default)
            {
                this.m_collection = list;
            }

            public override int Add(IPlugin x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override int AddRange(PluginCollection x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override int AddRange(IPlugin[] x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override void Clear()
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override bool Contains(IPlugin x) => 
                this.m_collection.Contains(x);

            public override void CopyTo(IPlugin[] array)
            {
                this.m_collection.CopyTo(array);
            }

            public override void CopyTo(IPlugin[] array, int start)
            {
                this.m_collection.CopyTo(array, start);
            }

            public override PluginCollection.IPluginCollectionEnumerator GetEnumerator() => 
                this.m_collection.GetEnumerator();

            public override int IndexOf(IPlugin x) => 
                this.m_collection.IndexOf(x);

            public override void Insert(int pos, IPlugin x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override void Remove(IPlugin x)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override void RemoveAt(int pos)
            {
                throw new NotSupportedException("This is a Read Only Collection and can not be modified");
            }

            public override int Capacity
            {
                get => 
                    this.m_collection.Capacity;
                set
                {
                    throw new NotSupportedException("This is a Read Only Collection and can not be modified");
                }
            }

            public override int Count =>
                this.m_collection.Count;

            public override bool IsFixedSize =>
                true;

            public override bool IsReadOnly =>
                true;

            public override bool IsSynchronized =>
                this.m_collection.IsSynchronized;

            public override IPlugin this[int i]
            {
                get => 
                    this.m_collection[i];
                set
                {
                    throw new NotSupportedException("This is a Read Only Collection and can not be modified");
                }
            }

            public override object SyncRoot =>
                this.m_collection.SyncRoot;
        }

        internal protected enum Tag
        {
            Default
        }
    }
}

