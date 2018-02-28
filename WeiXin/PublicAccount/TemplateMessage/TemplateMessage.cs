using System;
using System.Net;
using System.Drawing;
using System.Dynamic;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 模板消息
    /// </summary>
    public static class TemplateMessage
    {
        /// <summary>
        /// 模板消息相关的http方法（获取模板列表除外）
        /// </summary>
        private const string httpMethod = WebRequestMethods.Http.Post;
        /// <summary>
        /// 设置行业的地址
        /// </summary>
        private const string urlForSettingIndustry = "https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token={0}";
        /// <summary>
        /// 获取模板ID的地址
        /// </summary>
        private const string urlForGettingId = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={0}";
        /// <summary>
        /// 发送模板消息的地址
        /// </summary>
        private const string urlForSending = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        /// <summary>
        /// 获取所有已添加至账号下所有模板列表的地址
        /// </summary>
        private const string urlForGettingAllPrivateTemplates = "https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token={0}";
        /// <summary>
        /// 获取所有已添加至账号下所有模板列表的http方法
        /// </summary>
        private const string httpMethodForGettingAllPrivateTemplates = WebRequestMethods.Http.Get;
        /// <summary>
        /// 删除模板的地址
        /// </summary>
        private const string urlForDeletingPrivateTemplate = "https://api.weixin.qq.com/cgi-bin/template/del_private_template?access_token={0}";

        /// <summary>
        /// 设置行业
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="code1">行业代码1</param>
        /// <param name="code2">行业代码2</param>
        /// <returns>返回设置是否成功</returns>
        public static ErrorMessage SetIndustry(string userName, string code1, string code2)
        {
            if (string.IsNullOrWhiteSpace(code1) || string.IsNullOrWhiteSpace(code2))
                return new ErrorMessage(ErrorMessage.ExceptionCode, "行业代码不能为空。");
            string json = JsonConvert.SerializeObject(new { industry_id1 = code1, industry_id2 = code2 });
            return HttpHelper.RequestErrorMessage(urlForSettingIndustry, userName, null, httpMethod, json);
        }

        /// <summary>
        /// 设置行业
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="industry1">行业1</param>
        /// <param name="industry2">行业2</param>
        /// <returns>返回设置是否成功</returns>
        public static ErrorMessage SetIndustry(string userName, Industry industry1, Industry industry2)
        {
            if (industry1 == null || industry2 == null)
                return new ErrorMessage(ErrorMessage.ExceptionCode, "行业不能为空。");
            return SetIndustry(userName, industry1.Code, industry2.Code);
        }

        /// <summary>
        /// 获取模板ID
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="shortTemplateId">模板库中模板的编号，有“TM**”和“OPENTMTM**”等形式</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回模板ID；如果获取失败，返回空字符串。</returns>
        public static string GetId(string userName, string shortTemplateId, out ErrorMessage errorMessage)
        {
            string id = string.Empty;
            if (string.IsNullOrWhiteSpace(shortTemplateId))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "模板库中的模板编号不能为空。");
                return id;
            }
            string json = JsonConvert.SerializeObject(new { template_id_short = shortTemplateId });
            string responseContent = HttpHelper.RequestResponseContent(urlForGettingId, userName, null, httpMethod, json);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取模板id失败。");
            else
            {
                JObject jo = JObject.Parse(responseContent);
                JToken jt;
                if (jo.TryGetValue("errcode", out jt) && jo.TryGetValue("errmsg", out jt))
                {
                    errorMessage = new ErrorMessage((int)jo["errcode"], (string)jo["errmsg"]);
                    if (jo.TryGetValue("template_id", out jt))
                        id = (string)jt;
                }
                else
                    errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "解析返回结果失败。");
            }
            return id;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="touser">接收消息的账号</param>
        /// <param name="templateId">模板id</param>
        /// <param name="detailUrl">详情地址</param>
        /// <param name="topColor">顶端颜色</param>
        /// <param name="data">数据</param>
        /// <param name="errorMessage">返回发送是否成功</param>
        /// <returns>返回消息id；如果发送失败，返回-1。</returns>
        public static long Send(string userName, string touser, string templateId, string detailUrl, Color topColor,
            Tuple<string, string, Color>[] data, out ErrorMessage errorMessage)
        {
            errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "");
            long id = -1;
            //校验参数
            if (string.IsNullOrWhiteSpace(touser))
            {
                errorMessage.errmsg = "接收消息的账号不能为空。";
                return id;
            }
            if (string.IsNullOrWhiteSpace(templateId))
            {
                errorMessage.errmsg = "模板id不能为空。";
                return id;
            }
            if (data == null || data.Length == 0)
            {
                errorMessage.errmsg = "模板数据不能为空。";
                return id;
            }
            foreach (Tuple<string, string, Color> item in data)
            {
                if (string.IsNullOrWhiteSpace(item.Item1) || string.IsNullOrWhiteSpace(item.Item2))
                {
                    errorMessage.errmsg = "模板数据不能为空。";
                    return id;
                }
            }
            //获取许可令牌
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
            {
                errorMessage.errmsg = "获取许可令牌失败。";
                return id;
            }
            string url = string.Format(urlForSending, token.access_token);
            //生成待发送的数据
            dynamic postData = new ExpandoObject();
            postData.touser = touser;
            postData.template_id = templateId;
            postData.url = detailUrl ?? string.Empty;
            postData.topcolor = Utility.GetColorString(topColor);
            postData.data = new ExpandoObject();
            IDictionary<string, object> dataDict = (IDictionary<string, object>)postData.data;
            foreach (Tuple<string, string, Color> item in data)
            {
                dataDict.Add(item.Item1, new { value = item.Item2, color = Utility.GetColorString(item.Item3) });
            }
            string json = JsonConvert.SerializeObject(postData);
            //发送数据
            string responseContent;
            if (!HttpHelper.Request(url, out responseContent, httpMethod, json))
            {
                errorMessage.errmsg = "提交数据到微信服务器失败。";
                return id;
            }
            //解析结果
            JObject jo = JObject.Parse(responseContent);
            JToken jt;
            if (jo.TryGetValue("errcode", out jt) && jo.TryGetValue("errmsg", out jt))
            {
                errorMessage.errcode = (int)jo["errcode"];
                errorMessage.errmsg = (string)jo["errmsg"];
                if (jo.TryGetValue("msgid", out jt))
                    id = (long)jt;
            }
            else
                errorMessage.errmsg = "解析返回结果失败。";
            return id;
        }

        /// <summary>
        /// 获取已添加至账号下的模板列表。
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="errorMessage">返回发送是否成功</param>
        /// <returns>返回模板数组；如果获取失败，返回null。</returns>
        public static Template[] GetAllPrivateTemplates(string userName, out ErrorMessage errorMessage)
        {
            errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "");
            //获取许可令牌
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
            {
                errorMessage.errmsg = "获取许可令牌失败。";
                return null;
            }
            string url = string.Format(urlForGettingAllPrivateTemplates, token.access_token);
            //获取模板
            string responseContent;
            if (!HttpHelper.Request(url, out responseContent, httpMethodForGettingAllPrivateTemplates, (string)null))
            {
                errorMessage.errmsg = "提交数据到微信服务器失败。";
                return null;
            }
            //解析结果
            Template[] templates;
            errorMessage = Template.Parse(responseContent, out templates);
            return templates;
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="templateId">模板id</param>
        /// <param name="errorMessage">返回删除是否成功</param>
        /// <returns>返回删除是否成功。</returns>
        public static bool Delete(string userName, string templateId, out ErrorMessage errorMessage)
        {
            errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "");
            bool success = false;
            //校验参数
            if (string.IsNullOrWhiteSpace(templateId))
            {
                errorMessage.errmsg = "模板id不能为空。";
                return success;
            }
            //获取许可令牌
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
            {
                errorMessage.errmsg = "获取许可令牌失败。";
                return success;
            }
            string url = string.Format(urlForDeletingPrivateTemplate, token.access_token);
            //生成待发送的数据
            dynamic postData = new ExpandoObject();
            postData.template_id = templateId;
            string json = JsonConvert.SerializeObject(postData);
            //发送数据
            string responseContent;
            if (!HttpHelper.Request(url, out responseContent, httpMethod, json))
            {
                errorMessage.errmsg = "提交数据到微信服务器失败。";
                return success;
            }
            //解析结果
            if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
                errorMessage.errmsg = "解析返回结果失败。";
            return errorMessage.IsSuccess;
        }
    }
}