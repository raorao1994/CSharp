namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using System;
    using System.Diagnostics;

    public class DebugAppender : AppenderSkeleton
    {
        private bool m_immediateFlush;

        public DebugAppender()
        {
            this.m_immediateFlush = true;
        }

        [Obsolete("Instead use the default constructor and set the Layout property")]
        public DebugAppender(ILayout layout)
        {
            this.m_immediateFlush = true;
            this.Layout = layout;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            Debug.Write(base.RenderLoggingEvent(loggingEvent), loggingEvent.LoggerName);
            if (this.m_immediateFlush)
            {
                Debug.Flush();
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

