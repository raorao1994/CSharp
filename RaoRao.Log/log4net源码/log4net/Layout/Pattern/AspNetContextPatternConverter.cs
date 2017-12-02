namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;
    using System.Web;

    internal sealed class AspNetContextPatternConverter : AspNetPatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent, HttpContext httpContext)
        {
            if (this.Option != null)
            {
                PatternConverter.WriteObject(writer, loggingEvent.Repository, httpContext.Items[this.Option]);
            }
            else
            {
                PatternConverter.WriteObject(writer, loggingEvent.Repository, httpContext.Items);
            }
        }
    }
}

