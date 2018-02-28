using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 应用、软件、下载结果
    /// </summary>
    public class AppSoftDownloadResult : BaseResult
    {
        /// <summary>
        /// 软件列表
        /// </summary>
        public List<AppSoftDownload> Apps { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="apps"></param>
        internal AppSoftDownloadResult(string text, IEnumerable<AppSoftDownload> apps)
            : base(CodeEnum.AppSoftDownload, text)
        {
            if (apps == null)
                throw new ArgumentNullException("articles", "软件列表不能为空。");
            Apps = new List<AppSoftDownload>(apps);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendFormat("软件数：{0}", Apps.Count);
            if (Apps.Count > 0)
            {
                foreach (AppSoftDownload app in Apps)
                    sb.AppendFormat("\r\n{0}", app);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解析应用、软件、下载结果
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        internal static AppSoftDownloadResult Parse(JObject jo)
        {
            string text = (string)jo["text"];
            JArray ja = (JArray)jo["list"];
            AppSoftDownload[] artiles = new AppSoftDownload[ja.Count];
            int idx = 0;
            foreach (JObject item in ja)
            {
                artiles[idx] = new AppSoftDownload(item);
                idx++;
            }
            return new AppSoftDownloadResult(text, artiles);
        }
    }
}
