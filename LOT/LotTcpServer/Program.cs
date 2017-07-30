using System;
using System.Collections.Generic;
using System.Text;
using RaoRao.Socket.TCPHelper;
using RaoRao.Socket.UDPHelper;
using RaoRao.Socket.WebSocket;
using System.Net;

namespace LotTcpServer
{
    class Program
    {
        public static UDPEngine engine = null;
        static void Main(string[] args)
        {
            engine = new UDPEngine(9000);
            engine.CreateTcpSocket(100);
            engine.MessageReceived += receivemsg;
            Console.Read();
        }
        public static void receivemsg(EndPoint ip,string str)
        {
            Console.WriteLine(ip.ToString()+":"+str);
            engine.SendMsg(ip as IPEndPoint, str);
        }
    }
}
