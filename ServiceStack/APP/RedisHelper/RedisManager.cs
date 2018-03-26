using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace RaoRao.RedisHelper
{
    /// <summary>
    /// Redis操作类
    /// </summary>
    public class RedisManager
    {
        private static PooledRedisClientManager prcm;

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            string[] WriteServerConStr = SplitString(RedisConfig.WriteServerConStr, ",");
            string[] ReadServerConStr = SplitString(RedisConfig.ReadServerConStr, ",");
            prcm = new PooledRedisClientManager(ReadServerConStr, WriteServerConStr,
                        new RedisClientManagerConfig
                        {
                            MaxWritePoolSize = RedisConfig.MaxWritePoolSize,
                            MaxReadPoolSize = RedisConfig.MaxReadPoolSize,
                            AutoStart = RedisConfig.AutoStart,
                        });
        }
        /// <summary>
        /// 字符串分割
        /// </summary>
        /// <param name="strSource">字符串</param>
        /// <param name="split">字符串分隔符</param>
        /// <returns></returns>
        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }
        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient()
        {
            if (prcm == null)
                CreateManager();
            return prcm.GetClient();
        }
    }
}
