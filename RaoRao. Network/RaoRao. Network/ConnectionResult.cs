using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RaoRao.Network
{
    /// <summary>
    /// 扫描结果
    /// </summary>
    public class ConnectionResult
    {
        /// <summary>
        /// IPAddress 地址
        /// </summary>
        public IPAddress Address { get; set; }

        /// <summary>
        /// 是否可连接上
        /// </summary>
        public bool CanConnected { get; set; }
    }
}
