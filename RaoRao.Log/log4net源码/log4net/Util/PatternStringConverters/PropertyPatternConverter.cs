namespace log4net.Util.PatternStringConverters
{
    using log4net;
    using log4net.Util;
    using System;
    using System.IO;

    internal sealed class PropertyPatternConverter : PatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            CompositeProperties properties = new CompositeProperties();
            PropertiesDictionary dictionary = LogicalThreadContext.Properties.GetProperties(false);
            if (dictionary != null)
            {
                properties.Add(dictionary);
            }
            PropertiesDictionary dictionary2 = ThreadContext.Properties.GetProperties(false);
            if (dictionary2 != null)
            {
                properties.Add(dictionary2);
            }
            properties.Add(GlobalContext.Properties.GetReadOnlyProperties());
            if (this.Option != null)
            {
                PatternConverter.WriteObject(writer, null, properties[this.Option]);
            }
            else
            {
                PatternConverter.WriteDictionary(writer, null, properties.Flatten());
            }
        }
    }
}

