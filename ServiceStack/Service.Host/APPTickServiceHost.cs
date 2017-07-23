using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Host
{
    /// <summary>
    /// 控制台应用程序服务host
    ///
    /// </summary>
    public class AppTickServiceHost : ServiceStack.AppSelfHostBase
    {
        // Register your Web service with ServiceStack.
        public AppTickServiceHost()
            : base("WebApplication1", typeof(Service.TicketService).Assembly) { }
        public override void Configure(Funq.Container container)
        {
            // Register any dependencies your services use here.
        }
    }
}
