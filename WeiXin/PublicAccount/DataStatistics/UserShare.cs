using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 图文分享转发数据
    /// </summary>
    public class UserShare : IParsable
    {
        /// <summary>
        /// 获取数据的日期
        /// </summary>
        public DateTime ref_date { get; private set; }
        /// <summary>
        /// 获取分享的场景
        /// </summary>
        public ShareSceneEnum share_scene { get; private set; }
        /// <summary>
        /// 获取分享的次数
        /// </summary>
        public int share_count { get; private set; }
        /// <summary>
        /// 获取分享的人数
        /// </summary>
        public int share_user { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserShare()
        { }

        /// <summary>
        /// 从JObject对象解析统计数据
        /// </summary>
        /// <param name="jo"></param>
        public virtual void Parse(JObject jo)
        {
            ref_date = DateTime.Parse((string)jo["ref_date"]);
            share_scene = (ShareSceneEnum)(int)jo["share_scene"];
            share_count = (int)jo["share_count"];
            share_user = (int)jo["share_user"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("数据日期：{0:yyyy-MM-dd}\r\n分享场景：{1:g}\r\n分享次数：{2}\r\n分享人数：{3}",
                ref_date, share_scene, share_count, share_user);
        }
    }
}