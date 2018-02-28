using System;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 航班
    /// </summary>
    public class Flight
    {
        /// <summary>
        /// 航班
        /// </summary>
        public string Number { get; private set; }
        /// <summary>
        /// 航班路线
        /// </summary>
        public string Route { get; private set; }
        /// <summary>
        /// 起飞时间
        /// </summary>
        public string StartTime { get; private set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        public string EndTime { get; private set; }
        /// <summary>
        /// 航班状态
        /// </summary>
        public string State { get; private set; }
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
        /// <param name="number">航班</param>
        /// <param name="route">路线</param>
        /// <param name="startTime">开点</param>
        /// <param name="endTime">到点</param>
        /// <param name="state">状态</param>
        /// <param name="detailUrl">详情地址</param>
        /// <param name="icon">图标地址</param>
        internal Flight(string number, string route, string startTime, string endTime,string state, string detailUrl, string icon)
        {
            Number = number;
            Route = route;
            StartTime = startTime;
            EndTime = endTime;
            State = state;
            DetailUrl = detailUrl;
            Icon = icon;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jo">jobject对象</param>
        internal Flight(JObject jo)
        {
            JToken jt;
            Number = (string)jo["flight"];
            Route = jo.TryGetValue("route", out jt) ? (string)jt : string.Empty;
            StartTime = (string)jo["starttime"];
            EndTime = (string)jo["endtime"];
            State = jo.TryGetValue("state", out jt) ? (string)jt : string.Empty;
            DetailUrl = jo.TryGetValue("detailurl", out jt) ? (string)jt : string.Empty;
            Icon = (string)jo["icon"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("航班：{0}\r\n路线：{1}\r\n起飞时间：{2}\r\n" +
                "到达时间：{3}\r\n状态：{4}\r\n详情地址：{5}\r\n图标地址：{6}",
                Number, Route, StartTime,
                EndTime, State, DetailUrl, Icon);
        }
    }
}
