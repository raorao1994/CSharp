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
            string token = Utility.GetAppSettings("token");
            HttpRequestHelper.ValidUrl(context, token);
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