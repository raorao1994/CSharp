using System;
using Newtonsoft.Json;

namespace PublicAccount
{
    /// <summary>
    /// MultiMediaUploadResult：多媒体文件上传结果
    /// </summary>
    public class MultiMediaUploadResult
    {
        /// <summary>
        /// 多媒体类型
        /// </summary>
        public MultiMediaTypeEnum Type { get; private set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string MediaId { get; private set; }
        /// <summary>
        /// 创建时间（上传多媒体文件的时间）
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">多媒体类型</param>
        /// <param name="mediaId">媒体ID</param>
        /// <param name="createAt">创建时间</param>
        public MultiMediaUploadResult(MultiMediaTypeEnum type,string mediaId,DateTime createAt)
        {
            Type = type;
            MediaId = mediaId;
            CreatedAt = createAt;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("多媒体类型：{0:g}\r\n媒体ID：{1}\r\n创建时间：{2}",
                Type, MediaId, CreatedAt);
        }

        /// <summary>
        /// 从JSON字符串解析多媒体上传结果
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>返回多媒体上传结果</returns>
        public static MultiMediaUploadResult Parse(string json)
        {
            var result = JsonConvert.DeserializeAnonymousType(json, new { type = "TYPE", media_id = "MEDIA_ID", created_at = 123456789 });
            MultiMediaTypeEnum type = (MultiMediaTypeEnum)Enum.Parse(typeof(MultiMediaTypeEnum), result.type, true);
            string mediaId = result.media_id;
            DateTime createdAt = Utility.ToDateTime(result.created_at);
            return new MultiMediaUploadResult(type, mediaId, createdAt);
        }

        /// <summary>
        /// 尝试从JSON字符串解析多媒体上传结果
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <param name="msg">如果解析成功，返回多媒体上传结果；否则，返回null。</param>
        /// <returns>返回是否解析成功</returns>
        public static bool TryParse(string json, out MultiMediaUploadResult result)
        {
            bool success = false;
            result = null;
            try
            {
                result = Parse(json);
                success = true;
            }
            catch { }
            return success;
        }
    }
}