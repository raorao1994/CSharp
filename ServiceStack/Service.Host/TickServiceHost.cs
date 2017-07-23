using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Host
{
    public class TickServiceHost : ServiceStack.AppHostBase
    {
        // Register your Web service with ServiceStack.
        public TickServiceHost()
            : base("Ticket Service", typeof(Service.TicketService).Assembly) { }
        public override void Configure(Funq.Container container)
        {
            // Register any dependencies your services use here.
        }
    }
}
