﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketServer
{
    /// <summary>
    /// WebSocket客户端对象
    /// </summary>
    public class SocketSession
    {
        /// <summary>
        /// 客户端Socket对象
        /// </summary>
        private Socket _sockeclient;
        /// <summary>
        /// 数据接受容器
        /// </summary>
        private byte[] _buffer;
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        private string _ip;
        /// <summary>
        /// 是否是web客户端
        /// </summary>
        private bool _isweb = false;

        public Socket SockeClient
        {
            set { _sockeclient = value; }
            get { return _sockeclient; }
        }

        public byte[] buffer
        {
            set { _buffer = value; }
            get { return _buffer; }
        }

        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }

        public bool isWeb
        {
            set { _isweb = value; }
            get { return _isweb; }
        }
    }
}
