namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using log4net.Util;
    using System;
    using System.Diagnostics;
    using System.Security;
    using System.Threading;

    public class EventLogAppender : AppenderSkeleton
    {
        private static readonly Type declaringType = typeof(EventLogAppender);
        private string m_applicationName;
        private short m_category;
        private int m_eventId;
        private LevelMapping m_levelMapping;
        private string m_logName;
        private string m_machineName;
        private log4net.Core.SecurityContext m_securityContext;

        public EventLogAppender()
        {
            this.m_levelMapping = new LevelMapping();
            this.m_eventId = 0;
            this.m_category = 0;
            this.m_applicationName = Thread.GetDomain().FriendlyName;
            this.m_logName = "Application";
            this.m_machineName = ".";
        }

        [Obsolete("Instead use the default constructor and set the Layout property")]
        public EventLogAppender(ILayout layout) : this()
        {
            this.Layout = layout;
        }

        public override void ActivateOptions()
        {
            try
            {
                IDisposable disposable;
                base.ActivateOptions();
                if (this.m_securityContext == null)
                {
                    this.m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
                }
                bool flag = false;
                string str = null;
                using (disposable = this.SecurityContext.Impersonate(this))
                {
                    flag = EventLog.SourceExists(this.m_applicationName);
                    if (flag)
                    {
                        str = EventLog.LogNameFromSourceName(this.m_applicationName, this.m_machineName);
                    }
                }
                if (flag && (str != this.m_logName))
                {
                    LogLog.Debug(declaringType, "Changing event source [" + this.m_applicationName + "] from log [" + str + "] to log [" + this.m_logName + "]");
                }
                else if (!flag)
                {
                    LogLog.Debug(declaringType, "Creating event source Source [" + this.m_applicationName + "] in log " + this.m_logName + "]");
                }
                string str2 = null;
                using (disposable = this.SecurityContext.Impersonate(this))
                {
                    if (flag && (str != this.m_logName))
                    {
                        EventLog.DeleteEventSource(this.m_applicationName, this.m_machineName);
                        CreateEventSource(this.m_applicationName, this.m_logName, this.m_machineName);
                        str2 = EventLog.LogNameFromSourceName(this.m_applicationName, this.m_machineName);
                    }
                    else if (!flag)
                    {
                        CreateEventSource(this.m_applicationName, this.m_logName, this.m_machineName);
                        str2 = EventLog.LogNameFromSourceName(this.m_applicationName, this.m_machineName);
                    }
                }
                this.m_levelMapping.ActivateOptions();
                LogLog.Debug(declaringType, "Source [" + this.m_applicationName + "] is registered to log [" + str2 + "]");
            }
            catch (SecurityException exception)
            {
                this.ErrorHandler.Error("Caught a SecurityException trying to access the EventLog.  Most likely the event source " + this.m_applicationName + " doesn't exist and must be created by a local administrator.  Will disable EventLogAppender.  See http://logging.apache.org/log4net/release/faq.html#trouble-EventLog", exception);
                base.Threshold = Level.Off;
            }
        }

        public void AddMapping(Level2EventLogEntryType mapping)
        {
            this.m_levelMapping.Add(mapping);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            int eventId = this.m_eventId;
            object property = loggingEvent.LookupProperty("EventID");
            if (property != null)
            {
                if (property is int)
                {
                    eventId = (int) property;
                }
                else
                {
                    string s = property as string;
                    if (s == null)
                    {
                        s = property.ToString();
                    }
                    if ((s != null) && (s.Length > 0))
                    {
                        int num2;
                        if (SystemInfo.TryParse(s, out num2))
                        {
                            eventId = num2;
                        }
                        else
                        {
                            this.ErrorHandler.Error("Unable to parse event ID property [" + s + "].");
                        }
                    }
                }
            }
            short category = this.m_category;
            object obj3 = loggingEvent.LookupProperty("Category");
            if (obj3 != null)
            {
                if (obj3 is short)
                {
                    category = (short) obj3;
                }
                else
                {
                    string str2 = obj3 as string;
                    if (str2 == null)
                    {
                        str2 = obj3.ToString();
                    }
                    if ((str2 != null) && (str2.Length > 0))
                    {
                        short num4;
                        if (SystemInfo.TryParse(str2, out num4))
                        {
                            category = num4;
                        }
                        else
                        {
                            this.ErrorHandler.Error("Unable to parse event category property [" + str2 + "].");
                        }
                    }
                }
            }
            try
            {
                string message = base.RenderLoggingEvent(loggingEvent);
                if (message.Length > 0x7d00)
                {
                    message = message.Substring(0, 0x7d00);
                }
                EventLogEntryType entryType = this.GetEntryType(loggingEvent.Level);
                using (this.SecurityContext.Impersonate(this))
                {
                    EventLog.WriteEntry(this.m_applicationName, message, entryType, eventId, category);
                }
            }
            catch (Exception exception)
            {
                this.ErrorHandler.Error("Unable to write to event log [" + this.m_logName + "] using source [" + this.m_applicationName + "]", exception);
            }
        }

        private static void CreateEventSource(string source, string logName, string machineName)
        {
            EventSourceCreationData sourceData = new EventSourceCreationData(source, logName) {
                MachineName = machineName
            };
            EventLog.CreateEventSource(sourceData);
        }

        protected virtual EventLogEntryType GetEntryType(Level level)
        {
            Level2EventLogEntryType type = this.m_levelMapping.Lookup(level) as Level2EventLogEntryType;
            if (type != null)
            {
                return type.EventLogEntryType;
            }
            if (level >= Level.Error)
            {
                return EventLogEntryType.Error;
            }
            if (level == Level.Warn)
            {
                return EventLogEntryType.Warning;
            }
            return EventLogEntryType.Information;
        }

        public string ApplicationName
        {
            get => 
                this.m_applicationName;
            set
            {
                this.m_applicationName = value;
            }
        }

        public short Category
        {
            get => 
                this.m_category;
            set
            {
                this.m_category = value;
            }
        }

        public int EventId
        {
            get => 
                this.m_eventId;
            set
            {
                this.m_eventId = value;
            }
        }

        public string LogName
        {
            get => 
                this.m_logName;
            set
            {
                this.m_logName = value;
            }
        }

        public string MachineName
        {
            get => 
                this.m_machineName;
            set
            {
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

        public class Level2EventLogEntryType : LevelMappingEntry
        {
            private System.Diagnostics.EventLogEntryType m_entryType;

            public System.Diagnostics.EventLogEntryType EventLogEntryType
            {
                get => 
                    this.m_entryType;
                set
                {
                    this.m_entryType = value;
                }
            }
        }
    }
}

