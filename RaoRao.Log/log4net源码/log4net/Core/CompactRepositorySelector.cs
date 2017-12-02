namespace log4net.Core
{
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Threading;

    public class CompactRepositorySelector : IRepositorySelector
    {
        private static readonly Type declaringType = typeof(CompactRepositorySelector);
        private const string DefaultRepositoryName = "log4net-default-repository";
        private readonly Type m_defaultRepositoryType;
        private readonly Hashtable m_name2repositoryMap = new Hashtable();

        public event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent
        {
            add
            {
                this.m_loggerRepositoryCreatedEvent += value;
            }
            remove
            {
                this.m_loggerRepositoryCreatedEvent -= value;
            }
        }

        private event LoggerRepositoryCreationEventHandler m_loggerRepositoryCreatedEvent;

        public CompactRepositorySelector(Type defaultRepositoryType)
        {
            if (defaultRepositoryType == null)
            {
                throw new ArgumentNullException("defaultRepositoryType");
            }
            if (!typeof(ILoggerRepository).IsAssignableFrom(defaultRepositoryType))
            {
                throw SystemInfo.CreateArgumentOutOfRangeException("defaultRepositoryType", defaultRepositoryType, "Parameter: defaultRepositoryType, Value: [" + defaultRepositoryType + "] out of range. Argument must implement the ILoggerRepository interface");
            }
            this.m_defaultRepositoryType = defaultRepositoryType;
            LogLog.Debug(declaringType, "defaultRepositoryType [" + this.m_defaultRepositoryType + "]");
        }

        public ILoggerRepository CreateRepository(Assembly assembly, Type repositoryType)
        {
            if (repositoryType == null)
            {
                repositoryType = this.m_defaultRepositoryType;
            }
            lock (this)
            {
                ILoggerRepository repository = this.m_name2repositoryMap["log4net-default-repository"] as ILoggerRepository;
                if (repository == null)
                {
                    repository = this.CreateRepository("log4net-default-repository", repositoryType);
                }
                return repository;
            }
        }

        public ILoggerRepository CreateRepository(string repositoryName, Type repositoryType)
        {
            if (repositoryName == null)
            {
                throw new ArgumentNullException("repositoryName");
            }
            if (repositoryType == null)
            {
                repositoryType = this.m_defaultRepositoryType;
            }
            lock (this)
            {
                ILoggerRepository repository = null;
                repository = this.m_name2repositoryMap[repositoryName] as ILoggerRepository;
                if (repository != null)
                {
                    throw new LogException("Repository [" + repositoryName + "] is already defined. Repositories cannot be redefined.");
                }
                LogLog.Debug(declaringType, string.Concat(new object[] { "Creating repository [", repositoryName, "] using type [", repositoryType, "]" }));
                repository = (ILoggerRepository) Activator.CreateInstance(repositoryType);
                repository.Name = repositoryName;
                this.m_name2repositoryMap[repositoryName] = repository;
                this.OnLoggerRepositoryCreatedEvent(repository);
                return repository;
            }
        }

        public bool ExistsRepository(string repositoryName)
        {
            lock (this)
            {
                return this.m_name2repositoryMap.ContainsKey(repositoryName);
            }
        }

        public ILoggerRepository[] GetAllRepositories()
        {
            lock (this)
            {
                ICollection values = this.m_name2repositoryMap.Values;
                ILoggerRepository[] array = new ILoggerRepository[values.Count];
                values.CopyTo(array, 0);
                return array;
            }
        }

        public ILoggerRepository GetRepository(Assembly assembly) => 
            this.CreateRepository(assembly, this.m_defaultRepositoryType);

        public ILoggerRepository GetRepository(string repositoryName)
        {
            if (repositoryName == null)
            {
                throw new ArgumentNullException("repositoryName");
            }
            lock (this)
            {
                ILoggerRepository repository = this.m_name2repositoryMap[repositoryName] as ILoggerRepository;
                if (repository == null)
                {
                    throw new LogException("Repository [" + repositoryName + "] is NOT defined.");
                }
                return repository;
            }
        }

        protected virtual void OnLoggerRepositoryCreatedEvent(ILoggerRepository repository)
        {
            LoggerRepositoryCreationEventHandler loggerRepositoryCreatedEvent = this.m_loggerRepositoryCreatedEvent;
            if (loggerRepositoryCreatedEvent != null)
            {
                loggerRepositoryCreatedEvent(this, new LoggerRepositoryCreationEventArgs(repository));
            }
        }
    }
}

