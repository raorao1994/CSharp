namespace log4net.Layout
{
    using log4net.Core;
    using log4net.Layout.Pattern;
    using log4net.Util;
    using log4net.Util.PatternStringConverters;
    using System;
    using System.Collections;
    using System.IO;

    public class PatternLayout : LayoutSkeleton
    {
        public const string DefaultConversionPattern = "%message%newline";
        public const string DetailConversionPattern = "%timestamp [%thread] %level %logger %ndc - %message%newline";
        private PatternConverter m_head;
        private Hashtable m_instanceRulesRegistry;
        private string m_pattern;
        private static Hashtable s_globalRulesRegistry = new Hashtable(0x2d);

        static PatternLayout()
        {
            s_globalRulesRegistry.Add("literal", typeof(LiteralPatternConverter));
            s_globalRulesRegistry.Add("newline", typeof(NewLinePatternConverter));
            s_globalRulesRegistry.Add("n", typeof(NewLinePatternConverter));
            s_globalRulesRegistry.Add("aspnet-cache", typeof(AspNetCachePatternConverter));
            s_globalRulesRegistry.Add("aspnet-context", typeof(AspNetContextPatternConverter));
            s_globalRulesRegistry.Add("aspnet-request", typeof(AspNetRequestPatternConverter));
            s_globalRulesRegistry.Add("aspnet-session", typeof(AspNetSessionPatternConverter));
            s_globalRulesRegistry.Add("c", typeof(LoggerPatternConverter));
            s_globalRulesRegistry.Add("logger", typeof(LoggerPatternConverter));
            s_globalRulesRegistry.Add("C", typeof(TypeNamePatternConverter));
            s_globalRulesRegistry.Add("class", typeof(TypeNamePatternConverter));
            s_globalRulesRegistry.Add("type", typeof(TypeNamePatternConverter));
            s_globalRulesRegistry.Add("d", typeof(log4net.Layout.Pattern.DatePatternConverter));
            s_globalRulesRegistry.Add("date", typeof(log4net.Layout.Pattern.DatePatternConverter));
            s_globalRulesRegistry.Add("exception", typeof(ExceptionPatternConverter));
            s_globalRulesRegistry.Add("F", typeof(FileLocationPatternConverter));
            s_globalRulesRegistry.Add("file", typeof(FileLocationPatternConverter));
            s_globalRulesRegistry.Add("l", typeof(FullLocationPatternConverter));
            s_globalRulesRegistry.Add("location", typeof(FullLocationPatternConverter));
            s_globalRulesRegistry.Add("L", typeof(LineLocationPatternConverter));
            s_globalRulesRegistry.Add("line", typeof(LineLocationPatternConverter));
            s_globalRulesRegistry.Add("m", typeof(MessagePatternConverter));
            s_globalRulesRegistry.Add("message", typeof(MessagePatternConverter));
            s_globalRulesRegistry.Add("M", typeof(MethodLocationPatternConverter));
            s_globalRulesRegistry.Add("method", typeof(MethodLocationPatternConverter));
            s_globalRulesRegistry.Add("p", typeof(LevelPatternConverter));
            s_globalRulesRegistry.Add("level", typeof(LevelPatternConverter));
            s_globalRulesRegistry.Add("P", typeof(log4net.Layout.Pattern.PropertyPatternConverter));
            s_globalRulesRegistry.Add("property", typeof(log4net.Layout.Pattern.PropertyPatternConverter));
            s_globalRulesRegistry.Add("properties", typeof(log4net.Layout.Pattern.PropertyPatternConverter));
            s_globalRulesRegistry.Add("r", typeof(RelativeTimePatternConverter));
            s_globalRulesRegistry.Add("timestamp", typeof(RelativeTimePatternConverter));
            s_globalRulesRegistry.Add("stacktrace", typeof(StackTracePatternConverter));
            s_globalRulesRegistry.Add("stacktracedetail", typeof(StackTraceDetailPatternConverter));
            s_globalRulesRegistry.Add("t", typeof(ThreadPatternConverter));
            s_globalRulesRegistry.Add("thread", typeof(ThreadPatternConverter));
            s_globalRulesRegistry.Add("x", typeof(NdcPatternConverter));
            s_globalRulesRegistry.Add("ndc", typeof(NdcPatternConverter));
            s_globalRulesRegistry.Add("X", typeof(log4net.Layout.Pattern.PropertyPatternConverter));
            s_globalRulesRegistry.Add("mdc", typeof(log4net.Layout.Pattern.PropertyPatternConverter));
            s_globalRulesRegistry.Add("a", typeof(log4net.Layout.Pattern.AppDomainPatternConverter));
            s_globalRulesRegistry.Add("appdomain", typeof(log4net.Layout.Pattern.AppDomainPatternConverter));
            s_globalRulesRegistry.Add("u", typeof(log4net.Layout.Pattern.IdentityPatternConverter));
            s_globalRulesRegistry.Add("identity", typeof(log4net.Layout.Pattern.IdentityPatternConverter));
            s_globalRulesRegistry.Add("utcdate", typeof(log4net.Layout.Pattern.UtcDatePatternConverter));
            s_globalRulesRegistry.Add("utcDate", typeof(log4net.Layout.Pattern.UtcDatePatternConverter));
            s_globalRulesRegistry.Add("UtcDate", typeof(log4net.Layout.Pattern.UtcDatePatternConverter));
            s_globalRulesRegistry.Add("w", typeof(log4net.Layout.Pattern.UserNamePatternConverter));
            s_globalRulesRegistry.Add("username", typeof(log4net.Layout.Pattern.UserNamePatternConverter));
        }

        public PatternLayout() : this("%message%newline")
        {
        }

        public PatternLayout(string pattern)
        {
            this.m_instanceRulesRegistry = new Hashtable();
            this.IgnoresException = true;
            this.m_pattern = pattern;
            if (this.m_pattern == null)
            {
                this.m_pattern = "%message%newline";
            }
            this.ActivateOptions();
        }

        public override void ActivateOptions()
        {
            this.m_head = this.CreatePatternParser(this.m_pattern).Parse();
            for (PatternConverter converter = this.m_head; converter != null; converter = converter.Next)
            {
                PatternLayoutConverter converter2 = converter as PatternLayoutConverter;
                if ((converter2 != null) && !converter2.IgnoresException)
                {
                    this.IgnoresException = false;
                    break;
                }
            }
        }

        public void AddConverter(ConverterInfo converterInfo)
        {
            if (converterInfo == null)
            {
                throw new ArgumentNullException("converterInfo");
            }
            if (!typeof(PatternConverter).IsAssignableFrom(converterInfo.Type))
            {
                throw new ArgumentException("The converter type specified [" + converterInfo.Type + "] must be a subclass of log4net.Util.PatternConverter", "converterInfo");
            }
            this.m_instanceRulesRegistry[converterInfo.Name] = converterInfo;
        }

        public void AddConverter(string name, Type type)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            ConverterInfo converterInfo = new ConverterInfo {
                Name = name,
                Type = type
            };
            this.AddConverter(converterInfo);
        }

        protected virtual PatternParser CreatePatternParser(string pattern)
        {
            PatternParser parser = new PatternParser(pattern);
            foreach (DictionaryEntry entry in s_globalRulesRegistry)
            {
                ConverterInfo info = new ConverterInfo {
                    Name = (string) entry.Key,
                    Type = (Type) entry.Value
                };
                parser.PatternConverters[entry.Key] = info;
            }
            foreach (DictionaryEntry entry in this.m_instanceRulesRegistry)
            {
                parser.PatternConverters[entry.Key] = entry.Value;
            }
            return parser;
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            if (loggingEvent == null)
            {
                throw new ArgumentNullException("loggingEvent");
            }
            for (PatternConverter converter = this.m_head; converter != null; converter = converter.Next)
            {
                converter.Format(writer, loggingEvent);
            }
        }

        public string ConversionPattern
        {
            get => 
                this.m_pattern;
            set
            {
                this.m_pattern = value;
            }
        }
    }
}

