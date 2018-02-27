using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PublicLibrary
{
    /// <summary>
    /// Http请求处理
    /// </summary>
    public class HttpRequestHelper
    {
        /// <summary>
        /// 验证url权限， 接入服务器
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool ValidUrl(HttpContext context,string token)
        {
            string echoStr = context.Request["echoStr"];//echostr(随机字符串)
            if (CheckSignature(context,token))
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    context.Response.Write(echoStr);
                    context.Response.End();
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        public static bool CheckSignature(HttpContext context, string token)
        {
            //（微信加密签名，signature结合了开发者填写的token参数和
            //请求中的timestamp参数、nonce参数。）
            string signature = context.Request["signature"];
            string timestamp = context.Request["timestamp"];//timestamp（时间戳）
            string nonce = context.Request["nonce"];//nonce（随机数）
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = Utility.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
