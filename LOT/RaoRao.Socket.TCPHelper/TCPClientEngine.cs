using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RaoRao.Socket.TCPHelper
{
    public class TCPClientEngine
    {
        ///<summary>
        /// 构造一个TcpClient类对象,TCP客户端  
        /// </summary>
        TcpClient client = null;
        /// <summary>
        /// 创建网络流,获取数据流
        /// </summary>
        NetworkStream stream = null;
        /// <summary>
        /// 接收信息
        /// </summary>
        public Action<string> Receive = null;
        Thread t1 = null;
        bool isclose = false;
        public TCPClientEngine(string ip, int port)
        {
            client = new TcpClient();
            IPAddress myIP = IPAddress.Parse(ip);
            //与TCP服务器连接  
            client.Connect(myIP, port);
            //创建网络流,获取数据流  
            stream = client.GetStream();
            t1 = new Thread(() =>
            {
                //读数据流对象  
                StreamReader sr = new StreamReader(stream);
                while (true)
                {
                    if (isclose) break;
                    string str = sr.ReadLine();
                    try
                    { Receive(str); }
                    catch (Exception e)
                    { }
                }
            });
            t1.Start();
            Console.WriteLine("服务器已经连接...请输入对话内容...");
        }

        public bool SendMsg(string msg)
        {
            try
            {
                //写数据流对象  
                StreamWriter sw = new StreamWriter(stream);
                sw.Write(msg);
                sw.Flush();             //刷新流 
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Close()
        {
            isclose = true;
            client.Close();
        }
    }
}
