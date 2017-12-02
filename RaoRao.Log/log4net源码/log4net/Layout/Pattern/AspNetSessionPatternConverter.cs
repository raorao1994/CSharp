namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;
    using System.Web;

    internal sealed class AspNetSessionPatternConverter : AspNetPatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent, HttpContext httpContext)
        {
            if (httpContext.Session != null)
            {
                if (this.Option != null)
                {
                    PatternConverter.WriteObject(writer, loggingEvent.Repository, httpContext.Session.Contents[this.Option]);
                }
                else
                {
                    PatternConverter.WriteObject(writer, loggingEvent.Repository, httpContext.Session);
                }
            }
            else
            {
                writer.Write(SystemInfo.NotAvailableText);
            }
        }
    }
}

