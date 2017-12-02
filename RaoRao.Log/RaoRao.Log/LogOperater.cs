using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RaoRao.Log
{
    public class LogOperater
    {
        private static readonly Dictionary<Type, ILog> _loggers = new Dictionary<Type, ILog>();

        static LogOperater()
        {
            //GetConfig();
            string[] configFile = ConfigurationSettings.AppSettings.GetValues("Log4");
            FileInfo file = new FileInfo(configFile[0]);
            log4net.Config.XmlConfigurator.Configure(file);
        }
        private static void GetConfig()
        {
            //获取调用当前正在执行的方法的方法的 Assembly  
            Assembly assembly = Assembly.GetCallingAssembly();
            string path = string.Format("{0}.config", assembly.Location);

            if (File.Exists(path) == false)
            {
                string msg = string.Format("{0}路径下的文件未找到 ", path);
            }

            try
            {
                //ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
                //configFile.ExeConfigFilename = path;
                //Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);

                //return config;
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 获取记录器
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static ILog GetLogger(Type source)
        {
            if (_loggers.ContainsKey(source))
            {
                return _loggers[source];
            }
            else
            {
                ILog logger = LogManager.GetLogger(source);
                _loggers.Add(source, logger);
                return logger;
            }
        }

        #region Debug
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Debug(object source, string message)
        {
            Debug(source.GetType(), message);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="ps"></param>
        public static void Debug(object source, string message, params object[] ps)
        {
            Debug(source.GetType(), string.Format(message, ps));
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Debug(Type source, string message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsDebugEnabled)
                logger.Debug(message);
        }
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Debug(object source, object message, Exception exception)
        {
            Debug(source.GetType(), message, exception);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Debug(Type source, object message, Exception exception)
        {
            GetLogger(source).Debug(message, exception);
        }
        #endregion

        #region Info
        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Info(object source, object message)
        {
            Info(source.GetType(), message);
        }

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Info(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsInfoEnabled)
                logger.Info(message);
        }
        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(object source, object message, Exception exception)
        {
            Info(source.GetType(), message, exception);
        }

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(Type source, object message, Exception exception)
        {
            GetLogger(source).Info(message, exception);
        }
        #endregion

        #region Warn
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Warn(object source, object message)
        {
            Warn(source.GetType(), message);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Warn(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsWarnEnabled)
                logger.Warn(message);
        }
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(object source, object message, Exception exception)
        {
            Warn(source.GetType(), message, exception);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(Type source, object message, Exception exception)
        {
            GetLogger(source).Warn(message, exception);
        }
        #endregion

        #region Erro
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Error(object source, object message)
        {
            Error(source.GetType(), message);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Error(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsErrorEnabled)
                logger.Error(message);
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(object source, object message, Exception exception)
        {
            Error(source.GetType(), message, exception);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(Type source, object message, Exception exception)
        {
            GetLogger(source).Error(message, exception);
        }
        #endregion

        #region Fatal
        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Fatal(object source, object message)
        {
            Fatal(source.GetType(), message);
        }

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Fatal(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsFatalEnabled)
                logger.Fatal(message);
        }
        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Fatal(object source, object message, Exception exception)
        {
            Fatal(source.GetType(), message, exception);
        }

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Fatal(Type source, object message, Exception exception)
        {
            GetLogger(source).Fatal(message, exception);
        } 
        #endregion
    }
}
