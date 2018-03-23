using App.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service
{
    /// <summary>
    /// Redis服务
    /// </summary>
    public class RedisServer: ServiceStack.Service
    {
        public object Get(RedisRequestModel request)
        {
            int id= request.ID;
            var client = new RedisClient("127.0.0.1", 6379);
            RedisResponseModel model=client.Get<RedisResponseModel>(id.ToString());
            return model;
        }
    }
}
