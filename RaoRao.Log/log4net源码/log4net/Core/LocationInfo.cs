namespace log4net.Core
{
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Security;

    [Serializable]
    public class LocationInfo
    {
        private static readonly Type declaringType = typeof(LocationInfo);
        private readonly string m_className;
        private readonly string m_fileName;
        private readonly string m_fullInfo;
        private readonly string m_lineNumber;
        private readonly string m_methodName;
        private readonly System.Diagnostics.StackFrame[] m_stackFrames;
        private const string NA = "?";

        public LocationInfo(Type callerStackBoundaryDeclaringType)
        {
            this.m_className = "?";
            this.m_fileName = "?";
            this.m_lineNumber = "?";
            this.m_methodName = "?";
            this.m_fullInfo = "?";
            if (callerStackBoundaryDeclaringType != null)
            {
                try
                {
                    System.Diagnostics.StackFrame frame;
                    StackTrace trace = new StackTrace(true);
                    int index = 0;
                    while (index < trace.FrameCount)
                    {
                        frame = trace.GetFrame(index);
                        if ((frame != null) && (frame.GetMethod().DeclaringType == callerStackBoundaryDeclaringType))
                        {
                            break;
                        }
                        index++;
                    }
                    while (index < trace.FrameCount)
                    {
                        frame = trace.GetFrame(index);
                        if ((frame != null) && (frame.GetMethod().DeclaringType != callerStackBoundaryDeclaringType))
                        {
                            break;
                        }
                        index++;
                    }
                    if (index < trace.FrameCount)
                    {
                        int capacity = trace.FrameCount - index;
                        ArrayList list = new ArrayList(capacity);
                        this.m_stackFrames = new System.Diagnostics.StackFrame[capacity];
                        for (int i = index; i < trace.FrameCount; i++)
                        {
                            list.Add(trace.GetFrame(i));
                        }
                        list.CopyTo(this.m_stackFrames, 0);
                        System.Diagnostics.StackFrame frame2 = trace.GetFrame(index);
                        if (frame2 != null)
                        {
                            MethodBase method = frame2.GetMethod();
                            if (method != null)
                            {
                                this.m_methodName = method.Name;
                                if (method.DeclaringType != null)
                                {
                                    this.m_className = method.DeclaringType.FullName;
                                }
                            }
                            this.m_fileName = frame2.GetFileName();
                            this.m_lineNumber = frame2.GetFileLineNumber().ToString(NumberFormatInfo.InvariantInfo);
                            this.m_fullInfo = string.Concat(new object[] { this.m_className, '.', this.m_methodName, '(', this.m_fileName, ':', this.m_lineNumber, ')' });
                        }
                    }
                }
                catch (SecurityException)
                {
                    LogLog.Debug(declaringType, "Security exception while trying to get caller stack frame. Error Ignored. Location Information Not Available.");
                }
            }
        }

        public LocationInfo(string className, string methodName, string fileName, string lineNumber)
        {
            this.m_className = className;
            this.m_fileName = fileName;
            this.m_lineNumber = lineNumber;
            this.m_methodName = methodName;
            this.m_fullInfo = string.Concat(new object[] { this.m_className, '.', this.m_methodName, '(', this.m_fileName, ':', this.m_lineNumber, ')' });
        }

        public string ClassName =>
            this.m_className;

        public string FileName =>
            this.m_fileName;

        public string FullInfo =>
            this.m_fullInfo;

        public string LineNumber =>
            this.m_lineNumber;

        public string MethodName =>
            this.m_methodName;

        public System.Diagnostics.StackFrame[] StackFrames =>
            this.m_stackFrames;
    }
}

