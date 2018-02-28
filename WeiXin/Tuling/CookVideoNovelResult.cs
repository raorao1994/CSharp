using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 菜谱、视频、小说结果
    /// </summary>
    public class CookVideoNovelResult : BaseResult
    {
        /// <summary>
        /// 软件列表
        /// </summary>
        public List<CookVideoNovel> Lists { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lists"></param>
        internal CookVideoNovelResult(string text, IEnumerable<CookVideoNovel> lists)
            : base(CodeEnum.CookVideoNovel, text)
        {
            if (lists == null)
                throw new ArgumentNullException("articles", "列表不能为空。");
            Lists = new List<CookVideoNovel>(lists);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendFormat("列表数目：{0}", Lists.Count);
            if (Lists.Count > 0)
            {
                foreach (CookVideoNovel item in Lists)
                    sb.AppendFormat("\r\n{0}", item);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解析菜谱、视频、小说结果
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        internal static CookVideoNovelResult Parse(JObject jo)
        {
            string text = (string)jo["text"];
            JArray ja = (JArray)jo["list"];
            CookVideoNovel[] lists = new CookVideoNovel[ja.Count];
            int idx = 0;
            foreach (JObject item in ja)
            {
                lists[idx] = new CookVideoNovel(item);
                idx++;
            }
            return new CookVideoNovelResult(text, lists);
        }
    }
}
