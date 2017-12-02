namespace log4net.Util
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public sealed class EmptyDictionary : IDictionary, ICollection, IEnumerable
    {
        private static readonly EmptyDictionary s_instance = new EmptyDictionary();

        private EmptyDictionary()
        {
        }

        public void Add(object key, object value)
        {
            throw new InvalidOperationException();
        }

        public void Clear()
        {
            throw new InvalidOperationException();
        }

        public bool Contains(object key) => 
            false;

        public void CopyTo(Array array, int index)
        {
        }

        public IDictionaryEnumerator GetEnumerator() => 
            NullDictionaryEnumerator.Instance;

        public void Remove(object key)
        {
            throw new InvalidOperationException();
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            NullEnumerator.Instance;

        public int Count =>
            0;

        public static EmptyDictionary Instance =>
            s_instance;

        public bool IsFixedSize =>
            true;

        public bool IsReadOnly =>
            true;

        public bool IsSynchronized =>
            true;

        public object this[object key]
        {
            get => 
                null;
            set
            {
                throw new InvalidOperationException();
            }
        }

        public ICollection Keys =>
            EmptyCollection.Instance;

        public object SyncRoot =>
            this;

        public ICollection Values =>
            EmptyCollection.Instance;
    }
}

