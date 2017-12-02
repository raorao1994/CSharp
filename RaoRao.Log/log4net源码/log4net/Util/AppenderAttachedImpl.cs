namespace log4net.Util
{
    using log4net.Appender;
    using log4net.Core;
    using System;

    public class AppenderAttachedImpl : IAppenderAttachable
    {
        private static readonly Type declaringType = typeof(AppenderAttachedImpl);
        private IAppender[] m_appenderArray;
        private AppenderCollection m_appenderList;

        public void AddAppender(IAppender newAppender)
        {
            if (newAppender == null)
            {
                throw new ArgumentNullException("newAppender");
            }
            this.m_appenderArray = null;
            if (this.m_appenderList == null)
            {
                this.m_appenderList = new AppenderCollection(1);
            }
            if (!this.m_appenderList.Contains(newAppender))
            {
                this.m_appenderList.Add(newAppender);
            }
        }

        public int AppendLoopOnAppenders(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            if (this.m_appenderList == null)
            {
                return 0;
            }
            if (this.m_appenderArray == null)
            {
                this.m_appenderArray = this.m_appenderList.ToArray();
            }
            foreach (IAppender appender in this.m_appenderArray)
            {
                try
                {
                    appender.DoAppend(loggingEvent);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Failed to append to appender [" + appender.Name + "]", exception);
                }
            }
            return this.m_appenderList.Count;
        }

        public int AppendLoopOnAppenders(LoggingEvent[] loggingEvents)
        {
            if (loggingEvents == null)
            {
                throw new ArgumentNullException("loggingEvents");
            }
            if (loggingEvents.Length == 0)
            {
                throw new ArgumentException("loggingEvents array must not be empty", "loggingEvents");
            }
            if (loggingEvents.Length == 1)
            {
                return this.AppendLoopOnAppenders(loggingEvents[0]);
            }
            if (this.m_appenderList == null)
            {
                return 0;
            }
            if (this.m_appenderArray == null)
            {
                this.m_appenderArray = this.m_appenderList.ToArray();
            }
            foreach (IAppender appender in this.m_appenderArray)
            {
                try
                {
                    CallAppend(appender, loggingEvents);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Failed to append to appender [" + appender.Name + "]", exception);
                }
            }
            return this.m_appenderList.Count;
        }

        private static void CallAppend(IAppender appender, LoggingEvent[] loggingEvents)
        {
            IBulkAppender appender2 = appender as IBulkAppender;
            if (appender2 != null)
            {
                appender2.DoAppend(loggingEvents);
            }
            else
            {
                foreach (LoggingEvent event2 in loggingEvents)
                {
                    appender.DoAppend(event2);
                }
            }
        }

        public IAppender GetAppender(string name)
        {
            if ((this.m_appenderList != null) && (name != null))
            {
                foreach (IAppender appender in this.m_appenderList)
                {
                    if (name == appender.Name)
                    {
                        return appender;
                    }
                }
            }
            return null;
        }

        public void RemoveAllAppenders()
        {
            if (this.m_appenderList != null)
            {
                foreach (IAppender appender in this.m_appenderList)
                {
                    try
                    {
                        appender.Close();
                    }
                    catch (Exception exception)
                    {
                        LogLog.Error(declaringType, "Failed to Close appender [" + appender.Name + "]", exception);
                    }
                }
                this.m_appenderList = null;
                this.m_appenderArray = null;
            }
        }

        public IAppender RemoveAppender(IAppender appender)
        {
            if ((appender != null) && (this.m_appenderList != null))
            {
                this.m_appenderList.Remove(appender);
                if (this.m_appenderList.Count == 0)
                {
                    this.m_appenderList = null;
                }
                this.m_appenderArray = null;
            }
            return appender;
        }

        public IAppender RemoveAppender(string name) => 
            this.RemoveAppender(this.GetAppender(name));

        public AppenderCollection Appenders
        {
            get
            {
                if (this.m_appenderList == null)
                {
                    return AppenderCollection.EmptyCollection;
                }
                return AppenderCollection.ReadOnly(this.m_appenderList);
            }
        }
    }
}

