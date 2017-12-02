namespace log4net.Layout.Pattern
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    internal class StackTracePatternConverter : PatternLayoutConverter, IOptionHandler
    {
        private static readonly Type declaringType = typeof(StackTracePatternConverter);
        private int m_stackFrameLevel = 1;

        public void ActivateOptions()
        {
            if (this.Option != null)
            {
                string s = this.Option.Trim();
                if (s.Length != 0)
                {
                    int num;
                    if (SystemInfo.TryParse(s, out num))
                    {
                        if (num <= 0)
                        {
                            LogLog.Error(declaringType, "StackTracePatternConverter: StackeFrameLevel option (" + s + ") isn't a positive integer.");
                        }
                        else
                        {
                            this.m_stackFrameLevel = num;
                        }
                    }
                    else
                    {
                        LogLog.Error(declaringType, "StackTracePatternConverter: StackFrameLevel option \"" + s + "\" not a decimal integer.");
                    }
                }
            }
        }

        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            System.Diagnostics.StackFrame[] stackFrames = loggingEvent.LocationInformation.StackFrames;
            if ((stackFrames == null) || (stackFrames.Length <= 0))
            {
                LogLog.Error(declaringType, "loggingEvent.LocationInformation.StackFrames was null or empty.");
            }
            else
            {
                int index = this.m_stackFrameLevel - 1;
                while (index >= 0)
                {
                    if (index > stackFrames.Length)
                    {
                        index--;
                    }
                    else
                    {
                        System.Diagnostics.StackFrame frame = stackFrames[index];
                        writer.Write("{0}.{1}", frame.GetMethod().DeclaringType.Name, this.GetMethodInformation(frame.GetMethod()));
                        if (index > 0)
                        {
                            writer.Write(" > ");
                        }
                        index--;
                    }
                }
            }
        }

        internal virtual string GetMethodInformation(MethodBase method) => 
            method.Name;
    }
}

