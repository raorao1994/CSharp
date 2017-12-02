namespace log4net.Repository.Hierarchy
{
    using log4net.Appender;
    using log4net.Core;
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Threading;
    using System.Xml;

    public class Hierarchy : LoggerRepositorySkeleton, IBasicRepositoryConfigurator, IXmlRepositoryConfigurator
    {
        private static readonly Type declaringType = typeof(log4net.Repository.Hierarchy.Hierarchy);
        private ILoggerFactory m_defaultFactory;
        private bool m_emittedNoAppenderWarning;
        private Hashtable m_ht;
        private Logger m_root;

        public event LoggerCreationEventHandler LoggerCreatedEvent
        {
            add
            {
                this.m_loggerCreatedEvent += value;
            }
            remove
            {
                this.m_loggerCreatedEvent -= value;
            }
        }

        private event LoggerCreationEventHandler m_loggerCreatedEvent;

        public Hierarchy() : this(new DefaultLoggerFactory())
        {
        }

        public Hierarchy(ILoggerFactory loggerFactory) : this(new PropertiesDictionary(), loggerFactory)
        {
        }

        public Hierarchy(PropertiesDictionary properties) : this(properties, new DefaultLoggerFactory())
        {
        }

        public Hierarchy(PropertiesDictionary properties, ILoggerFactory loggerFactory) : base(properties)
        {
            this.m_emittedNoAppenderWarning = false;
            if (loggerFactory == null)
            {
                throw new ArgumentNullException("loggerFactory");
            }
            this.m_defaultFactory = loggerFactory;
            this.m_ht = Hashtable.Synchronized(new Hashtable());
        }

        internal void AddLevel(LevelEntry levelEntry)
        {
            if (levelEntry == null)
            {
                throw new ArgumentNullException("levelEntry");
            }
            if (levelEntry.Name == null)
            {
                throw new ArgumentNullException("levelEntry.Name");
            }
            if (levelEntry.Value == -1)
            {
                Level level = this.LevelMap[levelEntry.Name];
                if (level == null)
                {
                    throw new InvalidOperationException("Cannot redefine level [" + levelEntry.Name + "] because it is not defined in the LevelMap. To define the level supply the level value.");
                }
                levelEntry.Value = level.Value;
            }
            this.LevelMap.Add(levelEntry.Name, levelEntry.Value, levelEntry.DisplayName);
        }

        internal void AddProperty(PropertyEntry propertyEntry)
        {
            if (propertyEntry == null)
            {
                throw new ArgumentNullException("propertyEntry");
            }
            if (propertyEntry.Key == null)
            {
                throw new ArgumentNullException("propertyEntry.Key");
            }
            base.Properties[propertyEntry.Key] = propertyEntry.Value;
        }

        protected void BasicRepositoryConfigure(params IAppender[] appenders)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                foreach (IAppender appender in appenders)
                {
                    this.Root.AddAppender(appender);
                }
            }
            this.Configured = true;
            this.ConfigurationMessages = items;
            this.OnConfigurationChanged(new ConfigurationChangedEventArgs(items));
        }

        public void Clear()
        {
            this.m_ht.Clear();
        }

        private static void CollectAppender(ArrayList appenderList, IAppender appender)
        {
            if (!appenderList.Contains(appender))
            {
                appenderList.Add(appender);
                IAppenderAttachable container = appender as IAppenderAttachable;
                if (container != null)
                {
                    CollectAppenders(appenderList, container);
                }
            }
        }

        private static void CollectAppenders(ArrayList appenderList, IAppenderAttachable container)
        {
            foreach (IAppender appender in container.Appenders)
            {
                CollectAppender(appenderList, appender);
            }
        }

        public override ILogger Exists(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            return (this.m_ht[new LoggerKey(name)] as Logger);
        }

        public override IAppender[] GetAppenders()
        {
            ArrayList appenderList = new ArrayList();
            CollectAppenders(appenderList, this.Root);
            foreach (Logger logger in this.GetCurrentLoggers())
            {
                CollectAppenders(appenderList, logger);
            }
            return (IAppender[]) appenderList.ToArray(typeof(IAppender));
        }

        public override ILogger[] GetCurrentLoggers()
        {
            ArrayList list = new ArrayList(this.m_ht.Count);
            foreach (object obj2 in this.m_ht.Values)
            {
                if (obj2 is Logger)
                {
                    list.Add(obj2);
                }
            }
            return (Logger[]) list.ToArray(typeof(Logger));
        }

        public override ILogger GetLogger(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            return this.GetLogger(name, this.m_defaultFactory);
        }

        public Logger GetLogger(string name, ILoggerFactory factory)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            LoggerKey key = new LoggerKey(name);
            lock (this.m_ht)
            {
                Logger log = null;
                object obj2 = this.m_ht[key];
                if (obj2 == null)
                {
                    log = factory.CreateLogger(this, name);
                    log.Hierarchy = this;
                    this.m_ht[key] = log;
                    this.UpdateParents(log);
                    this.OnLoggerCreationEvent(log);
                    return log;
                }
                Logger logger2 = obj2 as Logger;
                if (logger2 != null)
                {
                    return logger2;
                }
                ProvisionNode pn = obj2 as ProvisionNode;
                if (pn != null)
                {
                    log = factory.CreateLogger(this, name);
                    log.Hierarchy = this;
                    this.m_ht[key] = log;
                    UpdateChildren(pn, log);
                    this.UpdateParents(log);
                    this.OnLoggerCreationEvent(log);
                    return log;
                }
                return null;
            }
        }

        public bool IsDisabled(Level level)
        {
            if (level == null)
            {
                throw new ArgumentNullException("level");
            }
            if (this.Configured)
            {
                return (this.Threshold > level);
            }
            return true;
        }

        public override void Log(LoggingEvent logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException("logEvent");
            }
            this.GetLogger(logEvent.LoggerName, this.m_defaultFactory).Log(logEvent);
        }

        void IBasicRepositoryConfigurator.Configure(IAppender appender)
        {
            this.BasicRepositoryConfigure(new IAppender[] { appender });
        }

        void IBasicRepositoryConfigurator.Configure(params IAppender[] appenders)
        {
            this.BasicRepositoryConfigure(appenders);
        }

        void IXmlRepositoryConfigurator.Configure(XmlElement element)
        {
            this.XmlRepositoryConfigure(element);
        }

        protected virtual void OnLoggerCreationEvent(Logger logger)
        {
            LoggerCreationEventHandler loggerCreatedEvent = this.m_loggerCreatedEvent;
            if (loggerCreatedEvent != null)
            {
                loggerCreatedEvent(this, new LoggerCreationEventArgs(logger));
            }
        }

        public override void ResetConfiguration()
        {
            this.Root.Level = this.LevelMap.LookupWithDefault(Level.Debug);
            this.Threshold = this.LevelMap.LookupWithDefault(Level.All);
            lock (this.m_ht)
            {
                this.Shutdown();
                foreach (Logger logger in this.GetCurrentLoggers())
                {
                    logger.Level = null;
                    logger.Additivity = true;
                }
            }
            base.ResetConfiguration();
            this.OnConfigurationChanged(null);
        }

        public override void Shutdown()
        {
            LogLog.Debug(declaringType, "Shutdown called on Hierarchy [" + this.Name + "]");
            this.Root.CloseNestedAppenders();
            lock (this.m_ht)
            {
                ILogger[] currentLoggers = this.GetCurrentLoggers();
                foreach (Logger logger in currentLoggers)
                {
                    logger.CloseNestedAppenders();
                }
                this.Root.RemoveAllAppenders();
                foreach (Logger logger in currentLoggers)
                {
                    logger.RemoveAllAppenders();
                }
            }
            base.Shutdown();
        }

        private static void UpdateChildren(ProvisionNode pn, Logger log)
        {
            for (int i = 0; i < pn.Count; i++)
            {
                Logger logger = (Logger) pn[i];
                if (!logger.Parent.Name.StartsWith(log.Name))
                {
                    log.Parent = logger.Parent;
                    logger.Parent = log;
                }
            }
        }

        private void UpdateParents(Logger log)
        {
            string name = log.Name;
            int length = name.Length;
            bool flag = false;
            for (int i = name.LastIndexOf('.', length - 1); i >= 0; i = name.LastIndexOf('.', i - 1))
            {
                LoggerKey key = new LoggerKey(name.Substring(0, i));
                object obj2 = this.m_ht[key];
                if (obj2 == null)
                {
                    ProvisionNode node = new ProvisionNode(log);
                    this.m_ht[key] = node;
                }
                else
                {
                    Logger logger = obj2 as Logger;
                    if (logger != null)
                    {
                        flag = true;
                        log.Parent = logger;
                        break;
                    }
                    ProvisionNode node2 = obj2 as ProvisionNode;
                    if (node2 != null)
                    {
                        node2.Add(log);
                    }
                    else
                    {
                        LogLog.Error(declaringType, "Unexpected object type [" + obj2.GetType() + "] in ht.", new LogException());
                    }
                }
            }
            if (!flag)
            {
                log.Parent = this.Root;
            }
        }

        protected void XmlRepositoryConfigure(XmlElement element)
        {
            ArrayList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                new XmlHierarchyConfigurator(this).Configure(element);
            }
            this.Configured = true;
            this.ConfigurationMessages = items;
            this.OnConfigurationChanged(new ConfigurationChangedEventArgs(items));
        }

        public bool EmittedNoAppenderWarning
        {
            get => 
                this.m_emittedNoAppenderWarning;
            set
            {
                this.m_emittedNoAppenderWarning = value;
            }
        }

        public ILoggerFactory LoggerFactory
        {
            get => 
                this.m_defaultFactory;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.m_defaultFactory = value;
            }
        }

        public Logger Root
        {
            get
            {
                if (this.m_root == null)
                {
                    lock (this)
                    {
                        if (this.m_root == null)
                        {
                            Logger logger = this.m_defaultFactory.CreateLogger(this, null);
                            logger.Hierarchy = this;
                            this.m_root = logger;
                        }
                    }
                }
                return this.m_root;
            }
        }

        internal class LevelEntry
        {
            private string m_levelDisplayName = null;
            private string m_levelName = null;
            private int m_levelValue = -1;

            public override string ToString() => 
                string.Concat(new object[] { "LevelEntry(Value=", this.m_levelValue, ", Name=", this.m_levelName, ", DisplayName=", this.m_levelDisplayName, ")" });

            public string DisplayName
            {
                get => 
                    this.m_levelDisplayName;
                set
                {
                    this.m_levelDisplayName = value;
                }
            }

            public string Name
            {
                get => 
                    this.m_levelName;
                set
                {
                    this.m_levelName = value;
                }
            }

            public int Value
            {
                get => 
                    this.m_levelValue;
                set
                {
                    this.m_levelValue = value;
                }
            }
        }
    }
}

