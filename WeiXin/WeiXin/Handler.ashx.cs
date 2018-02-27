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
            context.Response.ContentType = "text/plain";
            RaoRao.Log.LogOperater.Debug(new Exception(), "这是错误");
            context.Response.Write("OK");
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