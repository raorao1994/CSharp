namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class UdpAppender : AppenderSkeleton
    {
        private UdpClient m_client;
        private System.Text.Encoding m_encoding = System.Text.Encoding.Default;
        private int m_localPort;
        private IPAddress m_remoteAddress;
        private IPEndPoint m_remoteEndPoint;
        private int m_remotePort;

        public override void ActivateOptions()
        {
            string[] strArray;
            int num;
            base.ActivateOptions();
            if (this.RemoteAddress == null)
            {
                throw new ArgumentNullException("The required property 'Address' was not specified.");
            }
            if ((this.RemotePort < 0) || (this.RemotePort > 0xffff))
            {
                strArray = new string[5];
                strArray[0] = "The RemotePort is less than ";
                num = 0;
                strArray[1] = num.ToString(NumberFormatInfo.InvariantInfo);
                strArray[2] = " or greater than ";
                num = 0xffff;
                strArray[3] = num.ToString(NumberFormatInfo.InvariantInfo);
                strArray[4] = ".";
                throw SystemInfo.CreateArgumentOutOfRangeException("this.RemotePort", this.RemotePort, string.Concat(strArray));
            }
            if ((this.LocalPort != 0) && ((this.LocalPort < 0) || (this.LocalPort > 0xffff)))
            {
                strArray = new string[5];
                strArray[0] = "The LocalPort is less than ";
                num = 0;
                strArray[1] = num.ToString(NumberFormatInfo.InvariantInfo);
                strArray[2] = " or greater than ";
                strArray[3] = 0xffff.ToString(NumberFormatInfo.InvariantInfo);
                strArray[4] = ".";
                throw SystemInfo.CreateArgumentOutOfRangeException("this.LocalPort", this.LocalPort, string.Concat(strArray));
            }
            this.RemoteEndPoint = new IPEndPoint(this.RemoteAddress, this.RemotePort);
            this.InitializeClientConnection();
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                byte[] bytes = this.m_encoding.GetBytes(base.RenderLoggingEvent(loggingEvent).ToCharArray());
                this.Client.Send(bytes, bytes.Length, this.RemoteEndPoint);
            }
            catch (Exception exception)
            {
                this.ErrorHandler.Error(string.Concat(new object[] { "Unable to send logging event to remote host ", this.RemoteAddress.ToString(), " on port ", this.RemotePort, "." }), exception, ErrorCode.WriteFailure);
            }
        }

        protected virtual void InitializeClientConnection()
        {
            try
            {
                if (this.LocalPort == 0)
                {
                    this.Client = new UdpClient(this.RemoteAddress.AddressFamily);
                }
                else
                {
                    this.Client = new UdpClient(this.LocalPort, this.RemoteAddress.AddressFamily);
                }
            }
            catch (Exception exception)
            {
                this.ErrorHandler.Error("Could not initialize the UdpClient connection on port " + this.LocalPort.ToString(NumberFormatInfo.InvariantInfo) + ".", exception, ErrorCode.GenericFailure);
                this.Client = null;
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (this.Client != null)
            {
                this.Client.Close();
                this.Client = null;
            }
        }

        protected UdpClient Client
        {
            get => 
                this.m_client;
            set
            {
                this.m_client = value;
            }
        }

        public System.Text.Encoding Encoding
        {
            get => 
                this.m_encoding;
            set
            {
                this.m_encoding = value;
            }
        }

        public int LocalPort
        {
            get => 
                this.m_localPort;
            set
            {
                if ((value != 0) && ((value < 0) || (value > 0xffff)))
                {
                    string[] strArray = new string[5];
                    strArray[0] = "The value specified is less than ";
                    int num = 0;
                    strArray[1] = num.ToString(NumberFormatInfo.InvariantInfo);
                    strArray[2] = " or greater than ";
                    strArray[3] = 0xffff.ToString(NumberFormatInfo.InvariantInfo);
                    strArray[4] = ".";
                    throw SystemInfo.CreateArgumentOutOfRangeException("value", value, string.Concat(strArray));
                }
                this.m_localPort = value;
            }
        }

        public IPAddress RemoteAddress
        {
            get => 
                this.m_remoteAddress;
            set
            {
                this.m_remoteAddress = value;
            }
        }

        protected IPEndPoint RemoteEndPoint
        {
            get => 
                this.m_remoteEndPoint;
            set
            {
                this.m_remoteEndPoint = value;
            }
        }

        public int RemotePort
        {
            get => 
                this.m_remotePort;
            set
            {
                if ((value < 0) || (value > 0xffff))
                {
                    string[] strArray = new string[5];
                    strArray[0] = "The value specified is less than ";
                    int num = 0;
                    strArray[1] = num.ToString(NumberFormatInfo.InvariantInfo);
                    strArray[2] = " or greater than ";
                    strArray[3] = 0xffff.ToString(NumberFormatInfo.InvariantInfo);
                    strArray[4] = ".";
                    throw SystemInfo.CreateArgumentOutOfRangeException("value", value, string.Concat(strArray));
                }
                this.m_remotePort = value;
            }
        }

        protected override bool RequiresLayout =>
            true;
    }
}

