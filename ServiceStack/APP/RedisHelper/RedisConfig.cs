using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace RaoRao.RedisHelper
{
    public sealed class RedisConfig
    {
        /// <summary>
        /// 写入池Redis服务器连接符
        /// 以逗号分割(IP:Port,IP:Port)
        /// </summary>
        public static string WriteServerConStr
        {
            get
            {
                string val = ConfigurationManager.AppSettings["WriteServerConStr"];
                return val;
            }
        }
        /// <summary>
        /// 读取池Redis服务器连接符
        /// 以逗号分割(IP:Port,IP:Port)
        /// </summary>
        public static string ReadServerConStr
        {
            get
            {
                string val = ConfigurationManager.AppSettings["ReadServerConStr"];
                return val;
            }
        }
        /// <summary>
        /// 最大写入池数量
        /// </summary>
        public static int MaxWritePoolSize
        {
            get
            {
                return 50;
            }
        }
        /// <summary>
        /// 最大读取池数量
        /// </summary>
        public static int MaxReadPoolSize
        {
            get
            {
                return 200;
            }
        }
        /// <summary>
        /// 自动开启
        /// </summary>
        public static bool AutoStart
        {
            get
            {
                return true;
            }
        }

    }
}
