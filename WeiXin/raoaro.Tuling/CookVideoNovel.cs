using System;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 菜谱、视频、小说
    /// </summary>
    public class CookVideoNovel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string Info { get; private set; }
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
        /// <param name="info"></param>
        /// <param name="detailUrl"></param>
        /// <param name="icon"></param>
        internal CookVideoNovel(string name, string info, string detailUrl, string icon)
        {
            this.Name = name;
            Info = info;
            DetailUrl = detailUrl;
            Icon = icon;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jo">jobject对象</param>
        internal CookVideoNovel(JObject jo)
            : this((string)jo["name"], (string)jo["info"], (string)jo["detailurl"], (string)jo["icon"])
        { }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("名称：{0}\r\n详情：{1}\r\n详情地址：{2}\r\n图标地址：{3}",
                Name, Info, DetailUrl, Icon);
        }
    }
}
