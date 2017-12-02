namespace log4net.Util
{
    using System;
    using System.Reflection;
    using System.Threading;

    public sealed class ThreadContextProperties : ContextPropertiesBase
    {
        private static readonly LocalDataStoreSlot s_threadLocalSlot = Thread.AllocateDataSlot();

        internal ThreadContextProperties()
        {
        }

        public void Clear()
        {
            PropertiesDictionary properties = this.GetProperties(false);
            if (properties != null)
            {
                properties.Clear();
            }
        }

        internal PropertiesDictionary GetProperties(bool create)
        {
            PropertiesDictionary data = (PropertiesDictionary) Thread.GetData(s_threadLocalSlot);
            if ((data == null) && create)
            {
                data = new PropertiesDictionary();
                Thread.SetData(s_threadLocalSlot, data);
            }
            return data;
        }

        public void Remove(string key)
        {
            PropertiesDictionary properties = this.GetProperties(false);
            if (properties != null)
            {
                properties.Remove(key);
            }
        }

        public override object this[string key]
        {
            get
            {
                PropertiesDictionary properties = this.GetProperties(false);
                if (properties != null)
                {
                    return properties[key];
                }
                return null;
            }
            set
            {
                this.GetProperties(1)[key] = value;
            }
        }
    }
}

