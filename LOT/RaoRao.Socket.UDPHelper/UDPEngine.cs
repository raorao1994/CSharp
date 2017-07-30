using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RaoRao.Socket.UDPHelper
{
    /// <summary>
    /// UDP服务类
    /// </summary>
    public class UDPEngine
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
        public Dictionary<string, UDPClient> ClientPool = new Dictionary<string, UDPClient>();
        /// <summary>
        /// socket连接池，保存客户端对象
        /// </summary>
        public int ClientCount = 0;
        /// <summary>
        /// 客户端信息接收代理
        /// </summary>
        public Action<EndPoint, string> MessageReceived = null;
        /// <summary>
        /// 初始化WebSocket端口
        /// </summary>
        /// <param name="Port"></param>
        public UDPEngine(int Port)
        {
            port = Port;
        }

        /// <summary>
        /// 创建服务WebSocke对象
        /// </summary>
        /// <param name="ports">端口号</param>
        public bool CreateSocket()
        {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
            socketEngine = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                socketEngine.Bind(localEP);
                Console.WriteLine("创建UDP服务成功：服务地址为：127.0.0.1:" + port.ToString());
                Thread t = new Thread(recieve);//开启接收消息线程
                t.Start();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("创建失败");
                return false;
            }
        }
        /// <summary>
        /// 接受信息
        /// </summary>
        /// <param name="ar"></param>
        private void recieve()
        {
            try
            {
                while (true)
                {
                    EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                    byte[] buffer = new byte[1024];
                    int length = socketEngine.ReceiveFrom(buffer, ref point);//接收数据报
                    string message = Encoding.UTF8.GetString(buffer, 0, length);
                    MessageReceived(point, message);
                    Console.WriteLine("接受到来自客户端:" + point + "的信息：" + message);
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        /// <summary>
        /// 为某一用户发送信息
        /// </summary>
        /// <param name="ip">IPEndPoint</param>
        /// <param name="msg">msg</param>
        /// <returns></returns>
        public bool SendMsg(EndPoint ip, string msg)
        {
            try
            {
                bool result = false;
                byte[] buffer = UDPHelper.PackHandShakeData(msg);
                UdpClient client = new UdpClient();
                client.Send(buffer, buffer.Length, ip as IPEndPoint);//将数据发送到远程端点
                client.Close();//关闭连接
                result = true;
                Console.WriteLine("发送信息到客户端" + ip + ":" + msg);
                return result;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
