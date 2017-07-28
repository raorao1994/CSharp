using System;
using System.Collections.Generic;
using System.Text;

namespace RaoRao.Socket.UDPHelper
{
    public class UDPHelper
    {
        /// <summary>
        /// 打包握手信息
        /// </summary>
        /// <param name="secKeyAccept"></param>
        /// <returns></returns>
        public static byte[] PackHandShakeData(string secKeyAccept)
        {
            return Encoding.UTF8.GetBytes(secKeyAccept);
        }
        /// <summary>
        /// 解析客户端数据包
        /// </summary>
        /// <param name="recBytes">服务器接收的数据包</param>
        /// <param name="recByteLength">有效数据长度</param>
        /// <returns></returns>
        public static string AnalyzeClientData(byte[] recBytes)
        {
            return Encoding.UTF8.GetString(recBytes);
        }
    }
}
