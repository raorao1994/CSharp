namespace log4net.Util
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Security;

    public sealed class NativeError
    {
        private string m_message;
        private int m_number;

        private NativeError(int number, string message)
        {
            this.m_number = number;
            this.m_message = message;
        }

        [DllImport("Kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern int FormatMessage(int dwFlags, ref IntPtr lpSource, int dwMessageId, int dwLanguageId, ref string lpBuffer, int nSize, IntPtr Arguments);
        public static NativeError GetError(int number) => 
            new NativeError(number, GetErrorMessage(number));

        [SecuritySafeCritical]
        public static string GetErrorMessage(int messageId)
        {
            int num = 0x100;
            int num2 = 0x200;
            int num3 = 0x1000;
            string lpBuffer = "";
            IntPtr lpSource = new IntPtr();
            IntPtr arguments = new IntPtr();
            if (messageId != 0)
            {
                if (FormatMessage((num | num3) | num2, ref lpSource, messageId, 0, ref lpBuffer, 0xff, arguments) > 0)
                {
                    return lpBuffer.TrimEnd(new char[] { '\r', '\n' });
                }
                return null;
            }
            return null;
        }

        [SecuritySafeCritical]
        public static NativeError GetLastError()
        {
            int number = Marshal.GetLastWin32Error();
            return new NativeError(number, GetErrorMessage(number));
        }

        public override string ToString() => 
            (string.Format(CultureInfo.InvariantCulture, "0x{0:x8}", new object[] { this.Number }) + ((this.Message != null) ? (": " + this.Message) : ""));

        public string Message =>
            this.m_message;

        public int Number =>
            this.m_number;
    }
}

