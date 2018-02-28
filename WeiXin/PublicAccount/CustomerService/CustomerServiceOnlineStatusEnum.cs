using System;

namespace PublicAccount
{
    /// <summary>
    /// 客服在线状态
    /// </summary>
    [Flags]
    public enum CustomerServiceOnlineStatusEnum
    {
        /// <summary>
        /// PC在线
        /// </summary>
        pc = 1,
        /// <summary>
        /// 手机在线
        /// </summary>
        mobile = 2
    }
}
