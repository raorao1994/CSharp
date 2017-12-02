namespace log4net.Util
{
    using System;
    using System.Reflection;

    public sealed class ThreadContextStacks
    {
        private static readonly Type declaringType = typeof(ThreadContextStacks);
        private readonly ContextPropertiesBase m_properties;

        internal ThreadContextStacks(ContextPropertiesBase properties)
        {
            this.m_properties = properties;
        }

        public ThreadContextStack this[string key]
        {
            get
            {
                ThreadContextStack stack = null;
                object obj2 = this.m_properties[key];
                if (obj2 == null)
                {
                    stack = new ThreadContextStack();
                    this.m_properties[key] = stack;
                    return stack;
                }
                stack = obj2 as ThreadContextStack;
                if (stack != null)
                {
                    return stack;
                }
                string nullText = SystemInfo.NullText;
                try
                {
                    nullText = obj2.ToString();
                }
                catch
                {
                }
                LogLog.Error(declaringType, "ThreadContextStacks: Request for stack named [" + key + "] failed because a property with the same name exists which is a [" + obj2.GetType().Name + "] with value [" + nullText + "]");
                return new ThreadContextStack();
            }
        }
    }
}

