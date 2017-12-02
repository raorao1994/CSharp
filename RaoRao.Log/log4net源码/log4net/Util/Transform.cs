namespace log4net.Util
{
    using System;
    using System.Text.RegularExpressions;
    using System.Xml;

    public sealed class Transform
    {
        private const string CDATA_END = "]]>";
        private const string CDATA_UNESCAPABLE_TOKEN = "]]";
        private static Regex INVALIDCHARS = new Regex(@"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD]", RegexOptions.Compiled);

        private Transform()
        {
        }

        private static int CountSubstrings(string text, string substring)
        {
            int num = 0;
            int startIndex = 0;
            int length = text.Length;
            int num4 = substring.Length;
            if (length == 0)
            {
                return 0;
            }
            if (num4 == 0)
            {
                return 0;
            }
            while (startIndex < length)
            {
                int index = text.IndexOf(substring, startIndex);
                if (index == -1)
                {
                    return num;
                }
                num++;
                startIndex = index + num4;
            }
            return num;
        }

        public static string MaskXmlInvalidCharacters(string textData, string mask) => 
            INVALIDCHARS.Replace(textData, mask);

        public static void WriteEscapedXmlString(XmlWriter writer, string textData, string invalidCharReplacement)
        {
            string text = MaskXmlInvalidCharacters(textData, invalidCharReplacement);
            int num = 12 * (1 + CountSubstrings(text, "]]>"));
            int num2 = (3 * (CountSubstrings(text, "<") + CountSubstrings(text, ">"))) + (4 * CountSubstrings(text, "&"));
            if (num2 <= num)
            {
                writer.WriteString(text);
            }
            else
            {
                int index = text.IndexOf("]]>");
                if (index < 0)
                {
                    writer.WriteCData(text);
                }
                else
                {
                    int startIndex = 0;
                    while (index > -1)
                    {
                        writer.WriteCData(text.Substring(startIndex, index - startIndex));
                        if (index == (text.Length - 3))
                        {
                            startIndex = text.Length;
                            writer.WriteString("]]>");
                            break;
                        }
                        writer.WriteString("]]");
                        startIndex = index + 2;
                        index = text.IndexOf("]]>", startIndex);
                    }
                    if (startIndex < text.Length)
                    {
                        writer.WriteCData(text.Substring(startIndex));
                    }
                }
            }
        }
    }
}

