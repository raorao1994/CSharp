namespace log4net.Util
{
    using log4net.Core;
    using System;

    public class OnlyOnceErrorHandler : IErrorHandler
    {
        private static readonly Type declaringType = typeof(OnlyOnceErrorHandler);
        private DateTime m_enabledDate;
        private log4net.Core.ErrorCode m_errorCode;
        private System.Exception m_exception;
        private bool m_firstTime;
        private string m_message;
        private readonly string m_prefix;

        public OnlyOnceErrorHandler()
        {
            this.m_firstTime = true;
            this.m_message = null;
            this.m_exception = null;
            this.m_errorCode = log4net.Core.ErrorCode.GenericFailure;
            this.m_prefix = "";
        }

        public OnlyOnceErrorHandler(string prefix)
        {
            this.m_firstTime = true;
            this.m_message = null;
            this.m_exception = null;
            this.m_errorCode = log4net.Core.ErrorCode.GenericFailure;
            this.m_prefix = prefix;
        }

        public void Error(string message)
        {
            this.Error(message, null, log4net.Core.ErrorCode.GenericFailure);
        }

        public void Error(string message, System.Exception e)
        {
            this.Error(message, e, log4net.Core.ErrorCode.GenericFailure);
        }

        public void Error(string message, System.Exception e, log4net.Core.ErrorCode errorCode)
        {
            if (this.m_firstTime)
            {
                this.m_enabledDate = DateTime.Now;
                this.m_errorCode = errorCode;
                this.m_exception = e;
                this.m_message = message;
                this.m_firstTime = false;
                if (!(!LogLog.InternalDebugging || LogLog.QuietMode))
                {
                    LogLog.Error(declaringType, "[" + this.m_prefix + "] ErrorCode: " + errorCode.ToString() + ". " + message, e);
                }
            }
        }

        public void Reset()
        {
            this.m_enabledDate = DateTime.MinValue;
            this.m_errorCode = log4net.Core.ErrorCode.GenericFailure;
            this.m_exception = null;
            this.m_message = null;
            this.m_firstTime = true;
        }

        public DateTime EnabledDate =>
            this.m_enabledDate;

        public log4net.Core.ErrorCode ErrorCode =>
            this.m_errorCode;

        public string ErrorMessage =>
            this.m_message;

        public System.Exception Exception =>
            this.m_exception;

        public bool IsEnabled =>
            this.m_firstTime;
    }
}

