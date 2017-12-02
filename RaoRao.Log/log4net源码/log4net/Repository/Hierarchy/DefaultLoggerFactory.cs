namespace log4net.Repository.Hierarchy
{
    using log4net.Core;
    using log4net.Repository;
    using System;

    internal class DefaultLoggerFactory : ILoggerFactory
    {
        internal DefaultLoggerFactory()
        {
        }

        public Logger CreateLogger(ILoggerRepository repository, string name)
        {
            if (name == null)
            {
                return new RootLogger(repository.LevelMap.LookupWithDefault(Level.Debug));
            }
            return new LoggerImpl(name);
        }

        internal sealed class LoggerImpl : Logger
        {
            internal LoggerImpl(string name) : base(name)
            {
            }
        }
    }
}

