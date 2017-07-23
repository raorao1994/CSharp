using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 创建票据 请求实体
    /// </summary>
    /// 
    //[Route("/TicketRequest/{TicketId}/{TableNumber}/{ServerId}", Verbs = "GET")]
    [Route("/TicketRequest", Verbs = "GET")]

    public class TicketRequest : IReturn<TicketResponse>
    {
        public int TicketId { get; set; }
        public int TableNumber { get; set; }
        public int ServerId { get; set; }
    }

    [Route("/My/{TicketId}", Verbs = "GET")]
    public class TicketDeleteRequest
    {
        public int TicketId { get; set; }
    }
}
