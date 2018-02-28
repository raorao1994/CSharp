using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/// <summary>
/// 接收普通消息实体类
/// </summary>
namespace WeiXin.Model
{
    /// <summary>
    /// 信息父类类
    /// </summary>
    public class BaseMsg
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public string CreateTime { get; set; }
        public string MsgType { get; set; }
        public string MsgId { get; set; }

        public string Appid { get; set; }
        public string Token { get; set; }
        public string EncodingAESKey { get; set; }
    }
    /// <summary>
    /// 文本信息类
    /// </summary>
    public class TextMsg:BaseMsg
    {
        public string Content { get; set; }
    }
    /// <summary>
    /// 图片消息
    /// </summary>
    public class PictureMsg : BaseMsg
    {
        public string PicUrl { get; set; }
    }
    /// <summary>
    /// 音频消息
    /// </summary>
    public class VoiceMsg : BaseMsg
    {
        public string MediaId { get; set; }
        public string Format { get; set; }
    }
    /// <summary>
    /// 视频消息
    /// </summary>
    public class VideoMsg : BaseMsg
    {
        public string MediaId { get; set; }
        public string ThumbMediaId { get; set; }
    }
    /// <summary>
    /// 地理位置消息
    /// </summary>
    public class LocationMsg : BaseMsg
    {
        public string Location_X { get; set; }
        public string Location_Y { get; set; }
        public string Scale { get; set; }
        public string Label { get; set; }
    }
    /// <summary>
    /// 链接消息
    /// </summary>
    public class LinkMsg : BaseMsg
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
    /// <summary>
    /// 事件消息
    /// </summary>
    public class EventMsg : BaseMsg
    {
        public string Event { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Precision { get; set; }
    }
    /// <summary>
    /// Widget消息
    /// </summary>
    public class WidgetMsg : BaseMsg
    {
        public string Event { get; set; }
        public string Query { get; set; }
        public string Scene { get; set; }
    }
}