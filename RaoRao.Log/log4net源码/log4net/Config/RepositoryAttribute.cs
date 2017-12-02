namespace log4net.Config
{
    using System;

    [Serializable, AttributeUsage(AttributeTargets.Assembly)]
    public class RepositoryAttribute : Attribute
    {
        private string m_name;
        private Type m_repositoryType;

        public RepositoryAttribute()
        {
            this.m_name = null;
            this.m_repositoryType = null;
        }

        public RepositoryAttribute(string name)
        {
            this.m_name = null;
            this.m_repositoryType = null;
            this.m_name = name;
        }

        public string Name
        {
            get => 
                this.m_name;
            set
            {
                this.m_name = value;
            }
        }

        public Type RepositoryType
        {
            get => 
                this.m_repositoryType;
            set
            {
                this.m_repositoryType = value;
            }
        }
    }
}

