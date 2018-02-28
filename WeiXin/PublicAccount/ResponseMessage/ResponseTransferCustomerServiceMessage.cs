using System;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;

namespace PublicAccount
{
    /// <summary>
    /// 转发客服消息
    /// </summary>
    public class ResponseTransferCustomerServiceMessage : ResponseBaseMessage
    {
        /// <summary>
        /// 获取或设置接入会话的客服账号
        /// </summary>
        public string KfAccount { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName">接收方账号</param>
        /// <param name="fromUserName">开发者微信号</param>
        /// <param name="createTime">消息创建时间</param>
        /// <param name="kfAccount">接入会话的客服账号</param>
        public ResponseTransferCustomerServiceMessage(string toUserName, string fromUserName, DateTime createTime,
            string kfAccount="")
            : base(toUserName, fromUserName, createTime, ResponseMessageTypeEnum.transfer_customer_service)
        {
            KfAccount = kfAccount;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n接入会话的客服账号：{1}",
                base.ToString(), KfAccount ?? string.Empty);
        }

        /// <summary>
        /// 返回XML格式的响应消息
        /// </summary>
        /// <returns>返回响应消息</returns>
        protected override string ToXml()
        {
            XmlDocument doc = CreateXmlDocument();
            if (!string.IsNullOrWhiteSpace(KfAccount))
            {
                XmlElement root = doc.DocumentElement;
                XmlElement transInfo = CreateXmlElement(doc, "TransInfo");
                transInfo.AppendChild(CreateXmlElement(doc, "KfAccount", KfAccount));
                root.AppendChild(transInfo);
            }
            return doc.InnerXml;
        }

        /// <summary>
        /// 返回JSON格式的客服消息
        /// 注意：目前微信不支持将“转发客服消息”发送为客服消息，实现ToJson是为了该系列类的一致性
        /// </summary>
        /// <returns></returns>
        public override string ToJson()
        {
            if (!string.IsNullOrWhiteSpace(KfAccount))
            {
                var customerService = new
                {
                    touser = ToUserName,
                    msgtype = MsgType.ToString("g"),
                    TransInfo = new
                    {
                        KfAccount = KfAccount
                    }
                };
                return JsonConvert.SerializeObject(customerService);
            }
            else
            {
                var customerService = new
                {
                    touser = ToUserName,
                    msgtype = MsgType.ToString("g")
                };
                return JsonConvert.SerializeObject(customerService);
            }
        }
    }
}
