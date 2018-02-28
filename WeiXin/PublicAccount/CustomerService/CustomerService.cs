using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 客服消息
    /// </summary>
    public static class CustomerService
    {
        /// <summary>
        /// 发送客服消息的地址
        /// </summary>
        private const string urlForSendingMessage = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
        /// <summary>
        /// 发送客服消息的http方法
        /// </summary>
        private const string httpMethodForSendingMessage = WebRequestMethods.Http.Post;

        /// <summary>
        /// 添加客服账号的地址
        /// </summary>
        private const string urlForAdding = "https://api.weixin.qq.com/customservice/kfaccount/add?access_token={0}";
        /// <summary>
        /// 添加客服账号的http方法 
        /// </summary>
        private const string httpMethodForAdding = WebRequestMethods.Http.Post;
        /// <summary>
        /// 修改客服账号的地址
        /// </summary>
        private const string urlForUpdating = "https://api.weixin.qq.com/customservice/kfaccount/update?access_token={0}";
        /// <summary>
        /// 修改客服账号的http方法 
        /// </summary>
        private const string httpMethodForUpdating = WebRequestMethods.Http.Post;
        /// <summary>
        /// 删除客服账号的地址
        /// </summary>
        private const string urlForDeleting = "https://api.weixin.qq.com/customservice/kfaccount/del?access_token={0}&kf_account={1}";
        /// <summary>
        /// 删除客服账号的http方法 
        /// </summary>
        private const string httpMethodForDeleting = WebRequestMethods.Http.Get;

        /// <summary>
        /// 设置客服头像的地址
        /// </summary>
        private const string urlForUploadingHeadImg = "http://api.weixin.qq.com/customservice/kfaccount/uploadheadimg?access_token={0}&kf_account={1}";
        /// <summary>
        /// 设置客服头像的http方法
        /// </summary>
        private const string httpMethodForUploadingHeadImg = WebRequestMethods.Http.Post;

        /// <summary>
        /// 获取公众号中所有客服账号的地址
        /// </summary>
        private const string urlForGettingKfList = "https://api.weixin.qq.com/cgi-bin/customservice/getkflist?access_token={0}";
        /// <summary>
        /// 获取公众号中所有客服账号的http方法
        /// </summary>
        private const string httpMethodForGettingKfList = WebRequestMethods.Http.Get;
        /// <summary>
        /// 获取公众号中在线客服接待信息的地址
        /// </summary>
        private const string urlForGettingOnlineKfList = "https://api.weixin.qq.com/cgi-bin/customservice/getonlinekflist?access_token={0}";
        /// <summary>
        /// 获取公众号中在线客服接待信息的http方法
        /// </summary>
        private const string httpMethodForGettingOnlineKfList = WebRequestMethods.Http.Get;
        /// <summary>
        /// 获取公众号中客服聊天记录的地址
        /// </summary>
        private const string urlForGettingRecord = "https://api.weixin.qq.com/cgi-bin/customservice/getrecord?access_token={0}";
        /// <summary>
        /// 获取公众号中客服聊天记录的http方法
        /// </summary>
        private const string httpMethodForGettingRecord = WebRequestMethods.Http.Post;

        /// <summary>
        /// 创建会话的地址
        /// </summary>
        private const string urlForCreatingSession = "https://api.weixin.qq.com/customservice/kfsession/create?access_token={0}";
        /// <summary>
        /// 创建会话的http方法
        /// </summary>
        private const string httpMethodForCreatingSession = WebRequestMethods.Http.Post;
        /// <summary>
        /// 关闭会话的地址
        /// </summary>
        private const string urlForClosingSession = "https://api.weixin.qq.com/customservice/kfsession/close?access_token={0}";
        /// <summary>
        /// 关闭会话的http方法
        /// </summary>
        private const string httpMethodForClosingSession = WebRequestMethods.Http.Post;
        /// <summary>
        /// 获取客户会话状态的地址
        /// </summary>
        private const string urlForGettingSession = "https://api.weixin.qq.com/customservice/kfsession/getsession?access_token={0}&openid={1}";
        /// <summary>
        /// 获取客户会话状态的http方法
        /// </summary>
        private const string httpMethodForGettingSession = WebRequestMethods.Http.Get;
        /// <summary>
        /// 获取客服会话列表的地址
        /// </summary>
        private const string urlForGettingSessionList = "https://api.weixin.qq.com/customservice/kfsession/getsessionlist?access_token={0}&kf_account={1}";
        /// <summary>
        /// 获取客服会话列表的http方法
        /// </summary>
        private const string httpMethodForGettingSessionList = WebRequestMethods.Http.Get;
        /// <summary>
        /// 获取未接入会话列表的地址
        /// </summary>
        private const string urlForGettingWaitCase = "https://api.weixin.qq.com/customservice/kfsession/getwaitcase?access_token={0}";
        /// <summary>
        /// 获取未接入会话列表的http方法
        /// </summary>
        private const string httpMethodForGettingWaitCase = WebRequestMethods.Http.Get;

        /// <summary>
        /// 检查客服账号是否正确
        /// </summary>
        /// <param name="kfAccount">客服账号</param>
        /// <returns>返回客服账号是否正确</returns>
        private static bool CheckKfAccount(string kfAccount)
        {
            if (string.IsNullOrWhiteSpace(kfAccount))
                return false;
            string[] arr = kfAccount.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length != 2)
                return false;
            if (arr[0].Length > 10)
                return false;
            foreach (char c in arr[0])
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="responseMessage">响应消息</param>
        /// <param name="kfAccount">客服账号</param>
        /// <returns>返回发送是否成功</returns>
        public static ErrorMessage SendMessage(ResponseBaseMessage responseMessage, string kfAccount = "")
        {
            if (responseMessage == null)
                return new ErrorMessage(ErrorMessage.ExceptionCode, "响应消息不能为null。");
            if (!string.IsNullOrEmpty(kfAccount) && !CheckKfAccount(kfAccount))
                return new ErrorMessage(ErrorMessage.ExceptionCode, "客服账号格式不正确。");
            string json = responseMessage.ToJson();
            if (!string.IsNullOrWhiteSpace(kfAccount))
            {
                string kfInfo = string.Format(",\"customservice\":{0}}",
                    JsonConvert.SerializeObject(new { kf_account = kfAccount }));
                json = json.Remove(json.Length - 1) + kfInfo;
            }
            return HttpHelper.RequestErrorMessage(urlForSendingMessage, responseMessage.FromUserName, null, httpMethodForSendingMessage, json);
        }

        /// <summary>
        /// 添加客服账号
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="kfAccount">客服账号</param>
        /// <param name="nickname">客服昵称</param>
        /// <param name="password">客服密码（原始密码，未抽样）</param>
        /// <returns>返回添加客服账号是否成功</returns>
        public static ErrorMessage Add(string userName, string kfAccount, string nickname, string password)
        {
            return Edit(urlForAdding, httpMethodForAdding, userName, kfAccount, nickname, password);
        }

        /// <summary>
        /// 修改客服账号
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="kfAccount">客服账号</param>
        /// <param name="nickname">客服昵称</param>
        /// <param name="password">客服密码（原始密码，未抽样）</param>
        /// <returns>返回修改客服账号是否成功</returns>
        public static ErrorMessage Update(string userName, string kfAccount, string nickname, string password)
        {
            return Edit(urlForUpdating, httpMethodForUpdating, userName, kfAccount, nickname, password);
        }

        /// <summary>
        /// 编辑（添加、修改）客服账号
        /// </summary>
        /// <param name="urlForEditing">编辑客服账号的地址</param>
        /// <param name="httpMethod">编辑客服账号的http方法</param>
        /// <param name="userName">公众号</param>
        /// <param name="kfAccount">客服账号</param>
        /// <param name="nickname">客服昵称</param>
        /// <param name="password">客服密码（原始密码，未抽样）</param>
        /// <returns>返回编辑是否成功</returns>
        private static ErrorMessage Edit(string urlForEditing, string httpMethod,
            string userName, string kfAccount, string nickname, string password)
        {
            ErrorMessage errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "");
            if (!CheckKfAccount(kfAccount))
            {
                errorMessage.errmsg = "客服账号不正确。";
                return errorMessage;
            }
            if (string.IsNullOrWhiteSpace(nickname))
            {
                errorMessage.errmsg = "客服昵称不能为空。";
                return errorMessage;
            }
            string json = GetKfAccountJsonString(kfAccount, nickname, password);
            return HttpHelper.RequestErrorMessage(urlForEditing, userName, null, httpMethod, json);
        }

        /// <summary>
        /// 删除客服账号
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="kfAccount">客服账号</param>
        /// <param name="nickname">客服昵称</param>
        /// <param name="password">客服密码（原始密码，未抽样）</param>
        /// <returns>返回删除客服账号是否成功</returns>
        public static ErrorMessage Delete(string userName, string kfAccount)
        {
            if (!CheckKfAccount(kfAccount))
                return new ErrorMessage(ErrorMessage.ExceptionCode, "客服账号不正确。");
            return HttpHelper.RequestErrorMessage(urlForDeleting, userName, new object[] { kfAccount }, httpMethodForDeleting, null);
        }

        /// <summary>
        /// 获取客服账号json字符串
        /// </summary>
        /// <param name="kfAccount">客服账号</param>
        /// <param name="nickname">客服昵称</param>
        /// <param name="password">客服密码（未抽样）</param>
        /// <returns>返回客服账号json字符串</returns>
        private static string GetKfAccountJsonString(string kfAccount, string nickname, string password)
        {
            string md5Password;
            if (string.IsNullOrEmpty(password))
                md5Password = string.Empty;
            else if (!PublicLibrary.Utility.MD5(password, out md5Password))
                md5Password = password;
            var account = new
            {
                kf_account = kfAccount,
                nickname = nickname,
                password = md5Password
            };
            return JsonConvert.SerializeObject(account);
        }

        /// <summary>
        /// 设置（上传）客服头像
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="kfAccount">客服账号</param>
        /// <param name="imagePathName">图像全路径文件名</param>
        /// <returns>返回设置是否成功</returns>
        public static ErrorMessage UploadHeadImage(string userName, string kfAccount, string imagePathName)
        {
            byte[] data = null;
            FileStream fs = null;
            MemoryStream ms = null;
            try
            {
                fs = new FileStream(imagePathName, FileMode.Open, FileAccess.Read);
                ms = new MemoryStream();
                int bufferLength = 2048;
                byte[] buffer = new byte[bufferLength];
                int size = fs.Read(buffer, 0, bufferLength);
                while (size > 0)
                {
                    ms.Write(buffer, 0, size);
                    size = fs.Read(buffer, 0, bufferLength);
                }
                data = ms.ToArray();
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                if (ms != null)
                    ms.Close();
            }
            return UploadHeadImage(userName, kfAccount, data);
        }

        /// <summary>
        /// 设置（上传）客服头像
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="kfAccount">客服账号</param>
        /// <param name="imageData">图像数据</param>
        /// <returns>返回设置是否成功</returns>
        /// <returns></returns>
        public static ErrorMessage UploadHeadImage(string userName, string kfAccount, byte[] imageData)
        {
            ErrorMessage errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "");
            if (!CheckKfAccount(kfAccount))
            {
                errorMessage.errmsg = "客服账号不正确。";
                return errorMessage;
            }
            if (imageData == null || imageData.Length == 0)
            {
                errorMessage.errmsg = "没有图像数据。";
                return errorMessage;
            }
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
            {
                errorMessage.errmsg = "获取许可令牌失败。";
                return errorMessage;
            }
            string url = string.Format(urlForUploadingHeadImg, token.access_token, kfAccount);
            string responseContent;
            if (!HttpHelper.Request(url, out responseContent, httpMethodForUploadingHeadImg, imageData))
            {
                errorMessage.errmsg = "从微信服务器获取响应失败。";
                return errorMessage;
            }
            if (ErrorMessage.TryParse(responseContent, out errorMessage))
                return errorMessage;
            else
                return new ErrorMessage(ErrorMessage.ExceptionCode, "解析返回结果失败。");
        }

        /// <summary>
        /// 获取公众号中的完整客服账号列表
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回客服账号列表；如果获取失败，返回null。</returns>
        public static List<CustomerServiceAccount> GetKfList(string userName, out ErrorMessage errorMessage)
        {
            List<CustomerServiceAccount> accounts = null;
            string responseContent = HttpHelper.RequestResponseContent(urlForGettingKfList, userName, null, httpMethodForGettingKfList, null);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                var kf_list = new
                {
                    kf_list = new List<CustomerServiceAccount>()
                };
                var kfList = JsonConvert.DeserializeAnonymousType(responseContent, kf_list);
                accounts = kfList.kf_list;
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
            }
            return accounts;
        }

        /// <summary>
        /// 获取公众号中的在线客服接待信息列表
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回在线客服接待信息列表；如果获取失败，返回null。</returns>
        public static List<CustomerServiceOnlineInfo> GetOnlineKfList(string userName, out ErrorMessage errorMessage)
        {
            List<CustomerServiceOnlineInfo> infos = null;
            string responseContent = HttpHelper.RequestResponseContent(urlForGettingOnlineKfList, userName, null, httpMethodForGettingOnlineKfList, null);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                var kf_online_list = new
                {
                    kf_online_list = new List<CustomerServiceOnlineInfo>()
                };
                var kfOnlineList = JsonConvert.DeserializeAnonymousType(responseContent, kf_online_list);
                infos = kfOnlineList.kf_online_list;
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "获取在线客服接待信息成功。");
            }
            return infos;
        }

        /// <summary>
        /// 获取公众号中的客服聊天记录列表
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="openId">普通用户的标识</param>
        /// <param name="startTime">查询开始时间</param>
        /// <param name="endTime">查询结束时间</param>
        /// <param name="pageSize">每页大小，每页最多拉取1000条</param>
        /// <param name="pageIndex">查询第几页，从1开始</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回在线客服接待信息列表；如果获取失败，返回null。</returns>
        public static List<CustomerServiceRecord> GetRecord(string userName, string openId, DateTime startTime, DateTime endTime, int pageSize, int pageIndex, out ErrorMessage errorMessage)
        {
            List<CustomerServiceRecord> records = null;
            errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "");
            if (startTime > endTime)
            {
                errorMessage.errmsg = "查询开始时间不能大于查询结束时间。";
                return null;
            }
            if (pageSize <= 0 || pageSize > 1000)
            {
                errorMessage.errmsg = "每页大小错误。";
                return null;
            }
            if (pageIndex < 1)
            {
                errorMessage.errmsg = "查询页码错误。";
                return null;
            }
            var postData = new
            {
                starttime = Utility.ToWeixinTime(startTime),
                endtime = Utility.ToWeixinTime(endTime),
                openid = openId ?? string.Empty,
                pagesize = pageSize,
                pageindex = pageIndex
            };
            string json = JsonConvert.SerializeObject(postData);
            string responseContent = HttpHelper.RequestResponseContent(urlForGettingRecord, userName, null, json);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage.errmsg = "从微信服务器获取响应失败。";
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                var recordlist = new
                {
                    recordlist = new List<CustomerServiceRecord>()
                };
                var recordList = JsonConvert.DeserializeAnonymousType(responseContent, recordlist);
                records = recordlist.recordlist;
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
            }
            return records;
        }

        /// <summary>
        /// 创建会话
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="openId">普通用户的标识</param>
        /// <param name="kfAccount">客服账号</param>
        /// <param name="text">附加消息</param>
        /// <returns>返回创建会话是否成功</returns>
        public static ErrorMessage Create(string userName, string openId, string kfAccount, string text = "")
        {
            ErrorMessage errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "");
            if (string.IsNullOrWhiteSpace(openId))
            {
                errorMessage.errmsg = "用户标识不能为空。";
                return errorMessage;
            }
            if (!CheckKfAccount(kfAccount))
            {
                errorMessage.errmsg = "客服账号不正确。";
                return errorMessage;
            }
            string json = JsonConvert.SerializeObject(new { openid = openId, kf_account = kfAccount, text = text });
            return HttpHelper.RequestErrorMessage(urlForCreatingSession, userName, null, httpMethodForCreatingSession, json);
        }

        /// <summary>
        /// 关闭会话
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="openId">普通用户的标识</param>
        /// <param name="kfAccount">客服账号</param>
        /// <param name="text">附加消息</param>
        /// <returns>返回创建会话是否成功</returns>
        public static ErrorMessage Close(string userName, string openId, string kfAccount, string text = "")
        {
            ErrorMessage errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "");
            if (string.IsNullOrWhiteSpace(openId))
            {
                errorMessage.errmsg = "用户标识不能为空。";
                return errorMessage;
            }
            if (!CheckKfAccount(kfAccount))
            {
                errorMessage.errmsg = "客服账号不正确。";
                return errorMessage;
            }
            string json = JsonConvert.SerializeObject(new { openid = openId, kf_account = kfAccount, text = text });
            return HttpHelper.RequestErrorMessage(urlForClosingSession, userName, null, httpMethodForClosingSession, json);
        }

        /// <summary>
        /// 获取客户的会话状态
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="openId">客户账号</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回客户的会话状态</returns>
        public static CustomerSession GetSession(string userName, string openId, out ErrorMessage errorMessage)
        {
            return HttpHelper.RequestParsableResult<CustomerSession>(urlForGettingSession, userName, out errorMessage,
                new object[] { openId }, httpMethodForGettingSession, null);
        }

        /// <summary>
        /// 获取客服的会话列表
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="kfAccount">客服账号</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回客服的会话列表</returns>
        public static CustomerServiceSession[] GetSessionList(string userName, string kfAccount, out ErrorMessage errorMessage)
        {
            string responseContent = HttpHelper.RequestResponseContent(urlForGettingSessionList, userName, new object[] { kfAccount },
                httpMethodForGettingSessionList, null);
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
                return null;
            }
            else if (ErrorMessage.IsErrorMessage(responseContent))
            {
                errorMessage = ErrorMessage.Parse(responseContent);
                return null;
            }
            else
            {
                JObject jo = JObject.Parse(responseContent);
                JToken jt;
                if (jo.TryGetValue("sessionlist", out jt) && jt.Type == JTokenType.Array)
                {
                    JArray ja = (JArray)jt;
                    CustomerServiceSession[] sessions = new CustomerServiceSession[ja.Count];
                    for (int i = 0; i < ja.Count; i++)
                    {
                        sessions[i] = new CustomerServiceSession();
                        sessions[i].Parse((JObject)ja[i]);
                    }
                    errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
                    return sessions;
                }
                else
                {
                    errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "解析结果失败。");
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取未接入的会话列表
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回未接入的会话列表</returns>
        public static WaitCaseSession GetWaitCase(string userName, out ErrorMessage errorMessage)
        {
            return HttpHelper.RequestParsableResult<WaitCaseSession>(urlForGettingWaitCase, userName, out errorMessage,
                null, httpMethodForGettingWaitCase, null);
        }
    }
}