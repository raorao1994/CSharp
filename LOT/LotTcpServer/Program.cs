using System;
using System.Collections.Generic;
using System.Text;
using RaoRao.Socket.TCPHelper;
using RaoRao.Socket.UDPHelper;
using RaoRao.Socket.WebSocket;
using System.Net;
using System.Threading;

namespace LotTcpServer
{
    class Program
    {
        public static UDPEngine udpengine = null;
        public static TCPEngine tcpengine = null;
        public static WebSocketEngine websocketengine = null;
        static void Main(string[] args)
        {
            //CreateWebSocketServer();
            CreateTCPClient();
            Console.Read();
        }
        static void CreateTCPClient()
        {
            TCPEngine engine = new TCPEngine(8181);
            TCPClientEngine cilent = engine.CreateClient("121.42.180.30");//121.42.180.30
            cilent.Receive +=(str) => {
                Console.WriteLine(str);
            };
            cilent.SendMsg("{\"M\":\"checkin\",\"ID\":\"2795\",\"K\":\"d843dc6d8\"}\n");
            Thread.Sleep(1000);
            int i = 1000;
            while (i>0)
            {
                double d = 23.0;
                double r = new Random().Next(-10,10);
                d = d + r*1.0/10;
                cilent.SendMsg("{\"M\":\"update\",\"ID\":\"2795\",\"V\":{\"2697\":"+d+ ",\"2713\":" + d + "}}\n");
                Console.WriteLine("发送信息");
                Thread.Sleep(2000);
                i--;
            }
        }
        static void CreateTCPServer()
        {
            tcpengine = new TCPEngine(9000);
            tcpengine.CreateServer(100);
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
            str = "发送到客户端信息：" + str;
            Console.WriteLine(ip.ToString()+":"+str);
            try {
                websocketengine.SendMsg(ip as IPEndPoint, str);
            }catch (Exception e) { }
            try
            {
                tcpengine.SendMsg(ip as IPEndPoint, str);
            }
            catch (Exception e) { }
            try
            {
                udpengine.SendMsg(ip as IPEndPoint, str);
            }
            catch (Exception e) { }
        }
    }
}
