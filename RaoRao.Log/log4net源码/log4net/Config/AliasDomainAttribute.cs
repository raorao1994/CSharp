namespace log4net.Config
{
    using System;

    [Serializable, Obsolete("Use AliasRepositoryAttribute instead of AliasDomainAttribute"), AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    public sealed class AliasDomainAttribute : AliasRepositoryAttribute
    {
        public AliasDomainAttribute(string name) : base(name)
        {
        }
    }
}

