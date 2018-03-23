using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    /// <summary>
    /// 返回数据模型
    /// </summary>
    public class ResponseModel
    {
        public string state { get; set; }
        public string msg { get; set; }
    }

    /// <summary>
    /// 返回数据模型
    /// </summary>
    public class RedisResponseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
