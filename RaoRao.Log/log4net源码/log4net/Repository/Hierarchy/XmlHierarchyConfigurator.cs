namespace log4net.Repository.Hierarchy
{
    using log4net.Appender;
    using log4net.Core;
    using log4net.ObjectRenderer;
    using log4net.Util;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Reflection;
    using System.Security;
    using System.Xml;

    public class XmlHierarchyConfigurator
    {
        private const string ADDITIVITY_ATTR = "additivity";
        private const string APPENDER_REF_TAG = "appender-ref";
        private const string APPENDER_TAG = "appender";
        private const string CATEGORY_TAG = "category";
        private const string CONFIG_DEBUG_ATTR = "configDebug";
        private const string CONFIG_UPDATE_MODE_ATTR = "update";
        private const string CONFIGURATION_TAG = "log4net";
        private static readonly Type declaringType = typeof(XmlHierarchyConfigurator);
        private const string EMIT_INTERNAL_DEBUG_ATTR = "emitDebug";
        private const string INHERITED = "inherited";
        private const string INTERNAL_DEBUG_ATTR = "debug";
        private const string LEVEL_TAG = "level";
        private const string LOGGER_TAG = "logger";
        private Hashtable m_appenderBag;
        private readonly log4net.Repository.Hierarchy.Hierarchy m_hierarchy;
        private const string NAME_ATTR = "name";
        private const string PARAM_TAG = "param";
        private const string PRIORITY_TAG = "priority";
        private const string REF_ATTR = "ref";
        private const string RENDERED_TYPE_ATTR = "renderedClass";
        private const string RENDERER_TAG = "renderer";
        private const string RENDERING_TYPE_ATTR = "renderingClass";
        private const string ROOT_TAG = "root";
        private const string THRESHOLD_ATTR = "threshold";
        private const string TYPE_ATTR = "type";
        private const string VALUE_ATTR = "value";

        public XmlHierarchyConfigurator(log4net.Repository.Hierarchy.Hierarchy hierarchy)
        {
            this.m_hierarchy = hierarchy;
            this.m_appenderBag = new Hashtable();
        }

        public void Configure(XmlElement element)
        {
            if ((element != null) && (this.m_hierarchy != null))
            {
                if (element.LocalName != "log4net")
                {
                    LogLog.Error(declaringType, "Xml element is - not a <log4net> element.");
                }
                else
                {
                    if (!LogLog.EmitInternalMessages)
                    {
                        string argValue = element.GetAttribute("emitDebug");
                        LogLog.Debug(declaringType, "emitDebug attribute [" + argValue + "].");
                        if ((argValue.Length > 0) && (argValue != "null"))
                        {
                            LogLog.EmitInternalMessages = OptionConverter.ToBoolean(argValue, true);
                        }
                        else
                        {
                            LogLog.Debug(declaringType, "Ignoring emitDebug attribute.");
                        }
                    }
                    if (!LogLog.InternalDebugging)
                    {
                        string str3 = element.GetAttribute("debug");
                        LogLog.Debug(declaringType, "debug attribute [" + str3 + "].");
                        if ((str3.Length > 0) && (str3 != "null"))
                        {
                            LogLog.InternalDebugging = OptionConverter.ToBoolean(str3, true);
                        }
                        else
                        {
                            LogLog.Debug(declaringType, "Ignoring debug attribute.");
                        }
                        string str4 = element.GetAttribute("configDebug");
                        if ((str4.Length > 0) && (str4 != "null"))
                        {
                            LogLog.Warn(declaringType, "The \"configDebug\" attribute is deprecated.");
                            LogLog.Warn(declaringType, "Use the \"debug\" attribute instead.");
                            LogLog.InternalDebugging = OptionConverter.ToBoolean(str4, true);
                        }
                    }
                    ConfigUpdateMode merge = ConfigUpdateMode.Merge;
                    string attribute = element.GetAttribute("update");
                    if ((attribute != null) && (attribute.Length > 0))
                    {
                        try
                        {
                            merge = (ConfigUpdateMode) OptionConverter.ConvertStringTo(typeof(ConfigUpdateMode), attribute);
                        }
                        catch
                        {
                            LogLog.Error(declaringType, "Invalid update attribute value [" + attribute + "]");
                        }
                    }
                    LogLog.Debug(declaringType, "Configuration update mode [" + merge.ToString() + "].");
                    if (merge == ConfigUpdateMode.Overwrite)
                    {
                        this.m_hierarchy.ResetConfiguration();
                        LogLog.Debug(declaringType, "Configuration reset before reading config.");
                    }
                    foreach (XmlNode node in element.ChildNodes)
                    {
                        if (node.NodeType == XmlNodeType.Element)
                        {
                            XmlElement loggerElement = (XmlElement) node;
                            if (loggerElement.LocalName == "logger")
                            {
                                this.ParseLogger(loggerElement);
                            }
                            else if (loggerElement.LocalName == "category")
                            {
                                this.ParseLogger(loggerElement);
                            }
                            else if (loggerElement.LocalName == "root")
                            {
                                this.ParseRoot(loggerElement);
                            }
                            else if (loggerElement.LocalName == "renderer")
                            {
                                this.ParseRenderer(loggerElement);
                            }
                            else if (loggerElement.LocalName != "appender")
                            {
                                this.SetParameter(loggerElement, this.m_hierarchy);
                            }
                        }
                    }
                    string str6 = element.GetAttribute("threshold");
                    LogLog.Debug(declaringType, "Hierarchy Threshold [" + str6 + "]");
                    if ((str6.Length > 0) && (str6 != "null"))
                    {
                        Level level = (Level) this.ConvertStringTo(typeof(Level), str6);
                        if (level != null)
                        {
                            this.m_hierarchy.Threshold = level;
                        }
                        else
                        {
                            LogLog.Warn(declaringType, "Unable to set hierarchy threshold using value [" + str6 + "] (with acceptable conversion types)");
                        }
                    }
                }
            }
        }

        protected object ConvertStringTo(Type type, string value)
        {
            if (typeof(Level) == type)
            {
                Level level = this.m_hierarchy.LevelMap[value];
                if (level == null)
                {
                    LogLog.Error(declaringType, "XmlHierarchyConfigurator: Unknown Level Specified [" + value + "]");
                }
                return level;
            }
            return OptionConverter.ConvertStringTo(type, value);
        }

        protected object CreateObjectFromXml(XmlElement element, Type defaultTargetType, Type typeConstraint)
        {
            Type c = null;
            string attribute = element.GetAttribute("type");
            if ((attribute == null) || (attribute.Length == 0))
            {
                if (defaultTargetType == null)
                {
                    LogLog.Error(declaringType, "Object type not specified. Cannot create object of type [" + typeConstraint.FullName + "]. Missing Value or Type.");
                    return null;
                }
                c = defaultTargetType;
            }
            else
            {
                try
                {
                    c = SystemInfo.GetTypeFromString(attribute, true, true);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Failed to find type [" + attribute + "]", exception);
                    return null;
                }
            }
            bool flag = false;
            if ((typeConstraint != null) && !typeConstraint.IsAssignableFrom(c))
            {
                if (!OptionConverter.CanConvertTypeTo(c, typeConstraint))
                {
                    LogLog.Error(declaringType, "Object type [" + c.FullName + "] is not assignable to type [" + typeConstraint.FullName + "]. There are no acceptable type conversions.");
                    return null;
                }
                flag = true;
            }
            object target = null;
            try
            {
                target = Activator.CreateInstance(c);
            }
            catch (Exception exception2)
            {
                LogLog.Error(declaringType, "XmlHierarchyConfigurator: Failed to construct object of type [" + c.FullName + "] Exception: " + exception2.ToString());
            }
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    this.SetParameter((XmlElement) node, target);
                }
            }
            IOptionHandler handler = target as IOptionHandler;
            if (handler != null)
            {
                handler.ActivateOptions();
            }
            if (flag)
            {
                return OptionConverter.ConvertTypeTo(target, typeConstraint);
            }
            return target;
        }

        protected IAppender FindAppenderByReference(XmlElement appenderRef)
        {
            string attribute = appenderRef.GetAttribute("ref");
            IAppender appender = (IAppender) this.m_appenderBag[attribute];
            if (appender == null)
            {
                XmlElement appenderElement = null;
                if ((attribute != null) && (attribute.Length > 0))
                {
                    foreach (XmlElement element2 in appenderRef.OwnerDocument.GetElementsByTagName("appender"))
                    {
                        if (element2.GetAttribute("name") == attribute)
                        {
                            appenderElement = element2;
                            break;
                        }
                    }
                }
                if (appenderElement == null)
                {
                    LogLog.Error(declaringType, "XmlHierarchyConfigurator: No appender named [" + attribute + "] could be found.");
                    return null;
                }
                appender = this.ParseAppender(appenderElement);
                if (appender != null)
                {
                    this.m_appenderBag[attribute] = appender;
                }
            }
            return appender;
        }

        private MethodInfo FindMethodInfo(Type targetType, string name)
        {
            string strB = name;
            string str2 = "Add" + name;
            MethodInfo[] methods = targetType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo info in methods)
            {
                if ((!info.IsStatic && ((string.Compare(info.Name, strB, true, CultureInfo.InvariantCulture) == 0) || (string.Compare(info.Name, str2, true, CultureInfo.InvariantCulture) == 0))) && (info.GetParameters().Length == 1))
                {
                    return info;
                }
            }
            return null;
        }

        private bool HasAttributesOrElements(XmlElement element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if ((node.NodeType == XmlNodeType.Attribute) || (node.NodeType == XmlNodeType.Element))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsTypeConstructible(Type type)
        {
            if (type.IsClass && !type.IsAbstract)
            {
                ConstructorInfo constructor = type.GetConstructor(new Type[0]);
                if (!(((constructor == null) || constructor.IsAbstract) || constructor.IsPrivate))
                {
                    return true;
                }
            }
            return false;
        }

        protected IAppender ParseAppender(XmlElement appenderElement)
        {
            string attribute = appenderElement.GetAttribute("name");
            string typeName = appenderElement.GetAttribute("type");
            LogLog.Debug(declaringType, "Loading Appender [" + attribute + "] type: [" + typeName + "]");
            try
            {
                IAppender target = (IAppender) Activator.CreateInstance(SystemInfo.GetTypeFromString(typeName, true, true));
                target.Name = attribute;
                foreach (XmlNode node in appenderElement.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement appenderRef = (XmlElement) node;
                        if (appenderRef.LocalName == "appender-ref")
                        {
                            string str3 = appenderRef.GetAttribute("ref");
                            IAppenderAttachable attachable = target as IAppenderAttachable;
                            if (attachable != null)
                            {
                                LogLog.Debug(declaringType, "Attaching appender named [" + str3 + "] to appender named [" + target.Name + "].");
                                IAppender appender = this.FindAppenderByReference(appenderRef);
                                if (appender != null)
                                {
                                    attachable.AddAppender(appender);
                                }
                            }
                            else
                            {
                                LogLog.Error(declaringType, "Requesting attachment of appender named [" + str3 + "] to appender named [" + target.Name + "] which does not implement log4net.Core.IAppenderAttachable.");
                            }
                        }
                        else
                        {
                            this.SetParameter(appenderRef, target);
                        }
                    }
                }
                IOptionHandler handler = target as IOptionHandler;
                if (handler != null)
                {
                    handler.ActivateOptions();
                }
                LogLog.Debug(declaringType, "reated Appender [" + attribute + "]");
                return target;
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "Could not create Appender [" + attribute + "] of type [" + typeName + "]. Reported error follows.", exception);
                return null;
            }
        }

        protected void ParseChildrenOfLoggerElement(XmlElement catElement, Logger log, bool isRoot)
        {
            log.RemoveAllAppenders();
            foreach (XmlNode node in catElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    XmlElement appenderRef = (XmlElement) node;
                    if (appenderRef.LocalName == "appender-ref")
                    {
                        IAppender newAppender = this.FindAppenderByReference(appenderRef);
                        string attribute = appenderRef.GetAttribute("ref");
                        if (newAppender != null)
                        {
                            LogLog.Debug(declaringType, "Adding appender named [" + attribute + "] to logger [" + log.Name + "].");
                            log.AddAppender(newAppender);
                        }
                        else
                        {
                            LogLog.Error(declaringType, "Appender named [" + attribute + "] not found.");
                        }
                    }
                    else if ((appenderRef.LocalName == "level") || (appenderRef.LocalName == "priority"))
                    {
                        this.ParseLevel(appenderRef, log, isRoot);
                    }
                    else
                    {
                        this.SetParameter(appenderRef, log);
                    }
                }
            }
            IOptionHandler handler = log as IOptionHandler;
            if (handler != null)
            {
                handler.ActivateOptions();
            }
        }

        protected void ParseLevel(XmlElement element, Logger log, bool isRoot)
        {
            string name = log.Name;
            if (isRoot)
            {
                name = "root";
            }
            string attribute = element.GetAttribute("value");
            LogLog.Debug(declaringType, "Logger [" + name + "] Level string is [" + attribute + "].");
            if ("inherited" == attribute)
            {
                if (isRoot)
                {
                    LogLog.Error(declaringType, "Root level cannot be inherited. Ignoring directive.");
                }
                else
                {
                    LogLog.Debug(declaringType, "Logger [" + name + "] level set to inherit from parent.");
                    log.Level = null;
                }
            }
            else
            {
                log.Level = log.Hierarchy.LevelMap[attribute];
                if (log.Level == null)
                {
                    LogLog.Error(declaringType, "Undefined level [" + attribute + "] on Logger [" + name + "].");
                }
                else
                {
                    LogLog.Debug(declaringType, string.Concat(new object[] { "Logger [", name, "] level set to [name=\"", log.Level.Name, "\",value=", log.Level.Value, "]." }));
                }
            }
        }

        protected void ParseLogger(XmlElement loggerElement)
        {
            string attribute = loggerElement.GetAttribute("name");
            LogLog.Debug(declaringType, "Retrieving an instance of log4net.Repository.Logger for logger [" + attribute + "].");
            Logger log = this.m_hierarchy.GetLogger(attribute) as Logger;
            lock (log)
            {
                bool flag = OptionConverter.ToBoolean(loggerElement.GetAttribute("additivity"), true);
                LogLog.Debug(declaringType, string.Concat(new object[] { "Setting [", log.Name, "] additivity to [", flag, "]." }));
                log.Additivity = flag;
                this.ParseChildrenOfLoggerElement(loggerElement, log, false);
            }
        }

        protected void ParseRenderer(XmlElement element)
        {
            string attribute = element.GetAttribute("renderingClass");
            string typeName = element.GetAttribute("renderedClass");
            LogLog.Debug(declaringType, "Rendering class [" + attribute + "], Rendered class [" + typeName + "].");
            IObjectRenderer renderer = (IObjectRenderer) OptionConverter.InstantiateByClassName(attribute, typeof(IObjectRenderer), null);
            if (renderer == null)
            {
                LogLog.Error(declaringType, "Could not instantiate renderer [" + attribute + "].");
            }
            else
            {
                try
                {
                    this.m_hierarchy.RendererMap.Put(SystemInfo.GetTypeFromString(typeName, true, true), renderer);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Could not find class [" + typeName + "].", exception);
                }
            }
        }

        protected void ParseRoot(XmlElement rootElement)
        {
            Logger root = this.m_hierarchy.Root;
            lock (root)
            {
                this.ParseChildrenOfLoggerElement(rootElement, root, true);
            }
        }

        protected void SetParameter(XmlElement element, object target)
        {
            string attribute = element.GetAttribute("name");
            if (((element.LocalName != "param") || (attribute == null)) || (attribute.Length == 0))
            {
                attribute = element.LocalName;
            }
            Type targetType = target.GetType();
            Type propertyType = null;
            PropertyInfo property = null;
            MethodInfo info2 = null;
            property = targetType.GetProperty(attribute, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if ((property != null) && property.CanWrite)
            {
                propertyType = property.PropertyType;
            }
            else
            {
                property = null;
                info2 = this.FindMethodInfo(targetType, attribute);
                if (info2 != null)
                {
                    propertyType = info2.GetParameters()[0].ParameterType;
                }
            }
            if (propertyType == null)
            {
                LogLog.Error(declaringType, "XmlHierarchyConfigurator: Cannot find Property [" + attribute + "] to set object on [" + target.ToString() + "]");
            }
            else
            {
                TargetInvocationException exception2;
                string innerText = null;
                if (element.GetAttributeNode("value") != null)
                {
                    innerText = element.GetAttribute("value");
                }
                else if (element.HasChildNodes)
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {
                        if ((node.NodeType == XmlNodeType.CDATA) || (node.NodeType == XmlNodeType.Text))
                        {
                            if (innerText == null)
                            {
                                innerText = node.InnerText;
                            }
                            else
                            {
                                innerText = innerText + node.InnerText;
                            }
                        }
                    }
                }
                if (innerText != null)
                {
                    try
                    {
                        innerText = OptionConverter.SubstituteVariables(innerText, Environment.GetEnvironmentVariables());
                    }
                    catch (SecurityException)
                    {
                        LogLog.Debug(declaringType, "Security exception while trying to expand environment variables. Error Ignored. No Expansion.");
                    }
                    Type type3 = null;
                    string typeName = element.GetAttribute("type");
                    if ((typeName != null) && (typeName.Length > 0))
                    {
                        try
                        {
                            Type c = SystemInfo.GetTypeFromString(typeName, true, true);
                            LogLog.Debug(declaringType, "Parameter [" + attribute + "] specified subtype [" + c.FullName + "]");
                            if (!propertyType.IsAssignableFrom(c))
                            {
                                if (OptionConverter.CanConvertTypeTo(c, propertyType))
                                {
                                    type3 = propertyType;
                                    propertyType = c;
                                }
                                else
                                {
                                    LogLog.Error(declaringType, "subtype [" + c.FullName + "] set on [" + attribute + "] is not a subclass of property type [" + propertyType.FullName + "] and there are no acceptable type conversions.");
                                }
                            }
                            else
                            {
                                propertyType = c;
                            }
                        }
                        catch (Exception exception)
                        {
                            LogLog.Error(declaringType, "Failed to find type [" + typeName + "] set on [" + attribute + "]", exception);
                        }
                    }
                    object sourceInstance = this.ConvertStringTo(propertyType, innerText);
                    if ((sourceInstance != null) && (type3 != null))
                    {
                        LogLog.Debug(declaringType, "Performing additional conversion of value from [" + sourceInstance.GetType().Name + "] to [" + type3.Name + "]");
                        sourceInstance = OptionConverter.ConvertTypeTo(sourceInstance, type3);
                    }
                    if (sourceInstance != null)
                    {
                        if (property != null)
                        {
                            LogLog.Debug(declaringType, "Setting Property [" + property.Name + "] to " + sourceInstance.GetType().Name + " value [" + sourceInstance.ToString() + "]");
                            try
                            {
                                property.SetValue(target, sourceInstance, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
                            }
                            catch (TargetInvocationException exception4)
                            {
                                exception2 = exception4;
                                LogLog.Error(declaringType, string.Concat(new object[] { "Failed to set parameter [", property.Name, "] on object [", target, "] using value [", sourceInstance, "]" }), exception2.InnerException);
                            }
                        }
                        else if (info2 != null)
                        {
                            LogLog.Debug(declaringType, "Setting Collection Property [" + info2.Name + "] to " + sourceInstance.GetType().Name + " value [" + sourceInstance.ToString() + "]");
                            try
                            {
                                info2.Invoke(target, BindingFlags.InvokeMethod, null, new object[] { sourceInstance }, CultureInfo.InvariantCulture);
                            }
                            catch (TargetInvocationException exception5)
                            {
                                exception2 = exception5;
                                LogLog.Error(declaringType, string.Concat(new object[] { "Failed to set parameter [", attribute, "] on object [", target, "] using value [", sourceInstance, "]" }), exception2.InnerException);
                            }
                        }
                    }
                    else
                    {
                        LogLog.Warn(declaringType, string.Concat(new object[] { "Unable to set property [", attribute, "] on object [", target, "] using value [", innerText, "] (with acceptable conversion types)" }));
                    }
                }
                else
                {
                    object obj3 = null;
                    if (!(!(propertyType == typeof(string)) || this.HasAttributesOrElements(element)))
                    {
                        obj3 = "";
                    }
                    else
                    {
                        Type defaultTargetType = null;
                        if (IsTypeConstructible(propertyType))
                        {
                            defaultTargetType = propertyType;
                        }
                        obj3 = this.CreateObjectFromXml(element, defaultTargetType, propertyType);
                    }
                    if (obj3 == null)
                    {
                        LogLog.Error(declaringType, "Failed to create object to set param: " + attribute);
                    }
                    else if (property != null)
                    {
                        LogLog.Debug(declaringType, string.Concat(new object[] { "Setting Property [", property.Name, "] to object [", obj3, "]" }));
                        try
                        {
                            property.SetValue(target, obj3, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
                        }
                        catch (TargetInvocationException exception6)
                        {
                            exception2 = exception6;
                            LogLog.Error(declaringType, string.Concat(new object[] { "Failed to set parameter [", property.Name, "] on object [", target, "] using value [", obj3, "]" }), exception2.InnerException);
                        }
                    }
                    else if (info2 != null)
                    {
                        LogLog.Debug(declaringType, string.Concat(new object[] { "Setting Collection Property [", info2.Name, "] to object [", obj3, "]" }));
                        try
                        {
                            info2.Invoke(target, BindingFlags.InvokeMethod, null, new object[] { obj3 }, CultureInfo.InvariantCulture);
                        }
                        catch (TargetInvocationException exception7)
                        {
                            exception2 = exception7;
                            LogLog.Error(declaringType, string.Concat(new object[] { "Failed to set parameter [", info2.Name, "] on object [", target, "] using value [", obj3, "]" }), exception2.InnerException);
                        }
                    }
                }
            }
        }

        private enum ConfigUpdateMode
        {
            Merge,
            Overwrite
        }
    }
}

