namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;

    public class LocalSyslogAppender : AppenderSkeleton
    {
        private SyslogFacility m_facility = SyslogFacility.User;
        private IntPtr m_handleToIdentity = IntPtr.Zero;
        private string m_identity;
        private LevelMapping m_levelMapping = new LevelMapping();

        [SecuritySafeCritical]
        public override void ActivateOptions()
        {
            base.ActivateOptions();
            this.m_levelMapping.ActivateOptions();
            string identity = this.m_identity;
            if (identity == null)
            {
                identity = SystemInfo.ApplicationFriendlyName;
            }
            this.m_handleToIdentity = Marshal.StringToHGlobalAnsi(identity);
            openlog(this.m_handleToIdentity, 1, this.m_facility);
        }

        public void AddMapping(LevelSeverity mapping)
        {
            this.m_levelMapping.Add(mapping);
        }

        [SecuritySafeCritical, SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        protected override void Append(LoggingEvent loggingEvent)
        {
            int priority = GeneratePriority(this.m_facility, this.GetSeverity(loggingEvent.Level));
            string message = base.RenderLoggingEvent(loggingEvent);
            syslog(priority, "%s", message);
        }

        [DllImport("libc")]
        private static extern void closelog();
        private static int GeneratePriority(SyslogFacility facility, SyslogSeverity severity) => 
            ((int) ((facility * SyslogFacility.Uucp) + ((SyslogFacility) ((int) severity))));

        protected virtual SyslogSeverity GetSeverity(Level level)
        {
            LevelSeverity severity = this.m_levelMapping.Lookup(level) as LevelSeverity;
            if (severity != null)
            {
                return severity.Severity;
            }
            if (level >= Level.Alert)
            {
                return SyslogSeverity.Alert;
            }
            if (level >= Level.Critical)
            {
                return SyslogSeverity.Critical;
            }
            if (level >= Level.Error)
            {
                return SyslogSeverity.Error;
            }
            if (level >= Level.Warn)
            {
                return SyslogSeverity.Warning;
            }
            if (level >= Level.Notice)
            {
                return SyslogSeverity.Notice;
            }
            if (level >= Level.Info)
            {
                return SyslogSeverity.Informational;
            }
            return SyslogSeverity.Debug;
        }

        [SecuritySafeCritical]
        protected override void OnClose()
        {
            base.OnClose();
            try
            {
                closelog();
            }
            catch (DllNotFoundException)
            {
            }
            if (this.m_handleToIdentity != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.m_handleToIdentity);
            }
        }

        [DllImport("libc")]
        private static extern void openlog(IntPtr ident, int option, SyslogFacility facility);
        [DllImport("libc", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Ansi)]
        private static extern void syslog(int priority, string format, string message);

        public SyslogFacility Facility
        {
            get => 
                this.m_facility;
            set
            {
                this.m_facility = value;
            }
        }

        public string Identity
        {
            get => 
                this.m_identity;
            set
            {
                this.m_identity = value;
            }
        }

        protected override bool RequiresLayout =>
            true;

        public class LevelSeverity : LevelMappingEntry
        {
            private LocalSyslogAppender.SyslogSeverity m_severity;

            public LocalSyslogAppender.SyslogSeverity Severity
            {
                get => 
                    this.m_severity;
                set
                {
                    this.m_severity = value;
                }
            }
        }

        public enum SyslogFacility
        {
            Kernel,
            User,
            Mail,
            Daemons,
            Authorization,
            Syslog,
            Printer,
            News,
            Uucp,
            Clock,
            Authorization2,
            Ftp,
            Ntp,
            Audit,
            Alert,
            Clock2,
            Local0,
            Local1,
            Local2,
            Local3,
            Local4,
            Local5,
            Local6,
            Local7
        }

        public enum SyslogSeverity
        {
            Emergency,
            Alert,
            Critical,
            Error,
            Warning,
            Notice,
            Informational,
            Debug
        }
    }
}

