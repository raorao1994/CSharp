namespace log4net
{
    using log4net.Util;
    using System;

    public sealed class LogicalThreadContext
    {
        private static readonly LogicalThreadContextProperties s_properties = new LogicalThreadContextProperties();
        private static readonly ThreadContextStacks s_stacks = new ThreadContextStacks(s_properties);

        private LogicalThreadContext()
        {
        }

        public static LogicalThreadContextProperties Properties =>
            s_properties;

        public static ThreadContextStacks Stacks =>
            s_stacks;
    }
}

