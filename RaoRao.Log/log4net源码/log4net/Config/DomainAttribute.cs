namespace log4net.Config
{
    using System;

    [Serializable, Obsolete("Use RepositoryAttribute instead of DomainAttribute"), AttributeUsage(AttributeTargets.Assembly)]
    public sealed class DomainAttribute : RepositoryAttribute
    {
        public DomainAttribute()
        {
        }

        public DomainAttribute(string name) : base(name)
        {
        }
    }
}

