using System;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace PublicAccount
{
    /// <summary>
    /// 二维码
    /// </summary>
    public class QrCode
    {
        /// <summary>
        /// 创建二维码的地址
        /// </summary>
        private const string urlForCreating = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
        /// <summary>
        /// 创建二维码的http方法
        /// </summary>
        private const string httpMethodForCreating = WebRequestMethods.Http.Post;
        /// <summary>
        /// 显示二维码的地址
        /// </summary>
        private const string urlForShowing = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";

        /// <summary>
        /// 临时二维码的最大有效时间（单位：秒）。
        /// 注：2015年4月22日，微信将临时二维码的最大有效时间由半小时调整为7天。
        /// </summary>
        private const int maxExpireSeconds = 604800;
        /// <summary>
        /// 临时二维码
        /// </summary>
        private const string QR_SCENE = "QR_SCENE";
        /// <summary>
        /// 整形永久二维码
        /// </summary>
        private const string QR_LIMIT_SCENE = "QR_LIMIT_SCENE";
        /// <summary>
        /// 字符型永久二维码
        /// </summary>
        private const string QR_LIMIT_STR_SCENE = "QR_LIMIT_STR_SCENE";
        /// <summary>
        /// 整形永久二维码的最小id
        /// </summary>
        private const int minSceneId = 1;
        /// <summary>
        /// 整形永久二维码的最大id
        /// </summary>
        private const int maxSceneId = 100000;
        /// <summary>
        /// 字符型永久二维码的最小长度
        /// </summary>
        private const int minSceneStrLength = 1;
        /// <summary>
        /// 字符型永久二维码的最大长度
        /// </summary>
        private const int maxSceneStrLength = 64;

        /// <summary>
        /// 二维码ticket
        /// </summary>
        public string ticket { get; set; }
        /// <summary>
        /// 二维码的有效时间
        /// </summary>
        public int expire_seconds { get; set; }
        /// <summary>
        /// 二维码图片解析后的地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="expire_seconds"></param>
        /// <param name="url"></param>
        public QrCode(string ticket, int expire_seconds, string url)
        {
            this.ticket = ticket;
            this.expire_seconds = expire_seconds;
            this.url = url;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("二维码ticket：{0}\r\n有效时间：{1}秒\r\n图片地址：{2}",
                ticket, expire_seconds, url);
        }

        /// <summary>
        /// 创建临时二维码
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="expireSeconds">有效时间</param>
        /// <param name="sceneId">场景值id</param>
        /// <param name="errorMessage">返回创建是否成功</param>
        /// <returns>返回二维码；如果创建失败，返回null。</returns>
        public static QrCode Create(string userName, int expireSeconds, int sceneId, out ErrorMessage errorMessage)
        {
            if (expireSeconds > maxExpireSeconds)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, string.Format("有效时间不能大于{0}秒。", maxExpireSeconds));
                return null;
            }
            if (sceneId == 0)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "临时二维码的场景值id不能为0。");
                return null;
            }
            string json = JsonConvert.SerializeObject(new { expire_seconds = expireSeconds, action_name = QR_SCENE, action_info = new { scene = new { scene_id = sceneId } } });
            return CreateQrCode(userName, json, out errorMessage);
        }

        /// <summary>
        /// 创建整形永久二维码
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="sceneId">场景值id</param>
        /// <param name="errorMessage">返回创建是否成功</param>
        /// <returns>返回二维码；如果创建失败，返回null。</returns>
        public static QrCode Create(string userName, int sceneId, out ErrorMessage errorMessage)
        {
            if (sceneId < minSceneId || sceneId > maxSceneId)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode,
                    string.Format("整形永久二维码的场景值id必须介于{0}到{1}之间。", minSceneId, maxSceneId));
                return null;
            }
            string json = JsonConvert.SerializeObject(new { action_name = QR_LIMIT_SCENE, action_info = new { scene = new { scene_id = sceneId } } });
            return CreateQrCode(userName, json, out errorMessage);
        }

        /// <summary>
        /// 创建字符型永久二维码
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="sceneId">场景值id</param>
        /// <param name="errorMessage">返回创建是否成功</param>
        /// <returns>返回二维码；如果创建失败，返回null。</returns>
        public static QrCode Create(string userName, string sceneId, out ErrorMessage errorMessage)
        {
            if (string.IsNullOrWhiteSpace(sceneId) || sceneId.Length < minSceneId || sceneId.Length > maxSceneId)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode,
                    string.Format("字符型永久二维码的场景值id长度必须介于{0}到{1}之间。", minSceneStrLength, maxSceneStrLength));
                return null;
            }
            string json = JsonConvert.SerializeObject(new { action_name = QR_LIMIT_STR_SCENE, action_info = new { scene = new { scene_str = sceneId } } });
            return CreateQrCode(userName, json, out errorMessage);
        }

        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="json">json数据</param>
        /// <param name="errorMessage">返回创建是否成功</param>
        /// <returns>返回二维码；如果创建失败，返回null。</returns>
        private static QrCode CreateQrCode(string userName, string json, out ErrorMessage errorMessage)
        {
            QrCode code = null;
            string responseContent = HttpHelper.RequestResponseContent(urlForCreating, userName, null, httpMethodForCreating, json);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                var result = JsonConvert.DeserializeAnonymousType(responseContent, new { ticket = "", expire_seconds = 0, url = "" });
                code = new QrCode(result.ticket, result.expire_seconds, result.url);
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "创建二维码成功。");
            }
            return code;
        }

        /// <summary>
        /// 获取显示二维码的url地址
        /// </summary>
        /// <param name="ticket">二维码ticket</param>
        /// <returns>返回显示二维码的地址</returns>
        public static string GetUrl(string ticket)
        {
            return string.Format(urlForShowing, HttpUtility.UrlEncode(ticket));
        }
    }
}