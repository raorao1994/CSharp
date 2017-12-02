namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using log4net.Util;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;

    public class RemoteSyslogAppender : UdpAppender
    {
        private const int DefaultSyslogPort = 0x202;
        private SyslogFacility m_facility = SyslogFacility.User;
        private PatternLayout m_identity;
        private LevelMapping m_levelMapping = new LevelMapping();

        public RemoteSyslogAppender()
        {
            base.RemotePort = 0x202;
            base.RemoteAddress = IPAddress.Parse("127.0.0.1");
            base.Encoding = Encoding.ASCII;
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            this.m_levelMapping.ActivateOptions();
        }

        public void AddMapping(LevelSeverity mapping)
        {
            this.m_levelMapping.Add(mapping);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
                int num = GeneratePriority(this.m_facility, this.GetSeverity(loggingEvent.Level));
                writer.Write('<');
                writer.Write(num);
                writer.Write('>');
                if (this.m_identity != null)
                {
                    this.m_identity.Format(writer, loggingEvent);
                }
                else
                {
                    writer.Write(loggingEvent.Domain);
                }
                writer.Write(": ");
                base.RenderLoggingEvent(writer, loggingEvent);
                string str = writer.ToString();
                byte[] bytes = base.Encoding.GetBytes(str.ToCharArray());
                base.Client.Send(bytes, bytes.Length, base.RemoteEndPoint);
            }
            catch (Exception exception)
            {
                this.ErrorHandler.Error(string.Concat(new object[] { "Unable to send logging event to remote syslog ", base.RemoteAddress.ToString(), " on port ", base.RemotePort, "." }), exception, ErrorCode.WriteFailure);
            }
        }

        public static int GeneratePriority(SyslogFacility facility, SyslogSeverity severity)
        {
            if ((facility < SyslogFacility.Kernel) || (facility > SyslogFacility.Local7))
            {
                throw new ArgumentException("SyslogFacility out of range", "facility");
            }
            if ((severity < SyslogSeverity.Emergency) || (severity > SyslogSeverity.Debug))
            {
                throw new ArgumentException("SyslogSeverity out of range", "severity");
            }
            return (int) ((facility * SyslogFacility.Uucp) + ((SyslogFacility) ((int) severity)));
        }

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

        public SyslogFacility Facility
        {
            get => 
                this.m_facility;
            set
            {
                this.m_facility = value;
            }
        }

        public PatternLayout Identity
        {
            get => 
                this.m_identity;
            set
            {
                this.m_identity = value;
            }
        }

        public class LevelSeverity : LevelMappingEntry
        {
            private RemoteSyslogAppender.SyslogSeverity m_severity;

            public RemoteSyslogAppender.SyslogSeverity Severity
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

