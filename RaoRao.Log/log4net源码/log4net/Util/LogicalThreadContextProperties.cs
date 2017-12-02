namespace log4net.Util
{
    using System;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using System.Security;

    public sealed class LogicalThreadContextProperties : ContextPropertiesBase
    {
        private const string c_SlotName = "log4net.Util.LogicalThreadContextProperties";
        private static readonly Type declaringType = typeof(LogicalThreadContextProperties);
        private bool m_disabled = false;

        internal LogicalThreadContextProperties()
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

        [SecuritySafeCritical]
        private static PropertiesDictionary GetCallContextData() => 
            (CallContext.GetData("log4net.Util.LogicalThreadContextProperties") as PropertiesDictionary);

        internal PropertiesDictionary GetProperties(bool create)
        {
            if (!this.m_disabled)
            {
                try
                {
                    PropertiesDictionary callContextData = GetCallContextData();
                    if ((callContextData == null) && create)
                    {
                        callContextData = new PropertiesDictionary();
                        SetCallContextData(callContextData);
                    }
                    return callContextData;
                }
                catch (SecurityException exception)
                {
                    this.m_disabled = true;
                    LogLog.Warn(declaringType, "SecurityException while accessing CallContext. Disabling LogicalThreadContextProperties", exception);
                }
            }
            if (create)
            {
                return new PropertiesDictionary();
            }
            return null;
        }

        public void Remove(string key)
        {
            PropertiesDictionary properties = this.GetProperties(false);
            if (properties != null)
            {
                properties.Remove(key);
            }
        }

        [SecuritySafeCritical]
        private static void SetCallContextData(PropertiesDictionary properties)
        {
            CallContext.SetData("log4net.Util.LogicalThreadContextProperties", properties);
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

