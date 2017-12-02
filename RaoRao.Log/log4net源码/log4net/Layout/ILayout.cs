namespace log4net.Layout
{
    using log4net.Core;
    using System;
    using System.IO;

    public interface ILayout
    {
        void Format(TextWriter writer, LoggingEvent loggingEvent);

        string ContentType { get; }

        string Footer { get; }

        string Header { get; }

        bool IgnoresException { get; }
    }
}

