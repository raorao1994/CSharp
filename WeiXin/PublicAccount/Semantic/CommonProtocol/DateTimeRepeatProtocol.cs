using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 重复时间协议
    /// </summary>
    public class DateTimeRepeatProtocol : CommonProtocol
    {
        /// <summary>
        /// 时间，24 小时制，格式：HH:MM:SS
        /// </summary>
        public string time { get; private set; }
        /// <summary>
        /// 时间的原始字符串
        /// </summary>
        public string time_ori { get; private set; }
        /// <summary>
        /// 重复标记：0000000 注：依次代表周日，周六，…，周一；1 表示该天要重复，0 表示不重复；例如：0010101代表每周一三五
        /// </summary>
        public string repeat { get; private set; }
        /// <summary>
        /// 重复的原始字符串
        /// </summary>
        public string repeat_ori { get; private set; }

        /// <summary>
        /// 获取时，24小时制
        /// </summary>
        public int Hour
        {
            get
            {
                return GetDateTime().Hour;
            }
        }
        /// <summary>
        /// 获取分
        /// </summary>
        public int Minute
        {
            get
            {
                return GetDateTime().Minute;
            }
        }
        /// <summary>
        /// 获取秒
        /// </summary>
        public int Second
        {
            get
            {
                return GetDateTime().Second;
            }
        }
        /// <summary>
        /// 获取日期时间
        /// </summary>
        /// <returns></returns>
        private DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format("1900-1-1 {0}", time));
        }

        /// <summary>
        /// 获取星期日是否重复
        /// </summary>
        public bool Sunday
        {
            get
            {
                return IsRepeat(Weekday.星期天);
            }
        }
        public bool 星期天
        {
            get
            {
                return Sunday;
            }
        }
        /// <summary>
        /// 获取星期六是否重复
        /// </summary>
        public bool Saturday
        {
            get
            {
                return IsRepeat(Weekday.星期六);
            }
        }
        public bool 星期六
        {
            get
            {
                return Saturday;
            }
        }
        /// <summary>
        /// 获取星期五是否重复
        /// </summary>
        public bool Friday
        {
            get
            {
                return IsRepeat(Weekday.星期五);
            }
        }
        public bool 星期五
        {
            get
            {
                return Friday;
            }
        }
        /// <summary>
        /// 获取星期四是否重复
        /// </summary>
        public bool Thursday
        {
            get
            {
                return IsRepeat(Weekday.星期四);
            }
        }
        public bool 星期四
        {
            get
            {
                return Thursday;
            }
        }
        /// <summary>
        /// 获取星期三是否重复
        /// </summary>
        public bool Wednesday
        {
            get
            {
                return IsRepeat(Weekday.星期三);
            }
        }
        public bool 星期三
        {
            get
            {
                return Wednesday;
            }
        }
        /// <summary>
        /// 获取星期二是否重复
        /// </summary>
        public bool Tuesday
        {
            get
            {
                return IsRepeat(Weekday.星期二);
            }
        }
        public bool 星期二
        {
            get
            {
                return Tuesday;
            }
        }
        /// <summary>
        /// 获取星期一是否重复
        /// </summary>
        public bool Monday
        {
            get
            {
                return IsRepeat(Weekday.星期一);
            }
        }
        public bool 星期一
        {
            get
            {
                return Monday;
            }
        }
        /// <summary>
        /// 判断某日是否重复
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        private bool IsRepeat(Weekday weekday)
        {
            return repeat[(int)weekday] == '1';
        }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            time = (string)jo["time"];
            time_ori = (string)jo["time_ori"];
            repeat = (string)jo["repeat"];
            repeat_ori = (string)jo["repeat_ori"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.Append("重复：");
            foreach (int weekday in Enum.GetValues(typeof(Weekday)))
            {
                Weekday day = (Weekday)weekday;
                if (IsRepeat(day))
                    sb.AppendFormat("{0:g},", day);
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// 星期
    /// </summary>
    internal enum Weekday
    {
        星期天 = 0,
        星期六 = 1,
        星期五 = 2,
        星期四 = 3,
        星期三 = 4,
        星期二 = 5,
        星期一 = 6
    }
}