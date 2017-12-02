namespace log4net.Util.PatternStringConverters
{
    using log4net.Util;
    using System;
    using System.IO;
    using System.Security;
    using System.Threading;

    internal sealed class IdentityPatternConverter : PatternConverter
    {
        private static readonly Type declaringType = typeof(IdentityPatternConverter);

        protected override void Convert(TextWriter writer, object state)
        {
            try
            {
                if (((Thread.CurrentPrincipal != null) && (Thread.CurrentPrincipal.Identity != null)) && (Thread.CurrentPrincipal.Identity.Name != null))
                {
                    writer.Write(Thread.CurrentPrincipal.Identity.Name);
                }
            }
            catch (SecurityException)
            {
                LogLog.Debug(declaringType, "Security exception while trying to get current thread principal. Error Ignored.");
                writer.Write(SystemInfo.NotAvailableText);
            }
        }
    }
}

