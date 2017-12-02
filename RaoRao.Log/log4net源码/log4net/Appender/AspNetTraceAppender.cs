namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using System;
    using System.Web;

    public class AspNetTraceAppender : AppenderSkeleton
    {
        private PatternLayout m_category = new PatternLayout("%logger");

        protected override void Append(LoggingEvent loggingEvent)
        {
            if ((HttpContext.Current != null) && HttpContext.Current.Trace.IsEnabled)
            {
                if (loggingEvent.Level >= Level.Warn)
                {
                    HttpContext.Current.Trace.Warn(this.m_category.Format(loggingEvent), base.RenderLoggingEvent(loggingEvent));
                }
                else
                {
                    HttpContext.Current.Trace.Write(this.m_category.Format(loggingEvent), base.RenderLoggingEvent(loggingEvent));
                }
            }
        }

        public PatternLayout Category
        {
            get => 
                this.m_category;
            set
            {
                this.m_category = value;
            }
        }

        protected override bool RequiresLayout =>
            true;
    }
}

