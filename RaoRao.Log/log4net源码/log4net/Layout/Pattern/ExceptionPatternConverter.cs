namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;

    internal sealed class ExceptionPatternConverter : PatternLayoutConverter
    {
        public ExceptionPatternConverter()
        {
            this.IgnoresException = false;
        }

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (((loggingEvent.ExceptionObject == null) || (this.Option == null)) || (this.Option.Length <= 0))
            {
                string exceptionString = loggingEvent.GetExceptionString();
                if ((exceptionString != null) && (exceptionString.Length > 0))
                {
                    writer.WriteLine(exceptionString);
                }
            }
            else
            {
                switch (this.Option.ToLower())
                {
                    case "message":
                        PatternConverter.WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.Message);
                        break;

                    case "source":
                        PatternConverter.WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.Source);
                        break;

                    case "stacktrace":
                        PatternConverter.WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.StackTrace);
                        break;

                    case "targetsite":
                        PatternConverter.WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.TargetSite);
                        break;

                    case "helplink":
                        PatternConverter.WriteObject(writer, loggingEvent.Repository, loggingEvent.ExceptionObject.HelpLink);
                        break;
                }
            }
        }
    }
}

