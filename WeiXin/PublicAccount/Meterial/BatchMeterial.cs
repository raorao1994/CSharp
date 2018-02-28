using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 批量素材
    /// </summary>
    public class BatchMeterial:IParsable
    {
        /// <summary>
        /// 素材总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 本次调用获取的素材数量
        /// </summary>
        public int ItemCount { get; set; }
        /// <summary>
        /// 素材列表
        /// </summary>
        public MeterialItem[] Item { get; set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            
            TotalCount = (int)jo["total_count"];
            ItemCount = (int)jo["item_count"];
            JToken jt;
            if (jo.TryGetValue("item", out jt) && jt.Type == JTokenType.Array && ((JArray)jt).Count > 0)
            {
                JArray ja=(JArray)jt;
                Item = new MeterialItem[ja.Count];
                for (int i = 0; i < ja.Count; i++)
                    Item[i] = MeterialItem.ParseItem((JObject)ja[i]);
            }
            else
                Item = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("素材总数：{0}\r\n", TotalCount);
            sb.AppendFormat("本次获取的素材数量：{0}", ItemCount);
            if(Item!=null&&Item.Length>0)
            {
                for (int i = 0; i < Item.Length; i++)
                    sb.AppendFormat("\r\n素材{0}：\r\n{1}", i, Item[i]);
            }
            return sb.ToString();
        }
    }
}
