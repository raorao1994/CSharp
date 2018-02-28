using System;

namespace PublicAccount
{
    /// <summary>
    /// 发送消息量分布的区间
    /// </summary>
    public enum UpstreamMsgCountIntervalEnum
    {
        /// <summary>
        /// 0
        /// </summary>
        Zero = 0,
        /// <summary>
        /// 1-5
        /// </summary>
        OneToFive = 1,
        /// <summary>
        /// 6-10
        /// </summary>
        SixToTen = 2,
        /// <summary>
        /// 10次以上
        /// </summary>
        MoreThanTen = 3
    }
}
