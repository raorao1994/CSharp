using System;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;

namespace PublicAccount
{
    /// <summary>
    /// 响应音乐消息
    /// </summary>
    public class ResponseMusicMessage : ResponseBaseMessage
    {
        /// <summary>
        /// 获取或设置音乐标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 获取或设置音乐描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 获取或设置音乐链接
        /// </summary>
        public string MusicUrl { get; set; }
        /// <summary>
        /// 获取或设置高质量音乐链接
        /// </summary>
        public string HQMusicUrl { get; set; }
        private string thumbMediaId;
        /// <summary>
        /// 获取缩略图的媒体ID
        /// </summary>
        public string ThumbMediaId
        {
            get
            {
                return thumbMediaId;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("ThumbMediaId", "ThumbMediaId为空。");
                thumbMediaId = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName">接收方账号</param>
        /// <param name="fromUserName">开发者微信号</param>
        /// <param name="createTime">消息创建时间</param>
        /// <param name="title">标题</param>
        /// <param name="description">描述</param>
        /// <param name="musicUrl">音乐链接</param>
        /// <param name="hqMusicUrl">高质量音乐链接</param>
        /// <param name="thumbMediaId">缩略图的媒体ID</param>
        public ResponseMusicMessage(string toUserName, string fromUserName, DateTime createTime,
            string title, string description, string musicUrl, string hqMusicUrl, string thumbMediaId)
            : base(toUserName, fromUserName, createTime, ResponseMessageTypeEnum.music)
        {
            Title = title;
            Description = description;
            MusicUrl = musicUrl;
            HQMusicUrl = hqMusicUrl;
            ThumbMediaId = thumbMediaId;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName">接收方账号</param>
        /// <param name="fromUserName">开发者微信号</param>
        /// <param name="createTime">消息创建时间</param>
        /// <param name="thumbMediaId">缩略图的媒体ID</param>
        public ResponseMusicMessage(string toUserName, string fromUserName, DateTime createTime,
            string thumbMediaId)
            : this(toUserName, fromUserName, createTime, null, null, null, null, thumbMediaId)
        {
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n音乐标题：{1}\r\n音乐描述：{2}" +
            "音乐链接：{3}\r\n高质量音乐链接：{4}\r\n缩略图的媒体ID：{5}",
                base.ToString(), Title ?? "", Description ?? "",
                MusicUrl ?? "", HQMusicUrl ?? "", ThumbMediaId);
        }

        /// <summary>
        /// 返回XML格式的响应消息
        /// </summary>
        /// <returns>返回响应消息</returns>
        protected override string ToXml()
        {
            XmlDocument doc = CreateXmlDocument();
            XmlElement root = doc.DocumentElement;
            XmlElement music = CreateXmlElement(doc, "Music");
            music.AppendChild(CreateXmlElement(doc, "Title", Title ?? ""));
            music.AppendChild(CreateXmlElement(doc, "Description", Description ?? ""));
            music.AppendChild(CreateXmlElement(doc, "MusicUrl", MusicUrl ?? ""));
            music.AppendChild(CreateXmlElement(doc, "HQMusicUrl", HQMusicUrl ?? ""));
            music.AppendChild(CreateXmlElement(doc, "ThumbMediaId", ThumbMediaId));
            root.AppendChild(music);
            return doc.InnerXml;
        }

        /// <summary>
        /// 返回JSON格式的客服消息
        /// </summary>
        /// <returns></returns>
        public override string ToJson()
        {
            var customerService = new
            {
                touser = ToUserName,
                msgtype = MsgType.ToString("g"),
                music = new
                {
                    title = Title,
                    description = Description,
                    musicurl = MusicUrl,
                    hqmusicurl = HQMusicUrl,
                    thumb_media_id = ThumbMediaId
                }
            };
            return JsonConvert.SerializeObject(customerService);
        }
    }
}