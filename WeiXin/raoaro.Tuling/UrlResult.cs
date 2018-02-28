using System;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 链接结果
    /// </summary>
    public class UrlResult : BaseResult
    {
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="url"></param>
        internal UrlResult(string text, string url)
            : base(CodeEnum.Url, text)
        {
            Url = url;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n链接地址：{1}", base.ToString(), Url);
        }

        /// <summary>
        /// 解析链接结果
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        internal static UrlResult Parse(JObject jo)
        {
            return new UrlResult((string)jo["text"], (string)jo["url"]);
        }
    }
}
