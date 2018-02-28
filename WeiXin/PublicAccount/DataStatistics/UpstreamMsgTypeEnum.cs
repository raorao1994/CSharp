using System;

namespace PublicAccount
{
    /// <summary>
    /// 消息分析数据接口中的消息类型
    /// </summary>
    public enum UpstreamMsgTypeEnum
    {
        /// <summary>
        /// 文字
        /// </summary>
        text = 1,
        /// <summary>
        /// 图片
        /// </summary>
        image = 2,
        /// <summary>
        /// 语音
        /// </summary>
        voice = 3,
        /// <summary>
        /// 视频
        /// </summary>
        video = 4,
        /// <summary>
        /// 第三方应用消息（链接消息）
        /// </summary>
        third_party_app_msg = 6
    }
}
