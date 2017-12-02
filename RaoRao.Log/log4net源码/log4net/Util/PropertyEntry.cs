namespace log4net.Util
{
    using System;

    public class PropertyEntry
    {
        private string m_key = null;
        private object m_value = null;

        public override string ToString() => 
            string.Concat(new object[] { "PropertyEntry(Key=", this.m_key, ", Value=", this.m_value, ")" });

        public string Key
        {
            get => 
                this.m_key;
            set
            {
                this.m_key = value;
            }
        }

        public object Value
        {
            get => 
                this.m_value;
            set
            {
                this.m_value = value;
            }
        }
    }
}

