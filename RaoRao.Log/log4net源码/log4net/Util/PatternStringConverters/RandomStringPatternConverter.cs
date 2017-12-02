namespace log4net.Util.PatternStringConverters
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.IO;

    internal sealed class RandomStringPatternConverter : PatternConverter, IOptionHandler
    {
        private static readonly Type declaringType = typeof(RandomStringPatternConverter);
        private int m_length = 4;
        private static readonly Random s_random = new Random();

        public void ActivateOptions()
        {
            string option = this.Option;
            if ((option != null) && (option.Length > 0))
            {
                int num;
                if (SystemInfo.TryParse(option, out num))
                {
                    this.m_length = num;
                }
                else
                {
                    LogLog.Error(declaringType, "RandomStringPatternConverter: Could not convert Option [" + option + "] to Length Int32");
                }
            }
        }

        protected override void Convert(TextWriter writer, object state)
        {
            try
            {
                lock (s_random)
                {
                    for (int i = 0; i < this.m_length; i++)
                    {
                        char ch;
                        int num2 = s_random.Next(0x24);
                        if (num2 < 0x1a)
                        {
                            ch = (char) (0x41 + num2);
                            writer.Write(ch);
                        }
                        else if (num2 < 0x24)
                        {
                            ch = (char) (0x30 + (num2 - 0x1a));
                            writer.Write(ch);
                        }
                        else
                        {
                            writer.Write('X');
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogLog.Error(declaringType, "Error occurred while converting.", exception);
            }
        }
    }
}

