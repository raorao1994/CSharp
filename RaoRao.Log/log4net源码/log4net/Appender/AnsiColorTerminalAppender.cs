namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Globalization;
    using System.Text;

    public class AnsiColorTerminalAppender : AppenderSkeleton
    {
        public const string ConsoleError = "Console.Error";
        public const string ConsoleOut = "Console.Out";
        private LevelMapping m_levelMapping = new LevelMapping();
        private bool m_writeToErrorStream = false;
        private const string PostEventCodes = "\x001b[0m";

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            this.m_levelMapping.ActivateOptions();
        }

        public void AddMapping(LevelColors mapping)
        {
            this.m_levelMapping.Add(mapping);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            string str = base.RenderLoggingEvent(loggingEvent);
            LevelColors colors = this.m_levelMapping.Lookup(loggingEvent.Level) as LevelColors;
            if (colors != null)
            {
                str = colors.CombinedColor + str;
            }
            if (str.Length > 1)
            {
                if (str.EndsWith("\r\n") || str.EndsWith("\n\r"))
                {
                    str = str.Insert(str.Length - 2, "\x001b[0m");
                }
                else if (str.EndsWith("\n") || str.EndsWith("\r"))
                {
                    str = str.Insert(str.Length - 1, "\x001b[0m");
                }
                else
                {
                    str = str + "\x001b[0m";
                }
            }
            else if ((str[0] == '\n') || (str[0] == '\r'))
            {
                str = "\x001b[0m" + str;
            }
            else
            {
                str = str + "\x001b[0m";
            }
            if (this.m_writeToErrorStream)
            {
                Console.Error.Write(str);
            }
            else
            {
                Console.Write(str);
            }
        }

        protected override bool RequiresLayout =>
            true;

        public virtual string Target
        {
            get => 
                (this.m_writeToErrorStream ? "Console.Error" : "Console.Out");
            set
            {
                string strB = value.Trim();
                if (string.Compare("Console.Error", strB, true, CultureInfo.InvariantCulture) == 0)
                {
                    this.m_writeToErrorStream = true;
                }
                else
                {
                    this.m_writeToErrorStream = false;
                }
            }
        }

        [Flags]
        public enum AnsiAttributes
        {
            Blink = 8,
            Bright = 1,
            Dim = 2,
            Hidden = 0x20,
            Reverse = 0x10,
            Strikethrough = 0x40,
            Underscore = 4
        }

        public enum AnsiColor
        {
            Black,
            Red,
            Green,
            Yellow,
            Blue,
            Magenta,
            Cyan,
            White
        }

        public class LevelColors : LevelMappingEntry
        {
            private AnsiColorTerminalAppender.AnsiAttributes m_attributes;
            private AnsiColorTerminalAppender.AnsiColor m_backColor;
            private string m_combinedColor = "";
            private AnsiColorTerminalAppender.AnsiColor m_foreColor;

            public override void ActivateOptions()
            {
                base.ActivateOptions();
                StringBuilder builder = new StringBuilder();
                builder.Append("\x001b[0;");
                builder.Append((int) (30 + this.m_foreColor));
                builder.Append(';');
                builder.Append((int) (40 + this.m_backColor));
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Bright) > 0)
                {
                    builder.Append(";1");
                }
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Dim) > 0)
                {
                    builder.Append(";2");
                }
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Underscore) > 0)
                {
                    builder.Append(";4");
                }
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Blink) > 0)
                {
                    builder.Append(";5");
                }
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Reverse) > 0)
                {
                    builder.Append(";7");
                }
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Hidden) > 0)
                {
                    builder.Append(";8");
                }
                if ((this.m_attributes & AnsiColorTerminalAppender.AnsiAttributes.Strikethrough) > 0)
                {
                    builder.Append(";9");
                }
                builder.Append('m');
                this.m_combinedColor = builder.ToString();
            }

            public AnsiColorTerminalAppender.AnsiAttributes Attributes
            {
                get => 
                    this.m_attributes;
                set
                {
                    this.m_attributes = value;
                }
            }

            public AnsiColorTerminalAppender.AnsiColor BackColor
            {
                get => 
                    this.m_backColor;
                set
                {
                    this.m_backColor = value;
                }
            }

            internal string CombinedColor =>
                this.m_combinedColor;

            public AnsiColorTerminalAppender.AnsiColor ForeColor
            {
                get => 
                    this.m_foreColor;
                set
                {
                    this.m_foreColor = value;
                }
            }
        }
    }
}

