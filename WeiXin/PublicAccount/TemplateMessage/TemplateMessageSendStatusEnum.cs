using System;

namespace PublicAccount
{
    /// <summary>
    /// 模板消息发送状态
    /// </summary>
    public enum TemplateMessageSendStatusEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 失败：用户拒收
        /// </summary>
        UserBlock,
        /// <summary>
        /// 失败：系统失败
        /// </summary>
        SystemFailed
    }
}
