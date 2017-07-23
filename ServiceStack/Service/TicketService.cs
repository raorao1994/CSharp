using Interface;
using Model;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TicketService : ServiceStack.Service, ITicketService
    {
        /// <summary>
        /// 方法请求模式和路由规则 后面再说
        /// 请求票据列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<TicketResponse> Any(TicketRequest request)
        {
            List<TicketResponse> result = new List<TicketResponse>();
            List<OrderResponse> orderlist = new List<OrderResponse>();
            orderlist.Add(new OrderResponse() { OrderId = 1 });
            orderlist.Add(new OrderResponse() { OrderId = 2 });
            orderlist.Add(new OrderResponse() { OrderId = 3 });
            orderlist.Add(new OrderResponse() { OrderId = 4 });
            result.Add(new TicketResponse()
            {
                Orders = orderlist,
                ServerId = 1,
                TableNumber = 1,
                TicketId = 1,
                Timestamp = DateTime.Now
            });
            result.Add(new TicketResponse()
            {
                Orders = orderlist,
                ServerId = 2,
                TableNumber = 2,
                TicketId = 2,
                Timestamp = DateTime.Now
            });
            result.Add(new TicketResponse()
            {
                Orders = orderlist,
                ServerId = 3,
                TableNumber = 31,
                TicketId =3,
                Timestamp = DateTime.Now
            });
            result.Add(new TicketResponse()
            {
                Orders = orderlist,
                ServerId = 4,
                TableNumber = 4,
                TicketId = 42,
                Timestamp = DateTime.Now
            });
            return result;
        }
        /// <summary>
        /// 删除指定票据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Get(TicketDeleteRequest deleteid)
        {
            return "这是我的Get请求返回值";
        }
    }
}
