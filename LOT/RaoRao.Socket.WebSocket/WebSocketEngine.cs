﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RaoRao.Socket.WebSocket
{
    /// <summary>
    /// WebSocket服务类
    /// </summary>
    public class WebSocketEngine
    {
        //默认服务端口号
        private int port = 9000;
        //默认服务地址
        //数据大小
        byte[] buffer = new byte[4096];
        //创建服务对象
        private System.Net.Sockets.Socket socketEngine = null;
        /// <summary>
        /// socket连接池，保存客户端对象
        /// </summary>
        public Dictionary<string, SocketSession> ClientPool = new Dictionary<string, SocketSession>();
        /// <summary>
        /// socket连接池，保存客户端对象
        /// </summary>
        public int ClientCount = 0;
        /// <summary>
        /// 客户端连接代理
        /// </summary>
        public Action<IPEndPoint> ClientConnected = null;
        /// <summary>
        /// 客户端信息接收代理
        /// </summary>
        public Action<EndPoint, string> MessageReceived = null;
        /// <summary>
        /// 客户端下线收代理
        /// </summary>
        public Action<IPEndPoint> ClientDisconnected = null;
        /// <summary>
        /// 初始化WebSocket端口
        /// </summary>
        /// <param name="Port"></param>
        public WebSocketEngine(int Port)
        {
            port = Port;
        }

        /// <summary>
        /// 创建服务WebSocke对象
        /// </summary>
        /// <param name="ports">端口号</param>
        public bool CreateSocket(int ClientCount)
        {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
            socketEngine = new System.Net.Sockets.Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socketEngine.Bind(localEP);
                socketEngine.Listen(ClientCount);
                socketEngine.BeginAccept(new AsyncCallback(accept), socketEngine);
                Console.WriteLine("创建WebSocket服务成功：服务地址为：127.0.0.1:" + port.ToString());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("创建失败");
                return false;
            }
        }
        /// <summary>
        /// 客户端连接
        /// </summary>
        /// <param name="ar"></param>
        private void accept(IAsyncResult ar)
        {
            //获取socket服务套接字
            System.Net.Sockets.Socket server = (System.Net.Sockets.Socket)ar.AsyncState;
            // 在原始套接字上调用EndAccept方法，返回新的套接字
            System.Net.Sockets.Socket SockeClient = server.EndAccept(ar);
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
                lock (ClientPool)
                {
                    if (ClientPool.ContainsKey(session.IP))
                    {
                        ClientPool.Remove(session.IP);
                    }
                    ClientPool.Add(session.IP, session);
                    IPEndPoint ipaddress = SockeClient.RemoteEndPoint as IPEndPoint;
                    try { ClientConnected(ipaddress); } catch (Exception e) { }
                    Console.WriteLine("客户端:"+ ipaddress+"上线");
                }
                //准备接受下一个客户端
                server.BeginAccept(new AsyncCallback(accept), server);
            }
            catch (Exception e)
            {
                //throw e;
            }
        }
        /// <summary>
        /// 接受信息
        /// </summary>
        /// <param name="ar"></param>
        private void recieve(IAsyncResult ar)
        {
            System.Net.Sockets.Socket client = (System.Net.Sockets.Socket)ar.AsyncState;
            string ip = client.RemoteEndPoint.ToString();
            if (client == null || !ClientPool.ContainsKey(ip))
            {
                return;
            }
            IPEndPoint clientip = client.RemoteEndPoint as IPEndPoint;
            try
            {
                int length = client.EndReceive(ar);
                byte[] buffer = ClientPool[ip].buffer;
                client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(recieve), client);
                string msg = Encoding.UTF8.GetString(buffer, 0, length);
                //  websocket建立连接的时候，除了TCP连接的三次握手，websocket协议中客户端与服务器想建立连接需要一次额外的握手动作
                if (msg.Contains("Sec-WebSocket-Key"))
                {
                    client.Send(WebSocketHelper.PackageHandShakeData(buffer, length));
                    ClientPool[ip].isWeb = true;
                    Console.WriteLine("客户端:" + ip + "上线了");
                    return;
                }
                if (ClientPool[ip].isWeb)
                {
                    msg = WebSocketHelper.AnalyzeClientData(buffer, length);
                    Console.WriteLine("接受到来自客户端:");
                    MessageReceived(clientip, msg);
                    //sendmsg(str, ip);
                }
                Console.WriteLine(msg);
            }
            catch (Exception e)
            {
                //断开连接
                client.Disconnect(true);
                ClientPool.Remove(ip);
                Console.WriteLine("客户端:" + ip + "下线了");
                if (ClientDisconnected != null)
                    ClientDisconnected(clientip);
                return;
            }
        }

        /// <summary>
        /// 为某一用户发送信息
        /// </summary>
        /// <param name="ip">IPEndPoint</param>
        /// <param name="msg">msg</param>
        /// <returns></returns>
        public bool SendMsg(IPEndPoint ip, string msg)
        {
            try
            {
                bool result = false;
                SocketSession session = ClientPool[ip.ToString()];
                if (session != null)
                {
                    byte[] buffer = WebSocketHelper.PackageServerData(msg);
                    session.SockeClient.Send(buffer);
                    result = true;
                    Console.WriteLine("发送信息到客户端" + ip + ":" + msg);
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
                return false;
            }
        }
        /// <summary>
        /// 给所有用户发送信息
        /// </summary>
        /// <param name="msg"></param>
        public bool SendAll(string msg)
        {
            try
            {
                bool result = false;
                foreach (SocketSession item in ClientPool.Values)
                {
                    SocketSession session = item;
                    if (session != null)
                    {
                        byte[] buffer = WebSocketHelper.PackageServerData(msg);
                        session.SockeClient.Send(buffer);
                        result = true;
                        Console.WriteLine("发送信息到客户端" + session.IP + ":" + msg);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
                return false;
            }
        }
    }
}
