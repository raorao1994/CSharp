namespace log4net.Plugin
{
    using log4net.Appender;
    using log4net.Core;
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Runtime.Remoting;
    using System.Security;

    public class RemoteLoggingServerPlugin : PluginSkeleton
    {
        private static readonly Type declaringType = typeof(RemoteLoggingServerPlugin);
        private RemoteLoggingSinkImpl m_sink;
        private string m_sinkUri;

        public RemoteLoggingServerPlugin() : base("RemoteLoggingServerPlugin:Unset URI")
        {
        }

        public RemoteLoggingServerPlugin(string sinkUri) : base("RemoteLoggingServerPlugin:" + sinkUri)
        {
            this.m_sinkUri = sinkUri;
        }

        public override void Attach(ILoggerRepository repository)
        {
            base.Attach(repository);
            this.m_sink = new RemoteLoggingSinkImpl(repository);
            try
            {
                RemotingServices.Marshal(this.m_sink, this.m_sinkUri, typeof(RemotingAppender.IRemoteLoggingSink));
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "Failed to Marshal remoting sink", exception);
            }
        }

        [SecuritySafeCritical]
        public override void Shutdown()
        {
            RemotingServices.Disconnect(this.m_sink);
            this.m_sink = null;
            base.Shutdown();
        }

        public virtual string SinkUri
        {
            get => 
                this.m_sinkUri;
            set
            {
                this.m_sinkUri = value;
            }
        }

        private class RemoteLoggingSinkImpl : MarshalByRefObject, RemotingAppender.IRemoteLoggingSink
        {
            private readonly ILoggerRepository m_repository;

            public RemoteLoggingSinkImpl(ILoggerRepository repository)
            {
                this.m_repository = repository;
            }

            [SecurityCritical]
            public override object InitializeLifetimeService() => 
                null;

            public void LogEvents(LoggingEvent[] events)
            {
                if (events != null)
                {
                    foreach (LoggingEvent event2 in events)
                    {
                        if (event2 != null)
                        {
                            this.m_repository.Log(event2);
                        }
                    }
                }
            }
        }
    }
}

