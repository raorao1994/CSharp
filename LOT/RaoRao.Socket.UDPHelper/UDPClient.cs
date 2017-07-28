using System;
using System.Collections.Generic;
using System.Text;

namespace RaoRao.Socket.UDPHelper
{
    /// <summary>
    /// TCP客户端对象
    /// </summary>
    public class UDPClient
    {
        /// <summary>
        /// 数据接受容器
        /// </summary>
        private byte[] _buffer;
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        private string _ip;

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
    }
}
