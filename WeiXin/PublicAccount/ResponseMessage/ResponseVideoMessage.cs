using System;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;

namespace PublicAccount
{
    /// <summary>
    /// 响应视频消息
    /// </summary>
    public class ResponseVideoMessage : ResponseBaseMessage
    {
        private string mediaId;
        /// <summary>
        /// 获取或设置媒体ID
        /// </summary>
        public string MediaId
        {
            get
            {
                return mediaId;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("MediaId", "Media为空。");
                mediaId = value;
            }
        }
        /// <summary>
        /// 视频标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 视频描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName">接收方账号</param>
        /// <param name="fromUserName">开发者微信号</param>
        /// <param name="createTime">消息创建时间</param>
        /// <param name="mediaId">媒体ID</param>
        /// <param name="title">标题</param>
        /// <param name="description">描述</param>
        public ResponseVideoMessage(string toUserName, string fromUserName, DateTime createTime,
            string mediaId, string title = null, string description = null)
            : base(toUserName, fromUserName, createTime, ResponseMessageTypeEnum.video)
        {
            MediaId = mediaId;
            Title = title;
            Description = description;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n媒体ID：{1}\r\n视频标题：{2}\r\n视频描述：{3}",
                base.ToString(), MediaId, Title ?? "", Description ?? "");
        }

        /// <summary>
        /// 返回XML格式的响应消息
        /// </summary>
        /// <returns>返回响应消息</returns>
        protected override string ToXml()
        {
            XmlDocument doc = CreateXmlDocument();
            XmlElement root = doc.DocumentElement;
            XmlElement video = CreateXmlElement(doc, "Video");
            video.AppendChild(CreateXmlElement(doc, "MediaId", MediaId));
            video.AppendChild(CreateXmlElement(doc, "Title", Title ?? ""));
            video.AppendChild(CreateXmlElement(doc, "Description", Description ?? ""));
            root.AppendChild(video);
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
                voice = new
                {
                    media_id = MediaId,
                    title = Title,
                    description = Description
                }
            };
            return JsonConvert.SerializeObject(customerService);
        }
    }
}