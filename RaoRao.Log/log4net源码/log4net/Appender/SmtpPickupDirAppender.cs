namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;

    public class SmtpPickupDirAppender : BufferingAppenderSkeleton
    {
        private string m_from;
        private string m_pickupDir;
        private log4net.Core.SecurityContext m_securityContext;
        private string m_subject;
        private string m_to;

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (this.m_securityContext == null)
            {
                this.m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
            }
            using (this.SecurityContext.Impersonate(this))
            {
                this.m_pickupDir = ConvertToFullPath(this.m_pickupDir.Trim());
            }
        }

        protected static string ConvertToFullPath(string path) => 
            SystemInfo.ConvertToFullPath(path);

        protected override void SendBuffer(LoggingEvent[] events)
        {
            try
            {
                string path = null;
                StreamWriter writer = null;
                using (this.SecurityContext.Impersonate(this))
                {
                    path = Path.Combine(this.m_pickupDir, SystemInfo.NewGuid().ToString("N"));
                    writer = File.CreateText(path);
                }
                if (writer == null)
                {
                    this.ErrorHandler.Error("Failed to create output file for writing [" + path + "]", null, ErrorCode.FileOpenFailure);
                }
                else
                {
                    using (writer)
                    {
                        writer.WriteLine("To: " + this.m_to);
                        writer.WriteLine("From: " + this.m_from);
                        writer.WriteLine("Subject: " + this.m_subject);
                        writer.WriteLine("");
                        string header = this.Layout.Header;
                        if (header != null)
                        {
                            writer.Write(header);
                        }
                        for (int i = 0; i < events.Length; i++)
                        {
                            base.RenderLoggingEvent(writer, events[i]);
                        }
                        header = this.Layout.Footer;
                        if (header != null)
                        {
                            writer.Write(header);
                        }
                        writer.WriteLine("");
                        writer.WriteLine(".");
                    }
                }
            }
            catch (Exception exception)
            {
                this.ErrorHandler.Error("Error occurred while sending e-mail notification.", exception);
            }
        }

        public string From
        {
            get => 
                this.m_from;
            set
            {
                this.m_from = value;
            }
        }

        public string PickupDir
        {
            get => 
                this.m_pickupDir;
            set
            {
                this.m_pickupDir = value;
            }
        }

        protected override bool RequiresLayout =>
            true;

        public log4net.Core.SecurityContext SecurityContext
        {
            get => 
                this.m_securityContext;
            set
            {
                this.m_securityContext = value;
            }
        }

        public string Subject
        {
            get => 
                this.m_subject;
            set
            {
                this.m_subject = value;
            }
        }

        public string To
        {
            get => 
                this.m_to;
            set
            {
                this.m_to = value;
            }
        }
    }
}

