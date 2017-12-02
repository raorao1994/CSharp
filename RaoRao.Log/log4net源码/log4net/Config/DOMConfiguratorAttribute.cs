namespace log4net.Config
{
    using System;

    [Serializable, AttributeUsage(AttributeTargets.Assembly), Obsolete("Use XmlConfiguratorAttribute instead of DOMConfiguratorAttribute")]
    public sealed class DOMConfiguratorAttribute : XmlConfiguratorAttribute
    {
    }
}

