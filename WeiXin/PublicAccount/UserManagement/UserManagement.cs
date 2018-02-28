using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public static class UserManagement
    {
        /// <summary>
        /// 创建用户分组的地址
        /// </summary>
        private const string urlForCreatingGroup = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token={0}";
        /// <summary>
        /// 创建用户分组的http方法
        /// </summary>
        private const string httpMethodForCreatingGroup = WebRequestMethods.Http.Post;
        /// <summary>
        /// 获取用户分组的地址
        /// </summary>
        private const string urlForGettingGroup = "https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}";
        /// <summary>
        /// 获取用户分组的http方法
        /// </summary>
        private const string httpMethodForGettingGroup = WebRequestMethods.Http.Get;
        /// <summary>
        /// 获取用户分组id的地址
        /// </summary>
        private const string urlForGettingGroupId = "https://api.weixin.qq.com/cgi-bin/groups/getid?access_token={0}";
        /// <summary>
        /// 获取用户分组id的http方法
        /// </summary>
        private const string httpMethodForGettingGroupId = WebRequestMethods.Http.Post;
        /// <summary>
        /// 修改用户分组名称的地址
        /// </summary>
        private const string urlForChangingGroupName = "https://api.weixin.qq.com/cgi-bin/groups/update?access_token={0}";
        /// <summary>
        /// 修改用户分组名称的http方法
        /// </summary>
        private const string httpMethodForChangingGroupName = WebRequestMethods.Http.Post;
        /// <summary>
        /// 移动用户分组的地址
        /// </summary>
        private const string urlForMovingUser = "https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token={0}";
        /// <summary>
        /// 移动用户分组的http方法
        /// </summary>
        private const string httpMethodForMovingUser = WebRequestMethods.Http.Post;
        /// <summary>
        /// 批量移动用户分组的地址
        /// </summary>
        private const string urlForMovingUsers = "https://api.weixin.qq.com/cgi-bin/groups/members/batchupdate?access_token={0}";
        /// <summary>
        /// 批量移动用户分组的http方法
        /// </summary>
        private const string httpMethodForMovingUsers = WebRequestMethods.Http.Post;
        /// <summary>
        /// 修改用户备注的地址
        /// </summary>
        private const string urlForChangingUserRemark = "https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token={0}";
        /// <summary>
        /// 修改用户备注的http方法
        /// </summary>
        private const string httpMethodForChangingUserRemark = WebRequestMethods.Http.Post;
        /// <summary>
        /// 获取用户基本信息的地址
        /// </summary>
        private const string urlForGettingUserInfo = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang={2}";
        /// <summary>
        /// 获取用户基本信息的http方法
        /// </summary>
        private const string httpMethodForGettingUserInfo = WebRequestMethods.Http.Get;
        /// <summary>
        /// 获取用户列表的地址
        /// </summary>
        private const string urlForGettingUserList = "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}";
        /// <summary>
        /// 获取用户列表的http方法
        /// </summary>
        private const string httpMethodForGettingUserList = WebRequestMethods.Http.Get;

        /// <summary>
        /// 创建用户分组
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="groupName">分组名称</param>
        /// <param name="errorMessage">返回是否创建成功</param>
        /// <returns>返回分组id；如果创建失败，返回-1。</returns>
        public static int CreateGroup(string userName, string groupName, out ErrorMessage errorMessage)
        {
            int id = -1;
            if (string.IsNullOrWhiteSpace(groupName))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "分组名称不能为空。");
                return id;
            }
            string json = JsonConvert.SerializeObject(new { group = new { name = groupName } });
            string responseContent = HttpHelper.RequestResponseContent(urlForCreatingGroup, userName, null, httpMethodForCreatingGroup, json);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                JObject jo = JObject.Parse(responseContent);
                id = (int)jo["group"]["id"];
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
            }
            return id;
        }

        /// <summary>
        /// 获取用户分组
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="errorMessage">返回是否获取成功</param>
        /// <returns>返回用户分组；如果获取失败，返回null。</returns>
        public static UserGroup[] GetGroup(string userName, out ErrorMessage errorMessage)
        {
            UserGroup[] groups = null;
            string responseContent = HttpHelper.RequestResponseContent(urlForGettingGroup, userName, null, httpMethodForGettingGroup, null);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
                var result = JsonConvert.DeserializeAnonymousType(responseContent, new { groups = new UserGroup[1] });
                groups = result.groups;
            }
            return groups;
        }

        /// <summary>
        /// 查询用户所在分组
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="openId">用户id</param>
        /// <param name="errorMessage">返回查询是否成功</param>
        /// <returns>返回用户所在的分组id；如果查询失败，返回-1。</returns>
        public static int GetGroupId(string userName, string openId, out ErrorMessage errorMessage)
        {
            int id = -1;
            if (string.IsNullOrWhiteSpace(openId))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "用户id不能为空。");
                return id;
            }
            string json = JsonConvert.SerializeObject(new { openid = openId });
            string responseContent = HttpHelper.RequestResponseContent(urlForGettingGroupId, userName, null, httpMethodForGettingGroupId, json);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
                var result = JsonConvert.DeserializeAnonymousType(responseContent, new { groupid = 0 });
                id = result.groupid;
            }
            return id;
        }

        /// <summary>
        /// 修改用户分组名称
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="groupId">分组id</param>
        /// <param name="groupName">分组名称</param>
        /// <returns>返回修改是否成功</returns>
        public static ErrorMessage ChangeGroupName(string userName, int groupId, string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                return new ErrorMessage(ErrorMessage.ExceptionCode, "分组名称不能为空。");
            string json = JsonConvert.SerializeObject(new { group = new { id = groupId, name = groupName } });
            return HttpHelper.RequestErrorMessage(urlForChangingGroupName, userName, null, httpMethodForChangingGroupName, json);
        }

        /// <summary>
        /// 移动用户到指定的分组中
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="groupId">分组id</param>
        /// <param name="openIds">用户id</param>
        /// <returns>返回移动是否成功</returns>
        public static ErrorMessage MoveUser(string userName, int groupId, params string[] openIds)
        {
            if (openIds == null || openIds.Length == 0)
                return new ErrorMessage(ErrorMessage.ExceptionCode, "至少需要移动一个用户。");
            string json;
            if (openIds.Length == 1)
            {
                json = JsonConvert.SerializeObject(new { openid = openIds[0], to_groupid = groupId });
                return HttpHelper.RequestErrorMessage(urlForMovingUser, userName, null, httpMethodForMovingUser, json);
            }
            else
            {
                json = JsonConvert.SerializeObject(new { openid_list = openIds, to_groupid = groupId });
                return HttpHelper.RequestErrorMessage(urlForMovingUsers, userName, null, httpMethodForMovingUsers, json);
            }
        }

        /// <summary>
        /// 设置用户备注
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="openId">用户id</param>
        /// <param name="remark">备注</param>
        /// <returns>返回设置是否成功</returns>
        public static ErrorMessage ChangeUserRemark(string userName, string openId, string remark)
        {
            if (string.IsNullOrWhiteSpace(openId))
                return new ErrorMessage(ErrorMessage.ExceptionCode, "用户id不能为空。");
            string json = JsonConvert.SerializeObject(new { openid = openId, remark = remark });
            return HttpHelper.RequestErrorMessage(urlForChangingUserRemark, userName, null, httpMethodForChangingUserRemark, json);
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="openId">用户id</param>
        /// <param name="language">语言</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回用户基本信息；如果获取失败，返回null。</returns>
        public static UserInfo GetUserInfo(string userName, string openId, LanguageEnum language, out ErrorMessage errorMessage)
        {
            return GetUserInfo(userName, openId, language.ToString("g"), out errorMessage);
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="openId">用户id</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回用户基本信息；如果获取失败，返回null。</returns>
        public static UserInfo GetUserInfo(string userName, string openId, out ErrorMessage errorMessage)
        {
            return GetUserInfo(userName, openId, string.Empty, out errorMessage);
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="openId">用户id</param>
        /// <param name="language">语言</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回用户基本信息；如果获取失败，返回null。</returns>
        private static UserInfo GetUserInfo(string userName, string openId, string language, out ErrorMessage errorMessage)
        {
            UserInfo info = null;
            string responseContent = HttpHelper.RequestResponseContent(urlForGettingUserInfo, userName,
                new object[] { openId, language }, httpMethodForGettingUserInfo, null);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                JObject jo = JObject.Parse(responseContent);
                JToken jt;
                if (jo.TryGetValue("subscribe", out jt) && jo.TryGetValue("openid", out jt) && (string)jt == openId)
                {
                    int subscribe = (int)jo["subscribe"];
                    string nickname = jo.TryGetValue("nickname", out jt) ? (string)jt : string.Empty;
                    int sex = jo.TryGetValue("sex", out jt) ? (int)jt : (int)SexEnum.Unknown;
                    string lang = jo.TryGetValue("language", out jt) ? (string)jt : string.Empty;
                    string city = jo.TryGetValue("city", out jt) ? (string)jt : string.Empty;
                    string province = jo.TryGetValue("province", out jt) ? (string)jt : string.Empty;
                    string country = jo.TryGetValue("country", out jt) ? (string)jt : string.Empty;
                    string headimgurl = jo.TryGetValue("headimgurl", out jt) ? (string)jt : string.Empty;
                    int subscribe_time = jo.TryGetValue("subscribe_time", out jt) ? (int)jt : 0;
                    string unionid = jo.TryGetValue("unionid", out jt) ? (string)jt : string.Empty;
                    info = new UserInfo(subscribe, openId, nickname, sex, lang, city, province, country, headimgurl, subscribe_time, unionid);
                    errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
                }
                else
                    errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取用户基本信息失败。");
            }
            return info;
        }

        /// <summary>
        /// 获取OAuth用户基本信息
        /// </summary>
        /// <param name="accessToken">网页access token</param>
        /// <param name="openId">用户id</param>
        /// <param name="language">语言</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回用户基本信息；如果获取失败，返回null。</returns>
        public static UserInfo GetOAuthUserInfo(string accessToken, string openId, LanguageEnum language, out ErrorMessage errorMessage)
        {
            return OAuthAccessToken.GetUserInfo(accessToken, openId, language, out errorMessage);
        }

        /// <summary>
        /// 获取OAuth用户基本信息
        /// </summary>
        /// <param name="accessToken">网页access token</param>
        /// <param name="openId">用户id</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回用户基本信息；如果获取失败，返回null。</returns>
        public static UserInfo GetOAuthUserInfo(string accessToken, string openId, out ErrorMessage errorMessage)
        {
            return OAuthAccessToken.GetUserInfo(accessToken, openId, out errorMessage);
        }

        /// <summary>
        /// 获取用户的unionid
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="openId">用户id</param>
        /// <returns>返回用户的unionid；如果获取失败，返回空字符串。</returns>
        public static string GetUnionId(string userName, string openId)
        {
            ErrorMessage errorMessage;
            UserInfo info = GetUserInfo(userName, openId, out errorMessage);
            return errorMessage.IsSuccess && info != null ? info.unionid : string.Empty;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="nextOpenId">第一个拉取的OPENID，不填默认从头开始拉取</param>
        /// <param name="total">返回关注该公众账号的总用户数；如果获取失败，返回-1。</param>
        /// <param name="newNextOpenId">返回获取用户列表后，后一个用户的id</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回这次获取的用户列表；如果获取失败，返回null。</returns>
        public static string[] GetUserList(string userName, string nextOpenId,
            out int total, out string newNextOpenId, out ErrorMessage errorMessage)
        {
            string[] users = null;
            total = -1;
            newNextOpenId = string.Empty;
            if (nextOpenId == null)
                nextOpenId = string.Empty;
            string responseContent = HttpHelper.RequestResponseContent(urlForGettingUserList, userName,
                new object[] { nextOpenId }, httpMethodForGettingUserList, null);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                var result = JsonConvert.DeserializeAnonymousType(responseContent,
                    new { total = 2, count = 2, data = new { openid = new string[1] }, next_openid = "NEXT_OPENID" });
                if (result.count > 0 && result.data != null && result.data.openid != null && result.data.openid.Length > 0)
                    users = result.data.openid;
                total = result.total;
                newNextOpenId = result.next_openid;
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
            }
            return users;
        }

        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <returns>返回所有的用户列表</returns>
        public static string[] GetUserList(string userName)
        {
            List<string> allUsers = null;
            string[] users;
            ErrorMessage errorMessage;
            int total;
            string nextOpenId = string.Empty;
            string newNextOpenId;
            do
            {
                users = GetUserList(userName, nextOpenId, out total, out newNextOpenId, out errorMessage);
                if (errorMessage.IsSuccess && users != null && users.Length > 0)
                {
                    if (allUsers == null)
                        allUsers = new List<string>(total);
                    allUsers.AddRange(users);
                }
                nextOpenId = newNextOpenId;
            } while (errorMessage.IsSuccess && users != null && users.Length > 0 && !string.IsNullOrWhiteSpace(nextOpenId));
            return allUsers != null ? allUsers.ToArray() : null;
        }
    }
}
