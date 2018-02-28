using System;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 服务器地址
    /// </summary>
    public class ServerAddresses : IParsable
    {
        /// <summary>
        /// 获取微信服务器地址列表的地址
        /// </summary>
        private const string urlForGettingServerAddresses = "https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token={0}";
        /// <summary>
        /// 获取微信服务器地址列表的http方法
        /// </summary>
        private const string httpMethodForGettingServerAddresses = WebRequestMethods.Http.Get;

        /// <summary>
        /// 地址数组
        /// </summary>
        public string[] ip_list { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ServerAddresses()
        { }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            JArray ja = (JArray)jo["ip_list"];
            ip_list = new string[ja.Count];
            for (int i = 0; i < ja.Count; i++)
                ip_list[i] = (string)ja[i];
        }

        /// <summary>
        /// 返回地址列表字符串，每个地址间用逗号分隔
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (ip_list != null && ip_list.Length > 0)
            {
                foreach (string ip in ip_list)
                    sb.AppendFormat("{0},", ip);
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取微信服务器的地址列表
        /// </summary>
        /// <param name="msg">服务器返回的错误提示。</param>
        /// <returns>返回微信服务器的地址列表</returns>
        public static ServerAddresses Get(out ErrorMessage msg)
        {
            return HttpHelper.RequestParsableResult<ServerAddresses>(urlForGettingServerAddresses, AccountInfoCollection.First.UserName, out msg, null, httpMethodForGettingServerAddresses, null);
        }
    }
}
