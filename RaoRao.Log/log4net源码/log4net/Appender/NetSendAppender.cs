namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;

    public class NetSendAppender : AppenderSkeleton
    {
        private string m_recipient;
        private log4net.Core.SecurityContext m_securityContext;
        private string m_sender;
        private string m_server;

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (this.Recipient == null)
            {
                throw new ArgumentNullException("Recipient", "The required property 'Recipient' was not specified.");
            }
            if (this.m_securityContext == null)
            {
                this.m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
            }
        }

        [SecuritySafeCritical, SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        protected override void Append(LoggingEvent loggingEvent)
        {
            NativeError error = null;
            string buffer = base.RenderLoggingEvent(loggingEvent);
            using (this.m_securityContext.Impersonate(this))
            {
                int number = NetMessageBufferSend(this.Server, this.Recipient, this.Sender, buffer, buffer.Length * Marshal.SystemDefaultCharSize);
                if (number != 0)
                {
                    error = NativeError.GetError(number);
                }
            }
            if (error != null)
            {
                this.ErrorHandler.Error(error.ToString() + " (Params: Server=" + this.Server + ", Recipient=" + this.Recipient + ", Sender=" + this.Sender + ")");
            }
        }

        [DllImport("netapi32.dll", SetLastError=true)]
        protected static extern int NetMessageBufferSend([MarshalAs(UnmanagedType.LPWStr)] string serverName, [MarshalAs(UnmanagedType.LPWStr)] string msgName, [MarshalAs(UnmanagedType.LPWStr)] string fromName, [MarshalAs(UnmanagedType.LPWStr)] string buffer, int bufferSize);

        public string Recipient
        {
            get => 
                this.m_recipient;
            set
            {
                this.m_recipient = value;
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

        public string Sender
        {
            get => 
                this.m_sender;
            set
            {
                this.m_sender = value;
            }
        }

        public string Server
        {
            get => 
                this.m_server;
            set
            {
                this.m_server = value;
            }
        }
    }
}

