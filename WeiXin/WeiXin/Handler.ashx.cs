using PublicLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXin
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //RaoRao.Log.LogOperater.Debug(new Exception(), "这是错误");
            //context.Response.Write("OK");
            string echoStr = context.Request["echoStr"];//echostr(随机字符串)
            string signature = context.Request["signature"];
            string timestamp = context.Request["timestamp"];//timestamp（时间戳）
            string nonce = context.Request["nonce"];//nonce（随机数）
            string str = "echoStr="+ echoStr+
               "signature=" + signature +
               "timestamp=" + timestamp +
               "nonce=" + nonce;
            string[] ArrTmp = { "raorao", timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = Utility.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            str += "tmpStr="+ tmpStr;
            RaoRao.Log.LogOperater.Debug(new Exception(), str);
            HttpRequestHelper.ValidUrl(context,"raorao");

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}