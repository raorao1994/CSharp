using System;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 火车
    /// </summary>
    public class Train
    {
        /// <summary>
        /// 车次
        /// </summary>
        public string Number { get; private set; }
        /// <summary>
        /// 起点
        /// </summary>
        public string Start { get; private set; }
        /// <summary>
        /// 终点
        /// </summary>
        public string Terminal { get; private set; }
        /// <summary>
        /// 开点
        /// </summary>
        public string StartTime { get; private set; }
        /// <summary>
        /// 到点
        /// </summary>
        public string EndTime { get; private set; }
        /// <summary>
        /// 详情地址
        /// </summary>
        public string DetailUrl { get; private set; }
        /// <summary>
        /// 图标地址
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="number">车次</param>
        /// <param name="start">起点</param>
        /// <param name="terminal">终点</param>
        /// <param name="startTime">开点</param>
        /// <param name="endTime">到点</param>
        /// <param name="detailUrl">详情地址</param>
        /// <param name="icon">图标地址</param>
        internal Train(string number, string start, string terminal, string startTime, string endTime, string detailUrl, string icon)
        {
            Number = number;
            Start = start;
            Terminal = terminal;
            StartTime = startTime;
            EndTime = endTime;
            DetailUrl = detailUrl;
            Icon = icon;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jo">jobject对象</param>
        internal Train(JObject jo)
            : this((string)jo["trainnum"], (string)jo["start"], (string)jo["terminal"],
            (string)jo["starttime"], (string)jo["endtime"], (string)jo["detailurl"], (string)jo["icon"])
        { }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("车次：{0}\r\n起点：{1}\r\n终点：{2}\r\n开点：{3}\r\n" +
                "到点：{4}\r\n详情地址：{5}\r\n图标地址：{6}",
                Number, Start, Terminal, StartTime,
                EndTime, DetailUrl, Icon);
        }
    }
}
