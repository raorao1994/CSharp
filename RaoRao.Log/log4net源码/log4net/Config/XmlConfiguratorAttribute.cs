namespace log4net.Config
{
    using log4net.Repository;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;

    [Serializable, AttributeUsage(AttributeTargets.Assembly)]
    public class XmlConfiguratorAttribute : ConfiguratorAttribute
    {
        private static readonly Type declaringType = typeof(XmlConfiguratorAttribute);
        private string m_configFile;
        private string m_configFileExtension;
        private bool m_configureAndWatch;

        public XmlConfiguratorAttribute() : base(0)
        {
            this.m_configFile = null;
            this.m_configFileExtension = null;
            this.m_configureAndWatch = false;
        }

        public override void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository)
        {
            IList items = new ArrayList();
            using (new LogLog.LogReceivedAdapter(items))
            {
                string uriString = null;
                try
                {
                    uriString = SystemInfo.ApplicationBaseDirectory;
                }
                catch
                {
                }
                if ((uriString == null) || new Uri(uriString).IsFile)
                {
                    this.ConfigureFromFile(sourceAssembly, targetRepository);
                }
                else
                {
                    this.ConfigureFromUri(sourceAssembly, targetRepository);
                }
            }
            targetRepository.ConfigurationMessages = items;
        }

        private void ConfigureFromFile(ILoggerRepository targetRepository, FileInfo configFile)
        {
            if (this.m_configureAndWatch)
            {
                XmlConfigurator.ConfigureAndWatch(targetRepository, configFile);
            }
            else
            {
                XmlConfigurator.Configure(targetRepository, configFile);
            }
        }

        private void ConfigureFromFile(Assembly sourceAssembly, ILoggerRepository targetRepository)
        {
            string fileName = null;
            Exception exception;
            string applicationBaseDirectory;
            if ((this.m_configFile == null) || (this.m_configFile.Length == 0))
            {
                if ((this.m_configFileExtension == null) || (this.m_configFileExtension.Length == 0))
                {
                    try
                    {
                        fileName = SystemInfo.ConfigurationFileLocation;
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        LogLog.Error(declaringType, "XmlConfiguratorAttribute: Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when ConfigFile and ConfigFileExtension properties are not set.", exception);
                    }
                }
                else
                {
                    if (this.m_configFileExtension[0] != '.')
                    {
                        this.m_configFileExtension = "." + this.m_configFileExtension;
                    }
                    applicationBaseDirectory = null;
                    try
                    {
                        applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        LogLog.Error(declaringType, "Exception getting ApplicationBaseDirectory. Must be able to resolve ApplicationBaseDirectory and AssemblyFileName when ConfigFileExtension property is set.", exception);
                    }
                    if (applicationBaseDirectory != null)
                    {
                        fileName = Path.Combine(applicationBaseDirectory, SystemInfo.AssemblyFileName(sourceAssembly) + this.m_configFileExtension);
                    }
                }
            }
            else
            {
                applicationBaseDirectory = null;
                try
                {
                    applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                }
                catch (Exception exception3)
                {
                    exception = exception3;
                    LogLog.Warn(declaringType, "Exception getting ApplicationBaseDirectory. ConfigFile property path [" + this.m_configFile + "] will be treated as an absolute path.", exception);
                }
                if (applicationBaseDirectory != null)
                {
                    fileName = Path.Combine(applicationBaseDirectory, this.m_configFile);
                }
                else
                {
                    fileName = this.m_configFile;
                }
            }
            if (fileName != null)
            {
                this.ConfigureFromFile(targetRepository, new FileInfo(fileName));
            }
        }

        private void ConfigureFromUri(Assembly sourceAssembly, ILoggerRepository targetRepository)
        {
            Uri configUri = null;
            Exception exception;
            if ((this.m_configFile == null) || (this.m_configFile.Length == 0))
            {
                string configurationFileLocation;
                if ((this.m_configFileExtension == null) || (this.m_configFileExtension.Length == 0))
                {
                    configurationFileLocation = null;
                    try
                    {
                        configurationFileLocation = SystemInfo.ConfigurationFileLocation;
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        LogLog.Error(declaringType, "XmlConfiguratorAttribute: Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when ConfigFile and ConfigFileExtension properties are not set.", exception);
                    }
                    if (configurationFileLocation != null)
                    {
                        configUri = new Uri(configurationFileLocation);
                    }
                }
                else
                {
                    if (this.m_configFileExtension[0] != '.')
                    {
                        this.m_configFileExtension = "." + this.m_configFileExtension;
                    }
                    configurationFileLocation = null;
                    try
                    {
                        configurationFileLocation = SystemInfo.ConfigurationFileLocation;
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        LogLog.Error(declaringType, "XmlConfiguratorAttribute: Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when the ConfigFile property are not set.", exception);
                    }
                    if (configurationFileLocation != null)
                    {
                        UriBuilder builder = new UriBuilder(new Uri(configurationFileLocation));
                        string path = builder.Path;
                        int length = path.LastIndexOf(".");
                        if (length >= 0)
                        {
                            path = path.Substring(0, length);
                        }
                        path = path + this.m_configFileExtension;
                        builder.Path = path;
                        configUri = builder.Uri;
                    }
                }
            }
            else
            {
                string uriString = null;
                try
                {
                    uriString = SystemInfo.ApplicationBaseDirectory;
                }
                catch (Exception exception3)
                {
                    exception = exception3;
                    LogLog.Warn(declaringType, "Exception getting ApplicationBaseDirectory. ConfigFile property path [" + this.m_configFile + "] will be treated as an absolute URI.", exception);
                }
                if (uriString != null)
                {
                    configUri = new Uri(new Uri(uriString), this.m_configFile);
                }
                else
                {
                    configUri = new Uri(this.m_configFile);
                }
            }
            if (configUri != null)
            {
                if (configUri.IsFile)
                {
                    this.ConfigureFromFile(targetRepository, new FileInfo(configUri.LocalPath));
                }
                else
                {
                    if (this.m_configureAndWatch)
                    {
                        LogLog.Warn(declaringType, "XmlConfiguratorAttribute: Unable to watch config file loaded from a URI");
                    }
                    XmlConfigurator.Configure(targetRepository, configUri);
                }
            }
        }

        public string ConfigFile
        {
            get => 
                this.m_configFile;
            set
            {
                this.m_configFile = value;
            }
        }

        public string ConfigFileExtension
        {
            get => 
                this.m_configFileExtension;
            set
            {
                this.m_configFileExtension = value;
            }
        }

        public bool Watch
        {
            get => 
                this.m_configureAndWatch;
            set
            {
                this.m_configureAndWatch = value;
            }
        }
    }
}

