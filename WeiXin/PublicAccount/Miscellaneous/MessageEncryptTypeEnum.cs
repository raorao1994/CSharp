using System;

namespace PublicAccount
{
    /// <summary>
    /// 消息加密类型
    /// </summary>
    public enum MessageEncryptTypeEnum
    {
        /// <summary>
        /// 明文：请求与回复的消息体均为明文
        /// </summary>
        raw,
        /// <summary>
        /// aes密文：请求与回复的消息体均为aes加密之后的密文
        /// </summary>
        aes
    }
}