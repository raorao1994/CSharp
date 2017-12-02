namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;

    internal sealed class PropertyPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (this.Option != null)
            {
                PatternConverter.WriteObject(writer, loggingEvent.Repository, loggingEvent.LookupProperty(this.Option));
            }
            else
            {
                PatternConverter.WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            }
        }
    }
}

