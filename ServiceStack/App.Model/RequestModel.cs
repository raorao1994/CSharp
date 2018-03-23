using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    /// <summary>
    /// 请求模型
    /// </summary>
    [Route("/RequestModel")]
    [Route("/RequestModel/{ID}")]
    [Route("/RequestModel/{ID}/{Name}")]
    public class RequestModel
    {
        public int ID { get; set;}
        public string Name { get; set; }
    }
    /// <summary>
    /// 请求模型
    /// </summary>
    [Route("/WebRequestModel")]
    [Route("/WebRequestModel/{ID}")]
    [Route("/WebRequestModel/{ID}/{Name}")]
    public class WebRequestModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// 请求模型
    /// </summary>
    [Route("/OtherRequestModel")]
    [Route("/OtherRequestModel/{ID}")]
    [Route("/OtherRequestModel/{ID}/{Name}")]
    public class OtherRequestModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// 请求模型
    /// </summary>
    [Route("/RedisRequestModel")]
    [Route("/RedisRequestModel/{ID}")]
    public class RedisRequestModel
    {
        public int ID { get; set; }
    }
}
