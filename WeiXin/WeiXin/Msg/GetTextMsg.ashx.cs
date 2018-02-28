using PublicLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WeiXin.Msg
{
    /// <summary>
    /// 获取文字信息
    /// </summary>
    public class GetTextMsg : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string postStr =HttpRequestHelper.HttpRequestToStr(context);
            string EncodingAESKey = Utility.GetAppSettings("EncodingAESKey");
            string AppID = Utility.GetAppSettings("AppID");
            postStr=Cryptography.AES_decrypt(postStr, EncodingAESKey,ref AppID);

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