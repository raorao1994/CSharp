namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;
    using System.Web;

    internal sealed class AspNetCachePatternConverter : AspNetPatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent, HttpContext httpContext)
        {
            if (HttpRuntime.Cache != null)
            {
                if (this.Option != null)
                {
                    PatternConverter.WriteObject(writer, loggingEvent.Repository, HttpRuntime.Cache[this.Option]);
                }
                else
                {
                    PatternConverter.WriteObject(writer, loggingEvent.Repository, HttpRuntime.Cache.GetEnumerator());
                }
            }
            else
            {
                writer.Write(SystemInfo.NotAvailableText);
            }
        }
    }
}

