namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using System;
    using System.Globalization;

    public class ConsoleAppender : AppenderSkeleton
    {
        public const string ConsoleError = "Console.Error";
        public const string ConsoleOut = "Console.Out";
        private bool m_writeToErrorStream;

        public ConsoleAppender()
        {
            this.m_writeToErrorStream = false;
        }

        [Obsolete("Instead use the default constructor and set the Layout property")]
        public ConsoleAppender(ILayout layout) : this(layout, false)
        {
        }

        [Obsolete("Instead use the default constructor and set the Layout & Target properties")]
        public ConsoleAppender(ILayout layout, bool writeToErrorStream)
        {
            this.m_writeToErrorStream = false;
            this.Layout = layout;
            this.m_writeToErrorStream = writeToErrorStream;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (this.m_writeToErrorStream)
            {
                Console.Error.Write(base.RenderLoggingEvent(loggingEvent));
            }
            else
            {
                Console.Write(base.RenderLoggingEvent(loggingEvent));
            }
        }

        protected override bool RequiresLayout =>
            true;

        public virtual string Target
        {
            get => 
                (this.m_writeToErrorStream ? "Console.Error" : "Console.Out");
            set
            {
                string strB = value.Trim();
                if (string.Compare("Console.Error", strB, true, CultureInfo.InvariantCulture) == 0)
                {
                    this.m_writeToErrorStream = true;
                }
                else
                {
                    this.m_writeToErrorStream = false;
                }
            }
        }
    }
}

