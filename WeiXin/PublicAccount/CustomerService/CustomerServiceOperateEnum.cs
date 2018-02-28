using System;

namespace PublicAccount
{
    /// <summary>
    /// 操作（会话状态）
    /// </summary>
    public enum CustomerServiceOperateEnum
    {
        创建未接入会话 = 1000,
        接入会话 = 1001,
        主动发起会话 = 1002,
        关闭会话 = 1004,
        抢接会话 = 1005,
        公众号收到消息 = 2001,
        客服发送消息 = 2002,
        客服收到消息 = 2003
    }
}
