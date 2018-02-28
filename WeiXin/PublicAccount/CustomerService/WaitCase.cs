using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 未接入的客户
    /// </summary>
    public class WaitCase : IParsable
    {
        /// <summary>
        /// 客户账号
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 客服账号
        /// </summary>
        public string kf_account { get; set; }
        /// <summary>
        /// 客户来访时间
        /// </summary>
        public int createtime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WaitCase()
        { }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            openid = (string)jo["openid"];
            kf_account = (string)jo["kf_account"];
            createtime = (int)jo["createtime"];
        }

        /// <summary>
        /// 获取客户来访时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetCreateTime()
        {
            return Utility.ToDateTime(createtime);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("客户账号：{0}\r\n客服账号：{1}\r\n客户来访时间：{2}",
                openid, kf_account, GetCreateTime());
        }
    }

    /// <summary>
    /// 未接入的会话
    /// </summary>
    public class WaitCaseSession : IParsable
    {
        /// <summary>
        /// 未接入的客户总数
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 未接入的客户列表，最多返回100条数据，列表长度不一定等于未接入的客户总数
        /// </summary>
        public WaitCase[] waitcaselist { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WaitCaseSession()
        { }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            count = (int)jo["count"];
            JToken jt;
            if (jo.TryGetValue("waitcaselist", out jt))
            {
                if (jt.Type == JTokenType.Array)
                {
                    JArray ja = (JArray)jt;
                    waitcaselist = new WaitCase[ja.Count];
                    for (int i = 0; i < ja.Count; i++)
                    {
                        waitcaselist[i] = new WaitCase();
                        waitcaselist[i].Parse((JObject)ja[i]);
                    }
                }
                else
                    waitcaselist = null;
            }
            else
                waitcaselist = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("未接入的客户总数：{0}", count);
            if (waitcaselist != null && waitcaselist.Length > 0)
            {
                foreach (WaitCase waitcase in waitcaselist)
                    sb.AppendFormat("\r\n{0}", waitcase);
            }
            return sb.ToString();
        }
    }
}