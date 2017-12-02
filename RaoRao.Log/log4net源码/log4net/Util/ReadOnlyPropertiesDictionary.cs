namespace log4net.Util
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Xml;

    [Serializable]
    public class ReadOnlyPropertiesDictionary : ISerializable, IDictionary, ICollection, IEnumerable
    {
        private Hashtable m_hashtable;

        public ReadOnlyPropertiesDictionary()
        {
            this.m_hashtable = new Hashtable();
        }

        public ReadOnlyPropertiesDictionary(ReadOnlyPropertiesDictionary propertiesDictionary)
        {
            this.m_hashtable = new Hashtable();
            foreach (DictionaryEntry entry in (IEnumerable) propertiesDictionary)
            {
                this.InnerHashtable.Add(entry.Key, entry.Value);
            }
        }

        protected ReadOnlyPropertiesDictionary(SerializationInfo info, StreamingContext context)
        {
            this.m_hashtable = new Hashtable();
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                string introduced3 = XmlConvert.DecodeName(current.Name);
                this.InnerHashtable[introduced3] = current.Value;
            }
        }

        public virtual void Clear()
        {
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }

        public bool Contains(string key) => 
            this.InnerHashtable.Contains(key);

        public string[] GetKeys()
        {
            string[] array = new string[this.InnerHashtable.Count];
            this.InnerHashtable.Keys.CopyTo(array, 0);
            return array;
        }

        [SecurityCritical]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (DictionaryEntry entry in this.InnerHashtable)
            {
                string key = entry.Key as string;
                object obj2 = entry.Value;
                if (((key != null) && (obj2 != null)) && obj2.GetType().IsSerializable)
                {
                    info.AddValue(XmlConvert.EncodeLocalName(key), obj2);
                }
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.InnerHashtable.CopyTo(array, index);
        }

        void IDictionary.Add(object key, object value)
        {
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }

        bool IDictionary.Contains(object key) => 
            this.InnerHashtable.Contains(key);

        IDictionaryEnumerator IDictionary.GetEnumerator() => 
            this.InnerHashtable.GetEnumerator();

        void IDictionary.Remove(object key)
        {
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            ((IEnumerable) this.InnerHashtable).GetEnumerator();

        public int Count =>
            this.InnerHashtable.Count;

        protected Hashtable InnerHashtable =>
            this.m_hashtable;

        public virtual object this[string key]
        {
            get => 
                this.InnerHashtable[key];
            set
            {
                throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
            }
        }

        bool ICollection.IsSynchronized =>
            this.InnerHashtable.IsSynchronized;

        object ICollection.SyncRoot =>
            this.InnerHashtable.SyncRoot;

        bool IDictionary.IsFixedSize =>
            this.InnerHashtable.IsFixedSize;

        bool IDictionary.IsReadOnly =>
            true;

        object IDictionary.this[object key]
        {
            get
            {
                if (!(key is string))
                {
                    throw new ArgumentException("key must be a string");
                }
                return this.InnerHashtable[key];
            }
            set
            {
                throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
            }
        }

        ICollection IDictionary.Keys =>
            this.InnerHashtable.Keys;

        ICollection IDictionary.Values =>
            this.InnerHashtable.Values;
    }
}

