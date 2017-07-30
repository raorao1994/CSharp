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
        public static UDPEngine udpengine = null;
        public static TCPEngine tcpengine = null;
        public static WebSocketEngine websocketengine = null;
        static void Main(string[] args)
        {
            Console.Read();
        }
        static void CreateTCPServer()
        {
            tcpengine = new TCPEngine(9000);
            tcpengine.CreateSocket(100);
            tcpengine.MessageReceived += receivemsg;
        }
        static void CreateUDPServer()
        {
            udpengine = new UDPEngine(9000);
            udpengine.CreateSocket();
            udpengine.MessageReceived += receivemsg;
        }
        static void CreateWebSocketServer()
        {
            websocketengine = new WebSocketEngine(9000);
            websocketengine.CreateSocket(100);
            websocketengine.MessageReceived += receivemsg;
        }
        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="str"></param>
        public static void receivemsg(EndPoint ip,string str)
        {
            Console.WriteLine(ip.ToString()+":"+str);
            engine.SendMsg(ip as IPEndPoint, str);
        }
    }
}
