namespace log4net.Util
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class CompositeProperties
    {
        private PropertiesDictionary m_flattened = null;
        private ArrayList m_nestedProperties = new ArrayList();

        internal CompositeProperties()
        {
        }

        public void Add(ReadOnlyPropertiesDictionary properties)
        {
            this.m_flattened = null;
            this.m_nestedProperties.Add(properties);
        }

        public PropertiesDictionary Flatten()
        {
            if (this.m_flattened == null)
            {
                this.m_flattened = new PropertiesDictionary();
                int count = this.m_nestedProperties.Count;
                while (--count >= 0)
                {
                    ReadOnlyPropertiesDictionary dictionary = (ReadOnlyPropertiesDictionary) this.m_nestedProperties[count];
                    foreach (DictionaryEntry entry in (IEnumerable) dictionary)
                    {
                        this.m_flattened[(string) entry.Key] = entry.Value;
                    }
                }
            }
            return this.m_flattened;
        }

        public object this[string key]
        {
            get
            {
                if (this.m_flattened != null)
                {
                    return this.m_flattened[key];
                }
                foreach (ReadOnlyPropertiesDictionary dictionary in this.m_nestedProperties)
                {
                    if (dictionary.Contains(key))
                    {
                        return dictionary[key];
                    }
                }
                return null;
            }
        }
    }
}

