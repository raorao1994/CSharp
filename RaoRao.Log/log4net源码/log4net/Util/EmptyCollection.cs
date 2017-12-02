namespace log4net.Util
{
    using System;
    using System.Collections;

    [Serializable]
    public sealed class EmptyCollection : ICollection, IEnumerable
    {
        private static readonly EmptyCollection s_instance = new EmptyCollection();

        private EmptyCollection()
        {
        }

        public void CopyTo(Array array, int index)
        {
        }

        public IEnumerator GetEnumerator() => 
            NullEnumerator.Instance;

        public int Count =>
            0;

        public static EmptyCollection Instance =>
            s_instance;

        public bool IsSynchronized =>
            true;

        public object SyncRoot =>
            this;
    }
}

