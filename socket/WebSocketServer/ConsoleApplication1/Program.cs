using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            RaoRao.WebSocket.WebSocket web = new RaoRao.WebSocket.WebSocket(9000);
            web.CreateTcpSocket(100);
            web.ClientConnected += (IPEndPoint p) => {
                Console.WriteLine("客户端：" + p.ToString() + "上线");
            };
            web.MessageReceived += (IPEndPoint p, string msg) => {
                Console.WriteLine("客户端：" + p.ToString() + "的信息：" + msg);
                web.SendAll("你好啊：客户端");
            };
            Console.Read();
        }
    }
}
