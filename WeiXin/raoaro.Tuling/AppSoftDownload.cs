using System;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 应用、软件、下载
    /// </summary>
    public class AppSoftDownload
    {
        /// <summary>
        /// 软件名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 下载量
        /// </summary>
        public string Count { get; private set; }
        /// <summary>
        /// 详情地址
        /// </summary>
        public string DetailUrl { get; private set; }
        /// <summary>
        /// 图标地址
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        /// <param name="detailUrl"></param>
        /// <param name="icon"></param>
        internal AppSoftDownload(string name, string count, string detailUrl, string icon)
        {
            this.Name = name;
            Count = count;
            DetailUrl = detailUrl;
            Icon = icon;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jo">jobject对象</param>
        internal AppSoftDownload(JObject jo)
            : this((string)jo["name"], (string)jo["count"], (string)jo["detailurl"], (string)jo["icon"])
        { }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("软件名称：{0}\r\n下载量：{1}\r\n详情地址：{2}\r\n图标地址：{3}",
                Name, Count, DetailUrl, Icon);
        }
    }
}
