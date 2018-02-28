using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 模板
    /// </summary>
    public class Template : IParsable
    {
        /// <summary>
        /// 模板id
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 一级行业
        /// </summary>
        public string PrimaryIndustry { get; set; }
        /// <summary>
        /// 二级行业
        /// </summary>
        public string DeputyIndustry { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 示例
        /// </summary>
        public string Example { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="templateId">模板id</param>
        /// <param name="title">标题</param>
        /// <param name="primaryIndustry">一级行业</param>
        /// <param name="deputyIndustry">二级行业</param>
        /// <param name="content">内容</param>
        /// <param name="example">示例</param>
        public Template(string templateId, string title, string primaryIndustry, string deputyIndustry,
            string content, string example)
        {
            TemplateId = templateId;
            Title = title;
            PrimaryIndustry = primaryIndustry;
            DeputyIndustry = deputyIndustry;
            Content = content;
            Example = example;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Template()
        { }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("模板id：{0}\r\n标题：{1}\r\n一级行业：{2}\r\n二级行业：{3}\r\n" +
                "内容：{4}\r\n示例：{5}",
                TemplateId, Title, PrimaryIndustry, DeputyIndustry,
                Content, Example);
        }

        /// <summary>
        /// 从JObject对象中解析统计数据
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            TemplateId = (string)jo["template_id"];
            Title = (string)jo["title"];
            PrimaryIndustry = (string)jo["primary_industry"];
            DeputyIndustry = (string)jo["deputy_industry"];
            Content = (string)jo["content"];
            Example = (string)jo["example"];
        }

        /// <summary>
        /// 解析模板
        /// </summary>
        /// <param name="json">JSON格式的模板数据</param>
        /// <returns>返回模板数组</returns>
        public static ErrorMessage Parse(string json, out Template[] templates)
        {
            templates = null;
            ErrorMessage errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "");
            if (string.IsNullOrWhiteSpace(json))
            {
                errorMessage.errmsg = "请求失败。";
            }
            else if (ErrorMessage.IsErrorMessage(json))
            {
                errorMessage = ErrorMessage.Parse(json);
            }
            else
            {
                JObject jo = JObject.Parse(json);
                JToken jt;
                if (jo.TryGetValue("template_list", out jt) && jt.Type == JTokenType.Array)
                {
                    errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
                    JArray ja = (JArray)jt;
                    templates = new Template[ja.Count];
                    for (int i = 0; i < ja.Count; i++)
                    {
                        templates[i] = new Template();
                        templates[i].Parse((JObject)ja[i]);
                    }
                }
                else
                {
                    errorMessage.errmsg = "解析结果失败。";
                }
            }
            return errorMessage;
        }
    }
}