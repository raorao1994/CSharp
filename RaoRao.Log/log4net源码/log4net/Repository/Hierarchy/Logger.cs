namespace log4net.Repository.Hierarchy
{
    using log4net.Appender;
    using log4net.Core;
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Security;

    public abstract class Logger : IAppenderAttachable, ILogger
    {
        private static readonly Type declaringType = typeof(Logger);
        private bool m_additive = true;
        private AppenderAttachedImpl m_appenderAttachedImpl;
        private readonly ReaderWriterLock m_appenderLock = new ReaderWriterLock();
        private log4net.Repository.Hierarchy.Hierarchy m_hierarchy;
        private log4net.Core.Level m_level;
        private readonly string m_name;
        private Logger m_parent;

        protected Logger(string name)
        {
            this.m_name = string.Intern(name);
        }

        public virtual void AddAppender(IAppender newAppender)
        {
            if (newAppender == null)
            {
                throw new ArgumentNullException("newAppender");
            }
            this.m_appenderLock.AcquireWriterLock();
            try
            {
                if (this.m_appenderAttachedImpl == null)
                {
                    this.m_appenderAttachedImpl = new AppenderAttachedImpl();
                }
                this.m_appenderAttachedImpl.AddAppender(newAppender);
            }
            finally
            {
                this.m_appenderLock.ReleaseWriterLock();
            }
        }

        protected virtual void CallAppenders(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            int num = 0;
            for (Logger logger = this; logger != null; logger = logger.m_parent)
            {
                if (logger.m_appenderAttachedImpl != null)
                {
                    logger.m_appenderLock.AcquireReaderLock();
                    try
                    {
                        if (logger.m_appenderAttachedImpl != null)
                        {
                            num += logger.m_appenderAttachedImpl.AppendLoopOnAppenders(loggingEvent);
                        }
                    }
                    finally
                    {
                        logger.m_appenderLock.ReleaseReaderLock();
                    }
                }
                if (!logger.m_additive)
                {
                    break;
                }
            }
            if (!this.m_hierarchy.EmittedNoAppenderWarning && (num == 0))
            {
                LogLog.Debug(declaringType, "No appenders could be found for logger [" + this.Name + "] repository [" + this.Repository.Name + "]");
                LogLog.Debug(declaringType, "Please initialize the log4net system properly.");
                try
                {
                    LogLog.Debug(declaringType, "    Current AppDomain context information: ");
                    LogLog.Debug(declaringType, "       BaseDirectory   : " + SystemInfo.ApplicationBaseDirectory);
                    LogLog.Debug(declaringType, "       FriendlyName    : " + AppDomain.CurrentDomain.FriendlyName);
                    LogLog.Debug(declaringType, "       DynamicDirectory: " + AppDomain.CurrentDomain.DynamicDirectory);
                }
                catch (SecurityException)
                {
                }
                this.m_hierarchy.EmittedNoAppenderWarning = true;
            }
        }

        public virtual void CloseNestedAppenders()
        {
            this.m_appenderLock.AcquireWriterLock();
            try
            {
                if (this.m_appenderAttachedImpl != null)
                {
                    AppenderCollection appenders = this.m_appenderAttachedImpl.Appenders;
                    foreach (IAppender appender in appenders)
                    {
                        if (appender is IAppenderAttachable)
                        {
                            appender.Close();
                        }
                    }
                }
            }
            finally
            {
                this.m_appenderLock.ReleaseWriterLock();
            }
        }

        protected virtual void ForcedLog(LoggingEvent logEvent)
        {
            logEvent.EnsureRepository(this.Hierarchy);
            this.CallAppenders(logEvent);
        }

        protected virtual void ForcedLog(Type callerStackBoundaryDeclaringType, log4net.Core.Level level, object message, Exception exception)
        {
            this.CallAppenders(new LoggingEvent(callerStackBoundaryDeclaringType, this.Hierarchy, this.Name, level, message, exception));
        }

        public virtual IAppender GetAppender(string name)
        {
            IAppender appender;
            this.m_appenderLock.AcquireReaderLock();
            try
            {
                if ((this.m_appenderAttachedImpl == null) || (name == null))
                {
                    return null;
                }
                appender = this.m_appenderAttachedImpl.GetAppender(name);
            }
            finally
            {
                this.m_appenderLock.ReleaseReaderLock();
            }
            return appender;
        }

        public virtual bool IsEnabledFor(log4net.Core.Level level)
        {
            try
            {
                if (level != null)
                {
                    if (this.m_hierarchy.IsDisabled(level))
                    {
                        return false;
                    }
                    return (level >= this.EffectiveLevel);
                }
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "Exception while logging", exception);
            }
            return false;
        }

        public virtual void Log(LoggingEvent logEvent)
        {
            try
            {
                if ((logEvent != null) && this.IsEnabledFor(logEvent.Level))
                {
                    this.ForcedLog(logEvent);
                }
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "Exception while logging", exception);
            }
        }

        public virtual void Log(log4net.Core.Level level, object message, Exception exception)
        {
            if (this.IsEnabledFor(level))
            {
                this.ForcedLog(declaringType, level, message, exception);
            }
        }

        public virtual void Log(Type callerStackBoundaryDeclaringType, log4net.Core.Level level, object message, Exception exception)
        {
            try
            {
                if (this.IsEnabledFor(level))
                {
                    this.ForcedLog((callerStackBoundaryDeclaringType != null) ? callerStackBoundaryDeclaringType : declaringType, level, message, exception);
                }
            }
            catch (Exception exception2)
            {
                LogLog.Error(declaringType, "Exception while logging", exception2);
            }
        }

        public virtual void RemoveAllAppenders()
        {
            this.m_appenderLock.AcquireWriterLock();
            try
            {
                if (this.m_appenderAttachedImpl != null)
                {
                    this.m_appenderAttachedImpl.RemoveAllAppenders();
                    this.m_appenderAttachedImpl = null;
                }
            }
            finally
            {
                this.m_appenderLock.ReleaseWriterLock();
            }
        }

        public virtual IAppender RemoveAppender(IAppender appender)
        {
            this.m_appenderLock.AcquireWriterLock();
            try
            {
                if ((appender != null) && (this.m_appenderAttachedImpl != null))
                {
                    return this.m_appenderAttachedImpl.RemoveAppender(appender);
                }
            }
            finally
            {
                this.m_appenderLock.ReleaseWriterLock();
            }
            return null;
        }

        public virtual IAppender RemoveAppender(string name)
        {
            this.m_appenderLock.AcquireWriterLock();
            try
            {
                if ((name != null) && (this.m_appenderAttachedImpl != null))
                {
                    return this.m_appenderAttachedImpl.RemoveAppender(name);
                }
            }
            finally
            {
                this.m_appenderLock.ReleaseWriterLock();
            }
            return null;
        }

        public virtual bool Additivity
        {
            get => 
                this.m_additive;
            set
            {
                this.m_additive = value;
            }
        }

        public virtual AppenderCollection Appenders
        {
            get
            {
                AppenderCollection appenders;
                this.m_appenderLock.AcquireReaderLock();
                try
                {
                    if (this.m_appenderAttachedImpl == null)
                    {
                        return AppenderCollection.EmptyCollection;
                    }
                    appenders = this.m_appenderAttachedImpl.Appenders;
                }
                finally
                {
                    this.m_appenderLock.ReleaseReaderLock();
                }
                return appenders;
            }
        }

        public virtual log4net.Core.Level EffectiveLevel
        {
            get
            {
                for (Logger logger = this; logger != null; logger = logger.m_parent)
                {
                    log4net.Core.Level level = logger.m_level;
                    if (level != null)
                    {
                        return level;
                    }
                }
                return null;
            }
        }

        public virtual log4net.Repository.Hierarchy.Hierarchy Hierarchy
        {
            get => 
                this.m_hierarchy;
            set
            {
                this.m_hierarchy = value;
            }
        }

        public virtual log4net.Core.Level Level
        {
            get => 
                this.m_level;
            set
            {
                this.m_level = value;
            }
        }

        public virtual string Name =>
            this.m_name;

        public virtual Logger Parent
        {
            get => 
                this.m_parent;
            set
            {
                this.m_parent = value;
            }
        }

        public ILoggerRepository Repository =>
            this.m_hierarchy;
    }
}

