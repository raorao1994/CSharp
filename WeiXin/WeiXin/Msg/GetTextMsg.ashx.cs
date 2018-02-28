using PublicLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using WeiXin.Model;

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
            InitMsg msg = postStr.JsonStrToModel<InitMsg>();
            string EncodingAESKey = Utility.GetAppSettings("EncodingAESKey");
            string AppID = Utility.GetAppSettings("AppID");
            postStr=Cryptography.AES_decrypt(msg.Encrypt, EncodingAESKey,ref AppID);
            RaoRao.Log.LogOperater.Debug(new Exception(), "接收到的信息(解码)："+ postStr);
            context.Response.Write("");
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