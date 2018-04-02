using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    /// <summary>
    /// map 的摘要说明
    /// </summary>
    public class map : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string url = context.Request.RawUrl;
            context.Response.Redirect("http://172.30.17.212/ArcGIS/rest/services/DONGYING/DYDX/MapServer");
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