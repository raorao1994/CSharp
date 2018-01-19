using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaoRao.Network
{
    /// <summary>
    /// 扫描完成事件参数
    /// </summary>
    public class ScanCompleteEventArgs
    {
        /// <summary>
        /// 结果集合
        /// </summary>
        public List<ConnectionResult> Reslut { get; set; }
    }
}
