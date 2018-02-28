using System;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 响应基类
    /// </summary>
    public class BaseResult
    {
        /// <summary>
        /// 代码
        /// </summary>
        public CodeEnum Code { get; private set; }
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// 结果是否为错误
        /// </summary>
        public bool IsError
        {
            get
            {
                return Code >= CodeEnum.KeyLengthError && Code <= CodeEnum.ServerDataError;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="text"></param>
        internal BaseResult(CodeEnum code, string text)
        {
            Code = code;
            Text = text;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="text"></param>
        internal BaseResult(int code, string text)
            : this((CodeEnum)code, text)
        { }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("代码：{0}\r\n类型：{1:g}\r\n文本：{2}",
                (int)Code, Code, Text);
        }

        /// <summary>
        /// 解析结果
        /// </summary>
        /// <param name="json">图灵服务器返回的json字符串</param>
        /// <returns>返回结果</returns>
        public static BaseResult Parse(string json)
        {
            BaseResult result = null;
            JObject jo = JObject.Parse(json);
            JToken jt;
            if (!jo.TryGetValue("code", out jt) || !jo.TryGetValue("text", out jt))
                throw new ArgumentException("解析失败。", "json");
            CodeEnum code = (CodeEnum)(int)jo["code"];
            string text = (string)jo["text"];
            switch (code)
            {
                case CodeEnum.Text:
                case CodeEnum.KeyLengthError:
                case CodeEnum.InfoError:
                case CodeEnum.KeyOrAccountError:
                case CodeEnum.RequestUseUp:
                case CodeEnum.Unsupport:
                case CodeEnum.ServerUpgrading:
                case CodeEnum.ServerDataError:
                    result = new BaseResult(code, text);
                    break;
                case CodeEnum.Url:
                    result = UrlResult.Parse(jo);
                    break;
                case CodeEnum.News:
                    result = ArticleResult.Parse(jo);
                    break;
                case CodeEnum.AppSoftDownload:
                    result = AppSoftDownloadResult.Parse(jo);
                    break;
                case CodeEnum.Train:
                    result = TrainResult.Parse(jo);
                    break;
                case CodeEnum.Flight:
                    result = FlightResult.Parse(jo);
                    break;
                case CodeEnum.CookVideoNovel:
                    result = CookVideoNovelResult.Parse(jo);
                    break;
                case CodeEnum.Hotel:
                    result = HotelResult.Parse(jo);
                    break;
                case CodeEnum.Price:
                    result = PriceResult.Parse(jo);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 尝试解析结果
        /// </summary>
        /// <param name="json">图灵服务器返回的json字符串</param>
        /// <param name="result">如果解析成功，返回结果；否则返回null。</param>
        /// <returns>返回解析是否成功</returns>
        public static bool TryParse(string json, out BaseResult result)
        {
            bool success = true;
            try
            {
                result = Parse(json);
            }
            catch
            {
                result = null;
                success = false;
            }
            return success;
        }
    }
}
