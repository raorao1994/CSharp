namespace log4net.Config
{
    using System;
    using System.Configuration;
    using System.Xml;

    public class Log4NetConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section) => 
            section;
    }
}

