namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;

    public class TelnetAppender : AppenderSkeleton
    {
        private static readonly Type declaringType = typeof(TelnetAppender);
        private SocketHandler m_handler;
        private int m_listeningPort = 0x17;

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            try
            {
                LogLog.Debug(declaringType, "Creating SocketHandler to listen on port [" + this.m_listeningPort + "]");
                this.m_handler = new SocketHandler(this.m_listeningPort);
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "Failed to create SocketHandler", exception);
                throw;
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if ((this.m_handler != null) && this.m_handler.HasConnections)
            {
                this.m_handler.Send(base.RenderLoggingEvent(loggingEvent));
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (this.m_handler != null)
            {
                this.m_handler.Dispose();
                this.m_handler = null;
            }
        }

        public int Port
        {
            get => 
                this.m_listeningPort;
            set
            {
                if ((value < 0) || (value > 0xffff))
                {
                    string[] strArray = new string[5];
                    strArray[0] = "The value specified for Port is less than ";
                    int num = 0;
                    strArray[1] = num.ToString(NumberFormatInfo.InvariantInfo);
                    strArray[2] = " or greater than ";
                    strArray[3] = 0xffff.ToString(NumberFormatInfo.InvariantInfo);
                    strArray[4] = ".";
                    throw SystemInfo.CreateArgumentOutOfRangeException("value", value, string.Concat(strArray));
                }
                this.m_listeningPort = value;
            }
        }

        protected override bool RequiresLayout =>
            true;

        protected class SocketHandler : IDisposable
        {
            private ArrayList m_clients = new ArrayList();
            private Socket m_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            private const int MAX_CONNECTIONS = 20;

            public SocketHandler(int port)
            {
                this.m_serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                this.m_serverSocket.Listen(5);
                this.m_serverSocket.BeginAccept(new AsyncCallback(this.OnConnect), null);
            }

            private void AddClient(SocketClient client)
            {
                lock (this)
                {
                    ArrayList list = (ArrayList) this.m_clients.Clone();
                    list.Add(client);
                    this.m_clients = list;
                }
            }

            public void Dispose()
            {
                ArrayList clients = this.m_clients;
                foreach (SocketClient client in clients)
                {
                    client.Dispose();
                }
                this.m_clients.Clear();
                Socket serverSocket = this.m_serverSocket;
                this.m_serverSocket = null;
                try
                {
                    serverSocket.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
                try
                {
                    serverSocket.Close();
                }
                catch
                {
                }
            }

            private void OnConnect(IAsyncResult asyncResult)
            {
                try
                {
                    Socket socket = this.m_serverSocket.EndAccept(asyncResult);
                    LogLog.Debug(TelnetAppender.declaringType, "Accepting connection from [" + socket.RemoteEndPoint.ToString() + "]");
                    SocketClient client = new SocketClient(socket);
                    int count = this.m_clients.Count;
                    if (count < 20)
                    {
                        try
                        {
                            client.Send("TelnetAppender v1.0 (" + (count + 1) + " active connections)\r\n\r\n");
                            this.AddClient(client);
                        }
                        catch
                        {
                            client.Dispose();
                        }
                    }
                    else
                    {
                        client.Send("Sorry - Too many connections.\r\n");
                        client.Dispose();
                    }
                }
                catch
                {
                }
                finally
                {
                    if (this.m_serverSocket != null)
                    {
                        this.m_serverSocket.BeginAccept(new AsyncCallback(this.OnConnect), null);
                    }
                }
            }

            private void RemoveClient(SocketClient client)
            {
                lock (this)
                {
                    ArrayList list = (ArrayList) this.m_clients.Clone();
                    list.Remove(client);
                    this.m_clients = list;
                }
            }

            public void Send(string message)
            {
                ArrayList clients = this.m_clients;
                foreach (SocketClient client in clients)
                {
                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception)
                    {
                        client.Dispose();
                        this.RemoveClient(client);
                    }
                }
            }

            public bool HasConnections
            {
                get
                {
                    ArrayList clients = this.m_clients;
                    return ((clients != null) && (clients.Count > 0));
                }
            }

            protected class SocketClient : IDisposable
            {
                private Socket m_socket;
                private StreamWriter m_writer;

                public SocketClient(Socket socket)
                {
                    this.m_socket = socket;
                    try
                    {
                        this.m_writer = new StreamWriter(new NetworkStream(socket));
                    }
                    catch
                    {
                        this.Dispose();
                        throw;
                    }
                }

                public void Dispose()
                {
                    try
                    {
                        if (this.m_writer != null)
                        {
                            this.m_writer.Close();
                            this.m_writer = null;
                        }
                    }
                    catch
                    {
                    }
                    if (this.m_socket != null)
                    {
                        try
                        {
                            this.m_socket.Shutdown(SocketShutdown.Both);
                        }
                        catch
                        {
                        }
                        try
                        {
                            this.m_socket.Close();
                        }
                        catch
                        {
                        }
                        this.m_socket = null;
                    }
                }

                public void Send(string message)
                {
                    this.m_writer.Write(message);
                    this.m_writer.Flush();
                }
            }
        }
    }
}

