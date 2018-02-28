using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 火车结果
    /// </summary>
    public class TrainResult : BaseResult
    {
        /// <summary>
        /// 车次列表
        /// </summary>
        public List<Train> Trains { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="trains"></param>
        internal TrainResult(string text, IEnumerable<Train> trains)
            : base(CodeEnum.Train, text)
        {
            if (trains == null)
                throw new ArgumentNullException("trains", "火车列表不能为空。");
            Trains = new List<Train>(trains);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendFormat("车次数：{0}", Trains.Count);
            if (Trains.Count > 0)
            {
                foreach (Train train in Trains)
                    sb.AppendFormat("\r\n{0}", train);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解析火车结果
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        internal static TrainResult Parse(JObject jo)
        {
            string text = (string)jo["text"];
            JArray ja = (JArray)jo["list"];
            Train[] trains = new Train[ja.Count];
            int idx = 0;
            foreach (JObject item in ja)
            {
                trains[idx] = new Train(item);
                idx++;
            }
            return new TrainResult(text, trains);
        }
    }
}
