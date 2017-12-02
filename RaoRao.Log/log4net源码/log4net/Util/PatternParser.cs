namespace log4net.Util
{
    using log4net.Core;
    using System;
    using System.Collections;
    using System.Globalization;

    public sealed class PatternParser
    {
        private static readonly Type declaringType = typeof(PatternParser);
        private const char ESCAPE_CHAR = '%';
        private PatternConverter m_head;
        private string m_pattern;
        private Hashtable m_patternConverters = new Hashtable();
        private PatternConverter m_tail;

        public PatternParser(string pattern)
        {
            this.m_pattern = pattern;
        }

        private void AddConverter(PatternConverter pc)
        {
            if (this.m_head == null)
            {
                this.m_head = this.m_tail = pc;
            }
            else
            {
                this.m_tail = this.m_tail.SetNext(pc);
            }
        }

        private string[] BuildCache()
        {
            string[] array = new string[this.m_patternConverters.Keys.Count];
            this.m_patternConverters.Keys.CopyTo(array, 0);
            Array.Sort(array, 0, array.Length, StringLengthComparer.Instance);
            return array;
        }

        public PatternConverter Parse()
        {
            string[] matches = this.BuildCache();
            this.ParseInternal(this.m_pattern, matches);
            return this.m_head;
        }

        private void ParseInternal(string pattern, string[] matches)
        {
            int startIndex = 0;
            while (startIndex < pattern.Length)
            {
                int index = pattern.IndexOf('%', startIndex);
                if ((index < 0) || (index == (pattern.Length - 1)))
                {
                    this.ProcessLiteral(pattern.Substring(startIndex));
                    startIndex = pattern.Length;
                }
                else if (pattern[index + 1] == '%')
                {
                    this.ProcessLiteral(pattern.Substring(startIndex, (index - startIndex) + 1));
                    startIndex = index + 2;
                }
                else
                {
                    char ch;
                    this.ProcessLiteral(pattern.Substring(startIndex, index - startIndex));
                    startIndex = index + 1;
                    FormattingInfo formattingInfo = new FormattingInfo();
                    if ((startIndex < pattern.Length) && (pattern[startIndex] == '-'))
                    {
                        formattingInfo.LeftAlign = true;
                        startIndex++;
                    }
                    while ((startIndex < pattern.Length) && char.IsDigit(pattern[startIndex]))
                    {
                        if (formattingInfo.Min < 0)
                        {
                            formattingInfo.Min = 0;
                        }
                        ch = pattern[startIndex];
                        formattingInfo.Min = (formattingInfo.Min * 10) + int.Parse(ch.ToString(CultureInfo.InvariantCulture), NumberFormatInfo.InvariantInfo);
                        startIndex++;
                    }
                    if ((startIndex < pattern.Length) && (pattern[startIndex] == '.'))
                    {
                        startIndex++;
                    }
                    while ((startIndex < pattern.Length) && char.IsDigit(pattern[startIndex]))
                    {
                        if (formattingInfo.Max == 0x7fffffff)
                        {
                            formattingInfo.Max = 0;
                        }
                        ch = pattern[startIndex];
                        formattingInfo.Max = (formattingInfo.Max * 10) + int.Parse(ch.ToString(CultureInfo.InvariantCulture), NumberFormatInfo.InvariantInfo);
                        startIndex++;
                    }
                    int num3 = pattern.Length - startIndex;
                    for (int i = 0; i < matches.Length; i++)
                    {
                        if ((matches[i].Length <= num3) && (string.Compare(pattern, startIndex, matches[i], 0, matches[i].Length, false, CultureInfo.InvariantCulture) == 0))
                        {
                            startIndex += matches[i].Length;
                            string option = null;
                            if ((startIndex < pattern.Length) && (pattern[startIndex] == '{'))
                            {
                                startIndex++;
                                int num5 = pattern.IndexOf('}', startIndex);
                                if (num5 >= 0)
                                {
                                    option = pattern.Substring(startIndex, num5 - startIndex);
                                    startIndex = num5 + 1;
                                }
                            }
                            this.ProcessConverter(matches[i], option, formattingInfo);
                            break;
                        }
                    }
                }
            }
        }

        private void ProcessConverter(string converterName, string option, FormattingInfo formattingInfo)
        {
            LogLog.Debug(declaringType, string.Concat(new object[] { "Converter [", converterName, "] Option [", option, "] Format [min=", formattingInfo.Min, ",max=", formattingInfo.Max, ",leftAlign=", formattingInfo.LeftAlign, "]" }));
            ConverterInfo info = (ConverterInfo) this.m_patternConverters[converterName];
            if (info == null)
            {
                LogLog.Error(declaringType, "Unknown converter name [" + converterName + "] in conversion pattern.");
            }
            else
            {
                PatternConverter pc = null;
                try
                {
                    pc = (PatternConverter) Activator.CreateInstance(info.Type);
                }
                catch (Exception exception)
                {
                    LogLog.Error(declaringType, "Failed to create instance of Type [" + info.Type.FullName + "] using default constructor. Exception: " + exception.ToString());
                }
                pc.FormattingInfo = formattingInfo;
                pc.Option = option;
                pc.Properties = info.Properties;
                IOptionHandler handler = pc as IOptionHandler;
                if (handler != null)
                {
                    handler.ActivateOptions();
                }
                this.AddConverter(pc);
            }
        }

        private void ProcessLiteral(string text)
        {
            if (text.Length > 0)
            {
                this.ProcessConverter("literal", text, new FormattingInfo());
            }
        }

        public Hashtable PatternConverters =>
            this.m_patternConverters;

        private sealed class StringLengthComparer : IComparer
        {
            public static readonly PatternParser.StringLengthComparer Instance = new PatternParser.StringLengthComparer();

            private StringLengthComparer()
            {
            }

            public int Compare(object x, object y)
            {
                string str = x as string;
                string str2 = y as string;
                if ((str == null) && (str2 == null))
                {
                    return 0;
                }
                if (str == null)
                {
                    return 1;
                }
                return str2?.Length.CompareTo(str.Length);
            }
        }
    }
}

