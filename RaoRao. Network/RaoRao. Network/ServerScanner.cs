using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RaoRao.Network
{
    /// <summary>
    /// 扫描局域网中的服务
    /// </summary>
    public class ServerScanner : IServerScanner
    {
        /// <summary>
        /// 同一网段内 IP 地址的数量
        /// </summary>
        private const int SegmentIpMaxCount = 255;

        private DateTimeOffset _endTime;
        private object _locker = new object();
        private SynchronizationContext _originalContext = SynchronizationContext.Current;
        private List<ConnectionResult> _resultList = new List<ConnectionResult>();
        private DateTimeOffset _startTime;

        /// <summary>
        /// 记录调用/完成委托的数量
        /// </summary>
        private int _totalCount = 0;

        public ServerScanner()
        {
            Timeout = TimeSpan.FromSeconds(2);
        }

        /// <summary>
        /// 当扫描完成时，触发此事件
        /// </summary>
        public event EventHandler<List<ConnectionResult>> OnScanComplete;

        /// <summary>
        /// 当扫描进度发生更改时，触发此事件
        /// </summary>
        public event EventHandler<ScanProgressEventArgs> OnScanProgressChanged;

        /// <summary>
        /// 扫描端口
        /// </summary>
        public int ScanPort { get; set; }

        /// <summary>
        /// 单次请求的超时时长，默认为2秒
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// 使用 TcpClient 测试是否可以连上指定的 IP 与 Port
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool IsConnected(IPAddress ipAddress, int port)
        {
            var result = TestConnection(ipAddress, port);
            return result.CanConnected;
        }

        /// <summary>
        /// 使用 TcpClient 测试是否可以连上指定的 IP 与 Port
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool IsConnected(string ip, int port)
        {
            IPAddress ipAddress;
            if (IPAddress.TryParse(ip, out ipAddress))
            {
                return IsConnected(ipAddress, port);
            }
            else
            {
                throw new ArgumentException("IP 地址格式不正确");
            }
        }

        /// <summary>
        /// 开始扫描当前网段
        /// </summary>
        public void StartScan()
        {
            if (ScanPort == 0)
            {
                throw new InvalidOperationException("必须指定扫描的端口 ScanPort");
            }

            // 清除可能存在的数据
            _resultList.Clear();
            _totalCount = 0;
            _startTime = DateTimeOffset.Now;

            // 得到本网段的 IP
            var ipList = GetAllRemoteIPList();

            // 生成委托列表
            List<Func<IPAddress, int, ConnectionResult>> funcs = new List<Func<IPAddress, int, ConnectionResult>>();
            for (int i = 0; i < SegmentIpMaxCount; i++)
            {
                var tmpF = new Func<IPAddress, int, ConnectionResult>(TestConnection);
                funcs.Add(tmpF);
            }

            // 异步调用每个委托
            for (int i = 0; i < SegmentIpMaxCount; i++)
            {
                funcs[i].BeginInvoke(ipList[i], ScanPort, OnComplete, funcs[i]);
                _totalCount += 1;
            }
        }

        /// <summary>
        /// 得到本网段的所有 IP
        /// </summary>
        /// <returns></returns>
        private List<IPAddress> GetAllRemoteIPList()
        {
            var localName = Dns.GetHostName();
            var localIPEntry = Dns.GetHostEntry(localName);

            List<IPAddress> ipList = new List<IPAddress>();

            IPAddress localInterIP = localIPEntry.AddressList.FirstOrDefault(m => m.AddressFamily == AddressFamily.InterNetwork);
            if (localInterIP == null)
            {
                throw new InvalidOperationException("当前计算机不存在内网 IP");
            }

            var localInterIPBytes = localInterIP.GetAddressBytes();
            for (int i = 1; i <= SegmentIpMaxCount; i++)
            {
                // 对末位进行替换
                localInterIPBytes[3] = (byte)i;
                ipList.Add(new IPAddress(localInterIPBytes));
            }

            return ipList;
        }

        private void OnComplete(IAsyncResult ar)
        {
            var state = ar.AsyncState as Func<IPAddress, int, ConnectionResult>;
            var result = state.EndInvoke(ar);

            lock (_locker)
            {
                // 添加到结果中
                _resultList.Add(result);

                // 报告进度
                _totalCount -= 1;
                var percent = (SegmentIpMaxCount - _totalCount) * 100 / SegmentIpMaxCount;

                if (SynchronizationContext.Current == _originalContext)
                {
                    OnScanProgressChanged?.Invoke(this, new ScanProgressEventArgs { Percent = percent });
                }
                else
                {
                    _originalContext.Post(conState =>
                    {
                        OnScanProgressChanged?.Invoke(this, new ScanProgressEventArgs { Percent = percent });
                    }, null);
                }

                if (_totalCount == 0)
                {
                    // 通过事件抛出结果
                    if (SynchronizationContext.Current == _originalContext)
                    {
                        OnScanComplete?.Invoke(this, _resultList);
                    }
                    else
                    {
                        _originalContext.Post(conState =>
                        {
                            OnScanComplete?.Invoke(this, _resultList);
                        }, null);
                    }

                    // 计算耗时
                    Debug.WriteLine("Compete");
                    _endTime = DateTimeOffset.Now;
                    Debug.WriteLine($"Duration: {_endTime - _startTime}");
                }
            }
        }

        /// <summary>
        /// 测试是否可以连接到
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private ConnectionResult TestConnection(IPAddress address, int port)
        {
            TcpClient c = new TcpClient();

            ConnectionResult result = new ConnectionResult();
            result.Address = address;
            using (TcpClient tcp = new TcpClient())
            {
                IAsyncResult ar = tcp.BeginConnect(address, port, null, null);
                WaitHandle wh = ar.AsyncWaitHandle;
                try
                {
                    if (!ar.AsyncWaitHandle.WaitOne(Timeout, false))
                    {
                        tcp.Close();
                    }
                    else
                    {
                        tcp.EndConnect(ar);
                        result.CanConnected = true;
                    }
                }
                catch
                {
                }
                finally
                {
                    wh.Close();
                }
            }

            return result;
        }
    }
}
