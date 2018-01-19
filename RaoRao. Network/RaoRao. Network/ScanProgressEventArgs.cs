using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaoRao.Network
{
    /// <summary>
    /// 扫描进度事件参数
    /// </summary>
    public class ScanProgressEventArgs
    {
        /// <summary>
        /// 进度百分比
        /// </summary>
        public int Percent { get; set; }
    }
}
