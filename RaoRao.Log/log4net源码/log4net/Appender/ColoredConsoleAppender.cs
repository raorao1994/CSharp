namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Layout;
    using log4net.Util;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;
    using System.Text;

    public class ColoredConsoleAppender : AppenderSkeleton
    {
        public const string ConsoleError = "Console.Error";
        public const string ConsoleOut = "Console.Out";
        private StreamWriter m_consoleOutputWriter;
        private LevelMapping m_levelMapping;
        private bool m_writeToErrorStream;
        private static readonly char[] s_windowsNewline = new char[] { '\r', '\n' };
        private const uint STD_ERROR_HANDLE = 0xfffffff4;
        private const uint STD_OUTPUT_HANDLE = 0xfffffff5;

        public ColoredConsoleAppender()
        {
            this.m_writeToErrorStream = false;
            this.m_levelMapping = new LevelMapping();
            this.m_consoleOutputWriter = null;
        }

        [Obsolete("Instead use the default constructor and set the Layout property")]
        public ColoredConsoleAppender(ILayout layout) : this(layout, false)
        {
        }

        [Obsolete("Instead use the default constructor and set the Layout & Target properties")]
        public ColoredConsoleAppender(ILayout layout, bool writeToErrorStream)
        {
            this.m_writeToErrorStream = false;
            this.m_levelMapping = new LevelMapping();
            this.m_consoleOutputWriter = null;
            this.Layout = layout;
            this.m_writeToErrorStream = writeToErrorStream;
        }

        [SecuritySafeCritical, SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        public override void ActivateOptions()
        {
            base.ActivateOptions();
            this.m_levelMapping.ActivateOptions();
            Stream stream = null;
            if (this.m_writeToErrorStream)
            {
                stream = Console.OpenStandardError();
            }
            else
            {
                stream = Console.OpenStandardOutput();
            }
            Encoding encoding = Encoding.GetEncoding(GetConsoleOutputCP());
            this.m_consoleOutputWriter = new StreamWriter(stream, encoding, 0x100);
            this.m_consoleOutputWriter.AutoFlush = true;
            GC.SuppressFinalize(this.m_consoleOutputWriter);
        }

        public void AddMapping(LevelColors mapping)
        {
            this.m_levelMapping.Add(mapping);
        }

        [SecuritySafeCritical, SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (this.m_consoleOutputWriter != null)
            {
                CONSOLE_SCREEN_BUFFER_INFO console_screen_buffer_info;
                IntPtr zero = IntPtr.Zero;
                if (this.m_writeToErrorStream)
                {
                    zero = GetStdHandle(0xfffffff4);
                }
                else
                {
                    zero = GetStdHandle(0xfffffff5);
                }
                ushort attributes = 7;
                LevelColors colors = this.m_levelMapping.Lookup(loggingEvent.Level) as LevelColors;
                if (colors != null)
                {
                    attributes = colors.CombinedColor;
                }
                string str = base.RenderLoggingEvent(loggingEvent);
                GetConsoleScreenBufferInfo(zero, out console_screen_buffer_info);
                SetConsoleTextAttribute(zero, attributes);
                char[] buffer = str.ToCharArray();
                int length = buffer.Length;
                bool flag = false;
                if (((length > 1) && (buffer[length - 2] == '\r')) && (buffer[length - 1] == '\n'))
                {
                    length -= 2;
                    flag = true;
                }
                this.m_consoleOutputWriter.Write(buffer, 0, length);
                SetConsoleTextAttribute(zero, console_screen_buffer_info.wAttributes);
                if (flag)
                {
                    this.m_consoleOutputWriter.Write(s_windowsNewline, 0, 2);
                }
            }
        }

        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern int GetConsoleOutputCP();
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern bool GetConsoleScreenBufferInfo(IntPtr consoleHandle, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern IntPtr GetStdHandle(uint type);
        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern bool SetConsoleTextAttribute(IntPtr consoleHandle, ushort attributes);

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
        public enum Colors
        {
            Blue = 1,
            Cyan = 3,
            Green = 2,
            HighIntensity = 8,
            Purple = 5,
            Red = 4,
            White = 7,
            Yellow = 6
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public ColoredConsoleAppender.COORD dwSize;
            public ColoredConsoleAppender.COORD dwCursorPosition;
            public ushort wAttributes;
            public ColoredConsoleAppender.SMALL_RECT srWindow;
            public ColoredConsoleAppender.COORD dwMaximumWindowSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            public ushort x;
            public ushort y;
        }

        public class LevelColors : LevelMappingEntry
        {
            private ColoredConsoleAppender.Colors m_backColor;
            private ushort m_combinedColor = 0;
            private ColoredConsoleAppender.Colors m_foreColor;

            public override void ActivateOptions()
            {
                base.ActivateOptions();
                this.m_combinedColor = (ushort) (this.m_foreColor + (((int) this.m_backColor) << 4));
            }

            public ColoredConsoleAppender.Colors BackColor
            {
                get => 
                    this.m_backColor;
                set
                {
                    this.m_backColor = value;
                }
            }

            internal ushort CombinedColor =>
                this.m_combinedColor;

            public ColoredConsoleAppender.Colors ForeColor
            {
                get => 
                    this.m_foreColor;
                set
                {
                    this.m_foreColor = value;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SMALL_RECT
        {
            public ushort Left;
            public ushort Top;
            public ushort Right;
            public ushort Bottom;
        }
    }
}

