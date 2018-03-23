using APP.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WebApplication
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
            //初始化其它服务主机...以实现同一asp.net程序中发布多个服务（note:个人见解）
        }
    }
}