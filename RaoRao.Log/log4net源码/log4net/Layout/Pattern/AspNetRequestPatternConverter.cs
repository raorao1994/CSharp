namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;
    using System.Web;

    internal sealed class AspNetRequestPatternConverter : AspNetPatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent, HttpContext httpContext)
        {
            if (httpContext.Request != null)
            {
                if (this.Option != null)
                {
                    PatternConverter.WriteObject(writer, loggingEvent.Repository, httpContext.Request.Params[this.Option]);
                }
                else
                {
                    PatternConverter.WriteObject(writer, loggingEvent.Repository, httpContext.Request.Params);
                }
            }
            else
            {
                writer.Write(SystemInfo.NotAvailableText);
            }
        }
    }
}

