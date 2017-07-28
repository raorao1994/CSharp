using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LotTcpServer
{
    /// <summary>
    /// WebSocket服务类
    /// </summary>
    public class MySocket
    {

        //默认服务端口号
        static int port = 9000;
        //默认服务地址
        //数据大小
        byte[] buffer = new byte[4096];
        //创建服务对象
        static Socket socketEngine = null;
        /// <summary>
        /// socket连接池
        /// </summary>
        private static Dictionary<string, SocketSession> SessionPool = new Dictionary<string, SocketSession>();
        private Dictionary<string, string> MsgPool = new Dictionary<string, string>();
        /// <summary>
        /// 创建服务
        /// </summary>
        /// <param name="ports">端口号</param>
        public static bool CreateTcpSocket(int ports, int ClientCount)
        {
            port = ports;
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
            socketEngine = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socketEngine.Bind(localEP);
                socketEngine.Listen(ClientCount);
                socketEngine.BeginAccept(new AsyncCallback(accept), socketEngine);
                Console.WriteLine("创建服务成功：服务地址为：127.0.0.1:" + port.ToString());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("创建失败");
                return false;
            }
        }
        /// <summary>
        /// 处理客户端连接请求
        /// </summary>
        /// <param name="ar"></param>
        private static void accept(IAsyncResult ar)
        {
            //获取socket服务套接字
            Socket server = (Socket)ar.AsyncState;
            // 在原始套接字上调用EndAccept方法，返回新的套接字
            Socket SockeClient = server.EndAccept(ar);
            byte[] buffer = new byte[4096];
            try
            {
                SockeClient.BeginReceive(buffer, 0, buffer.Length,
                    SocketFlags.None, new AsyncCallback(recieve), SockeClient);
                //保存客户端对象到socket池
                SocketSession session = new SocketSession();
                session.SockeClient = SockeClient;
                session.IP = SockeClient.RemoteEndPoint.ToString();
                session.buffer = buffer;
                lock (SessionPool)
                {
                    if (SessionPool.ContainsKey(session.IP))
                    {
                        SessionPool.Remove(session.IP);
                    }
                    SessionPool.Add(session.IP, session);
                }
                //准备接受下一个客户端
                server.BeginAccept(new AsyncCallback(accept), server);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 接受信息
        /// </summary>
        /// <param name="ar"></param>
        private static void recieve(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            string ip = client.RemoteEndPoint.ToString();
            if (client == null || !SessionPool.ContainsKey(ip))
            {
                return;
            }
            try
            {
                int length = client.EndReceive(ar);
                byte[] buffer = SessionPool[ip].buffer;
                client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(recieve), client);
                string msg = Encoding.UTF8.GetString(buffer, 0, length);
                //  websocket建立连接的时候，除了TCP连接的三次握手，websocket协议中客户端与服务器想建立连接需要一次额外的握手动作
                if (msg.Contains("Sec-WebSocket-Key"))
                {
                    client.Send(SocketHelper.PackageHandShakeData(buffer, length));
                    SessionPool[ip].isWeb = true;
                    Console.WriteLine("接受到来自客户端:" + ip + "的信息：" + msg);
                    return;
                }
                if (SessionPool[ip].isWeb)
                {
                    msg = SocketHelper.AnalyzeClientData(buffer, length);
                    string str = "你好啊！客户端:" + msg;
                    sendmsg(str, ip);
                }
                //Console.WriteLine(msg);
                //sendmsg("123456789", ip);
            }
            catch (Exception e)
            {
                //断开连接
                client.Disconnect(true);
                SessionPool.Remove(ip);
                Console.WriteLine("接受失败");
                return;
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="ips"></param>
        public static bool sendmsg(string msg, params string[] ips)
        {
            try
            {
                bool result = false;
                foreach (var item in ips)
                {
                    SocketSession session = SessionPool[item];
                    if (session != null)
                    {
                        byte[] buffer = SocketHelper.PackageServerData(msg);
                        //session.SockeClient.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(send), session.SockeClient);
                        session.SockeClient.Send(buffer);
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

}
