﻿using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 周边语义应答
    /// </summary>
    public class NearbyReply:BaseReply
    {
        /// <summary>
        /// 周边语义
        /// </summary>
        public NearbySemantic semantic { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            semantic = Utility.Parse<NearbySemantic>((JObject)jo["semantic"]);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n语义应答：{1}",
                base.ToString(), semantic);
        }
    }
}
