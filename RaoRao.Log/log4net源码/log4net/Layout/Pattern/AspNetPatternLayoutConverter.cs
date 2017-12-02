namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;
    using System.Web;

    internal abstract class AspNetPatternLayoutConverter : PatternLayoutConverter
    {
        protected AspNetPatternLayoutConverter()
        {
        }

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (HttpContext.Current == null)
            {
                writer.Write(SystemInfo.NotAvailableText);
            }
            else
            {
                this.Convert(writer, loggingEvent, HttpContext.Current);
            }
        }

        protected abstract void Convert(TextWriter writer, LoggingEvent loggingEvent, HttpContext httpContext);
    }
}

