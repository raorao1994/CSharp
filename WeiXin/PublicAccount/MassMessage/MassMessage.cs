using System;
using System.Collections.Generic;
using System.Net;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 群发消息
    /// </summary>
    public static class MassMessage
    {
        /// <summary>
        /// 群发消息所用的http方法
        /// </summary>
        private const string httpMethod = WebRequestMethods.Http.Post;
        /// <summary>
        /// 根据分组群发消息的地址
        /// </summary>
        private const string urlForSendingAll = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}";
        /// <summary>
        /// 按OpenId列表群发消息的地址
        /// </summary>
        private const string urlForSending = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";
        /// <summary>
        /// 按OpenId列表群发消息的最大用户数
        /// </summary>
        private const int maxUserCount = 10000;
        /// <summary>
        /// 删除群发消息的地址
        /// </summary>
        private const string urlForDeleting = "https://api.weixin.qq.com/cgi-bin/message/mass/delete?access_token={0}";
        /// <summary>
        /// 预览群发消息的地址
        /// </summary>
        private const string urlForPreviewing = "https://api.weixin.qq.com/cgi-bin/message/mass/preview?access_token={0}";
        /// <summary>
        /// 查询群发消息发送状态的地址
        /// </summary>
        private const string urlForGettingStatus = "https://api.weixin.qq.com/cgi-bin/message/mass/get?access_token={0}";
        /// <summary>
        /// 消息发送状态为发送成功的提示
        /// </summary>
        private const string sendSuccess = "SEND_SUCCESS";

        /// <summary>
        /// 根据分组群发消息
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="isToAll">是否群发给所有用户</param>
        /// <param name="groupId">如果群发给所有用户，忽略该参数；否则群发给该组中的用户</param>
        /// <param name="messageType">群发消息类型</param>
        /// <param name="mediaIdOrContent">多媒体id或者文本内容</param>
        /// <param name="errorMessage">返回发送是否成功</param>
        /// <returns>如果发送成功，返回消息ID；否则，返回-1。</returns>
        public static long Send(string userName, bool isToAll, string groupId, MassMessageTypeEnum messageType, string mediaIdOrContent, out ErrorMessage errorMessage)
        {
            long messageId = -1;
            if (!isToAll && string.IsNullOrWhiteSpace(groupId))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "缺少分组ID。");
                return messageId;
            }
            if (string.IsNullOrWhiteSpace(mediaIdOrContent))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode,
                    string.Format("缺少{0}。", messageType == MassMessageTypeEnum.text ? "文本内容" : "媒体ID"));
                return messageId;
            }
            string json = GetMassMessageJsonString(isToAll, groupId, messageType, mediaIdOrContent);
            return Send(userName, urlForSendingAll, json, out errorMessage);
        }

        /// <summary>
        /// 根据OpenId列表群发消息
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="tousers">OpenId列表</param>
        /// <param name="messageType">群发消息类型</param>
        /// <param name="mediaIdOrContent">多媒体id或者文本内容</param>
        /// <param name="errorMessage">返回发送是否成功</param>
        /// <returns>如果发送成功，返回消息ID；否则，返回-1。</returns>
        public static long Send(string userName, IEnumerable<string> tousers, MassMessageTypeEnum messageType, string mediaIdOrContent, out ErrorMessage errorMessage)
        {
            long messageId = -1;
            if (tousers == null)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, string.Format("接收者不正确，数目必须大于0，小于等于{0}。", maxUserCount));
                return messageId;
            }
            List<string> touserList = new List<string>(tousers);
            if (touserList.Count == 0 || touserList.Count > maxUserCount)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, string.Format("接收者不正确，数目必须大于0，小于等于{0}。", maxUserCount));
                return messageId;
            }
            if (string.IsNullOrWhiteSpace(mediaIdOrContent))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode,
                    string.Format("缺少{0}。", messageType == MassMessageTypeEnum.text ? "文本内容" : "媒体ID"));
                return messageId;
            }
            string json = GetMassMessageJsonString(touserList.ToArray(), messageType, mediaIdOrContent);
            return Send(userName, urlForSending, json, out errorMessage);
        }

        /// <summary>
        /// 群发消息
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="urlFormat">包含格式的服务器地址</param>
        /// <param name="json">消息json字符串</param>
        /// <param name="errorMessage">返回发送是否成功</param>
        /// <returns>如果发送成功，返回消息id；否则，返回-1。</returns>
        private static long Send(string userName, string urlFormat, string json, out ErrorMessage errorMessage)
        {
            long messageId = -1;
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取许可令牌失败。");
                return messageId;
            }
            string url = string.Format(urlFormat, token.access_token);
            string responseContent;
            if (!HttpHelper.Request(url, out responseContent, httpMethod, json))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "提交数据到微信服务器失败。");
                return messageId;
            }
            JObject jo = JObject.Parse(responseContent);
            JToken jt;
            if (jo.TryGetValue("errcode", out jt) && jo.TryGetValue("errmsg", out jt))
            {
                errorMessage = new ErrorMessage((int)jo["errcode"], (string)jo["errmsg"]);
                if (jo.TryGetValue("msg_id", out jt))
                    messageId = (long)jt;
            }
            else
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "解析返回结果出错。");
            return messageId;
        }

        /// <summary>
        /// 获取群发消息的json字符串。
        /// </summary>
        /// <param name="isToAll">是否群发给所有用户</param>
        /// <param name="groupId">如果群发给所有用户，忽略该参数；否则群发给该组中的用户</param>
        /// <param name="messageType">群发消息类型</param>
        /// <param name="mediaIdOrContent">多媒体id或者文本内容</param>
        /// <returns>返回群发消息的json字符串。</returns>
        private static string GetMassMessageJsonString(bool isToAll, string groupId, MassMessageTypeEnum messageType, string mediaIdOrContent)
        {
            dynamic msg = new ExpandoObject();
            msg.filter = new
            {
                is_to_all = isToAll,
                group_id = isToAll ? string.Empty : groupId
            };
            if (messageType == MassMessageTypeEnum.text)
                ((IDictionary<string, object>)msg).Add(messageType.ToString(), new { content = mediaIdOrContent });
            else
                ((IDictionary<string, object>)msg).Add(messageType.ToString(), new { media_id = mediaIdOrContent });
            msg.msgtype = messageType.ToString();
            return JsonConvert.SerializeObject(msg);
        }

        /// <summary>
        /// 获取群发消息的json字符串。
        /// </summary>
        /// <param name="tousers">OpenId列表</param>
        /// <param name="messageType">群发消息类型</param>
        /// <param name="mediaIdOrContent">多媒体id或者文本内容</param>
        /// <returns>返回群发消息的json字符串。</returns>
        private static string GetMassMessageJsonString(string[] tousers, MassMessageTypeEnum messageType, string mediaIdOrContent)
        {
            dynamic msg = new ExpandoObject();
            msg.touser = tousers;
            if (messageType == MassMessageTypeEnum.text)
                ((IDictionary<string, object>)msg).Add(messageType.ToString(), new { content = mediaIdOrContent });
            else
                ((IDictionary<string, object>)msg).Add(messageType.ToString(), new { media_id = mediaIdOrContent });
            msg.msgtype = messageType.ToString();
            return JsonConvert.SerializeObject(msg);
        }

        /// <summary>
        /// 获取群发消息的json字符串。
        /// </summary>
        /// <param name="touser">OpenId</param>
        /// <param name="messageType">群发消息类型</param>
        /// <param name="mediaIdOrContent">多媒体id或者文本内容</param>
        /// <returns>返回群发消息的json字符串。</returns>
        private static string GetMassMessageJsonString(string touser, MassMessageTypeEnum messageType, string mediaIdOrContent)
        {
            dynamic msg = new ExpandoObject();
            msg.touser = touser;
            if (messageType == MassMessageTypeEnum.text)
                ((IDictionary<string, object>)msg).Add(messageType.ToString(), new { content = mediaIdOrContent });
            else
                ((IDictionary<string, object>)msg).Add(messageType.ToString(), new { media_id = mediaIdOrContent });
            msg.msgtype = messageType.ToString();
            return JsonConvert.SerializeObject(msg);
        }

        /// <summary>
        /// 删除群发消息。
        /// 注：只能删除图文消息和视频消息。
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="messageId">消息id</param>
        /// <returns>返回删除是否成功</returns>
        public static ErrorMessage Delete(string userName, long messageId)
        {
            string json = JsonConvert.SerializeObject(new { msg_id = messageId });
            return HttpHelper.RequestErrorMessage(urlForDeleting, userName, null, httpMethod, json);
        }

        /// <summary>
        /// 预览群发消息
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="touser">OpenId</param>
        /// <param name="messageType">群发消息类型</param>
        /// <param name="mediaIdOrContent">多媒体id或者文本内容</param>
        /// <param name="errorMessage">返回发送是否成功</param>
        /// <returns>如果发送成功，返回消息ID；否则，返回-1。</returns>
        public static long Preview(string userName, string touser, MassMessageTypeEnum messageType, string mediaIdOrContent, out ErrorMessage errorMessage)
        {
            long messageId = -1;
            if (string.IsNullOrWhiteSpace(mediaIdOrContent))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode,
                    string.Format("缺少{0}。", messageType == MassMessageTypeEnum.text ? "文本内容" : "媒体ID"));
                return messageId;
            }
            string json = GetMassMessageJsonString(touser, messageType, mediaIdOrContent);
            return Send(userName, urlForPreviewing, json, out errorMessage);
        }

        /// <summary>
        /// 查询群发消息的发送状态
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="messageId">消息id</param>
        /// <param name="errorMessage">返回查询是否成功</param>
        /// <returns>返回消息是否发送成功</returns>
        public static bool GetStatus(string userName, long messageId, out ErrorMessage errorMessage)
        {
            string json = JsonConvert.SerializeObject(new { msg_id = messageId });
            string responseContent = HttpHelper.RequestResponseContent(urlForGettingStatus, userName, null, httpMethod, json);
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "从微信服务器获取响应失败。");
                return false;
            }
            else
            {
                JObject jo = JObject.Parse(responseContent);
                JToken jt;
                if (jo.TryGetValue("msg_status", out jt))
                {
                    errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "查询群发消息发送状态成功。");
                    return (string)jt == sendSuccess;
                }
                else
                {
                    errorMessage = ErrorMessage.Parse(responseContent);
                    return false;
                }
            }
        }
    }
}