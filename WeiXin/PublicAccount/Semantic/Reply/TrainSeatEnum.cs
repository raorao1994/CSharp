using System;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 火车座位级别： YZ （硬座）， RZ （软座），YW（硬卧），RW（软卧），YD（一等座）， ED （二等座）， TD （特等座）
    /// </summary>
    public enum TrainSeatEnum
    {
        /// <summary>
        /// 硬座
        /// </summary>
        YZ,
        /// <summary>
        /// 软座
        /// </summary>
        RZ,
        /// <summary>
        /// 硬卧
        /// </summary>
        YW,
        /// <summary>
        /// 软卧
        /// </summary>
        RW,
        /// <summary>
        /// 一等座
        /// </summary>
        YD,
        /// <summary>
        /// 二等座
        /// </summary>
        ED,
        /// <summary>
        /// 特等座
        /// </summary>
        TD
    }
}