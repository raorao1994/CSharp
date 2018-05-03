using Exceptionless;
using Exceptionless.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robin.Exceptionless
{
    /// <summary>
    /// 安全日志
    /// </summary>
    public static class SecurityLog
    {
        private static ExceptionlessClient client;
        /// <summary>
        /// 访问日志
        /// </summary>
        static SecurityLog()
        {
            string ApiKey = ConfigurationManager.AppSettings["EL_SecurityLog"];
            string ServerUrl = ConfigurationManager.AppSettings["EL_ServerUrl"];
            client = new ExceptionlessClient(c => {
                c.ApiKey = ApiKey;
                c.ServerUrl = ServerUrl;
            });
            //client.Startup(ApiKey);
        }
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="Exception">异常信息</param>
        public static void Exception(Exception ex)
        {
            client.SubmitException(ex);
        }
        /// <summary>
        /// Log日志
        /// </summary>
        /// <param name="msg">错误信息</param>
        /// <param name="LogLevel">信息级别</param>
        public static void Log(string msg, LogLevel leve)
        {
            client.CreateLog(msg, leve).Submit();
        }
        /// <summary>
        /// FeatureUsage日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void FeatureUsage(string msg)
        {
            client.CreateFeatureUsage(msg).Submit();
        }
        /// <summary>
        /// BrokenLinks日志
        /// </summary>
        /// <param name="feature">日志信息</param>
        public static void BrokenLinks(string msg)
        {
            client.CreateNotFound(msg).Submit();
        }
    }
}
