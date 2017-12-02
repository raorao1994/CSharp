namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using System;
    using System.Diagnostics;

    public class TraceAppender : AppenderSkeleton
    {
        private PatternLayout m_category;
        private bool m_immediateFlush;

        public TraceAppender()
        {
            this.m_immediateFlush = true;
            this.m_category = new PatternLayout("%logger");
        }

        [Obsolete("Instead use the default constructor and set the Layout property")]
        public TraceAppender(ILayout layout)
        {
            this.m_immediateFlush = true;
            this.m_category = new PatternLayout("%logger");
            this.Layout = layout;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            Trace.Write(base.RenderLoggingEvent(loggingEvent), this.m_category.Format(loggingEvent));
            if (this.m_immediateFlush)
            {
                Trace.Flush();
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

        public bool ImmediateFlush
        {
            get => 
                this.m_immediateFlush;
            set
            {
                this.m_immediateFlush = value;
            }
        }

        protected override bool RequiresLayout =>
            true;
    }
}

