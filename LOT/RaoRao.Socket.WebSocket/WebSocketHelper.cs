using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RaoRao.Socket.WebSocket
{
    public class WebSocketHelper
    {
        /// <summary>
        /// 打包握手信息
        /// </summary>
        /// <param name="secKeyAccept"></param>
        /// <returns></returns>
        public static byte[] PackHandShakeData(string secKeyAccept)
        {
            var responseBuilder = new StringBuilder();
            responseBuilder.Append("HTTP/1.1 101 Switching Protocols" + Environment.NewLine);
            responseBuilder.Append("Upgrade: websocket" + Environment.NewLine);
            responseBuilder.Append("Connection: Upgrade" + Environment.NewLine);
            responseBuilder.Append("Sec-WebSocket-Accept: " + secKeyAccept + Environment.NewLine + Environment.NewLine);
            //如果把上一行换成下面两行，才是thewebsocketprotocol-17协议，但居然握手不成功，目前仍没弄明白！
            //responseBuilder.Append("Sec-WebSocket-Accept: " + secKeyAccept + Environment.NewLine);
            //responseBuilder.Append("Sec-WebSocket-Protocol: chat" + Environment.NewLine);

            return Encoding.UTF8.GetBytes(responseBuilder.ToString());
        }

        /// <summary>
        /// 生成Sec-WebSocket-Accept
        /// </summary>
        /// <param name="handShakeText">客户端握手信息</param>
        /// <returns>Sec-WebSocket-Accept</returns>
        public static string GetSecKeyAccetp(byte[] handShakeBytes, int bytesLength)
        {
            string handShakeText = Encoding.UTF8.GetString(handShakeBytes, 0, bytesLength);
            string key = string.Empty;
            Regex r = new Regex(@"Sec\-WebSocket\-Key:(.*?)\r\n");
            Match m = r.Match(handShakeText);
            if (m.Groups.Count != 0)
            {
                key = Regex.Replace(m.Value, @"Sec\-WebSocket\-Key:(.*?)\r\n", "$1").Trim();
            }
            byte[] encryptionString = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(key + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"));
            return Convert.ToBase64String(encryptionString);
        }

        /// <summary>
        /// 解析客户端数据包
        /// </summary>
        /// <param name="recBytes">服务器接收的数据包</param>
        /// <param name="recByteLength">有效数据长度</param>
        /// <returns></returns>
        public static string AnalyticData(byte[] recBytes, int recByteLength)
        {
            if (recByteLength < 2) { return string.Empty; }

            bool fin = (recBytes[0] & 0x80) == 0x80; // 1bit，1表示最后一帧  
            if (!fin)
            {
                return string.Empty;// 超过一帧暂不处理 
            }

            bool mask_flag = (recBytes[1] & 0x80) == 0x80; // 是否包含掩码  
            if (!mask_flag)
            {
                return string.Empty;// 不包含掩码的暂不处理
            }

            int payload_len = recBytes[1] & 0x7F; // 数据长度  

            byte[] masks = new byte[4];
            byte[] payload_data;

            if (payload_len == 126)
            {
                Array.Copy(recBytes, 4, masks, 0, 4);
                payload_len = (UInt16)(recBytes[2] << 8 | recBytes[3]);
                payload_data = new byte[payload_len];
                Array.Copy(recBytes, 8, payload_data, 0, payload_len);

            }
            else if (payload_len == 127)
            {
                Array.Copy(recBytes, 10, masks, 0, 4);
                byte[] uInt64Bytes = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    uInt64Bytes[i] = recBytes[9 - i];
                }
                UInt64 len = BitConverter.ToUInt64(uInt64Bytes, 0);

                payload_data = new byte[len];
                for (UInt64 i = 0; i < len; i++)
                {
                    payload_data[i] = recBytes[i + 14];
                }
            }
            else
            {
                Array.Copy(recBytes, 2, masks, 0, 4);
                payload_data = new byte[payload_len];
                Array.Copy(recBytes, 6, payload_data, 0, payload_len);

            }

            for (var i = 0; i < payload_len; i++)
            {
                payload_data[i] = (byte)(payload_data[i] ^ masks[i % 4]);
            }

            return Encoding.UTF8.GetString(payload_data);
        }

        /// <summary>
        /// 打包服务器数据
        /// </summary>
        /// <param name="message">数据</param>
        /// <returns>数据包</returns>
        public static byte[] PackData(string message)
        {
            byte[] contentBytes = null;
            byte[] temp = Encoding.UTF8.GetBytes(message);

            if (temp.Length < 126)
            {
                contentBytes = new byte[temp.Length + 2];
                contentBytes[0] = 0x81;
                contentBytes[1] = (byte)temp.Length;
                Array.Copy(temp, 0, contentBytes, 2, temp.Length);
            }
            else if (temp.Length < 0xFFFF)
            {
                contentBytes = new byte[temp.Length + 4];
                contentBytes[0] = 0x81;
                contentBytes[1] = 126;
                contentBytes[2] = (byte)(temp.Length & 0xFF);
                contentBytes[3] = (byte)(temp.Length >> 8 & 0xFF);
                Array.Copy(temp, 0, contentBytes, 4, temp.Length);
            }
            else
            {
                // 暂不处理超长内容  
            }

            return contentBytes;
        }


        #region 客户端和服务端的响应
        /*
         * 客户端向服务器发送请求
         * 
         * GET / HTTP/1.1
         * Origin: http://localhost:1416
         * Sec-WebSocket-Key: vDyPp55hT1PphRU5OAe2Wg==
         * Connection: Upgrade
         * Upgrade: Websocket
         * Sec-WebSocket-Version: 13
         * User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
         * Host: localhost:8064
         * DNT: 1
         * Cache-Control: no-cache
         * Cookie: DTRememberName=admin
         * 
         * 服务器给出响应
         * 
         * HTTP/1.1 101 Switching Protocols
         * Upgrade: websocket
         * Connection: Upgrade
         * Sec-WebSocket-Accept: xsOSgr30aKL2GNZKNHKmeT1qYjA=
         * 
         * 在请求中的“Sec-WebSocket-Key”是随机的，服务器端会用这些数据来构造出一个SHA-1的信息摘要。把“Sec-WebSocket-Key”加上一个魔幻字符串
         * “258EAFA5-E914-47DA-95CA-C5AB0DC85B11”。使用 SHA-1 <a class="keylink" href="http://www.2cto.com/article/jiami/" target="_blank">
         * 加密</a>，之后进行 BASE-64编码，将结果做为 “Sec-WebSocket-Accept” 头的值，返回给客户端
         */
        #endregion

        #region 打包请求连接数据
        /// <summary>
        /// 打包请求连接数据
        /// </summary>
        /// <param name="handShakeBytes"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] PackageHandShakeData(byte[] handShakeBytes, int length)
        {
            string handShakeText = Encoding.UTF8.GetString(handShakeBytes, 0, length);
            string key = string.Empty;
            Regex reg = new Regex(@"Sec\-WebSocket\-Key:(.*?)\r\n");
            Match m = reg.Match(handShakeText);
            if (m.Value != "")
            {
                key = Regex.Replace(m.Value, @"Sec\-WebSocket\-Key:(.*?)\r\n", "$1").Trim();
            }
            byte[] secKeyBytes = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(key + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"));
            string secKey = Convert.ToBase64String(secKeyBytes);
            var responseBuilder = new StringBuilder();
            responseBuilder.Append("HTTP/1.1 101 Switching Protocols" + "\r\n");
            responseBuilder.Append("Upgrade: websocket" + "\r\n");
            responseBuilder.Append("Connection: Upgrade" + "\r\n");
            responseBuilder.Append("Sec-WebSocket-Accept: " + secKey + "\r\n\r\n");
            return Encoding.UTF8.GetBytes(responseBuilder.ToString());
        }
        #endregion

        #region 处理接收的数据
        /// <summary>
        /// 处理接收的数据
        /// 参考 http://www.cnblogs.com/smark/archive/2012/11/26/2789812.html
        /// </summary>
        /// <param name="recBytes"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string AnalyzeClientData(byte[] recBytes, int length)
        {
            int start = 0;
            // 如果有数据则至少包括3位
            if (length < 2) return "";
            // 判断是否为结束针
            bool IsEof = (recBytes[start] >> 7) > 0;
            // 暂不处理超过一帧的数据
            if (!IsEof) return "";
            start++;
            // 是否包含掩码
            bool hasMask = (recBytes[start] >> 7) > 0;
            // 不包含掩码的暂不处理
            if (!hasMask) return "";
            // 获取数据长度
            UInt64 mPackageLength = (UInt64)recBytes[start] & 0x7F;
            start++;
            // 存储4位掩码值
            byte[] Masking_key = new byte[4];
            // 存储数据
            byte[] mDataPackage;
            if (mPackageLength == 126)
            {
                // 等于126 随后的两个字节16位表示数据长度
                mPackageLength = (UInt64)(recBytes[start] << 8 | recBytes[start + 1]);
                start += 2;
            }
            if (mPackageLength == 127)
            {
                // 等于127 随后的八个字节64位表示数据长度
                mPackageLength = (UInt64)(recBytes[start] << (8 * 7) | recBytes[start] << (8 * 6) | recBytes[start] << (8 * 5) | recBytes[start] << (8 * 4) | recBytes[start] << (8 * 3) | recBytes[start] << (8 * 2) | recBytes[start] << 8 | recBytes[start + 1]);
                start += 8;
            }
            mDataPackage = new byte[mPackageLength];
            for (UInt64 i = 0; i < mPackageLength; i++)
            {
                mDataPackage[i] = recBytes[i + (UInt64)start + 4];
            }
            Buffer.BlockCopy(recBytes, start, Masking_key, 0, 4);
            for (UInt64 i = 0; i < mPackageLength; i++)
            {
                mDataPackage[i] = (byte)(mDataPackage[i] ^ Masking_key[i % 4]);
            }
            return Encoding.UTF8.GetString(mDataPackage);
        }
        #endregion

        #region 发送数据
        /// <summary>
        /// 把发送给客户端消息打包处理（拼接上谁什么时候发的什么消息）
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="message">Message.</param>
        public static byte[] PackageServerData(string msg)
        {
            byte[] content = null;
            byte[] temp = Encoding.UTF8.GetBytes(msg);
            if (temp.Length < 126)
            {
                content = new byte[temp.Length + 2];
                content[0] = 0x81;
                content[1] = (byte)temp.Length;
                Buffer.BlockCopy(temp, 0, content, 2, temp.Length);
            }
            else if (temp.Length < 0xFFFF)
            {
                content = new byte[temp.Length + 4];
                content[0] = 0x81;
                content[1] = 126;
                content[2] = (byte)(temp.Length & 0xFF);
                content[3] = (byte)(temp.Length >> 8 & 0xFF);
                Buffer.BlockCopy(temp, 0, content, 4, temp.Length);
            }
            return content;
        }
        #endregion
    }
}
