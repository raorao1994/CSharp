using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RaoRao.Network
{
    /// <summary>
    /// 扫描服务
    /// </summary>
    public interface IServerScanner
    {
        /// <summary>
        /// 扫描完成
        /// </summary>
        event EventHandler<List<ConnectionResult>> OnScanComplete;

        /// <summary>
        /// 报告扫描进度
        /// </summary>
        event EventHandler<ScanProgressEventArgs> OnScanProgressChanged;

        /// <summary>
        /// 扫描端口
        /// </summary>
        int ScanPort { get; set; }

        /// <summary>
        /// 单次连接超时时长
        /// </summary>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// 返回指定的IP与端口是否能够连接上
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        bool IsConnected(IPAddress ipAddress, int port);

        /// <summary>
        /// 返回指定的IP与端口是否能够连接上
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        bool IsConnected(string ip, int port);

        /// <summary>
        /// 开始扫描
        /// </summary>
        void StartScan();
    }
}
